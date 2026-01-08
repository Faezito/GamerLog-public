//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Z1.Model;
//using GameDB_v3.Libraries.Login;
//using Z2.Services;

//namespace GameDB_v3.Libraries.Filtros
//{
//    public class UsuarioAutorizacaoAttribute : Attribute, IAuthorizationFilter
//    {
//        private string _autorizacao;
//        private readonly IUsuarioServicos _usuario;
//        LoginUsuario _login;

//        public UsuarioAutorizacaoAttribute(string autorizacao, IUsuarioServicos usuario)
//        {
//            _autorizacao = autorizacao;
//            _usuario = usuario;
//        }

//        // TODO: REVISAR ESSA OBTENÇÃO DE USUÁRIO E TODA ESSA CLASSE
//        public async void OnAuthorization(AuthorizationFilterContext context)
//        {
//            _login = (LoginUsuario)context.HttpContext.RequestServices.GetService(typeof(LoginUsuario));
//            int? id = _login.GetUserId();
//            UsuarioModel model = await _usuario.Obter(id, null, null);

//            if (model == null)
//            {
//                context.Result = new RedirectToActionResult("Login", "Home", null);
//            }
//            else
//            {
//                if (model.Tipo == "C")
//                {
//                    context.Result = new ForbidResult();
//                }
//            }
//        }
//    }
//}