using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBaseController.Entitys;
using Microsoft.AspNetCore.Http;

namespace ScriptServerStation.Service
{
    public interface IUserService
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        List<User> GetUsers();
        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool AddUser(User user, string verification, ISession session);
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        bool IsLogion(string account, ISession session);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        bool Login(string account, string password, ISession session);
        /// <summary>
        /// 获取用户信息，根据账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        User GetUser(string account);
        /// <summary>
        /// 买会员
        /// </summary>
        /// <param name="user"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        bool BuyVIP(User user, int day);
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="user"></param>
        /// <param name="code"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        bool Recharge(User user, string code, double money);
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool GetVerification(string email, ISession session);
    }
}
