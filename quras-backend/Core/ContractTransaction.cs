using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quras.IO.Json;

namespace Quras.Core
{
    class ContractTransaction : Transaction
    {
        public string FromAddress = "";
        public string AssetName = "";
        public double Fee = 0.0f;
        public double Amount = 0.0f;
        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);

            FromAddress = DatabaseModel.Default.GetFromAddress(this);
            AssetName = DatabaseModel.Default.GetAssetName(Vout[0].Asset);
            Fee = Vout[0].Fee;
            Amount = Vout[0].Value;
        }
    }
}
