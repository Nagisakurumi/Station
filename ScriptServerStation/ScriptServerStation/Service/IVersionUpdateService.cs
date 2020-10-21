using Microsoft.AspNetCore.Http;
using ScriptServerStation.Database;
using ScriptServerStation.FileUpdate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Service.Impl
{
    public interface IVersionUpdateService
    {
        /// <summary>
        /// 保存版本文件
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        bool AddVersion(Versioninfo version);
        /// <summary>
        /// 获取最后的更新配置文件
        /// </summary>
        /// <returns></returns>
        Versioninfo GetLastUpdateConfig();
        /// <summary>
        /// 根据版本号获取版本信息
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns>版本信息</returns>
        Versioninfo GetVersionByVersion(string version);
        /// <summary>
        /// 根据版本号获取版本信息
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns>版本信息</returns>
        VersionUpdateInfo GetVersionInfoByVersion(string version);
        /// <summary>
        /// 获取版本号为versionn之后的所有版本
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns></returns>
        List<Versioninfo> GetAfterVersion(string version);
        /// <summary>
        /// 获指定版本号之前的所有版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        List<Versioninfo> GetBeforeVersion(string version);
        /// <summary>
        /// 获取所有版本信息
        /// </summary>
        /// <returns></returns>
        List<Versioninfo> GetAllVersion();
        /// <summary>
        /// 获取版本 > oldversion 且小于等于target
        /// </summary>
        /// <param name="oldversion"></param>
        /// <param name="targetversion"></param>
        /// <returns></returns>
        List<Versioninfo> GetVersionBetween(string oldversion, string targetversion);
    }
}
