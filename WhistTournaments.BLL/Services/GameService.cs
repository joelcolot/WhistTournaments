using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhistTournaments.DAL.Repositories;
using WhistTournaments.DL.Entities;
using WhistTournaments.DL.Enums;

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

        public Step GetStepFromTournamentId(int tournamentid)
        {

            int step = _gameRepository.GetStepFromTournamentId(tournamentid);

            return (Step)step;
        }

        //public void WriteResults(int tournamentid, int step)
        //{
        //    if (step == 1)
        //    {
        //        _gameRepository.WriteResults
        //    }

        //}

        public List<Game> GetGamesTour1(int tournamentid)
        {
            return _gameRepository.GetGamesTour1(tournamentid);
        }

        public List<Game> GetGamesSemiFinals(int tournamentid)
        {
            return _gameRepository.GetGamesSemiFinals(tournamentid);
        }

        public Game GetGameFinal(int tournamentid)
        {
            return _gameRepository.GetGameFinal(tournamentid);
        }


    }
}
