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

                    List<Load> Ucitani = proxy.LoadDataFromCsv(file, tip, datum);//Ovo trebamo ucitati u bazu    

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

                List<Load> Ucitani = proxy.LoadDataFromCsv(fileName, tip, datum);//Ovo trebamo ucitati u bazu    

                Console.WriteLine("Importovanje završeno!");
            }
            else
            {
                Console.WriteLine("Neispravan format naziva datoteke!");
            }

        }
        static void ProracunOdstupanja(IEvidencija proxy, bool vrsta)
        {

            while (true) {

                Console.WriteLine("Odredte odstupanje za specifičan datum u formatu yyyy-mm-dd ili yyyy-mm-dd hh:mm:ss");
                string date = Console.ReadLine();
              
                if (DateTime.TryParse(date, out DateTime dateTime))
                {
                    Console.WriteLine("Input: " + dateTime);
                    if (date.Contains(" "))
                    {
                        Load l = new Load();//IZ BAZE PRONACI LOAD SA OVIM DATUMOM I VREMENOM
                        if (vrsta) proxy.AbsDeviationForLoad(l); 
                        else proxy.SquDeviationForLoad(l);
                    }
                    else
                    {
                        //foreach(); IZ BAZE PRONACI SVE LOAD SA OVIM DATUMOM
                        //if (vrsta) proxy.AbsDeviationForLoad();
                        //else proxy.SquDeviationForLoad();
                    }
                }
                else
                {
                    Console.WriteLine("Niste uneli validan datum");
                }
            }
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
                Console.WriteLine("3.Proračun Apsolutnog odstupanja");
                Console.WriteLine("4.Proračun Kvadratnog odstupanja");
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
                        ProracunOdstupanja(proxy, true);
                        break;
                    case "4":
                        ProracunOdstupanja(proxy, false);
                        break;
                    default:
                        Console.WriteLine("Unesite broj operacije koju želite da izvršite");
                        break;
                }          
            }
        }
    }
}
