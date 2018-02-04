using ISA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ISA.Filters
{
    public class CustomAuthFilter : ActionFilterAttribute
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CustomAuthFilter> _logger;
        SignInManager<ApplicationUser> _signInManager;

        public CustomAuthFilter(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<CustomAuthFilter> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = GetCurrentUserAsync(context.HttpContext).Result;
            if (user is null && context.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                _signInManager.SignOutAsync().GetAwaiter().GetResult();
                var ctrl = context.Controller as Controller;
                context.Result = ctrl.RedirectToAction("Index", "Home");
            }
        }


        protected Task<ApplicationUser> GetCurrentUserAsync(HttpContext HttpContext)
        {
            try
            {
                return _userManager.GetUserAsync(HttpContext.User);
            }
            catch
            {
                return Task.FromResult<ApplicationUser>(null);
            }
        }
    }
}
