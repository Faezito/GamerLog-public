using GameDB_v3.Extensions;
using GenerativeAI.Types;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Z1.Model;
using Z2.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameDB_v3.Controllers
{
    public class TierlistController : Controller
    {
        private readonly IJogoServicos _jogo;

        public TierlistController(IJogoServicos jogo)
        {
            _jogo = jogo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Gerar(int? Ano, int? Mes, int UsuarioID)
        {
            List<RegistroJogoModel> lst = await _jogo.ListarJogosDoUsuario(null, null, null, UsuarioID, Ano, Mes);

            List<RegistroJogoModel> S = lst.Where(x => x.Tier == 5).OrderByDescending(x=>x.Nota).ToList();
            List<RegistroJogoModel> A = lst.Where(x => x.Tier == 4).OrderByDescending(x=>x.Nota).ToList();
            List<RegistroJogoModel> B = lst.Where(x => x.Tier == 3).OrderByDescending(x=>x.Nota).ToList();
            List<RegistroJogoModel> C = lst.Where(x => x.Tier == 2).OrderByDescending(x=>x.Nota).ToList();
            List<RegistroJogoModel> D = lst.Where(x => x.Tier == 1).OrderByDescending(x => x.Nota).ToList();

            ViewBag.S = S;
            ViewBag.A = A;
            ViewBag.B = B;
            ViewBag.C = C;
            ViewBag.D = D;

            return PartialView("_Tabela");
        }
    }
}
