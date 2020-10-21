using Microsoft.Extensions.Configuration;
using ScriptServerStation.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.HelpClasses.Cache
{
    /// <summary>
    /// 读取配置文件的值
    /// </summary>
    public abstract class ConfigurationHelp
    {
        /// <summary>
        /// 堆栈信息
        /// </summary>
        private StackTrace trace = new StackTrace();

         /// <summary>
         /// 配置文件读取
         /// </summary>
        private IConfiguration Configuration { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigurationHelp(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        /// <summary>
        /// 读取内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T Read<T>()
        {
            string name = new StackFrame(1).GetMethod().Name.Replace("get_", "");
            return Configuration["{0}:{1}".Format(this.GetType().Name, name)].ToBaseType<T>();
        }
    }
}
