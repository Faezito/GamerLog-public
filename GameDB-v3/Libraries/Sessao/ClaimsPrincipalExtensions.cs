using System.Security.Claims;

namespace GameDB_v3.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user?.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null)
                throw new Exception("Usuário não possui Claim de Id.");

            return int.Parse(idClaim.Value);
        }
    }
}
