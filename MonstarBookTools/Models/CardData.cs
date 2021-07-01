using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstarBookTools.Models
{
    public class CardData
    {
        public CardData(string name, string rank, int cp, int charge, int composite, int maxLv, IEnumerable<int> status, IEnumerable<string> statusRank, string imgSrc, IEnumerable<Skill> skills)
        {
            Name = name;
            Rank = rank;
            CP = cp;
            Charge = charge;
            Composite = composite;
            MaxLv = maxLv;
            ImgSrc = imgSrc;
            Status = status.ToArray();
            StatusRank = statusRank.ToArray();
            Skills = skills.ToArray();
        }

        public string Name { get; }
        public string Rank { get; }
        public int CP { get; }
        public int Charge { get; }
        public int Composite { get; }
        public int MaxLv { get; }
        public int[] Status { get; }
        public string[] StatusRank { get; }
        public string ImgSrc { get; }
        public Skill[] Skills { get; }
    }
}
