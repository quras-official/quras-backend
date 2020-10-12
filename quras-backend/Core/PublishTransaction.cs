using Quras.IO.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras.Core
{
    class ContractCode
    {
        public string Hash;
        public string Script;
        public string Parameters;
        public string Returntype;

        public void fromJson(JObject obj)
        {
            Hash = obj["hash"].AsString();
            Script = obj["script"].AsString();
            Parameters = obj["parameters"].AsString();
            Returntype = obj["returntype"].AsString();
        }

    }
    class PublishTransaction : Transaction
    {
        public ContractCode Code;
        public string NeedStorage;
        public string Name;
        public string CodeVersion;
        public string Author;
        public string Email;
        public string Description;

        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);
            Code.fromJson(obj["contract"]["code"]);
            NeedStorage = obj["contract"]["needstorage"].AsString();
            Name = obj["contract"]["name"].AsString();
            CodeVersion = obj["contract"]["version"].AsString();
            Author = obj["contract"]["author"].AsString();
            Email = obj["contract"]["email"].AsString();
            Description = obj["contract"]["description"].AsString();
        }
    }
}
