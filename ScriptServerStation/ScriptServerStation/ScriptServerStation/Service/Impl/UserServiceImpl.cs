﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBaseController;
using DataBaseController.Entitys;
using Microsoft.AspNetCore.Http;
using CoreHelper.Expends;
using ScriptServerStation.HelpClasses;
using ScriptServerStation.HelpClasses.Cache;

namespace ScriptServerStation.Service.Impl
{
    public class UserServiceImpl : BaseService, IUserService
    {

namespace ScriptServerStation.Service.Impl
{
    public class UserServiceImpl : BaseService, IUserService
    {
        /// <summary>
        /// 缓存
        /// </summary>
        private ICacheOption session = null;

        public UserServiceImpl(DataBaseContext DataBaseContext, 
            ICacheOption cacheOption) : base(DataBaseContext)
        {
            this.session = cacheOption;
        }
        /// <summary>
        /// 激活账号
        /// </summary>
        /// <param name="user"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool ActiveAccount(string account, string code, bool isregisteruser)
        {
            User user = GetUser(account);
            var list = DataBaseContext.ActiveCodes.Where(c =>
            c.Code.Equals(code) && !Convert.ToBoolean(c.IsUsed));
            if(list.Count() == 0)
            {
                throw new Exception("激活码错误!");
            }
            DataBaseController.Entitys.ActiveCode activeCode = list.First();
            if(Convert.ToDateTime(activeCode.OverDate) < DateTime.Now)
            {
                throw new Exception("激活码已经过期!");
            }
            //user.EndDate = (DateTime.Now + TimeSpan.FromDays(31)).ToString();
            if(activeCode.GetCodeType() == CodeType.Normal)
            {
                user.LastBuyDate = DateTime.Now.ToString();
                ///如果 过期了就从现在开始计算时间，如果 没过期就从没过期的 时间 开始计算 
                user.EndDate = (user.EndDate.ToDateTime() < DateTime.Now ? DateTime.Now + TimeSpan.FromDays(activeCode.ValidityDays)
                    : user.EndDate.ToDateTime() + TimeSpan.FromDays(activeCode.ValidityDays)).ToString();
            }
            else if(activeCode.GetCodeType() == CodeType.Active)
            {
                user.EndDate = (DateTime.Now + TimeSpan.FromDays(activeCode.ValidityDays)).ToString();
                user.UnKnown2 = "used";
            }
            else if(activeCode.GetCodeType() == CodeType.Recommend)
            {
                if (!isregisteruser) return false;
                user.EndDate = (DateTime.Now + TimeSpan.FromDays(activeCode.ValidityDays)).ToString();
                user.UnKnown2 = "used";
                if(activeCode.BuyAccount != null && !activeCode.BuyAccount.Equals(""))
                {
                    User buyuser = GetUser(activeCode.BuyAccount);
                    ///如果 过期了就从现在开始计算时间，如果 没过期就从没过期的 时间 开始计算 
                    buyuser.EndDate = (buyuser.EndDate.ToDateTime() < DateTime.Now ? DateTime.Now + 
                        TimeSpan.FromDays(activeCode.ValidityDays)
                        : buyuser.EndDate.ToDateTime() + TimeSpan.FromDays(activeCode.ValidityDays)).ToString();
                }
            }
            else
            {
                throw new Exception("激活码类型错误!");
            }
            activeCode.IsUsed = true.ToString();
            activeCode.ByUser = account;
            this.DataBaseContext.SaveChanges();
            return true;
        }
        /// <summary>
        /// 创建一个激活码
        /// </summary>
        /// <param name="codeType">激活码类型</param>
        /// <param name="days">激活码激活的时长</param>
        /// <param name="buyaccount">购买激活码的用户</param>
        /// <returns></returns>
        public ActiveCode CreateActiveCode(CodeType codeType, int days, string buyaccount)
        {
            ActiveCode activeCode = new ActiveCode()
            {
                Code = MD5Comm.Get32MD5Two(Guid.NewGuid().ToString() + DateTime.Now.ToString()),
                CreateDate = DateTime.Now.ToString(),
                OverDate = (DateTime.Now + TimeSpan.FromDays(7)).ToString(),
                IsUsed = false.ToString(),
                CodeType = (int)codeType,
                ValidityDays = days,
                BuyAccount = buyaccount,
            };
            this.DataBaseContext.ActiveCodes.Add(activeCode);
            this.DataBaseContext.SaveChanges();
            return activeCode;
        }
        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            var list = DataBaseContext.Users.Where(x => x.Account.Equals(user.Account));
            if (list.Count() != 0)
            {
                return false;
            }
            this.DataBaseContext.Add<User>(user);
            this.DataBaseContext.SaveChanges();
            return true;
        }

        public bool BuyVIP(User user, int day)
        {
            //按每日计算会员需要多少钱
            if (user.Balance < day * 1.0)
                return false;
            user.Balance -= day * 1.0;
            user.LastBuyDate = DateTime.Now.ToString();
            user.EndDate = (DateTime.Now + TimeSpan.FromDays(day)).ToString();
            this.DataBaseContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public User GetUser(string account)
        {
            var user = this.DataBaseContext.Users.Where(x => x.Account == account);
            return user.Count() == 0 ? null : user.First();
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            return this.DataBaseContext.Users.ToList();
        }
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool IsLogion(string account)
        {
            return !string.IsNullOrEmpty(session.GetValue<string>(account));
        }
        /// <summary>
        /// 是否是同一个地方的登录信息
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="uuid">地方信息</param>
        /// <returns></returns>
        public bool IsSingleLoginInfo(string account, string uuid)
        {
            return IsLogion(account) && session.GetValue<string>(account).Equals(uuid);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool Login(string account, string password, string uuid)
        {
            if (setLoginInfo(account, password))
            {
                session.SetValue(account, uuid);
                return true;
            }
            return false;
        }

        public bool Recharge(User user, string code, double money)
        {
            //支付信息
            user.Balance += money;
            this.DataBaseContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 设置登录信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected bool setLoginInfo(string account, string password)
        {
            var user = GetUser(account);
            if (user.Password.Equals(password))
            {
                user.LastLoginDate = DateTime.Now.ToString();
                this.DataBaseContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 激活体验
        /// </summary>
        /// <param name="account"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public bool ExperienceCode(string account, string uuid)
        {
            User user = GetUser(account);
            if (user == null) throw new Exception("用户不存在!");
            if (user.UnKnown2 != null && user.UnKnown2.Equals("used")) throw new Exception("已经激活过了，不能重复激活!");
            else if (DataBaseContext.Users.Where(c => c.UnKnown2.Equals(uuid)).Count() != 0) throw new Exception("本设备已经被使用过激活!");
            else
            {
                user.UnKnown2 = uuid;
                //user.LastBuyDate = DateTime.Now.ToString();
                DateTime ed = DateTime.Now;
                try
                {
                    if (user.EndDate != null)
                        ed = Convert.ToDateTime(user.EndDate);
                }
                catch (Exception)
                {
                    ed = DateTime.Now;
                }
                user.EndDate = (ed + TimeSpan.FromDays(2)).ToString();
                this.DataBaseContext.SaveChanges();
            }
            return true;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        public bool RestPassword(User user, string newpassword)
        {
            user.Password = MD5Comm.Get32MD5One(newpassword);
            this.DataBaseContext.SaveChanges();
            return true;
        }
        /// <summary>
        /// 更新用户邮箱 信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        public void UpdateUserEmail(User user, string email)
        {
            user.Email = email;
            this.DataBaseContext.SaveChanges();
        }
    }
}
