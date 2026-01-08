using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Z1.Model;

namespace Z4.Bibliotecas
{
    public class ManipularModels
    {
        public static void LimparModelState(ModelStateDictionary modelState, params string[] excecoes)
        {
            var chavesRemover = modelState.Keys
                .Where(k => !excecoes.Contains(k))
                .ToList();

            foreach (var key in chavesRemover)
            {
                modelState.Remove(key);
            }
        }

        public static (bool valido, string? mensagem) ValidarUsuario(UsuarioModel model)
        {
            bool valido = true;
            string mensagem = string.Empty;

            if (string.IsNullOrWhiteSpace(model.NomeCompleto)
                || string.IsNullOrWhiteSpace(model.Usuario)
                || string.IsNullOrWhiteSpace(model.Email)
                || string.IsNullOrWhiteSpace(model.Genero)
                )
            {
                mensagem += "Preencha todos os campos. <br />";
                valido = false;
            }

            // VALIDAÇÃO DE SENHA

            if (model.SenhaTemporaria == false)
            {
                if (string.IsNullOrWhiteSpace(model.Senha)
                    || string.IsNullOrWhiteSpace(model.ConfirmacaoSenha)
                    )
                {
                    mensagem += "Preencha todos os campos. <br />";
                    valido = false;
                }

                if (model.Senha != model.ConfirmacaoSenha)
                {
                    mensagem += "As senhas não coincidem. <br />";
                    valido = false;
                }

                if (model.Senha?.Length < 8)
                {
                    mensagem += "Senha insegura! A senha deve conter, no mínimo, 8 caracteres. <br />";
                    valido = false;
                }
            }

            return (valido, mensagem);
        }

        public static (bool valido, string? mensagem) ValidarRegistro(RegistroJogoModel model)
        {
            bool valido = true;
            string mensagem = string.Empty;

            if ((model.UsuarioID == 0) || (model.JogoID == 0) || (model.PlataformaID == 0) || (model.Status == 0)
                )
            {
                mensagem += "Preencha todos os campos. <br />";
                valido = false;
            }

            return (valido, mensagem);
        }

        public static (bool senhaValida, string? mensagem) ValidarSenha(string senha)
        {
            bool senhaValida = true;
            string mensagem = string.Empty;

            if (senha.Length < 8)
            {
                senhaValida = false;
                mensagem = "A senha deve ter no mínimo 8 caracteres.";
            }

            if (!Regex.IsMatch(senha, "[A-Z]"))
            {
                senhaValida = false;
                mensagem = "A senha deve conter pelo menos uma letra maiúscula.";
            }

            if (!Regex.IsMatch(senha, "[a-z]"))
            {
                senhaValida = false;
                mensagem = "A senha deve conter pelo menos uma letra minúscula.";
            }

            if (!Regex.IsMatch(senha, "[0-9]"))
            {
                senhaValida = false;
                mensagem = "A senha deve conter pelo menos um número.";
            }
            return (senhaValida, mensagem);
        }

        public static DateTime ConverterData(string data)
        {
            var cultura = new CultureInfo("pt-BR");

            DateTime dataConvertida = DateTime.ParseExact(
                                                data,
                                                "dd/MM/yyyy",
                                                cultura
                                            );

            return dataConvertida;
        }

        public static string GerarUsuario(string email, string id)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(id))
            {
                throw new Exception("Usuário inválido.");
            }

            var usermail = email.Split("@")[0];
            id = id.Substring(3, 6);

            string usuario = usermail + '.' + id;
            return usuario;
        }
    }
}
