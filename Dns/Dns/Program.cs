using Client.classi;
using Lib;
using Lib.TypeData;
using Lib.Udp;
using System;
using System.Collections.Generic;
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

            //while(1==1/*non esce*/)
            //{ 
                //default
                MessaggioUdp mu = new MessaggioUdp();
                mu.ip = "localhost";
                mu.porta = 6000;
                Messaggio m = new Messaggio();
                m.identificativo = 1120;
                m.Opcode = 0;
                m.NScount = 0;
                m.QDcount = 0;
                m.QR = true;
                m.RD = false;
                m.ANcount = 0;
                mu.messaggio = m;
                m.QDcount++;
                ResourceRecord s = new ResourceRecord();
                s.RData = new Data();
                s.Type = TypeDatas.NIENTE;
                s.name = "comp4.alunni.com.";
                s.TTL = 0;  
                m.query.Add(s);
                u.Invia(mu);
                MessaggioUdp RISP=u.Ricevi();
                Console.WriteLine(RISP.messaggio.toCsv());
            Thread.Sleep(100000);
            //}
        }
    }
}
