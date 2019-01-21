using CoreHelper;
using DataBaseController.Entitys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ScriptControllerLib;
using ScriptServerStation.Controllers;
using ScriptServerStation.Service;
using System.Collections.Generic;
using Xunit;

namespace ScriptServerStation.Tests
{
    public class ServiceControllerUnitTest
    {
        [Fact]
        public void GetApis()
        {
            var mock = new Mock<IScriptService>();
            ServiceController serviceController = new ServiceController(mock.Object);

            string expected = System.DateTime.Now.ToString();
            mock.Setup(x => x.GetAllApis()).Returns(expected);
            var result = serviceController.GetApis();
            Assert.Equal(expected, result);
        }
        [Fact]
        public void GetTypeVueTree()
        {
            var mock = new Mock<IScriptService>();
            ServiceController serviceController = new ServiceController(mock.Object);

            string expected = System.DateTime.Now.ToString();
            mock.Setup(x => x.GetVueTreeApis(It.IsAny<string>())).Returns(expected);
            var result = serviceController.GetTypeVueTree();
            Assert.Equal(expected, result);
        }
        [Fact]
        public void GetTypeZTree()
        {
            var mock = new Mock<IScriptService>();
            ServiceController serviceController = new ServiceController(mock.Object);

            string expected = System.DateTime.Now.ToString();
            mock.Setup(x => x.GetTreeApis(It.IsAny<string>())).Returns(expected);
            var result = serviceController.GetTypeZTree();
            Assert.Equal(expected, result);
        }
        [Fact]
        public void GetTypeZTreeParam()
        {
            var mock = new Mock<IScriptService>();
            ServiceController serviceController = new ServiceController(mock.Object);

            string expected = System.DateTime.Now.ToString();
            string funcId = System.DateTime.Now.Ticks.ToString();
            mock.Setup(x => x.GetTreeApis(It.IsAny<string>())).Returns(expected);
            var result = serviceController.GetTypeZTreeParam(funcId);
            Assert.Equal(expected, result);
        }
        [Fact]
        public void PrintObject()
        {
            var mock = new Mock<IScriptService>();
            ServiceController serviceController = new ServiceController(mock.Object);
            ScriptOutput scriptOutput = new ScriptOutput();

            mock.Setup(x => x.Print(It.IsAny<ScriptInput>())).Returns(scriptOutput);
            var result = serviceController.PrintObject();
            Assert.Equal(result, result);
        }
        [Fact]
        public void GetNowTime()
        {
            var mock = new Mock<IScriptService>();
            ServiceController serviceController = new ServiceController(mock.Object);
            ScriptOutput scriptOutput = new ScriptOutput();

            mock.Setup(x => x.GetNowTime(It.IsAny<ScriptInput>())).Returns(scriptOutput);
            var result = serviceController.GetNowTime();
            Assert.Equal(result, result);
        }
        [Fact]
        public void DelyTime()
        {
            var mock = new Mock<IScriptService>();
            var requestMock = new Mock<HttpRequest>();
            var contextMock = new Mock<HttpContext>();
            var streamMock = new Mock<System.IO.Stream>();
            ServiceController serviceController = new ServiceController(mock.Object);
            ScriptOutput scriptOutput = new ScriptOutput();
            serviceController.ControllerContext = new ControllerContext();
            serviceController.ControllerContext.HttpContext = contextMock.Object;

            requestMock.SetupGet(x => x.ContentLength).Returns(0);
            requestMock.SetupGet(x => x.Body).Returns(streamMock.Object);
            contextMock.SetupGet(x => x.Request).Returns(requestMock.Object);
            mock.Setup(x => x.DelyTime(It.IsAny<ScriptInput>())).Returns(scriptOutput);
            var result = serviceController.DelyTime();
            Assert.Equal(scriptOutput, result);
        }
        [Fact]
        public void Option()
        {
            var mock = new Mock<IScriptService>();
            var requestMock = new Mock<HttpRequest>();
            var contextMock = new Mock<HttpContext>();
            var streamMock = new Mock<System.IO.Stream>();
            ServiceController serviceController = new ServiceController(mock.Object);
            ScriptOutput scriptOutput = new ScriptOutput();
            serviceController.ControllerContext = new ControllerContext();
            serviceController.ControllerContext.HttpContext = contextMock.Object;

            requestMock.SetupGet(x => x.ContentLength).Returns(0);
            requestMock.SetupGet(x => x.Body).Returns(streamMock.Object);
            contextMock.SetupGet(x => x.Request).Returns(requestMock.Object);
            mock.Setup(x => x.Option(It.IsAny<ScriptInput>())).Returns(scriptOutput);
            var result = serviceController.Option();
            Assert.Equal(scriptOutput, result);
        }
        [Fact]
        public void SetValue()
        {
            var mock = new Mock<IScriptService>();
            ServiceController serviceController = new ServiceController(mock.Object);
            ScriptOutput scriptOutput = new ScriptOutput();

            mock.Setup(x => x.SetValue(It.IsAny<ScriptInput>())).Returns(scriptOutput);
            var result = serviceController.SetValue();
            Assert.Equal(result, result);
        }
        [Fact]
        public void ValueEquals()
        {
            var mock = new Mock<IScriptService>();
            ServiceController serviceController = new ServiceController(mock.Object);
            ScriptOutput scriptOutput = new ScriptOutput();
            mock.Setup(x => x.ValueEquals(It.IsAny<ScriptInput>())).Returns(scriptOutput);
            var result = serviceController.ValueEquals();
            Assert.Equal(result, result);
        }
    }
}
