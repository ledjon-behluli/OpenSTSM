using System;
using System.Diagnostics;
using System.IO;


namespace OpenSTSM.ImageRecognition
{
    public class PythonCaller
    {
        private string _ProgramPath;
        private string _FilePath;
        private object[] _args;
        private ProcessStartInfo start;

        public PythonCaller(string ProgramPath, string FilePath, params Object[] args)
        {
            _ProgramPath = ProgramPath;
            _FilePath = FilePath;
            _args = args;
        }

        public void InitProgramInstance()
        {
            start = new ProcessStartInfo();
            start.FileName = _ProgramPath;
            start.Arguments = PrepareArgs(_args);
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.WindowStyle = ProcessWindowStyle.Hidden;
        }

        public Results GetResults()
        {
            string stderr = "", result = "";
            try
            {
                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        stderr = process.StandardError.ReadToEnd(); // Exceptions from Python script
                        if (!String.IsNullOrEmpty(stderr))
                            return new Results() { HasErrors = true, Result = stderr };

                        result = reader.ReadToEnd(); // Result of StdOut    
                        return new Results() { HasErrors = false, Result = result };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Results() { HasErrors = true, Result = ex.Message };
            }
        }

        private string PrepareArgs(params Object[] args)
        {
            string _argumentsString = string.Format("\"{0}\"", _FilePath);
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = string.Format("\"{0}\"", args[i]);
                _argumentsString += string.Format(" {0}", (string)args[i]);
            }

            return _argumentsString;
        }
    }

    public class Results
    {
        public bool HasErrors { get; set; }
        public string Result { get; set; }
    }
}

