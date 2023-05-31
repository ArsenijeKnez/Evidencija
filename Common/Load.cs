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
        public string ForecastFileID { get; set; }
        public string MeasuredFileId { get; set; }

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
    }
}
