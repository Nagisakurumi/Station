using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataBaseController;
using DataBaseController.Entitys;
using ScriptServerStation.HelpClasses;
using ScriptServerStation.Service;

namespace ScriptServerStation.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        /// <summary>
        /// service
        /// </summary>
        private IUserService userService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        //[HttpGet("test")]
        public ReturnObj GetTest()
        {
            return new ReturnObj() { IsSuccess = "true", Result = "访问成功" };
            //return "访问成功!";
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("getuserlist")]
        public ReturnObj GetUserList()
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                returnObj.Result = userService.GetUsers();
                returnObj.SetIsSuccess(true);
            }
            catch (Exception)
            {
                returnObj.SetIsSuccess(false);
            }
            return returnObj;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("userinfo")]
        public ReturnObj GetUserInfo(string account)
        {
            ReturnObj obj = new ReturnObj();
            try
            {
                obj.Result = userService.GetUser(account);
                obj.SetIsSuccess(true);
            }
            catch (Exception)
            {
                obj.SetIsSuccess(false);
            }
            return obj;
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public ReturnObj Login(string account, string password)
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {

                returnObj.SetIsSuccess(userService.Login(account, password, HttpContext.Session));
            }
            catch (Exception)
            {
                returnObj.SetIsSuccess(false);
            }
            return returnObj;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost("regiest")]
        public ReturnObj Regiest(string account, string password, string email, string phone)
        {
            ReturnObj obj = new ReturnObj();
            try
            {
                User user = new User() {
                    Account = account,
                    Guid = Guid.NewGuid(),
                    Password = password,
                    Email = email,
                    Phone = phone,
                    Level = 0,
                    LevelValue = 0,
                    CreateTime = DateTime.Now,
                    IsSpecial = false,
                };

                obj.SetIsSuccess(userService.AddUser(user));
            }
            catch (Exception)
            {
                obj.SetIsSuccess(false);
            }
            return obj;
        }
    }
}
