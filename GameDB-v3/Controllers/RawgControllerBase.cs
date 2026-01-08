using Microsoft.AspNetCore.Mvc;
using Z2.Services.Externo;

namespace GameDB_v3.Controllers
{
    [ApiController]
    [Route("api/rawg")]
    public class RawgControllerBase : ControllerBase
    {
        private readonly RawgService _rawg;

        public RawgControllerBase(RawgService rawg)
        {
            _rawg = rawg;
        }

        [HttpGet("games")]
        public async Task<IActionResult> GetGames(int page = 1)
        {
            try
            {
                var games = await _rawg.ObterJogos(page);
                return Ok(games);
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
}
