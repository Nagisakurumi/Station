
namespace ScriptControllerLib
{
    public class zTreeModel
    {
        /// <summary>
        /// 节点id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 节点pId
        /// </summary>
        public string pId { get; set; }
        /// <summary>
        /// 显示文本
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool open { get; set; }
        /// <summary>
        /// 是否父级
        /// </summary>
        public bool isParent { get; set; }
        /// <summary>
        /// 复选框检查状态
        /// </summary>
        public bool @checked { get; set; }
        /// <summary>
        /// 节点禁用
        /// </summary>
        public bool chkDisabled { get; set; }
        /// <summary>
        /// 不显示 checkbox
        /// </summary>
        public bool nocheck { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        //public string url { get; set; }
    }
}
