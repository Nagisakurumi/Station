using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ScriptServerStation.Utils;
using StackExchange.Redis;

namespace ScriptServerStation.HelpClasses.Cache.Redis
{
    /// <summary>
    /// redis缓存实现类
    /// </summary>
    public class RedisDataBase : IMemoryCache
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
        public bool IsContain(string key)
            => cache.KeyExistsAsync(key).Result;
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key) => ConvertValue<T>(cache.StringGetAsync(key).Result);
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
            => cache.StringSetAsync(key, GetString(obj));
        /// <summary>
        /// 设置存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="timeOffset"></param>
        public void SetValue(string key, object obj, DateTimeOffset timeOffset)
            => cache.StringSetAsync(key, GetString(obj), timeOffset - DateTime.Now);
        /// <summary>
        /// 设置存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        public void SetValue(string key, object value, DateTime timeout) => cache.StringSetAsync(key, GetString(value), timeout - DateTime.Now);

        /// <summary>
        /// 从对象中转变为string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetString(object obj)
        {
            //如果是值类型
            if (obj.GetType().IsValueType)
            {
                return obj.ToString();
            }
            else
            {
                return JsonConvert.SerializeObject(obj);
            }
        }
        /// <summary>
        /// 转换为对应的对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="content">对象字符串</param>
        /// <returns>对象实体</returns>
        private T ConvertValue<T>(string content)
        {
            if (typeof(T).IsValueType)
            {
                return content.ToBaseType<T>();
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

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
