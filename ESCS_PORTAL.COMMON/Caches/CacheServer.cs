using ESCS_PORTAL.COMMON.Caches.interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESCS_PORTAL.COMMON.Caches
{
    public class CacheServer : ICacheServer
    {
        public TimeSpan _defaultExpiry;
        public string _serverName;
        public int _dataBase;
        private RedisServer _redisServer;

        public T Get<T>(string serverName, string endpoint, string key, int database = 0)
        {
            try
            {
                _redisServer = new RedisServer(serverName, endpoint);
                var value = _redisServer.GetDatabase(database).StringGet(key);
                if (value == RedisValue.Null)
                {
                    return default(T);
                }
                if (typeof(T).Equals(typeof(string)))
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                return JsonConvert.DeserializeObject<T>(value.ToString());
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        public bool Set<T>(string serverName, string endpoint, String key, T item, TimeSpan? expiry = null, int database = 0)
        {
            try
            {
                _redisServer = new RedisServer(serverName, endpoint);
                if (typeof(T).Equals(typeof(string)))
                {
                    _redisServer.GetDatabase(database).StringSet(key, item?.ToString(), expiry);
                }
                else
                {
                    _redisServer.GetDatabase(database).StringSet(key, JsonConvert.SerializeObject(item), expiry);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Remove(string serverName, string endpoint, string key, int database = 0)
        {
            _redisServer = new RedisServer(serverName, endpoint);
            return _redisServer.GetDatabase(database).KeyDelete(key);
        }

        public bool Expired(string serverName, string endpoint, string key, TimeSpan expiry, int database = 0)
        {
            _redisServer = new RedisServer(serverName, endpoint);
            return _redisServer.GetDatabase(database).KeyExpire(key, expiry);
        }

        public RedisValueWithExpiry StringGetWithExpiry(string serverName, string endpoint, string key, int database = 0)
        {
            _redisServer = new RedisServer(serverName, endpoint);
            var res = _redisServer.GetDatabase(database).StringGetWithExpiry(key, CommandFlags.None);
            return res;
        }

        public double GetExpiryTimeWithConfigDB(string key)
        {
            _redisServer = new RedisServer(RedisCacheEnvironment.ServerName, RedisCacheEnvironment.Endpoint);
            var res = _redisServer.GetDatabase(RedisCacheEnvironment.Database).StringGetWithExpiry(key, CommandFlags.None);
            double expireTime = res.Expiry.Value.TotalMinutes;
            return expireTime;
        }

        public bool SetWithConfigDB<T>(string key, T item, TimeSpan? expiry = null)
        {
            try
            {
                _redisServer = new RedisServer(RedisCacheEnvironment.ServerName, RedisCacheEnvironment.Endpoint);
                if (typeof(T).Equals(typeof(string)))
                {
                    _redisServer.GetDatabase(RedisCacheEnvironment.Database).StringSet(key, item?.ToString(), expiry);
                }
                else
                {
                    _redisServer.GetDatabase(RedisCacheEnvironment.Database).StringSet(key, JsonConvert.SerializeObject(item), expiry);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool RemoveWithConfigDB(string key)
        {
            _redisServer = new RedisServer(RedisCacheEnvironment.ServerName, RedisCacheEnvironment.Endpoint);
            return _redisServer.GetDatabase(RedisCacheEnvironment.Database).KeyDelete(key);
        }

        public T GetWithConfigDB<T>(string key)
        {
            try
            {
                _redisServer = new RedisServer(RedisCacheEnvironment.ServerName, RedisCacheEnvironment.Endpoint);
                var value = _redisServer.GetDatabase(RedisCacheEnvironment.Database).StringGet(key);
                if (value == RedisValue.Null)
                {
                    return default(T);
                }
                if (typeof(T).Equals(typeof(string)))
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                return JsonConvert.DeserializeObject<T>(value.ToString());
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
        public bool RemoveKeyCacheByPattern(string endpoint, string pattern, int database = 0)
        {
            _redisServer = new RedisServer("", endpoint);
            var keys = _redisServer.GetAllKeyServerOfDatabase(endpoint, pattern, database);
            bool delSuccess = true;
            if (keys.Length > 0)
            {
                foreach (var key in keys)
                {
                    bool check = _redisServer.GetDatabase(database).KeyDelete(key);
                    if (!check)
                    {
                        delSuccess = false;
                    }
                }
            }
            return delSuccess;
        }
        public List<string> GetKeysByPatterm(string endpoint, string pattern, int database = 0)
        {
            _redisServer = new RedisServer("", endpoint);
            var keys = _redisServer.GetAllKeyServerOfDatabase(endpoint, pattern, database);
            if (keys==null)
                return null;
            return keys.Select(n => n.ToString()).ToList();
        }
    }
}
