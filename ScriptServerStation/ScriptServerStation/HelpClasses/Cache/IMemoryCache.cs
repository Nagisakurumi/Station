using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.HelpClasses.Cache
{
    /// <summary>
    /// 缓存
    /// </summary>
    public interface IMemoryCache
    {
        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetValue(string key, object value);
        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout">超时时刻</param>
        void SetValue(string key, object value, DateTime timeout);
        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeOffset">超时时刻</param>
        void SetValue(string key, object value, DateTimeOffset timeOffset);
        /// <summary>
        /// 获取缓存的内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetValue<T>(string key);
        /// <summary>
        /// 是否包含key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsContain(string key);
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        bool Remove(string key);
    }
}
