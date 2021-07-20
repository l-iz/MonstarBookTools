using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstarBookTools.Models
{
    public record Distribution(int Value, double Probability)
    {
        public Distribution(Distribution d1, Distribution d2) : this(d1.Value + d2.Value, d1.Probability * d2.Probability) { }
    }
    public class StatusRateCalculator
    {
        public StatusType StatusType { get; set; } = StatusType.STAB;
        public int RankBonusCount { get; set; }
        public int TakumiMainSLv { get; set; }
        public int TakumiSubSLv { get; set; }
        public int RoomBonusCount { get; set; }
        public int RoomGrowBonusCount { get; set; }


        private IEnumerable<Distribution> RankUpDist => GetRate(RankBonusCount, 1.0 / 7, StatusType.RareBonusCorrect);
        private IEnumerable<Distribution> RoomUpDist => GetRate(RoomBonusCount, 1.0 / 7);
        private IEnumerable<Distribution> TakumiMainUpDist => GetRate(1, 1.0 / 7, TakumiMainSLv);
        private IEnumerable<Distribution> TakumiSubUpDist => GetRate(1, 1.0 / 7, TakumiSubSLv);

        public IEnumerable<Distribution> StatusDistribution => RankUpDist
            .SelectMany(rk => RoomUpDist.Select(rm => new Distribution(rk, rm)))
            .SelectMany(d => TakumiMainUpDist.Select(t => new Distribution(d, t)))
            .SelectMany(d => TakumiSubUpDist.Select(t => new Distribution(d, t)))
            .GroupBy(d => d.Value)
            .Select(g => new Distribution(g.Key, g.Sum(dd => dd.Probability)))
            .OrderBy(d => d.Value);

        public IEnumerable<Distribution> GrowDistribution => GetRate(RoomGrowBonusCount, 1.0 / 7);

        private static IEnumerable<Distribution> GetRate(int num, double rate, int aofI = 1)
        {
            return Enumerable.Range(0, num + 1)
                .Select(e => new Distribution(e * aofI, Math.Pow(rate, e) * Math.Pow(1 - rate, num - e) * Combin(num, e)));
        }

        private static int Combin(int n, int r)
        {
            return r == 0 ? 1 :
            Enumerable.Range(n - r + 1, r).Aggregate((x, y) => x * y) / Enumerable.Range(1, r).Aggregate((x, y) => x * y);
        }
    }
}
