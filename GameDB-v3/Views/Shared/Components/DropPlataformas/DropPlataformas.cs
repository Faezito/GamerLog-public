using Microsoft.AspNetCore.Mvc;
using Z2.Services;

namespace GameDB_v3.Views.Shared.Components.DropPlataformas
{
    public class DropPlataformas : ViewComponent
    {
        private readonly IPlataformaServicos _sePlataforma;
        public DropPlataformas(IPlataformaServicos sePlataforma)
        {
            _sePlataforma = sePlataforma;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lst = await _sePlataforma.Listar(null, null);
            return View("Default", lst);
        }
    }
}
