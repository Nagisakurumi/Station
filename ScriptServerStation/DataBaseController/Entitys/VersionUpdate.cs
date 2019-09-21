using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseController.Entitys
{
    public class VersionUpdate
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 更新包路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 更新日期
        /// </summary>
        public string Date { get; set; }
    }
}
