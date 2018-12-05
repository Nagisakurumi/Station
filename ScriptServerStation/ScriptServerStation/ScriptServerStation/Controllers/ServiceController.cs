using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScriptControllerLib;
using ScriptServerStation.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ScriptServerStation.Controllers
{
    //[Produces("application/json")]
    [Route("api/Service")]
    public class ServiceController : Controller
    {
        /// <summary>
        /// 脚本service
        /// </summary>
        private IScriptService scriptService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scriptService"></param>
        public ServiceController(IScriptService scriptService)
        {
            this.scriptService = scriptService;
        }

        [HttpGet("getallapis")]
        public string GetApis()
        {
            try
            {
                return scriptService.GetAllApis();
            }
            catch (Exception)
            {
                return "";
            }
        }

        [HttpGet("GetTypeVueTree")]
        public string GetTypeVueTree()
        {
            try
            {
                return scriptService.GetVueTreeApis();
            }
            catch (Exception)
            {
                return "";
            }
        }



        [HttpGet("gettypeZTree")]
        public string GetTypeZTree()
        {
            try
            {
                return scriptService.GetTreeApis();
            }
            catch (Exception)
            {
                return "";
            }
        }

        [HttpGet("getTypeZTreeParam")]
        public string GetTypeZTreeParam(string funcId)
        {
            try
            {
                return scriptService.GetTreeApis(funcId);
            }
            catch (Exception)
            {
                return "";
            }
        }


        /// <summary>
        /// 打印接口
        /// </summary>
        /// <returns></returns>
        [HttpPost("PrintObject")]
        public ScriptOutput PrintObject()
        {
            try
            {
                return scriptService.Print(GetScriptInput());
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [HttpPost("GetNowTime")]
        public ScriptOutput GetNowTime()
        {
            try
            {
                return scriptService.GetNowTime(GetScriptInput());
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [HttpPost("DelyTime")]
        public ScriptOutput DelyTime()
        {
            try
            {
                return scriptService.DelyTime(GetScriptInput());
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [HttpPost("Option")]
        public ScriptOutput Option()
        {
            try
            {
                return scriptService.Option(GetScriptInput());
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [HttpPost("SetValue")]
        public ScriptOutput SetValue()
        {
            try
            {
                return scriptService.SetValue(GetScriptInput());
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [HttpPost("ValueEquals")]
        public ScriptOutput ValueEquals()
        {
            try
            {
                return scriptService.ValueEquals(GetScriptInput());
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取函数的参数信息
        /// </summary>
        /// <returns></returns>
        private ScriptInput GetScriptInput()
        {
            byte[] datas = new byte[HttpContext.Request.ContentLength.Value];
            HttpContext.Request.Body.Read(datas, 0, datas.Length);
            string stream = System.Text.Encoding.UTF8.GetString(datas);
            return JsonConvert.DeserializeObject<ScriptInput>(stream);
        }
    }
}