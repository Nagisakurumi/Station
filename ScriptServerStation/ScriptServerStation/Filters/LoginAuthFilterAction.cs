using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ScriptServerStation.Expends;
using ScriptServerStation.HelpClasses.Cache;
using ScriptServerStation.HelpClasses.Configs.Configuration;
using ScriptServerStation.Items;
using ScriptServerStation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Filters
{
    /// <summary>
    /// 登陆验证拦截器
    /// </summary>
    public class LoginAuthFilterAction : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// 不需要验证的集合
        /// </summary>
        private string[] UnAuthList { get; set; }
        /// <summary>
        /// 全局日志
        /// </summary>
        public ILog Log { get; set; }
        /// <summary>
        /// 配置
        /// </summary>
        public CacheKey CacheKey { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>
        public IMemoryCache MemoryCache { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unAuth">不需要验证列表</param>
        /// <param name="log">日志</param>
        public LoginAuthFilterAction(string[] unAuth, ILog log, CacheKey cacheKey, IMemoryCache memory)
        {
            this.UnAuthList = unAuth;
            this.Log = log;
            this.CacheKey = cacheKey;
            this.MemoryCache = memory;
        }

        /// <summary>
        /// 当动作要被执行的时候
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //base.OnActionExecuting(context);
            if (context.HttpContext.Request == null) return;
            try
            {
                string path = GetRoutePath(context);
                if (!path.EqualLists(s => false, UnAuthList))
                {
                    return;
                }
                var content = context.HttpContext.GetLoginUser(CacheKey, MemoryCache);
                //如果用户为登陆则直接拦截
                if (content == null)
                {
                    context.Result = new BadRequestObjectResult(Result.Fail("还未登陆，或者token过期!"));
                }
            }
            catch (Exception ex)
            {
                Log.Log(ex);
                context.Result = new BadRequestObjectResult(Result.Fail(ex.Message));
            }
        }

        /// <summary>
        /// 获取路由路径
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        private string GetRoutePath(ActionExecutingContext context)
        {
            return context.ActionDescriptor.RouteValues["controller"] + "/" + context.ActionDescriptor.RouteValues["action"];
        }
    }
}
