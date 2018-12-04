using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using IdentityModel;
using IdentityModel.Client;
using ScriptServerStation.Controllers;
using Moq;
using ScriptServerStation.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataBaseController.Entitys;
using System.Collections.Generic;
using CoreHelper;

namespace ScriptServerStation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task ClientCredentials_Test()
        {
            // request token
            //var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var DiscoveryClient = new DiscoveryClient("http://localhost:5000") { Policy = { RequireHttps = false } };
            var disco = await DiscoveryClient.GetAsync();
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client1", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            Assert.False(tokenResponse.IsError);
            Console.WriteLine(tokenResponse.Json);

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5010/values");
            Assert.True(response.IsSuccessStatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }
        [Fact]
        public void Login()
        {
            var mock = new Mock<IUserService>();
            var session = new Mock<ISession>().Object;
            UserController userController = new UserController(mock.Object);
            userController.ControllerContext = new ControllerContext();
            userController.ControllerContext.HttpContext = new DefaultHttpContext();
            userController.ControllerContext.HttpContext.Session = session;
            mock.Setup(x => x.Login("liu", MD5Helper.GetMD5("123"), session)).Returns(true);
            //userController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";
            var result = userController.Login("liu", "123");
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
            mock.Setup(x => x.GetUser("123")).Returns(new User());
            UserController userController = new UserController(mock.Object);
            var result = userController.GetUserInfo("123");
            Assert.True(result.Result is User);
        }
        [Fact]
        public void Register()
        {
            var mock = new Mock<IUserService>();
            var session = new Mock<ISession>().Object;
            var mockGuidService = new Mock<DataBaseController.Interface.IGuidService>();
            var timeMock = new Mock<TimeProvider>();
            var user = new User("123", "qwe", "asd");
            UserController userController = new UserController(mock.Object);

            mockGuidService.Setup(x => x.NewGuid()).Returns(new Guid("{CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C}"));
            timeMock.SetupGet(tp => tp.Now).Returns(new DateTime(2010, 3, 11));
            TimeProvider.Current = timeMock.Object;
            mock.Setup(x => x.AddUser(user, null, session)).Returns(true);
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

            mock.Setup(x => x.GetVerification("123", session)).Returns(true);
            userController.ControllerContext = new ControllerContext();
            userController.ControllerContext.HttpContext = new DefaultHttpContext();
            userController.ControllerContext.HttpContext.Session = session;

            var result = userController.GetVerification("123");
            Assert.True(result.IsSuccess == true.ToString());
        }
    }
}
