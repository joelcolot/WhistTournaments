using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhistTournaments.DAL.Repositories;
using WhistTournaments.DL.Entities;

namespace WhistTournaments.BLL.Services
{
    public class GameService
    {
        private readonly GameRepository _gameRepository;

        public GameService(GameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public List<Game> GetAllGamesByTournamentId(int id)
        {
            return _gameRepository.GetAllGamesByTournamentId(id);
        }


    }
}
