using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptControllerLib
{
    public static class Expends
    {

        /// <summary>
        /// 转换到list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this T[] t)
        {
            List<T> ts = new List<T>(t.Length);
            foreach (var item in t)
            {
                ts.Add(item);
            }
            return ts;
        }
        /// <summary>
        /// 获取第一项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static T First<T>(this IEnumerable<T> ts)
        {
            foreach (var item in ts)
            {
                return item;
            }
            return default(T);
        }
        
    }
}
