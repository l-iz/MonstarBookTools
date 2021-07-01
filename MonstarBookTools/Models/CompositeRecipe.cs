using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstarBookTools.Models
{
    public class CompositeRecipe
    {
        public enum SkillSelectType
        {
            MustSelect = -1,
            CanChangeLv = 0,
            Selectable = 1
        }

        public class SkillSelect
        {

            public SkillSelect(string name, IEnumerable<int> lvs, SkillSelectType compare)
            {
                Name = name;
                Lvs = (compare, lvs.Count()) switch
                {
                    (SkillSelectType.MustSelect, _) => lvs.OrderByDescending(l => l).Take(1).ToArray(),
                    (SkillSelectType.CanChangeLv, 1) => lvs.ToArray(),
                    (SkillSelectType.CanChangeLv, _) => lvs.OrderByDescending(l => l).ToArray(),
                    (SkillSelectType.Selectable, 1) => lvs.ToArray(),
                    _ => lvs.Append(0).OrderByDescending(l => l).ToArray()
                };
                SelectedLv = Lvs.First();
            }

            public string Name { get; }
            public int[] Lvs { get; }
            public bool IsChecked { get; set; }
            public int SelectedLv { get; set; }
        }

        private Card? mainCard;
        public Card? MainCard
        {
            get => mainCard;
            set
            {
                mainCard = value;
                Composite();
            }
        }

        private Card? subCard;
        public Card? SubCard
        {
            get => subCard;
            set
            {
                subCard = value;
                Composite();
            }
        }

        public SkillSelectType Type { get; set; }

        public Card? ResultCard { get; set; }

        private void Composite()
        {
            AllSkills = (MainCard?.Skills, SubCard?.Skills) switch
            {
                (Skill[] main, Skill[] sub) => main.Concat(sub).ToArray(),
                _ => Array.Empty<Skill>()
            };

            var lookup = AllSkills.ToLookup(s => s.Name, s => s.Lv);
            Type = (SkillSelectType)lookup.Count.CompareTo(MainCard?.Rare.MaxSkillCount ?? 4);

            SelectableSkill = lookup
                .Select(lk => new SkillSelect(lk.Key, lk.Distinct(), Type))
                .ToArray();

            SkillSelectChanged();
        }

        public void SkillSelectChanged()
        {
            if (MainCard != null)
            {
                switch (Type)
                {
                    case SkillSelectType.MustSelect:
                        ResultCard = MainCard with { Skills = SelectableSkill.Select(ss => new Skill(ss.Name, ss.Lvs.Max())).ToArray() };
                        MinRate = MaxRate = 1;
                        break;
                    case SkillSelectType.CanChangeLv:
                        ResultCard = MainCard with { Skills = SelectableSkill.Select(ss => new Skill(ss.Name, ss.Lvs.Length == 1 ? ss.Lvs[0] : ss.IsChecked ? ss.SelectedLv : 0)).ToArray() };
                        MinRate = MaxRate = (1 - (double)SelectableSkill.Count(s => s.IsChecked) / (ResultCard.Rare.MaxSkillCount + 1));
                        break;
                    case SkillSelectType.Selectable:
                        ResultCard = MainCard with { Skills = SelectableSkill.Where(ss => ss.IsChecked).Select(ss => new Skill(ss.Name, ss.SelectedLv)).Concat(Enumerable.Repeat(new Skill("***", 0), MainCard.Rare.MaxSkillCount)).Take(MainCard.Rare.MaxSkillCount).ToArray() };
                        var maxCount = AllSkills.Length;
                        var maxCountDistinct = SelectableSkill.Length;
                        var takeCount = ResultCard.Rare.MaxSkillCount;
                        var reqCount = SelectableSkill.Count(s => s.IsChecked);

                        MinRate = (double)Comb(maxCount - reqCount, takeCount - reqCount) / Comb(maxCount, takeCount);
                        MaxRate = (double)Comb(maxCountDistinct - reqCount, takeCount - reqCount) / Comb(maxCountDistinct, takeCount);
                        break;
                    default:
                        break;
                }
            }
        }

        private static int Comb(int n, int r)
        {
            if (r == 0 || r == n)
                return 1;
            else if (r == 1)
                return n;
            else
                return Comb(n - 1, r - 1) + Comb(n - 1, r);
        }

        public Skill[] AllSkills { get; private set; } = Array.Empty<Skill>();
        public int AllSkillCountDistinct => AllSkills.Select(s => s.Name).Distinct().Count();

        public SkillSelect[] SelectableSkill { get; private set; } = Array.Empty<SkillSelect>();
        public IEnumerable<SkillSelect> VisibleSkill => SelectableSkill.Where(s => Type == SkillSelectType.Selectable || (Type == SkillSelectType.CanChangeLv && s.Lvs.Length > 1));
        public double MinRate { get; private set; } = 1;
        public double MaxRate { get; private set; } = 1;

        public IEnumerable<int> GetTrailNums(double peoplePersentage)
        {
            var min = Math.Max(1, (int)Math.Ceiling(Math.Log(1 - peoplePersentage, 1 - MaxRate)));
            var max = Math.Max(1, (int)Math.Ceiling(Math.Log(1 - peoplePersentage, 1 - MinRate)));
            return new[] { min, max }.Distinct();
        }
    }
}
