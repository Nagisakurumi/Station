using System;
using System.Collections.Generic;

namespace ScriptServerStation.Database
{
    public partial class Activatecode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OverDate { get; set; }
        public string ByUser { get; set; }
        public bool IsUsed { get; set; }
        public int ValidityDays { get; set; }
        public int CodeType { get; set; }
        public string BuyAccount { get; set; }
    }
}
