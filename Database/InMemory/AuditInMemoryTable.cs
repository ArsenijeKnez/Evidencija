using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.InMemory {
    public class AuditInMemoryTable {
        private Dictionary<int, Audit> auditTable = new Dictionary<int, Audit>();

        public void Insert(Audit audit) {
            auditTable.Add(audit.Id, audit);
        }
    }
}
