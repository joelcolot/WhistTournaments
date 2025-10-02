using WhistTournaments.BLL.Services;
using WhistTournaments.DL.Entities;
using WhistTournaments.Models.Games;

namespace WhistTournaments.Mappers
{
    public static class GameMappers
    {
        public static GameDto ToGameDto(this Game game, UserService userservice)
        {
            return new GameDto()
            {
                Step = game.Step,
                TournamentId = game.TournamentId,
                IdPlayer1 = game.Player1,
                IdPlayer2 = game.Player2,
                IdPlayer3 = game.Player3,
                IdPlayer4 = game.Player4,
                Player1 = userservice.GetUsernameById(game.Player1),
                Player2 = userservice.GetUsernameById(game.Player2),
                Player3 = userservice.GetUsernameById(game.Player3),
                Player4 = userservice.GetUsernameById(game.Player4),
                ScorePlayer1 = game.Score1,
                ScorePlayer2 = game.Score2,
                ScorePlayer3 = game.Score3,
                ScorePlayer4 = game.Score4,
                RankingPlayer1 = game.Ranking1,
                RankingPlayer2 = game.Ranking2,
                RankingPlayer3 = game.Ranking3,
                RankingPlayer4 = game.Ranking4,
            };
        }
    }
}
