using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using Common;
using static Common.Enums;

namespace Database.XML {
    public class XMLDatabase {
        public LoadXMLTable LoadTable { get; set; } = new LoadXMLTable();
        public AuditXMLTable AuditTable { get; set; } = new AuditXMLTable();
        public ImportedFileXMLTable ImportedFileTable { get; set; } = new ImportedFileXMLTable();

        public XMLDatabase() {
            CreateXMLFiles();
        }

        private void CreateXMLFiles() {
            if (File.Exists("Database/TBL_AUDIT.xml")) {
                CreateXMLFile("Database/TBL_AUDIT.xml");
            }

            if (File.Exists("Database/TBL_IMPORTED_FILE.xml")) {
                CreateXMLFile("Database/TBL_IMPORTED_FILE.xml");
            }

            if (File.Exists("Database/TBL_LOAD.xml")) {
                CreateXMLFile("Database / TBL_LOAD.xml");
            }
        }

        private void CreateXMLFile(string filePath) {
            File.Create(filePath);

            string initialXML =
                "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>" + Environment.NewLine +
                "<rows>" + Environment.NewLine +
                "</rows>" + Environment.NewLine;

            File.WriteAllText(filePath, initialXML);
        }
    }
}
