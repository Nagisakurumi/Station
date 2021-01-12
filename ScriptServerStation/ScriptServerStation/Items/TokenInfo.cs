using Newtonsoft.Json;
using ScriptServerStation.Expends;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Items
{
    /// <summary>
    /// 用户token令牌
    /// </summary>
    public class TokenInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime OverTime { get; set; }

        /// <summary>
        /// token 偏移密钥
        /// </summary>
        private static string TokenOffset = "wxxandxyx";


        public TokenInfo(string username, DateTime date) {
            this.UserName = username;
            this.OverTime = date;
        }

        /// <summary>
        /// token信息转换为字符串可以传递
        /// </summary>
        /// <param name="tokenInfo">token信息</param>
        /// <returns>字符串</returns>
        public static string TokenToString(TokenInfo tokenInfo)
        {
            tokenInfo.UserName = tokenInfo.UserName.ToBase64();
            return JsonConvert.SerializeObject(tokenInfo).Offset(TokenOffset).ToBase64();
        }
        /// <summary>
        /// 字符串转换到token信息
        /// </summary>
        /// <param name="content">token字符串</param>
        /// <returns></returns>
        public static TokenInfo StringToToken(string content)
        {
            try
            {
                TokenInfo tokenInfo = content.FromBase64().ReOffset(TokenOffset).ToAny<TokenInfo>();
                tokenInfo.UserName = tokenInfo.UserName.FromBase64();
                return tokenInfo;
            }catch(Exception ex)
            {
                return null;
            }
        }

    }
}
