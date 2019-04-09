using Com.ACBC.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SchduleServer
{
    public class SchduleDao
    {
        public List<TaskItem> GetTaskList()
        {
            List<TaskItem> list = new List<TaskItem>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(SchduleSqls.SELECT_TASK_LIST);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperation.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    TaskItem taskItem = new TaskItem
                    {
                        taskDb = dr["TASK_DB"].ToString(),
                        taskKey = dr["TASK_KEY"].ToString(),
                        taskCode = dr["TASK_CODE"].ToString(),
                        taskStateFrom = dr["TASK_STATE_FROM"].ToString(),
                        taskStateTo = dr["TASK_STATE_TO"].ToString(),
                        taskTable = dr["TASK_TABLE"].ToString(),
                        ifInterval = dr["IF_INTERVAL"].ToString(),
                        interval = dr["INTERVAL"].ToString(),
                        remark = dr["REMARK"].ToString(),
                    };
                    list.Add(taskItem);
                }
            }

            return list;
        }
    }

    public class SchduleSqls
    {
        public const string SELECT_TASK_LIST = ""
            + "SELECT * FROM T_BUSS_TASK WHERE IF_USE = 1";
    }
}
