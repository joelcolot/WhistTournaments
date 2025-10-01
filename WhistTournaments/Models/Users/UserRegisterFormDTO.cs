using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WhistTournaments.DL.Enums;

namespace WhistTournaments.Models.Users
{
    public class UserRegisterFormDTO
    {
        [DisplayName("First name: ")]
        public string FirstName { get; set; }

        [DisplayName("Last name: ")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Username: ")]
        public string UserName { get; set; } = null!;

        [Required]
        [DisplayName("Email: ")]
        public string Email { get; set; } = null!;

        [Required]
        [DisplayName("Password: ")]
        public string Password { get; set; } = null!;

        [Required]
        [DisplayName("Password: ")]
        public string RepeatPassword {  get; set; } = null!;

        [DisplayName("Gender: ")]
        public Gender Gender { get; set; }
    }
}
