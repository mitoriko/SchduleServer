using System;
using System.Collections.Generic;
using System.Text;

namespace SchduleServer
{
    public class ConfigItem
    {
        public string key;
        public string value;
    }

    public class ConfigParam
    {
        public string env;
        public string group;
    }

    public class RequestParam
    {
        public string method;
        public object param;
    }

    public class ResponseObj
    {
        public bool success;
        public ResponseMsg msg;
        public List<ConfigItem> data;
    }

    public class ResponseMsg
    {
        public string code;
        public string msg;
    }

    public class TaskItem
    {
        public string taskDb;
        public string taskTable;
        public string taskKey;
        public string taskStateFrom;
        public string taskStateTo;
        public string taskCode;
        public string interval;
        public string ifInterval;
        public string remark;
    }
}
