using Common;
using Database;
using Database.XML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Enums;

namespace Service {

    public class CsvReader : ICsvReader
    {
        private DatabaseHandler database;
        private void SetDB() {
            if (ConfigurationManager.AppSettings["dbType"] == "InMemory") {
                database = new DatabaseHandler(DatabaseType.InMemory);
            } else {
                database = new DatabaseHandler(DatabaseType.XML);
            }
        }

        public void ReadFiles(string folderPath) {
            SetDB();
            string[] filePaths = Directory.GetFiles(folderPath);
            foreach (string filePath in filePaths) {
                if(IsValidFileNameFormat(folderPath)){

                    ImportedFile importedFile = new ImportedFile(filePath);
                    database.InsertImportedFile(importedFile);

                    ReadFile(filePath, importedFile);
                }
            }
            Deviation dev = new Deviation();
            if(database.DBType() == DatabaseType.InMemory)
                dev.CalculateDeviation(dev.CalculateDeviationInMem);
            else
                dev.CalculateDeviation(dev.CalculateDeviationXML);
        }

        public void ReadFile(string filePath, ImportedFile currentFile) {
            string[] lines = File.ReadAllLines(filePath);

            FileType fileType = GetFileType(filePath);

            AuditErrorCheck(lines.Length, filePath);

            for (int i = 1; i < lines.Length; i++) {
                if (lines[i] == "") {   // ako je prazan red
                    return;
                }

                string[] delovi = lines[i].Split(',');
                string timeStamp = delovi[0]; 
                double value = Double.Parse(delovi[1]);

                Load load;
                if (fileType == FileType.Ostv) {
                    load = new Load(timeStamp, value, -1, -1, -1, currentFile.Id, -1);
                } else {
                    load = new Load(timeStamp, -1, value, -1, -1, -1, currentFile.Id);
                }

                database.InsertLoad(load, fileType);
            }
        }

        private void AuditErrorCheck(int lineNumber, string filePath) {
            if (lineNumber != 23+1 && lineNumber != 24+1 && lineNumber != 25+1) { // +1 zbog prve linije
                Audit audit = new Audit(DateTime.Now, MessageType.Error, "File '" + Path.GetFileName(filePath) + "' has an invalid number of lines");
                
                database.InsertAudit(audit);
            }
        }

        private bool IsValidFileNameFormat(string filePath) {
            string fileName = Path.GetFileName(filePath);

            string[] delovi = fileName.Split('_');

            if (delovi.Length != 4 || !delovi[3].EndsWith(".csv")) {
                return false;
            }

            return true;
        }

        private FileType GetFileType(string filePath) {
            if (filePath.Contains("forecast")) {
                return FileType.Prog;
            } else {
                return FileType.Ostv;
            }
        }
    }
}
