using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace PCG_ProcessWav
{
    public class ProcessTraningData
    {
        public List<double[]> ffts;
        public List<string> lines;
        public List<string> filer;

        /// <summary>
        /// Bruges til at klargører data fra Wav. filer til spektogrammer, inden de kan bruges til ML larning.
        /// !!!Man skal huske at skrive en sti til der ens data ligger!!!!
        /// </summary>
        /// <param name="PreccesTestData">Hvis true, er det mappen med test data der bruges, altså færre filer. Hvis false er det ALLE data der bruges</param>
        /// <param name="savePngAndTxt">Hvis true, gemmes spektogrammet som png og frekvenskurve som txt fil</param>
        public void CreateTrainDataSet(bool PreccesTestData = true, bool savePngAndTxt = false)
        {

            //TODO Her skal man indtaste den sti på den rootmappe hvor undermapperne skal mapperne lægges i!!!
            //string rootFolder = DataAccess.GetROOTFolder(); //Denne finder mappen til projektet, og er der for dynamisk
            string rootFolder = "C:\\Users\\memil\\Desktop\\ML_Data\\";
            //string rootFolder = $"C:\\{Console.ReadLine()}";


            string folder1 = "Abnormal\\Fra 10 til 20 sekunder";
            string folder2 = "Normal\\Fra 10 til 20 sekunder";
            string folder3 = "TestData";
            string Path1 = FileSystemAccess.GetCombinePath(rootFolder, folder1);
            string Path2 = FileSystemAccess.GetCombinePath(rootFolder, folder2);
            string Path3 = FileSystemAccess.GetCombinePath(rootFolder, folder3);

            filer = new List<string>();

            if (PreccesTestData)
            {
                filer.AddRange(DataAccess.GetWavFilesInFolder(Path3));
            }
            else
            {
                filer.AddRange(DataAccess.GetWavFilesInFolder(Path1));
                filer.AddRange(DataAccess.GetWavFilesInFolder(Path2));
            }

            Console.WriteLine("Antal linjer i alt: " + filer.Count);
            lines = new List<string>();
            ffts = new List<double[]>();
            Spektogram newSpectogram = new Spektogram();

            int tæller = 0;
            Stopwatch sw = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();
            sw.Start();
            foreach (string fullWavFilePath in filer)
            {
                tæller++;
                sw2.Start();

                (string line, double[] fft) = newSpectogram.CreateFftAndSpectogram(fullWavFilePath, rootFolder, savePngAndTxt, savePngAndTxt);
                Console.WriteLine("Tæller: " + tæller + "\tTime: " + sw2.ElapsedMilliseconds +
                                  "ms\tTotal Time:" + ((double)sw.ElapsedMilliseconds / 1000.0) + "s");
                sw2.Reset();
                lines.Add(line);
                ffts.Add(fft);
            }

            sw.Stop();
            sw2.Stop();
            string dateTime = "" + DateTime.Now.ToString("D") + "_" + DateTime.Now.ToString("t");
            dateTime = dateTime.Trim().Replace(':', '_');
            string text = "TraningData_" + dateTime + "_AntalLinjer" + lines.Count + "_.txt";
            string AllLinePath = Path.Combine(rootFolder, text);
            File.WriteAllLines(AllLinePath, lines);

        }
    }
}
