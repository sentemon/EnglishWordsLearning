using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using EnglishWordsLearning.Models;

namespace EnglishWordsLearning.Controllers;

public class AccessController : Controller
{
    // GET
    public IActionResult Login()
    {
        ClaimsPrincipal claimsUser = HttpContext.User;

        if (claimsUser.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel modelLogin)
    {
        if (modelLogin.Username == "admin" &&
            modelLogin.Password == "admin")
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, modelLogin.Username),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme
            );

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = modelLogin.KeepLoggedIn
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("Index", "Home");
        }

        ViewData["ValidateMessage"] = "user not found";
        return View();
    }
}