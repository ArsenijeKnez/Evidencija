using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client {
    public class FileSender {
        private string folderPath;
        private IFileHandling proxy;
        public FileSender(IFileHandling proxy, string folderPath) {
            this.proxy = proxy;
            this.folderPath = folderPath;
        }

        public void SendFiles() {
            string[] filePaths = GetFilePaths(folderPath);
            List<UploadedFile> files = new List<UploadedFile>();

            Console.WriteLine();

            foreach (string filePath in filePaths) {
                var fileName = Path.GetFileName(filePath);
                UploadedFile file = new UploadedFile(GetMemoryStream(filePath), fileName);
                files.Add(file);

                Console.WriteLine("  Fajl '" + fileName + "' se salje.");
            }

            proxy.SendFiles(files);

            foreach (UploadedFile file in files) {
                file.Dispose();
            }
        }

        // Procita datoteku sa prosledjenom lokacijom i kreira i vrati memory stream
        private MemoryStream GetMemoryStream(string filePath) {
            MemoryStream ms = new MemoryStream();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                fileStream.CopyTo(ms);
                fileStream.Close();
            }

            return ms;
        }

        private string[] GetFilePaths(string folderPath) {
            return Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
        }
    }
}
