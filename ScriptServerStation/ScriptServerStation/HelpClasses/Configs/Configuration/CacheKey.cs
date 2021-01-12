using Microsoft.Extensions.Configuration;
using ScriptServerStation.Expends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.HelpClasses.Configs.Configuration
{
    // <summary>
    /// 配置文件中的CacheKey
    /// </summary>
    public class CacheKey : ConfigurationHelp
    {
        /// <summary>
        /// 读取UserCacheName
        /// </summary>
        public string TokenName => Read<CacheKey, string>(c => c.TokenName);
        /// <summary>
        /// 上传缓存文件根路径
        /// </summary>
        public string UploadCacheFileRootPath => Read<CacheKey, string>(c => c.UploadCacheFileRootPath);
        /// <summary>
        /// 上传文件
        /// </summary>
        public string UploadCacheFileName => UploadCacheFileRootPath.AppendPath(Read<CacheKey, string>(c => c.UploadCacheFileName));
        /// <summary>
        /// 版本更新文件保存
        /// </summary>
        public string VersionUpdateFileSaveName => UploadCacheFileRootPath.AppendPath(Read<CacheKey, string>(c => c.VersionUpdateFileSaveName));
        /// <summary>
        /// /构造函数
        /// </summary>
        /// <param name="config"></param>
        public CacheKey(IConfiguration config) : base(config) { }
    }
}
