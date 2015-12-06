using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors
{
    class GameHistory
    {
        [PrimaryKey, AutoIncrement]
        public int Game { get; set; }

        public string NamePlayerOne { get; set; }
        public string NamePlayerTwo { get; set; }
        public string RoundOne { get; set; }
        public string RoundTwo { get; set; }
        public string RoundThree { get; set; }
        public string Winner { get; set; }
        public int PointsPlayerOne { get; set; }
        public int PointsPlayerTwo { get; set; }
    }
}
