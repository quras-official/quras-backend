using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quras.IO.Json;

namespace Quras
{
    class PayUploadTransaction : Transaction
    {
        public string UtxHash;
        public double UploadByteSize;
        public string FromAddress;
        public string AssetName;
        public double Amount;
        public double Fee;

        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);
            UtxHash = obj["dTXHash"].AsString();
            UploadByteSize = obj["uploadByteSize"].AsNumber();

            FromAddress = DatabaseModel.Default.GetFromAddress(this);
            AssetName = DatabaseModel.Default.GetAssetName(Vout[0].Asset);
            Fee = Vout[0].Fee;
            Amount = Vout[0].Value;
        }
    }
}
