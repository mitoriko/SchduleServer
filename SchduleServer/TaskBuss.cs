using System;
using System.Collections.Generic;
using System.Text;

namespace SchduleServer
{
    public class TaskBuss
    {
        public List<TaskItem> InitTask()
        {
            SchduleDao schduleDao = new SchduleDao();
            return schduleDao.GetTaskList();

        }

        public List<string> GetJobsIds(
            string db,
            string table,
            string key,
            string state,
            string stateFrom,
            string stateTo)
        {
            SchduleDao schduleDao = new SchduleDao();
            return schduleDao.GetJobsIds(db, table, key, state, stateFrom, stateTo);
        }
    }
}
