using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
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

        static async Task NoUpdateSch()
        {
            try
            {
                IScheduler sched = await Global.GetScheduler();

                if (!sched.IsStarted)
                {
                    await sched.Start();
                }

                await sched.Clear();

                TaskBuss taskBuss = new TaskBuss();
                List<TaskItem> list = taskBuss.InitTask();

                foreach (TaskItem item in list)
                {
                    JobDataMap keyValues = new JobDataMap();
                    Type t = item.GetType();
                    foreach (FieldInfo fieldInfo in t.GetFields())
                    {
                        keyValues.Add(fieldInfo.Name, fieldInfo.GetValue(item));
                    }
                    IJobDetail job;
                    if (item.taskType == "0")
                    {
                        job = JobBuilder.Create<TaskDBJob>()
                        .WithIdentity(item.taskCode, "BussDB")
                        .SetJobData(keyValues)
                        .Build();
                    }
                    else
                    {
                        job = JobBuilder.Create<TaskLoopJob>()
                        .WithIdentity(item.taskCode, "BussLoop")
                        .SetJobData(keyValues)
                        .Build();
                    }
                    if (item.triggerType == "0")
                    {
                        ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(item.taskCode + "Trigger", "BussTrig")
                        .StartNow()
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(item.interval)
                            .RepeatForever())
                        .Build();
                        await sched.ScheduleJob(job, trigger);
                    }
                    else
                    {
                        ICronTrigger cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                        .WithCronSchedule(item.cronExpression)
                        .Build();
                        await sched.ScheduleJob(job, cronTrigger);
                    }
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "已启动任务：" + item.taskCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + ex.StackTrace);
            }

        }
    }
}
