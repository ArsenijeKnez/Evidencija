using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Load
    {
        public Load()
        {
        }
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double ForecastValue { get; set; }
        public double MeasuredValue { get; set; }
        public double AbsolutePercentageDeviation { get; set; }
        public double SquaredDeviation { get; set; }
        public string ImportedForecastFileId { get; set; }
        public string MeasuredFileId { get; set; }


    }
}
