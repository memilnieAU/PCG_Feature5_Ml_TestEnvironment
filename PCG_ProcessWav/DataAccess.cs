using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Spectrogram;

namespace PCG_ProcessWav
{
    public static class DataAccess
    {
        public static (double[] audio, int sampleRate) ReadWAV(string filePath, double multiplier = 16_000)
        {
            using var afr = new NAudio.Wave.AudioFileReader(filePath);
            int sampleRate = afr.WaveFormat.SampleRate;
            int sampleCount = (int)(afr.Length / afr.WaveFormat.BitsPerSample / 8);
            int channelCount = afr.WaveFormat.Channels;
            var audio = new List<double>(sampleCount);
            var buffer = new float[sampleRate * channelCount];
            int samplesRead = 0;
            while ((samplesRead = afr.Read(buffer, 0, buffer.Length)) > 0)
                audio.AddRange(buffer.Take(samplesRead).Select(x => x * multiplier));
            return (audio.ToArray(), sampleRate);
        }
        
        public static void SaveLineAsTxt(string input,string _filePathtxt)
        {
            File.WriteAllText(_filePathtxt, input);
        }

     
        public static string GetROOTFolder()
        {
            string startupPath = Directory.GetCurrentDirectory();
            string projectRootFolder = Path.GetFullPath(Path.Combine(startupPath, @"..\..\..\..\PCG_Feature5_ML\\"));
            return projectRootFolder;
        }

        public static string[] GetWavFilesInFolder(string RootAndSubFolder)
        {
            string[] fileEntries = Directory.GetFiles(RootAndSubFolder, "*.wav");
            return fileEntries;
        }

        public static void SaveSgAsPNG(SpectrogramGenerator sg, string filePathFig)
        {
            sg.SaveImage(filePathFig ,intensity:8,dB:false , roll: true);
        }
    }
    public static class FileSystemAccess
    {
        public static string GetCombinePath(string path,string fileName)
        {
            return Path.Combine(path+ fileName);
        }
        
    }

}
