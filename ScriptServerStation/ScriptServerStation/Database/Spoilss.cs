using System;
using System.Collections.Generic;

namespace ScriptServerStation.Database
{
    public partial class Spoils
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string UserName { get; set; }
        public string CreateTime { get; set; }
        public sbyte IsSave { get; set; }
        public int? Score { get; set; }
        public int? Level { get; set; }
        public int? Star { get; set; }
        public string UserId { get; set; }
    }
}
