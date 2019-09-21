using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataBaseController.Entitys
{
    public class User
    {
        /// <summary>
        /// 用户物理id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户guid
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 最近一次购买日期
        /// </summary>
        public string LastBuyDate { get; set; }
        /// <summary>
        /// 预计过期时间
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public string LastLoginDate { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 经验值
        /// </summary>
        public int LevelValue { get; set; }
        /// <summary>
        /// 是否是特许权限
        /// </summary>
        public string IsSpecial { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public double Balance { get; set; }
        /// <summary>
        /// 保留字段
        /// </summary>
        public string UnKnown1 { get; set; }
        /// <summary>
        /// 保留字段2
        /// </summary>
        public string UnKnown2 { get; set; }
    }
}
