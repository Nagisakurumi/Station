using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ScriptServerStation.Service.Impl
{
    public class EmailConnect : IEmailConnect
    {
        /// <summary>
        /// 邮件发送接口
        /// </summary>
        private SmtpClient smtpClient = new SmtpClient();
        /// <summary>
        /// 发送地址
        /// </summary>
        private MailAddress fromAdress = null;
        /// <summary>
        /// /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account">邮箱账号</param>
        /// <param name="password">邮箱密码</param>
        /// <param name="fromaddress">邮箱地址</param>
        /// <param name="port">端口</param>
        /// <param name="nickname">昵称</param>
        /// <param name="host">主机</param>
        public EmailConnect(string account, string password, string fromaddress, int port, string nickname,
            string host)
        {
            smtpClient.Timeout = 15 * 1000;
            smtpClient.Host = host;
            smtpClient.Port = port;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(account, password);
            fromAdress = new MailAddress(fromaddress, nickname);
            smtpClient.EnableSsl = true;
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        /// <param name="address">地址</param>
        /// <returns>是否成功发送</returns>
        public bool SendEmail(string title, string msg, string address)
        {
            try
            {
                MailMessage mailMessage = new MailMessage(fromAdress, new MailAddress(address));
                mailMessage.Subject = title;
                mailMessage.Body = msg;
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("邮箱错误，请检查邮箱设置!");
            }
        }
    }
}
