using EnglishWordsLearning.Application.Common;
using EnglishWordsLearning.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Web.Controllers;

public class TestController : Controller
{
    private readonly IHistoryLogsService _historyLogsService;
    private readonly IMyMemoryService _myMemoryService;

    public TestController(IHistoryLogsService historyLogsService, IMyMemoryService myMemoryService)
    {
        _historyLogsService = historyLogsService;
        _myMemoryService = myMemoryService;
    }

    // private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words.json");

    public async Task<IActionResult> CheckTranslation(string userLanguage = "Ukrainian", string level = "AllLevels")
    {
        userLanguage = LanguageDictionary.GetLanguageDictionary(userLanguage);
        
        ViewBag.DisplayedLevel = LoadWordsHelper.Levels[level];
        ViewBag.SelectedLevel = level;

        var words = await LoadWordsHelper.LoadCsvWordsAsync(level);
        var randomWord = LoadWordsHelper.GetRandomWord(words);
        
        var translation = await _myMemoryService.TranslateAsync(userLanguage, randomWord.English);
        randomWord.Translation = translation;

        return View(randomWord);
    }

    [HttpPost]
    public async Task<IActionResult> CheckTranslation(string translation, string userTranslation, string level, string userLanguage = "Ukrainian")
    {
        userLanguage = LanguageDictionary.GetLanguageDictionary(userLanguage);
        
        var words = await LoadWordsHelper.LoadCsvWordsAsync(level);
        var word = words.FirstOrDefault(w => w.Translation == translation);
        
        int correctAnswers = HttpContext.Session.GetInt32("correctAnswers") ?? 0;
        int totalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;

        var randomWord = LoadWordsHelper.GetRandomWord(words);
        translation = await _myMemoryService.TranslateAsync(userLanguage, randomWord.English);
        randomWord.Translation = translation;

        if (word != null)
        {
            randomWord = LoadWordsHelper.GetRandomWord(words);
            translation = await _myMemoryService.TranslateAsync(userLanguage, randomWord.English);
            randomWord.Translation = translation;
            
            totalQuestions++;

            if (word.English.Equals(userTranslation, StringComparison.OrdinalIgnoreCase))
            {
                correctAnswers++;
                ViewBag.Result = "Correct!";
            }
            else
            {
                ViewBag.Result = "Incorrect. The correct translation is: " + word.English;
            }
        }


        // Save correct answers and total questions back to session
        HttpContext.Session.SetString("level", LoadWordsHelper.Levels[level]);
        HttpContext.Session.SetInt32("correctAnswers", correctAnswers);
        HttpContext.Session.SetInt32("totalQuestions", totalQuestions);


        // Pass the result and count to the view
        ViewBag.CorrectAnswers = correctAnswers;
        ViewBag.TotalQuestions = totalQuestions;

        ViewBag.DisplayedLevel = LoadWordsHelper.Levels[level];
        ViewBag.SelectedLevel = level;

        return View(randomWord);
    }



    public IActionResult FinishTranslation()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            int totalQuestions = Convert.ToInt32(HttpContext.Session.GetInt32("totalQuestions"));
            int correctAnswers = Convert.ToInt32(HttpContext.Session.GetInt32("correctAnswers"));
            double resultInPercentage = totalQuestions > 0 ? (double)correctAnswers / totalQuestions * 100 : 0.0;
            string level = HttpContext.Session.GetString("level") ?? "AllLevels";
            string username = ViewBag.Username;

            _historyLogsService.HistoryLogsOfTestsAdd(totalQuestions, correctAnswers, resultInPercentage, username, level);
        }
        
        HttpContext.Session.Remove("level");
        HttpContext.Session.Remove("correctAnswers");
        HttpContext.Session.Remove("totalQuestions");

        return RedirectToAction("CheckTranslation");
    }
}