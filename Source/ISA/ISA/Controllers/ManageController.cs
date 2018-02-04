using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ISA.Models;
using ISA.Models.ManageViewModels;
using ISA.Services;
using Microsoft.AspNetCore.Authentication;
using ISA.Services.EmailService;
using ISA.DataAccess.Context;
using AutoMapper;
using System.Collections.Generic;
using ISA.DataAccess.Models.Enumerations;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ISA.DataAccess.Models;

namespace ISA.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailService emailSender,
          ISmsSender smsSender,
          ILogger<ManageController> logger,
          ISAContext context,
          IMapper mapper)
            : base(userManager, signInManager, emailSender, smsSender, logger, context, mapper)
        {

        }

        //
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var user = CurrentUser;
            if (user is null) return View("Error");

            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
            };

            if (!(user.Profile is null))
            {
                _mapper.Map(user.Profile, model);

                var friends = _context.FriendRequests
                    .Include(x => x.Sender)
                    .Include(x => x.Receiver)
                    .Where(r => (r.SenderId == user.Profile.Id || r.ReceiverId == user.Profile.Id)
                              && r.Status == FriendshipStatus.Accepted)
                              .Select(r => r.SenderId == user.Profile.Id ? r.Receiver : r.Sender)
                              .Distinct()
                              .Select(_mapper.Map<FriendViewModel>)
                              .ToList();

                model.Friends = new List<FriendViewModel>();
                model.Friends.AddRange(friends);
            }

            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
        {
            ManageMessageId? message = ManageMessageId.Error;
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    message = ManageMessageId.RemoveLoginSuccess;
                }
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public IActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            await _smsSender.SendSmsAsync(model.PhoneNumber, "Your security code is: " + code);
            return RedirectToAction(nameof(VerifyPhoneNumber), new { model.PhoneNumber });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(1, "User enabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(2, "User disabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            // Send an SMS to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
                }
            }
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, "Failed to verify phone number");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemovePhoneSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User changed their password successfully.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //GET Manage/AddFriends
        public async Task<IActionResult> AddFriends()
        {
            var user = await GetCurrentUserAsync();
            if (user != null && user.UserProfileId.HasValue)
            {
                var profile = _context.UserProfiles.Find(user.UserProfileId.Value);

                if (profile != null)
                {

                    var friends = _context.FriendRequests
                        .Include(x => x.Sender)
                        .Include(x => x.Receiver)
                        .Where(r => (r.SenderId == profile.Id || r.ReceiverId == profile.Id)
                                  && (r.Status == FriendshipStatus.Accepted
                                        || r.Status == FriendshipStatus.Blocked
                                        || r.Status == FriendshipStatus.Pending))
                                  .Select(r => r.SenderId == profile.Id ? r.Receiver : r.Sender)
                                  .Distinct()
                                  .Select(x => x.Id)
                                  .ToList();

                    var nonFriends = _context.UserProfiles
                        .Where(x => !friends.Any(f => f == x.Id) && x.Id != profile.Id)
                        .Select(_mapper.Map<FriendViewModel>)
                        .ToList();

                    return View(nonFriends);
                }
            }

            return Unauthorized();
        }

        public async Task<IActionResult> SendFriendRequest(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("id");
            }

            var user = await GetCurrentUserAsync();
            if (user != null && user.UserProfileId.HasValue)
            {
                var userProfile = _context.UserProfiles.Find(user.UserProfileId.Value);

                if (userProfile != null)
                {
                    var friendship = _context.FriendRequests
                        .Where(x => ((x.ReceiverId == userProfile.Id && x.SenderId == id)
                        || (x.ReceiverId == id && x.SenderId == userProfile.Id))
                                    && x.Status == FriendshipStatus.Accepted)
                        .ToList();

                    if (friendship.Any())
                    {
                        //TODO: add messages like for index
                        // ALREADY_FRIENDS
                        return RedirectToAction(nameof(AddFriends));
                    }

                    var user2 = _context.UserProfiles.Find(id.Value);
                    if (user2 is null)
                    {
                        return BadRequest("userid");
                    }

                    _context.FriendRequests.Add(new FriendRequest
                    {
                        SenderId = userProfile.Id,
                        ReceiverId = id.Value,
                        Status = FriendshipStatus.Pending
                    });
                    _context.SaveChanges();
                    return RedirectToAction(nameof(AddFriends));
                }
            }

            return RedirectToAction(nameof(AddFriends));
        }

        public async Task<IActionResult> RemoveFriend(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("ID");
            }

            var user = await GetCurrentUserAsync();
            if (user != null && user.UserProfileId.HasValue)
            {
                var userProfile = _context.UserProfiles.Find(user.UserProfileId.Value);

                if (userProfile != null)
                {
                    var friendship = _context.FriendRequests
                        .Where(x => ((x.ReceiverId == userProfile.Id && x.SenderId == id)
                        || (x.ReceiverId == id && x.SenderId == userProfile.Id))
                                    && x.Status == FriendshipStatus.Accepted)
                        .SingleOrDefault();

                    if (friendship is null)
                    {
                        return NotFound("friendship");
                    }

                    friendship.Status = FriendshipStatus.Declined;
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return NotFound("userProfile");
            }
            return NotFound("user");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserProfile([Bind("EmailAddress,FirstName,LastName,TelephoneNr,City")] IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (CurrentUser?.Profile is null)
            {
                ModelState.AddModelError(string.Empty, $"UserProfile not found");
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
            }

            _mapper.Map(model, CurrentUser.Profile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        //
        // GET: /Manage/SetPassword
        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //GET: /Manage/ManageLogins
        [HttpGet]
        public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.AddLoginSuccess ? "The external login was added."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            var otherLogins = schemes.Where(auth => userLogins.All(ul => auth.Name != ul.LoginProvider)).ToList();
            ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action(nameof(LinkLoginCallback), "Manage");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return Challenge(properties, provider);
        }

        //
        // GET: /Manage/LinkLoginCallback
        [HttpGet]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            if (info == null)
            {
                return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(user, info);
            var message = ManageMessageId.Error;
            if (result.Succeeded)
            {
                message = ManageMessageId.AddLoginSuccess;
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        //private Task<ApplicationUser> GetCurrentUserAsync()
        //{
        //    return _userManager.GetUserAsync(HttpContext.User);
        //}

        #endregion
    }
}
