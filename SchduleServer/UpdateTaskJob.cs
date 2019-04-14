﻿using Quartz;
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
            IScheduler sched = await Global.GetScheduler();

            if(!sched.IsStarted)
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
                foreach(FieldInfo fieldInfo in t.GetFields())
                {
                    keyValues.Add(fieldInfo.Name, fieldInfo.GetValue(item));
                }
                IJobDetail job = JobBuilder.Create<TaskJob>()
                .WithIdentity(item.taskCode, "Buss")
                .SetJobData(keyValues)
                .Build();
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(item.taskCode + "Trigger", "Buss")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(item.interval)
                        .RepeatForever())
                .Build();
                await sched.ScheduleJob(job, trigger);
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "已启动任务：" + item.taskCode);
            }
        }
    }
}
