using System;
using System.Collections.Generic;
using System.Text;

namespace SchduleServer
{
    public class TaskBuss
    {
        public void InitTask()
        {
            SchduleDao schduleDao = new SchduleDao();
            var list = schduleDao.GetTaskList();

        }
    }
}
