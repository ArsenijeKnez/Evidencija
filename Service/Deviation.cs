using Common;
using Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Enums;

namespace Service
{

    public class Deviation {
        private DatabaseHandler database;
        public Deviation(DatabaseHandler database)
        {
            this.database = database;
        }
        public void CalculateDeviation()
        {
            string deviationCalculationMethod = ConfigurationManager.AppSettings["DeviationCalculationMethod"];

            switch (deviationCalculationMethod)
            {
                case "AbsDeviation":
                    AbsDeviation();
                    break;
                case "SquDeviation":
                    SquDeviation();
                    break;
                default:
                    throw new ConfigurationErrorsException("Niste dobro uneli konfiguraciju proracuna u App.config(AbsDeviation ili SquDeviation).");
            }

        }
        public void AbsDeviation() {
            DeviationUpdate du = new DeviationUpdate();
            foreach (Load load in database.NestoStaVec()) //ovde se vade podaci iz baze
            {  

                double ostvarena = load.MeasuredValue; //mozda da stavimo proveru da li su postavljene obe vrednosti
                double prognozirana = load.ForecastValue;

                double odstupanje = (Math.Abs(ostvarena - prognozirana) / ostvarena) * 100;
                load.AbsolutePercentageDeviation = odstupanje;

                du.UpdateForLoad(load, du.UpdateXML);
            }
        }

        public void SquDeviation() {
            DeviationUpdate du = new DeviationUpdate();
            foreach (Load load in database.NestoStaVec()) //ovde se vade podaci iz baze
            {  

                double ostvarena = load.MeasuredValue; //mozda da stavimo proveru da li su postavljene obe vrednosti
                double prognozirana = load.ForecastValue;

                double odstupanje = Math.Pow((ostvarena - prognozirana) / ostvarena, 2);
                load.SquaredDeviation = odstupanje;

                du.UpdateForLoad(load, du.UpdateXML);
            }
        }
    }
}
