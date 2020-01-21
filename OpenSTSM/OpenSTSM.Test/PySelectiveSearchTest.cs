using IpcPythonCS.Engine.ML;
using IpcPythonCS.Engine.CSharp.Communication.Pipe;
using IpcPythonCS.Engine.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IpcPythonCS.Engine.CSharp.Example;

namespace OpenSTSM.Test
{
    [TestClass]
    public class PySelectiveSearchTest
    {
        [TestMethod]
        public void Run()
        {
            SelectiveSearch ss;
            PythonExecutor python;
            PipeClient client;

            python = new PythonExecutor(@"C:\Users\kutiatore\AppData\Local\Programs\Python\Python35\python.exe");
            python.RunScript("main.py");

            client = new PipeClient();
            client.Connect("openSTSM");

            ss = new SelectiveSearch(client);

            //ss.Run("E:\\Libraries\\Desktop\\Visa Docs\\test_selective_1.png", 80);
            ss.Test(6);

            client.Close();
            python.Close();
        }

        [TestMethod]
        public void MyTestMethod()
        {
            PyCalculator calculator;
            PythonExecutor python;
            PipeClient client;

            python = new PythonExecutor(@"C:\Users\kutiatore\AppData\Local\Programs\Python\Python35\python.exe");
            python.RunScript("main.py");

            client = new PipeClient();
            client.Connect("openSTSM");

            calculator = new PyCalculator(client);

            var a = calculator.Addition(1, 2);

            client.Close();
            python.Close();
        }

    }
}
