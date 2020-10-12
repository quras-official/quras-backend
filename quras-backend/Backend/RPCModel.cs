using Quras.Core;
using Quras.IO.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Quras
{
    class RPCModel
    {
        public JObject QueryRPC(string request, JArray parameter)
        {
            JObject result = null;
            String url = Constants.Default.RPC_URL;
            string param = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 30000;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"jsonrpc\":\"2.0\"," +
                                "\"method\":\"" + request + "\"," +
                                "\"params\":" + parameter.ToString() + "," +
                                "\"id\":\"1234\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                JObject jResult = JObject.Parse(streamReader.ReadToEnd());
                result = jResult["result"];
            }
            return result;
        }
        public int GetBlockchainNumber()
        {
            int blockNumber = -1;
            try
            {
                blockNumber = int.Parse(QueryRPC("getblockcount", new JArray()).ToString());
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
            }

            return blockNumber;
        }

        public Block GetBlock(int block_num)
        {
            Block block = new Block();
            try
            {
                JArray param = new JArray();
                param.Add(block_num);
                param.Add(1); //verbose

                block.FromJson(QueryRPC("getblock", param));
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                block = null;
            }
            return block;
        }
    }
}
