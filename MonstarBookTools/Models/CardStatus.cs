using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MonstarBookTools.Models
{
    public record StatusType(string Name, int RareBonusCorrect)
    {
        public static readonly StatusType STAB = new("STAB", 2);
        public static readonly StatusType HACK = new("HACK", 2);
        public static readonly StatusType INT = new("INT", 2);
        public static readonly StatusType DEF = new("DEF", 2);
        public static readonly StatusType MR = new("MR", 2);
        public static readonly StatusType AGI = new("AGI", 1);
        public static readonly StatusType DEX = new("DEX", 1);

        public static readonly StatusType[] Statuses = new[] { STAB, HACK, INT, DEF, MR, DEX, AGI };
        public static bool TryParse(string s, [NotNullWhen(true)] out StatusType? statusType)
        {
            statusType = Statuses.SingleOrDefault(st => st.Name == s);
            return statusType != null;
        }
    }

    public record StatusGrowRank(string Name, double Rate)
    {
        public static readonly StatusGrowRank F = new("F", 0.1);
        public static readonly StatusGrowRank E = new("E", 0.2);
        public static readonly StatusGrowRank D = new("D", 0.3);
        public static readonly StatusGrowRank C = new("C", 0.4);
        public static readonly StatusGrowRank B = new("B", 0.5);
        public static readonly StatusGrowRank A = new("A", 0.6);
        public static readonly StatusGrowRank S = new("S", 0.7);
        public static readonly StatusGrowRank SS = new("SS", 0.8);

        public static readonly StatusGrowRank[] Ranks = new[] { F, E, D, C, B, A, S, SS };

        public static StatusGrowRank operator +(StatusGrowRank rank, int value)
        {
            var idx = Array.IndexOf(Ranks, rank);
            return Ranks[Math.Max(0, Math.Min(Ranks.Length - 1, idx + value))];
        }

        public static bool TryParse(string s, [NotNullWhen(true)] out StatusGrowRank? rank)
        {
            rank = Ranks.SingleOrDefault(r => r.Name == s);
            return rank != null;
        }
    }

    public class CardStatus
    {
        public CardStatus(int baseValue, StatusGrowRank baseGrowRank, StatusIncreaseSkill skill)
        {
            BaseValue = baseValue;
            BaseGrowRank = baseGrowRank;
            Skill = skill;
        }

        public int BaseValue { get; }
        public int CorrectedValue => BaseValue + (Skill.IncreaseSkillChecked ? Skill.IncreaseSkillLv * Skill.StatusType.RareBonusCorrect : 0);
        public StatusGrowRank BaseGrowRank { get; }
        public StatusGrowRank CorrectedGrowRank => BaseGrowRank + (Skill.GrowSkillChecked ? Skill.GrowSkillLv * Skill.StatusType.RareBonusCorrect : 0);
        public StatusIncreaseSkill Skill { get; }
    }

    public class StatusIncreaseSkill
    {
        public StatusIncreaseSkill(StatusType statusType)
        {
            StatusType = statusType;
        }
        public StatusType StatusType { get; }

        public bool IncreaseSkillChecked { get; set; }
        public int IncreaseSkillLv { get; set; } = 5;
        public int[] IncreaseSkillLvs { get; } = new[] { 5, 4, 3, 2, 1, -1, -2, -3, -4 };

        public bool GrowSkillChecked { get; set; }
        public int GrowSkillLv { get; set; } = 3;
        public int[] GrowSkillLvs { get; } = new[] { 3, 2, 1, -1, -2, -3 };
    }

    public class TakuminoWazaSkill
    {
        public bool MainChecked { get; set; }
        public int MainLv { get; set; } = 3;
        public bool SubChecked { get; set; }
        public int SubLv { get; set; } = 3;
        public int StBonus => (MainChecked ? MainLv : 0) + (SubChecked ? SubLv : 0);
        public int[] SkillLvs { get; } = new[] { 3, 2, 1, -1, -2, -3 };
    }
}
