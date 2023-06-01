using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Enums;

namespace Database.XML {
    public class LoadXMLTable {
        private const string xmlFilePath = "Database/TBL_LOAD.xml";
        public void Insert(Load load, FileType fileType) {
            List<string> lines = new List<string>(File.ReadAllLines(xmlFilePath));
            int endLineIndex = -1;

            for (int i = 0; i < lines.Count; i++) {
                if (lines[i].Contains("</rows>")) {
                    endLineIndex = i;
                }

                if (lines[i].Contains("<TIME_STAMP>" + load.Timestamp + "</TIME_STAMP>")) {
                    Update(load, lines, fileType, i);
                    File.WriteAllLines(xmlFilePath, lines.ToArray());

                    return;
                }
            }

            // ako ne postoji load sa ovim timestamp-om u fajlu, onda se doda novi load na kraj
            AddToEnd(load, lines, endLineIndex);
            File.WriteAllLines(xmlFilePath, lines.ToArray());
        }

        public List<Load> ReadForDeviationCalculation() {
            List<Load> loads = new List<Load>();

            List<string> lines = new List<string>(File.ReadAllLines(xmlFilePath));
            for (int i = 2; i < lines.Count - 1; i += 10) {   // ne gledam <?xml>, <rows> i </rows>, uvecavam za 10 pa ce svaki lines[i] da bude naredni <row>
                int id = Int32.Parse(TrimStartString(TrimEndString(lines[i + 1], "</ID>"), "\t\t<ID>"));
                double forecastValue = Double.Parse(TrimStartString(TrimEndString(lines[i + 3], "</FORECAST_VALUE>"), "\t\t<FORECAST_VALUE>"));
                double measuredValue = Double.Parse(TrimStartString(TrimEndString(lines[i + 4], "</MEASURED_VALUE>"), "\t\t<MEASURED_VALUE>"));
                
                if (forecastValue != -1 && measuredValue != -1) {
                    Load load = new Load(id, forecastValue, measuredValue);
                    loads.Add(load);
                }
            }

            return loads;
        }

        public void UpdateDeviations(List<Load> loads, DeviationType deviationType) {
            List<string> lines = new List<string>(File.ReadAllLines(xmlFilePath));

            int loadIndex = 0;
            for (int i = 2; i < lines.Count - 1; i += 10) {   // ne gledam <?xml>, <rows> i </rows>, uvecavam za 10 pa ce svaki lines[i] da bude naredni <row>
                int id = Int32.Parse(TrimStartString(TrimEndString(lines[i + 1], "</ID>"), "\t\t<ID>"));
                
                if (id == loads[loadIndex].Id) {    // jer se devijacija racuna samo za one kod kojih i measured i forecast value nisu 1
                    if (deviationType == DeviationType.SquDeviation) {
                        lines[i + 6] = "\t\t<SQUARED_DEVIATION>" + loads[loadIndex].SquaredDeviation + "</SQUARED_DEVIATION>";
                    } else {
                        lines[i + 5] = "\t\t<ABSOLUTE_PERCENTAGE_DEVIATION>" + loads[loadIndex].AbsolutePercentageDeviation + "</ABSOLUTE_PERCENTAGE_DEVIATION>";
                    }
                }

                loadIndex++;
            }

            File.WriteAllLines(xmlFilePath, lines.ToArray());
        }

        private List<string> GetXML(Load load) {
            List<string> loadXML = new List<string>();

            loadXML.Add("\t<row>");
            loadXML.Add("\t\t<ID>" + load.Id + "</ID>");
            loadXML.Add("\t\t<TIME_STAMP>" + load.Timestamp + "</TIME_STAMP>");
            loadXML.Add("\t\t<FORECAST_VALUE>" + load.ForecastValue + "</FORECAST_VALUE>");
            loadXML.Add("\t\t<MEASURED_VALUE>" + load.MeasuredValue + "</MEASURED_VALUE>");
            loadXML.Add("\t\t<ABSOLUTE_PERCENTAGE_DEVIATION>" + load.AbsolutePercentageDeviation + "</ABSOLUTE_PERCENTAGE_DEVIATION>");
            loadXML.Add("\t\t<SQUARED_DEVIATION>" + load.SquaredDeviation + "</SQUARED_DEVIATION>");
            loadXML.Add("\t\t<FORECAST_FILE_ID>" + load.ForecastFileID + "</FORECAST_FILE_ID>");
            loadXML.Add("\t\t<MEASURED_FILE_ID>" + load.MeasuredFileId + "</MEASURED_FILE_ID>");
            loadXML.Add("\t</row>");

            return loadXML;
        }

        private void Update(Load load, List<string> lines, FileType fileType, int timestampIndex) {
            if (fileType == FileType.Ostv) {    // treba samo da upisem forecastValue i forecastFileId
                lines[timestampIndex + 1] = "\t\t<FORECAST_VALUE>" + load.ForecastValue + "</FORECAST_VALUE>";
                lines[timestampIndex + 5] = "\t\t<FORECAST_FILE_ID>" + load.ForecastFileID + "</FORECAST_FILE_ID>";
            } else {    // treba samo da upisem measuredValue i measuredFileId
                lines[timestampIndex + 2] = "\t\t<MEASURED_VALUE>" + load.MeasuredValue + "</MEASURED_VALUE>";
                lines[timestampIndex + 6] = "\t\t<MEASURED_FILE_ID>" + load.MeasuredFileId + "</MEASURED_FILE_ID>";
            }
        }


        // dodaje load na kraj fajla 
        private void AddToEnd(Load load, List<string> lines, int endLineIndex) {
            List<string> loadLines = GetXML(load);
            for (int j = loadLines.Count; j >= 0; j--) {
                lines.Insert(endLineIndex, loadLines[j]);
            }
        }

        private string TrimStartString(string source, string trimString) {
            if (source.StartsWith(trimString)) {
                return source.Substring(trimString.Length);
            }

            return source;
        }

        private string TrimEndString(string source, string trimString) {
            if (source.EndsWith(trimString)) {
                return source.Substring(0, source.Length - trimString.Length);
            }

            return source;
        }
    }
}
