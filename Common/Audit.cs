using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum MessageType
    {
        Info,
        Warning,
        Error
    }
    public class Audit
    {
        public Audit()
        {
        }
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public MessageType MessageType { get; set; }
        public string Message { get; set; }

    }
}
