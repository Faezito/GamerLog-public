using GameDB_v3.Libraries.Lang;
using GameDB_v3.Libraries.Login;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Z1.Model;
using Z1.Model.APIs;
using Z2.Services;
using Z2.Services.Externo;

namespace GameDB_v3.Controllers
{
    [Autorizacoes]
    public class JogoController : Controller
    {
        private readonly RawgService _rawg;
        private readonly IJogoServicos _jogos;
        private readonly IGeminiServicos _gemini;
        private readonly LoginUsuario _login;
        private readonly IRegistroJogoServicos _registro;

        public JogoController(RawgService rawg, IJogoServicos jogos, IGeminiServicos gemini, LoginUsuario login, IRegistroJogoServicos registro)
        {
            _rawg = rawg;
            _jogos = jogos;
            _gemini = gemini;
            _login = login;
            _registro = registro;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registro(int id)
        {
            try
            {
                var jogo = await _jogos.Obter(id);

                if (jogo == null)
                {
                    int jogoID = await _jogos.Cadastro(id, jogo);
                    jogo = await _jogos.Obter(jogoID);
                }
                return View("Registro", jogo);
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
        public async Task<IActionResult> Registro(int id, RegistroJogoModel registro)
        {
            var usuario = _login.GetCliente();
            var jogo = await _jogos.Obter(id);
            var tempoStr = string.IsNullOrWhiteSpace(registro.TempoJogadoString) ? "0" : registro.TempoJogadoString;
            registro.TempoJogado = decimal.Parse(tempoStr, new CultureInfo("pt-BR"));

            if (jogo == null)
            {
                try
                {
                    int jogoID = await _jogos.Cadastro(id, jogo);
                    jogo = await _jogos.Obter(jogoID);
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

            registro.JogoID = jogo.ID.Value;
            registro.UsuarioID = usuario.ID.Value;
            registro.DataAdicionado = DateTime.Now;

            await _registro.Inserir(registro);

            TempData["MSG_S"] = Mensagem.S_CADASTRADO;
            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("Index", "Usuario")
            });
        }

        [HttpPost]
        public async Task<IActionResult> ListarJogos(int? id, int? status, string? titulo)
        {
            try
            {
                var usuario = _login.GetCliente();
                var lst = await _jogos.Listar(id, titulo, status, usuario.ID.Value);

                return PartialView("_Tabela", lst);
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
