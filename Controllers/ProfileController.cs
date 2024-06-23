using System.Security.Claims;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using EnglishWordsLearning.Data;
using EnglishWordsLearning.Models;

namespace EnglishWordsLearning.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileController (AppDbContext context)
        {
            _context = context;
        }

        [Route("Profile/{username?}")]
        public IActionResult Index(string username)
        {
            var userProfile = _context.Users
                .Where(u => u.Username == username)
                .Select(u => new User
                {
                    Username = u.Username,
                    Password = u.Password
                })
                .FirstOrDefault();

            if (userProfile == null)
            {
                return NotFound();
            }

            ViewBag.Username = userProfile;
            
            return View(userProfile);
        }
        

    }
}