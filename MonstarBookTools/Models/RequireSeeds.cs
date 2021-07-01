using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstarBookTools.Models
{
    public class RequireSeeds
    {
        public RequireSeeds(int rank, int cp, int chargeSeed, int takeSeed)
        {
            Rank = rank;
            Cp = cp;
            ChargeSeed = chargeSeed;
            TakeSeed = takeSeed;
        }

        public int Rank { get; }
        public int Cp { get; }
        public int ChargeSeed { get; }
        public int TakeSeed { get; }
    }
}
