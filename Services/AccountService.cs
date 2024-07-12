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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
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

        public bool SignUpValidateUserName(string username)
        {
            Regex regexUsername = new Regex(@"^[a-z]+([._]?[a-z0-9]+)*$");
            
            if (!regexUsername.IsMatch(username))
            {
                return false;
            }
            
            return true;
        }

        public bool SignUpValidateUserPassword(string password)
        {
            Regex regexPassword = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

            if (!regexPassword.IsMatch(password))
            {
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
            var username = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return username ?? string.Empty;
        }

        public void SaveUserToDb(User user)
        {
            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();
        }
    }
}

