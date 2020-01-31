using IpcPythonCS.Engine.CSharp.Communication;
using IpcPythonCS.Engine.CSharp.RPC;

namespace IpcPythonCS.Engine.ML
{
    public class SelectiveSearch : RPCWrapper
    {
        public SelectiveSearch(ICommunicator communicator)
            : base(communicator)
        {

        }

        public bool Run(string inputImgPath, int numRegionProposals)
        {
            return CallPythonFunction<bool>(inputImgPath, numRegionProposals);
        }

        public int MachineComp(int a)
        {
            return CallPythonFunction<int>(a);
        }
    }
}
