using System.Security.Claims;

namespace ISA.Data
{
    public static class Constants
    {
        #region SYSTEM ROLES (KEEP IN SYNC)
        public static string[] SYSTEM_ROLES = new[] { "User", "SuperAdmin", "FunZoneAdmin", "MovieAdmin" };

        public const string ROLE_USER = "User";
        public const string ROLE_SUPERADMIN = "SuperAdmin";
        public const string ROLE_FUNZONEADMIN = "FunZoneAdmin";
        public const string ROLE_MOVIEADMIN = "MovieAdmin";
        #endregion
    }


    public static class Expressions
    {
        public static bool IsAnon(ClaimsPrincipal user)
        {
            return !user.Identity.IsAuthenticated;
        }

        public static bool IsFunZoneAdmin(ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated && (user.IsInRole("FunZoneAdmin") || user.IsInRole("SuperAdmin"));
        }
    }
}
