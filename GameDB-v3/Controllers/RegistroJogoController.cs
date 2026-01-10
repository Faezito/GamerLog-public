using GameDB_v3.Extensions;
using GameDB_v3.Libraries.Filtros;
using GameDB_v3.Libraries.Lang;
using GameDB_v3.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Z1.Model;
using Z1.Model.APIs;
using Z2.Services;
using Z2.Services.Externo;

namespace GameDB_v3.Controllers
{
    [Autorizacoes]
    public class RegistroJogoController : Controller
    {
        private readonly RawgService _rawg;
        private readonly IJogoServicos _jogos;
        private readonly IGeminiServicos _gemini;
        private readonly LoginUsuario _login;
        private readonly IRegistroJogoServicos _registro;

        public RegistroJogoController(RawgService rawg, IJogoServicos jogos, IGeminiServicos gemini, LoginUsuario login, IRegistroJogoServicos registro)
        {
            _rawg = rawg;
            _jogos = jogos;
            _gemini = gemini;
            _login = login;
            _registro = registro;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var usuario = this.User.ObterUsuario();

                var jogo = await _jogos.Obter(id);

                if (jogo == null)
                {
                    try
                    {
                        int jogoID = await _jogos.Cadastro(id, jogo);
                        jogo = await _jogos.Obter(jogoID);
                    }
                    catch (Exception ex)
                    {
                        return Problem(title: "Erro ao carregar registros", detail: ex.Message);
                    }
                }

                var registros = await _registro.Listar(id, usuario.ID.Value);
                decimal? mediaAvaliacoes = registros.Any()
                        ? registros.Average(x => (decimal?)x.Nota)
                        : null; decimal? tempoJogado = registros.Sum(x => x.TempoJogado);
                string ultimaSessao = registros.Max(x=>x.UltimaSessao)?.ToString("dd/MM/yyyy");

                ViewBag.mediaAvaliacoes = mediaAvaliacoes;
                ViewBag.tempoJogado = tempoJogado;
                ViewBag.ultimaSessao = ultimaSessao;

                return View(jogo);
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
        public async Task<IActionResult> ListarRegistros(int id)
        {
            try
            {
                var usuario = this.User.ObterUsuario();
                List<RegistroJogoModel> registros = await _registro.Listar(id, usuario.ID.Value);

                return PartialView("_Registros", registros);
            }
            catch (Exception ex)
            {
                return Problem(title: "Erro ao carregar registros", detail: ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletar(int id, int JogoID)
        {
            try
            {
                var usuario = this.User.ObterUsuario();
                await _registro.Deletar(id, usuario.ID.Value);
                TempData["MSG_S"] = Mensagem.S_DELETADO;

                return RedirectToAction("Index", new { id = JogoID });
            }
            catch (Exception ex)
            {
                return Problem(
                    title: "Erro",
                    detail: ex.Message
                );
            }
        }
    }
}
