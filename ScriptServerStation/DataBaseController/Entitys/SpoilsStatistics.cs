using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseController.Entitys
{
    public class SpoilsStatistics
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 战利品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 战利品类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 所属用户
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 出产日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 是否保留
        /// </summary>
        public string IsReserved { get; set; }
        /// <summary>
        /// 属性1
        /// </summary>
        public int PropertyType1 { get; set; }
        /// <summary>
        /// 属性2
        /// </summary>
        public int PropertyType2 { get; set; }
        /// <summary>
        /// 属性3
        /// </summary>
        public int PropertyType3 { get; set; }
        /// <summary>
        /// 属性4
        /// </summary>
        public int PropertyType4 { get; set; }
        /// <summary>
        /// 对于战利品的评分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 品级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 星级
        /// </summary>
        public int Star { get; set; }
    }
}
