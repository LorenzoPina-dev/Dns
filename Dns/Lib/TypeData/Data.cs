using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.TypeData
{
    public enum TypeDatas
    {
        A, AAAA, CNAME, MX, NS, PTR, TXT, SOA,NIENTE
    }
    public class Data
    {
        public Data() { }
        public virtual void Carica(int indice,string[] csv)
        {

        }
        public virtual string ToCsv()
        {
            return "";
        }
    }
}
