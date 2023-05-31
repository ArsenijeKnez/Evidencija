using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public enum MessageType {
        Info,
        Warning,
        Error
    }

    public class Audit {
        private static int nextId;
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public MessageType MessageType { get; set; }
        public string Message { get; set; }

        public Audit(DateTime timestamp, MessageType messageType, string message) {
            Timestamp = timestamp;
            MessageType = messageType;
            Message = message;
            Id = nextId;

            nextId++;
        }
    }
}
