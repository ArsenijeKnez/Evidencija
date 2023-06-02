using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service {
    public class FileHandlingService : IFileHandling {
        [OperationBehavior(AutoDisposeParameters = true)]
        public void SendFiles(List<UploadedFile> uploadedFiles) {
            Dictionary<string, string> files = new Dictionary<string, string>();

            foreach (UploadedFile file in uploadedFiles) {
                string fileContent = Encoding.UTF8.GetString(file.Content.ToArray());
                files.Add(file.Name, fileContent);
            }

            Program.CsvReader.ReadFiles(files);

            Console.WriteLine("\n\n********************************\n\n");

            foreach (UploadedFile uploadedFile in uploadedFiles) {
                uploadedFile.Dispose();
            }
        }
    }
}
