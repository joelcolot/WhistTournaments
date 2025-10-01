using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhistTournaments.DAL.Repositories;
using WhistTournaments.DL.Entities;

namespace WhistTournaments.BLL.Services
{
    public class TournamentService
    {
        private readonly TournamentRepository _tournamentRepository;
        private readonly GameRepository _gameRepository;

        public TournamentService(TournamentRepository tournamentRepository, GameRepository gameRepository)
        {
            _tournamentRepository = tournamentRepository;
            _gameRepository = gameRepository;
        }

        public List<Tournament> GetAll()
        {
            return _tournamentRepository.GetAll();
        }
        public Tournament? GetById(int id)
        {
            Tournament? tournament = _tournamentRepository.GetById(id);
            if(tournament is not null) 
            {
                List<Game> games=_gameRepository.GetAllGamesByTournamentId(id);
                tournament.Games = games;
            }
            return tournament;
        }

        public bool AddTournament(Tournament tournament) 
        {
            return _tournamentRepository.Add(tournament);
        }

        public bool UpdateTournament(Tournament tournament,int id) 
        {
            return _tournamentRepository.Add(tournament);
        }
    }
}
