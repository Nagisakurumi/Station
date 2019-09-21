using Microsoft.Extensions.Caching.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.HelpClasses.Cache.Redis
{
    public class ExRedisCacheOptions : RedisCacheOptions
    {
        /// <summary>
        /// 数据库序号
        /// </summary>
        public int DataBaseIdx { get; set; } = 0;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ExRedisCacheOptions()
        {

        }
    }
}
