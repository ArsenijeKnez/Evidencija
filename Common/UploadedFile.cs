using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    [DataContract]
    public class UploadedFile : IDisposable {
        [DataMember]
        public MemoryStream Content { get; set; }

        [DataMember]
        public string Name { get; set; }

        public UploadedFile(MemoryStream content, string name) {
            this.Content = content;
            this.Name = name;
        }

        public void Dispose() {
            if (Content == null)
                return;

            try {
                Content.Dispose();
                Content.Close();
                Content = null;
            } catch (System.Exception) {
                Console.WriteLine("Unsuccesful disposing!");
            }
        }
    }
}
