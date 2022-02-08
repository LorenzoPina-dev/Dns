using Client.classi;
using Lib;
using Lib.TypeData;
using Lib.Udp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            GestioneUdp u = new GestioneUdp(5003);
            MessaggioUdp mu = new MessaggioUdp();
            mu.ip = "localhost";
            mu.porta = 5000;
            Messaggio m = new Messaggio();
            m.identificativo = 1100;
            m.Opcode = 0;
            m.NScount = 0;
            m.QDcount = 0;
            m.QR = true;
            m.ANcount = 0;
            mu.messaggio = m;
            m.QDcount++;
            ResourceRecord s = new ResourceRecord();
            s.RData = new Data();
            s.Type = TypeDatas.NIENTE;
            s.name = "www.pippo.com.";
            s.TTL = 800;
            m.query.Add(s);
            u.Invia(mu);

        }
    }
}
