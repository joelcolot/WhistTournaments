using WhistTournaments.DL.Entities;
using WhistTournaments.Models.Users;

namespace WhistTournaments.Mappers
{
    public static class UserMapper
    {
        public static User FromUserRegisterDTO(this UserRegisterFormDTO form) 
        {
            return new User()
            {
                UserName= form.UserName,
                Email= form.Email,
                FirstName= form.FirstName,
                LastName= form.LastName,
                Password= form.Password,
                Gender= form.Gender,
            };
        }

        public static UserAccountDTO ToUserAccountDTO(this User user) 
        {
            return new UserAccountDTO()
            {
                UserName=user.UserName,
                Email=user.Email,
                Gender=user.Gender,
                Role=user.Role,
            };
        }
    }
}
