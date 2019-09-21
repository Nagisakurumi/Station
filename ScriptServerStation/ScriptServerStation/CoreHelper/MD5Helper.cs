using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CoreHelper
{
    public class MD5Helper
    {
        /// <summary>
		/// 对字符串进行MD5加密
		/// </summary>
		/// <param name="myString"></param>
		/// <returns></returns>
		public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            StringBuilder byte2String = new StringBuilder();

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String.Append(targetData[i].ToString("x2"));
            }

            return byte2String.ToString();
        }
    }
}
