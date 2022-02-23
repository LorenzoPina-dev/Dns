using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Udp
{
    public class MessaggioUdp
    {
        public string ip;
        public int porta;
        public Messaggio messaggio;
        public MessaggioUdp() { }
        public string toCsv()
        {
            return ip + ";" + porta + ";" + messaggio.toCsv();
        }
    }
}
