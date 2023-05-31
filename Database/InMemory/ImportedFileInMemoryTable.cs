using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.InMemory {
    public class ImportedFileInMemoryTable {
        public Dictionary<int, ImportedFile> importedFileTable = new Dictionary<int, ImportedFile>();

        public void Insert(ImportedFile importedFile) {
            foreach (ImportedFile iFile in importedFileTable.Values) {
                if (iFile.FileName == importedFile.FileName) {
                    return;
                }
            }

            importedFileTable.Add(importedFile.Id, importedFile);
        }
    }
}
