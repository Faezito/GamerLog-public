using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Z1.Model;
using Z1.Model.Email;
using Z3.DataAccess;

namespace Z2.Services.Externo
{
    public interface IEmailServicos
    {
        Task EnviarEmailAsync(string destinatario, string assunto, string corpoHtml);
        Task EnviarSenhaPorEmail(bool cadastro, UsuarioModel model, string senhaNova);
    }

    public class EmailServicos : IEmailServicos
    {
        private readonly IEmailDataAccess _daEmail;
        public EmailServicos(IEmailDataAccess daEmail)
        {
            _daEmail = daEmail;
        }

        public async Task EnviarEmailAsync(string destinatario, string assunto, string corpoHtml)
        {
            var mensagem = new MimeMessage();
            EmailConfig model = await _daEmail.Obter(1);

            mensagem.From.Add(new MailboxAddress(
                model.FromName,
                model.Username
            ));

            mensagem.To.Add(MailboxAddress.Parse(destinatario));
            mensagem.Subject = assunto;

            mensagem.Body = new TextPart("html")
            {
                Text = corpoHtml
            };

            using var client = new SmtpClient();

            try
            {
                if (model.UseStartTls)
                {
                    await client.ConnectAsync(model.Server, model.Port, SecureSocketOptions.StartTls);
                }
                else if (model.UseSSL)
                {
                    await client.ConnectAsync(model.Server, model.Port, SecureSocketOptions.SslOnConnect);
                }
                else
                {
                    await client.ConnectAsync(model.Server, model.Port, SecureSocketOptions.None);
                }

                await client.AuthenticateAsync(model.Username, model.Password);

                await client.SendAsync(mensagem);
            }
            catch
            {
                throw new Exception("Erro ao conectar ao serviço de email. Contate um administrador.");
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        public async Task EnviarSenhaPorEmail(bool cadastro, UsuarioModel model, string senhaNova)
        {
            try
            {
                string primeiroNome = model.NomeCompleto.Split(" ")[0];
                string destinatario = model.Email.Trim();
                string assunto = string.Empty;
                string corpo = string.Empty;

                // TODO: USAR O ESTILO DO E-MAIL DE RECUPERAÇÃO NO E-MAIL DE CADASTRO

                if (cadastro == true)
                {
                    assunto = "Bem-vindo ao GamerLog 🎮 Sua conta está pronta!";
                    corpo = $@"
<!DOCTYPE html>
<html>
<body style='
    margin:0;
    padding:0;
    background:#0f172a;
    font-family: Arial, Helvetica, sans-serif;
    color:#e5e7eb;
'>

<table width='100%' cellpadding='0' cellspacing='0'>
<tr>
<td align='center'>

    <table width='100%' cellpadding='0' cellspacing='0' style='max-width:600px;padding:24px;'>

        <!-- Cabeçalho -->
        <tr>
            <td align='center' style='padding-bottom:24px;'>
                <h1 style='margin:0;color:#a78bfa;'>🎮 GamerLog</h1>
                <p style='margin:4px 0 0;color:#9ca3af;'>
                    Sua biblioteca de jogatinas
                </p>
            </td>
        </tr>

        <!-- Conteúdo -->
        <tr>
            <td style='
                background:#020617;
                border-radius:8px;
                padding:24px;
            '>

                <p style='font-size:16px;'>
                    Olá, <strong>{primeiroNome}</strong> 👋
                </p>

                <p style='font-size:15px;line-height:1.6;color:#d1d5db;'>
                            Seu cadastro no <strong>GamerLog</strong> foi concluído com sucesso!<br>
                            Em breve você poderá registrar e compartilhar suas jogatinas 🎮
                </p>

                <p style='font-size:15px;line-height:1.6;color:#d1d5db;'>
                    Abaixo está sua <strong>senha provisória</strong>.  
                    Use-a para acessar sua conta e, por segurança, altere-a
                    imediatamente após o login.
                </p>

                <!-- Senha provisória -->
                <div style='
                    background:#020617;
                    border:1px dashed #7c3aed;
                    border-radius:6px;
                    padding:16px;
                    margin:24px 0;
                    text-align:center;
                '>
                    <p style='margin:0;font-size:14px;color:#9ca3af;'>Senha provisória</p>
                    <p style='
                        margin:8px 0 0;
                        font-size:22px;
                        font-weight:bold;
                        letter-spacing:2px;
                        color:#a78bfa;
                    '>
                        {senhaNova}
                    </p>
                </div>

                <!-- Botão -->
                <div style='text-align:center;margin:32px 0;'>
                    <a href='https://gamerlog.runasp.net/'
                       style='
                        background:#7c3aed;
                        color:#ffffff;
                        padding:14px 28px;
                        text-decoration:none;
                        border-radius:6px;
                        font-weight:bold;
                        font-size:16px;
                        display:inline-block;
                       '>
                        Acessar GamerLog
                    </a>
                </div>

                <hr style='border:none;border-top:1px solid #1e293b;margin:24px 0;'>
            </td>
        </tr>

        <!-- Rodapé -->
        <tr>
            <td align='center' style='padding-top:24px;font-size:12px;color:#6b7280;'>
                © {DateTime.Now.Year} GamerLog · Todos os direitos reservados
            </td>
        </tr>

    </table>

</td>
</tr>
</table>

</body>
</html>

                    ";
                }
                else
                {
                    assunto = "Recuperação de senha - GamerLog";
                    corpo = $@"
<!DOCTYPE html>
<html>
<body style='
    margin:0;
    padding:0;
    background:#0f172a;
    font-family: Arial, Helvetica, sans-serif;
    color:#e5e7eb;
'>

<table width='100%' cellpadding='0' cellspacing='0'>
<tr>
<td align='center'>

    <table width='100%' cellpadding='0' cellspacing='0' style='max-width:600px;padding:24px;'>

        <!-- Cabeçalho -->
        <tr>
            <td align='center' style='padding-bottom:24px;'>
                <h1 style='margin:0;color:#a78bfa;'>🎮 GamerLog</h1>
                <p style='margin:4px 0 0;color:#9ca3af;'>
                    Sua biblioteca de jogatinas
                </p>
            </td>
        </tr>

        <!-- Conteúdo -->
        <tr>
            <td style='
                background:#020617;
                border-radius:8px;
                padding:24px;
            '>

                <p style='font-size:16px;'>
                    Olá, <strong>{primeiroNome}</strong> 👋
                </p>

                <p style='font-size:15px;line-height:1.6;color:#d1d5db;'>
                    Sua senha do <strong>GamerLog</strong> foi redefinida com sucesso.
                </p>

                <p style='font-size:15px;line-height:1.6;color:#d1d5db;'>
                    Abaixo está sua <strong>senha provisória</strong>.  
                    Use-a para acessar sua conta e, por segurança, altere-a
                    imediatamente após o login.
                </p>

                <!-- Senha provisória -->
                <div style='
                    background:#020617;
                    border:1px dashed #7c3aed;
                    border-radius:6px;
                    padding:16px;
                    margin:24px 0;
                    text-align:center;
                '>
                    <p style='margin:0;font-size:14px;color:#9ca3af;'>Senha provisória</p>
                    <p style='
                        margin:8px 0 0;
                        font-size:22px;
                        font-weight:bold;
                        letter-spacing:2px;
                        color:#a78bfa;
                    '>
                        {senhaNova}
                    </p>
                </div>

                <!-- Botão -->
                <div style='text-align:center;margin:32px 0;'>
                    <a href='https://gamerlog.runasp.net/'
                       style='
                        background:#7c3aed;
                        color:#ffffff;
                        padding:14px 28px;
                        text-decoration:none;
                        border-radius:6px;
                        font-weight:bold;
                        font-size:16px;
                        display:inline-block;
                       '>
                        Acessar GamerLog
                    </a>
                </div>

                <hr style='border:none;border-top:1px solid #1e293b;margin:24px 0;'>

                <p style='font-size:13px;color:#9ca3af;line-height:1.6;'>
                    ⚠️ Caso você não tenha solicitado a redefinição de senha,
                    recomendamos que entre em contato com o suporte imediatamente.
                </p>

            </td>
        </tr>

        <!-- Rodapé -->
        <tr>
            <td align='center' style='padding-top:24px;font-size:12px;color:#6b7280;'>
                © {DateTime.Now.Year} GamerLog · Todos os direitos reservados
            </td>
        </tr>

    </table>

</td>
</tr>
</table>

</body>
</html>
";
                }

                await EnviarEmailAsync(destinatario, assunto, corpo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
