using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ScriptServerStation.Database;
using ScriptServerStation.Items;

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
        bool AddUser(User user);
        /// <summary>
        /// 判断密码是否正确
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        bool CheckPassword(string username, string password);
        /// <summary>
        /// 获取用户信息，根据账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        User GetUser(string account);
        /// <summary>
        /// 激活账号
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="code">激活码</param>
        /// <param name="isregisteruser">是否是新注册的用户</param>
        /// <returns></returns>
        bool ActiveAccount(string account, string code, bool isregisteruser);
        /// <summary>
        /// 创建一个激活码
        /// </summary>
        /// <param name="codeType">激活码类型</param>
        /// <param name="days">有效日期</param>
        /// <param name="buyaccount">购买激活码的用户</param>
        /// <returns>创建的激活码</returns>
        Activatecode CreateActivatecode(CodeType codeType, int days, string buyaccount);
        /// <summary>
        /// 追加时间
        /// </summary>
        /// <param name="isContain">是否包含非会员</param>
        /// <param name="time">追加的天数</param>
        /// <returns></returns>
        bool AddTimeForEveryOne(bool isContain, int time);
        /// <summary>
        /// 激活体验
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool ExperienceCode(string account);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        bool RestPassword(User user, string newpassword);
        /// <summary>
        /// 更新用户邮箱 信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        void UpdateUserEmail(User user, string email);
    }
}
