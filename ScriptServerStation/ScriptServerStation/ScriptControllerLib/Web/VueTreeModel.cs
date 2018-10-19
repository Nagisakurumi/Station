
namespace ScriptControllerLib
{
    public class VueTreeModel
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
        /// 是否父级
        /// </summary>
        public bool isParent { get; set; }

    }
}
