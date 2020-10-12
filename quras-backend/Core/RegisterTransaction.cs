using Quras.IO.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras.Core
{
    class AssetInfo
    {
        public string Type;
        public string Name;
        public int Amount;
        public int Precision;
        public string Owner;
        public string Admin;
        public double TFeeMin;
        public double TFeeMax;
        public double AFee;

        public void fromJson(JObject obj)
        {
            Type = obj["type"].AsString();
            Name = ((JArray)obj["name"])[0]["name"].AsString();
            Amount = (int) obj["amount"].AsNumber();
            Precision = (int) obj["precision"].AsNumber();
            Owner = obj["owner"].AsString();
            Admin = obj["admin"].AsString();
            TFeeMin = obj["T_fee_min"].AsNumber();
            TFeeMax = obj["T_fee_max"].AsNumber();
            AFee = obj["A_fee"].AsNumber();
        }

        public AssetInfo()
        {

        }

        public AssetInfo(string type, string name, int amount, int precision, string owner, string admin, double tfeemin, double tfeemax, double afee)
        {
            Type = type;
            Name = name;
            Amount = amount;
            Precision = precision;
            Owner = owner;
            Admin = admin;
            TFeeMin = tfeemin;
            TFeeMax = tfeemax;
            AFee = afee;
        }
    }

    class RegisterTransaction : Transaction
    {
        public AssetInfo Asset = new AssetInfo();

        public override void FromJson(JObject obj)
        {
            base.FromJson(obj);
            Asset.fromJson(obj["asset"]);
        }
    }
}
