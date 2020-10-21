using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.FileUpdate
{
    /// <summary>
    /// 文件状态
    /// </summary>
    public enum FileStatus
    {
        /// <summary>
        /// 追加
        /// </summary>
        Add,
        /// <summary>
        /// 保留
        /// </summary>
        On,
        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// /修改
        /// </summary>
        Modify,
    }
}
