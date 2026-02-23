using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Z1.Model;
using Z3.DataAccess.Database;

namespace Z3.DataAccess
{
    public interface IUsuarioDataAccess
    {
        Task<List<UsuarioModel>> Listar(int? id, string? nome, string? GoogleId, string? usuario, string? email);
        Task<int?> Adicionar(UsuarioModel model);
        Task Atualizar(UsuarioModel model);
        Task AtualizarSenha(UsuarioModel model);
        Task Deletar(UsuarioModel model);
        Task<UsuarioModel> Obter(int? id, string? GoogleId, string? usuario);
        Task<UsuarioModel> ObterPorSteam(string? steamId);
        Task<int?> AdicionarSteam(UsuarioModel model);
    }

    public class UsuarioDataAccess : IUsuarioDataAccess
    {
        private readonly IDapper _dapper;

        public UsuarioDataAccess(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<int?> Adicionar(UsuarioModel model)
        {
            try
            {
                string sql = @"
INSERT INTO dbo.Usuarios (
NomeCompleto
,Usuario
,Email
,Senha
,Tipo
,Genero
,DataCriacao
,GoogleId
,SteamID
,SteamUsername
,SteamAvatar
,SteamProfileUrl
,SenhaTemporaria
)
OUTPUT INSERTED.ID
VALUES (
@NomeCompleto,
@Usuario,
@Email,
@Senha,
@Tipo,
@Genero,
@DataCriacao,
@GoogleId,
@steamid,
@personaname, 
@avatarmedium,
@profileurl,
SenhaTemporaria
)
";
                return await _dapper.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: model);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    if (ex.Message.Contains("UK_Usuario"))
                    {
                        throw new Exception("Este nome de usuário já está sendo usado.");
                    }
                    else if (ex.Message.Contains("UK_Email"))
                    {
                        throw new Exception("Este e-mail já está cadastrado. Tente recuperar sua senha.");
                    }

                    throw new Exception("Dados duplicados encontrados no cadastro.");
                }
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro inesperado ao salvar os dados.");
            }
        }

        public async Task Atualizar(UsuarioModel model)
        {
            try
            {
                string sql = @"
UPDATE dbo.Usuarios
SET Email = COALESCE(@Email, Email),
Usuario = COALESCE(@Usuario, Usuario),
Senha = COALESCE(@Senha, Senha),
Tipo = COALESCE(@Tipo, Tipo),
NomeCompleto = COALESCE(@NomeCompleto, NomeCompleto),
GoogleId = COALESCE(@GoogleId, GoogleId),
SenhaTemporaria = COALESCE(@SenhaTemporaria, SenhaTemporaria),
Genero = COALESCE(@Genero, Genero)
WHERE ID = @id
";
                await _dapper.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: model);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    if (ex.Message.Contains("UK_Usuario"))
                    {
                        throw new Exception("Este nome de usuário já está sendo usado.");
                    }
                    else if (ex.Message.Contains("UK_Email"))
                    {
                        throw new Exception("Este e-mail já está cadastrado. Tente recuperar sua senha.");
                    }

                    throw new Exception("Dados duplicados encontrados no cadastro.");
                }
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro inesperado ao salvar os dados.");
            }
        }

        public async Task Deletar(UsuarioModel model)
        {
            try
            {
                string sql = @"
UPDATE dbo.Usuarios
SET DataDeletado = @DataDeletado
WHERE ID = @Id
";
                await _dapper.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UsuarioModel>> Listar(int? id, string? nome, string? GoogleId, string? usuario, string? email)
        {
            try
            {
                string sql = @"
SELECT U.[ID]
      ,U.[GoogleId]
      ,U.[NomeCompleto]
      ,U.[Usuario]
      ,U.[Senha]
      ,U.[Email]
      ,U.[DataCriacao]
      ,U.[DataDeletado]
      ,U.[Tipo]
      ,U.[Genero]
      ,U.[SenhaTemporaria]
      ,S.[steamid]
      ,S.[personaname]
      ,S.[avatarmedium]
      ,S.[profileurl]
FROM dbo.Usuarios U WITH(NOLOCK)
LEFT JOIN [steam].[UsuariosSteam] S ON S.UsuarioID = U.ID
WHERE U.DataDeletado IS NULL
AND (@id IS NULL OR U.ID = @id)
AND (@GoogleId IS NULL OR U.GoogleId = @GoogleId)
AND (@nome IS NULL OR U.NomeCompleto LIKE CONCAT(@nome, '%'))
AND (@usuario IS NULL OR U.Usuario LIKE CONCAT(@usuario, '%'))
AND (@email IS NULL OR U.Email LIKE CONCAT(@email, '%'))
";

                var obj = new
                {
                    id = id,
                    nome = nome,
                    usuario = usuario,
                    email = email,
                    GoogleId = GoogleId
                };

                var ret = await _dapper.QueryAsync<UsuarioModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AtualizarSenha(UsuarioModel model)
        {
            try
            {
                string sql = @"
UPDATE dbo.Usuarios
SET Senha = @Senha, SenhaTemporaria = @SenhaTemporaria
WHERE ID = @id
";
                var obj = new
                {
                    Senha = model.Senha,
                    id = model.ID
                };

                await _dapper.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UsuarioModel> Obter(int? id, string? usuario, string? GoogleId)
        {
            try
            {
                string sql = @"
SELECT U.[ID]
      ,U.[GoogleId]
      ,U.[NomeCompleto]
      ,U.[Usuario]
      ,U.[Senha]
      ,U.[Email]
      ,U.[DataCriacao]
      ,U.[DataDeletado]
      ,U.[Tipo]
      ,U.[Genero]
      ,U.[SenhaTemporaria]
      ,S.[steamid]
      ,S.[personaname]
      ,S.[avatarmedium]
      ,S.[profileurl]
FROM dbo.Usuarios U WITH(NOLOCK)
LEFT JOIN [steam].[UsuariosSteam] S ON S.UsuarioID = U.ID
WHERE U.DataDeletado IS NULL
AND (@id IS NULL OR U.ID = @id)
AND (@GoogleId IS NULL OR U.GoogleId = @GoogleId)
AND ((U.Usuario = @usuario)
OR (U.Email = @usuario))
";

                var obj = new
                {
                    id = id,
                    usuario = usuario,
                    GoogleId = GoogleId
                };
                return await _dapper.QueryFirstOrDefaultAsync<UsuarioModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UsuarioModel> ObterPorSteam(string? steamId)
        {
            try
            {
                string sql = @"
SELECT U.[ID]
      ,U.[GoogleId]
      ,U.[NomeCompleto]
      ,U.[Usuario]
      ,U.[Senha]
      ,U.[Email]
      ,U.[DataCriacao]
      ,U.[DataDeletado]
      ,U.[Tipo]
      ,U.[Genero]
      ,U.[SenhaTemporaria]
      ,S.[steamid]
      ,S.[personaname]
      ,S.[avatarmedium]
      ,S.[profileurl]
FROM [db36109].[dbo].[Usuarios] U
LEFT JOIN [steam].[UsuariosSteam] S ON S.UsuarioID = U.ID
WHERE S.steamid = @steamid
";

                var obj = new
                {
                    steamid = steamId
                };
                return await _dapper.QueryFirstOrDefaultAsync<UsuarioModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int?> AdicionarSteam(UsuarioModel model)
        {
            try
            {
                string sql = @"
INSERT INTO [steam].[UsuariosSteam] (
UsuarioID
,steamid
,personaname
,avatarmedium
,profileurl
)
OUTPUT INSERTED.UsuarioID
VALUES (
@ID,
@steamid,
@personaname, 
@avatarmedium,
@profileurl
)
";
                return await _dapper.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: model);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    if (ex.Message.Contains("UK_steamid"))
                    {
                        throw new Exception("Esta conta já está vinculada.");
                    }
                    throw new Exception("Dados duplicados encontrados no cadastro.");
                }
                throw;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro inesperado ao salvar os dados."); 
            }
        }
    }
}