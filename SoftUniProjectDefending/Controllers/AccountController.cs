using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using FaceitRankChecker.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FaceitRankChecker.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private string ErrorMessage;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }


        public IActionResult Login(string returnUrl)
        {
            var redirectUrl = Url.Action(nameof(Callback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Faceit", redirectUrl);
            return Challenge(properties, "Faceit");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Set the provider and returnUrl parameters to the desired values
            provider = "Faceit";
            returnUrl = "/Home/Index";
            string clientId = "e9e58299-32c8-425d-9d12-0b61f4955774";
            string clientSicret = "ggmW0rmIgTXbZakY1wMU0jcRiquBYpPP9Vu1OzLb";
            string auth = $"{clientId}:{clientSicret}";

            string faceitAuthUrl = "https://accounts.faceit.com/";
            faceitAuthUrl += "?response_type=code";
            faceitAuthUrl += $"&client_id={clientId}";
            faceitAuthUrl += "&redirect_popup=true";

            // Redirect the user to the Faceit login URL
            return Redirect(faceitAuthUrl);
        }
        public async Task<IActionResult> Callback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }


            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            throw new NotImplementedException();
        }

        private IActionResult Lockout()
        {
            return View();
        }
    }

    public class ExternalLoginViewModel
    {
        public string Email { get; set; }
    }

    
}
