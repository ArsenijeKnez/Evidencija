using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.XML {
    public class AuditXMLTable {
        private const string xmlFilePath = "Database/TBL_AUDIT.xml";

        public void Insert(Audit audit) {
            List<string> lines = new List<string>(File.ReadAllLines(xmlFilePath));
            int endLineIndex = -1;

            for (int i = 0; i < lines.Count; i++) {
                if (lines[i].Contains("</rows>")) {
                    endLineIndex = i;
                }
            }

            // ako ne postoji importefFile sa ovim fileName-om u fajlu, onda se doda novi importedFile na kraj
            AddToEnd(audit, lines, endLineIndex);
            File.WriteAllLines(xmlFilePath, lines.ToArray());
        }

        private List<string> GetXML(Audit audit) {
            List<string> auditXML = new List<string>();

            auditXML.Add("\t<row>");
            auditXML.Add("\t\t<ID>" + audit.Id + "</ID>");
            auditXML.Add("\t\t<TIME_STAMP>" + audit.Timestamp + "</TIME_STAMP>");
            auditXML.Add("\t\t<MESSAGE_TYPE>" + audit.MessageType + "</MESSAGE_TYPE>");
            auditXML.Add("\t\t<MESSAGE>" + audit.Message + "</MESSAGE>");
            auditXML.Add("\t</row>");

            return auditXML;
        }

        private void AddToEnd(Audit audit, List<string> lines, int endLineIndex) {
            List<string> auditLines = GetXML(audit);
            for (int j = auditLines.Count - 1; j >= 0; j--) {
                lines.Insert(endLineIndex, auditLines[j]);
            }
        }
    }
}
