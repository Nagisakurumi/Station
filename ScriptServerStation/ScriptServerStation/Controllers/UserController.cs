using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScriptServerStation.HelpClasses;
using ScriptServerStation.Service;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using ScriptServerStation.HelpClasses.Cache;
using ScriptServerStation.Items;
using ScriptServerStation.Database;
using ScriptServerStation.Utils;
using ScriptServerStation.HelpClasses.Cache.Configuration;
using Microsoft.AspNetCore.Http;

namespace ScriptServerStation.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// service
        /// </summary>
        public IUserService UserService { get; set; }
        /// <summary>
        /// 识别
        /// </summary>
        public ITextDetectionService TextDetectionService { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>
        public IMemoryCache CacheOption { get; set; }
        /// <summary>
        /// 邮件服务
        /// </summary>
        public IEmailConnect EmailConnect { get; set; }
        /// <summary>
        /// 日志
        /// </summary>
        public ILog Log { get; set; }
        /// <summary>
        /// 缓存名
        /// </summary>
        public CacheKey CacheKey { get; set; }

        private HttpClient HttpClient = new HttpClient();
        private string url = "http://127.0.0.1:8000/";

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("getuserlist")]
        public Result GetUserList()
        {
            try
            {
                return Result.Success("请求成功", UserService.GetUsers());
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("userinfo")]
        public Result GetUserInfo()
        {
            try
            {
                return Result.Success(this.CacheOption.GetValue<User>(this.GetUserToken()));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Result.Fail(ex.Message);
            }
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet("login/{username}/{password}")]
        public Result Login(string username, string password)
        {
            Result Result = new Result();
            try
            {
                User user = UserService.GetUser(username);
                if(!UserService.CheckPassword(username, MD5Comm.Get32MD5One(password)))
                {
                    return Result.Fail("账号或密码错误!");
                }
                //计算cookie的值
                string cookieValue = MD5Comm.Get32MD5One(DateTime.Now.ToString());

                Console.WriteLine("用户 : " + user.Name + ", token : " + cookieValue);
                //存入缓存
                CacheOption.SetValue(cookieValue, user, DateTime.Now + TimeSpan.FromDays(100));
                //设置用户token
                this.SetToken(cookieValue);
                Result.Data = new { IsSpecial = user.IsHasSpecialPower() || ishaveroot(user),
                    Time = user.IsHasSpecialPower() ? (
                    DateTime.Now + TimeSpan.FromDays(30)).ToString() : (user.EndDate == null || user.EndDate < DateTime.Now ? "未开通会员" : user.EndDate.ToString()),
                    IsShowMsg = (user.Email == null || user.Email.Equals("")).ToString(),
                    Msg = "检测到您还没绑定邮箱，请有空尽快去绑定一下邮箱，群里有修改邮箱地址，" +
                    "绑定邮箱之后可以通过邮箱找回密码!",
                    IsLogin = true,
                    UserName = user.Name,
                    EndTime = user.EndDate == null ? "" : user.EndDate.ToString()
                };
            }
            catch (Exception ex)
            {
                Result.SetCode( ResultCode.Fail);
                Result.Msg = ex.Message;
            }
            return Result;
        }
        /// <summary>
        /// 注册
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public Result Register(string account, string password, string code, string email, string recommend)
        {
            Result obj = new Result();
            try
            {
                string value = CacheOption.GetValue<string>("register_{0}".ToFormat(account));
                if (string.IsNullOrEmpty(value)) throw new Exception("该账号没有被发送过邮箱验证,请在发送邮箱验证之后不要" +
                      "更改填写的账号!");
                if (!value.Equals(code)) throw new Exception("验证码错误,请检查发送到指定邮箱的验证码!");

                User user = new User() {
                    Name = account,
                    Password = MD5Comm.Get32MD5One(password),
                    Email = email,
                    Level = 0,
                    CreateTime = DateTime.Now,
                };
                
                obj.SetCode(UserService.AddUser(user) ? ResultCode.Success : ResultCode.Fail);
                try
                {
                    UserService.ActiveAccount(user.Name, recommend, true);
                }
                catch (Exception)
                {

                }
            }
            catch (Exception ex)
            {
                obj.Msg = ex.Message;
                obj.SetCode( ResultCode.Fail);
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
        public Result SendVerificationCode(string account, string email)
        {
            Result Result = new Result();
            try
            {
                Random random = new Random();
                if (UserService.GetUser(account) != null) throw new Exception("该账号已经被注册!");
                string code = random.Next(0, 9).ToString() + random.Next(0, 9).ToString()
                    + random.Next(0, 9).ToString() + random.Next(0, 9).ToString();
                CacheOption.SetValue("register_{0}".ToFormat(account), code, 
                    DateTime.Now + TimeSpan.FromHours(1));
                EmailConnect.SendEmail("火星人脚本账号注册", "验证码:{0}".ToFormat(code), email);
                Result.Data = "已经成功发送验证码,验证码只有一小时的有效期!";
            }
            catch (Exception ex)
            {
                Result.Msg = ex.Message;
                Result.SetCode( ResultCode.Fail);
            }
            return Result;
        }
        /// <summary>
        /// 激活账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("activeaccount")]
        public Result ActiveAccount(string account, string code)
        {
            Result Result = new Result();
            try
            {
                User user = UserService.GetUser(account);
                if(user == null)
                {
                    throw new Exception("用户名错误不存在!");
                }
                if(UserService.ActiveAccount(account, code, false))
                {
                    Result.Data = "激活成功!";
                }
                else
                {
                    throw new Exception("激活失败，激活码错误！");
                }
            }
            catch (Exception ex)
            {
                Result.Msg = ex.Message;
                Result.SetFail();
            }
            return Result;
        }
        /// <summary>
        /// 激活体验期
        /// </summary>
        /// <returns></returns>
        [HttpPost("experience")]
        public Result ExperienceCode()
        {
            Result Result = new Result();
            try
            {
                if (!this.IsLogin())
                {
                    return Result.Fail("登陆以过期，或者被别人从其他地方顶掉了!尽快修改密码!");
                }
                User user = CacheOption.GetValue<User>(this.GetUserToken());
                Result.SetCode(UserService.ExperienceCode(user.Name) ? ResultCode.Success : ResultCode.Fail);
                Result.Data = "激活成功!";
            }
            catch (Exception ex)
            {
                Result.SetFail();
                Result.Msg = "激活失败!" + ex.Message;
            }

            return Result;
        }
        /// <summary>
        ///  临时接口
        /// </summary>
        /// <returns></returns>
        [HttpPost("temporary")]
        public Result TemporaryInterface()
        {
            Result Result = new Result();
            try
            {
                List<Activatecode> Activatecodes = new List<Activatecode>();
                List<User> users = UserService.GetUsers();
                users.ForEach(c =>
                {
                    ///检测是否拥有权限
                    if (ishaveroot(c))
                    {
                        if(c.EndDate > DateTime.Now && 
                        c.EndDate < (DateTime.Now + TimeSpan.FromDays(40)))
                        {
                            Activatecodes.Add(UserService.CreateActivatecode(CodeType.Recommend, 7, c.Name));
                        }
                        else if(c.EndDate > (DateTime.Now + TimeSpan.FromDays(40)))
                        {
                            Activatecodes.Add(UserService.CreateActivatecode(CodeType.Recommend, 22, c.Name));
                        }
                    }
                });

                Activatecodes.ForEach(c => {
                    Result.Data += string.Format("{0} <br/>",
                        c.Code);
                });
            }
            catch (Exception ex)
            {
                Result.SetFail();
                Result.Msg = ex.Message;
            }
            return Result;
        }
        /// <summary>
        /// 活动追加时间接口
        /// </summary>
        /// <returns></returns>
        [HttpPost("activateaddtime")]
        public Result ActivateAddTime(string account, string password, int time)
        {
            Result Result = new Result();
            try
            {
                User user = UserService.GetUser(account);
                if (user == null)
                {
                    throw new Exception("用户名或者密码不正确!");
                }
                if (user.Password.Equals(MD5Comm.Get32MD5One(password))
                    && user.IsHasAdminPower())
                {
                    UserService.AddTimeForEveryOne(true, time);
                }
                else
                {
                    throw new Exception("用户名或者密码不正确!");
                }
            }
            catch (Exception ex)
            {
                Result.SetFail();
            }
            return Result;
        }
        /// <summary>
        /// 创建激活码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("createActivatecode")]
        public Result CreateActivatecode(string account, string password, int type, string buyaccount)
        {
            Result Result = new Result();
            try
            {
                ///如果 购买者不填写就默认没有
                if (UserService.GetUser(buyaccount == null ? "" : buyaccount) == null)
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

                User user = UserService.GetUser(account);
                if(user == null)
                {
                    throw new Exception("用户名或者密码不正确!");
                }
                if(user.Password.Equals(MD5Comm.Get32MD5One(password))
                    && user.IsHasAdminPower())
                {
                    if(type == 4)
                    {
                        Activatecode recommedn = UserService.CreateActivatecode(CodeType.Activate, 7, buyaccount);
                        Result.Data = string.Format("7天活动码:{0},",
                            recommedn.Code);
                    }
                    else
                    {
                        Activatecode Activatecode = UserService.CreateActivatecode(CodeType.Normal, days, buyaccount);
                        Activatecode recommedn = null;
                        if (type >= 1 && !buyaccount.Equals(""))
                            recommedn = UserService.CreateActivatecode(CodeType.Recommend, (int)(days * 0.15), buyaccount);
                        Result.Data = string.Format("激活码:{0}," +
                            "<br/>赠送的推荐码:{1}", Activatecode.Code, recommedn == null ? "没有推荐码" : recommedn.Code);
                    }
                }
                else
                {
                    throw new Exception("用户名或者密码不正确!");
                }
            }
            catch (Exception ex)
            {
                Result.SetFail();
                Result.Msg = ex.Message;
            }
            return Result;
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        [HttpPost("restpassword")]
        public Result RestPassword(string account)
        {
            Result Result = new Result();
            try
            {
                Random random = new Random();
                User user = UserService.GetUser(account);
                if (user == null) throw new Exception("用户名不存在!");
                string newpassword = random.Next(0, 9).ToString() + random.Next(0, 9).ToString()
                    + random.Next(0, 9).ToString() + random.Next(0, 9).ToString();
                EmailConnect.SendEmail("重置密码!", "新密码为:" + newpassword, user.Email);
                UserService.RestPassword(user, newpassword);
                Result.Data = "重置密码成功,打开自己的邮箱查看新密码!";
            }
            catch (Exception ex)
            {
                Result.SetFail();
                Result.Msg = ex.Message;
            }

            return Result;
        }
        /// <summary>
        /// 账号补全
        /// </summary>
        /// <returns></returns>
        [HttpPost("accountcompletion")]
        public Result AccountCompletion(string account, string password, string email)
        {
            Result Result = new Result();
            try
            {
                User user = UserService.GetUser(account);
                if (!user.Password.Equals(MD5Comm.Get32MD5One(password))) throw new Exception("账号或密码不正确!");
                EmailConnect.SendEmail("火星人脚本绑定邮箱提示!", "恭喜你绑定邮箱成功!", email);
                UserService.UpdateUserEmail(user, email);
            }
            catch (Exception ex)
            {
                Result.SetFail();
                Result.Msg = ex.Message;
            }
            return Result;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="oldpassword">旧账号</param>
        /// <param name="newpassword">新账号</param>
        /// <returns></returns>
        [HttpPost("modifypassword")]
        public Result ModifyPassword(string account, string oldpassword, string newpassword)
        {
            Result Result = new Result();
            try
            {
                User user = UserService.GetUser(account);
                if (user == null || !user.Password.Equals(MD5Comm.Get32MD5One(oldpassword))) throw new Exception("用户名或密码不正确!");
                EmailConnect.SendEmail("警告!", "来自火星人的警告，你的火星人脚本密码已经被修改，" +
                    "若非本人操作请及时连续群主!", user.Email);
                UserService.RestPassword(user, newpassword);
                Result.Data = "修改密码成功!";
            }
            catch (Exception ex)
            {
                Result.SetFail();
                Result.Msg = ex.Message;
            }
            return Result;
        }
        /// <summary>
        /// 答题检测
        /// </summary>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        [HttpPost("checkimage")]
        public Result CheckImage([FromForm] IFormCollection formFiles)
        {
            Result Result = new Result();
            try
            {
                if (!this.IsLogin())
                {
                    return Result.Fail("登陆以过期，或者被别人从其他地方顶掉了!尽快修改密码!");
                }
                User user = CacheOption.GetValue<User>(this.GetUserToken());
                if(Convert.ToBoolean(user.IsHasSpecialPower()) == false &&
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
                    foreach (var item in ((FormCollection)formFiles).Files)
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
                            string id = idx + "----------------" + Guid.NewGuid().ToString().Replace("-", "_") + ".jpg";
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
                        string text = TextDetectionService.TextDetection(datas);
                        foreach (var item in imagedatas)
                        {
                            //item.FileValue = textDetectionService.ImageDetection(item.File).ToString();
                            item.FileValue = HttpClient.GetStringAsync(url + item.FileName).Result;
                        }
                        List<int> resultValues = checkimagecode(text);

                        for (int i = 0; i < imagedatas.Count; i++)
                        {
                            if(resultValues.Contains(Convert.ToInt32(imagedatas[i].FileValue)))
                            {
                                imagedatas[i].FileValue = "true";
                            }
                            else
                            {
                                imagedatas[i].FileValue = "false";
                            }
                            imagedatas[i].FileName = i.ToString();
                        }
                        Result.Data = imagedatas;
                    }
                }
            }
            catch (Exception ex)
            {
                Result.SetFail();
                Result.Msg = ex.Message;
                Log.Log(ex.ToString());
            }
            return Result;
        }
        /// <summary>
        /// 符文检测
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("checkfw")]
        public Result CheckFw()
        {
            Result Result = new Result();
            try
            {
                if (!this.IsLogin())
                {
                    return Result.Fail("登陆以过期，或者被别人从其他地方顶掉了!尽快修改密码!");
                }
                User user = CacheOption.GetValue<User>(this.GetUserToken(CacheKey.UserCacheName));
                if (user.IsHasSpecialPower() == false
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
                    datas.ForEach(c => msgdatas.AddRange(TextDetectionService.TextDetections(c)));
                    Result.Data = msgdatas;
                }
            }
            catch(Exception e)
            {
                Result.SetFail();
                Result.Msg = e.Message;
                Log.Error(e.ToString());
                try
                {
                    Log.Log("用户名 : {0}, Token : {1}".Format(CacheOption.GetValue<User>(this.GetUserToken()), this.GetUserToken()));
                }
                catch (Exception ex)
                {
                    Log.Error("获取信息异常 : ", ex);
                }
            }
            return Result;
        }
        /// <summary>
        /// 检测所有需要被点击的序号
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private List<int> checkimagecode(string msg)
        {
            List<int> result = new List<int>();
            bool isNegate = msg.Contains("非") || msg.Contains("不是");
            if (msg.Contains("艾琳"))
            {
                result.AddRange(isNegate ? new int[] { 0, 2, 3, 4, 5 } : new int[] { 1 });
            }
            else if (msg.Contains("首领") || msg.Contains("boss", StringComparison.CurrentCultureIgnoreCase) || 
                msg.Contains("BOSS") || msg.Contains("Boss"))
            {
                if (msg.Contains("地下城") || msg.Contains("地下") || msg.Contains("城"))
                {
                    result.AddRange(isNegate ? new int[] { 1, 2, 0, 4, 5 } : new int[] { 3 });
                }
                else if(msg.Contains("异界") || msg.Contains("突袭"))
                {
                    result.AddRange(isNegate ? new int[] { 1, 3, 0, 4, 5 } : new int[] { 2 });
                }
                else if (msg.Contains("迷宫"))
                {
                    result.AddRange(isNegate ? new int[] { 1, 3, 0, 2, 5 } : new int[] { 4 });
                }
                else if(msg.Contains("次元") || msg.Contains("裂缝"))
                {
                    result.AddRange(isNegate ? new int[] { 1, 3, 0, 2, 4 } : new int[] { 5 });
                }
                else
                {
                    result.AddRange(isNegate ? new int[] { 0, 1 } : new int[] { 2, 3, 4, 5, });
                }
            }
            else if (msg.Contains("魔灵"))
            {
                //if (msg.Contains("首领"))
                //{
                //    //
                //    result.AddRange(isNegate ? new int[] { 0 } : new int[] { 2, 3, 4, 5 });
                //}
                //else
                //{
                //    result.AddRange(isNegate ? new int[] { 1, 2, 3, 4, 5 } : new int[] { 0 });
                //}
                result.AddRange(isNegate ? new int[] { 1, 2, 3, 4, 5 } : new int[] { 0 });
            }
            return result;
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
                return user.EndDate > DateTime.Now;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 检测用户是否已经登陆
        /// </summary>
        /// <returns></returns>
        private bool IsLogin()
        {
            var token = this.GetUserToken();
            var user = this.CacheOption.GetValue<User>(token);
            
            if(user == null)
            {
                Log.Error("未能找到用户 : ", " token,", "cookie : ", token);
                return false;
            }
            else
            {
                return true;
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
