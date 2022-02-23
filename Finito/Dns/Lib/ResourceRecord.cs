using Lib.TypeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class ResourceRecord
    {
        public string name;
        public TypeDatas Type;
        public string Class="IN";
        public int TTL, RDLength;
        public Data RData;
        public ResourceRecord()
        {

        }
        public ResourceRecord(ref int indice,string[] csv)
        {
            this.name = csv[indice++];
            this.Type = (TypeDatas)int.Parse(csv[indice++]);
            this.Class =csv[indice++];
            this.TTL = int.Parse(csv[indice++]);
            indice++;

            if (Type == TypeDatas.A)
                this.RData = new A();
            else if (Type == TypeDatas.NS)
                this.RData = new NS();
            else if (Type == TypeDatas.SOA)
                this.RData = new SOA();
            else
                this.RData = new Data();
            RData.Carica(ref indice, csv);
            this.RDLength = RData.ToCsv().Length;

        }
        public string ToCsv()
        {
            string data = RData.ToCsv();
            int lung = data.Length;
            if (lung > 0 && Type == TypeDatas.A)
                lung = 4;
            return name + ";" + (int)Type + ";" + Class + ";" + TTL + ";" + lung + ";" +data;
        }

    }
}
