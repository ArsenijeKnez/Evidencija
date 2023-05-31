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
