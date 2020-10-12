using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras
{
    internal sealed partial class Constants
    {
        public const int FAST_DELAY = 100;
        public const int NORMAL_DELAY = 1000 * 5;

        public static Constants instance;

        public string RPC_URL = "";
        public string Database_IP = "";
        public int Database_PORT = 0;
        public string Database_USER = "";
        public string Database_PASSWORD = "";
        public string Database_NAME = "";

        public string NAME_GOVERNMENT_TOKEN = "XQC";
        public string NAME_UTILITY_TOKEN = "XQG";

        public string UTXOSTATUS_SPENT = "spent";
        public string UTXOSTATUS_UNSPENT = "unspent";

        public byte AddressVersion = 31;

        public enum STATE_TYPE
        {
            UPLOAD_STATES = 0,
            DOWNLOAD_STATES = 1,
            STORAGE_STATES = 2,
            NOTIFICATION_STATES = 3,
            PERMISSION_STATES = 4,
        }

        public enum UPLOAD_STATES
        {
            UPLOAD_COMPLETED = 0,
            UPLOAD_FAILED = 1,
            UPLOAD_PERCENTAGE = 2,
            UPLOAD_CANCEL = 3,

            UPLOAD_START = 4,
            UPLOAD_START_REPLY = 5
        }

        public enum DOWNLOAD_STATES
        {
            DOWNLOAD_COMPLETED = 0,
            DOWNLOAD_FAILED = 1,
            DOWNLOAD_PERCENTAGE = 2,
            DOWNLOAD_CANCEL = 3,

            DOWNLOAD_START = 4,
            DOWNLOAD_START_REPLY = 5
        }

        public enum STORAGE_STATES
        {
            STORAGE_CONTRACT_STARTED = 0,
            STORAGE_COMTRACT_ENDED = 1,
            STORAGE_CAPACITY = 2,
            STORAGE_OVERFLOW = 3,
        }

        public enum NOTIFCATION_STATES
        {
            NOTIFICATION_MESSAGE = 0,
            NOTIFICATION_URL = 1,
        }

        public enum PERMISSION_STATES
        {
            PERMISSION_APPROVER_ON = 0,
            PERMISSION_APPROVER_OFF = 1,
            PERMISSION_PRIVACY_SC_ON = 2,
            PERMISSION_PRIVACY_SC_OFF = 3
        }

        public static Constants Default
        {
            get
            {
                if (instance == null)
                    instance = new Constants();

                return instance;
            }
        }

        public Constants()
        {
            string filepath = "config.json";
            string result = string.Empty;
            using (StreamReader r = new StreamReader(filepath))
            {
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);

                RPC_URL = jobj.GetValue("UriPrefix").ToString();

                try
                {
                    Database_IP = jobj.GetValue("DbHost").ToString();
                    Database_PORT = int.Parse(jobj.GetValue("DbPort").ToString());
                    Database_USER = jobj.GetValue("DbUser").ToString();
                    Database_PASSWORD = jobj.GetValue("DbPassword").ToString();
                    Database_NAME = jobj.GetValue("DbName").ToString();
                }
                catch (Exception ex)
                {
                    LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                }
            }
        }
    }
}
