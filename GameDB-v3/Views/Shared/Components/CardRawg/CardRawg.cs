using Microsoft.AspNetCore.Mvc;
using Z1.Model;
using Z1.Model.APIs;

namespace GameDB_v3.Views.Shared.Components.CardJogo
{
    public class CardRawg : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(RawgGameDto model)
        {
            return View("Default", model);
        }
    }
}
