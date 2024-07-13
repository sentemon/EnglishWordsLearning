using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EnglishWordsLearning.Services;

namespace EnglishWordsLearning.Controllers
{
    [Authorize]
    public class WordsController : Controller
    {
        public async Task<IActionResult> Index(string? searchTerm = null, string level = "AllLevels")
        {
            var words = await LoadWordsHelper.LoadCsvWordsAsync(ViewData);
            
            words = words.Where(w => w.Level == level || level == "AllLevels" && w.English == searchTerm || searchTerm == null).ToList();
            
            return View(words);
        }

        
        
        /*
        public IActionResult AddWord()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddWord(Word wordViewModel)
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
        
        */
    }
}