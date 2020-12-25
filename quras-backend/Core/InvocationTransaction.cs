using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quras.IO.Json;

namespace Quras
{
    class InvocationTransaction : Transaction
    {
        public string Script;
        public string Gas;
        public string FromAddress;

        public AssetInfo Asset = null;
        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);
            Script = obj["script"].AsString();
            Gas = obj["gas"].AsString();

            FromAddress = DatabaseModel.Default.GetFromAddress(this);

            ParseAsset();
        }

        public void ParseAsset()
        {
            byte[] byScript = CommonUtils.HexStringToByte(Script);

            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(byScript);

                string feeAddress = InvocationUtils.GetUINT160(ms);
                if (feeAddress == "") throw new Exception();

                double tFeeMax = InvocationUtils.GetFixed8(ms);
                if (tFeeMax < 0) throw new Exception();

                double tFeeMin = InvocationUtils.GetFixed8(ms);
                if (tFeeMin < 0) throw new Exception();

                double tFee = InvocationUtils.GetFixed8(ms);
                if (tFee < 0) throw new Exception();

                double aFee = InvocationUtils.GetFixed8(ms);
                if (aFee < 0) throw new Exception();

                string issure = InvocationUtils.GetUINT160(ms);
                if (issure == "") throw new Exception();

                string admin = InvocationUtils.GetUINT160(ms);
                if (admin == "") throw new Exception();

                if (!InvocationUtils.SkipECPoint(ms)) throw new Exception();

                int precision = InvocationUtils.GetByte(ms);
                if (precision < 0) throw new Exception();

                int amount = (int) InvocationUtils.GetFixed8(ms);
                if (amount <= 0) throw new Exception();

                string jname = InvocationUtils.GetString(ms);
                string name = ((JArray)JObject.Parse(jname))[0]["name"].AsString();
                if (name == "") throw new Exception();
                

                int assetType = InvocationUtils.GetByte(ms);
                string strAssetType = InvocationUtils.GetAssetType(assetType);
                if (strAssetType == "") throw new Exception();

                string syscall = InvocationUtils.GetSysCall(ms);
                if (syscall != "Quras.Asset.Create") throw new Exception();

                Asset = new AssetInfo(strAssetType, name, amount, precision, issure, issure, tFeeMin, tFeeMax, aFee);

                ms.Close();
            }
            catch (Exception ex)
            {
                if (ms != null)
                {
                    ms.Close();
                }

                Asset = null;
            }
        }
    }
}
