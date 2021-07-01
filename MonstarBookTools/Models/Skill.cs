using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonstarBookTools.Models
{
    public class Skill
    {
        public Skill(string name, int lv)
        {
            Name = name;
            Lv = lv;
        }

        public string Name { get; }
        public int Lv { get; }

        public override string ToString() => $"{Name}{Lv:+#;-#;**}";

        public static Skill Parse(string s)
        {
            var m = Regex.Match(s, @"^(.+)([+-]\d)$");
            return new Skill(m.Groups[1].Value, int.Parse(m.Groups[2].Value));
        }
    }
}
