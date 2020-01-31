using IpcPythonCS.Engine.ML;
using IpcPythonCS.Engine.CSharp.Communication.Pipe;
using IpcPythonCS.Engine.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IpcPythonCS.Engine.CSharp.Example;
using System.Threading;

namespace OpenSTSM.Test
{
    [TestClass]
    public class PySelectiveSearchTest
    {
        [TestMethod]
        public void Run()
        {
            SelectiveSearch ss;
            PythonExecutor python = new PythonExecutor(@"C:\Users\kutiatore\AppData\Local\Programs\Python\Python35\python.exe");
            PipeClient client = new PipeClient();

            try
            {
                python.RunScript("main.py");
                client = new PipeClient();
              
                client.Connect("openstsm");
                ss = new SelectiveSearch(client);
                //ss.Run("E:\\Libraries\\Desktop\\Visa Docs\\test_selective_1.png", 80);
                var b = ss.MachineComp(8);

                client.Close();
                python.Close();
            }
            catch (System.Exception)
            {
                client.Close();
                python.Close();
            }
        }
    }
}
