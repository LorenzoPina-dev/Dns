using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.TypeData
{
    public class SOA:Data
    {
        public string Name, Mail;
        public int MinTTL,RefreshTime, RetryTime, ExpiryTime;
        public SOA() { }
        public override void Carica(int indice, string[] csv)
        {
            Name = csv[indice++];
            Mail = csv[indice++];
            MinTTL = int.Parse(csv[indice++]);
            RefreshTime = int.Parse(csv[indice++]);
            RetryTime = int.Parse(csv[indice++]);
            ExpiryTime = int.Parse(csv[indice++]);
        }
        public override string ToCsv()
        {
            return Name + ";" + Mail+";"+ MinTTL + ";"+RefreshTime+";"+ RetryTime+";"+ExpiryTime;
        }
    }
}
