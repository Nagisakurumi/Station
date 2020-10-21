using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Service
{
    /// <summary>
    /// 邮件操作接口
    /// </summary>
    public interface IEmailConnect
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">正文</param>
        /// <param name="address">地址</param>
        /// <returns>是否成功发送</returns>
        bool SendEmail(string title, string msg, string address);
    }
}
