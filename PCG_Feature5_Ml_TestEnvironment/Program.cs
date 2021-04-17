using System;
using PCG_ProcessWav;

namespace PCG_Feature5_Ml_TestEnvironment
{
    class Program
    {
        private static CreateTrainData createTrainData;
        static void Main(string[] args)
        {

            ProcessTraningData processTraningData = new ProcessTraningData();


            processTraningData.CreateTrainDataSet(false,false);
            Console.WriteLine("Hello World!");
        }
    }
}
