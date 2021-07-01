using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstarBookTools.Models
{
    public class SkillData
    {
        public SkillData(string name, string color, int[] lvs)
        {
            Name = name;
            Color = color;
            Lvs = lvs;
        }

        public string Name { get; }
        public string Color { get; }
        public int[] Lvs { get; }
    }
}
