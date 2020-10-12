using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quras.IO.Json;

namespace Quras.Core
{
    class AnonymousContractTransaction :Transaction
    {
        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);
        }
    }
}
