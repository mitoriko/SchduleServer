using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace SchduleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Global.Startup();
            StartAsync().GetAwaiter().GetResult();
            Console.ReadLine();
        }


        static async Task StartAsync()
        {
            NameValueCollection pros = new NameValueCollection();
            pros.Add("quartz.scheduler.instanceName", "System");
            pros.Add("quartz.threadPool.threadCount", "10");
            StdSchedulerFactory sf = new StdSchedulerFactory(pros);
            IScheduler sched = await sf.GetScheduler();

            await sched.Start();

            IJobDetail job = JobBuilder.Create<UpdateTaskJob>()
                .WithIdentity("UpdateTask", "System")
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("UpdateTaskTrigger", "System")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(Global.Interval)
                    .RepeatForever())
            .Build();

            await sched.ScheduleJob(job, trigger);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "已启动更新任务列表计划");
        }
    }
}
