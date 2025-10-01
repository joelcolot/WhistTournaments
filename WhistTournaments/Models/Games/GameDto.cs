using System.ComponentModel;
using WhistTournaments.DL.Enums;

namespace WhistTournaments.Models.Games
{
    public class GameDto
    {
        [DisplayName("Etape")]

        public int Step { get; set; }

        [DisplayName("Nom du Tournoi")]

        public int TournamentId { get; set; }

        [DisplayName("ID Joueur 1")]

        public int IdPlayer1 { get; set; }

        [DisplayName("ID Joueur 2")]

        public int IdPlayer2 { get; set; }

        [DisplayName("ID Joueur 3")]

        public int IdPlayer3 { get; set; }

        [DisplayName("ID Joueur 4")]

        public int IdPlayer4 { get; set; }

        [DisplayName("Joueur 1")]

        public string Player1 { get; set; } = null!;

        [DisplayName("Joueur 2")]

        public string Player2 { get; set; } = null!;

        [DisplayName("Joueur 3")]

        public string Player3 { get; set; } = null!;

        [DisplayName("Joueur 4")]

        public string Player4 { get; set; } = null!;

        [DisplayName("Score Joueur 1")]

        public int ScorePlayer1 { get; set; }

        [DisplayName("Score Joueur 2")]

        public int ScorePlayer2 { get; set; }

        [DisplayName("Score Joueur 3")]

        public int ScorePlayer3 { get; set; }

        [DisplayName("Score Joueur 4")]

        public int ScorePlayer4 { get; set; }

    }
}
