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
            public bool AA;
        }

        public static Risposta ResolveStandardQuery(DatiCondivisi d, MessaggioUdp request,ResourceRecord daElaborare)
        {
            if(daElaborare.name[daElaborare.name.Length-1]=='.')
                daElaborare.name = daElaborare.name.Substring(0, daElaborare.name.Length - 1);
            if (d.Cache.ContainsKey(daElaborare.name))
            {
                return new Risposta() { tipo = TypeRisposta.Risposta, risposta = d.Cache[daElaborare.name], AA = false };
            }
            else if (d.A.ContainsKey(daElaborare.name))
            {
                return new Risposta() { tipo = TypeRisposta.Risposta, risposta = d.A[daElaborare.name], AA = true };
            }
            else
            {
                return ResolveLivelliDominio(d, request, 3, daElaborare);
            }
            return new Risposta() { tipo = TypeRisposta.Niente };
        }
        public static Risposta ResolveInverseQuery(DatiCondivisi d, MessaggioUdp request)
        {
            if(d.PTR.ContainsKey(request.messaggio.query[0].name))
            {
                return new Risposta() { tipo = TypeRisposta.Risposta, risposta = d.PTR[request.messaggio.query[0].name], AA = false };
            }
            return new Risposta() { tipo = TypeRisposta.Niente };
        }
        private static Risposta ResolveLivelliDominio(DatiCondivisi d, MessaggioUdp request, int livello, ResourceRecord daElaborare)
        {
            string[] split = daElaborare.name.Split('.');
            string name = "";
            if (livello >= 0)
            {
                for (int i = 0; i < livello; i++)
                    name = split[split.Length - 1 - i] + "." + name;
                if(livello>0)
                    name = name.Substring(0, name.Length - 1);
                if (name == "")
                    name = ".";
                if (d.NS.ContainsKey(name))
                {
                    if (livello == 3)
                    {
                        return new Risposta() { tipo = TypeRisposta.Risposta, risposta = d.A[((NS)d.NS[name].RData).Dominio], AA = true };
                        /* request.messaggio.risposte.Add(d.A[((NS)d.NS[name].RData).Dominio]);
                        request.messaggio.ANcount++;*/
                    }
                    else
                    {
                        return new Risposta() { tipo = TypeRisposta.Additional, risposta = d.A[((NS)d.NS[name].RData).Dominio],AA=false };
                        /*request.messaggio.additional.Add(d.A[((NS)d.NS[name].RData).Dominio]);
                        request.messaggio.ARcount++;*/
                    }
                }
                Risposta ris = ResolveLivelliDominio(d, request, livello - 1,daElaborare);
                if (ris.tipo != TypeRisposta.Niente)
                    return ris;
            }
            return new Risposta() { tipo = TypeRisposta.Niente };
        }
    }
}
