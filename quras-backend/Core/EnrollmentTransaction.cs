using Quras.IO.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras.Core
{
    class EnrollmentTransaction : Transaction
    {
        public string Pubkey;

        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);
            Pubkey = obj["pubkey"].AsString();
        }
    }
}
