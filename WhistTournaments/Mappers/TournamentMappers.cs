using WhistTournaments.DL.Entities;
using WhistTournaments.Models.Games;
using WhistTournaments.Models.Tournaments;

namespace WhistTournaments.Mappers
{
    public static class TournamentMappers
    {
        public static TournamentIndexDto ToTournamentIndexDto(this Tournament tournament)
        {
            return new TournamentIndexDto()
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Type = tournament.Type,
                RegEndDate = tournament.RegistrationEndDate,
                StartDate = tournament.StartDate,
                NbSubscribedPlayers = tournament.NbSubscribedPlayers,
                NbPlayers = tournament.NbPlayers,
                NbMatchs = tournament.NbGames,
                OnGoing = tournament.OnGoing,
            };
        }

        public static TournamentDetailDto ToTournamentDetailDto(this Tournament tournament, List<Game> games)
        {
            

            return new TournamentDetailDto()
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Type = tournament.Type,
                RegEndDate = tournament.RegistrationEndDate,
                StartDate = tournament.StartDate,
                NbSubscribedPlayers = tournament.NbSubscribedPlayers,
                NbPlayers = tournament.NbPlayers,
                NbMatchs = tournament.NbGames,
                OnGoing = tournament.OnGoing,
                gameDtos = games
                    .Select(g => g.ToGameDto())
                    .ToList()
            };
        }

        public static Tournament FromTournamentDetailDto(this TournamentDetailDto tournament) 
        {
            return new Tournament() 
            {
                Id=tournament.Id,
                Name=tournament.Name,
                Type=tournament.Type,
                RegistrationEndDate=tournament.RegEndDate,
                StartDate=tournament.StartDate,
                NbSubscribedPlayers=tournament.NbSubscribedPlayers,
                NbPlayers=tournament.NbPlayers,
                NbGames=tournament.NbMatchs,
                OnGoing=tournament.OnGoing,
            };
        }
    }
}
