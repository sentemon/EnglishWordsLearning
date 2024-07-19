using EnglishWordsLearning.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EnglishWordsLearning.Web.ActionFilters;

public class AddUsernameToViewBagFilter : IActionFilter
{
    private readonly IAccountRepository _accountRepository;

    public AddUsernameToViewBagFilter(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is Controller controller)
        {
            var username = _accountRepository.GetCurrentUsername();
            controller.ViewBag.Username = username;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Do nothing
    }
}