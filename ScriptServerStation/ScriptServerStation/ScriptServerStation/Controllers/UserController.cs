﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataBaseController;
using DataBaseController.Entitys;
using ScriptServerStation.HelpClasses;
using ScriptServerStation.Service;
using System.IO;
using CoreHelper.Expends;
using System.Net.Http;
using Newtonsoft.Json;
using ScriptServerStation.HelpClasses.Cache;

namespace ScriptServerStation.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/User")]
    public class UserController : Controller
    {
        /// <summary>
        /// service
        /// </summary>
        private IUserService userService;
        /// <summary>
        /// 识别
        /// </summary>
        private ITextDetectionService textDetectionService;
        /// <summary>
        /// 缓存
        /// </summary>
        private ICacheOption cacheOption;
        /// <summary>
        /// 邮件服务
        /// </summary>
        private IEmailConnect emailConnect;
        /// <summary>
        /// 日志
        /// </summary>
        private ILog log;

        private HttpClient HttpClient = new HttpClient();
        private string url = "http://127.0.0.1:8000/";
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService, ITextDetectionService textDetectionService
            , ICacheOption cacheOption, IEmailConnect emailConnect, ILog log)
        {
            this.userService = userService;
            this.textDetectionService = textDetectionService;
            this.cacheOption = cacheOption;
            this.emailConnect = emailConnect;
            this.log = log;
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
        public ReturnObj Login(string account, string uuid, string password)
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                Dictionary<string, string> resultcontents = new Dictionary<string, string>();
                if (uuid.Length < 10)
                    throw new Exception("非正常加密的uuid!");
                User user = userService.GetUser(account);
                returnObj.SetIsSuccess(userService.Login(account, MD5Comm.Get32MD5One(password), uuid));
                resultcontents.Add("isSpecial", (Convert.ToBoolean(user.IsSpecial) || ishaveroot(user)).ToString());
                resultcontents.Add("time", user.IsSpecial.ToBool() ? (
                    DateTime.Now + TimeSpan.FromDays(30)).ToString() : (user.EndDate == null || 
                    user.EndDate.Equals("") ? "未开通会员" : user.EndDate));
                resultcontents.Add("isShowMsg", (user.Email == null || user.Email.Equals("")).ToString());
                resultcontents.Add("msg", "检测到您还没绑定邮箱，请有空尽快去绑定一下邮箱，群里有修改邮箱地址，" +
                    "绑定邮箱之后可以通过邮箱找回密码!");
                returnObj.Result = resultcontents;
            }
            catch (Exception ex)
            {
                returnObj.SetIsSuccess(false);
                returnObj.ErrorMsg = ex.Message;
            }
            return returnObj;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="verification"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public ReturnObj Register(string account, string password, string code, string email, string recommend)
        {
            ReturnObj obj = new ReturnObj();
            try
            {
                string value = cacheOption.GetValue<string>("register_{0}".ToFormat(account));
                if (string.IsNullOrEmpty(value)) throw new Exception("该账号没有被发送过邮箱验证,请在发送邮箱验证之后不要" +
                      "更改填写的账号!");
                if (!value.Equals(code)) throw new Exception("验证码错误,请检查发送到指定邮箱的验证码!");

                User user = new User() {
                    Account = account,
                    Guid = Guid.NewGuid().ToString(),
                    Password = MD5Comm.Get32MD5One(password),
                    Email = email,
                    //Phone = phone,
                    Level = 0,
                    LevelValue = 0,
                    Balance = 0,
                    CreateTime = DateTime.Now.ToString(),
                    IsSpecial = false.ToString(),
                };
                
                obj.SetIsSuccess(userService.AddUser(user, verification, HttpContext.Session));
                try
                {
                    userService.ActiveAccount(user.Account, recommend, true);
                }
                catch (Exception)
            {
                obj.SetIsSuccess(false);
            }
            return obj;
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("getverification")]
        public ReturnObj GetVerification(string email)
        {
            ReturnObj obj = new ReturnObj();
            try
            {
                obj.SetIsSuccess(userService.GetVerification(email, HttpContext.Session));
            }
            catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
                obj.ErrorMsg = ex.Message;
                obj.SetIsSuccess(false);
            }
            return obj;
        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        [HttpPost("sendcode")]
        public ReturnObj SendVerificationCode(string account, string email)
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                Random random = new Random();
                if (userService.GetUser(account) != null) throw new Exception("该账号已经被注册!");
                string code = random.Next(0, 9).ToString() + random.Next(0, 9).ToString()
                    + random.Next(0, 9).ToString() + random.Next(0, 9).ToString();
                cacheOption.SetValue("register_{0}".ToFormat(account), code, new DateTimeOffset(
                    DateTime.Now + TimeSpan.FromHours(1)));
                emailConnect.SendEmail("火星人脚本账号注册", "验证码:{0}".ToFormat(code), email);
                returnObj.SetIsSuccess(true);
                returnObj.Result = "已经成功发送验证码,验证码只有一小时的有效期!";
            }
            catch (Exception ex)
            {
                returnObj.ErrorMsg = ex.Message;
                returnObj.SetIsSuccess(false);
            }
            return returnObj;
        }
        /// <summary>
        /// 买会员
        /// </summary>
        /// <param name="account"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpPost("BuyVIP")]
        public ReturnObj BuyVIP(string account, int day)
        {
            ReturnObj obj = new ReturnObj();
            try
            {
                User user = userService.GetUser(account);
                obj.SetIsSuccess(userService.BuyVIP(user,day));
            }
            catch (Exception)
            {
                obj.SetIsSuccess(false);
            }
            return obj;
        }
        /// <summary>
        /// 激活账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("activeaccount")]
        public ReturnObj ActiveAccount(string account, string code)
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                User user = userService.GetUser(account);
                if(user == null)
                {
                    throw new Exception("用户名错误不存在!");
                }
                if(userService.ActiveAccount(account, code, false))
                {
                    returnObj.SetIsSuccess(true);
                    returnObj.Result = "激活成功!";
                }
                else
                {
                    throw new Exception("激活失败，激活码错误！");
                }
            }
            catch (Exception ex)
            {
                returnObj.Result = ex.Message;
                returnObj.SetIsSuccess(false);
            }
            return returnObj;
        }
        /// <summary>
        /// 激活体验期
        /// </summary>
        /// <param name="account">要激活体验的账号</param>
        /// <param name="uuid">激活的设备号</param>
        /// <returns></returns>
        [HttpPost("experience")]
        public ReturnObj ExperienceCode(string account, string uuid)
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                returnObj.SetIsSuccess(userService.ExperienceCode(account, uuid));
                returnObj.Result = "激活成功!";
            }
            catch (Exception ex)
            {
                returnObj.SetIsSuccess(false);
                returnObj.ErrorMsg = "激活失败!" + ex.Message;
            }

            return returnObj;
        }
        /// <summary>
        ///  临时接口
        /// </summary>
        /// <returns></returns>
        [HttpPost("temporary")]
        public ReturnObj TemporaryInterface()
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                List<ActiveCode> activeCodes = new List<ActiveCode>();
                returnObj.SetIsSuccess(true);
                List<User> users = userService.GetUsers();
                users.ForEach(c =>
                {
                    ///检测是否拥有权限
                    if (ishaveroot(c))
                    {
                        if(c.EndDate.ToDateTime() > DateTime.Now && 
                        c.EndDate.ToDateTime() < (DateTime.Now + TimeSpan.FromDays(40)))
                        {
                            activeCodes.Add(userService.CreateActiveCode(CodeType.Recommend, 7, c.Account));
                        }
                        else if(c.EndDate.ToDateTime() > (DateTime.Now + TimeSpan.FromDays(40)))
                        {
                            activeCodes.Add(userService.CreateActiveCode(CodeType.Recommend, 22, c.Account));
                        }
                    }
                });

                activeCodes.ForEach(c => {
                    returnObj.Result += string.Format("{0}:{1} <br/>",
                        c.BuyAccount, c.Code);
                });
            }
            catch (Exception ex)
            {
                returnObj.SetIsSuccess(false);
            }
            return returnObj;
        }
        /// <summary>
        /// 创建激活码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("createactivecode")]
        public ReturnObj CreateActiveCode(string account, string password, int type, string buyaccount)
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                ///如果 购买者不填写就默认没有
                if (userService.GetUser(buyaccount == null ? "" : buyaccount) == null)
                {
                    buyaccount = "";
                }
                int days = 0;
                switch (type)
                {
                    case 0:
                        days = 7;
                        break;
                    case 1:
                        days = 31;
                        break;
                    case 2:
                        days = 93;
                        break;
                    case 3:
                        days = 186;
                        break;
                    default:
                        break;
                }

                User user = userService.GetUser(account);
                if(user == null)
                {
                    throw new Exception("用户名或者密码不正确!");
                }
                if(user.Password.Equals(MD5Comm.Get32MD5One(password))
                    && user.UnKnown1.Equals("admin"))
                {
                    if(type == 4)
                    {
                        ActiveCode recommedn = userService.CreateActiveCode(CodeType.Active, 7, buyaccount);
                        returnObj.Result = string.Format("7天活动码:{0},",
                            recommedn.Code);
                    }
                    else
                    {
                        ActiveCode activeCode = userService.CreateActiveCode(CodeType.Normal, days, buyaccount);
                        ActiveCode recommedn = null;
                        if (type >= 1 && !buyaccount.Equals(""))
                            recommedn = userService.CreateActiveCode(CodeType.Recommend, (int)(days * 0.15), buyaccount);
                        returnObj.Result = string.Format("激活码:{0}," +
                            "<br/>赠送的推荐码:{1}", activeCode.Code, recommedn == null ? "没有推荐码" : recommedn.Code);
                    }
                    returnObj.SetIsSuccess(true);
                }
                else
                {
                    throw new Exception("用户名或者密码不正确!");
                }
            }
            catch (Exception ex)
            {
                returnObj.SetIsSuccess(false);
                returnObj.Result = ex.Message;
            }
            return returnObj;
        }
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        [HttpPost("Recharge")]
        public ReturnObj Recharge(string account, string code, double money)
        {
            ReturnObj obj = new ReturnObj();
            try
            {
                User user = userService.GetUser(account);
                obj.SetIsSuccess(userService.Recharge(user, code, money));
            }
            catch (Exception)
            {
                obj.SetIsSuccess(false);
            }
            return obj;
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        [HttpPost("restpassword")]
        public ReturnObj RestPassword(string account)
        {
            ReturnObj returnObj = new ReturnObj();
            returnObj.SetIsSuccess(false);
            try
            {
                Random random = new Random();
                User user = userService.GetUser(account);
                if (user == null) throw new Exception("用户名不存在!");
                string newpassword = random.Next(0, 9).ToString() + random.Next(0, 9).ToString()
                    + random.Next(0, 9).ToString() + random.Next(0, 9).ToString();
                emailConnect.SendEmail("重置密码!", "新密码为:" + newpassword, user.Email);
                userService.RestPassword(user, newpassword);
                returnObj.SetIsSuccess(true);
                returnObj.Result = "重置密码成功,打开自己的邮箱查看新密码!";
            }
            catch (Exception ex)
            {
                returnObj.SetIsSuccess(false);
                returnObj.ErrorMsg = ex.Message;
            }

            return returnObj;
        }
        /// <summary>
        /// 账号补全
        /// </summary>
        /// <returns></returns>
        [HttpPost("accountcompletion")]
        public ReturnObj AccountCompletion(string account, string password, string email)
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                User user = userService.GetUser(account);
                if (!user.Password.Equals(MD5Comm.Get32MD5One(password))) throw new Exception("账号或密码不正确!");
                emailConnect.SendEmail("火星人脚本绑定邮箱提示!", "恭喜你绑定邮箱成功!", email);
                userService.UpdateUserEmail(user, email);
                returnObj.SetIsSuccess(true);
            }
            catch (Exception ex)
            {
                returnObj.SetIsSuccess(false);
                returnObj.ErrorMsg = ex.Message;
            }
            return returnObj;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="oldpassword">旧账号</param>
        /// <param name="newpassword">新账号</param>
        /// <returns></returns>
        [HttpPost("modifypassword")]
        public ReturnObj ModifyPassword(string account, string oldpassword, string newpassword)
        {
            ReturnObj returnObj = new ReturnObj();
            returnObj.SetIsSuccess(false);
            try
            {
                User user = userService.GetUser(account);
                if (user == null || !user.Password.Equals(MD5Comm.Get32MD5One(oldpassword))) throw new Exception("用户名或密码不正确!");
                emailConnect.SendEmail("警告!", "来自火星人的警告，你的火星人脚本密码已经被修改，" +
                    "若非本人操作请及时连续群主!", user.Email);
                userService.RestPassword(user, newpassword);
                returnObj.SetIsSuccess(true);
                returnObj.Result = "修改密码成功!";
            }
            catch (Exception ex)
            {
                returnObj.SetIsSuccess(false);
                returnObj.ErrorMsg = ex.Message;
            }
            return returnObj;
        }
        /// <summary>
        /// 答题检测
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("checkimage")]
        public ReturnObj CheckImage(string param)
        {
            string account = param.Split('&')[0].Split('=')[1];
            string uuid = System.Net.WebUtility.UrlDecode(param.Split('&')[1].Split('=')[1]);
            ReturnObj returnObj = new ReturnObj();
            try
            {
                if (uuid.Length < 10)
                    throw new Exception("非正常加密的uuid!");
                User user = userService.GetUser(account);
                if (!userService.IsSingleLoginInfo(user.Account, uuid))
                {
                    throw new Exception("同一账号不同地点登录，请检测账号是否被盗取,或者尽快修改密码!");
                }
                if(Convert.ToBoolean(user.IsSpecial) == false &&
                    ishaveroot(user) == false)
                {
                    throw new Exception("权限不足!");
                }
                else
                {
                    string rootdir = AppContext.BaseDirectory + "//FileCheck//" + DateTime.Now.ToString("yyyy-MM-dd") + "//";
                    if (Directory.Exists(rootdir) == false)
                    {
                        Directory.CreateDirectory(rootdir);
                    }
                    List<ImageItem> imagedatas = new List<ImageItem>();
                    int idx = 0;
                    byte[] datas = null;
                    ///获取图片信息
                    foreach (var item in HttpContext.Request.Form.Files)
                    {
                        if(idx++ == 0)
                        {
                            using (MemoryStream stream = new MemoryStream())
                            {
                                item.CopyTo(stream);
                                datas = stream.ToArray();
                            }
                        }
                        else
                        {
                            string id = Guid.NewGuid().ToString().Replace("-", "_") + ".jpg";
                            id = Path.Combine(rootdir, id);
                            using (FileStream stream = System.IO.File.Open(id, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            imagedatas.Add(new ImageItem(id, ""));
                        }
                    }
                    if(imagedatas.Count != 8 || datas == null)
                    {
                        throw new Exception("请求的图片参数错误!");
                    }
                    else
                    {
                        string text = textDetectionService.TextDetection(datas);
                        foreach (var item in imagedatas)
                        {
                            //item.FileValue = textDetectionService.ImageDetection(item.File).ToString();
                            item.FileValue = HttpClient.GetStringAsync(url + item.FileName).Result;
                        }
                        string value = checkimagecode(text).ToString();
                        bool iselse = text.IndexOf("非") == -1;

                        for (int i = 0; i < imagedatas.Count; i++)
                        {
                            if(imagedatas[i].FileValue.Equals(value) == iselse)
                            {
                                imagedatas[i].FileValue = "true";
                            }
                            else
                            {
                                imagedatas[i].FileValue = "false";
                            }
                            imagedatas[i].FileName = i.ToString();
                        }
                        returnObj.SetIsSuccess(true);
                        returnObj.Result = imagedatas;
                    }
                }
            }
            catch (Exception ex)
            {
                returnObj.SetIsSuccess(false);
                returnObj.ErrorMsg = ex.Message;
                log.Log(ex.ToString());
            }
            return returnObj;
        }
        /// <summary>
        /// 符文检测
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("checkfw")]
        public ReturnObj CheckFw(string param)
        {
            string account = param.Split('&')[0].Split('=')[1];
            string uuid = System.Net.WebUtility.UrlDecode(param.Split('&')[1].Split('=')[1]);
            ReturnObj returnObj = new ReturnObj();
            try
            {
                if (uuid.Length < 10)
                    throw new Exception("非正常加密的uuid!");
                User user = userService.GetUser(account);
                if (!userService.IsSingleLoginInfo(user.Account, uuid))
                {
                    throw new Exception("同一账号不同地点登录，请检测账号是否被盗取,或者尽快修改密码!");
                }
                if (Convert.ToBoolean(user.IsSpecial) == false
                    && ishaveroot(user) == false)
                {
                    throw new Exception("权限不足!");
                }
                else
                {
                    List<byte[]> datas = new List<byte[]>();
                    foreach (var item in HttpContext.Request.Form.Files)
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            item.CopyTo(stream);
                            datas.Add(stream.ToArray());
                            stream.Dispose();
                        }
                    }

                    List<string> msgdatas = new List<string>();
                    datas.ForEach(c => msgdatas.AddRange(textDetectionService.TextDetections(c)));
                    returnObj.Result = msgdatas;
                }
                returnObj.SetIsSuccess(true);
            }
            catch(Exception e)
            {
                returnObj.SetIsSuccess(false);
                returnObj.ErrorMsg = e.Message;
                log.Log(e.ToString());
            }
            return returnObj;
        }
        /// <summary>
        /// 设置单个服务的ip等待获取
        /// </summary>
        /// <param name="username"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("setserverip")]
        public ReturnObj SetSingleServerIp(string username, string ip)
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                cacheOption.SetValue(username, ip, new DateTimeOffset(DateTime.Now + TimeSpan.FromDays(1)));
                returnObj.SetIsSuccess(true);
            }
            catch (Exception e)
            {
                returnObj.SetIsSuccess(false);
                returnObj.ErrorMsg = e.Message;
            }
            return returnObj;
        }
        /// <summary>
        /// 获取单个服务的ip和端口
        /// </summary>
        /// <param name="username">用户id</param>
        /// <returns></returns>
        [HttpGet("getserverip")]
        public ReturnObj GetSingleServerIp(string username)
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                if (cacheOption.Exists(username))
                {
                    returnObj.Result = cacheOption.GetValue<string>(username);
                }
                returnObj.SetIsSuccess(true);
            }
            catch (Exception e)
            {
                returnObj.SetIsSuccess(false);
                returnObj.ErrorMsg = e.Message;
            }
            return returnObj;
        }
        /// <summary>
        /// 检测所有需要被点击的序号
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private int checkimagecode(string msg)
        {
            if(msg.IndexOf("暗") != -1)
            {
                return 0;
            }
            else if (msg.IndexOf("火") != -1)
            {
                return 1;
            }
            else if (msg.IndexOf("光") != -1)
            {
                return 2;
            }
            else if (msg.IndexOf("水") != -1)
            {
                return 3;
            }
            else if (msg.IndexOf("风") != -1)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
        /// <summary>
        /// 检测用户是否还拥有vip权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool ishaveroot(User user)
        {
            try
            {
                return Convert.ToDateTime(user.EndDate) > DateTime.Now;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 图片
        /// </summary>
        protected class ImageItem
        {
            //[JsonIgnore]
            public string FileName { get; set; }
            public string FileValue { get; set; }

            public ImageItem(string n, string v)
            {
                this.FileName = n;
                this.FileValue = v;
            }
        }
    }
}
