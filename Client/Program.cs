using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Client {
    class Program {
        static void Main(string[] args) {
            ChannelFactory<IFileHandling> factory = new ChannelFactory<IFileHandling>("FileHandlingService");
            IFileHandling proxy = factory.CreateChannel();

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*           Evidencija prognozirane i ostvarene     *");
            Console.WriteLine("*             potrošnje električne energije         *");
            Console.WriteLine("*****************************************************\n\n");

            while (true) {
                Console.WriteLine("Unesite putanju foldera sa csv fajlovima");
                string folderPath = Console.ReadLine();

                FileSender fileSender = new FileSender(proxy, folderPath);
                fileSender.SendFiles();

                Console.WriteLine("\n********************************\n");
            }
        }
    }
}
