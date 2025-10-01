using Microsoft.AspNetCore.Mvc;
using WhistTournaments.BLL.Services;
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
            int j = 0;
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
            _userService.AddUser(user.FromUserRegisterDTO());
            return RedirectToAction("Index","Home");
        }

        //public IActionResult FindUser() 
        //{
        //    return View();
        //}

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
    }
}
