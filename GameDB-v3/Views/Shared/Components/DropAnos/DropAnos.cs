using Microsoft.AspNetCore.Mvc;
using Z2.Services;

namespace GameDB_v3.Views.Shared.Components.DropAnos
{
    public class DropAnos : ViewComponent
    {
        private readonly IRegistroJogoServicos _reg;
        public DropAnos(IRegistroJogoServicos reg)
        {
            _reg = reg;
        }
        public async Task<IViewComponentResult> InvokeAsync(int usuarioId)
        {
            var lst = await _reg.ListarAnos(usuarioId);
            return View("Default", lst);
        }
    }
}
