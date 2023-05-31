using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public class ImportedFile {
        private static int nextId;
        public int Id { get; set; }
        public string FileName { get; set; }

        public ImportedFile(string fileName) {
            FileName = fileName;
            Id = nextId;

            nextId++;
        }
    }
}
