using EnglishWordsLearning.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EnglishWordsLearning.ActionFilters;

public class AddUsernameToViewBagFilter : IActionFilter
{
    private readonly IAccountService _accountService;

    public AddUsernameToViewBagFilter(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is Controller controller)
        {
            var username = _accountService.GetCurrentUsername();
            controller.ViewBag.Username = username;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Do nothing
    }
}