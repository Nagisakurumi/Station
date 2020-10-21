using System;
using System.Collections.Generic;

namespace ScriptServerStation.Database
{
    public partial class Powerroleinfo
    {
        public int Id { get; set; }
        public int? PowerId { get; set; }
        public int? RoleId { get; set; }
    }
}
