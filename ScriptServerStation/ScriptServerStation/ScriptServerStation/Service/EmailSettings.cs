using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation
{
    public class EmailSettings
    {
        /// <summary>  
        /// smtp 服务器   
        /// </summary>  
        public string SmtpHost { get; set; }
        /// <summary>  
        /// smtp 服务器端口  默认为25  
        /// </summary>  
        public int SmtpPort { get; set; }
        /// <summary>  
        /// 发送者 Eamil 地址  
        /// </summary>  
        public string FromEmailAddress { get; set; }
        /// <summary>  
        /// 发送者 Eamil 密码  
        /// </summary>  
        public string FormEmailPassword { get; set; }
    }
}
