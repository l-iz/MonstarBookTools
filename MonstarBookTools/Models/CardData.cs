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
            Statuses = status.Zip(statusRank, (v, g) => new Status(v, g)).ToArray();
            Skills = skills.ToArray();
        }

        public string Name { get; }
        public string Rank { get; }
        public int StBonus => int.TryParse(Rank, out var value) ? value / 2 : 0;
        public int CP { get; }
        public int Charge { get; }
        public int Composite { get; }
        public int MaxLv { get; }
        public Status[] Statuses { get; }
        public string ImgSrc { get; }
        public Skill[] Skills { get; }

        public class Status
        {
            public Status(int value, string grow)
            {
                Value = value;
                Grow = StatusGrowRank.TryParse(grow, out var gr) ? gr : StatusGrowRank.F;
            }

            public int Value { get; }
            public StatusGrowRank Grow { get; }
        }
    }
}
