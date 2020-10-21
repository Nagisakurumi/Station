using System;
using System.Collections.Generic;

namespace ScriptServerStation.Database
{
    public partial class Userroleinfo
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
    }
}
