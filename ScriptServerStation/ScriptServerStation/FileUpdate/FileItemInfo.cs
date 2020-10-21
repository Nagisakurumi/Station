using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.FileUpdate
{
    /// <summary>
    /// 单个文件信息
    /// </summary>
    public class FileItemInfo
    {
        /// <summary>
        /// 从哪个版本中获取
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 文件md5码
        /// </summary>
        public string Md5 { get; set; }
        /// <summary>
        /// 文件状态
        /// </summary>
        public FileStatus Status { get; set; }
        /// <summary>
        /// 文件所在路径
        /// </summary>
        public string Path { get; set; }

    }
}
