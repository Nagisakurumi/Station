using DataBaseController.Entitys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseController
{
    /// <summary>
    /// 访问数据库上下文
    /// </summary>
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
           : base(options)
        {
        }

        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// 版本文件
        /// </summary>
        public DbSet<VersionUpdate> VersionUpdates { get; set; }
        /// <summary>
        /// 版本文件
        /// </summary>
        public DbSet<ActiveCode> ActiveCodes { get; set; }
        /// <summary>
        /// 战利品统计
        /// </summary>
        public DbSet<SpoilsStatistics> SpoilsStatistics { get; set; }
    }
}
