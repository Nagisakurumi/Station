
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
        /// <summary>
        /// 来源，source： system 系统定义。 service 服务器自定义 //现在读取全是service。可维护字段 
        /// </summary>
        public string source { get; set; }
    }
}
