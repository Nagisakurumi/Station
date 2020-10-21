using System;
using System.Collections.Generic;

namespace ScriptServerStation.Database
{
    public partial class Roleinfo
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? UserGroupId { get; set; }
    }
}
