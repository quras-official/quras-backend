using Quras.IO.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras
{
    class Transaction : IEquatable<Transaction>
    {
        public string Txid;
        public int Size;
        public string Type;
        public int Version;
        public string Attributes;
        public TransactionInput[] Vin;
        public string JVin;
        public TransactionOutput[] Vout;
        public string JVout;
        public double SysFee;
        public double NetFee;
        public string Scripts;
        public long Nonce;

        public bool Equals(Transaction other)
        {
            throw new NotImplementedException();
        }

        public virtual void FromJson(JObject obj)
        {
            Txid = obj["txid"].AsString();
            Size = int.Parse(obj["size"].AsString());
            Type = obj["type"].AsString();
            Version = int.Parse(obj["version"].AsString());
            Attributes = obj["attributes"].ToString();
            Vin = TransactionInput.parse((JArray)obj["vin"]);
            JVin = obj["vin"].ToString();
            Vout = TransactionOutput.parse((JArray)obj["vout"]);
            JVout = obj["vout"].ToString();
            SysFee = obj["sys_fee"].AsNumber();
            NetFee = obj["net_fee"].AsNumber();
            Scripts = obj["scripts"].ToString();
            if (obj["nonce"] != null)
                Nonce = long.Parse(obj["nonce"].AsString());
            else
                Nonce = 0;
        }
    }
}
