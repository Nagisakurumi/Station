using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScriptServerStation.Expends;
using ScriptServerStation.FileUpdate;
using ScriptServerStation.HelpClasses;
using ScriptServerStation.HelpClasses.Cache;
using ScriptServerStation.Items;
using ScriptServerStation.Service;
using ScriptServerStation.Service.Impl;
using Versioninfo = ScriptServerStation.Database.Versioninfo;

namespace ScriptServerStation.Controllers
{
    /// <summary>
    /// 版本更新
    /// </summary>
    [Route("api/version/")]
    [ApiController]
    public class VersionUpdateController : ScriptBaseController
    {
        /// <summary>
        /// 1mb的大小
        /// </summary>
        public static int PackageSize { get; set; } = 1048576 * 6;
        /// <summary>
        /// 版本更新接口
        /// </summary>
        public IVersionUpdateService VersionUpdateService { get; set; }
        /// <summary>
        /// 上传最新版本信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("uploadinfo")]
        public Result UploadInfo([FromBody]VersionUpdateInfo versioninfo)
        {
            try
            {
                if(versioninfo.FlushVersion() == false)
                {
                    return "版本号异常!";
                }
                string root = Path.Combine(AppContext.BaseDirectory, CacheKey.UploadCacheFileRootPath, versioninfo.Version);
                //如果文件夹存在,进行删除
                if (Directory.Exists(root) || VersionUpdateService.GetVersionByVersion(versioninfo.Version) != null)
                {
                    return "已经存在该版本，不可以进行覆盖!";
                }
                else
                {
                    Directory.CreateDirectory(root);
                }
                //版本信息
                Versioninfo version = new Versioninfo();
                version.Path = versioninfo.Version;
                version.Value = versioninfo.Version;
                version.UpdateMessage = versioninfo.Message;
                versioninfo.Path = root;
                //添加版本
                VersionUpdateService.AddVersion(version);
                //更新当前最新的正在上传的版本信息
                MemoryCache.SetValue(CacheKey.UploadCacheFileName, versioninfo);
                //保存版本更新内容
                versioninfo.ToFile(root.Combine(CacheKey.VersionUpdateFileSaveName));
                //返回成功
                return Result.Success(null);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Result.Fail(ex.Message);
            }
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("uploadfile")]
        [DisableRequestSizeLimit]
        public Result UploadFile([FromForm]IFormCollection formCollection)
        {
            try
            {
                VersionUpdateInfo updateInfo = MemoryCache.GetValue<VersionUpdateInfo>(CacheKey.UploadCacheFileName);
                if (updateInfo == null)
                    return "必须先上传版本信息!";
                
                foreach (IFormFile file in ((FormCollection)formCollection).Files)
                {
                    StreamReader reader = new StreamReader(file.OpenReadStream());
                    string filename = Path.Combine(updateInfo.Path, file.FileName);
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                    string root = Path.GetDirectoryName(filename);
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }
                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        // 复制文件
                        file.CopyTo(fs);
                        // 清空缓冲区数据
                        fs.Flush();
                    }
                }
                return Result.Success("成功!");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 分片上传
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("uploadfileitem")]
        public Result UploadFileItem([FromForm] IFormCollection formCollection)
        {
            try
            {
                VersionUpdateInfo updateInfo = MemoryCache.GetValue<VersionUpdateInfo>(CacheKey.UploadCacheFileName);
                if (updateInfo == null)
                    return "必须先上传版本信息!";
                //要写入的文件流
                MemoryStream writeStream = null;
                FormCollection form = ((FormCollection)formCollection);
                bool isFirst = form["First"][0].ToBaseType<bool>();
                bool isCenter = form["Center"][0].ToBaseType<bool>();
                bool isEnd = form["End"][0].ToBaseType<bool>();
                string md5 = form["MD5"][0].ToBaseType<string>();
                IFormFile file = form.Files.First();
                string filename = Path.Combine(updateInfo.Path, file.FileName);
                string root = Path.GetDirectoryName(filename);
                //检测路径是否存在
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                if (isFirst)
                {
                    writeStream = new MemoryStream();
                    MemoryCache.SetValue(md5, writeStream, DateTime.Now + TimeSpan.FromMinutes(10));
                }
                else
                {
                    writeStream = MemoryCache.GetValue<MemoryStream>(md5) as MemoryStream;
                }
                // 复制文件
                file.CopyTo(writeStream);
                if (isEnd)
                {
                    using (FileStream stream = System.IO.File.Create(filename))
                    {
                        writeStream.Position = 0;
                        writeStream.CopyTo(stream);
                        stream.Flush();
                        writeStream.Flush();
                        writeStream.Close();
                    }
                    
                }
                return Result.Success("成功!");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 检测所有版本
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkversions")]
        public Result CheckUpdate()
        {
            try
            {
                //历史版本信息
                List<Versioninfo> versioninfos = VersionUpdateService.GetAllVersion();
                //versioninfos.ForEach(v => v.Path = "");
                return Result.Success("成功", versioninfos);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 获取更新到指定版本的信息
        /// </summary>
        /// <param name="oldversion"></param>
        /// <param name="targetversion"></param>
        /// <returns></returns>
        [HttpGet("updateversion/{oldversion}/{targetversion}")]
        public Result UpdateVersion(string oldversion, string targetversion)
        {
            try
            {
                oldversion = oldversion.FlushVersion();
                targetversion = targetversion.FlushVersion();
                VersionUpdateInfo old = VersionUpdateService.GetVersionInfoByVersion(oldversion);
                VersionUpdateInfo target = VersionUpdateService.GetVersionInfoByVersion(targetversion);
                if (target == null)
                {
                    return "请求的版本不存在!";
                }
                //获取所有相差的版本信息
                List<Versioninfo> versioninfos = null;
                if (old == null)
                {
                    versioninfos = VersionUpdateService.GetBeforeVersion(targetversion);
                }
                else
                {
                    //如果反向更新
                    if (targetversion.ToVersion() < oldversion.ToVersion())
                    {
                        versioninfos = VersionUpdateService.GetBeforeVersion(oldversion);
                        List<Versioninfo> removes = new List<Versioninfo>();
                        foreach (var item in versioninfos)
                        {
                            if (item.Value == targetversion)
                            {
                                break;
                            }
                            removes.Add(item);
                        }
                        //删除多余的
                        foreach (var item in removes)
                        {
                            versioninfos.Remove(item);
                        }
                    }
                    else
                        versioninfos = VersionUpdateService.GetVersionBetween(oldversion, targetversion);
                }
                return Result.Success("", StaticHelp.ConvertToHistory(versioninfos, 
                    CacheKey, oldversion.ToVersion() > targetversion.ToVersion()));
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpGet("download/{path}")]
        public IActionResult DownloadFile(string path)
        {
            try
            {
                path = path.FromBase64();
                string name = System.IO.Path.GetFileName(path);
                return File(System.IO.File.OpenRead(System.IO.Path.Combine(AppContext.BaseDirectory, CacheKey.UploadCacheFileRootPath, path)), "application/octet-stream", name);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        /// <summary>
        /// 下载大文件
        /// </summary>
        /// <param name="path">文件名称</param>
        /// <returns></returns>
        [HttpGet("downloadbig/{path}")]
        public void DownloadBigFile(string path)
        {
            try
            {
                path = path.FromBase64();
                path = System.IO.Path.Combine(AppContext.BaseDirectory, CacheKey.UploadCacheFileRootPath, path);
                if(System.IO.File.Exists(path) == false)
                {
                    //return Result.Fail("文件不存在!");
                    return;
                }
                int size = PackageSize;
                var response = HttpContext.Response;
                using (FileStream stream = System.IO.File.OpenRead(path))
                {
                    while (true)
                    {
                        //如果超出
                        if (stream.Position + PackageSize >= stream.Length)
                        {
                            size = (int)(stream.Length - stream.Position);
                        }

                        //如果请求被中途取消
                        if (HttpContext.RequestAborted.IsCancellationRequested)
                        {
                            break;
                        }
                        byte[] datas = new byte[size];
                        stream.Read(datas, 0, size);
                        response.Body.WriteAsync(datas, 0, size).Wait();
                        response.Body.Flush();
                
                        if (stream.Position == stream.Length)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Log.Log("下载大文件失败!");
            }
        }
    }
}