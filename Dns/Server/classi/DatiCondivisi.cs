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
    class DatiCondivisi
    {
        public Dictionary<string, ResourceRecord> A, NS, Cache;
        public DatiCondivisi()
        {
            A = new Dictionary<string, ResourceRecord>();
            NS = new Dictionary<string, ResourceRecord>();
            Cache = new Dictionary<string, ResourceRecord>();
        }
        public void Upload(string file)
        {
            StreamReader sr = new StreamReader(file);
            string line = "";
            while((line=sr.ReadLine())!=null)
            {
                string[] split = line.Split(';');
                ResourceRecord record = new ResourceRecord(0, split);
                if(record.Type==TypeDatas.A)
                    A.Add(record.name,record);
                else if (record.Type == TypeDatas.NS)
                    NS.Add(record.name, record);
            }
        }
    }
}
