using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseController.Entitys
{
    public class User
    {
        /// <summary>
        /// 用户物理id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户guid
        /// </summary>
        public Guid Guid{ get; set; }
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
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最近一次购买日期
        /// </summary>
        public DateTime LastBuyDate { get; set; }
        /// <summary>
        /// 预计过期时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime LastLogionDate { get; set; }
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
        public bool IsSpecial { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 保留字段
        /// </summary>
        public string UnKnow1 { get; set; }
        /// <summary>
        /// 保留字段2
        /// </summary>
        public string UnKnow2 { get; set; }
    }
}
