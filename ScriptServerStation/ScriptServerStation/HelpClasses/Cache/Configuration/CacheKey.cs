using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.HelpClasses.Cache.Configuration
{
    /// <summary>
    /// 配置文件中的CacheKey
    /// </summary>
    public class CacheKey : ConfigurationHelp
    {
        /// <summary>
        /// 读取UserCacheName
        /// </summary>
        public string UserCacheName => Read<string>();
        /// <summary>
        /// 读取UploadFileName
        /// </summary>
        public string UploadCacheFileName => Read<string>();
        /// <summary>
        /// 上传文件根目录
        /// </summary>
        public string UploadCacheFileRootPath => Read<string>();

        /// <summary>
        /// 读取配置文件名
        /// </summary>
        public string VersionUpdateFileSaveName => Read<string>();
        /// <summary>
        /// 读取再cookie中的用户名
        /// </summary>
        public string UserCookieName => Read<string>();
        /// <summary>
        /// /构造函数
        /// </summary>
        /// <param name="config"></param>
        public CacheKey(IConfiguration config) : base(config) { }


    }
}
