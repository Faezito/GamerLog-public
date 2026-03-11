using GameDB_v3.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Z1.Model.APIs;
using Z2.Services;
using Z2.Services.Externo;

namespace GameDB_v3.Controllers
{
    [Autorizacoes]
    [Route("AdicionarJogo")]
    public class RawgController : Controller
    {
        private readonly RawgService _rawg;
        private readonly IJogoServicos _jogos;

        public RawgController(RawgService rawg, IJogoServicos jogos)
        {
            _rawg = rawg;
            _jogos = jogos;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return Problem(title: "Erro ao listar jogos", detail: ex.Message);
            }
        }

        [HttpPost("Pesquisar")]
        public async Task<IActionResult> Pesquisar(int? id, string? titulo, int page = 1)
        {
            try
            {
                var games = await _rawg.ObterJogos(page, 16, titulo);
                return PartialView("_Tabela", games);
            }
            catch (Exception ex)
            {
                return Problem(
                    title: "Erro",
                    detail: ex.Message
                );
            }
        }


        [HttpPost("ListaInicial")]
        public async Task<IActionResult> ListaInicial(string? titulo)
        {
            try
            {
                var lst = await _jogos.ListarInicial();

                return PartialView("_Tabela", lst);
            }
            catch (Exception ex)
            {
                return Problem(title: "Erro ao listar jogos", detail: ex.Message);
            }
        }
    }

}
