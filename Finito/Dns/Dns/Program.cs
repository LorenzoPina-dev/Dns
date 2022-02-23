using Client.classi;
using Lib;
using Lib.TypeData;
using Lib.Udp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string porta;
            if (args.Length > 0)
            {
                porta = args[0];
                Console.WriteLine(int.Parse(porta));
            }
            else
            {
                Console.WriteLine("numero porta");
                porta = Console.ReadLine();
            }
            GestioneUdp u = new GestioneUdp(int.Parse(porta));
            Dictionary<string, int> mIp;
            mIp = new Dictionary<string, int>();
            StreamReader sr = new StreamReader("./mapIp.txt");
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] split = line.Split('=');
                mIp.Add(split[0], int.Parse(split[1]));
            }
            sr.Close();
            while(true)
            {

            Console.WriteLine("Risoluzione ricorsiva? Y/N");
            string ricorsiva = Console.ReadLine();
            Console.WriteLine("Dominio?");
            string Dominio = Console.ReadLine();
                Random r = new Random();
                MessaggioUdp mu = new MessaggioUdp();
                mu.ip = "localhost";
                mu.porta = 6000;
                Messaggio m = new Messaggio();
                m.identificativo = r.Next(1, 2000);
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
                s.name = Dominio;
                s.TTL = 0;  
                m.query.Add(s);
                u.Invia(mu);
                Console.WriteLine("REQUEST:\r\n" + JsonConvert.SerializeObject(mu.messaggio, Formatting.Indented) + "\r\n");
                MessaggioUdp RISP = u.Ricevi();
                Console.WriteLine("RESPONSE:\r\n" + JsonConvert.SerializeObject(RISP.messaggio, Formatting.Indented) + "\r\n");

                if (RISP.messaggio.ANcount>0)
                {
                    Console.WriteLine(Dominio+" = "+ ((A)RISP.messaggio.risposte[0].RData).Ip);
                    string ipDominio = ((A)RISP.messaggio.risposte[0].RData).Ip;
                    TcpClient client = new TcpClient();
                    client.Connect("localhost", mIp[((A)RISP.messaggio.risposte[0].RData).Ip]);
                    string PaginaHtml="";
                    StreamReader srHtml = new StreamReader(client.GetStream());
                    PaginaHtml=srHtml.ReadToEnd();
                    StreamWriter swHtml = new StreamWriter("./pagina.html");
                    swHtml.Write(PaginaHtml);
                    swHtml.Flush();
                    swHtml.Close();
                    srHtml.Close();
                    client.Close();
                    Process.Start("cmd.exe", "/C pagina.html");
                }
                Console.WriteLine("Vuoi continuora? Y/N");
                if(Console.ReadLine().ToUpper() == "N")
                    return;
            }
        }
    }
}
