using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using EnglishWordsLearning.Data;
using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using EnglishWordsLearning.Services;
using EnglishWordsLearning.Interfaces;

namespace EnglishWordsLearning.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
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
                    if (!SignUpValidateUser(model.Username, model.Password))
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
        //ToDo: wyswietlac dokladny blad
        public bool SignUpValidateUser(string username, string password)
        {
            List<User> users = _accountService.LoadUsersFromDb();
            var user = users.FirstOrDefault(u => u.Username == username);
            
            if (user != null)
            {
                ViewData["ValidateMessage"] = "Username already exists.";
                return false;
            }

            if (!(_accountService.SignUpValidateUserName(username) && _accountService.SignUpValidateUserPassword(password)))
            {
                if (!_accountService.SignUpValidateUserName(username))
                {
                    ViewData["ValidateMessage"] = "Username must contain only letters and numbers.";
                }
                else if (_accountService.SignUpValidateUserPassword(password))
                {
                    ViewData["ValidateMessage"] = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number and one special character.";
                }

                return false;
            }

            return true;
        }
    }
}