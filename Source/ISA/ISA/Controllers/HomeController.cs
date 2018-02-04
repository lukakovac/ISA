using AutoMapper;
using ISA.DataAccess.Context;
using ISA.Models;
using ISA.Models.AccountViewModels;
using ISA.Models.HomeViewModels;
using ISA.Services;
using ISA.Services.EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ISA.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailSender,
            ISmsSender smsSender,
            ILogger<HomeController> logger,
            ISAContext context,
            IMapper mapper
        )
            : base(userManager, signInManager, emailSender, smsSender, logger, context, mapper)
        {
        }

        public IActionResult Index()
        {
            return View(new List<ReservationViewModel>());
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private void TestMethod(LoginViewModel vm)
        {
            vm.Email = "Some random email";
            vm.Password = "hehehehe";
        }
    }
}
