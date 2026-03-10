using Microsoft.AspNetCore.Mvc;
using Z1.Model;

namespace GameDB_v3.Views.Shared.Components.CardJogo
{
    public class CardJogo : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(RegistroJogoModel model)
        {
            return View("Default", model);
        }
    }
}
