using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstarBookTools.Models
{
    public record Rare(string Name, int MaxSkillCount)
    {
        public override string ToString() => Name;

        public static readonly Rare Bronze = new("銅", 4);
        public static readonly Rare Silver = new("銀", 6);
        public static readonly Rare Gold = new("金", 8);

        public static readonly Rare[] Rares = new[] { Bronze, Silver, Gold };
    }

    public record Card(string Name, string Rank, int Composite, Rare Rare, string ImgSrc, Skill[] Skills)
    {
        public Card(CardData cardData) : this(cardData.Name, cardData.Rank, cardData.Composite, Rare.Bronze, cardData.ImgSrc, cardData.Skills) { }
    }
}
