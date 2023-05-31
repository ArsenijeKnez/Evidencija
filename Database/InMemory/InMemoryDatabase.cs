using Common;
using Database.InMemory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Database {
    public class InMemoryDatabase {
        public LoadInMemoryTable LoadTable { get; set; } = new LoadInMemoryTable();
        public AuditInMemoryTable AuditTable { get; set; } = new AuditInMemoryTable();
        public ImportedFileInMemoryTable ImportedFileTable { get; set;} = new ImportedFileInMemoryTable();
    }
}
