using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataBaseController;
using DataBaseController.Entitys;
using Microsoft.AspNetCore.Http;

namespace ScriptServerStation.Service.Impl
{
    public class VersionUpdateServiceImpl : BaseService, IVersionUpdateService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataBaseContext"></param>
        public VersionUpdateServiceImpl(DataBaseContext DataBaseContext) : base(DataBaseContext)
        {
        }
        /// <summary>
        /// 保存版本文件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public bool AddVersion(IFormFileCollection files)
        {
            DateTime now = DateTime.Now;
            VersionUpdate versionUpdate = new VersionUpdate();
            versionUpdate.Date = now.ToString("yyyy-MM-dd HH:mm:ss:fff");
            string rootdir = AppContext.BaseDirectory + "//VersionUpdates//" + now.ToString("yyyy-MM-dd HH-mm-ss-fff") + "//";
            if(Directory.Exists(rootdir) == false)
            {
                Directory.CreateDirectory(rootdir);
            }
            ///存放更新信息的文件
            versionUpdate.Path = rootdir;
            foreach (var item in files)
            {
                string filepath = rootdir + item.FileName;
                using (FileStream stream = File.Open(filepath, FileMode.Create))
                {
                    item.CopyTo(stream);
                }
            }
            DataBaseContext.VersionUpdates.Add(versionUpdate);
            this.DataBaseContext.SaveChanges();
            return true;
        }
        /// <summary>
        /// 获取最后更新配置文件
        /// </summary>
        /// <returns></returns>
        public string GetLastUpdateConfig()
        {

            var configpath = (from m in this.DataBaseContext.VersionUpdates where m.Date == (from s in this.DataBaseContext.VersionUpdates select s).Max(s => s.Date)
                              select m);
            if(configpath.Count() == 0)
            {
                return null;
            }
            else
            {
                return configpath.First().Path;
            }

        }
    }
}
