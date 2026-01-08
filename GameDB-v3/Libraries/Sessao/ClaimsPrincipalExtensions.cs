using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Z1.Model;

namespace GameDB_v3.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var claim = user?.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                // tenta limpar o cookie
                try
                {
                    var http = new HttpContextAccessor();
                    http.HttpContext?.SignOutAsync("CookieAuth");
                }
                catch { }

                return 0;
            }

            if (!int.TryParse(claim.Value, out int id))
                return 0;

            return id;
        }

        public static string GetUserTipo(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static UsuarioModel ObterUsuario(this ClaimsPrincipal user)
        {
            int id = GetUserId(user);
            string tipo = GetUserTipo(user);
            string email = GetUserEmail(user);
            string nome = GetUserName(user);

            var usuario = new UsuarioModel
            {
                ID = id,
                Tipo = tipo,
                Email = email,
                NomeCompleto = nome
            };
            return usuario;

            // OBTER USUÁRIO OU QUALQUER PROPRIEDADE DELE ASSIM
            //
            //UsuarioModel usuario = this.User.ObterUsuario();

        }
    }


    //public static class ClaimsPrincipalExtensions
    //{
    //    public static int GetUserId(this ClaimsPrincipal user)
    //    {
    //        var idClaim = user?.FindFirst(ClaimTypes.NameIdentifier);
    //        if (idClaim == null)
    //            throw new Exception("Usuário não possui Claim de Id.");

    //        return int.Parse(idClaim.Value);
    //    }
    //}
}
