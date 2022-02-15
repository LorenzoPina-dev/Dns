using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Lib;
using Lib.TypeData;
using Lib.Udp;
using static Server.classi.GestioneRequest;
using Newtonsoft.Json;


namespace Server.classi
{
    class GestioneUdp
    {
        public string ip;
        public int porta;
        UdpClient client;
        Queue<MessaggioUdp> DaInviare, DaElaborare;
        public bool Termina;
        Dictionary<int,MessaggioUdp> request;
        DatiCondivisi d;
        public GestioneUdp(DatiCondivisi d)
        {
            this.d = d;
            client = new UdpClient(d.porta);
            DaInviare = new Queue<MessaggioUdp>();
            DaElaborare = new Queue<MessaggioUdp>();
            request = new Dictionary<int, MessaggioUdp>();
            Thread s = new Thread(Server);
             Thread c = new Thread(Client);
             Thread v = new Thread(Elabora);
             s.Start();
             c.Start();
             v.Start();
            /*Thread gestione = new Thread(GestioneRichieste);
            gestione.Start();*/
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
            if(m.QDcount==0)
            {
                DaInviare.Enqueue(new MessaggioUdp() { ip = "localhost", porta = mUdp.porta, messaggio = mUdp.messaggio });
                return;
            }
            if (m.QR)
            {
                ResourceRecord record = mUdp.messaggio.query[0];
                if (!request.ContainsKey(mUdp.messaggio.identificativo))
                    request.Add(mUdp.messaggio.identificativo, mUdp);
                if (m.Opcode == 0)
                {
                    Risposta risp=  GestioneRequest.ResolveStandardQuery(d, mUdp,record);
                    if (risp.tipo != TypeRisposta.Niente)
                    {
                        switch(risp.tipo)
                        {
                            case TypeRisposta.Additional:
                                mUdp.messaggio.ARcount++;
                                mUdp.messaggio.additional.Add(risp.risposta);
                                if (mUdp.messaggio.RD == true)
                                    DaInviare.Enqueue(new MessaggioUdp() { ip = "localhost", porta = d.ip[((A)risp.risposta.RData).Ip], messaggio = mUdp.messaggio });
                                else if (d.possoIterativa)
                                {
                                    mUdp.messaggio.QR = false;
                                    DaInviare.Enqueue(new MessaggioUdp() { ip = "localhost", porta = mUdp.porta, messaggio = mUdp.messaggio });
                                }
                                else
                                    DaInviare.Enqueue(new MessaggioUdp() { ip = "localhost", porta = d.ip[((A)risp.risposta.RData).Ip], messaggio = mUdp.messaggio });
                                break;
                            case TypeRisposta.Risposta:
                                mUdp.messaggio.QR = false;
                                mUdp.messaggio.ANcount++;
                                mUdp.messaggio.risposte.Add(risp.risposta);
                                DaInviare.Enqueue(new MessaggioUdp() { ip = "localhost", porta = mUdp.porta, messaggio = mUdp.messaggio });
                                break;
                        }
                    }
                    else
                    {
                        mUdp.messaggio.RCode = 3;
                        DaInviare.Enqueue(new MessaggioUdp() { ip = "localhost", porta = mUdp.porta, messaggio = mUdp.messaggio });
                    }
                }
                else if(m.Opcode==1)
                {
                    Risposta risp = GestioneRequest.ResolveInverseQuery(d, mUdp);
                    if (risp.tipo != TypeRisposta.Niente)
                    {
                        switch (risp.tipo)
                        {
                            case TypeRisposta.Additional:
                                mUdp.messaggio.ARcount++;
                                mUdp.messaggio.additional.Add(risp.risposta);
                                break;
                            case TypeRisposta.Risposta:
                                mUdp.messaggio.QR = false;
                                mUdp.messaggio.ANcount++;
                                mUdp.messaggio.risposte.Add(risp.risposta);
                                break;
                        }
                        Console.WriteLine(JsonConvert.SerializeObject(mUdp.messaggio, Formatting.Indented) + "\r\n");
                        DaInviare.Enqueue(new MessaggioUdp() { ip = "localhost", porta = d.ip[mUdp.messaggio.query[0].name], messaggio = mUdp.messaggio });
                    }
                    else
                    {
                        mUdp.messaggio.RCode = 3;
                        DaInviare.Enqueue(new MessaggioUdp() { ip = "localhost", porta = mUdp.porta, messaggio = mUdp.messaggio });
                    }
                }
            }
            else
            {
                if (!mUdp.messaggio.RD && mUdp.messaggio.RCode == 0 && mUdp.messaggio.ANcount < mUdp.messaggio.QDcount)
                {
                    mUdp.messaggio.QR = true;
                    DaInviare.Enqueue(new MessaggioUdp() { ip = "localhost", porta = d.ip[((A)mUdp.messaggio.additional[mUdp.messaggio.ARcount - 1].RData).Ip], messaggio = mUdp.messaggio });

                }
                else
                {
                    if (mUdp.messaggio.ANcount > 0)
                        d.Cache.Add(mUdp.messaggio.risposte[0].name, mUdp.messaggio.risposte[0]);
                    Console.WriteLine(JsonConvert.SerializeObject(mUdp.messaggio, Formatting.Indented) + "\r\n");
                    DaInviare.Enqueue(new MessaggioUdp() { ip = "localhost", porta = request[mUdp.messaggio.identificativo].porta, messaggio = mUdp.messaggio });
                    request = new Dictionary<int, MessaggioUdp>();
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
                        Invia(client,m);
                }
            }
        }
        public void Server()
        {
            while(!Termina)
            {
                MessaggioUdp m = Ricevi();
                Console.WriteLine(JsonConvert.SerializeObject(m.messaggio, Formatting.Indented) + "\r\n");
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
        public static void Invia(UdpClient client,MessaggioUdp m)
        {
            byte[] data = Encoding.ASCII.GetBytes(m.messaggio.toCsv());
            client.Send(data, data.Length, m.ip, m.porta);
        }
    }

}
