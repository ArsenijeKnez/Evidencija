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

    public class CsvReader
    {
        private DatabaseHandler database;
        private bool lastFileHasInvalidLine = false;

        public CsvReader() {
            if (ConfigurationManager.AppSettings["dbType"] == "InMemory") {
                database = new DatabaseHandler(DatabaseType.InMemory);
            } else {
                database = new DatabaseHandler(DatabaseType.XML);
            }
        }

        public void ReadFiles(Dictionary<string, string> files) {

            foreach (KeyValuePair<string, string> file in files) {
                string fileName = file.Key;

                string[] stringSeparators = new string[] { "\r\n" };
                string[] linesArray = file.Value.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                List<string> lines = new List<string>(linesArray);

                ReadFile(lines, fileName);

                if (lastFileHasInvalidLine) {
                    InvalidLineWarning(fileName);
                    lastFileHasInvalidLine = false;
                }
            }

            Deviation();
        }

        private void ReadFile(List<string> lines, string fileName) {
            FileType fileType = GetFileType(fileName);

            if (!IsFileValid(lines, fileName)) {
                Console.WriteLine("Fajl '" + fileName + "' se odbacuje jer nije validan");
                return;
            }

            Console.WriteLine("Ucitavaju se podaci iz fajla '" + fileName + "'");
            ImportedFile importedFile = new ImportedFile(fileName);
            database.InsertImportedFile(importedFile);

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
                    load = new Load(timeStamp, value, -1, -1, -1, importedFile.Id, -1);
                } else {
                    load = new Load(timeStamp, -1, value, -1, -1, -1, importedFile.Id);
                }

                database.InsertLoad(load, fileType);
            }
        }

        private void Deviation()
        {
            string deviationCalculationMethod = ConfigurationManager.AppSettings["DeviationCalculationMethod"];
            Deviation dev = new Deviation();
            List<Load> loads = database.ReadLoadsForDeviationCalculation();
            dev.DatabaseUpdate += UpdateDatabase;
            switch (deviationCalculationMethod)
            {
                case "AbsDeviation":
                    dev.CalculateDeviation(loads, dev.AbsDeviation);
                    break;
                case "SquDeviation":
                    dev.CalculateDeviation(loads, dev.SquDeviation);
                    break;
                default:
                    throw new ConfigurationErrorsException("Niste dobro uneli konfiguraciju proracuna u App.config(AbsDeviation ili SquDeviation).");
            }
        }
        private void UpdateDatabase(List<Load> loads)
        {
            Deviation dev = new Deviation();
            if (database.DBType() == DatabaseType.InMemory)
                dev.CalculateDeviation(loads, dev.InMemWrite);
            else
                dev.CalculateDeviation(loads, dev.XMLWrite);
        }

        private bool IsFileValid(List<string> lines, string fileName) {
            bool valid = true;

            if (!IsValidFileNameFormat(fileName)) {
                Audit audit = new Audit(DateTime.Now, MessageType.Error, "File '" + fileName + "' has an invalid name");
                database.InsertAudit(audit);

                valid = false;
            } else if (!lines[0].Contains("TIME_STAMP,FORECAST_VALUE") && !lines[0].Contains("TIME_STAMP,MEASURED_VALUE")) {
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

        private void InvalidLineWarning(string fileName) {
            Audit audit = new Audit(DateTime.Now, MessageType.Warning, "File '" + fileName + "' contains invalid lines");
            database.InsertAudit(audit);
        }

        private bool IsLineValid(string line) {
            if (Regex.IsMatch(line, @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2},\d+(\.\d+)?$")) {
                return true;
            }

            lastFileHasInvalidLine = true;

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
