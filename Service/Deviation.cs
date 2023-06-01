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
    public delegate void DatabaseUpdateDelegate(List<Load> loads);
    public class Deviation
    {
        public event DatabaseUpdateDelegate DatabaseUpdated;
        private DeviationType devType;
        public void CalculateDeviation(List<Load> loads,DatabaseUpdateDelegate Method)
        {
            Method(loads);
        }
        public void AbsDeviation(List<Load> loads)
        {
            foreach (Load load in loads)
            {
                double ostvarena = load.MeasuredValue;
                double prognozirana = load.ForecastValue;

                double odstupanje = (Math.Abs(ostvarena - prognozirana) / ostvarena) * 100;
                load.AbsolutePercentageDeviation = odstupanje;
            }
            devType = DeviationType.AbsDeviation;
            DatabaseUpdated?.Invoke(loads);
        }

        public void SquDeviation(List<Load> loads)
        {
            foreach (Load load in loads)
            {

                double ostvarena = load.MeasuredValue;
                double prognozirana = load.ForecastValue;

                double odstupanje = Math.Pow((ostvarena - prognozirana) / ostvarena, 2);
                load.SquaredDeviation = odstupanje;
            }
            devType = DeviationType.SquDeviation;
            DatabaseUpdated?.Invoke(loads);
        }

        public void InMemWrite(List<Load> loads)
        {
            DatabaseHandler InMemdatabase = new DatabaseHandler(DatabaseType.InMemory);
            InMemdatabase.UpdateDeviationsInMemory(loads, devType);
        }

        public void XMLWrite(List<Load> loads)
        {
            DatabaseHandler InMemdatabase = new DatabaseHandler(DatabaseType.XML);
            InMemdatabase.UpdateDeviationsInMemory(loads, devType);
        }
    }
}
