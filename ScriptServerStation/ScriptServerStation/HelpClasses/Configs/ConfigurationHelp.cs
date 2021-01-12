using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ScriptServerStation.Expends;

namespace ScriptServerStation.HelpClasses.Configs
{
    /// <summary>
    /// 读取配置文件的值
    /// </summary>
    public abstract class ConfigurationHelp
    {
        /// <summary>
        /// 配置文件读取
        /// </summary>
        protected IConfiguration Configuration { get; set; }
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
        /// <typeparam name="T"><peparam>
        /// <returns></returns>
        protected V Read<T, V>(Expression<Func<T, object>> expression)
        {
            string name = expression.GetExpressionProperty();
            return Configuration["{0}:{1}".Format(this.GetType().Name, name)].ToBaseType<V>();
        }
    }
}
