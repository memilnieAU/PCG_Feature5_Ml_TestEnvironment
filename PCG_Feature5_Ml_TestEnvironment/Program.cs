using System;
using PCG_ProcessWav;

namespace PCG_Feature5_Ml_TestEnvironment
{
    class Program
    {
        private static CreateTrainData createTrainData;
        static void Main(string[] args)
        {
            //TODO Her skal man indtaste den sti på den rootmappe hvor undermapperne skal mapperne lægges i!!!
            //string rootFolder = DataAccess.GetROOTFolder();                   //Denne finder mappen til projektet, og er der for dynamisk
            string rootFolder = "C:\\Users\\memil\\Desktop\\ML_Data\\";         // Denne finder mappen ved Mads
            //string rootFolder = "C:\\Users\\username\\Desktop\\ML_Data\\";    // Denne finder mappen ved Mads
            //string rootFolder = $"C:\\{Console.ReadLine()}";                  //Denne er blot default

            ProcessTraningData processTraningData = new ProcessTraningData();

            processTraningData.CreateTrainDataSet(rootFolder,false,false);
 
            Console.WriteLine("Nu er proceceringen af data done og vi begynder på prediction!");


            UseML Ml = new UseML(rootFolder, processTraningData);
            Ml.RealTest();

        }
    }
}
