using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreHelper;
using DataBaseController;
using DataBaseController.Entitys;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ScriptServerStation.Service.Impl
{
    public class UserServiceImpl : BaseService, IUserService, IProfileService
    {

        private EmailSettings ConfigSettings { get; set; }
        public UserServiceImpl(DataBaseContext DataBaseContext, IOptions<EmailSettings> settings) : base(DataBaseContext)
        {
            ConfigSettings = settings.Value;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                var claims = context.Subject.Claims.ToList();

                //set issued claims to return
                context.IssuedClaims = claims.ToList();
            }
            catch (Exception ex)
            {
                //log your error
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(User user, string verification, ISession session)
        {
            if (session.GetString(user.Email) != verification)
                return false;
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
            user.LastBuyDate = TimeProvider.Current.Now.ToString();
            user.EndDate = (TimeProvider.Current.Now + TimeSpan.FromDays(day)).ToString();
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
                session.SetString(account, TimeProvider.Current.Now.ToString());
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

        public bool GetVerification(string email, ISession session)
        {
            string verification = GetVerification(4);
            session.SetString(email, verification);
            EmailHelper mail = new EmailHelper(ConfigSettings, email, "测试邮件2", "<html><body><div style='color:red;'>验证码为：" + verification + "</div></body></html>");
            mail.Send();
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
                user.LastLoginDate = TimeProvider.Current.Now.ToString();
                this.DataBaseContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 随机生成验证码
        /// </summary>
        /// <param name="count">几位验证码</param>
        /// <returns></returns>
        private string GetVerification(int count)
        {
            int number;
            string checkCode = String.Empty;     //存放随机码的字符串   

            System.Random random = new Random();

            for (int i = 0; i < count; i++) //产生4位校验码   
            {
                number = random.Next();
                number = number % 36;
                if (number < 10)
                {
                    number += 48;    //数字0-9编码在48-57   
                }
                else
                {
                    number += 55;    //字母A-Z编码在65-90   
                }

                checkCode += ((char)number).ToString();
            }
            return checkCode;
        }
    }
}
