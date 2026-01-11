using Microsoft.AspNetCore.Mvc;
using Z2.Services;

namespace GameDB_v3.Views.Shared.Components.DropMeses
{
    public class DropMeses : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Default");
        }
    }
}
