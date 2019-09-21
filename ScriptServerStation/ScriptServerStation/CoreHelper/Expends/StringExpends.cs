using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace CoreHelper.Expends
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExpends
    {
        /// <summary>
        /// 读取该路径下的文件作为json数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFileAsJsonData(this string path)
        {
            if (!File.Exists(path)) return null;
            string content = "";
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                byte[] datas = new byte[stream.Length];
                stream.Read(datas, 0, datas.Length);
                content = System.Text.Encoding.UTF8.GetString(datas);
                datas = null;
            }
            return content;
        }
        /// <summary>
        /// 转换到bool
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool ToBool(this string msg)
        {
            return Convert.ToBoolean(msg);
        }
        /// <summary>
        /// 转换到日期
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string msg)
        {
            return Convert.ToDateTime(msg);
        }
        /// <summary>
        /// 字符串格式化
        /// </summary>
        /// <param name="vs"></param>
        public static string ToFormat(this string str, params object [] vs)
        {
            return string.Format(str, vs);
        }
    }
}
