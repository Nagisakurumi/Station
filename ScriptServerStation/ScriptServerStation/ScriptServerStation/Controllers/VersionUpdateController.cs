using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreHelper.Expends;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScriptServerStation.HelpClasses;
using ScriptServerStation.Service.Impl;

namespace ScriptServerStation.Controllers
{
    [Produces("application/json")]
    [Route("api/VersionUpdate")]
    public class VersionUpdateController : Controller
    {
        /// <summary>
        /// 版本更新接口
        /// </summary>
        IVersionUpdateService VersionUpdateService;

        public VersionUpdateController(IVersionUpdateService versionUpdateService)
        {
            this.VersionUpdateService = versionUpdateService;
        }
        /// <summary>
        /// 上传最新版本
        /// </summary>
        /// <returns></returns>
        [HttpPost("updateload")]
        public ReturnObj UpdateLoadVersion()
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                VersionUpdateService.AddVersion(HttpContext.Request.Form.Files);
                returnObj.SetIsSuccess(true);
            }
            catch (Exception ex)
            {
                returnObj.SetIsSuccess(false);
            }
            return returnObj;
        }
        /// <summary>
        /// 检测最新版本
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkupdate")]
        public ReturnObj CheckUpdate()
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                string rootdir = VersionUpdateService.GetLastUpdateConfig();
                string file = Directory.GetFiles(rootdir, "*.config", SearchOption.TopDirectoryOnly).First();
                if (rootdir != null)
                {
                    returnObj.Result = file.ReadFileAsJsonData();
                }
                returnObj.SetIsSuccess(true);
            }
            catch (Exception)
            {
                returnObj.SetIsSuccess(false);
            }
            return returnObj;
        }
        /// <summary>
        /// 下载更新包
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadupdatepacket")]
        public void DownLoadUpdatePacket()
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                string rootdir = VersionUpdateService.GetLastUpdateConfig();
                if(rootdir != null)
                {
                    string file = Directory.GetFiles(rootdir, "*.zip", SearchOption.TopDirectoryOnly).First();
                    using (FileStream stream = System.IO.File.OpenRead(file))
                    {
                        byte[] datas = new byte[stream.Length];
                        stream.Read(datas, 0, datas.Length);
                        object statu = new object();
                        var req = HttpContext.Response.Body.BeginWrite(datas, 0, datas.Length, _ => { }, statu);
                        HttpContext.Response.Body.EndWrite(req);
                        datas = null;
                        statu = null;
                    }
                    HttpContext.Response.Body.Flush();
                    //HttpContext.Response.Body.EndWrite();
                    returnObj.SetIsSuccess(true);
                }
                else
                {
                    returnObj.SetIsSuccess(false);
                }
            }
            catch (Exception)
            {
                returnObj.SetIsSuccess(true);
            }
            //return returnObj;
        }
    }
}