using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.HelpClasses.Cache.MyCache
{
    /// <summary>
    /// 缓存类的实现
    /// </summary>
    public class CacheInterfaceImplement : IMemoryCache
    {
        /// <summary>
        /// 内存缓存
        /// </summary>
        public MemoryCache Cache { get; set; } = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(string key, object value)
        {
            this.Cache.Set<object>(key, value);
        }
        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout">超时时刻</param>
        public void SetValue(string key, object value, DateTime timeout)
        {
            this.Cache.Set<object>(key, value, timeout);
        }
        /// <summary>
        /// 获取缓存的内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            return (T)Cache.Get<object>(key);
        }
        /// <summary>
        /// 是否包含key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsContain(string key)
        {
            return Cache.Get(key) != null;
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        public bool Remove(string key)
        {
            if (IsContain(key))
            {
                Cache.Remove(key);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeOffset"></param>
        public void SetValue(string key, object value, DateTimeOffset timeOffset) => Cache.Set<object>(key, value, timeOffset);

    }
}
