using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Authorization;
using EnglishWordsLearning.Helper;

namespace EnglishWordsLearning.Controllers
{
    [Authorize]
    public class WordsController : Controller
    {
        // ToDo: add all words so you can just watch them
        public async Task<IActionResult> Index(string level)
        {
            var words = await LoadWordsHelper.LoadJsonWordsAsync() ;
            if (!string.IsNullOrEmpty(level))
            {
                words = words.Where(w => w.Level == level).ToList();
            }
            return View(words);
        }
        
        public IActionResult AddWord()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddWord(WordViewModel wordViewModel)
        {
            if (ModelState.IsValid)
            {
                wordViewModel.Id = Guid.NewGuid();
                var words = await LoadWordsHelper.LoadJsonWordsAsync();
                words.Add(wordViewModel);

                // Save updated words back to the file
                var json = JsonConvert.SerializeObject(words, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(LoadWordsHelper.JsonWordsFilePath, json);

                return RedirectToAction("Index");
            }
            
            return View(wordViewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveWord(Guid id)
        {
            var words = await LoadWordsHelper.LoadJsonWordsAsync();
            var wordToRemove = words.FirstOrDefault(w => w.Id == id);
            if (wordToRemove != null)
            {
                words.Remove(wordToRemove);

                // Save updated words back to the file
                var json = JsonConvert.SerializeObject(words, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(LoadWordsHelper.JsonWordsFilePath, json);

                return RedirectToAction("Index");
            }

            return NotFound();
        }

    }
}