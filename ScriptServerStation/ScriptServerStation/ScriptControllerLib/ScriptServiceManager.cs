using Newtonsoft.Json;
using ScriptControllerLib.Agreements;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptControllerLib
{
    /// <summary>
    /// 脚本方法的管理
    /// </summary>
    public class ScriptServiceManager
    {
        /// <summary>
        /// 脚本请求地址
        /// </summary>
        public static string ScriptUrl = "";

        /// <summary>
        /// api地址
        /// </summary>
        public static Dictionary<string, ScriptAPI> ScriptAPIs = new Dictionary<string, ScriptAPI>();

        /// <summary>
        /// 添加脚本函数
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="scriptFunction">脚本函数</param>
        public static bool AddScriptFunction(string url, ScriptFunction scriptFunction)
        {
            if (ScriptAPIs.ContainsKey(url) == false)
            {
                ScriptAPIs.Add(url, new ScriptAPI(ScriptUrl + url, scriptFunction));
                return true;
            }
            else
            {
                ScriptAPIs[url] = new ScriptAPI(ScriptUrl + url, scriptFunction);
                return true;
            }
        }

        /// <summary>
        /// 删除一个脚本函数
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool RemoveScriptFunction(string url)
        {
            if (ScriptAPIs.ContainsKey(url) == true)
            {
                ScriptAPIs.Remove(url);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取所有的api信息
        /// </summary>
        /// <returns></returns>
        public static string GetAllApi()
        {
            List<ScriptMethAttribute> scriptMeths = new List<ScriptMethAttribute>();
            foreach (var item in ScriptAPIs)
            {
                scriptMeths.Add(item.Value.ScriptMethAttribute);
            }
            return JsonConvert.SerializeObject(scriptMeths).ToString();
        }

        /// <summary>
        /// 获取所有的api信息
        /// </summary>
        /// <returns></returns>
        public static string GetTreeApis(string key = null)
        {
            var treeList = new List<zTreeModel>();
            if (key == null)
            {
                foreach (var item in ScriptAPIs)
                {
                    zTreeModel tree = new zTreeModel();
                    bool hasChildren = false;

                    tree.id = item.Key.ToString();
                    tree.name = item.Value.ScriptMethAttribute.Name.ToString();
                    tree.pId = "0";
                    tree.open = true;
                    tree.@checked = false;
                    tree.isParent = hasChildren;
                    treeList.Add(tree);
                }
                return treeList.ZTreeJson();
            }
            else
            {
                List<ScriptMethAttribute> scriptMeths = new List<ScriptMethAttribute>();
                scriptMeths.Add(ScriptAPIs[key].ScriptMethAttribute);
                return JsonConvert.SerializeObject(scriptMeths).ToString(); ;
            }
        }


        /// <summary>
        /// 操作函数
        /// </summary>
        /// <param name="scriptInput"></param>
        /// <returns></returns>
        public static ScriptOutput DoFunction(string key, ScriptInput scriptInput)
        {
            if (ScriptAPIs.ContainsKey(key))
            {
                return ScriptAPIs[key].ScriptFunction(scriptInput);
            }
            return null;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public static void InitFunction()
        {
            if (ScriptAPIs.Count != 0) return;
            AddScriptFunction("PrintObject", ScriptToolsFunction.PrintObject);
            AddScriptFunction("DelyTime", ScriptToolsFunction.DelyTime);
            AddScriptFunction("GetNowTime", ScriptToolsFunction.GetNowTime);
            AddScriptFunction("TimeDesc", ScriptToolsFunction.TimeDesc);
            AddScriptFunction("ValueEquals", ScriptToolsFunction.ValueEquals);
            AddScriptFunction("Options", ScriptToolsFunction.Options);
            AddScriptFunction("SetValue", ScriptToolsFunction.SetValue);
        }
    }
}
