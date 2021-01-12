using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using ScriptServerStation.Controllers;
using ScriptServerStation.Database;
using ScriptServerStation.HelpClasses.Cache;
using ScriptServerStation.HelpClasses.Configs.Configuration;
using ScriptServerStation.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Expends
{
    public static class HttpExpends
    {
        /// <summary>
        /// 设置token到返回的信息
        /// </summary>
        /// <param name="baseController">http请求上下文</param>
        /// <param name="token">token内容</param>
        public static void SetUserToken(this ScriptBaseController baseController, TokenInfo token, User user)
        {
            baseController.HttpContext.Response.Headers.Add(baseController.CacheKey.TokenName, TokenInfo.TokenToString(token));
            baseController.MemoryCache.SetValue(user.Name, user, token.OverTime);
        }

        /// <summary>
        /// 设置token到返回的信息
        /// </summary>
        /// <param name="httpContext">http请求上下文</param>
        /// <param name="cacheKey">缓存键值</param>
        /// <param name="token">token内容</param>
        public static void SetUserCookie(this HttpContext httpContext, CacheKey cacheKey, string token)
        {
            httpContext.Response.Cookies.Append(cacheKey.TokenName, token);
        }
        /// <summary>
        /// 从cookie中获取用户信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static string GetUserCookie(this HttpContext httpContext, CacheKey cacheKey)
        {
            if (httpContext.Request.Cookies.ContainsKey(cacheKey.TokenName))
            {
                return httpContext.Request.Cookies[cacheKey.TokenName].ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取用户的token
        /// </summary>
        /// <param name="httpContext">http请求上下文</param>
        /// <param name="cacheKey">缓存</param>
        /// <remarks>token</remarks>
        public static string GetUserToken(this HttpContext httpContext, CacheKey cacheKey)
        {
            StringValues value;
            if (httpContext == null || httpContext.Request == null || httpContext.Request.Headers == null) return null;
            if (!httpContext.Request.Headers.ContainsKey(cacheKey.TokenName)) return null;
            if (!httpContext.Request.Headers.TryGetValue(cacheKey.TokenName, out value))
            {
                return null;
            }
            //如果有token
            if (value.Count == 0)
            {
                return null;
            }
            return value.FirstOrDefault();
        }
        /// <summary>
        /// 获取登陆的用户信息
        /// </summary>
        /// <param name="httpContext">http请求上下文</param>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="memoryCache">缓存</param>
        /// <returns>用户信息</returns>
        public static User GetLoginUser(this HttpContext httpContext, CacheKey cacheKey, IMemoryCache memoryCache)
        {
            string token = httpContext.GetUserCookie(cacheKey);
            if (token == null)
            {
                return null;
            }
            TokenInfo info = TokenInfo.StringToToken(token);
            if (info == null) return null;
            return memoryCache.GetValue<User>(info.UserName);
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="daweiBase"></param>
        /// <returns></returns>
        public static User GetLoginUser(this ScriptBaseController baseController)
        {
            return baseController.HttpContext.GetLoginUser(baseController.CacheKey, baseController.MemoryCache);
        }
    }
}
