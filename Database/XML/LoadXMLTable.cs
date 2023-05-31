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
    }
}
