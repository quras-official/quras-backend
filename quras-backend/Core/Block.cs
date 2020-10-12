using Quras.IO.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras.Core
{
    class Block
    {
        public string Hash;
        public int Size;
        public int Version;
        public string PrevBlockHash;
        public string MerkleRoot;
        public long Time;
        public int BlockNumber;
        public long Nonce;
        public string CurrentConsensus;
        public string NextConsensus;
        public string Script;
        public int TxCount;
        public List<string> Txs = new List<string>();

        public void FromJson(JObject obj)
        {
            Hash = obj["hash"].AsString();
            Size = int.Parse(obj["size"].AsString());
            Version = int.Parse(obj["version"].AsString());
            PrevBlockHash = obj["previousblockhash"].AsString();
            MerkleRoot = obj["merkleroot"].AsString();
            Time = long.Parse(obj["time"].AsString());
            BlockNumber = int.Parse(obj["index"].AsString());
            Nonce = long.Parse(obj["nonce"].AsString(), System.Globalization.NumberStyles.HexNumber);
            CurrentConsensus = obj["currentconsensus"].AsString();
            NextConsensus = obj["nextconsensus"].AsString();
            Script = obj["script"].ToString();

            JArray jTxs = (JArray)obj["tx"];
            for(int i = 0; i < jTxs.Count; i ++)
            {
                Txs.Add(jTxs[i].ToString());
            }

            TxCount = Txs.Count;
        }
    }
}
