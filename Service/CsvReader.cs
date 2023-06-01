using Common;
using Database;
using Database.XML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
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

        public void ReadFiles(MemoryStream dataStream) {
            SetDB();

            XmlSerializer serializer = new XmlSerializer(typeof(List<MemoryStream>));

            dataStream.Position = 0;
            Dictionary<string, MemoryStream> deserializedData = (Dictionary<string, MemoryStream>)serializer.Deserialize(dataStream);

            foreach (KeyValuePair<string, MemoryStream> file in deserializedData)
            {
                string fileName = file.Key;
                MemoryStream data = file.Value;

                ImportedFile importedFile = new ImportedFile(fileName);
                database.InsertImportedFile(importedFile);

                ReadFile(data, importedFile);
            }

            Deviation dev = new Deviation(database);
            dev.CalculateDeviation();
        }

        private void ReadFile(MemoryStream data, ImportedFile currentFile) {
            data.Position = 0;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(data, Encoding.UTF8))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                    Console.WriteLine(line);
                }
            }
           
            if (lines[lines.Count - 1] == "") {     // ako na kraju fajla postoji prazan red
                lines.RemoveAt(lines.Count - 1);
            }
            string fileName = currentFile.FileName;
            FileType fileType = GetFileType(fileName);

            if (!IsFileValid(lines, fileName)) {
                return;
            }

            for (int i = 1; i < lines.Count; i++) {
                if (lines[i] == "") {   // ako je prazan red (poslednji red je prazan)
                    continue;
                }

                if (!IsLineValid(lines[i])) {
                    continue;
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

        private bool IsFileValid(List<string> lines, string fileName) {
            bool valid = true;

            if (!IsValidFileNameFormat(fileName)) {
                Audit audit = new Audit(DateTime.Now, MessageType.Error, "File '" + fileName + "' has an invalid name");
                database.InsertAudit(audit);

                valid = false;
            } else if (lines[0] != "TIME_STAMP,FORECAST_VALUE" && lines[0] != "TIME_STAMP,MEASURED_VALUE") {
                Audit audit = new Audit(DateTime.Now, MessageType.Error, "File '" + fileName + "' does not have a header");
                database.InsertAudit(audit);

                valid = false;
            } else if (lines.Count != 23 + 1 && lines.Count != 24 + 1 && lines.Count != 25 + 1) { // +1 zbog prve linije
                Audit audit = new Audit(DateTime.Now, MessageType.Error, "File '" + fileName + "' has an invalid number of lines");
                database.InsertAudit(audit);

                valid = false;
            }

            return valid;
        }

        private bool IsLineValid(string line) {
            if (Regex.IsMatch(line, @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2},\d+(\.\d+)?$")) {
                return true;
            }

            return false;
        }

        private bool IsValidFileNameFormat(string fileName) {

            string[] delovi = fileName.Split('_');

            if (delovi.Length != 4 || !delovi[3].EndsWith(".csv")) {
                return false;
            }

            return true;
        }

        private FileType GetFileType(string fileName) {
            if (fileName.Contains("forecast")) {
                return FileType.Prog;
            } else {
                return FileType.Ostv;
            }
        }
    }
}
