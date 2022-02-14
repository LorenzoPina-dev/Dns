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
    public class GestioneRequest
    {
        public enum TypeRisposta
        {
            Additional, Risposta, Niente
        }
        public struct Risposta
        {
            public TypeRisposta tipo;
            public ResourceRecord risposta;
        }

        public static Risposta ResolveStandardQuery(DatiCondivisi d, MessaggioUdp request)
        {
            if (d.Cache.ContainsKey(request.messaggio.query[0].name))
            {
                return new Risposta() { tipo = TypeRisposta.Risposta, risposta = d.Cache[request.messaggio.query[0].name] };
            }
            else if (d.A.ContainsKey(request.messaggio.query[0].name))
            {
                return new Risposta() { tipo = TypeRisposta.Risposta, risposta = d.A[request.messaggio.query[0].name] };
            }
            else
            {
                return ResolveLivelliDominio(d, request, 3);
            }
            return new Risposta() { tipo = TypeRisposta.Niente };
        }
        public static Risposta ResolveInverseQuery(DatiCondivisi d, MessaggioUdp request)
        {
            if(d.PTR.ContainsKey(request.messaggio.query[0].name))
            {
                return new Risposta() { tipo = TypeRisposta.Risposta, risposta = d.PTR[request.messaggio.query[0].name] };
            }
            return new Risposta() { tipo = TypeRisposta.Niente };
        }
        private static Risposta ResolveLivelliDominio(DatiCondivisi d, MessaggioUdp request, int livello)
        {
            string[] split = request.messaggio.query[0].name.Split('.');
            string name = "";
            if (livello >= 0)
            {
                for (int i = 0; i < livello; i++)
                    name = split[split.Length - 2 - i] + "." + name;
                name = name.Substring(0, name.Length - 1);
                if (d.NS.ContainsKey(name))
                {
                    d.Cache.Add(request.messaggio.query[0].name, d.A[((NS)d.NS[name].RData).Dominio]);
                    if (livello == 3)
                    {
                        return new Risposta() { tipo = TypeRisposta.Risposta, risposta = d.A[((NS)d.NS[name].RData).Dominio] };
                        /* request.messaggio.risposte.Add(d.A[((NS)d.NS[name].RData).Dominio]);
                        request.messaggio.ANcount++;*/
                    }
                    else
                    {
                        return new Risposta() { tipo = TypeRisposta.Additional, risposta = d.A[((NS)d.NS[name].RData).Dominio] };
                        /*request.messaggio.additional.Add(d.A[((NS)d.NS[name].RData).Dominio]);
                        request.messaggio.ARcount++;*/
                    }
                }
                Risposta ris = ResolveLivelliDominio(d, request, livello - 1);
                if (ris.tipo != TypeRisposta.Niente)
                    return ris;
            }
            return new Risposta() { tipo = TypeRisposta.Niente };
        }
    }
}
