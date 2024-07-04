using Microsoft.AspNetCore.Mvc;
using EnglishWordsLearning.Data;
using EnglishWordsLearning.Models;
using EnglishWordsLearning.Interfaces;

namespace EnglishWordsLearning.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ProfileController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        
        [Route("Profile/{username}")]
        public IActionResult Index(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                RedirectToAction("SignIn", "Account");
            }

            var userProfile = _appDbContext.Users
                .Where(u => u.Username == username)
                .Select(u => new User
                {
                    Username = u.Username,
                    Password = u.Password // Do not expose password in the view
                })
                .FirstOrDefault();

            if (userProfile == null)
            {
                return NotFound();
            }
            
            return View(userProfile);
        }
    }
}