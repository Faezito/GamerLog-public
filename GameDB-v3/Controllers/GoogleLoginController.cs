using GameDB_v3.Extensions;
using GameDB_v3.Libraries.Login;
using GameDB_v3.Libraries.Sessao;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Z1.Model;
using Z2.Services;
using Z2.Services.Externo;
using Z4.Bibliotecas;

namespace GameDB_v3.Controllers
{
    public class GoogleLoginController : Controller
    {
        private readonly IUsuarioServicos _seUsuario;
        private readonly IEmailServicos _emailServicos;
        private readonly LoginUsuario _login;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Sessao _sessao;

        public GoogleLoginController(IUsuarioServicos seUsuario,
            LoginUsuario login,
            IHttpContextAccessor httpContextAccessor,
            IEmailServicos emailServicos,
            Sessao sessao
            )
        {
            _seUsuario = seUsuario;
            _login = login;
            _httpContextAccessor = httpContextAccessor;
            _emailServicos = emailServicos;
            _sessao = sessao;
        }

        public IActionResult LoginGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "GoogleLogin");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded || result.Principal == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var claims = result.Principal.Claims;

            var id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var nome = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var nick = ManipularModels.GerarUsuario(email, id);

            // Verifica se usuário já existe em sua base
            UsuarioModel usuario = await _seUsuario.Obter(null, id, email);

            if (usuario == null)
            {
                // Cria novo usuário
                usuario = new UsuarioModel()
                {
                    NomeCompleto = nome,
                    Usuario = nick,
                    Email = email,
                    Tipo = "C", // padrão
                    GoogleId = id,
                    Senha = KeyGenerator.GetUniqueKey(10)
                };

                // TODO: Adicionar e-mail de confirmação de cadastro para o usuário
                int? novoid = await _seUsuario.Cadastrar(usuario);
                usuario = await _seUsuario.Obter(novoid, null, null);
            }

            // Configura claims internas do sistema
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.ID.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
            identity.AddClaim(new Claim(ClaimTypes.Name, usuario.NomeCompleto));
            identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Tipo));

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(3)
                });

            return RedirectToAction("Index", "Usuario", new { id = usuario.ID });
        }
    }
}
