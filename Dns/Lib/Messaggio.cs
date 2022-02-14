using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class Messaggio
    {
        public int identificativo;
        public bool QR;
        public int Opcode;
        public bool AA, TC,RD,RA;
        public int Zero;
        public int RCode;
        public int QDcount, ANcount, NScount, ARcount;
        public List<ResourceRecord> query,risposte,autority,additional;

        public Messaggio()
        {
            query = new List<ResourceRecord>();
            risposte = new List<ResourceRecord>();
            autority = new List<ResourceRecord>();
            additional = new List<ResourceRecord>();
        }
        public Messaggio(string csv)
        {
            query = new List<ResourceRecord>();
            risposte = new List<ResourceRecord>();
            autority = new List<ResourceRecord>();
            additional = new List<ResourceRecord>();
            string[] split = csv.Split(';');
            int indice = 0;
            identificativo = int.Parse(split[indice++]);
            QR = bool.Parse(split[indice++]);
            Opcode = int.Parse(split[indice++]);
            AA = bool.Parse(split[indice++]);
            TC = bool.Parse(split[indice++]);
            RD = bool.Parse(split[indice++]);
            RA = bool.Parse(split[indice++]);
            Zero = int.Parse(split[indice++]);
            RCode = int.Parse(split[indice++]);
            QDcount = int.Parse(split[indice++]);
            ANcount = int.Parse(split[indice++]);
            NScount = int.Parse(split[indice++]);
            ARcount = int.Parse(split[indice++]);
            for (int i = 0; i < QDcount; i++)
                query.Add(new ResourceRecord(ref indice,split));
            for (int i = 0; i < ANcount; i++)
                risposte.Add(new ResourceRecord(ref indice, split));
            for (int i = 0; i < NScount; i++)
                autority.Add(new ResourceRecord(ref indice, split));
            for (int i = 0; i < ARcount; i++)
                additional.Add(new ResourceRecord(ref indice, split));
        }
        public string toCsv()
        {
            string ris= identificativo + ";" + QR + ";" + Opcode + ";" + AA + ";" + TC + ";" + RD + ";" + RA + ";" + Zero + ";" + RCode + ";" + QDcount + ";" + ANcount + ";" + NScount + ";" + ARcount + ";";
            foreach(ResourceRecord rc in query)
                ris+=rc.ToCsv()+";";
            foreach (ResourceRecord rc in risposte)
                ris += rc.ToCsv() + ";";
            foreach (ResourceRecord rc in autority)
                ris += rc.ToCsv() + ";";
            foreach (ResourceRecord rc in additional)
                ris += rc.ToCsv() + ";";
            return ris;
        }
    }
}
