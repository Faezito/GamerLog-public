using GameDB_v3.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Z2.Services.Externo;

namespace GameDB_v3.Controllers
{
    [Autorizacoes]
    [Route("jogos")]
    public class RawgController : Controller
    {
        private readonly RawgService _rawg;

        public RawgController(RawgService rawg)
        {
            _rawg = rawg;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View();
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

    }

}
