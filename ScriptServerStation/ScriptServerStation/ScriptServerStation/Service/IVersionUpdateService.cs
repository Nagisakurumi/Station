using Microsoft.AspNetCore.Http;
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
        /// <param name="files"></param>
        /// <returns></returns>
        bool AddVersion(IFormFileCollection files);
        /// <summary>
        /// 获取最后的更新配置文件
        /// </summary>
        /// <returns></returns>
        string GetLastUpdateConfig();
    }
}
