using DataBaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScriptServerStation;
using ScriptControllerLib;

namespace ScriptServerStation.Service.Impl
{
    public class ScriptServiceImpl : BaseService, IScriptService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataBaseContext"></param>
        public ScriptServiceImpl(DataBaseContext DataBaseContext):base(DataBaseContext)
        {
            ScriptServiceManager.ScriptUrl = @"http://115.217.255.142:8383/api/Service/";
            ScriptServiceManager.InitFunction();
        }
        /// <summary>
        /// 脚本管理对象
        /// </summary>
        public static ScriptControllerLib.ScriptServiceManager ScriptServiceManager = new ScriptControllerLib.ScriptServiceManager();
        /// <summary>
        /// 获取api列表
        /// </summary>
        /// <returns></returns>
        public string GetAllApis()
        {
            return ScriptServiceManager.GetAllApi();
        }
        /// <summary>
        /// 打印函数
        /// </summary>
        /// <param name="scriptInput"></param>
        /// <returns></returns>
        public ScriptOutput Print(ScriptInput scriptInput)
        {
            return ScriptServiceManager.DoFunction("PrintObject", scriptInput);
        }
        /// <summary>
        /// 获取当前时间接口
        /// </summary>
        /// <param name="scriptInput"></param>
        /// <returns></returns>
        public ScriptOutput GetNowTime(ScriptInput scriptInput)
        {
            return ScriptServiceManager.DoFunction("GetNowTime", scriptInput);
        }
        /// <summary>
        /// 进行值比较函数
        /// </summary>
        /// <param name="scriptInput"></param>
        /// <returns></returns>
        public ScriptOutput ValueEquals(ScriptInput scriptInput)
        {
            return ScriptServiceManager.DoFunction("ValueEquals", scriptInput);
        }

        public ScriptOutput DelyTime(ScriptInput scriptInput)
        {
            return ScriptServiceManager.DoFunction("DelyTime", scriptInput);
        }

        public ScriptOutput Option(ScriptInput scriptInput)
        {
            return ScriptServiceManager.DoFunction("Option", scriptInput);
        }

        public ScriptOutput SetValue(ScriptInput scriptInput)
        {
            return ScriptServiceManager.DoFunction("SetValue", scriptInput);
        }
    }
}
