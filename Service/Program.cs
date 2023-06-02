using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Service {
    public class Program {
        static void Main(string[] args) {
            ServiceHost host = new ServiceHost(typeof(FileHandlingService));
            host.Open();

            Console.WriteLine("Servis je spreman\n\n");

            Console.ReadKey();
            host.Close();
            Console.WriteLine("Servis je zatvoren");
        }
    }
}
