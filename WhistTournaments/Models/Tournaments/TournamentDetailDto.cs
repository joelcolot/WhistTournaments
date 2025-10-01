using System.ComponentModel;
using WhistTournaments.DL.Enums;
using WhistTournaments.Models.Games;

namespace WhistTournaments.Models.Tournaments
{
    public class TournamentDetailDto
    {
        [DisplayName("Id")]
        public int Id { get; set; }


        [DisplayName("Nom du Tournoi")]
        public string Name { get; set; }

        [DisplayName("Type")]

        public Type_Tournament Type { get; set; }

        [DisplayName("Fin des Inscriptions")]
        public DateTime RegEndDate { get; set; }

        [DisplayName("Date et heure de début")]
        public DateTime StartDate { get; set; }

        [DisplayName("Nombre de Joueurs Inscrits")]
        public int NbSubscribedPlayers { get; set; }

        [DisplayName("Nombre de Joueurs Maximum")]

        public int NbPlayers { get; set; }

        [DisplayName("Nombre total de Matchs")]

        public int NbMatchs { get; set; }

        [DisplayName("En Cours")]

        public bool OnGoing { get; set; }

        public List<GameDto>? gameDtos { get; set; }

    }
}
