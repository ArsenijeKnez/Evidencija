using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Enums;

namespace Database.InMemory {
    public class LoadInMemoryTable {
        private Dictionary<int, Load> loadTable = new Dictionary<int, Load>();
        public void Insert(Load load, FileType fileType) {
            if (loadTable.ContainsKey(load.Id)) {
                Update(load, fileType);
            } else {
                loadTable.Add(load.Id, load);
            }
        }

        public List<Load> ReadForDeviationCalculation() {
            List<Load> loads = new List<Load>();
            
            foreach (Load load in loadTable.Values) {
                if (load.MeasuredValue != -1 && load.ForecastValue != -1) {
                    loads.Add(new Load(load.Id, load.ForecastValue, load.MeasuredValue));
                }
            }

            return loads;
        }

        public void UpdateDeviations(List<Load> loads, DeviationType deviationType) {
            foreach (Load load in loads) {
                if (deviationType == DeviationType.SquDeviation) {
                    loadTable[load.Id].SquaredDeviation = load.SquaredDeviation;
                } else {
                    loadTable[load.Id].AbsolutePercentageDeviation = load.AbsolutePercentageDeviation;
                }
            }
        }

        private void Update(Load load, FileType fileType) {
            if (fileType == FileType.Ostv) {
                loadTable[load.Id].ForecastValue = load.ForecastValue;
                loadTable[load.Id].ForecastFileID = load.ForecastFileID;
            } else {
                loadTable[load.Id].MeasuredValue = load.MeasuredValue;
                loadTable[load.Id].MeasuredFileId = load.MeasuredFileId;
            }
        }
    }
}
