using ScriptServerStation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Utils
{
    /// <summary>
    /// 用户控制器扩展方法
    /// </summary>
    public static class UserControllerExpends
    {
        /// <summary>
        /// 检测用户是否已经登陆
        /// </summary>
        /// <param name="userController"></param>
        /// <returns></returns>
        public static bool IsLogin(this UserController userController)
        {
            var token = userController.GetUserToken();
            var username = userController.GetUserToken(userController.CacheKey.UserCookieName);
            if(token != null && username != null && userController.CacheOption.GetValue<string>(username).Equals(token))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取用户token
        /// </summary>
        /// <param name="userController"></param>
        /// <returns></returns>
        public static string GetUserToken(this UserController userController)
        {
            return userController.GetUserToken(userController.CacheKey.UserCacheName);
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="userController"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetUserToken(this UserController userController, string key)
        {
            var cookie = userController.Request.Cookies.Where(p => p.Key == key);
            if (cookie.Count() == 0)
                return null;
            else
                return cookie.FirstOrDefault().Value;
        }
        /// <summary>
        /// 设置token
        /// </summary>
        /// <param name="userController"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool SetToken(this UserController userController, string token)
        {
            userController.SetToken(userController.CacheKey.UserCacheName, token);
            return true;
        }
        /// <summary>
        /// 设置token
        /// </summary>
        /// <param name="userController"></param>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool SetToken(this UserController userController, string key, string token)
        {
            userController.Response.Cookies.Append(key, token);
            return true;
        }
    }
}
