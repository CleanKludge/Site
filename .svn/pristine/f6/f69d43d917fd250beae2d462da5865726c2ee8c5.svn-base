using System.Linq;
using System.Threading.Tasks;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Api.Responses.Authentication;
using CleanKludge.Core.Authentication;
using CleanKludge.Server.Articles.Filters;
using CleanKludge.Services.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace CleanKludge.Server.Controllers
{
    [Authorize]
    [Route("account")]
    [DynamicArea(Location.Account)]
    public class AccountController : Controller
    {
        private readonly IPasswordHasher<UserLogin> _passwordHasher;
        private readonly UserService _userService;
        private readonly ILogger _logger;

        public AccountController(ILogger logger, UserService userService, IPasswordHasher<UserLogin> passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _logger = logger.ForContext<AccountController>();
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if(User.Identities.Any(identity => identity.IsAuthenticated))
            {
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            if(!ModelState.IsValid)
                return View(model);

            var result = _userService.Validate(UserLogin.From(model.Email, model.Password, _passwordHasher));
            if (result.Succeeded)
            {
                _logger.Information("User logged in.");

                var principal = result.User.ToPrincipal(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });

                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

           if (result.IsLockedOut)
           {
               _logger.Warning("User account locked out.");
               return View("Lockout");
           }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties { IsPersistent = true });
            _logger.Information("User logged out.");
            return RedirectToAction("Index", "Home");
        }
    }
}