using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Database {
    public class InMemoryDatabase {
        private InMemoryDatabase() { }

        private static readonly Lazy<InMemoryDatabase> lazyInstance = new Lazy<InMemoryDatabase>(() => {
            return new InMemoryDatabase();
        });

        public static InMemoryDatabase Instance {
            get {
                return lazyInstance.Value;
            }
        }

        private ConcurrentDictionary<string, Load> loadDB = new ConcurrentDictionary<string, Load>();
        private ConcurrentDictionary<string, Audit> auditDB = new ConcurrentDictionary<string, Audit>();
        private ConcurrentDictionary<string, ImportedFile> importefFileDB = new ConcurrentDictionary<string, ImportedFile>();


        public Dictionary<string, object> GetFileData(string fileBeginsWith, Type type) {
            Dictionary<string, object> fileData = new Dictionary<string, object>();
            List<string> keys;

            if (type == typeof(Audit)) {
                keys = auditDB.Keys.Where(t => t.StartsWith(fileBeginsWith, StringComparison.InvariantCultureIgnoreCase)).ToList();
                
            } else if (type == typeof(Load)) {
                keys = loadDB.Keys.Where(t => t.StartsWith(fileBeginsWith, StringComparison.InvariantCultureIgnoreCase)).ToList();
            } else {
                keys = importefFileDB.Keys.Where(t => t.StartsWith(fileBeginsWith, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            keys.ForEach(t => AddToDict(fileData, t, type));

            return fileData;
        }

        private void AddToDict(Dictionary<string, object> fileData, string key, Type type) {
            if (type == typeof(Audit)) {
                Audit fd;
                if (auditDB.TryGetValue(key, out fd)) {
                    fileData.Add(key, fd);
                }
            } else if (type == typeof(Load)) {
                Load fd;
                if (loadDB.TryGetValue(key, out fd)) {
                    fileData.Add(key, fd);
                }
            } else {
                ImportedFile fd;
                if (importefFileDB.TryGetValue(key, out fd)) {
                    fileData.Add(key, fd);
                }
            }
        }

        public bool InsertFile<T>(string fileName, T objectDB) {
            if (objectDB.GetType() == typeof(Audit)) {
                return auditDB.TryAdd(fileName, objectDB as Audit);

            } else if (objectDB.GetType() == typeof(Load)) {
                return loadDB.TryAdd(fileName, objectDB as Load);
            } else {
                return importefFileDB.TryAdd(fileName, objectDB as ImportedFile);
            }
        }
    }
}
