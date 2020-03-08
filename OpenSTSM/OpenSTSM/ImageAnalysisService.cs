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

        private const string title = "Image Analysis";
        private const string initText = "Initiating server connection";
        private const string loadModelText = "Loading neural network model";
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
            isFirstLoad = true;         
            Predictions = null;
            pythonPath = GetPythonPath();
            python = !string.IsNullOrEmpty(pythonPath) ? new PythonExecutor(pythonPath) : new PythonExecutor();
            python.AddStandartOutputErrorFilters("Using TensorFlow backend.");
            python.AddStandartOutputErrorFilters("CUDA_ERROR_NO_DEVICE: no CUDA-capable device is detected");
            client = new PipeClient();
            predict = new Predict(client);
            TinfoBox = new ThreadedInfoBox();
            TinfoBox.Canceled += () => {
                client.Close();
                python.Close();
            };
            python.OnPythonError += Python_OnPythonError;
            TinfoBox.Start(initText, title);
            python.RunScript("main.py");
            client.Connect("openstsm");
        }

        ~ImageAnalysisService()
        {            
            TinfoBox.Close();
            if (client.isConnected())
                client.Close();

            python.OnPythonError -= Python_OnPythonError;
            python.Close();
        }

        private void Python_OnPythonError(string output)
        {
            HandleException(new Exception(output));
        }

        public bool LoadModel()
        {
            try
            {
                if (isFirstLoad)
                {
                    TinfoBox.DisplayTextChanged?.Invoke(loadModelText);
                    return predict.LoadModel(Settings.Default.NN_ModelPath);
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public bool RunSelectiveSearch(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                    throw new ArgumentException("Invalid image path!");

                if (isFirstLoad)
                    TinfoBox.DisplayTextChanged?.Invoke(runSSText);
                else
                    TinfoBox.Start(runSSText, title);

                return predict.RunSelectiveSearch(imagePath, Settings.Default.NumberOfRegionProposals);
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
                        return true;
                    }
                }
                
                return false;
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
                python.Close();
        }

        private bool HandleException(Exception e)
        {
            TinfoBox?.Close();
            if (client.isConnected())
                client.Close();

            python.Close();

            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    string pythonPathFromEnv = path + "\\python.exe";
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
        public string Name => this.Class.FirstCharToUpper();

        [JsonIgnore]
        public Point Location => new Point(X, Y);

        public Prediction()
        {
            UniqueId = Interlocked.Increment(ref _uniqueId);
        }
    }
}
