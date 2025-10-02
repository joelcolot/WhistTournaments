using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhistTournaments.DL.Enums;

namespace WhistTournaments.DL.Entities
{
    public class Tournament
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public Type_Tournament Type { get; set; }

        public DateTime RegistrationEndDate { get; set; }

        public DateTime StartDate { get; set; }

        public int RegisteredPlayers { get; set; }

        public int NbPlayers { get; set; }

        public int NbGames { get; set; }

        public bool OnGoing { get; set; } = false;

        public List<Game> Games { get; set; } = null!;


    }
}
