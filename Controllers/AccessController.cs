using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EnglishWordsLearning.Data;
using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using EnglishWordsLearning.Services;
using EnglishWordsLearning.Interfaces;

namespace EnglishWordsLearning.Controllers
{
    public class AccessController : Controller
    {
        private readonly IAccountService _accountService;

        public AccessController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult SignIn()
        {
            var claimsUser = HttpContext.User;

            if (claimsUser.Identity != null && claimsUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel modelLogin)
        {
            // ProfileController.Index(modelLogin.Username)
            
            
            if (ModelState.IsValid)
            {
                if (_accountService.SignInValidateUser(modelLogin.Username, modelLogin.Password))
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new (ClaimTypes.NameIdentifier, modelLogin.Username)
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

                ViewData["ValidateMessage"] = "User not found";
            }
            return View(modelLogin);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the user fulfills the requirements
                    if (!_accountService.SignUpValidateUser(model.Username, model.Password))
                    {
                        return View(model);
                    }
                    
                    var newUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = model.Username,
                        Password = _accountService.HashPassword(model.Password)
                    };

                    // Add the new user to the database
                    _accountService.SaveUserToDb(newUser);

                    return RedirectToAction("SignIn");
                }
                catch (Exception ex)
                {
                    ViewData["ValidateMessage"] = $"Error saving data: {ex.Message}";
                }
            }
            else
            {
                ViewData["ValidateMessage"] = "Please correct the errors and try again.";
            }

            return View();
        }
    }
}