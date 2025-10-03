using WhistTournaments.BLL.Services;
using WhistTournaments.DL.Entities;
using WhistTournaments.DL.Enums;
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

        public static GameResultDto ToGameResultDto(this Game game, UserService userservice)
        {
            return new GameResultDto()
            {
                Player1 = userservice.GetUsernameById(game.Player1),
                Player2 = userservice.GetUsernameById(game.Player2),
                Player3 = userservice.GetUsernameById(game.Player3),
                Player4 = userservice.GetUsernameById(game.Player4),
            };
        }

        public static Game ToGame(this GameResultDto resultDto, UserService userservice)
        {
            return new Game()
            {
                Player1 = (userservice.GetUserByUsername(resultDto.Player1)).Id,
                Player2 = (userservice.GetUserByUsername(resultDto.Player2)).Id,
                Player3 = (userservice.GetUserByUsername(resultDto.Player3)).Id,
                Player4 = (userservice.GetUserByUsername(resultDto.Player4)).Id,
                Score1 = resultDto.Score1,
                Score2 = resultDto.Score2,
                Score3 = resultDto.Score3,
                Score4 = resultDto.Score4,

            };
        }
    }
}
