using Microsoft.AspNetCore.Mvc;

namespace GameDB_v3.Views.Shared.Components.Avaliacoes
{
    public class Avaliacoes : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();
        }
    }
}
