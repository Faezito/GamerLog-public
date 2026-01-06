using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;
using Z1.Model;

namespace GameDB_v3.Libraries.Login
{
    public class AutorizacoesAttribute : Attribute, IAuthorizationFilter
    {
        public string Tipos { get; set; } // Ex: "Admin,Gerente"

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Recupera serviço de login
            var loginUsuario = (LoginUsuario)context.HttpContext.RequestServices
                .GetService(typeof(LoginUsuario));

            var usuario = loginUsuario.GetCliente();

            // Se não estiver logado
            if (usuario == null)
            {
                //context.HttpContext.Items["MSG_E"] = "Sessão expirada, faça login novamente.";
                context.Result = new JsonResult(new
                {
                    success = false,
                    swal = new
                    {
                        icon = "error",
                        title = "Sessão expirada",
                        text = "Faça login novamente"
                    }
                });

                context.Result = new RedirectToActionResult("Login", "Home", null);
                return;
            }

            // Se nenhum cargo foi exigido, permite acesso
            if (string.IsNullOrEmpty(Tipos))
                return;

            // Converte string em lista
            var tiposPermitidos = Tipos.Split(',')
                                         .Select(c => c.Trim().ToLower())
                                         .ToList();

            var cargoUsuario = usuario.Tipo.ToLower();

            // Se usuário não possui cargo ou cargo não está na lista:
            if (string.IsNullOrEmpty(cargoUsuario) ||
                !tiposPermitidos.Contains(cargoUsuario))
            {
                //context.HttpContext.Items["MSG_E"] = "Acesso negado.";

                context.Result = new JsonResult(new
                {
                    success = false,
                    swal = new
                    {
                        icon = "error",
                        title = "Acesso negado!",
                        text = "Você não tem permissão para acessar esta página."
                    }
                });
                context.Result = new RedirectToActionResult("Login", "Home", null);
                return;
            }
        }
    }
}
