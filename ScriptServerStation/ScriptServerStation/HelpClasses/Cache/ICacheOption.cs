using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.HelpClasses.Cache
{
    /// <summary>
    /// 缓存类
    /// </summary>
    public interface ICacheOption
    {
        /// <summary>
        /// 设置缓存的值
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">缓存的对象</param>
        void SetValue(string key, object obj);
        /// <summary>
        /// 键值是否存在
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>是否存在</returns>
        bool Exists(string key);
        /// <summary>
        /// 获取缓存的值
        /// </summary>
        /// <typeparam name="T">取出来并且转换类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>缓存的值</returns>
        T GetValue<T>(string key);
        /// <summary>
        /// 设置缓存的值
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">缓存的对象</param>
        /// <param name="timeOffset">超时时间</param>
        void SetValue(string key, object obj, DateTimeOffset timeOffset);
        /// <summary>
        /// 删除键值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>是否删除成功</returns>
        bool Remove(string key);
    }
}
