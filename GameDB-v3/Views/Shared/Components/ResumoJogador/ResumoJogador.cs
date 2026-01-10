using GameDB_v3.Extensions;
using GameDB_v3.Libraries.Login;
using GenerativeAI.Types;
using Microsoft.AspNetCore.Mvc;
using Z1.Model;
using Z2.Services;

namespace GameDB_v3.Views.Shared.Components.Navbar
{
    public class ResumoJogador : ViewComponent
    {
        private readonly LoginUsuario _login;
        private readonly IJogoServicos _seJogo;

        public ResumoJogador(LoginUsuario login, IJogoServicos seJogo)
        {
            _login = login;
            _seJogo = seJogo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            UsuarioModel usuario = new();
            usuario = ViewContext.HttpContext.User.ObterUsuario();

            int anoAtual = DateTime.Now.Year;

            List<RegistroJogoModel> lstJogosUsuario = await _seJogo.ListarJogosDoUsuario(null, null, null, usuario.ID.Value, null);
            ViewBag.JogadoAnoAtual = lstJogosUsuario.Where(x => x.UltimaSessao?.Year == anoAtual).Count();
            ViewBag.JogadoAnoAnterior = lstJogosUsuario.Where(x => x.UltimaSessao?.Year == (anoAtual-1)).Count();
            ViewBag.GeneroFavorito = lstJogosUsuario
                                                .GroupBy(x => x.Genero)
                                                .OrderByDescending(g => g.Count())
                                                .Select(g => g.Key)
                                                .FirstOrDefault();

            ViewBag.JogoMaisJogado = lstJogosUsuario.OrderByDescending(x=>x.TempoJogado).Select(x=>x.Titulo).FirstOrDefault();
            ViewBag.QtdJogosZerados = lstJogosUsuario.Where(x=>x.DataZerado?.Year == (anoAtual - 1)).Count();
            ViewBag.QtdJogosPlatinados = lstJogosUsuario.Where(x=>x.DataPlatinado?.Year == (anoAtual - 1)).Count();
            ViewBag.QtdJogosAbandonados = lstJogosUsuario.Where(x=>x.Status == 0).Count();
            ViewBag.HorasJogadas = lstJogosUsuario.Select(x => x.TempoJogadoTotal).Sum();


            return View("Default");
        }
    }
}
