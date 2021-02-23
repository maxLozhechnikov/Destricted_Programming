using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DBVAL
{
    public class RedisStorage : IStorage
    {
        private readonly string Host = "localhost:6379";
        private readonly string TextSetKey = "textSetKey";
        private readonly ILogger<RedisStorage> _logger;

        public RedisStorage(ILogger<RedisStorage> logger) => this._logger = logger;

        public string Get(string key)
        {
            var db = this.GetDB();
            return db.StringGet(key);
        }

        public bool HasTextDuplicate(string text)
        {
            var db = GetDB();
            return db.SetContains(TextSetKey, text);
        }

        public void Put(string key, string value)
        {
            var db = this.GetDB();
            if (key.StartsWith("TEXT-"))
            {
                db.SetAdd(TextSetKey, value);
            }
            db.StringSet(key, value);
        }

        private IDatabase GetDB()
        {
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(Host);
            return connectionMultiplexer.GetDatabase();
        }

        private ConnectionMultiplexer GetConnection()
        {
            return ConnectionMultiplexer.Connect(Host);
        }
    }
}