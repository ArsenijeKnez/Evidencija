using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Enums;

namespace Database.XML {
    public class ImportedFileXMLTable {
        private const string xmlFilePath = "Database/TBL_IMPORTED_FILE.xml";
        public void Insert(ImportedFile importedFile) {
            List<string> lines = new List<string>(File.ReadAllLines(xmlFilePath));
            int endLineIndex = -1;

            for (int i = 0; i < lines.Count; i++) {
                if (lines[i].Contains("</rows>")) {
                    endLineIndex = i;
                }

                if (lines[i].Contains("<FILE_NAME>" + importedFile.FileName + "</FILE_NAME>")) {    // ako postoji vec
                    return;     
                }
            }

            // ako ne postoji importefFile sa ovim fileName-om u fajlu, onda se doda novi importedFile na kraj
            AddToEnd(importedFile, lines, endLineIndex);
            File.WriteAllLines(xmlFilePath, lines.ToArray());
        }
        private List<string> GetXML(ImportedFile importedFile) {
            List<string> importedFileXML = new List<string>();

            importedFileXML.Add("\t<row>");
            importedFileXML.Add("\t\t<ID>" + importedFile.Id + "</ID>");
            importedFileXML.Add("\t\t<FILE_NAME>" + importedFile.FileName + "</FILE_NAME>");
            importedFileXML.Add("\t</row>");

            return importedFileXML;
        }


        // dodaje importedFile na kraj fajla 
        private void AddToEnd(ImportedFile importedFile, List<string> lines, int endLineIndex) {
            List<string> ifLines = GetXML(importedFile);
            for (int j = ifLines.Count; j >= 0; j--) {
                lines.Insert(endLineIndex, ifLines[j]);
            }
        }
    }
}
