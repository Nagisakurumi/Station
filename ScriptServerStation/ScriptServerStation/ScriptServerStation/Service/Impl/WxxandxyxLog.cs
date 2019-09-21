using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xyxandwxx.LobLib;

namespace ScriptServerStation.Service.Impl
{
    /// <summary>
    /// 日志实体
    /// </summary>
    public class WxxandxyxLog : ILog
    {
        /// <summary>
        /// 日志输出
        /// </summary>
        private LogInfo log = new LogInfo();
        /// <summary>
        /// 日志系统
        /// </summary>
        /// <param name="logFileName"></param>
        public WxxandxyxLog(string logFileName)
        {
            log.FileName = logFileName;
        }
        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="msg"></param>
        public void Log(string msg) => log.log(msg);
        /// <summary>
        /// 输出日志 
        /// </summary>
        /// <param name="objs"></param>
        public void Log(params object[] objs) => log.log(objs);
    }
}
