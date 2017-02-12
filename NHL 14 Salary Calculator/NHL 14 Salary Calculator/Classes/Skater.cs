using System.Collections.Generic;

namespace NHL_14_Salary_Calculator.Classes
{
    public class Skater
    {
        public Stats Stats { get; set; }

        public Skater(IEnumerable<string> stats)
        {
            Stats = new Stats(stats);
        }
    }
}