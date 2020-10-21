using System;
using System.Collections.Generic;

namespace ScriptServerStation.Database
{
    public partial class Version
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Date { get; set; }
        public string VersionCode { get; set; }
    }
}
