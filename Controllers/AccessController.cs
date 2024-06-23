using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EnglishWordsLearning.Data;
using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace EnglishWordsLearning.Controllers
{
    public class AccessController : Controller
    {
        private readonly string _usersFilePath;
        private readonly AppDbContext _appDbContext;

        public AccessController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _usersFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");
        }

        public IActionResult SignIn()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;

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
                if (SignInValidateUser(modelLogin.Username, modelLogin.Password))
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new (ClaimTypes.NameIdentifier, modelLogin.Username),
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
                    // Load existing users from JSON file
                    List<User>? users = LoadUsersFromJsonFile();

                    // Check if the user fulfills the requirements
                    if (!SignUpValidateUser(model.Username, model.Password))
                    {
                        return View(model);
                    }

                    // Create a new user
                    User newUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = model.Username,
                        Password = HashPassword(model.Password)
                    };

                    // Add the new user to the list to the JSON file
                    users?.Add(newUser);
                    SaveUsersToJsonFile(users);

                    // Add the new user to the database
                    _appDbContext.Users.Add(newUser);
                    _appDbContext.SaveChanges();

                    

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

            return View(model);
        }

        private bool SignInValidateUser(string username, string password)
        {
            List<User>? users = LoadUsersFromJsonFile();
            var user = users?.FirstOrDefault(u => u.Username == username);

            if (user != null && user.Password == HashPassword(password))
            {
                return true;
            }

            return false;
        }

        private bool SignUpValidateUser(string username, string password)
        {
            List<User>? users = LoadUsersFromJsonFile();
            var user = users?.FirstOrDefault(u => u.Username == username);

            Regex regexUsername = new Regex(@"^[a-zA-Z0-9]+([._]?[a-zA-Z0-9]+)*$");
            Regex regexPassword = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

            if (!regexUsername.IsMatch(username) || !regexPassword.IsMatch(password))
            {
                if (!regexUsername.IsMatch(username))
                {
                    ViewData["ValidateMessage"] = "Username must contain only letters and numbers.";
                }
                else if (!regexPassword.IsMatch(password))
                {
                    ViewData["ValidateMessage"] = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number and one special character.";
                }

                return false;
            }

            if (user != null)
            {
                ViewData["ValidateMessage"] = "Username already exists.";
                return false;
            }

            return true;
        }

        private string HashPassword(string password)
        {
            // return BCrypt.Net.BCrypt.HashPassword(password); // (change it)
            return password;
        }

        private List<User>? LoadUsersFromJsonFile()
        {
            if (!System.IO.File.Exists(_usersFilePath))
            {
                return new List<User>(); // Return an empty list if file doesn't exist
            }

            string json = System.IO.File.ReadAllText(_usersFilePath);
            return JsonConvert.DeserializeObject<List<User>>(json);
        }

        private void SaveUsersToJsonFile(List<User>? users)
        {
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            System.IO.File.WriteAllText(_usersFilePath, json);
        }
    }
}