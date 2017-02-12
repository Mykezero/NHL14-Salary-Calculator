using System;
using System.Collections.Generic;
using System.Linq;

namespace NHL_14_Salary_Calculator.Classes
{
    public class Stats
    {
        public Dictionary<int, object> Map =>
            new Dictionary<int, object>()
            {
                { 0,  Index },
                { 1, Name },
                { 2, Team },
                { 3, Position },
                { 4,  GamesPlayed },
                { 5,  Goals },
                { 6,  Assists },
                { 7,  Points },
                { 8,  Shots },
                { 9,  Penalties },
                { 10,  GoalsPerGame },
                { 11,  PointsPerGame },
                { 12,  AssistsPerGame },
                { 13,  PlusMinus },
                { 14,  PowerPlayGoals },
                { 15,  ShortHandedGoals },
                { 16,  OverTimeGoals },
                { 17,  WinningGoals },
                { 18,  Hits },
                { 19,  PenaltyShotGoals },
                { 20,  FaceoffsWon },
                { 21,  Faceoffs },
                { 22,  FaceoffWonPercent },
                { 23,  GoalPercentPerShot }
            };


        public Stats(IEnumerable<string> stats)
        {
            MapStats(stats.ToList());
        }

        private void MapStats(IEnumerable<string> rawStats)
        {
            var stats = rawStats.Map((x) => x == "-" ? "0" : x).ToList();

            Index = Convert.ToInt32(stats[0]);
            Name = stats[1];
            Team = stats[2];
            Position = stats[3];
            GamesPlayed = Convert.ToInt32(stats[4]);
            Goals = Convert.ToInt32(stats[5]);
            Assists = Convert.ToInt32(stats[6]);
            Points = Convert.ToInt32(stats[7]);
            Shots = Convert.ToInt32(stats[8]);
            Penalties = Convert.ToInt32(stats[9]);
            GoalsPerGame = Convert.ToDouble(stats[10]);
            PointsPerGame = Convert.ToDouble(stats[11]);
            AssistsPerGame = Convert.ToDouble(stats[12]);
            var plusMinus = stats[13].Replace(" ", "");
            PlusMinus = plusMinus.Contains("-")
                ? Convert.ToInt32(plusMinus.Replace("-", "")) * -1
                : Convert.ToInt32(plusMinus.Replace("+", ""));
            PowerPlayGoals = Convert.ToInt32(stats[14]);
            ShortHandedGoals = Convert.ToInt32(stats[15]);
            OverTimeGoals = Convert.ToInt32(stats[16]);
            WinningGoals = Convert.ToInt32(stats[17]);
            Hits = Convert.ToInt32(stats[18]);
            PenaltyShotGoals = Convert.ToInt32(stats[19]);
            FaceoffsWon = Convert.ToInt32(stats[20]);
            Faceoffs = Convert.ToInt32(stats[21]);
            FaceoffWonPercent = Convert.ToDouble(stats[22].Replace("%", ""));
            GoalPercentPerShot = Convert.ToDouble(stats[23].Replace("%", ""));
        }

        public int Index { get; set; }
        public string Name { get; set; }
        public string Team { get; set; }
        public string Position { get; set; }
        public int GamesPlayed { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points { get; set; }
        public int Shots { get; set; }
        public int Penalties { get; set; }
        public double GoalsPerGame { get; set; }
        public double PointsPerGame { get; set; }
        public double AssistsPerGame { get; set; }
        public int PlusMinus { get; set; }
        public int PowerPlayGoals { get; set; }
        public int ShortHandedGoals { get; set; }
        public int OverTimeGoals { get; set; }
        public int WinningGoals { get; set; }
        public int Hits { get; set; }
        public int PenaltyShotGoals { get; set; }
        public int FaceoffsWon { get; set; }
        public int Faceoffs { get; set; }
        public double FaceoffWonPercent { get; set; }
        public double GoalPercentPerShot { get; set; }
    }
}