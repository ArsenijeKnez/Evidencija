using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void UnosSvih(ICsvReader proxy) 
        {
            Console.Write("Unesite putanju: ");
            string Putanja = Console.ReadLine();
            string[] Directories = Directory.GetDirectories(Putanja);

            foreach (string dir in Directories)
            {

                proxy.ReadFiles(dir);
 
            }
            Console.Write("Unos podataka završen");
        }
  

        static void Main(string[] args)
        {
            ChannelFactory<ICsvReader> server = new ChannelFactory<ICsvReader>("EvidencijaService");

            ICsvReader proxy = server.CreateChannel();

            ChannelFactory<ICsvReader> bazaPodataka = new ChannelFactory<ICsvReader>("BazaPodatakaService");

            ICsvReader Bazaproxy = bazaPodataka.CreateChannel();

            Console.WriteLine("****************************************************");
            Console.WriteLine("*           Evidencija prognozirane i ostvarene     *");
            Console.WriteLine("*             potrošnje električne energije         *");
            Console.WriteLine("****************************************************");
            while (true) {
                Console.WriteLine("Odaberite jednu od ponudjenih opcija: ");
                Console.WriteLine("1.Unos svih podataka iz datoteke");
                string opcija = Console.ReadLine();
                switch (opcija)
                {
                    case "1": 
                        UnosSvih(proxy);
                        break;
                    default:
                        Console.WriteLine("Unesite broj operacije koju želite da izvršite");
                        break;
                }          
            }
        }
    }
}
