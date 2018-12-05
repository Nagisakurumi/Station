﻿using Newtonsoft.Json;
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
        private static Dictionary<string, ScriptAPI> ScriptAPIs = new Dictionary<string, ScriptAPI>();
        // 定义一个静态变量来保存类的实例
        private static ScriptServiceManager _scriptServiceManager;
        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        private ScriptServiceManager()
        { }

        public static ScriptServiceManager CreateInstance()
        {
            if (_scriptServiceManager == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (_scriptServiceManager == null)
                    {
                        _scriptServiceManager = new ScriptServiceManager();
                    }
                }
            }
            return _scriptServiceManager;
        }
        /// <summary>
        /// 添加脚本函数
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="scriptFunction">脚本函数</param>
        public bool AddScriptFunction(string url, ScriptFunction scriptFunction)
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
        public bool RemoveScriptFunction(string url)
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
        public string GetAllApi()
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
        public string GetTreeApis(string key = null)
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
        public string GetVueTreeApis(string key = null)
        {
            var treeList = new List<VueTreeModel>();
            if (key == null)
            {
                foreach (var item in ScriptAPIs)
                {
                    VueTreeModel tree = new VueTreeModel();
                    bool hasChildren = false;
                    tree.id = item.Key.ToString();
                    tree.name = item.Value.ScriptMethAttribute.Name.ToString();
                    tree.source = "service";
                    tree.pId = "0";
                    tree.isParent = hasChildren;
                    treeList.Add(tree);
                }
                return treeList.VueTreeJson();
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
        public ScriptOutput DoFunction(string key, ScriptInput scriptInput)
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
        public void InitFunction()
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
