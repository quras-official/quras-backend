using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quras.IO.Json;

namespace Quras.Core
{
    class StateTransaction: Transaction
    {
        public string FromAddr;
        public string ToAddr;
        public string TxHash;
        public string StrStateScript;
        public byte[] StateScript;

        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);

            FromAddr = obj["fromAddr"].AsString();
            ToAddr = obj["toAddr"].AsString();
            TxHash = obj["txHash"].AsString();
            StrStateScript = obj["stateScript"].AsString();

            List<byte> scriptlst = new List<byte>();
            Console.WriteLine(StrStateScript);

            for(int j = 0; j < StrStateScript.Length; j += 2)
            {
                scriptlst.Add(byte.Parse(StrStateScript.Substring(j, 2), System.Globalization.NumberStyles.HexNumber));
            }
            StateScript = scriptlst.ToArray();
        }
    }
}
