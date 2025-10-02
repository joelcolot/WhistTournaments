using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System.Configuration;
using System.Security.Claims;
using WhistTournaments.BLL.Services;
using WhistTournaments.DL.Entities;
using WhistTournaments.DL.Enums;
using WhistTournaments.Mappers;
using WhistTournaments.Models.Users;

namespace WhistTournaments.Controllers
{
    public class UserController:Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService) 
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            List<Gender> gender = [];
            foreach(Gender i in Enum.GetValues(typeof(Gender)))
            {
                gender.Add(i);
            }
            ViewData["Gender"]=gender;
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] UserRegisterFormDTO user) 
        {
            List<Gender> gender = [];
            foreach(Gender i in Enum.GetValues(typeof(Gender)))
            {
                gender.Add(i);
            }
            ViewData["Gender"]=gender;
            if(!ModelState.IsValid) 
            {
                user.Password="";
                user.RepeatPassword="";
                Console.WriteLine("Bad Model");
                return View();
            }
            if(!user.Password.Equals(user.RepeatPassword)) 
            {
                user.Password="";
                user.RepeatPassword="";
                Console.WriteLine("Passwords not matching");
                return View();
            }
            if(_userService.GetUserByUsername(user.UserName) is not null)
            {
                user.Password="";
                user.RepeatPassword="";
                Console.WriteLine("Username already used");
                return View();
            }
            user.Password = _userService.HashPassword(user.Password);

            if(!_userService.AddUser(user.FromUserRegisterDTO())) 
            {
                user.Password="";
                user.RepeatPassword="";
                Console.WriteLine("User not added");
                return View();
            }
            return RedirectToAction("Index","Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult OwnAccount() 
        {
            UserAccountDTO user = new UserAccountDTO();
            user=_userService.GetUserByUsername(User.Identity.Name).ToUserAccountDTO();
            return View(user);
        }

        [HttpGet("/User/CheckAccount/{id}")]
        public IActionResult CheckAccount([FromQuery]int id) 
        {
            UserAccountDTO user = _userService.GetUserById(id).ToUserAccountDTO();
            return View(user);
        }

        [HttpGet("/User/FindUser")]
        public IActionResult FindUser()
        {
            string username = "";
            Console.WriteLine("Beginings");
            return View(new UserAccountDTO());
        }

        [HttpPost("/User/CheckAccount/{username}")]
        public IActionResult FindUser([FromForm] UserAccountDTO user,[FromRoute] string username ) 
        {
            user=_userService.GetUserByUsername(username).ToUserAccountDTO();
            Console.WriteLine("Id = " + user.Id);
            return Ok(new { id = user.Id });
        }

        //[HttpPost]
        public IActionResult FindUsernameById() 
        {
            int id=1;       //  Not fully implemented
            ViewData["Username"]=_userService.GetUsernameById(id);
            return View();
        }


        //  Work in progress...
        [HttpPost]
        public IActionResult FindUserByUsername(string username) 
        {
            UserAccountDTO user = _userService.GetUserByUsername(username).ToUserAccountDTO();
            return View(user);
        }

        public IActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] UserLoginFormDTO login) 
        {
            if(!ModelState.IsValid) 
            {
                login.Password="";
                Console.WriteLine("Bad model state");
                return View(login);
            }
            User? user = _userService.GetUserByUsername(login.Username);
            if(user is null) 
            {
                login.Password="";
                Console.WriteLine("No user found");
                return View(login);
            }
            if(!_userService.VerifyPassword(login.Password, user.Password)) 
            {
                login.Password="";
                Console.WriteLine("Wrong password");
                return View(login);
            }
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity([
                    new Claim(ClaimTypes.Sid, user.Id.ToString() ),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.GivenName, (user.FirstName + " " + user.LastName))
                    ], CookieAuthenticationDefaults.AuthenticationScheme)
                );
            HttpContext.SignInAsync(claimsPrincipal);

            

            return RedirectToAction("Index","Home");
        }

        public IActionResult Logout() 
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

    }
}
