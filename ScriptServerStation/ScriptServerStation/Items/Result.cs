using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Items
{
    /// <summary>
    /// 接口返回数据结构
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 接口执行结果代码
        /// </summary>
        public ResultCode Code { get; set; }
        /// <summary>
        /// 接口执行消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 接口返回数据
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 设置返回数据码
        /// </summary>
        /// <param name="code"></param>
        public void SetCode(ResultCode code)
        {
            this.Code = code;
        }
        /// <summary>
        /// 设置失败
        /// </summary>
        public void SetFail()
        {
            SetCode(ResultCode.Fail);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public Result()
        {
            SetCode(ResultCode.Success);
        }

        /// <summary>
        /// 创建一个表示失败的
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result Fail(string message)
        {
            return new Result() { Code = ResultCode.Fail, Msg = message };
        }
        /// <summary>
        /// 创建一个表示成功的
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result Success(object data)
        {
            return new Result() { Code = ResultCode.Success, Data = data };
        }
        /// <summary>
        /// 创建一个表示成功的
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result Success(string message, object data)
        {
            return new Result() { Code = ResultCode.Success, Data = data, Msg = message };
        }
        /// <summary>
        /// 默认转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator Result(string message)
        {
            return Fail(message);
        }
    }
}
