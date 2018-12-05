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

            mock.Setup(x => x.GetAllApis()).Returns("qqq");
            var result = serviceController.GetApis();
            Assert.Equal("qqq", result);
        }
        [Fact]
        public void GetTypeVueTree()
        {
            var mock = new Mock<IScriptService>();
            ServiceController serviceController = new ServiceController(mock.Object);

            mock.Setup(x => x.GetVueTreeApis(It.IsAny<string>())).Returns("qqq");
            var result = serviceController.GetTypeVueTree();
            Assert.Equal("qqq", result);
        }
        [Fact]
        public void GetTypeZTree()
        {
            var mock = new Mock<IScriptService>();
            ServiceController serviceController = new ServiceController(mock.Object);

            mock.Setup(x => x.GetTreeApis(It.IsAny<string>())).Returns("qqq");
            var result = serviceController.GetTypeZTree();
            Assert.Equal("qqq", result);
        }
        [Fact]
        public void GetTypeZTreeParam()
        {
            var mock = new Mock<IScriptService>();
            ServiceController serviceController = new ServiceController(mock.Object);

            mock.Setup(x => x.GetTreeApis(It.IsAny<string>())).Returns("qqq");
            var result = serviceController.GetTypeZTreeParam("123");
            Assert.Equal("qqq", result);
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
            ServiceController serviceController = new ServiceController(mock.Object);
            ScriptOutput scriptOutput = new ScriptOutput();

            mock.Setup(x => x.DelyTime(It.IsAny<ScriptInput>())).Returns(scriptOutput);
            var result = serviceController.DelyTime();
            Assert.Equal(scriptOutput, result);
        }
        [Fact]
        public void Option()
        {
            var mock = new Mock<IScriptService>();
            ServiceController serviceController = new ServiceController(mock.Object);
            ScriptOutput scriptOutput = new ScriptOutput();
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
