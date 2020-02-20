using IpcPythonCS.Engine.CSharp.Communication;
using IpcPythonCS.Engine.CSharp.RPC;
using System;
using System.IO;

namespace IpcPythonCS.Engine.ML
{
    public class SelectiveSearch : RPCWrapper
    {
        public SelectiveSearch(ICommunicator communicator)
            : base(communicator)
        {
            Directory.CreateDirectory($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\OpenSTSM\Temp\SelectiveOutput\Coordinates");
        }

        public bool Run(string inputImgPath, int numRegionProposals)
        {
            return CallPythonFunction<bool>(inputImgPath, numRegionProposals);
        }
    }
}
