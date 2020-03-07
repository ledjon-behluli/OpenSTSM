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
    public class ImageAnalysis
    {
        private string imagePath;
        private string pythonPath;        
        private Predict predict;
        private PythonExecutor python;
        private PipeClient client;
        private ThreadedInfoBox TinfoBox;


        public List<Prediction> Predictions { get; private set; }

        public ImageAnalysis(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                throw new ArgumentException("Invalid image path!");

            this.imagePath = imagePath;
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
        }

        ~ImageAnalysis()
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
                python.RunScript("main.py");
                client.Connect("openstsm");

                TinfoBox.Start("Loading neural network model", "Image Analysis");
                return predict.LoadModel(Settings.Default.NN_ModelPath);
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
                TinfoBox.DisplayTextChanged?.Invoke("Running selective search algorithm");                                    
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
                    TinfoBox.DisplayTextChanged?.Invoke("Running object prediction on image");
                    string results = predict.RunPrediction(Settings.Default.MiddlePointDistanceThreshold, Settings.Default.OuterSelectionThreshold, Settings.Default.DecimalPointProbabilityRounding,
                                                           Settings.Default.RegionProposalsMultiplicity, Settings.Default.SpatialDistanceOfCoordinatePointsThreshold, Settings.Default.NumberOfResultsPerElement,
                                                           Settings.Default.UseGpuAcceleration);
                    TinfoBox.Close();
                    client.Close();
                    python.Close();

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

        private bool HandleException(Exception e)
        {
            TinfoBox.Close();
            if (client.isConnected())
                client.Close();

            python.Close();

            System.Windows.Forms.MessageBox.Show(e.Message);
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
