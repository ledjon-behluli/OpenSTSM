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

        public bool LoadModel(string modelPath)
        {
            return CallPythonFunction<bool>(modelPath);
        }

        public bool ConvertToGrayscale(string inputImgPath)
        {
            return CallPythonFunction<bool>(inputImgPath);
        }

        public bool RunSelectiveSearch(int numRegionProposals, float imgResizeFactor)
        {
            return CallPythonFunction<bool>(numRegionProposals, imgResizeFactor);
        }

        public string RunPrediction(int middlePointDistance_Threshold, int outerSelection_Threshold, int decimalPoint_Probability, int regionProposals_Multiplicity, int spatialDistanceOfCoordinatePoints_Threshold, int numOfResultsPerElement, bool useGpuAcceleration)
        {
            return CallPythonFunction<string>(middlePointDistance_Threshold, outerSelection_Threshold, decimalPoint_Probability, regionProposals_Multiplicity, spatialDistanceOfCoordinatePoints_Threshold, numOfResultsPerElement, useGpuAcceleration);
        }
    }
}
