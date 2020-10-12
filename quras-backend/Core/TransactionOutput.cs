using Quras.IO.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras
{
    class TransactionOutput
    {
        public int Index;
        public string Asset;
        public double Value;
        public string Address;
        public double Fee;
        public void FromJson(JObject obj)
        {
            Index = int.Parse(obj["n"].AsString());
            Asset = obj["asset"].AsString();
            Value = obj["value"].AsNumber();
            Address = obj["address"].AsString();
            Fee = obj["fee"].AsNumber();
        }

        public static TransactionOutput parse(JObject obj)
        {
            TransactionOutput txOut = new TransactionOutput();
            txOut.FromJson(obj);
            return txOut;
        }

        public static TransactionOutput[] parse(JArray obj)
        {
            List<TransactionOutput> txOuts = new List<TransactionOutput>();
            for(int i = 0; i < obj.Count; i ++)
            {
                txOuts.Add(TransactionOutput.parse(obj[i]));
            }
            return txOuts.ToArray();
        }
    }
}
