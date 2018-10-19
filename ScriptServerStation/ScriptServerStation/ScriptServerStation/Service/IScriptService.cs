using ScriptControllerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Service
{
    public interface IScriptService
    {
        /// <summary>
        /// 获取api列表
        /// </summary>
        /// <returns></returns>
        string GetAllApis();
        /// <summary>
        /// 获取Web树api列表
        /// </summary>
        /// <returns></returns>
        string GetTreeApis(string key = null);
        string GetVueTreeApis(string key = null);
        /// <summary>
        /// 打印信息
        /// </summary>
        /// <param name="scriptInput"></param>
        /// <returns></returns>
        ScriptOutput Print(ScriptInput scriptInput);
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <param name="scriptInput"></param>
        /// <returns></returns>
        ScriptOutput GetNowTime(ScriptInput scriptInput);
        /// <summary>
        /// 值比较函数
        /// </summary>
        /// <param name="scriptInput"></param>
        /// <returns></returns>
        ScriptOutput ValueEquals(ScriptInput scriptInput);
        /// <summary>
        /// 延迟函数
        /// </summary>
        /// <param name="scriptInput"></param>
        /// <returns></returns>
        ScriptOutput DelyTime(ScriptInput scriptInput);
        /// <summary>
        /// 数值操作函数
        /// </summary>
        /// <param name="scriptInput"></param>
        /// <returns></returns>
        ScriptOutput Option(ScriptInput scriptInput);
        /// <summary>
        /// 数值设置操作
        /// </summary>
        /// <param name="scriptInput"></param>
        /// <returns></returns>
        ScriptOutput SetValue(ScriptInput scriptInput);
    }
}
