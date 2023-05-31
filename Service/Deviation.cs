using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service {
    public class Deviation {
        public void AbsDeviationForLoad(Load load) {
            double ostvarena = load.MeasuredValue; //mozda da stavimo proveru da li su postavljene obe vrednosti
            double prognozirana = load.ForecastValue;

            double odstupanje = (Math.Abs(ostvarena - prognozirana) / ostvarena) * 100;
            load.AbsolutePercentageDeviation = odstupanje;
        }

        public void SquDeviationForLoad(Load load) {
            double ostvarena = load.MeasuredValue; //mozda da stavimo proveru da li su postavljene obe vrednosti
            double prognozirana = load.ForecastValue;

            double odstupanje = Math.Pow((ostvarena - prognozirana) / ostvarena, 2);
            load.SquaredDeviation = odstupanje;
        }
    }
}
