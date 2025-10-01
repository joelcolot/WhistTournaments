using Microsoft.AspNetCore.Mvc;
using WhistTournaments.DL.Entities;
using WhistTournaments.Models.Tournaments;
using WhistTournaments.BLL.Services;
using WhistTournaments.Mappers;

namespace WhistTournaments.Controllers
{
    public class TournamentController : Controller
    {
        private readonly TournamentService _tournamentService;
        private readonly GameService _gameService;

        public TournamentController(TournamentService tournamentservice, GameService gameService)
        {
            _tournamentService = tournamentservice;
            _gameService = gameService;
        }



        public IActionResult Index()
        {
            List<Tournament> tournaments = _tournamentService.GetAll();

            List<TournamentIndexDto> dtos = tournaments
                .Select(t => t.ToTournamentIndexDto())
                .ToList();


            return View(dtos);
        }

        [HttpGet("/tournament/details/{id}")]
        public IActionResult Details([FromRoute] int id)
        {

            Tournament? tournament = _tournamentService.GetById(id);

            if (tournament is null) throw new Exception("Erreur : pas de tournoi");

            List<Game> games = _gameService.GetAllGamesByTournamentId(id);

            TournamentDetailDto dto = tournament.ToTournamentDetailDto(games);

            return View(dto);

        }

        [HttpGet]
        public IActionResult CreateTournament() 
        {
            return View(new TournamentDetailDto());
        }
        [HttpPost]
        public IActionResult CreateTournament([FromForm] TournamentDetailDto tournament) 
        {
            Console.WriteLine(tournament.RegEndDate);
            Console.WriteLine(tournament.StartDate);
            Console.WriteLine(tournament.Name);
            if(!_tournamentService.AddTournament(tournament.FromTournamentDetailDto())) 
            {
                throw new Exception();
            }

            return RedirectToAction("Index", "Tournament");
        }
    }
}
