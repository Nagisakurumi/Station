using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.HelpClasses
{
    /// <summary>
    /// 返回htpp请求格式
    /// </summary>
    public class ReturnObj
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public string IsSuccess { get; set; }
        /// <summary>
        /// 返回对象
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// 设置是否成功
        /// </summary>
        /// <param name="issuccess"></param>
        public void SetIsSuccess(bool issuccess)
        {
            this.IsSuccess = issuccess.ToString();
        }
    }
}
