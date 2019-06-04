using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchduleServer
{
    public class TaskLoopJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap data = context.JobDetail.JobDataMap;
            var redis = RedisManager.getRedisConn();
            var db = redis.GetDatabase(Global.REDIS_DB);
            await Global.Topic(data["taskCode"].ToString());
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "任务(" + data["taskCode"] + ")部署了1个Job");
        }
    }
}
