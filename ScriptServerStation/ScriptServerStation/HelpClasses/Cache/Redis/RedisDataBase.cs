using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ScriptServerStation.HelpClasses.Cache.Redis
{
    /// <summary>
    /// redis缓存实现类
    /// </summary>
    public class RedisDataBase : ICacheOption
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="redisCacheOptions"></param>
        public RedisDataBase(ExRedisCacheOptions redisCacheOptions)
        {
            this.redisCacheOptions = redisCacheOptions;
            this.connection = ConnectionMultiplexer.Connect(redisCacheOptions.Configuration);
            this.cache = connection.GetDatabase(redisCacheOptions.DataBaseIdx);
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
            => cache.KeyExistsAsync(key).Result;
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(cache.StringGetAsync(key).Result);
        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key) => cache.KeyDeleteAsync(key).Result;
        /// <summary>
        /// 设置存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void SetValue(string key, object obj)
            => cache.StringSetAsync(key, Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        /// <summary>
        /// 设置存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="timeOffset"></param>
        public void SetValue(string key, object obj, DateTimeOffset timeOffset)
            => cache.StringSetAsync(key, Newtonsoft.Json.JsonConvert.SerializeObject(obj), timeOffset - DateTime.Now);
        /// <summary>
        /// 连接redis类
        /// </summary>
        private ConnectionMultiplexer connection;
        /// <summary>
        /// 存放缓存的数据库
        /// </summary>
        private IDatabase cache;
        /// <summary>
        /// 缓存参数
        /// </summary>
        private ExRedisCacheOptions redisCacheOptions;
    }
}
