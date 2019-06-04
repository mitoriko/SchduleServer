using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchduleServer
{
    public class TaskDBJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap data = context.JobDetail.JobDataMap;

            TaskBuss taskBuss = new TaskBuss();
            var list = taskBuss.GetJobsIds(
                data["taskDb"].ToString(),
                data["taskTable"].ToString(),
                data["taskKey"].ToString(),
                data["taskState"].ToString(),
                data["taskStateFrom"].ToString(),
                data["taskStateTo"].ToString()
                );

            var redis = RedisManager.getRedisConn();
            var db = redis.GetDatabase(Global.REDIS_DB);

            foreach(string id in list)
            {
                await db.ListLeftPushAsync(Global.TASK_PREFIX + "." + data["taskCode"], id);
            }

            if(list.Count > 0)
            {
                await Global.Topic(data["taskCode"].ToString());
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "任务(" + data["taskCode"] + ")部署了" + list.Count + "个Job");
            }
        }
    }
}
