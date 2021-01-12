using ScriptServerStation.Database;
using ScriptServerStation.FileUpdate;
using ScriptServerStation.HelpClasses.Cache.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Utils
{
    /// <summary>
    /// 静态参数
    /// </summary>
    public static class StaticHelp
    {
        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GetRandomString()
        {
            string code = DateTime.Now.ToString("yyyyMMddHHmmss");
            string codes = "1234567890ABCDEF";
            string msg = "";
            int index = 0;
            foreach (var item in code)
            {
                int idx = index++ % 16;
                msg += codes[idx] + (item + idx);
            }
            return msg;
        }



        /// <summary>
        /// 根据历史版本的改动获取所有需要下载的文件的集合
        /// </summary>
        /// <param name="historys">历史改动版本</param>
        /// <param name="cacheKey">配置</param>
        /// <param name="isDesc">是否逆向操作</param>
        /// <returns>需要修改的文件信息</returns>
        public static VersionUpdateInfo ConvertToHistory(List<Versioninfo> historys, CacheKey cacheKey, bool isDesc = false)
        {
            VersionUpdateInfo version = new VersionUpdateInfo();
            if (isDesc == false)
            {
                //遍历历史
                foreach (var item in historys)
                {
                    //获取根目录
                    string root = cacheKey.UploadCacheFileRootPath.FromProjectRootCombine(item.Path, cacheKey.VersionUpdateFileSaveName);
                    //读取对应版本的配置文件
                    VersionUpdateInfo info = root.ReadFromFile<VersionUpdateInfo>();
                    foreach (var fileInfo in info.Files.Values)
                    {
                        version.AddFileInfo(fileInfo);
                    }
                }
            }
            else
            {

                for (int i = historys.Count - 1; i > 0; i--)
                {
                    Versioninfo item = historys[i];
                    //获取根目录
                    string root = cacheKey.UploadCacheFileRootPath.FromProjectRootCombine(item.Path, cacheKey.VersionUpdateFileSaveName);

                    //读取对应版本的配置文件
                    VersionUpdateInfo info = root.ReadFromFile<VersionUpdateInfo>();
                    foreach (var fileInfo in info.Files.Values)
                    {
                        if (fileInfo.Status == FileStatus.Add)
                        {
                            fileInfo.Status = FileStatus.Delete;
                        }
                        else if (fileInfo.Status == FileStatus.Delete)
                        {
                            fileInfo.Version = historys[i - 1].Value;
                            fileInfo.Status = FileStatus.Add;
                        }
                        else if (fileInfo.Status == FileStatus.Modify)
                        {
                            fileInfo.Version = historys[i - 1].Value;
                            fileInfo.Status = FileStatus.Modify;
                        }
                        version.AddFileInfo(fileInfo);
                    }
                }
            }
            return version;
        }
    }
}
