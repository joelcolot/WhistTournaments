using System.Security.Claims;

namespace WhistTournaments.Extensions
{
    public static class UsersExtensions
    {
        public static int GetId(this ClaimsPrincipal principal)
        {
            return int.Parse(principal.FindFirst(ClaimTypes.Sid)!.Value);
        }

        public static bool IsConnected(this ClaimsPrincipal claim) 
        {
            return claim.Identity.IsAuthenticated;
        }

        public static bool IsAdmin(this ClaimsPrincipal claim) 
        {
            return claim.IsInRole("Admin");
        }
    }
}
