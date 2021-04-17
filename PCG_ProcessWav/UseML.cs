using System;
using System.IO;
using System.Reflection;
using PCG_Feature5_MLML.Model;

namespace PCG_ProcessWav
{

    public class UseML
    {
        private readonly string rootFolder;
        private readonly ProcessTraningData _traningData;

        public UseML(string _RootFolder, ProcessTraningData traningData)
        {
            rootFolder = _RootFolder;
            _traningData = traningData;
        }



        public void RealTest()
        {

            var allData = _traningData.ffts;
            int AntalFaktiskRaskTot = 0;
            int AntalPosRask = 0;
            int AntalFaslkSyge = 0;
            int AntalFaktiskeSygeTot = 0;
            int AntalFalskRask = 0;
            int AntalPosSyge = 0;
            int x = 0;
            string DelResult = "";

            Console.WriteLine("Du kan først bruge denne metode når du har lavet en ML lokalt!!!");

            {
                foreach (double[] data in allData)
                {
                    var input = new ModelInput();
                    int i = 1;
                    var props = input.GetType().GetProperties();
                    foreach (PropertyInfo propertyInfo in props)
                    {
                        float floatpoint;
                        if (i >= data.Length)
                        {
                            break;
                            floatpoint = 0;
                        }
                        else
                        {
                            floatpoint = (float)data[i];
                        }

                        PropertySetLooping(input, props[i].Name, floatpoint);
                        //propertyInfo.SetValue(props[i].GetType(),data[i] );
                        // do stuff here
                        i++;
                    }
                    // Load model and predict output of sample data


                    var predictionResult = ConsumeModel.Predict(input);
                    //Console.WriteLine("======================NEW======================");
                    //Console.WriteLine("Using model to make single prediction -- Comparing actual Col0 with predicted Col0 from sample data...");
                    //Console.WriteLine($"Dine data var dette filnavn: {createTrainData.filer[x]}");
                    //Console.WriteLine($"\n\nPredicted Col0 value {predictionResult.Prediction} \nPredicted Col0 scores: [{String.Join(",", predictionResult.Score)}]\n\n");
                    //Console.WriteLine("=============== End of process, hit any key to finish ===============");


                    string udskrift = "======================NEW======================\n";

                    if (_traningData.filer[x][rootFolder.Length] == 'N') //Faktisk rask
                    {
                        udskrift += "Faktisk Rask";
                        AntalFaktiskRaskTot++;
                        if (Convert.ToInt32(predictionResult.Prediction) == 0) //Detekteret rask
                        {
                            udskrift += "\nDetekteret Rask :\t00";
                            AntalPosRask++;
                        }
                        else //Detekteret syg
                        {
                            udskrift += "\nDetekteret Syg  :\t01";
                            AntalFaslkSyge++;
                        }
                    }
                    else //Faktisk Syg
                    {
                        udskrift += "Faktisk Syg";
                        AntalFaktiskeSygeTot++;
                        if (Convert.ToInt32(predictionResult.Prediction) == 1) //Detekteret Syg
                        {
                            udskrift += "\nDetekteret Syg  :\t11";
                            AntalPosSyge++;
                        }
                        else //Detekteret rask
                        {
                            udskrift += "\nDetekteret Rask :\t10";
                            AntalFalskRask++;
                        }
                    }

                    int længde = rootFolder.Length;
                    udskrift += "\n" + _traningData.filer[x].Substring(længde) + "\n\n";

                    Console.WriteLine(udskrift);
                    DelResult += udskrift + "\n";
                    //Console.WriteLine("____________END_____________");
                    // Console.ReadKey();
                    x++;

                }

                string udskriftResult = "";
                float procentRigtigRask =
                    MathF.Round((float)(((double)AntalPosRask / (double)AntalFaktiskRaskTot) * (double)100), 2);
                float procentRigtigSyge =
                    MathF.Round((float)(((double)AntalPosSyge / (double)AntalFaktiskeSygeTot) * (double)100), 2);

                string dateTime = "" + DateTime.Now.ToString("D") + "_" + DateTime.Now.ToString("t");
                dateTime = dateTime.Trim().Replace(':', '_');
                string text = "RealDataTest_" + dateTime + "_.txt";

                udskriftResult = text + "\n";
                udskriftResult +=
                    ($"Der var {AntalFaktiskRaskTot} som faktisk var Raske, men vi detektere {AntalPosRask}, svarende til {procentRigtigRask}%"
                    );
                udskriftResult +=
                    ($"\nDer var {AntalFaktiskeSygeTot} som faktisk var Syge , men vi detektere {AntalPosSyge}, svarende til {procentRigtigSyge}%\n\n"
                    );
                udskriftResult += "Her efter kommer delresultaterne efter alle målinger\n";
                Console.WriteLine(udskriftResult);
                udskriftResult += DelResult;

                string AllLinePath = Path.Combine(rootFolder, text);
                File.WriteAllText(AllLinePath, udskriftResult);
            }
        }

        static void PropertySetLooping(object p, string propName, object value)
        {
            Type t = p.GetType();
            foreach (PropertyInfo info in t.GetProperties())
            {
                if (info.Name == propName && info.CanWrite)
                {
                    info.SetValue(p, value, null);
                }
            }
        }
    }
}
