namespace Z4.Lib
{
    public static class PasswordHasher
    {
        public static string Hash(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public static bool Autenticar(string senhaDigitada, string senhaCriptografada)
        {
            return BCrypt.Net.BCrypt.Verify(senhaDigitada, senhaCriptografada);
        }
    }
}
