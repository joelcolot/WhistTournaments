using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WhistTournaments.DL.Enums;

namespace WhistTournaments.Models.Games
{
    public class GameResultDto
    {
        [DisplayName("Joueur 1")]
        [Required]
        public string Player1 { get; set; }

        [DisplayName("Score Joueur 1")]
        [Required]

        public int Score1 { get; set; }

        [DisplayName("Joueur 2")]
        [Required]

        public string Player2 { get; set; }

        [DisplayName("Score Joueur 2")]
        [Required]

        public int Score2 { get; set; }

        [DisplayName("Joueur 3")]
        [Required]

        public string Player3 { get; set; }

        [DisplayName("Score Joueur 3")]
        [Required]

        public int Score3 { get; set; }

        [DisplayName("Joueur 4")]
        [Required]
        public string Player4 { get; set; }


        [DisplayName("Score Joueur 4")]
        [Required]

        public int Score4 { get; set; }

    }
}
