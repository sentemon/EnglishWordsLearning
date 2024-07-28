using EnglishWordsLearning.Application.Common;
using EnglishWordsLearning.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EnglishWordsLearning.Core.Models;

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

    public async Task<IActionResult> CheckTranslation(string userLanguage = "Spanish", string level = "AllLevels")
    {
        if (!LoadWordsHelper.Levels.ContainsKey(level))
        {
            return BadRequest("Invalid level");
        }

        ViewBag.LanguageDictionary = LanguageDictionary.GetLanguageDictionary();
        ViewBag.UserLanguage = userLanguage;
        ViewBag.DisplayedLevel = LoadWordsHelper.Levels[level];
        ViewBag.SelectedLevel = level;

        var randomWord = await GetRandomTranslatedWord(LanguageDictionary.GetLanguage(userLanguage), level);

        return View(randomWord);
    }

    [HttpPost]
    public async Task<IActionResult> CheckTranslation(string englishWord, string userTranslation, string level, string userLanguage)
    {
        if (!LoadWordsHelper.Levels.ContainsKey(level))
        {
            return BadRequest("Invalid level");
        }

        ViewBag.LanguageDictionary = LanguageDictionary.GetLanguageDictionary();
        var words = await LoadWordsHelper.LoadCsvWordsAsync(level);
        var word = words.FirstOrDefault(w => w.English == englishWord);

        int correctAnswers = HttpContext.Session.GetInt32("correctAnswers") ?? 0;
        int totalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;

        if (word != null)
        {
            totalQuestions++;
            if (word.English.Equals(userTranslation, StringComparison.OrdinalIgnoreCase))
            {
                correctAnswers++;
                ViewBag.Result = "Correct!";
            }
            else
            {
                ViewBag.Result = $"Incorrect. The correct translation is: {word.English}";
            }
        }

        // Save correct answers and total questions back to session
        HttpContext.Session.SetString("level", LoadWordsHelper.Levels[level]);
        HttpContext.Session.SetString("userLanguage", userLanguage);
        HttpContext.Session.SetInt32("correctAnswers", correctAnswers);
        HttpContext.Session.SetInt32("totalQuestions", totalQuestions);

        // Pass the result and count to the view
        ViewBag.DisplayedLevel = LoadWordsHelper.Levels[level];
        ViewBag.SelectedLevel = level;
        ViewBag.UserLanguage = userLanguage;
        ViewBag.CorrectAnswers = correctAnswers;
        ViewBag.TotalQuestions = totalQuestions;

        var randomWord = await GetRandomTranslatedWord(LanguageDictionary.GetLanguage(userLanguage), level);

        return View(randomWord);
    }

    public IActionResult FinishTranslation()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            int totalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;
            int correctAnswers = HttpContext.Session.GetInt32("correctAnswers") ?? 0;
            double resultInPercentage = totalQuestions > 0 ? (double)correctAnswers / totalQuestions * 100 : 0.0;
            string level = HttpContext.Session.GetString("level") ?? "AllLevels";
            string username = ViewBag.Username;

            _historyLogsService.HistoryLogsOfTestsAdd(totalQuestions, correctAnswers, resultInPercentage, username, level);
        }
        
        HttpContext.Session.Remove("level");
        HttpContext.Session.Remove("userLanguage");
        HttpContext.Session.Remove("correctAnswers");
        HttpContext.Session.Remove("totalQuestions");

        return RedirectToAction("CheckTranslation");
    }

    private async Task<Word> GetRandomTranslatedWord(string userLanguage, string level)
    {
        var words = await LoadWordsHelper.LoadCsvWordsAsync(level);
        var randomWord = LoadWordsHelper.GetRandomWord(words);
        var translation = await _myMemoryService.TranslateAsync(userLanguage, randomWord.English);
        randomWord.Translation = translation;
        return randomWord;
    }
}