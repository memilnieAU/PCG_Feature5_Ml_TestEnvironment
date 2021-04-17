using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Spectrogram;

namespace PCG_ProcessWav
{
    public class Spektogram
    {
        private string _filePathwav;
        private string _filePathFig;
        private string _filePathtxt;
        private string filename;
        private string rootFolder;

        public (string line, double[] fft) CreateFftAndSpectogram(string totalFilePath, string _rootFolder, bool SaveTxt,bool SavePng)
        {
            rootFolder = _rootFolder;
            filename = totalFilePath;
            int længde = filename.Length - 4;
            _filePathFig = filename.Substring(0, længde) + ".png";
            _filePathtxt = filename.Substring(0, længde) + ".txt";
            _filePathwav = filename.Substring(0, længde) + ".wav";

            (double[] audio, int sampleRate) = DataAccess.ReadWAV(_filePathwav);

            var sg = GetSpectogram(audio, sampleRate);
            
            List<double[]> spektogram = sg.GetFFTs();
            double[] powerspectrum = TakeSumAndAvg(spektogram);
            string line = ToLineWithTabs(powerspectrum);
           
            if (SaveTxt) DataAccess.SaveLineAsTxt(line, _filePathtxt);
            if (SavePng) DataAccess.SaveSgAsPNG(sg, _filePathFig);
            return (line, powerspectrum);
        }
        private string ToLineWithTabs(double[] input)
        {
            string line = "";
            if (filename[rootFolder.Length] == 'A')
            {
                line = "1\t";
            }
            else line = "0\t";

            foreach (double d in input)
            {
                line += d + "\t";
            }
            line = line.Replace(',', '.');
            return line;
        }
        
        private SpectrogramGenerator GetSpectogram(double[] audio, int sampleRate)
        {
            if (sampleRate == 2000)
            {
                int targetWidthPx = 300;
                int fftSize1 = (int)MathF.Pow(2, 5);
                int stepSize1 = 80;
                var sg = new SpectrogramGenerator(sampleRate, fftSize: fftSize1, stepSize: stepSize1, maxFreq: 1000);
                sg.Add(audio);
                return sg;
            }
            else /*(sampleRate == 44100)*/
            {

                int fftSize1 = 16384/2;
                int stepSize1 = 400;
                var sg = new SpectrogramGenerator(sampleRate, fftSize: fftSize1, stepSize: stepSize1, maxFreq: 1200);
                sg.Add(audio);
                return sg;
            }

        }


        private double[] TakeSum(List<double[]> input)
        {
            double[] EndeligtArray = new double[input[0].Length];
            for (int i = 0; i < EndeligtArray.Length - 1; i++)
            {
                foreach (double[] doubles in input)
                {
                    EndeligtArray[i] += doubles[i];
                }
            }

            return EndeligtArray;
        }

        private double[] TakeSumAndAvg(List<double[]> input)
        {
            double[] EndeligtArray = new double[input[0].Length];
            for (int i = 0; i < EndeligtArray.Length - 1; i++)
            {
                foreach (double[] doubles in input)
                {
                    EndeligtArray[i] += doubles[i];
                }

                EndeligtArray[i] = EndeligtArray[i] / input[i].Length;
            }

            return EndeligtArray;
        }
    }

   

}
