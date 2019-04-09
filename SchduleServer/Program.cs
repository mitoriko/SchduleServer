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
            TaskBuss taskBuss = new TaskBuss();
            taskBuss.InitTask();
            Console.ReadLine();
        }

    }
}
