using System;
using System.Collections.Generic;

namespace ScriptServerStation.Database
{
    public partial class Versioninfo
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public DateTime Date { get; set; }
        public string Value { get; set; }
        public string UpdateMessage { get; set; }
    }
}
