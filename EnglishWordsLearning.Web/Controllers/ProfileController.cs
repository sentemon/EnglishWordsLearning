using EnglishWordsLearning.Core.Interfaces;
using EnglishWordsLearning.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Web.Controllers;

    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [Route("Profile/{username?}")]
        public async Task<IActionResult> Index(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("SignIn","Account");
            }
            
            var userProfile = await _profileService.GetUserProfile(username);
            

            var userDto = new UserDto
            {
                Username = userProfile.Username,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = userProfile.Email
            };
            
            return View(userDto);
        }

        [Route("Profile/Edit/{username?}")]
        public async Task<IActionResult> Edit(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("SignIn","Account");
            }
            
            var userProfile = await _profileService.GetUserProfile(username);
            
            
            var userDto = new UserDto
            {
                Username = userProfile.Username,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = userProfile.Email
            };
            
            return View(userDto);
        }

        [HttpPost]
        [Route("Profile/Edit/{username?}")]
        public async Task<IActionResult> Edit(string username, UserDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _profileService.GetUserProfile(username);
            
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            
            var success = await _profileService.UpdateUserProfile(username, user);
            
            if (!success)
            {
                ViewBag.AddModelError("", "Failed to update profile.");
                
                return View(model);
            }
            
            return RedirectToAction("Index", "Profile");
        }

        // [Route("Profile/ChangePassword/{username}")]
        // public IActionResult ChangePassword(string username)
        // {
        //     var userProfile = _profileService.GetUserProfile(username);
        //     
        //     if (string.IsNullOrEmpty(userProfile.Username))
        //     {
        //         return RedirectToAction("SignIn","Account");
        //     }
        //     return View(new ChangePassword { Username = username });
        // }
        //
        // [HttpPost]
        // [Route("Profile/ChangePassword/{username}")]
        // public IActionResult ChangePassword(string username, ChangePassword model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(model);
        //     }
        //
        //     var success = _profileService.ChangePassword(username, model);
        //     if (!success)
        //     {
        //         ModelState.AddModelError("", "Failed to change password.");
        //         return View(model);
        //     }
        //
        //     return RedirectToAction("Index", new { Username = username });
        // }
    }