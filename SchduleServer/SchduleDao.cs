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
                        taskState = dr["TASK_STATE"].ToString(),
                        taskStateFrom = dr["TASK_STATE_FROM"].ToString(),
                        taskStateTo = dr["TASK_STATE_TO"].ToString(),
                        taskTable = dr["TASK_TABLE"].ToString(),
                        interval = Convert.ToInt32(dr["INTERVAL"]),
                        remark = dr["REMARK"].ToString(),
                        taskType = dr["TASK_TYPE"].ToString(),
                        triggerType = dr["TRIGGER_TYPE"].ToString(),
                        cronExpression = dr["CRON_EXPRESSION"].ToString(),
                    };
                    list.Add(taskItem);
                }
            }

            return list;
        }

        public List<string> GetJobsIds(
            string db, 
            string table, 
            string key, 
            string state, 
            string stateFrom,
            string stateTo)
        {
            List<string> list = new List<string>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(
                SchduleSqls.SELECT_JOB_IDS,
                db,
                table,
                key,
                state,
                stateFrom);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperation.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                string ids = "";
                foreach (DataRow dr in dt.Rows)
                {
                    string id = dr[0].ToString();
                    list.Add(id);
                    ids += id + ",";
                }
                ids = ids.Substring(0, ids.Length - 1);
                builder.Clear();
                builder.AppendFormat(
                SchduleSqls.UPDATE_JOB_IDS,
                db,
                table,
                key,
                state,
                stateTo,
                ids);
                sql = builder.ToString();
                if(!DatabaseOperation.ExecuteDML(sql))
                {
                    list = new List<string>();
                }
            }

            return list;
        }
    }

    public class SchduleSqls
    {
        public const string SELECT_TASK_LIST = ""
            + "SELECT * FROM T_BUSS_TASK WHERE IF_USE = 1";
        public const string SELECT_JOB_IDS = ""
            + "SELECT {2} FROM {0}.{1} WHERE {3} = {4} ";
        public const string UPDATE_JOB_IDS = ""
            + "UPDATE {0}.{1} SET {3} = {4} WHERE {2} IN ({5}) ";
    }
}
