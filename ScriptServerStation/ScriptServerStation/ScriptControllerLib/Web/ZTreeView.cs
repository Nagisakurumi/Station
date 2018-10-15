using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptControllerLib
{
    public static class ZTreeView
    {
        public static string ZTreeJson(this List<zTreeModel> data, string pId = "0")
        {
            StringBuilder strJson = new StringBuilder();
            List<zTreeModel> item = data.FindAll(t => t.pId == pId);
            strJson.Append("[");
            if (item.Count > 0)
            {
                foreach (zTreeModel entity in item)
                {
                    strJson.Append("{");
                    strJson.Append("\"id\":\"" + entity.id.ToString() + "\",");
                    strJson.Append("\"name\":\"" + entity.name.Replace("&nbsp;", "") + "\",");
                    strJson.Append("\"open\":" + entity.open.ToString().ToLower() + ",");
                    strJson.Append("\"isParent\":" + entity.isParent.ToString().ToLower() + ",");
                    strJson.Append("\"checked\":" + entity.@checked.ToString().ToLower() + "");


                    if (entity.isParent)
                    {
                        strJson.Append(",");
                        strJson.Append("\"children\":" + ZTreeJson(data, entity.id) + "");
                    }
                    strJson.Append("},");
                }
                strJson = strJson.Remove(strJson.Length - 1, 1);
            }
            strJson.Append("]");
            return strJson.ToString();
        }
    }
}
