using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameDB_v3.Libraries.Filtros
{
    public class ValidateHttpRefererAttributes : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
                return;
            }

            //string referer = context.HttpContext.Request.Headers["Referer"].ToString();
            //if (string.IsNullOrEmpty(referer))
            //{
            //    var tempDataFactory = context.HttpContext
            //        .RequestServices
            //        .GetRequiredService<ITempDataDictionaryFactory>();

            //    var tempData = tempDataFactory.GetTempData(context.HttpContext);

            //    tempData["MSG_E"] = "Sessão inválida ou acesso não autorizado.";

            //    context.Result = new RedirectToActionResult("Login", "Home", null);

            //    //context.Result = new ContentResult() { Content = "Acesso negado." };
            //}
            //else
            //{
            //    Uri uri = new Uri(referer);

            //    string hostReferer = uri.Host;
            //    string hostServer = context.HttpContext.Request.Host.Host;

            //    if (hostReferer != hostServer)
            //    {
            //        var tempDataFactory = context.HttpContext
            //            .RequestServices
            //            .GetRequiredService<ITempDataDictionaryFactory>();

            //        var tempData = tempDataFactory.GetTempData(context.HttpContext);

            //        tempData["MSG_E"] = "Sessão inválida ou acesso não autorizado.";

            //        context.Result = new RedirectToActionResult("Login", "Home", null);
            //    }
            //}
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Possivel Logging
        }
    }
}