using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ScriptServerStation.Attribute;
using ScriptServerStation.Database;
using ScriptServerStation.HelpClasses;
using ScriptServerStation.HelpClasses.Cache;
using ScriptServerStation.Items;
using ScriptServerStation.Utils;

namespace ScriptServerStation.Service.Impl
{
    [Interface]
    public class UserServiceImpl : IUserService
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public DataBaseContext DataBaseContext { get; set; }
        /// <summary>
        /// 激活账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code"></param>
        /// <param name="isregisteruser"></param>
        /// <returns></returns>
        public bool ActiveAccount(string account, string code, bool isregisteruser)
        {
            User user = GetUser(account);
            var list = DataBaseContext.Activatecode.Where(c =>
            c.Code.Equals(code) && !Convert.ToBoolean(c.IsUsed));
            if(list.Count() == 0)
            {
                throw new Exception("激活码错误!");
            }
            Activatecode Activatecode = list.First();
            if(Convert.ToDateTime(Activatecode.OverDate) < DateTime.Now)
            {
                throw new Exception("激活码已经过期!");
            }
            //user.EndDate = (DateTime.Now + TimeSpan.FromDays(31)).ToString();
            if(Activatecode.CodeType.EqualsType<CodeType>(CodeType.Normal))
            {
                user.LastBuyDate = DateTime.Now;
                ///如果 过期了就从现在开始计算时间，如果 没过期就从没过期的 时间 开始计算 
                user.EndDate = (user.EndDate < DateTime.Now ? DateTime.Now + TimeSpan.FromDays(Activatecode.ValidityDays)
                    : user.EndDate + TimeSpan.FromDays(Activatecode.ValidityDays));
            }
            else if(Activatecode.CodeType.EqualsType<CodeType>(CodeType.Activate))
            {
                user.EndDate = DateTime.Now + TimeSpan.FromDays(Activatecode.ValidityDays);
                user.IsActivated = true;
            }
            else if(Activatecode.CodeType.EqualsType<CodeType>(CodeType.Recommend))
            {
                if (!isregisteruser) return false;
                user.EndDate = DateTime.Now + TimeSpan.FromDays(Activatecode.ValidityDays);
                user.IsActivated = true;
                if(Activatecode.BuyAccount != null && !Activatecode.BuyAccount.Equals(""))
                {
                    User buyuser = GetUser(Activatecode.BuyAccount);
                    ///如果 过期了就从现在开始计算时间，如果 没过期就从没过期的 时间 开始计算 
                    buyuser.EndDate = buyuser.EndDate < DateTime.Now ? DateTime.Now + 
                        TimeSpan.FromDays(Activatecode.ValidityDays)
                        : buyuser.EndDate+ TimeSpan.FromDays(Activatecode.ValidityDays);
                }
            }
            else
            {
                throw new Exception("激活码类型错误!");
            }
            Activatecode.IsUsed = true;
            Activatecode.ByUser = user.Guid;
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
        public Activatecode CreateActivatecode(CodeType codeType, int days, string buyaccount)
        {
            Activatecode Activatecode = new Activatecode()
            {
                Code = MD5Comm.Get32MD5One(Guid.NewGuid().ToString() + DateTime.Now.ToString()),
                CreateDate = DateTime.Now,
                OverDate = DateTime.Now + TimeSpan.FromDays(7),
                IsUsed = false,
                CodeType = (int)codeType,
                ValidityDays = days,
                BuyAccount = buyaccount,
            };
            this.DataBaseContext.Activatecode.Add(Activatecode);
            this.DataBaseContext.SaveChanges();
            return Activatecode;
        }
        /// <summary>
        /// 追加时间
        /// </summary>
        /// <param name="isContain">是否包含非会员</param>
        /// <param name="time">增加的时间天数</param>
        /// <returns></returns>
        public bool AddTimeForEveryOne(bool isContain, int time)
        {
            var users = from p in this.DataBaseContext.User select p;
            users.Foreach<User>(u => {
                if(u.EndDate > DateTime.Now)
                {
                    u.EndDate = u.EndDate + TimeSpan.FromDays(time);
                }
                else if(isContain)
                {
                    u.EndDate = DateTime.Now + TimeSpan.FromDays(time);
                }
                return true;
            });
            this.DataBaseContext.SaveChanges();
            return true;
        }
        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            var list = DataBaseContext.User.Where(x => x.Name.Equals(user.Name));
            if (list.Count() != 0)
            {
                return false;
            }
            this.DataBaseContext.Add<User>(user);
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
            var user = this.DataBaseContext.User.Where(x => x.Name == account);
            return user.Count() == 0 ? null : user.First();
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            return (from u in this.DataBaseContext.User select u).ToList();
        }
        /// <summary>
        /// 判断密码是否正确
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool CheckPassword(string username, string password)
        {
            var user = GetUser(username);
            if (user.Password.Equals(password))
            {
                user.LastLoginDate = DateTime.Now;
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
        /// <returns></returns>
        public bool ExperienceCode(string account)
        {
            User user = GetUser(account);
            if (user == null) throw new Exception("用户不存在!");
            if (user.IsActivated) throw new Exception("已经激活过了，不能重复激活!");
            else
            {
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
                user.EndDate = ed + TimeSpan.FromDays(2);
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
