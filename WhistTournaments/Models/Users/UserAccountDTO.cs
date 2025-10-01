using System.ComponentModel;
using WhistTournaments.DL.Enums;

namespace WhistTournaments.Models.Users
{
    public class UserAccountDTO
    {

        [DisplayName("Username")]
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Gender Gender { get; set; }
        public Role Role { get; set; }
    }
}
