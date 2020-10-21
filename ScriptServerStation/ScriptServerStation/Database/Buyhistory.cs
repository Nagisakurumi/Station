using System;
using System.Collections.Generic;

namespace ScriptServerStation.Database
{
    public partial class Buyhistory
    {
        public int Id { get; set; }
        public DateTime? BuyTime { get; set; }
        public float? Money { get; set; }
        public int? UserId { get; set; }
        public string UserNickName { get; set; }
        public int? CodeType { get; set; }
        public DateTime? ValidityDate { get; set; }
        public int? ActivateCodeId { get; set; }
        public ulong? IsUsed { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? AppendDay { get; set; }
    }
}
