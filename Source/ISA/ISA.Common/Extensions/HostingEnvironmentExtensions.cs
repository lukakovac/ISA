using Microsoft.AspNetCore.Hosting;

namespace ISA.Common.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsNemanjaRistic(this IHostingEnvironment environment)
        {
            return environment.EnvironmentName == "NemanjaRistic";
        }

        public static bool IsLukaKovac(this IHostingEnvironment environment)
        {
            return environment.EnvironmentName == "LukaKovac";
        }

        public static bool IsDusanNikolic(this IHostingEnvironment environment)
        {
            return environment.EnvironmentName == "DusanNikolic";
        }
    }
}
