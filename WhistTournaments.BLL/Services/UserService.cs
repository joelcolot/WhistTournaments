using Isopoh.Cryptography.Argon2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhistTournaments.DAL.Repositories;
using WhistTournaments.DL.Entities;

namespace WhistTournaments.BLL.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public bool AddUser(User user) 
        {
            return _userRepository.AddUser(user);
        }

        public string? GetUsernameById(int? id) 
        {
            string username=_userRepository.GetUsernameById(id);
            Console.WriteLine(username);
            return username;
        }

        public User GetUserById(int id) 
        {
            return _userRepository.GetUserById(id);
        }

        public User? GetUserByUsername(string username)
        {
            return _userRepository.GetUserByUsername(username);
        }

        public User? Login(string username) 
        {
            User? user = _userRepository.GetUserByUsername(username);
            return user;
        }

        public string HashPassword(string password)
        {
            return Argon2.Hash(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return Argon2.Verify(hashedPassword, password);
        }
    }
}
