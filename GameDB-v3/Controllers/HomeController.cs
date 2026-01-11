using GameDB_v3.Libraries.Lang;
using GameDB_v3.Libraries.Login;
using GameDB_v3.Libraries.Sessao;
using GenerativeAI;
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
    public class HomeController : Controller
    {
        private readonly IUsuarioServicos _seUsuario;
        private readonly IEmailServicos _emailServicos;
        private readonly LoginUsuario _login;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Sessao _sessao;

        public HomeController(IUsuarioServicos seUsuario,
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

        // LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Items.ContainsKey("MSG_E"))
            {
                TempData["MSG_E"] = HttpContext.Items["MSG_E"];
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] string? Login, string? Senha)
        {
            UsuarioModel user = new();
            user = await _seUsuario.Login(null, Login, Senha);

            if (user == null)
            {
                return Problem(title: "Erro", detail: "Credenciais inválidas.");
            }

            try
            {
                // CRIAÇÃO DOS CLAIMS
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                    new Claim(ClaimTypes.Name, user.NomeCompleto),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Tipo)
                };

                // Identidade
                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                // Criar principal
                var principal = new ClaimsPrincipal(identity);

                // Efetuar login via cookie
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties()
                    {
                        IsPersistent = true,       // manter logado
                        ExpiresUtc = DateTime.UtcNow.AddHours(3)
                    }
                );

                // Salvar dados complementares na sessão
                _sessao.Cadastrar("NomeUsuario", user.NomeCompleto);
                _sessao.Cadastrar("ID", user.ID.ToString());
                _sessao.Cadastrar("Tipo", user.Tipo.ToString());

                var validarSenha = ManipularModels.ValidarSenha(user.Senha);

                if (!validarSenha.senhaValida)
                {
                    user.SenhaTemporaria = true;
                    TempData["MSG_A"] = "Sua senha não é segura, recomendamos a troca da mesma imediatamente!";

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new
                        {
                            success = true,
                            redirectUrl = Url.Action("Cadastro", "Usuario", new { id = user.ID, senhaTemporaria = user.SenhaTemporaria })
                        });
                    }

                    return RedirectToAction("Cadastro", "Usuario", new { id = user.ID, senhaTemporaria = user.SenhaTemporaria });
                }
                else
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new
                        {
                            success = true,
                            redirectUrl = Url.Action("Index", "Usuario", new { id = user.ID })
                        });
                    }

                    return RedirectToAction("Index", "Usuario", new { id = user.ID });
                }

            }
            catch (Exception ex)
            {
                return Problem(
                    title: "Erro",
                    detail: ex.Message
                    );
            }
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _sessao.RemoverTodos();
            return RedirectToAction(nameof(Login));
        }


        [HttpGet]
        public IActionResult RecuperarSenha()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarSenha(string email)
        {
            try
            {
                UsuarioModel usuario = await _seUsuario.Obter(null, null, email.Trim().ToLower());

                if (usuario == null)
                {
                    return Problem(detail: "Não conseguimos encontrar um usuário para o e-mail informado.", title: "Erro");
                }

                usuario.Senha = KeyGenerator.GetUniqueKey(6);
                await _seUsuario.AtualizarSenha(usuario);
                await _emailServicos.EnviarSenhaPorEmail(false, usuario);

                TempData["MSG_S"] = Mensagem.S_EMAILRECUPERACAO;
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Login", "Home")
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
    }
}
