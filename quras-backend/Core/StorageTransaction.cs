using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quras.IO.Json;

namespace Quras.Core
{
    class StorageTransaction :Transaction
    {
        public string OwnerHash;
        public double StorageSize;
        public double GuaranteeAmount;
        public double PayAmount;
        public int EndtimeStamp;

        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);
            OwnerHash = obj["ownerHash"].AsString();
            StorageSize = obj["storageSize"].AsNumber();
            GuaranteeAmount = obj["guaranteeAmount"].AsNumber();
            PayAmount = obj["payAmount"].AsNumber();
            EndtimeStamp = int.Parse(obj["endtimeStamp"].AsString());
        }
    }
}
