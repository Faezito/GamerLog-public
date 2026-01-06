using GameDB_v3.Libraries.Lang;
using GameDB_v3.Libraries.Login;
using GenerativeAI.Types;
using Microsoft.AspNetCore.Authentication;
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

        public HomeController(IUsuarioServicos seUsuario,
            LoginUsuario login,
            IHttpContextAccessor httpContextAccessor,
            IEmailServicos emailServicos
            )
        {
            _seUsuario = seUsuario;
            _login = login;
            _httpContextAccessor = httpContextAccessor;
            _emailServicos = emailServicos;
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
            user = await _seUsuario.Login(Login, Senha);

            if (user == null)
            {
                TempData["MSG_E"] = "Credenciais inválidas.";
            }

            try
            {
                if (user != null)
                {
                    _login.Login(user);

                    _httpContextAccessor.HttpContext.Session.SetInt32("ID", user.ID.Value);
                    _httpContextAccessor.HttpContext.Session.SetString("Nome", user.NomeCompleto);
                    _httpContextAccessor.HttpContext.Session.SetString("Tipo", user.Tipo);

                    var validarSenha = ManipularModels.ValidarSenha(user.Senha);

                    if (!validarSenha.senhaValida)
                    {
                        user.SenhaTemporaria = true;

                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new
                            {
                                success = true,
                                redirectUrl = Url.Action("Cadastro", "Usuario", new { id = user.ID })
                            });
                        }

                        return RedirectToAction("Cadastro", "Usuario", new { id = user.ID });
                    }
                    else
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new
                            {
                                success = true,
                                redirectUrl = Url.Action("Index", "Usuario")
                            });
                        }

                        return RedirectToAction("Index", "Usuario");
                    }
                }
                else
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Credenciais inválidas."
                        });
                    }

                    TempData["MSG_E"] = "Credenciais inválidas.";
                    return View();
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
            _login.Logout();
            _httpContextAccessor.HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
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
                UsuarioModel usuario = await _seUsuario.Obter(null, email.Trim().ToLower());

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



        // TODO: REMOVER SE NÃO FIZER FALTA
        public async Task GerarSenha(UsuarioModel model)
        {
            try
            {
                model.Senha = KeyGenerator.GetUniqueKey(6);

                await _seUsuario.AtualizarSenha(model);
                model = await _seUsuario.Obter(model.ID.Value, null);

                string destinatario = model.Email.Trim();
                string assunto = "Recuperação de senha - GamerLog";
                string corpo = $@"
Olá, {model.NomeCompleto} você solicitou a recuperação da sua senha. <br />
Segue abaixo sua nova senha, basta fazer login com seu usuário ou e-mail, e a senha abaixo. <br />
<h4> {model.Senha} </h4>

<a href='https://gamerlog.runasp.net/'>Acessar</a>

";

                await _emailServicos.EnviarEmailAsync(destinatario, assunto, corpo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}