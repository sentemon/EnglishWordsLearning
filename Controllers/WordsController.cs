using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using EnglishWordsLearning.Models;

namespace EnglishWordsLearning.Controllers
{
    public class WordsController : Controller
    {
        private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words.json");

        private async Task<List<Word>> LoadWordsAsync()
        {
            using (var reader = new StreamReader(_jsonFilePath))
            {
                var json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<List<Word>>(json);
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
        public async Task<IActionResult> AddWord(Word word)
        {
            if (ModelState.IsValid)
            {
                var words = await LoadWordsAsync();
                words.Add(word);

                // Save updated words back to the file
                var json = JsonConvert.SerializeObject(words, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(_jsonFilePath, json);

                return RedirectToAction("Index");
            }
            
            return View(word);
        }

        public async Task<IActionResult> Test()
        {
            var words = await LoadWordsAsync();
            var randomWord = words.OrderBy(w => Guid.NewGuid()).FirstOrDefault();
            return View(randomWord);
        }

        [HttpPost]
        public async Task<IActionResult> Test(string russian, string userTranslation)
        {
            var words = await LoadWordsAsync();
            var word = words.FirstOrDefault(w => w.Russian == russian);

            if (word != null && word.English.Equals(userTranslation, StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.Result = "Correct!";
            }
            else
            {
                ViewBag.Result = "Incorrect. The correct translation is: " + (word?.English ?? "Unknown");
            }

            var randomWord = words.OrderBy(w => Guid.NewGuid()).FirstOrDefault();
            return View(randomWord);
        }
    }
}