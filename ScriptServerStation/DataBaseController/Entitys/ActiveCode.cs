using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseController.Entitys
{
    public class ActiveCode
    {
        //public Guid Guid;
        /// <summary>
        /// 用户物理id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 激活码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 过期日期
        /// </summary>
        public string OverDate { get; set; }
        /// <summary>
        /// 使用的用户
        /// </summary>
        public string ByUser { get; set; }
        /// <summary>
        /// 是否被使用过
        /// </summary>
        public string IsUsed { get; set; }
        /// <summary>
        /// 有效时长
        /// </summary>
        public long ValidityDays { get; set; }
        /// <summary>
        /// 激活码类型
        /// </summary>
        public long CodeType { get; set; }
        /// <summary>
        /// 购买激活码的用户
        /// </summary>
        public string BuyAccount { get; set; }

        /// <summary>
        /// 获取激活码 类型
        /// </summary>
        /// <returns></returns>
        public CodeType GetCodeType()
        {
            return (CodeType)CodeType;
        }
    }

    /// <summary>
    /// 激活码类型
    /// </summary>
    public enum CodeType : int
    {
        /// <summary>
        /// 正常的激活码
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 推荐码
        /// </summary>
        Recommend = 2,
        /// <summary>
        /// 活动使用的激活码
        /// </summary>
        Active = 3,
    }
}
