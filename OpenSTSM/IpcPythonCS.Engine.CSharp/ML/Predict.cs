using IpcPythonCS.Engine.CSharp.Communication;
using IpcPythonCS.Engine.CSharp.RPC;
using System;
using System.IO;

namespace IpcPythonCS.Engine.ML
{
    public class Predict : RPCWrapper
    {
        public Predict(ICommunicator communicator)
            : base(communicator)
        {
            Directory.CreateDirectory($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\OpenSTSM\Temp\SelectiveOutput\Coordinates");
        }

        public bool RunSelectiveSearch(string inputImgPath, int numRegionProposals)
        {
            return CallPythonFunction<bool>(inputImgPath, numRegionProposals);
        }

        public string RunPrediction(string modelPath, int middlePointDistance_Threshold, int outerSelection_Threshold, int decimalPoint_Probability, int regionProposals_Multiplicity, int spatialDistanceOfCoordinatePoints_Threshold, int numOfResultsPerElement, bool useGpuAcceleration)
        {
            return CallPythonFunction<string>(modelPath, middlePointDistance_Threshold, outerSelection_Threshold, decimalPoint_Probability, regionProposals_Multiplicity, spatialDistanceOfCoordinatePoints_Threshold, numOfResultsPerElement, useGpuAcceleration);
        }
    }
}
