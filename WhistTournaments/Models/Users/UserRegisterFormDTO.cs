using WhistTournaments.DL.Enums;

namespace WhistTournaments.Models.Users
{
    public class UserRegisterFormDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Gender Gender { get; set; }
    }
}
