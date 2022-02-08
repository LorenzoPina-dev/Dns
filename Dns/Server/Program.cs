using Lib;
using Lib.TypeData;
using Server.classi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        public enum ServerType
        {
            Privato,Default,Root,Authoritative
        }
        static void Main(string[] args)
        {
            DatiCondivisi d = new DatiCondivisi();
            d.Upload("./Server/root.csv");
             GestioneUdp g = new GestioneUdp(5000,d);
            
            /*int porta;
            Type*/
            /*StreamWriter sw = new StreamWriter("./Server/dns_alunni_com.csv");
            string file = "";
            //vaalunni.it
            ResourceRecord r = new ResourceRecord();
            r.name = "comp3.alunni.com";
            r.Type = TypeDatas.A;
            r.TTL = 86400;
            A a = new A();
            a.Ip = "192.168.2.7";
            r.RData = a;
            file += r.ToCsv() + "\r\n";
            
            r = new ResourceRecord();
            r.name = "comp4.alunni.com";
            r.Type = TypeDatas.A;
            r.TTL = 86400;
            a = new A();
            a.Ip = "192.168.2.8";
            r.RData = a;
            file += r.ToCsv() + "\r\n";

            sw.Write(file);
            sw.Flush();
            sw.Close();*/
        }
    }
}
