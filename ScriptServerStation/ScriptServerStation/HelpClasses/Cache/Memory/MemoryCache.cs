﻿using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.HelpClasses.Cache.Memory
{
    /// <summary>
    /// 缓存类的实现
    /// </summary>
    public class MemoryCache : IMemoryCache
    {
        /// <summary>
        /// 内存缓存
        /// </summary>
        public Microsoft.Extensions.Caching.Memory.MemoryCache Cache { get; set; } = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());

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
        /// <param name="timeout">超时时间(s)</param>
        public void SetValue(string key, object value, int timeout)
        {
            this.Cache.Set<object>(key, value, DateTime.Now + TimeSpan.FromSeconds(timeout));
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
        public void Remove(string key)
        {
            if (IsContain(key))
            {
                Cache.Remove(key);
            }
        }
    }
}
