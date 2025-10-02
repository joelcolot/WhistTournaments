using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using WhistTournaments.BLL.Services;
using WhistTournaments.DL.Entities;
using WhistTournaments.Extensions;
using WhistTournaments.Mappers;
using WhistTournaments.Models.Tournaments;

namespace WhistTournaments.Controllers
{
    public class TournamentController : Controller
    {
        private readonly TournamentService _tournamentService;
        private readonly GameService _gameService;
        private readonly UserService _userService;

        public TournamentController(TournamentService tournamentservice, GameService gameService, UserService userService)
        {
            _tournamentService = tournamentservice;
            _gameService = gameService;
            _userService = userService;
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

            TournamentDetailDto dto = tournament.ToTournamentDetailDto(games, _userService);

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
            if(!_tournamentService.AddTournament(tournament.FromTournamentDetailDto())) 
            {
                throw new Exception();
            }
            Console.WriteLine(tournament.RegEndDate);
            Console.WriteLine(tournament.StartDate);
            Console.WriteLine(tournament.Name);
            
            return RedirectToAction("Index", "Tournament");
        }
        
        [Authorize]
        public IActionResult Subscribe([FromRoute] int id)
        {
            Tournament tournament = _tournamentService.GetById(id);

            if (tournament.RegisteredPlayers < tournament.NbPlayers)
            {
                _tournamentService.SubscribeToTournament(id, User.GetId());
            }
            else
            {
                throw new Exception("Le tournoi ne peut plus accepter de nouveau joueurs.");
            }

            return RedirectToAction("Index", "Tournament");
        }

        [HttpGet("/tournament/update/{id}")]
        public IActionResult UpdateTournament([FromRoute] int id) 
        {
             TournamentDetailDto tournament=new TournamentDetailDto();
            List<Game> games = new List<Game>();
            games=_gameService.GetAllGamesByTournamentId(id);
//            if(games.Count!=0) 
//            {
                tournament = _tournamentService.GetById(id)!.ToTournamentDetailDto(games, _userService);
//            }
            Console.WriteLine("id="+id);
            return View(tournament);
        }

        [HttpPost("/tournament/update/{id}")]
        public IActionResult UpdateTournament([FromForm] TournamentDetailDto tournament,[FromRoute] int id) 
        {
            //int id = tournament.Id;
            Console.WriteLine("id="+id);
            if(_tournamentService.UpdateTournament(tournament.FromTournamentDetailDto(),id)) 
            {
                return RedirectToAction("Index","tournament");
            }
            Console.WriteLine("id="+id);
            throw new Exception();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult InitiateTournament([FromRoute] int id)
        {
            Tournament tournament = _tournamentService.GetById(id);

            tournament.OnGoing = true;

            if (tournament is null) throw new Exception("Erreur : pas de tournoi");

            if (tournament.RegisteredPlayers == tournament.NbPlayers)
            {
                _tournamentService.InitiateGames(id);

                
            }

            return RedirectToAction("Index", "Tournament");
        }
    }
}
