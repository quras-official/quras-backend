using Quras.IO.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras
{
    class TransactionInput
    {
        public string Txid;
        public int Vout;
        public void fromJson(JObject obj)
        {
            Txid = obj["txid"].AsString();
            Vout = int.Parse(obj["vout"].AsString());
        }

        public static TransactionInput[] parse(JArray obj)
        {
            List<TransactionInput> txIns = new List<TransactionInput>();
            for(int i = 0;i < obj.Count;i++)
            {
                txIns.Add(TransactionInput.parse(obj[i]));
            }
            return txIns.ToArray();
        }

        public static TransactionInput parse(JObject obj)
        {
            TransactionInput txIn = new TransactionInput();
            txIn.fromJson(obj);
            return txIn;
        }
    }
}
