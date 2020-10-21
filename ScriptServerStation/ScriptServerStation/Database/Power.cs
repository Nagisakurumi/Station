using System;
using System.Collections.Generic;

namespace ScriptServerStation.Database
{
    public partial class Power
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
    }
}
