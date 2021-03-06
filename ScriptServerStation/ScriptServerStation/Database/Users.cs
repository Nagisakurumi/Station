﻿using System;
using System.Collections.Generic;

namespace ScriptServerStation.Database
{
    public partial class User
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastBuyDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int Level { get; set; }
        public int LevelValue { get; set; }
        public string IsSpecial { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public double Balance { get; set; }
        public string Type { get; set; }
        public byte[] IsActivated { get; set; }
    }
}
