using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.FileUpdate
{
    /// <summary>
    /// 版本信息更新文件
    /// </summary>
    public class VersionUpdateInfo
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 文件信息集合
        /// </summary>
        public Dictionary<string, FileItemInfo> Files { get; set; } = new Dictionary<string, FileItemInfo>();
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 上传作者
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 数据包总大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 保存目录路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 版本信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 添加一个文件修改信息
        /// </summary>
        /// <param name="info"></param>
        public void AddFileInfo(FileItemInfo info)
        {
            //如果已经包含了,进行替换
            if (this.Files.ContainsKey(info.Path))
            {
                this.Files[info.Path] = info;
            }
            else
            {
                this.Files.Add(info.Path, info);
            }
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <returns></returns>
        public Version GetVersion()
        {
            return System.Version.Parse(this.Version);
        }
        /// <summary>
        /// 完善版本号格式
        /// </summary>
        public bool FlushVersion()
        {
            string newVersion = "";
            string[] versions = this.Version.Split('.');
            foreach (var item in versions)
            {
                if(item.Length == 1)
                {
                    newVersion += "0" + item + ".";
                }
                else if(item.Length == 2)
                {
                    newVersion += item + ".";
                }
                else
                {
                    return false;
                }
            }

            this.Version = newVersion.Substring(0, newVersion.Length - 1);
            return true;
        }
    }
}
