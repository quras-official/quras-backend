using Quras.IO.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras.Core
{
    class ClaimTransaction : Transaction
    {
        public string JClaims;
        public TransactionInput[] Claims;

        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);
            JClaims = obj["claims"].ToString();
            Claims = TransactionInput.parse((JArray)obj["claims"]);
        }
    }
}
