using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server čeka konekciju...");

            ServiceHost host = new ServiceHost(typeof(CsvReader));
            host.Open();

            Console.WriteLine("Uspostavljena konekcija.");



            Console.ReadKey();

            host.Close();
            Console.WriteLine("Zatvorena konekcija");
        }
    }
}
