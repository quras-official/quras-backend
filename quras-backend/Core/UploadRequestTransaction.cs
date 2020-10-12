using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quras.IO.Json;

namespace Quras.Core
{
    class UploadRequestTransaction : Transaction
    {
        public string FileName;
        public string FileDescription;
        public string FileUrl;
        public double PayAmount;
        public string UploadHash;
        public string FileVerifiers;
        public int DurationDays;

        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);

            FileName = obj["fileName"].AsString();
            FileDescription = obj["fileDescription"].AsString();
            FileUrl = obj["fileUrl"].AsString();
            PayAmount = obj["payAmount"].AsNumber();
            UploadHash = obj["uploadHash"].AsString();
            FileVerifiers = obj["fileVerifiers"].ToString();
            DurationDays = int.Parse(obj["durationDays"].AsString());
        }
    }
}
