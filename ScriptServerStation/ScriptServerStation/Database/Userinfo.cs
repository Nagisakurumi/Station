using System;
using System.Collections.Generic;

namespace ScriptServerStation.Database
{
    public partial class Userinfo
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? UserGroupId { get; set; }
    }
}
