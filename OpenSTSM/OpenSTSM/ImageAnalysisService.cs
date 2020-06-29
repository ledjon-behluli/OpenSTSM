using IpcPythonCS.Engine.CSharp;
using IpcPythonCS.Engine.CSharp.Communication.Pipe;
using IpcPythonCS.Engine.ML;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenSTSM.Extensions;
using OpenSTSM.ViewModels.MainWindow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;

namespace OpenSTSM
{
    public class ImageAnalysisService
    {
        private bool isFirstLoad;
        private bool isCanceled;

        private const string title = "Image Analysis";
        private const string initText = "Initiating server connection";
        private const string loadModelText = "Loading neural network model";
        private const string imageCorrectionsText = "Applying image corrections";
        private const string runSSText = "Running selective search algorithm";
        private const string runPredictText = "Running object prediction on image";

        private string pythonPath;        
        private Predict predict;
        private PythonExecutor python;
        private PipeClient client;
        private ThreadedInfoBox TinfoBox;               


        public List<Prediction> Predictions { get; private set; }

        public ImageAnalysisService()
        {
            pythonPath = GetPythonPath();
            TinfoBox = new ThreadedInfoBox();
            TinfoBox.Canceled += () =>
            {
                Close();
                isCanceled = true;
            };
            Init();
        }

        ~ImageAnalysisService()
        {            
            TinfoBox.Close();
            Close();
        }        

        private void Python_OnPythonError(string output)
        {
            HandleException(new Exception(output));
        }

        private void Init() 
        {
            isFirstLoad = true;
            Predictions = null;            
            python = !string.IsNullOrEmpty(pythonPath) ? new PythonExecutor(pythonPath) : new PythonExecutor();
            python.AddStandartOutputErrorFilters("Using TensorFlow backend.");
            python.AddStandartOutputErrorFilters("CUDA_ERROR_NO_DEVICE: no CUDA-capable device is detected");
            client = new PipeClient();
            predict = new Predict(client);            
            python.OnPythonError += Python_OnPythonError;
            TinfoBox.Start(initText, title);
            python.RunScript("main.py");
            client.Connect("openstsm");
        }

        public bool LoadModel()
        {
            try
            {
                bool retValue = false;
                if (isCanceled)
                {
                    Init();
                    isCanceled = false;
                }

                if (isFirstLoad)
                {
                    TinfoBox.DisplayTextChanged?.Invoke(loadModelText);
                    retValue = predict.LoadModel(Settings.Default.NN_ModelPath);
                }
                else
                {
                    retValue = true;
                }

                if (!retValue)
                    TinfoBox.DisplayTextChanged?.Invoke("Failed to load model!");

                return retValue;
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public bool ImageDimessionCorrections(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                    throw new ArgumentException("Invalid image path!");

                bool retValue = false;

                if (isFirstLoad)
                    TinfoBox.DisplayTextChanged?.Invoke(imageCorrectionsText);
                else
                    TinfoBox.Start(imageCorrectionsText, title);

                retValue = predict.ImageDimessionCorrections(imagePath);

                if (!retValue)
                    TinfoBox.DisplayTextChanged?.Invoke("Failed to apply image corrections!");

                return retValue;
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public bool RunSelectiveSearch()
        {
            try
            {
                bool retValue = false;

                TinfoBox.DisplayTextChanged?.Invoke(runSSText);

                retValue = predict.RunSelectiveSearch(Settings.Default.NumberOfRegionProposals, Settings.Default.ImageResizeFactor);

                if (!retValue)
                    TinfoBox.DisplayTextChanged?.Invoke("Failed to run selective search successfully!");

                return retValue;
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public bool RunPrediction()
        {
            try
            {
                bool retValue = false;

                if (File.Exists(Settings.Default.NN_ModelPath))
                {
                    TinfoBox.DisplayTextChanged?.Invoke(runPredictText);
                    string results = predict.RunPrediction(Settings.Default.MiddlePointDistanceThreshold, Settings.Default.OuterSelectionThreshold, Settings.Default.DecimalPointProbabilityRounding,
                                                           Settings.Default.RegionProposalsMultiplicity, Settings.Default.SpatialDistanceOfCoordinatePointsThreshold, Settings.Default.NumberOfResultsPerElement,
                                                           Settings.Default.UseGpuAcceleration);
                    TinfoBox.Close();

                    isFirstLoad = false;
                    if (!string.IsNullOrEmpty(results))
                    {
                        Predictions = JsonConvert.DeserializeObject<List<Prediction>>(results);
                        retValue = true;
                    }
                }

                if (!retValue)
                    TinfoBox.DisplayTextChanged?.Invoke("Failed to run object prediction successfully!");

                return retValue;
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public void Close()
        {
            if (client != null)
                if (client.isConnected())
                    client.Close();

            if (python != null)
            {
                python.OnPythonError -= Python_OnPythonError;
                python.Close();
            }
        }

        private bool HandleException(Exception e)
        {
            TinfoBox?.Close();            

            string errorMessage = e.Message;
            if (e.Message == "Root element is missing.")   // This is a know error. This happens if there is still a python instance running.
                errorMessage = "An existing python instance is running.\nClose that instance and run image analysis again.";

            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
            return false;
        }

        private string GetPythonPath()
        {
            var environmentVariables = Environment.GetEnvironmentVariables();
            string pathVariable = environmentVariables["Path"] as string;

            if (pathVariable != null)
            {
                string[] allPaths = pathVariable.Split(';');
                foreach (var path in allPaths)
                {
                    string pythonexe;
                    if (path.EndsWith("\\"))
                        pythonexe = "python.exe";
                    else
                        pythonexe = "\\python.exe";

                    string pythonPathFromEnv = path + pythonexe;
                    if (File.Exists(pythonPathFromEnv))
                        return pythonPathFromEnv;
                }
            }

            return string.Empty;
        }
    }


    public class Prediction
    {
        public int Id { get; set; }

        public string Class { get; set; }

        public decimal Probability { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        [JsonIgnore]
        private static volatile int _uniqueId;

        [JsonIgnore]
        public int UniqueId { get; private set; }

        [JsonIgnore]
        public ControlElementType ControlElementType => this.Class.Contains("arrow") ? ControlElementType.Connector : ControlElementType.Node;

        [JsonIgnore]
        public string Name => ControlElementType == ControlElementType.Connector ? "Arrow" : this.Class.FirstCharToUpper();                
        
        [JsonIgnore]
        public Point Location => new Point(X, Y);

        public Prediction()
        {
            UniqueId = Interlocked.Increment(ref _uniqueId);
        }
    }
}
