using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ScriptServerStation.Attribute;
using ScriptServerStation.Database;
using ScriptServerStation.FileUpdate;
using ScriptServerStation.HelpClasses.Cache.Configuration;
using ScriptServerStation.Utils;

namespace ScriptServerStation.Service.Impl
{
    /// <summary>
    /// 版本更新
    /// </summary>
    [Interface]
    public class VersionUpdateServiceImpl : IVersionUpdateService
    {
        /// <summary>
        /// 缓存key
        /// </summary>
        public CacheKey CacheKey { get; set; }
        /// <summary>
        /// 数据库
        /// </summary>
        public DataBaseContext DataBaseContext { get; set; }
        /// <summary>
        /// 添加一个新版本
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns>是否添加成功</returns>
        public bool AddVersion(Versioninfo version)
        {
            version.Date = DateTime.Now;
            DataBaseContext.Versioninfo.Add(version);
            this.DataBaseContext.SaveChanges();
            return true;
        }
        /// <summary>
        /// 获取版本号version之后的所有版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<Versioninfo> GetAfterVersion(string version)
        {
            return (from v in this.DataBaseContext.Versioninfo where v.Date > 
                    (from c in this.DataBaseContext.Versioninfo where c.Value == version select c.Date).First() select v).ToList();
        }
        /// <summary>
        /// 获取所有版本信息
        /// </summary>
        /// <returns></returns>
        public List<Versioninfo> GetAllVersion()
        {
            return (from v in this.DataBaseContext.Versioninfo select v).ToList();
        }
        /// <summary>
        /// 获取指定版本号之前的所有版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<Versioninfo> GetBeforeVersion(string version)
        {
            return (from v in this.DataBaseContext.Versioninfo where v.Value.CompareTo(version) <= 0 select v).ToList();
        }

        /// <summary>
        /// 获取最后更新配置文件
        /// </summary>
        /// <returns></returns>
        public Versioninfo GetLastUpdateConfig()
        {
            var configpath = (from m in this.DataBaseContext.Versioninfo where m.Date == 
                              (from s in this.DataBaseContext.Versioninfo select s).Max(s => s.Date)
                              select m);
            return configpath.FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldversion"></param>
        /// <param name="targetversion"></param>
        /// <returns></returns>
        public List<Versioninfo> GetVersionBetween(string oldversion, string targetversion)
        {
            return (from v in this.DataBaseContext.Versioninfo
                    where v.Value.CompareTo(oldversion) > 0 && (v.Value.CompareTo(targetversion) == 0
                        || v.Value.CompareTo(targetversion) == -1)
                    select v).ToList();
        }

        /// <summary>
        /// 根据版本号获取版本信息
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns></returns>
        public Versioninfo GetVersionByVersion(string version)
        {
            return (from v in this.DataBaseContext.Versioninfo where v.Value == version select v).FirstOrDefault();
        }
        /// <summary>
        /// 根据版本号获取版本信息
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns></returns>
        public VersionUpdateInfo GetVersionInfoByVersion(string version)
        {
            Versioninfo versioninfo = this.GetVersionByVersion(version);
            if(versioninfo != null)
            {
                return CacheKey.UploadCacheFileRootPath.FromProjectRootCombine(versioninfo.Path, CacheKey.VersionUpdateFileSaveName).ReadFromFile<VersionUpdateInfo>();
            }
            else
            {
                return null;
            }
        }
    }
}
