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
        /// <param name="msg"></param>
        void Log(string msg);
        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="objs"></param>
        void Log(params object[] objs);
    }
}
