using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Service
{
    /// <summary>
    /// 日志
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="objs"></param>
        void Log(params object[] objs);
        /// <summary>
        /// 输出错误日志
        /// </summary>
        /// <param name="objs"></param>
        void Error(params object[] objs);
        /// <summary>
        /// 输出警告日志
        /// </summary>
        /// <param name="objs"></param>
        void Waring(params object[] objs);
    }
}
