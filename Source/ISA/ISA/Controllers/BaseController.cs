using AutoMapper;
using ISA.DataAccess.Context;
using ISA.Filters;
using ISA.Models;
using ISA.Services;
using ISA.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ISA.Controllers
{
    [ServiceFilter(typeof(CustomAuthFilter))]
    public abstract class BaseController : Controller
    {

        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;
        public readonly IEmailService _emailSender;
        public readonly ISmsSender _smsSender;
        public readonly ILogger<BaseController> _logger;
        public readonly ISAContext _context;
        public readonly IMapper _mapper;

        public BaseController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailSender,
            ISmsSender smsSender,
            ILogger<BaseController> logger,
            ISAContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        protected ApplicationUser CurrentUser => CurrentUserWithProfile();

        private ApplicationUser CurrentUserWithProfile()
        {
            var user = GetCurrentUserAsync().Result;

            if (user is null)
            {
                return null;
            }

            if (user.UserProfileId.HasValue)
            {
                var profile = _context.UserProfiles
                    .Find(user.UserProfileId.Value);

                user.Profile = profile;

                return user;
            }

            return null;
        }

        protected Task<ApplicationUser> GetCurrentUserAsync()
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
