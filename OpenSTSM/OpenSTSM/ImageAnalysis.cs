using IpcPythonCS.Engine.CSharp;
using IpcPythonCS.Engine.CSharp.Communication.Pipe;
using IpcPythonCS.Engine.ML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSTSM
{
    public class ImageAnalysis
    {
        private string imagePath;
        private string pythonPath;        
        private Predict predict;
        private PythonExecutor python;
        private PipeClient client;

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
        }

        public bool RunSelectiveSearch()
        {
            try
            {
                python.RunScript("main.py");
                client.Connect("openstsm");

                ThreadedInfoBox TinfoBox = new ThreadedInfoBox();
                TinfoBox.Canceled += () => {
                    client.Close();
                    python.Close();
                };
                TinfoBox.StartNewThreadInfoBox("Running selective search algorithm...", "Image Analysis");                             

                bool retValue = predict.RunSelectiveSearch(imagePath, 80);

                TinfoBox.EndNewThreadInfoBox();
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

                ThreadedInfoBox TinfoBox = new ThreadedInfoBox();
                TinfoBox.Canceled += () => {
                    client.Close();
                    python.Close();
                };
                TinfoBox.StartNewThreadInfoBox("Running object prediction on image...", "Image Analysis");

                string results = predict.RunPrediction(@"E:\\Storage\\Python\\OpenSTSM\\ML\\models\\model.model", 5, 3, 5, 1, 8, 2, true);
                if (!string.IsNullOrEmpty(results))
                {
                    PredictionObject predObj = JsonConvert.DeserializeObject<PredictionObject>(results);
                    retValue = true;
                }

                TinfoBox.EndNewThreadInfoBox();
                
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
