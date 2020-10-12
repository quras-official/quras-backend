using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quras.IO.Json;

namespace Quras.Core
{
    class PayFileTransaction : Transaction
    {
        public string DtxHash;
        public string FromAddress;
        public string AssetName;
        public double Amount;
        public double Fee;
        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);

            DtxHash = obj["dTXHash"].AsString();
            FromAddress = DatabaseModel.Default.GetFromAddress(this);
            AssetName = DatabaseModel.Default.GetAssetName(Vout[0].Asset);
            Fee = Vout[0].Fee;
            Amount = Vout[0].Value;

        }
    }
}
