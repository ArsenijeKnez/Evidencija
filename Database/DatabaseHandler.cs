using Common;
using Database.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Enums;

namespace Database {
    public class DatabaseHandler {
        private InMemoryDatabase IMDatabase;
        private XMLDatabase XMLDatabase;
        private DatabaseType databaseType;

        public DatabaseType DBType() { return databaseType; }

        public DatabaseHandler(DatabaseType databaseType) {
            if (databaseType == DatabaseType.XML) {
                XMLDatabase = new XMLDatabase();
            } else {
                IMDatabase = new InMemoryDatabase();
            }
        }

        public void InsertLoad(Load load, FileType fileType) {
            if (databaseType == DatabaseType.XML) {
                XMLDatabase.LoadTable.Insert(load, fileType);
            } else {
                IMDatabase.LoadTable.Insert(load, fileType);
            }
        }

        public void InsertAudit(Audit audit) {
            if (databaseType == DatabaseType.XML) {
                XMLDatabase.AuditTable.Insert(audit);
            } else {
                IMDatabase.AuditTable.Insert(audit);
            }
        }

        public void InsertImportedFile(ImportedFile importedFile) {
            if (databaseType == DatabaseType.XML) {
                XMLDatabase.ImportedFileTable.Insert(importedFile);
            } else {
                IMDatabase.ImportedFileTable.Insert(importedFile);
            }
        }

        public List<Load> ReadLoadsForDeviationCalculation() {
            if (databaseType == DatabaseType.XML) {
                return XMLDatabase.LoadTable.ReadForDeviationCalculation();
            } else {
                return IMDatabase.LoadTable.ReadForDeviationCalculation();
            }
        }

        public void UpdateDeviationsXML(List<Load> loads, DeviationType deviationType) {
            XMLDatabase.LoadTable.UpdateDeviations(loads, deviationType);
        }

        public void UpdateDeviationsInMemory(List<Load> loads, DeviationType deviationType) {
            IMDatabase.LoadTable.UpdateDeviations(loads, deviationType);

        }
    }
}
