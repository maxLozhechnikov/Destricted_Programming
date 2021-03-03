using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Valuators;

namespace DBVAL
{
    public class RedisStorage : IStorage
    {
        private readonly string _host = "localhost:6379";
        private readonly string _textSetKey = "textSetKey";
        
        public string Get(string key)
        {
            var db = GetDB();
            return db.StringGet(key);
        }

        public bool HasTextDuplicate(string text)
        {
            var db = GetDB();
            return db.SetContains(_textSetKey, text);
        }

        public void Put(string key, string value)
        {
            var db = GetDB();
            db.StringSet(key, value);
        }

        public void PutText(string value)
        {
            var db = GetDB();
            db.SetAdd(_textSetKey, value);
        }

        private IDatabase GetDB()
        {
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(_host);
            return connectionMultiplexer.GetDatabase();
        }
    }
}