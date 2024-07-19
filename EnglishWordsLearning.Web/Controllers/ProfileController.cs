using EnglishWordsLearning.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Web.Controllers;

public class ProfileController : Controller
{
    private readonly IAccountRepository _accountRepository;

    public ProfileController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public IActionResult Index()
    {
        return RedirectToAction("SignIn", "Account");
    }
        
    [Route("Profile/{username}")]
    public IActionResult Index(string username)
    {
        var userProfile = _accountRepository.GetCurrentUser();
            
        return View(userProfile);
    }
}