using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using EnglishWordsLearning.Core.Interfaces;
using EnglishWordsLearning.Core.Models;


namespace EnglishWordsLearning.Core.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(IAccountRepository accountRepository, IHttpContextAccessor httpContextAccessor)
    {
        _accountRepository = accountRepository;
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }


    public bool SignInValidateUser(string username, string password)
    {
        var users = _accountRepository.LoadUsersFromDb();
        var user = users.FirstOrDefault(u => u.Username == username);

        return user != null && CheckHashPasswords(password, user.Password);
    }

    public bool SignUpValidateUserName(string username)
    {
        Regex regexUsername = new Regex(@"^[a-z]+([._]?[a-z0-9]+)*$");
            
        return regexUsername.IsMatch(username);
    }

    public bool SignUpValidateUserPassword(string password)
    {
        var regexPassword = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

        return regexPassword.IsMatch(password);
    }

    public bool SignUpValidateUserEmail(string? email)
    {
        if (email == null)
        {
            return false;
        }

        var regexEmail = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

        return regexEmail.IsMatch(email);
    }

    public bool SignUpValidateUserFullName(string firstName, string lastName)
    {
        var regexFullName = new Regex(@"^[a-zA-Z]+$");

        return regexFullName.IsMatch(firstName) && regexFullName.IsMatch(lastName);
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
        
    public string GetCurrentUsername()
    {
        var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return username ?? string.Empty;
    }

    public User GetCurrentUser()
    {
        var username = GetCurrentUsername();
        var user = _accountRepository.GetCurrentUser(username);

        return user;
    }

    public void SaveUserToDb(User user)
    {
        _accountRepository.SaveUserToDb(user);
    }

    public bool IsUsernameExists(string username)
    {
        var users = _accountRepository.LoadUsersFromDb();
        var user = users.FirstOrDefault(u => u.Username == username);

        return user == null;
    }
}