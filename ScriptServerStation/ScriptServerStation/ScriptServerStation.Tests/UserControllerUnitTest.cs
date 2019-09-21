using CoreHelper;
using DataBaseController.Entitys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ScriptServerStation.Controllers;
using ScriptServerStation.Service;
using System.Collections.Generic;
using Xunit;

namespace ScriptServerStation.Tests
{
    public class UserControllerUnitTest
    {
        [Fact]
        public void Login()
        {
            var mock = new Mock<IUserService>();
            var session = new Mock<ISession>().Object;
            UserController userController = new UserController(mock.Object);
            userController.ControllerContext = new ControllerContext();
            userController.ControllerContext.HttpContext = new DefaultHttpContext();
            userController.ControllerContext.HttpContext.Session = session;

            string account = System.DateTime.Now.ToString();
            string password = System.DateTime.Now.Ticks.ToString();
            mock.Setup(x => x.Login(account, MD5Helper.GetMD5(password), session)).Returns(true);
            //userController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";
            var result = userController.Login(account, password);
            Assert.True(result.IsSuccess == true.ToString());
        }
        [Fact]
        public void GetUserList()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(x => x.GetUsers()).Returns(new List<User>());
            UserController userController = new UserController(mock.Object);
            var result = userController.GetUserList();
            Assert.True(result.Result is List<User>);
        }
        [Fact]
        public void GetUserInfo()
        {
            var mock = new Mock<IUserService>();
            string account = System.DateTime.Now.ToString();
            mock.Setup(x => x.GetUser(account)).Returns(new User());
            UserController userController = new UserController(mock.Object);
            var result = userController.GetUserInfo(account);
            Assert.True(result.Result is User);
        }
        [Fact]
        public void Register()
        {
            var mock = new Mock<IUserService>();
            var session = new Mock<ISession>().Object;
            //var mockGuidService = new Mock<DataBaseController.Interface.IGuidService>();
            //var timeMock = new Mock<TimeProvider>();
            UserController userController = new UserController(mock.Object);

            //mockGuidService.Setup(x => x.NewGuid()).Returns(new Guid("{CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C}"));
            //timeMock.SetupGet(tp => tp.Now).Returns(new DateTime(2010, 3, 11));
            //TimeProvider.Current = timeMock.Object;
            mock.Setup(x => x.AddUser(It.IsAny<User>(), null, session)).Returns(true);
            userController.ControllerContext = new ControllerContext();
            userController.ControllerContext.HttpContext = new DefaultHttpContext();
            userController.ControllerContext.HttpContext.Session = session;
            var result = userController.Register("123", "qwe", "asd", null);
            Assert.True(result.IsSuccess == true.ToString());
        }
        [Fact]
        public void GetVerification()
        {
            var mock = new Mock<IUserService>();
            var session = new Mock<ISession>().Object;
            UserController userController = new UserController(mock.Object);

            string email = System.DateTime.Now.ToString();
            mock.Setup(x => x.GetVerification(email, session)).Returns(true);
            userController.ControllerContext = new ControllerContext();
            userController.ControllerContext.HttpContext = new DefaultHttpContext();
            userController.ControllerContext.HttpContext.Session = session;

            var result = userController.GetVerification(email);
            Assert.True(result.IsSuccess == true.ToString());
        }
        [Fact]
        public void BuyVIP()
        {
            var mock = new Mock<IUserService>();
            UserController userController = new UserController(mock.Object);

            string account = System.DateTime.Now.ToString();
            int day = new System.Random().Next();
            mock.Setup(x => x.BuyVIP(It.IsAny<User>(), day)).Returns(true);
            
            var result = userController.BuyVIP(account, day);
            Assert.True(result.IsSuccess == true.ToString());
        }
        [Fact]
        public void Recharge()
        {
            var mock = new Mock<IUserService>();
            UserController userController = new UserController(mock.Object);

            string account = System.DateTime.Now.ToString();
            string code = System.Guid.NewGuid().ToString();
            double money = System.DateTime.Now.Ticks;
            mock.Setup(x => x.Recharge(It.IsAny<User>(), code, money)).Returns(true);

            var result = userController.Recharge(account, code, money);
            Assert.True(result.IsSuccess == true.ToString());
        }
    }
}
