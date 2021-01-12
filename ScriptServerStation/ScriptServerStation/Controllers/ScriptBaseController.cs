using Microsoft.AspNetCore.Mvc;
using ScriptServerStation.HelpClasses.Cache;
using ScriptServerStation.HelpClasses.Configs.Configuration;
using ScriptServerStation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Controllers
{
    public class ScriptBaseController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        public ILog Log { get; set; }
        /// <summary>
        /// 配置文件
        /// </summary>
        public CacheKey CacheKey { get; set; }
        /// <summary>
        /// 内存缓存
        /// </summary>
        public IMemoryCache MemoryCache { get; set; }
    }
}
