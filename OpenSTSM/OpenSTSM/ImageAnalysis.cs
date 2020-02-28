using IpcPythonCS.Engine.CSharp;
using IpcPythonCS.Engine.CSharp.Communication.Pipe;
using IpcPythonCS.Engine.ML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenSTSM
{
    public class ImageAnalysis
    {
        private string imagePath;
        private string pythonPath;        
        private Predict predict;
        private PythonExecutor python;
        private PipeClient client;
        ThreadedInfoBox TinfoBox;

        public ImageAnalysis(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                throw new ArgumentException("Invalid image path!");

            this.imagePath = imagePath;
            pythonPath = GetPythonPath();
            python = !string.IsNullOrEmpty(pythonPath) ? new PythonExecutor(pythonPath) : new PythonExecutor();
            python.AddStandartOutputErrorFilters("Using TensorFlow backend.");
            python.AddStandartOutputErrorFilters("CUDA_ERROR_NO_DEVICE: no CUDA-capable device is detected");
            client = new PipeClient();
            predict = new Predict(client);
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

        public bool RunSelectiveSearch()
        {
            try
            {
                python.RunScript("main.py");
                client.Connect("openstsm");

                TinfoBox = new ThreadedInfoBox();
                TinfoBox.Canceled += () => {
                    client.Close();
                    python.Close();
                };
                TinfoBox.Start("Running selective search algorithm...", "Image Analysis");                             

                bool retValue = predict.RunSelectiveSearch(imagePath, 80);

                TinfoBox.Close();
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

                TinfoBox = new ThreadedInfoBox();
                TinfoBox.Canceled += () => {
                    client.Close();
                    python.Close();
                };
                TinfoBox.Start("Running object prediction on image...", "Image Analysis");

                string results = predict.RunPrediction(Settings.Default.NN_ModelPath, 5, 3, 5, 1, 8, 2, true);
                if (!string.IsNullOrEmpty(results))
                {
                    List<PredictionObject> predObjs = JsonConvert.DeserializeObject<List<PredictionObject>>(results);
                    retValue = true;
                }

                TinfoBox.Close();                
                client.Close();
                python.Close();
                                
                return retValue;
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


    public class PredictionObject
    {
        public string Class { get; set; }
        public double Probability { get; set; }
        public int Id { get; set; }
    }
}
