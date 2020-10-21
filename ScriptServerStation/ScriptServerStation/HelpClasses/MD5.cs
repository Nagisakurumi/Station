using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace ScriptServerStation.HelpClasses
{
    /// <summary>
    /// 此类获取md5加密值均为大写，如果要获取小写：MD5Comm.Get32MD5One(xx).ToLower();或完善此类。
    /// </summary>
    public class MD5Comm
    {
        #region --Expired code--
        ///// <summary>
        ///// MD5加密
        ///// </summary>
        //public string Md5(string txt)
        //{
        //    //return FormsAuthentication.HashPasswordForStoringInConfigFile(txt, "MD5");//此方法nf4.5后不再支持
        //}
        //public string Md5Pass(string pwd)
        //{
        //    return Md5(pwd + "Jiahao");
        //} 
        #endregion

        /// <summary>
        /// 此代码示例通过创建哈希字符串适用于任何 MD5 哈希函数 （在任何平台） 上创建 32 个字符的十六进制格式哈希字符串
        /// 官网案例改编
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Get32MD5One(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                string hash = sBuilder.ToString();
                hash = hash.ToUpper();
                string codes = "1234567890ABCDEF";
                string result = "";

                for (int i = 0; i < hash.Length; i++)
                {
                    result += hash[i] + codes[hash[i] % 2] << 2;
                }
                return result;
            }
        }

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
    }
}
