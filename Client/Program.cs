using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void UnosSvih(IEvidencija proxy) 
        {
            Console.Write("Unesite putanju: ");
            string Putanja = Console.ReadLine();
            string[] files = Directory.GetFiles(Putanja);

            foreach (string file in files)
            {
                if (proxy.IsValidFileNameFormat(file))
                {
                    string tip = proxy.GetFileType(file);
                    string datum = proxy.GetFileDate(file);

                    tip = (tip == "forecast") ? "prog" : "ostv";

                    Console.WriteLine($"Importovanje podataka iz datoteke tipa \"{tip}\" sa datumom {datum}...");

                    List<Load> Ucitani = proxy.LoadDataFromCsv(file, tip);//Ovo trebamo ucitati u bazu    

                    if(Ucitani.Count > 0)
                    {
                        foreach(Load Load in Ucitani)
                        {
                            //Upisati u Bazu
                        }
                    }
                    else
                    {
                        Audit audit = new Audit { Id = file, MessageType = MessageType.Error, Message=$"datoteka {file} je odbacena kao nevalidna", Timestamp = DateTime.Parse(datum) }; //U bazu
                    }
                    ImportedFile importovan = new ImportedFile { FileName = file, Id = datum }; //Ubaciti u bazu

                    Console.WriteLine("Importovanje završeno!");
                }
                else
                {
                    Console.WriteLine("Neispravan format naziva datoteke!");
                }
                
            }

        }
        static void UnosJednog(IEvidencija proxy) 
        {
            Console.Write("Unesite putanju: ");
            string fileName = Console.ReadLine();
            if (proxy.IsValidFileNameFormat(fileName))
            {
                string tip = proxy.GetFileType(fileName);
                string datum = proxy.GetFileDate(fileName);
                tip = (tip == "forecast") ? "prog" : "ostv";

                Console.WriteLine($"Importovanje podataka iz datoteke tipa \"{tip}\" sa datumom {datum}...");

                List<Load> Ucitani = proxy.LoadDataFromCsv(fileName, tip);//Ovo trebamo ucitati u bazu    

                Console.WriteLine("Importovanje završeno!");
            }
            else
            {
                Console.WriteLine("Neispravan format naziva datoteke!");
            }

        }
        static void ProracunOdstupanja(IEvidencija proxy)
        {
            throw new NotImplementedException();
        }

        static void Main(string[] args)
        {
            ChannelFactory<IEvidencija> server = new ChannelFactory<IEvidencija>("EvidencijaService");

            IEvidencija proxy = server.CreateChannel();

            ChannelFactory<IEvidencija> bazaPodataka = new ChannelFactory<IEvidencija>("BazaPodatakaService");

            IEvidencija Bazaproxy = bazaPodataka.CreateChannel();

            Console.WriteLine("****************************************************");
            Console.WriteLine("*           Evidencija prognozirane i ostvarene     *");
            Console.WriteLine("*             potrošnje električne energije         *");
            Console.WriteLine("****************************************************");
            while (true) {
                Console.WriteLine("Odaberite jednu od ponudjenih opcija: ");
                Console.WriteLine("1.Unos svih podataka iz datoteke");
                Console.WriteLine("2.Unos novih podataka iz jedne datoteke");
                Console.WriteLine("3.Proracun odstupanja");
                string opcija = Console.ReadLine();
                switch (opcija)
                {
                    case "1": 
                        UnosSvih(proxy);
                        break;
                    case "2": 
                        UnosJednog(proxy);
                        break;
                    case "3":
                        ProracunOdstupanja(proxy);
                        break;
                    default:
                        Console.WriteLine("Unesite broj operacije koju želite da izvršite");
                        break;
                }          
            }
        }
    }
}
