using Microsoft.AspNetCore.Mvc;
using EnglishWordsLearning.Data;
using EnglishWordsLearning.Models;

namespace EnglishWordsLearning.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ProfileController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        
        [Route("Profile/{username?}")]
        public IActionResult Index(string? username)
        {
            // If no username is provided, use the current logged-in user's username
            if (string.IsNullOrEmpty(username))
            {
                username = User.Identity?.Name;

                if (string.IsNullOrEmpty(username))
                {
                    RedirectToAction("SignIn", "Access");
                }
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

            ViewData["username"] = userProfile.Username;
            
            return View(userProfile);
        }
    }
}