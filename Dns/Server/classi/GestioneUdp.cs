using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lib;
using Lib.Udp;
namespace Server.classi
{
    class GestioneUdp
    {
        public string ip;
        public int porta;
        UdpClient client;
        Queue<MessaggioUdp> DaInviare, DaElaborare;
        public bool Termina;
        MessaggioUdp request;
        DatiCondivisi d;
        public GestioneUdp(int porta,DatiCondivisi d)
        {
            this.d = d;
            client = new UdpClient(porta);
            DaInviare = new Queue<MessaggioUdp>();
            DaElaborare = new Queue<MessaggioUdp>();
            /* Thread s = new Thread(Server);
             Thread c = new Thread(Client);
             Thread v = new Thread(Elabora);
             s.Start();
             c.Start();
             v.Start();*/
            Thread gestione = new Thread(GestioneRichieste);
            gestione.Start();
        }
        public void GestioneRichieste()
        {
            while(!Termina)
            {
                MessaggioUdp m= Ricevi();
                ElaboraM(m);
            }
        }
        public void ElaboraM (MessaggioUdp mUdp)
        {
            Messaggio m = mUdp.messaggio;
            if (m.QR)
            {
                request = mUdp;
                if (m.Opcode == 0)
                {
                    GestioneRequest.ResolveStandardQuery(d, mUdp);
                }
            }

        }
        public void Elabora()
        {
            while(!Termina)
            {
                if (DaElaborare.Count > 0)
                {
                    MessaggioUdp m = DaElaborare.Dequeue();
                    if (m != null)
                        ElaboraM(m);
                }
            }
        }
        public void Client()
        {
            while(!Termina)
            {
                if (DaInviare.Count > 0)
                {
                    MessaggioUdp m = DaInviare.Dequeue();
                    if (m != null)
                        Invia(m);
                }
            }
        }
        public void Server()
        {
            while(!Termina)
            {
                MessaggioUdp m = Ricevi();
                DaElaborare.Enqueue(m);
            }
        }
        public MessaggioUdp Ricevi()
        {
            IPEndPoint riceveEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] dataReceived = client.Receive(ref riceveEP);
            MessaggioUdp mUdp = new MessaggioUdp();
            mUdp.messaggio=new Messaggio( Encoding.ASCII.GetString(dataReceived));
            mUdp.ip = riceveEP.Address.ToString();
            mUdp.porta = riceveEP.Port;
            return mUdp;
        }
        public static void Invia(MessaggioUdp m)
        {
            byte[] data = Encoding.ASCII.GetBytes(m.messaggio.toCsv());

            UdpClient client = new UdpClient();
            client.Send(data, data.Length, m.ip, m.porta);
        }
    }
}
