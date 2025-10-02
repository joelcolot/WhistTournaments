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
            return _tournamentRepository.UpdateTournament(tournament, id);
        }

        public void SubscribeToTournament(int tournament_id, int user_id)
        {
            _tournamentRepository.SubscribeToTournament(tournament_id, user_id);
        }

        public int GetCountSubscribedPlayersByTournamentId(int tournamentid)
        {
            return _tournamentRepository.GetCountSubscribedPlayersByTournamentId(tournamentid);
        }

        public void InitiateGames(int id)
        {
            //Création des tours suivants 
            for (int step = 1; step < 4; step++)
            {
                _tournamentRepository.InitiateGame(id ,step);

            }

            Random random = new Random();

            int player1 = 0;
            int player2 = 0;
            int player3 = 0;
            int player4 = 0;

            List<int> remainingplayers = _tournamentRepository.GetAllSubscribedPlayersByTournamentId(id);

            for (int step = 4; step < 8; step++)
            {
                player1 = remainingplayers[random.Next(remainingplayers.Count)];
                remainingplayers.Remove(player1);
                player2 = remainingplayers[random.Next(remainingplayers.Count)];
                remainingplayers.Remove(player2);
                player3 = remainingplayers[random.Next(remainingplayers.Count)];
                remainingplayers.Remove(player3);
                player4 = remainingplayers[random.Next(remainingplayers.Count)];
                remainingplayers.Remove(player4);

                _tournamentRepository.InitiateGame(id, step, player1, player2, player3, player4);
            }


        }
    }
}
