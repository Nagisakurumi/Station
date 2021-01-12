using ScriptServerStation.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Expends
{
    public static class UserExpends
    {
        /// <summary>
        /// 检测角色是否拥有vip权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsHasSpecialPower(this User user)
        {
            return user.IsSpecial || user.Type > 1;
        }
        /// <summary>
        /// 是否是管路员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsHasAdminPower(this User user)
        {
            return user.Type == 0;
        }
    }
}
