using System.Security.Claims;
using EnglishWordsLearning.Core.Models;
using System.Text.RegularExpressions;
using EnglishWordsLearning.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using EnglishWordsLearning.Core.Interfaces;


namespace EnglishWordsLearning.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountRepository(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }


    public bool SignInValidateUser(string username, string password)
    {
        List<User> users = LoadUsersFromDb();
        var user = users.FirstOrDefault(u => u.Username == username);

        if (user != null && CheckHashPasswords(password, user.Password))
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

    public bool SignUpValidateUserEmail(string? email)
    {
        if (email == null)
        {
            return false;
        }

        Regex regexEmail = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

        if (!regexEmail.IsMatch(email))
        {
            return false;
        }

        return true;
    }

    public bool SignUpValidateUserFullName(string firstName, string lastName)
    {
        Regex regexFullName = new Regex(@"^[a-zA-Z]+$");

        if (!regexFullName.IsMatch(firstName) || !regexFullName.IsMatch(lastName))
        {
            return false;
        }

        return true;
    }
        
    public string HashPassword(string password)
    {
        var hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
        return hashPassword;
    }

    public bool CheckHashPasswords(string enterPassword, string userPassword)
    {
        var hashPassword = BCrypt.Net.BCrypt.Verify(enterPassword, userPassword);

        return hashPassword;
    }

    public List<User> LoadUsersFromDb()
    {
        var users = _appDbContext.Users;
            
        return users.ToList();
    }
        
    public string GetCurrentUsername()
    {
        var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return username ?? string.Empty;
    }

    public User GetCurrentUser()
    {
        var username = GetCurrentUsername();
        var user = _appDbContext.Users.Single(u => u.Username == username);

        return user;
    }

    public void SaveUserToDb(User user)
    {
        _appDbContext.Users.Add(user);
        _appDbContext.SaveChanges();
    }
}