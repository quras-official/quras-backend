using Quras.IO.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras
{
    class IssueTransaction : Transaction
    {
        public string FromAddress = "";
        public string AssetName = "";
        public IssueTransaction()
        {
            
        }

        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);

            FromAddress = DatabaseModel.Default.GetFromAddress(this);
            AssetName = DatabaseModel.Default.GetAssetName(Vout[0].Asset);
        }
    }
}
