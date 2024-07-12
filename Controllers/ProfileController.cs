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
            var userProfile = _appDbContext.Users
                .Where(u => u.Username == username)
                .Select(u => new User
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Username = u.Username,
                    Password = u.Password
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