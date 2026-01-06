using GameDB_v3.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Z1.Model;
using Z2.Services;

namespace GameDB_v3.Views.Shared.Components.Navbar
{
    public class Navbar : ViewComponent
    {
        private readonly LoginUsuario _login;
        public Navbar(LoginUsuario login)
        {
            _login = login;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            UsuarioModel usuario = new();
            usuario = _login.GetCliente();

            return View("Default", usuario);
        }
    }
}
