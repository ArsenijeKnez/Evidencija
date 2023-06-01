using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public class Load {
        private static int nextId;
        public int Id { get; set; }
        public string Timestamp { get; set; }
        public double ForecastValue { get; set; }
        public double MeasuredValue { get; set; }
        public double AbsolutePercentageDeviation { get; set; }
        public double SquaredDeviation { get; set; }
        public int ForecastFileID { get; set; }
        public int MeasuredFileId { get; set; }

        // konstruktor za ucitavanje load-ova iz csv fajlova (id nije parametar nego se automatski postavlja njegova vrijednost)
        public Load(string timestamp, double forecastValue, double measuredValue, double absolutePercentageDeviation, double squaredDeviation, int forecastFileID, int measuredFileId) {
            Id = nextId;
            Timestamp = timestamp;
            ForecastValue = forecastValue;
            MeasuredValue = measuredValue;
            AbsolutePercentageDeviation = absolutePercentageDeviation;
            SquaredDeviation = squaredDeviation;
            ForecastFileID = forecastFileID;
            MeasuredFileId = measuredFileId;

            nextId++;
        }

        // konstruktor za citanje load-ova iz baze 
        public Load(int id, double forecastValue, double measuredValue) {
            Id = id;
            Timestamp = "";
            ForecastValue = forecastValue;
            MeasuredValue = measuredValue;
            AbsolutePercentageDeviation = -1;
            SquaredDeviation = -1;
            ForecastFileID = -1;
            MeasuredFileId = -1;
        }
    }
}
