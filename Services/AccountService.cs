using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EnglishWordsLearning.Data;
using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Controllers;


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
                var usernameClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                
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

