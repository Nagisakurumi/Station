using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ScriptServerStation.FileUpdate
{
    /// <summary>
    /// 文件持续输出流
    /// </summary>
    public class FileOutStream
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// 设定的自动关闭时间
        /// </summary>
        public DateTime CloseTime { get; private set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool IsClose { get; private set; } = false;
        /// <summary>
        /// 自动关闭任务
        /// </summary>
        private Task<bool> AutoCloseTask { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// 文件保存路径
        /// </summary>
        public string FilePath { get; private set; }
        /// <summary>
        /// 文件写出流
        /// </summary>
        private FileStream WriteStream { get; set; }
        /// <summary>
        /// 文件持续写出流
        /// </summary>
        /// <param name="name">文件</param>
        /// <param name="closeTime">自动关闭时间</param>
        /// <param name="path">路径</param>
        public FileOutStream(string name, DateTime closeTime, string path)
        {
            if (closeTime < DateTime.Now) throw new Exception("CloseTime muset over then now!");
            this.FileName = name;
            this.CloseTime = closeTime;
            this.FilePath = path;

            this.WriteStream = File.OpenWrite(path);
            this.AutoCloseTask = new Task<bool>(() => {
                try
                {
                    int mlsecond = (int)(this.CloseTime - DateTime.Now).TotalMilliseconds;
                    Thread.Sleep(mlsecond);
                    if (!this.IsClose)
                    {
                        this.IsClose = true;
                        this.WriteStream.Flush();
                        this.WriteStream.Close();
                    }
                }
                catch (Exception)
                {
                }
                return true;
            });
        }




    }
}
