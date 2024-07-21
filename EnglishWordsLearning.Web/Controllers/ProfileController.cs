using EnglishWordsLearning.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Web.Controllers;

public class ProfileController : Controller
{
    private readonly IAccountService _accountService;

    public ProfileController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public IActionResult Index()
    {
        return RedirectToAction("SignIn", "Account");
    }
        
    [Route("Profile/{username}")]
    public IActionResult Index(string username)
    {
        var userProfile = _accountService.GetCurrentUser();
            
        return View(userProfile);
    }
}