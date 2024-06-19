using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnglishWordsLearning.Controllers;

public class TestController : Controller
{
    private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words.json");

    private async Task<List<WordViewModel>> LoadWordsAsync()
    {
        using (var reader = new StreamReader(_jsonFilePath))
        {
            var json = await reader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<List<WordViewModel>>(json)!;
        }
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
            
        int correctAnswers = HttpContext.Session.GetInt32("correctAnswers") ?? 0;
        int totalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;
            
        totalQuestions++;

        // Check the user's translation
        if (word != null && word.English.Equals(userTranslation, StringComparison.OrdinalIgnoreCase))
        {
            correctAnswers++;
            ViewBag.Result = "Correct!";
        }
        else
        {
            ViewBag.Result = "Incorrect. The correct translation is: " + (word?.English ?? "Unknown");
        }

        // Save correct answers and total questions back to session
        HttpContext.Session.SetInt32("correctAnswers", correctAnswers);
        HttpContext.Session.SetInt32("totalQuestions", totalQuestions);

        // Select a new random word
        var randomWord = words.OrderBy(w => Guid.NewGuid()).FirstOrDefault();

        // Pass the result and count to the view
        ViewBag.CorrectAnswers = correctAnswers;
        ViewBag.TotalQuestions = totalQuestions;
            
        return View(randomWord);
    }
}