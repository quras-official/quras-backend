using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quras.IO.Json;

namespace Quras.Core
{
    class RegisterMultiSignTransaction : Transaction
    {
        public string MultiSigAddressScript = "";
        public string MultiSigAddress = "";
        public string JAddresses = "";
        
        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);

            MultiSigAddressScript = obj["MultiSigRedeemScript"].AsString();
            MultiSigAddress = CommonUtils.ToAddress(MultiSigAddressScript);
            JAddresses = obj["ValidatorList"].ToString();
        }
    }
}
