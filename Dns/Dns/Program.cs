using Client.classi;
using Lib;
using Lib.TypeData;
using Lib.Udp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            GestioneUdp u = new GestioneUdp(7000);
            /*Dictionary<string, int> ip;
            ip = new Dictionary<string, int>();
           StreamReader sr = new StreamReader("./Server/mapIp.txt");
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] split = line.Split('=');
                ip.Add(split[0], int.Parse(split[1]));
            }
            sr.Close();*/
            while(true)
            {

            Console.WriteLine("Risoluzione ricorsiva? Y/N");
            string ricorsiva = Console.ReadLine();
            Console.WriteLine("Ip?");
            string ip = Console.ReadLine();
            MessaggioUdp mu = new MessaggioUdp();
                mu.ip = "localhost";
                mu.porta = 6000;
                Messaggio m = new Messaggio();
                m.identificativo = 1120;
                m.Opcode = 0;
                m.NScount = 0;
                m.QDcount = 0;
                m.QR = true;
                m.RD = ricorsiva.ToUpper() == "Y";
                m.ANcount = 0;
                mu.messaggio = m;
                m.QDcount++;
                ResourceRecord s = new ResourceRecord();
                s.RData = new Data();
                s.Type = TypeDatas.NIENTE;
                s.name = ip;
                s.TTL = 0;  
                m.query.Add(s);
                u.Invia(mu);
                MessaggioUdp RISP=u.Ricevi();
                Console.WriteLine(JsonConvert.SerializeObject(RISP.messaggio, Formatting.Indented) + "\r\n");
                Console.WriteLine("Vuoi continuora? Y/N");
                if(Console.ReadLine().ToUpper() == "N")
                    return;
            }
        }
    }
}
