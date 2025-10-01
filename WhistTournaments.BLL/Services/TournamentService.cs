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

        public TournamentService(TournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public List<Tournament> GetAll()
        {
            return _tournamentRepository.GetAll();
        }
        public Tournament? GetById(int id)
        {
            return _tournamentRepository.GetById(id);
        }
    }
}
