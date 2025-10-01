using System.ComponentModel.DataAnnotations;

namespace WhistTournaments.Models.Users
{
    public class UserLoginFormDTO
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
