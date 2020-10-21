using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ScriptServerStation.Database
{
    public partial class DataBaseContext : DbContext
    {
        public DataBaseContext()
        {
        }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activatecode> Activatecode { get; set; }
        public virtual DbSet<Spoils> Spoils { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Versioninfo> Versioninfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=www.wxxandxyx.cn;userid=mytest;pwd=123456;port=3306;database=martian;sslmode=none", x => x.ServerVersion("5.7.31-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activatecode>(entity =>
            {
                entity.ToTable("activatecode");

                entity.HasComment("激活码表");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.BuyAccount)
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasComment("购买人账号")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ByUser)
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasComment("使用 的账号")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasComment("激活码")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CodeType)
                    .HasColumnType("int(11)")
                    .HasComment("激活码类型");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'")
                    .HasComment("创建日期");

                entity.Property(e => e.IsUsed).HasComment("是否被使用");

                entity.Property(e => e.OverDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'")
                    .HasComment("过期时间");

                entity.Property(e => e.ValidityDays)
                    .HasColumnType("int(11)")
                    .HasComment("时效");
            });

            modelBuilder.Entity<Spoils>(entity =>
            {
                entity.ToTable("spoils");

                entity.HasComment("战利品");

                entity.Property(e => e.Id)
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasComment("id")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'")
                    .HasComment("存入日期");

                entity.Property(e => e.IsSave).HasComment("是否保留(0, 出售， 1.保留)");

                entity.Property(e => e.Level)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'-1'")
                    .HasComment("等级");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasComment("战利品名称")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Score)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'-1'")
                    .HasComment("评分");

                entity.Property(e => e.Star)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'-1'")
                    .HasComment("星级");

                entity.Property(e => e.Type)
                    .HasColumnType("int(11)")
                    .HasComment("战利品类型");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasComment("用户id")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasComment("用户名称")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Balance).HasComment("余额");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'2020-00-00 00:00:00'")
                    .HasComment("创建日期");

                entity.Property(e => e.Email)
                    .HasColumnType("varchar(36)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasComment("结束日期");

                entity.Property(e => e.Guid)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasComment("逻辑id")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.IsActivated).HasComment("用户是否被激活（0 : 未激活， 1 : 已激活 ）");

                entity.Property(e => e.IsSpecial).HasComment("是否是特殊用户");

                entity.Property(e => e.LastBuyDate)
                    .HasColumnType("datetime")
                    .HasComment("最后购买日期");

                entity.Property(e => e.LastLoginDate)
                    .HasColumnType("datetime")
                    .HasComment("最后登陆日期");

                entity.Property(e => e.Level)
                    .HasColumnType("int(11)")
                    .HasComment("用户等级");

                entity.Property(e => e.LevelValue)
                    .HasColumnType("int(11)")
                    .HasComment("等级级");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(36)")
                    .HasDefaultValueSql("''")
                    .HasComment("名称")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(36)")
                    .HasDefaultValueSql("''")
                    .HasComment("密码")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Phone)
                    .HasColumnType("varchar(36)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Type)
                    .HasColumnType("int(11)")
                    .HasComment("用户类型");
            });

            modelBuilder.Entity<Versioninfo>(entity =>
            {
                entity.ToTable("versioninfo");

                entity.HasComment("版本更新记录表");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'")
                    .HasComment("更新日期");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasComment("更新包路径")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UpdateMessage)
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasComment("更新日志")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'1.0.0.1'")
                    .HasComment("版本号")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
