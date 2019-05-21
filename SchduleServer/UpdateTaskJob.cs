using Quartz;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SchduleServer
{
    public class UpdateTaskJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                IScheduler sched = await Global.GetScheduler();

                if (sched.IsStarted)
                {
                    await sched.Shutdown();
                    await sched.Clear();
                }

                if (!sched.IsStarted)
                {
                    await sched.Start();
                }
                
                

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
            catch(Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + ex.StackTrace);
            }
            
        }
    }
}
