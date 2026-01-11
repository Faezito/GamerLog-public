using GameDB_v3.Extensions;
using GameDB_v3.Libraries.Filtros;
using GameDB_v3.Libraries.Lang;
using GameDB_v3.Libraries.Login;
using GameDB_v3.Libraries.Sessao;
using GenerativeAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
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


        public UsuarioController(IUsuarioServicos seUsuario, IEmailServicos emailServicos, Sessao sessao, LoginUsuario login)
        {
            _seUsuario = seUsuario;
            _emailServicos = emailServicos;
            _sessao = sessao;
            _login = login;
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
        [ValidateHttpRefererAttributes]
        public async Task<IActionResult> Cadastro(int? id, bool? senhaTemporaria)
        {
            UsuarioModel model = new();

            if (id.HasValue)
            {
                try
                {
                    model = await _seUsuario.Obter(id.Value, null, null);
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
            if(senhaTemporaria == true)
                model.SenhaTemporaria = senhaTemporaria.Value;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(UsuarioModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagens = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return Problem(
                        detail: string.Join("<br/>", mensagens),
                        title: "Erro de validação",
                        statusCode: StatusCodes.Status400BadRequest
                    );
                }
                model.Tipo = "C";
                int? id;

                //se não existir, cria
                if (!model.ID.HasValue)
                {
                    try
                    {
                        model.SenhaTemporaria = true;
                        var valido = ManipularModels.ValidarUsuario(model);
                        if (!valido.valido)
                            return Problem(detail: valido.mensagem, title: "Erro", statusCode: StatusCodes.Status400BadRequest);

                        model.Senha = KeyGenerator.GetUniqueKey(6);

                        id = await _seUsuario.Cadastrar(model);
                        await _emailServicos.EnviarSenhaPorEmail(true, model);
                    }
                    catch (Exception ex)
                    {
                        return Problem(
                            title: "Erro",
                            detail: ex.Message,
                            statusCode: StatusCodes.Status500InternalServerError
                        );
                    }

                    TempData["MSG_S"] = "Cadastrado com sucesso! Confira sua caixa de e-mail para prosseguir. (Confira sua caixa de spam e lixeira caso o e-mail não apareça.)";

                    return Json(new
                    {
                        success = true,
                        redirectUrl = Url.Action("Login", "Home")
                    });
                }
                // se existir, edita
                else
                {
                    try
                    {
                        model.SenhaTemporaria = false;
                        var valido = ManipularModels.ValidarUsuario(model);

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
            }
            // TODO: COLOCAR ESTE RETORNO PARA TODOS OS CATCH DE CONTROLLER
            catch (Exception ex)
            {
                return Problem(
                    title: "Erro",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
