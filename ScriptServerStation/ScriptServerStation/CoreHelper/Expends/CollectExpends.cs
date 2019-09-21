using System;
using System.Collections.Generic;
using System.Text;

namespace CoreHelper.Expends
{
    public static class CollectExpends
    {
        /// <summary>
        /// 枚举循环
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> ts, Action<T> action)
        {
            foreach (var item in ts)
            {
                action?.Invoke(item);
            }
        } 
    }
}
