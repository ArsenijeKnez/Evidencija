using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class EvidencijaService : IEvidencija
    {
        public bool IsValidFileNameFormat(string fileName)
        {
            string[] delovi = fileName.Split('_');

            if (delovi.Length != 4 || !delovi[3].EndsWith(".csv"))
            {
                return false;
            }

            return true;
        }

        public string GetFileType(string fileName)
        {
           
            string[] delovi = fileName.Split('_');
            return delovi[0];
        }

        public string GetFileDate(string fileName)
        {
          
            string[] delovi = fileName.Split('_');

     
            string year = delovi[1];
            string month = delovi[2];
            string day = delovi[3].Substring(0,2);

            return $"{year}-{month}-{day}";
        }

        public List<Load> LoadDataFromCsv(string filePath, string filetype)
        {
            List<Load> loads = new List<Load>();

            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length > 1)
            {
                int sati = 0;
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] delovi = line.Split(',');

                    string timeStamp = delovi[0] + " " + delovi[1];
                    if (DateTime.TryParse(timeStamp, out DateTime timeStampDT) &&
                        double.TryParse(delovi[2], out double vrednost))
                    {
                        //TREBA UBACITI PROVERU DA LI VEC POSTOJI
                        Load load = new Load {Id=filetype+timeStamp, Timestamp = timeStampDT}; //za fileID treba baza a odstupanje se kasnije pravi 
                        if (filetype == "prog") 
                            load.ForecastValue= vrednost;
                        else
                            load.MeasuredValue= vrednost;

                        loads.Add(load);
                        sati++;
                    }  
                }
                if (sati != 23 && sati != 24 && sati != 25) //provera sati u danu
                {
                    return new List<Load>(); //nevalidna
                }
                
            }
            
            return loads;
        }

        public void AbsDeviationForLoad(Load load)
        {
            double ostvarena = load.MeasuredValue; //mozda da stavimo proveru da li su postavljene obe vrednosti
            double prognozirana = load.ForecastValue;

            double odstupanje = (Math.Abs(ostvarena - prognozirana) / ostvarena) * 100;
            load.AbsolutePercentageDeviation = odstupanje;
        }

        public void SquDeviationForLoad(Load load)
        {
            double ostvarena = load.MeasuredValue; //mozda da stavimo proveru da li su postavljene obe vrednosti
            double prognozirana = load.ForecastValue;

            double odstupanje = Math.Pow((ostvarena - prognozirana) / ostvarena, 2);
            load.SquaredDeviation = odstupanje;
        }
    }

}
