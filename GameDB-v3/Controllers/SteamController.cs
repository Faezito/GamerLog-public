using GameDB_v3.Extensions;
using GameDB_v3.Libraries.Login;
using GameDB_v3.Libraries.Sessao;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Web;
using Z2.Services;
using Z2.Services.Externo;

namespace GameDB_v3.Controllers
{
    public class SteamController : Controller
    {
        private readonly IUsuarioServicos _seUsuario;
        private readonly IEmailServicos _emailServicos;
        private readonly LoginUsuario _login;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Sessao _sessao;
        private readonly IAPIsServicos _api;
        private readonly ISteamServicos _steam;

        public SteamController(IUsuarioServicos seUsuario,
            LoginUsuario login,
            IHttpContextAccessor httpContextAccessor,
            IEmailServicos emailServicos,
            Sessao sessao,
            IAPIsServicos api,
            ISteamServicos steam
            )
        {
            _seUsuario = seUsuario;
            _login = login;
            _httpContextAccessor = httpContextAccessor;
            _emailServicos = emailServicos;
            _sessao = sessao;
            _api = api;
            _steam = steam;
        }

        public async Task<IActionResult> LoginSteam()
        {
            var returnUrl = Url.Action("Login", "Steam", null, Request.Scheme);
            var query = HttpUtility.ParseQueryString(string.Empty);
            var api = await _api.Obter(30);
            string SteamOpenIdUrl = api.Url;

            query["openid.ns"] = "http://specs.openid.net/auth/2.0";
            query["openid.mode"] = "checkid_setup";
            query["openid.return_to"] = returnUrl;
            query["openid.realm"] = $"{Request.Scheme}://{Request.Host}";
            query["openid.identity"] = "http://specs.openid.net/auth/2.0/identifier_select";
            query["openid.claimed_id"] = "http://specs.openid.net/auth/2.0/identifier_select";

            var redirectUrl = $"{SteamOpenIdUrl}?{query}";

            return Redirect(redirectUrl);
        }

        [Autorizacoes]
        public async Task<IActionResult> VincularSteam()
        {
            var returnUrl = Url.Action("Vincular", "Steam", null, Request.Scheme);
            var query = HttpUtility.ParseQueryString(string.Empty);
            var api = await _api.Obter(30);
            string SteamOpenIdUrl = api.Url;

            query["openid.ns"] = "http://specs.openid.net/auth/2.0";
            query["openid.mode"] = "checkid_setup";
            query["openid.return_to"] = returnUrl;
            query["openid.realm"] = $"{Request.Scheme}://{Request.Host}";
            query["openid.identity"] = "http://specs.openid.net/auth/2.0/identifier_select";
            query["openid.claimed_id"] = "http://specs.openid.net/auth/2.0/identifier_select";

            var redirectUrl = $"{SteamOpenIdUrl}?{query}";

            return Redirect(redirectUrl);
        }
        public async Task<IActionResult> Login()
        {
            if (!await ValidarSteamOpenId(Request))
                return Unauthorized("Resposta Steam inválida.");

            var claimedId = Request.Query["openid.claimed_id"].ToString();

            if (string.IsNullOrEmpty(claimedId))
                return Unauthorized("Login Steam falhou.");
            var steamId = claimedId.Split('/').Last();
            var player = await _steam.GetPlayerAsync(steamId);

            var usuario = await _seUsuario.ObterPorSteam(steamId);
            if (usuario == null)
                return Problem(title: "Usuário não encontrado.", detail: "Não encontramos um usuário cadastrado com esta conta, crie uma nova conta e depois vincule sua conta Steam.");

            // Configura claims internas do sistema
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.ID.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
            identity.AddClaim(new Claim(ClaimTypes.Name, usuario.NomeCompleto));
            identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Tipo));
            identity.AddClaim(new Claim("SteamID", usuario.steamid));

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(3)
                });

            // ATUALIZA DADOS
            if (usuario.personaname != player.personaname || usuario.avatarmedium != player.avatarmedium)
            {
                usuario.personaname = player.personaname;
                usuario.avatarmedium = player.avatarmedium;
                await _seUsuario.Cadastrar(usuario);
            }

            return RedirectToAction("Index", "Usuario", new { id = usuario.ID });
        }

        [Autorizacoes]
        public async Task<IActionResult> Vincular()
        {
            if (!await ValidarSteamOpenId(Request))
                return Unauthorized("Resposta Steam inválida.");

            var claimedId = Request.Query["openid.claimed_id"].ToString();

            if (string.IsNullOrEmpty(claimedId))
                return Unauthorized("Login Steam falhou.");
            var steamId = claimedId.Split('/').Last();
            var player = await _steam.GetPlayerAsync(steamId);

            var usuario = this.User.ObterUsuario();

            // Configura claims internas do sistema
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            var jaVinculado = await _seUsuario.ObterPorSteam(steamId);

            if (jaVinculado != null && jaVinculado.ID != usuario.ID)
                return Problem("Esta conta Steam já está vinculada a outro usuário.");

            usuario.steamid = player.steamid;
            usuario.personaname = player.personaname;
            usuario.profileurl = player.profileurl;
            usuario.avatarmedium = player.avatarmedium;

            // TODO: Adicionar e-mail de confirmação de cadastro para o usuário
            int? novoid = await _seUsuario.AdicionarSteam(usuario);
            usuario = await _seUsuario.Obter(novoid, null, null); // TODO: VERIFICAR SE PODE SER REMOVIDO

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.ID.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
            identity.AddClaim(new Claim(ClaimTypes.Name, usuario.NomeCompleto));
            identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Tipo));
            identity.AddClaim(new Claim("steamid", usuario.steamid));

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



        private async Task<bool> ValidarSteamOpenId(HttpRequest request)
        {
            using var client = new HttpClient();

            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (var key in request.Query.Keys)
                query[key] = request.Query[key];

            query["openid.mode"] = "check_authentication";

            var response = await client.PostAsync(
                "https://steamcommunity.com/openid/login",
                new StringContent(query.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded")
            );

            var result = await response.Content.ReadAsStringAsync();

            return result.Contains("is_valid:true");
        }
    }
}
