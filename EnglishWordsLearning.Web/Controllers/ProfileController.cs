using EnglishWordsLearning.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Web.Controllers;

public class ProfileController : Controller
{
    private readonly IProfileRepository _profileRepository;

    public ProfileController(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public IActionResult Index()
    {
        return RedirectToAction("SignIn", "Account");
    }
        
    [Route("Profile/{username}")]
    public IActionResult Index(string username)
    {
        var userProfile = _profileRepository.GetUser(username);
            
        return View(userProfile);
    }
}