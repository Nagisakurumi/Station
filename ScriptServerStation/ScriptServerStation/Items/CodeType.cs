using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Items
{
    /// <summary>
    /// /激活码类型
    /// </summary>
    public enum CodeType : int
    {
        /// <summary>
        /// 正常激活码
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 推荐吗
        /// </summary>
        Recommend = 2,
        /// <summary>
        /// 活动吗
        /// </summary>
        Activate = 3,
    }
}
