using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quras.IO.Json;

namespace Quras.Core
{
    class ApproveDownloadTransaction : Transaction
    {
        public string ApproveHash;
        public string DownloadHash;
        public string DtxHash;
        public int ApproveState;

        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);
            ApproveHash = obj["approveHash"].AsString();
            DownloadHash = obj["downloadHash"].AsString();
            DtxHash = obj["dTXHash"].AsString();
            ApproveState = int.Parse(obj["approveState"].AsString());
        }
    }
}
