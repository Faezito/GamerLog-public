using GameDB_v3.Extensions;
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
using Z4.Bibliotecas;

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
            try
            {
                var usuario = this.User.ObterUsuario();
                var jogo = await _jogos.Obter(id);
                var tempoStr = string.IsNullOrWhiteSpace(registro.TempoJogadoString) ? "0" : registro.TempoJogadoString;
                registro.TempoJogado = decimal.Parse(tempoStr, new CultureInfo("pt-BR"));

                if (jogo == null)
                {
                    int jogoID = await _jogos.Cadastro(id, jogo);
                    jogo = await _jogos.Obter(jogoID);
                }

                registro.JogoID = jogo.ID.Value;
                registro.UsuarioID = usuario.ID.Value;
                registro.DataAdicionado = DateTime.Now;

                if (registro.UltimaSessao == null)
                {
                    if (registro.DataPlatinado == null)
                        registro.UltimaSessao = registro.DataZerado;
                    else
                        registro.UltimaSessao = registro.DataPlatinado;
                }

                var ret = ManipularModels.ValidarRegistro(registro);

                if (ret.valido == false)
                    return Problem(title: "Erro", detail: ret.mensagem);

                await _registro.Inserir(registro);

                TempData["MSG_S"] = Mensagem.S_CADASTRADO;
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Index", "Usuario")
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

        [HttpPost]
        public async Task<IActionResult> ListarJogos(int? id, int? status, string? titulo, int? Ordem)
        {
            try
            {
                var usuario = this.User.ObterUsuario();
                var lst = await _jogos.Listar(id, titulo, status, usuario.ID.Value);

                //switch (Ordem)
                //{
                //    case null: //sessao
                //        lst = lst;
                //        break;
                //    case 0: //sessao
                //        lst = lst.OrderBy(x => x.UltimaSessao).ToList();
                //        break;
                //    case 1: //jogando
                //        lst = lst.OrderBy(x => x.Status).ToList();
                //        break;
                //    case 2: //zerado
                //        lst = lst.OrderBy(x => x.DataZerado).ToList();
                //        break;
                //    case 3: //sessao
                //        lst = lst.OrderBy(x => x.DataPlatinado).ToList();
                //        break;
                //    case 4: //sessao
                //        lst = lst.OrderBy(x => x.Titulo).ToList();
                //        break;
                //}

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
