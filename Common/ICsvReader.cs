using System;
using System.Collections.Generic;
using System.IO;
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
        void ReadFiles(MemoryStream dataStraeam);
        
    }
}
