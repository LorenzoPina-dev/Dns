using Lib;
using Lib.TypeData;
using Lib.Udp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.classi
{
    class GestioneRequest
    {

        public static MessaggioUdp ResolveStandardQuery(DatiCondivisi d,MessaggioUdp request)
        {
            if (d.Cache.ContainsKey(request.messaggio.query[0].name))
            {
                request.messaggio.ANcount++;
                request.messaggio.risposte.Add(d.A[request.messaggio.query[0].name]);
            }
            else
            {
                ResolveLivelliDominio(d, request, 1);
            }
            return request;
        }
        private static void ResolveLivelliDominio(DatiCondivisi d, MessaggioUdp request,int livello)
        {
            string[] split = request.messaggio.query[0].name.Split('.');
            string name = "";
            if (livello <4)
            {
                for (int i = 0; i < livello; i++)
                    name = split[split.Length-2 - i]+"."+name;
                name = name.Substring(0, name.Length - 1);
                if (d.A.ContainsKey(name))
                {
                    d.Cache.Add(name, d.A[name]);
                    if (livello == 3)
                    {
                        request.messaggio.risposte.Add(d.A[name]);
                        request.messaggio.ANcount++;
                    }
                    else
                    {
                        request.messaggio.additional.Add(d.A[name]);
                        request.messaggio.ARcount++;
                    }
                }
                else if (d.NS.ContainsKey(name))
                {
                    d.Cache.Add(request.messaggio.query[0].name, d.A[((NS)d.NS[name].RData).Dominio]);
                    if (livello == 3)
                    {
                        request.messaggio.risposte.Add(d.A[((NS)d.NS[name].RData).Dominio]);
                        request.messaggio.ANcount++;
                    }
                    else
                    {
                        request.messaggio.additional.Add(d.A[((NS)d.NS[name].RData).Dominio]);
                        request.messaggio.ARcount++;
                    }
                }
                 ResolveLivelliDominio(d, request, livello + 1);
            }
        }
    }
}
