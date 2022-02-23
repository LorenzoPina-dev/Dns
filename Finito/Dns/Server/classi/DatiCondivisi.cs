using Lib;
using Lib.TypeData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.classi
{
    public class DatiCondivisi
    {
        public bool possoIterativa;
        public int porta;
        public Dictionary<string, int> ip;
        public Dictionary<string, ResourceRecord> A, NS, Cache;
        public DatiCondivisi()
        {
            A = new Dictionary<string, ResourceRecord>();
            NS = new Dictionary<string, ResourceRecord>();
            Cache = new Dictionary<string, ResourceRecord>();
            ip = new Dictionary<string, int>();
            StreamReader sr = new StreamReader("./Server/mapIp.txt");
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] split = line.Split('=');
                ip.Add(split[0], int.Parse(split[1]));
            }
            sr.Close();
        }
        public void Upload(string file)
        {
            StreamReader sr = new StreamReader(file);
            string line = "";
            if (int.Parse(sr.ReadLine()) == 1)
                possoIterativa = true;
            else
                possoIterativa = false;
            porta = int.Parse(sr.ReadLine());
            while((line=sr.ReadLine())!=null)
            {
                string[] split = line.Split(';');
                int indice = 0;
                ResourceRecord record = new ResourceRecord(ref indice, split);
                if(record.Type==TypeDatas.A)
                    A.Add(record.name,record);
                else if (record.Type == TypeDatas.NS)
                    NS.Add(record.name, record);
            }
        }
    }
}
