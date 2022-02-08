using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.TypeData
{
    public class A:Data
    {
        public string Ip;
        public A() { }
        public override void Carica(int indice,string[] csv)
        {
            Ip = csv[indice++];
        }
        public override string ToCsv()
        {
            return Ip;
        }
    }
}
