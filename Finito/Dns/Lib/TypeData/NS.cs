using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.TypeData
{
    public class NS:Data
    {
        public string  Dominio;
        public NS() { }
        public override void Carica(ref int indice,string[] csv)
        {
            Dominio = csv[indice++];
        }
        public override string ToCsv()
        {
            return Dominio;
        }
    }
}
