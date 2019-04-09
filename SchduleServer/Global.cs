using Com.ACBC.Framework.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchduleServer
{
    public class Global
    {
        public const string ENV = "PRO";
        public const string GROUP = "Schdule";

        public static void Startup()
        {
            GetConfig();
            if (DatabaseOperation.TYPE == null)
            {
                DatabaseOperation.TYPE = new DBManager();
            }
        }

        public static void GetConfig()
        {
            string url = "http://" + ConfigServer + "/api/config/Config/Open";
            ConfigParam configParam = new ConfigParam
            {
                env = ENV,
                group = GROUP
            };
            RequestParam requestParam = new RequestParam
            {
                method = "GetConfig",
                param = configParam
            };
            string body = JsonConvert.SerializeObject(requestParam);
            string resp = Utils.PostHttp(url, body);
            ResponseObj responseObj = JsonConvert.DeserializeObject<ResponseObj>(resp);

            foreach(ConfigItem item in responseObj.data)
            {
                Environment.SetEnvironmentVariable(item.key, item.value);
            }

        }

        public static string ConfigServer
        {
            get
            {
                return Environment.GetEnvironmentVariable("ConfigServer");
            }
        }

        public static string DBUrl
        {
            get
            {
                return Environment.GetEnvironmentVariable("DBUrl");
            }
        }

        public static string DBUser
        {
            get
            {
                return Environment.GetEnvironmentVariable("DBUser");
            }
        }

        public static string DBPort
        {
            get
            {
                return Environment.GetEnvironmentVariable("DBPort");
            }
        }

        public static string DBPassword
        {
            get
            {
                return Environment.GetEnvironmentVariable("DBPassword");
            }
        }
    }
}
