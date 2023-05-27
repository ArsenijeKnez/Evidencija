using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BazaPodataka
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Baza Podataka čeka konekciju...");

            ServiceHost host = new ServiceHost(typeof(BazaPodatakaService));
            host.Open();

            Console.WriteLine("Uspostavljena konekcija.");



            Console.ReadKey();

            host.Close();
            Console.WriteLine("Zatvorena konekcija");
        }
    }
}
