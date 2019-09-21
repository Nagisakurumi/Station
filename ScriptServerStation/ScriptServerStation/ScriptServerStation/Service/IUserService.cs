﻿using System;
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
        bool AddUser(User user);
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        bool IsLogion(string account);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Login(string account, string password, string uuid);
        /// <summary>
        /// 是否是同一个地方的登录信息
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="uuid">地方信息</param>
        /// <returns></returns>
        bool IsSingleLoginInfo(string account, string uuid);
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
        ActiveCode CreateActiveCode(CodeType codeType, int days, string buyaccount);
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="user"></param>
        /// <param name="code"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        bool Recharge(User user, string code, double money);
        /// <summary>
        /// 激活体验
        /// </summary>
        /// <param name="account"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        bool ExperienceCode(string account, string uuid);
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
