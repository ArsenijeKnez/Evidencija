﻿using Common;
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
    }
}