using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Authorization;

namespace EnglishWordsLearning.Controllers
{
    [Authorize]
    public class WordsController : Controller
    {
        private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words.json");

        private async Task<List<WordViewModel>> LoadWordsAsync()
        {
            using (var reader = new StreamReader(_jsonFilePath))
            {
                var json = await reader.ReadToEndAsync();
                
                return JsonConvert.DeserializeObject<List<WordViewModel>>(json) ?? throw new InvalidOperationException();
            }
        }

        public async Task<IActionResult> Index(string level)
        {
            var words = await LoadWordsAsync();
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
                var words = await LoadWordsAsync();
                words.Add(wordViewModel);

                // Save updated words back to the file
                var json = JsonConvert.SerializeObject(words, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(_jsonFilePath, json);

                return RedirectToAction("Index");
            }
            
            return View(wordViewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveWord(Guid id)
        {
            var words = await LoadWordsAsync();
            var wordToRemove = words.FirstOrDefault(w => w.Id == id);
            if (wordToRemove != null)
            {
                words.Remove(wordToRemove);

                // Save updated words back to the file
                var json = JsonConvert.SerializeObject(words, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(_jsonFilePath, json);

                return RedirectToAction("Index");
            }

            return NotFound();
        }

    }
}