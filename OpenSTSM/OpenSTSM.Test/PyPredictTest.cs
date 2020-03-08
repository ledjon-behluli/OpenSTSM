using IpcPythonCS.Engine.ML;
using IpcPythonCS.Engine.CSharp.Communication.Pipe;
using IpcPythonCS.Engine.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenSTSM.Test
{
    [TestClass]
    public class PyPredictTest
    {        
        [TestMethod]
        public void RunSelectiveSearch()
        {
            Predict predict;
            PythonExecutor python = new PythonExecutor(@"C:\Users\kutiatore\AppData\Local\Programs\Python\Python35\python.exe");
            python.AddStandartOutputErrorFilters("Using TensorFlow backend.");
            python.AddStandartOutputErrorFilters("CUDA_ERROR_NO_DEVICE: no CUDA-capable device is detected");
            PipeClient client = new PipeClient();
            predict = new Predict(client);

            try
            {
                python.RunScript("main.py");

                client.Connect("openstsm");
                predict = new Predict(client);
                bool run = predict.LoadModel(@"E:\\Storage\\Python\\OpenSTSM\\ML\\models\\model.model");
                run = predict.RunSelectiveSearch("E:\\Libraries\\Desktop\\Visa Docs\\test_selective_1.png", 80);
                string results = predict.RunPrediction(5, 3, 5, 1, 8, 2, true);

                client.Close();
                python.Close();
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine("---------------------");
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                System.Diagnostics.Debug.WriteLine("---------------------");
                if (client.isConnected())
                    client.Close();

                python.Close();
            }
        }
    }
}
