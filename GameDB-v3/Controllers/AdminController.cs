using GameDB_v3.Libraries.Login;
using GenerativeAI;
using GenerativeAI.Types;
using Microsoft.AspNetCore.Mvc;
using Z1.Model;
using Z2.Services;
using Z2.Services.Externo;

namespace KitchenApp.Controllers
{
    [Autorizacoes(Tipos = "A")]
    public class AdminController : Controller
    {
        private readonly IUsuarioServicos _seUsuario;
        private readonly IEmailServicos _emailServicos;

        public AdminController(IUsuarioServicos seUsuario, IEmailServicos emailServicos)
        {
            _seUsuario = seUsuario;
            _emailServicos = emailServicos;
        }

        #region AdminUsers
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Usuarios()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Usuarios(int? id, string? Nome, string? email, string? GoogleId, string? usuario)
        {
            try
            {
                List<UsuarioModel> lst = await _seUsuario.Listar(id, Nome, email, GoogleId, usuario);
                return PartialView("_TabelaUsuarios", lst);
            }
            catch (Exception ex)
            {
                return Problem(
                    title: "Erro",
                    detail: ex.Message
                );
            }
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro(int? id)
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
                        detail: ex.Message
                    );
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(UsuarioModel model)
        {
            try
            {
                ModelState.Remove("Senha");
                ModelState.Remove("DataCriacao");
                ModelState.Remove("DataAtivacao");
                ModelState.Remove("DataDesativacao");

                if (ModelState.IsValid)
                {
                    await _seUsuario.Cadastrar(model);
                    TempData["MSG_S"] = "Usuário cadastrado com sucesso!";

                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return Problem(
                    title: "Erro",
                    detail: ex.Message
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> Deletar(int id)
        {
            UsuarioModel model = await _seUsuario.Obter(id, null, null);

            try
            {
                await _seUsuario.Deletar(model);
                TempData["MSG_S"] = "Deletado";
            }
            catch (Exception ex)
            {
                return Problem(
                    title: "Erro",
                    detail: ex.Message
                );
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> GerarSenha(int id)
        { 
            try
            {
                UsuarioModel usuario = await _seUsuario.Obter(id, null, null);
                string senhaNova = KeyGenerator.GetUniqueKey(6);
                usuario.Senha = senhaNova;

                await _seUsuario.AtualizarSenha(usuario);
                await _emailServicos.EnviarSenhaPorEmail(false, usuario, senhaNova);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Problem(
                    title: "Erro",
                    detail: ex.Message
                );
            }
        }

        #endregion
    }
}
