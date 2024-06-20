using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EnglishWordsLearning.Data;
using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

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
                if (ValidateUser(modelLogin.Username, modelLogin.Password))
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

                    // Check if username already exists
                    if (users != null && users.Any(u => u.Username == model.Username))
                    {
                        ViewData["ValidateMessage"] = "Username already exists.";
                        return View(model);
                    }

                    // Create a new user
                    User newUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = model.Username,
                        Password = HashPassword(model.Password)
                    };

                    // Add the new user to the list
                    users?.Add(newUser);
                    _appDbContext.Users.Add(newUser);
                    _appDbContext.SaveChanges();

                    // Save updated users list back to JSON file
                    SaveUsersToJsonFile(users);

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

        private bool ValidateUser(string username, string password)
        {
            List<User>? users = LoadUsersFromJsonFile();
            var user = users?.FirstOrDefault(u => u.Username == username);

            if (user != null && user.Password == HashPassword(password))
            {
                return true;
            }

            return false;
        }

        private string HashPassword(string password)
        {
            // return BCrypt.Net.BCrypt.HashPassword(password);
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