using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Z1.Model;

namespace GameDB_v3.Libraries.Login
{
    public class LoginUsuario
    {
        private readonly IHttpContextAccessor _context;

        public LoginUsuario(IHttpContextAccessor context)
        {
            _context = context;
        }

        public async Task LoginAsync(UsuarioModel usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.ID.ToString()),
                new Claim(ClaimTypes.Name, usuario.NomeCompleto),
                new Claim(ClaimTypes.Role, usuario.Tipo)
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await _context.HttpContext.SignInAsync("CookieAuth", principal);
        }

        public async Task LogoutAsync()
        {
            await _context.HttpContext.SignOutAsync("CookieAuth");
        }

        public int? GetUserId()
        {
            var idClaim = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null) return null;

            if (int.TryParse(idClaim.Value, out int id))
                return id;

            return null;
        }
    }


    //public class LoginUsuario
    //{
    //    private Sessao.Sessao _sessao;
    //    private string Key = "Login.Usuario";
    //    public LoginUsuario(Sessao.Sessao sessao)
    //    {
    //        _sessao = sessao;
    //    }

    //    public void Login(UsuarioModel model)
    //    {
    //        string userJSONstring = JsonConvert.SerializeObject(model);
    //        _sessao.Cadastrar(Key, userJSONstring);
    //    }

    //    public UsuarioModel GetCliente()
    //    {
    //        if (_sessao.Existe(Key))
    //        {
    //            string clienteJSONstring = _sessao.Consultar(Key);
    //            return JsonConvert.DeserializeObject<UsuarioModel>(clienteJSONstring);
    //        }
    //        return null;
    //    }
    //    public void Logout()
    //    {
    //        _sessao.RemoverTodos();
    //    }
    //}
}
