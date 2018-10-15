using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBaseController;
using DataBaseController.Entitys;
using Microsoft.AspNetCore.Http;

namespace ScriptServerStation.Service.Impl
{
    public class UserServiceImpl : BaseService, IUserService
    {

        public UserServiceImpl(DataBaseContext DataBaseContext) : base(DataBaseContext)
        {
        }
        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
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
            user.LastBuyDate = DateTime.Now;
            user.EndDate = DateTime.Now + TimeSpan.FromDays(day);
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
        public bool IsLogion(string account, ISession session)
        {
            return !string.IsNullOrEmpty(session.GetString(account));
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool Login(string account, string password, ISession session)
        {
            if (setLoginInfo(account, password))
            {
                session.SetString(account, DateTime.Now.ToString());
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
                user.LastLogionDate = DateTime.Now;
                this.DataBaseContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
