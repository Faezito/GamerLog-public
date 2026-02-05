using GameDB_v3.Extensions;
using GameDB_v3.Libraries.Filtros;
using GameDB_v3.Libraries.Lang;
using GameDB_v3.Libraries.Login;
using GameDB_v3.Libraries.Sessao;
using GenerativeAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using System.Web;
using Z1.Model;
using Z2.Services;
using Z2.Services.Externo;
using Z4.Bibliotecas;

namespace GameDB_v3.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioServicos _seUsuario;
        private readonly IEmailServicos _emailServicos;
        private readonly Sessao _sessao;
        private readonly LoginUsuario _login;
        private readonly IJogoServicos _jogos;
        private readonly ISteamServicos _steam;
        private readonly IAPIsServicos _api;

        public UsuarioController(IUsuarioServicos seUsuario,
            IEmailServicos emailServicos,
            Sessao sessao,
            LoginUsuario login,
            ISteamServicos steam,
            IAPIsServicos api)
        {
            _seUsuario = seUsuario;
            _emailServicos = emailServicos;
            _sessao = sessao;
            _login = login;
            _steam = steam;
            _api = api;

        }

        [Autorizacoes]
        [ValidateHttpRefererAttributes]
        public async Task<IActionResult> Index(int? id)
        {
            UsuarioModel model = new();
            if (id == null)
            {
                id = this.User.GetUserId();
            }
            model = await _seUsuario.Obter(id, null, null);

            return View(model);
        }

        [HttpGet]
        [Autorizacoes]
        [ValidateHttpRefererAttributes]
        public async Task<IActionResult> Edicao()
        {
            try
            {
                int id = this.User.GetUserId();

                UsuarioModel model = new();
                try
                {
                    model = await _seUsuario.Obter(id, null, null);
                }
                catch (Exception ex)
                {
                    return Problem(
                        title: "Erro",
                        detail: ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                }

                return View(model);
            }
            catch (Exception ex)
            {
                return Problem(
                    title: "Erro",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edicao(UsuarioModel model)
        {
            try
            {
                if (this.User.GetUserId() != model.ID)
                    return Forbid();

                var valido = ManipularModels.ValidarUsuario(model);
                model.SenhaTemporaria = false;

                if (!valido.valido)
                    return Problem(detail: valido.mensagem, title: "Erro", statusCode: StatusCodes.Status400BadRequest);

                await _seUsuario.Cadastrar(model);
                TempData["MSG_S"] = Mensagem.S_EDITADO;

                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Index", "Usuario", model)

                });
            }
            catch (Exception ex)
            {
                return Problem(
                    title: "Erro",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterPerfilSteam(string steamId)
        {
            Player player = await _steam.GetPlayerAsync(steamId);
            return View(player);
        }


    }
}
