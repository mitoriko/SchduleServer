using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchduleServer
{
    public class RedisManager
    {
        private static ConfigurationOptions connDCS;

        private static readonly object Locker = new object();

        private static ConnectionMultiplexer redisConn;

        public static ConnectionMultiplexer getRedisConn()
        {
            if (redisConn == null)
            {
                connDCS = ConfigurationOptions.Parse(Global.Redis);
                lock (Locker)
                {
                    if (redisConn == null || !redisConn.IsConnected)
                    {
                        redisConn = ConnectionMultiplexer.Connect(connDCS);
                    }
                }
            }
            return redisConn;
        }

        public static RedisChannel TaskTopic(string taskCode)
        {
            return new RedisChannel("TaskTopic." + taskCode, RedisChannel.PatternMode.Literal);
        }
    }
}
