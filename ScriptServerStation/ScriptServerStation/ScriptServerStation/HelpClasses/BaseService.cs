using DataBaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation
{
    public class BaseService
    {
        /// <summary>
        /// 访问数据库
        /// </summary>
        public DataBaseController.DataBaseContext DataBaseContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataBaseContext"></param>
        public BaseService(DataBaseContext DataBaseContext)
        {
            this.DataBaseContext = DataBaseContext;
        }
    }
}
