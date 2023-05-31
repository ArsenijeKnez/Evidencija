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
    public delegate void DeviationDBType();

    public class Deviation {

        public void CalculateDeviation(DeviationDBType CalculateDeviationDBType)
        {
            CalculateDeviationDBType();
        }
        public Deviation()
        {

        }
        public void CalculateDeviationXML()
        {
            DatabaseHandler XMLdatabase = new DatabaseHandler(DatabaseType.XML);
            //foreach(Load l in XMLdatabase.NestoStaVec())  ovde se vade podaci iz baze
            //    CalculateDeviationForLoad(l); 
            //    XMLdatabase.PromeniLodaIliNesto() ovde se ubacije u bazu
        }

        public void CalculateDeviationInMem()
        {
            DatabaseHandler InMemdatabase = new DatabaseHandler(DatabaseType.InMemory);
            //foreach(Load l in InMemdatabase.NestoStaVec())  ovde se vade podaci iz baze
            //    CalculateDeviationForLoad(l);
            //    InMemdatabase.PromeniLodaIliNesto() ovde se ubacije u bazu
        }
        public void CalculateDeviationForLoad(Load load)
        {
            string deviationCalculationMethod = ConfigurationManager.AppSettings["DeviationCalculationMethod"];

            switch (deviationCalculationMethod)
            {
                case "AbsDeviation":
                    AbsDeviationForLoad(load);
                    break;
                case "SquDeviation":
                    SquDeviationForLoad(load);
                    break;
                default:
                    throw new ConfigurationErrorsException("Niste dobro uneli konfiguraciju proracuna u App.config(AbsDeviation ili SquDeviation).");
            }

        }
        public void AbsDeviationForLoad(Load load) {
            double ostvarena = load.MeasuredValue; //mozda da stavimo proveru da li su postavljene obe vrednosti
            double prognozirana = load.ForecastValue;

            double odstupanje = (Math.Abs(ostvarena - prognozirana) / ostvarena) * 100;
            load.AbsolutePercentageDeviation = odstupanje;
            //Ovde treba upisati u Bazu odredjenu eventom
        }

        public void SquDeviationForLoad(Load load) {
            double ostvarena = load.MeasuredValue; //mozda da stavimo proveru da li su postavljene obe vrednosti
            double prognozirana = load.ForecastValue;

            double odstupanje = Math.Pow((ostvarena - prognozirana) / ostvarena, 2);
            load.SquaredDeviation = odstupanje;
        }
    }
}
