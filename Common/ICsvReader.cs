using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ICsvReader
    {
        [OperationContract]
        void ReadFiles(string folderPath);
        [OperationContract]
        void ReadFile(string filePath, ImportedFile currentFile);
        
    }
}
