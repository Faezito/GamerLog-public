using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Z1.Model;
using GameDB_v3.Libraries.Login;

namespace GameDB_v3.Libraries.Filtros
{
    public class UsuarioAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        private string _autorizacao;

        public UsuarioAutorizacaoAttribute(string autorizacao)
        {
            _autorizacao = autorizacao;
        }

        LoginUsuario _login;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _login = (LoginUsuario)context.HttpContext.RequestServices.GetService(typeof(LoginUsuario));
            UsuarioModel model = _login.GetCliente();

            if (model == null)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }
            else
            {
                if (model.Tipo == "C")
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}