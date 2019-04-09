using Com.ACBC.Framework.Database;
using System;

namespace SchduleServer
{
    public class DBManager : IType
    {
        private DBType dbt;
        private string str = "";

        public DBManager()
        {
            this.str = "Server=" + Global.DBUrl
                     + ";Port=" + Global.DBPort
                     + ";Database=core;Uid=" + Global.DBUser
                     + ";Pwd=" + Global.DBPassword
                     + ";CharSet=utf8mb4; SslMode =none;";
            Console.Write(this.str);
            this.dbt = DBType.Mysql;
        }

        public DBManager(DBType d, string s)
        {
            this.dbt = d;
            this.str = s;
        }

        public DBType getDBType()
        {
            return dbt;
        }

        public string getConnString()
        {
            return str;
        }

        public void setConnString(string s)
        {
            this.str = s;
        }
    }
}
