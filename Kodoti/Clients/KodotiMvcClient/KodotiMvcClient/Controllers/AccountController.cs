using KodotiMvcClient.Proxies;
using KodotiMvcClient.Proxies.Models.Auth;
using KodotiMvcClient.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KodotiMvcClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthProxy _authProxy;

        public AccountController(IAuthProxy authProxy)
        {
            _authProxy = authProxy;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _authProxy.Authenticate(new LoginAuthModel
            {
                Email = model.Email,
                Password = model.Password
            });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.user_id),
                new Claim(ClaimTypes.Email, result.user_email),
                new Claim("access_token", result.access_token)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { });

            return Redirect(model.ReturnUrl ?? "~/");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("~/");
        }
    }
}