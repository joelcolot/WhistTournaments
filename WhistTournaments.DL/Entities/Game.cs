using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhistTournaments.DL.Enums;

namespace WhistTournaments.DL.Entities
{
    public class Game
    {
        public int Id { get; set; }

        public int TournamentId { get; set; }

        public int Step { get; set; }

        public int? Player1 {  get; set; }
        public int? Player2 { get; set; }

        public int? Player3 { get; set; }

        public int? Player4 { get; set; }

        public int? Score1 { get; set; }

        public int? Score2 { get; set; }

        public int? Score3 { get; set; }

        public int? Score4 { get; set; }

        public int? Ranking1 { get; set; }

        public int? Ranking2 { get; set; }

        public int? Ranking3 { get; set; }

        public int? Ranking4 { get; set; }

    }
}
