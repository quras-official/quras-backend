using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quras.IO.Json;

namespace Quras.Core
{
    class DownloadRequestTransaction : Transaction
    {
        public string FileName;
        public string FileDescription;
        public string FileUrl;
        public double PayAmount;
        public string UploadHash;
        public string DownloadHash;
        public string UtxHash;
        public string FileVerifiers;
        public string RequestPK;

        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);

            FileName = obj["fileName"].AsString();
            FileDescription = obj["fileDescription"].AsString();
            FileUrl = obj["fileUrl"].AsString();
            PayAmount = obj["payAmount"].AsNumber();
            UploadHash = obj["uploadHash"].AsString();
            DownloadHash = obj["downloadHash"].AsString();
            UtxHash = obj["txId"].AsString();
            FileVerifiers = obj["fileVerifiers"].ToString();
            RequestPK = obj["requestPK"].AsString();

        }
    }
}
