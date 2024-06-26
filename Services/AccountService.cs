using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EnglishWordsLearning.Data;
using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.Text.RegularExpressions;


namespace EnglishWordsLearning.Services
{
    public class AccountService : Interfaces.IAccountService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ViewComponent _viewComponent;

        public AccountService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public bool SignInValidateUser(string username, string password)
        {
            List<User> users = LoadUsersFromDb();
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user != null && user.Password == HashPassword(password))
            {
                return true;
            }

            return false;
        }
        
        public bool SignUpValidateUser(string username, string password)
        {
            List<User> users = LoadUsersFromDb();
            var user = users.FirstOrDefault(u => u.Username == username);
            
            if (user != null)
            {
                _viewComponent.ViewData["ValidateMessage"] = "Username already exists.";
                return false;
            }

            Regex regexUsername = new Regex(@"^[a-zA-Z0-9]+([._]?[a-zA-Z0-9]+)*$");
            Regex regexPassword = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

            if (!regexUsername.IsMatch(username) || !regexPassword.IsMatch(password))
            {
                if (!regexUsername.IsMatch(username))
                {
                    _viewComponent.ViewData["ValidateMessage"] = "Username must contain only letters and numbers.";
                }
                else if (!regexPassword.IsMatch(password))
                {
                    _viewComponent.ViewData["ValidateMessage"] = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number and one special character.";
                }

                return false;
            }

            return true;
        }
        
        public string HashPassword(string password)
        {
            // return BCrypt.Net.BCrypt.HashPassword(password); // (change it)
            return password;
        }

        public List<User> LoadUsersFromDb()
        {
            var users = _appDbContext.Users;
            
            return users.ToList();
        }
        
        public string GetCurrentUsername()
        {
            if (_viewComponent.HttpContext.User.Identity is ClaimsIdentity identity)
            {
                Claim? usernameClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                return usernameClaim?.Value ?? string.Empty;
            }
            
            return string.Empty;
        }

        public void SaveUserToDb(User user)
        {
            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();
        }
    }
}

