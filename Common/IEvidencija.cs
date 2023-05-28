using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IEvidencija
    {
        [OperationContract]
        bool IsValidFileNameFormat(string fileName);
        [OperationContract]
        string GetFileType(string fileName);
        [OperationContract]
        string GetFileDate(string fileName);
        [OperationContract]
        List<Load> LoadDataFromCsv(string filePath, string filetype, string fileDate);
        [OperationContract]
        void AbsDeviationForLoad(Load load);
        [OperationContract]
        void SquDeviationForLoad(Load load);
    }
}
