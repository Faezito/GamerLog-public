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
        Task<List<UsuarioModel>> Listar(int? id, string? nome, string? usuario, string? email);
        Task<int?> Adicionar(UsuarioModel model);
        Task Atualizar(UsuarioModel model);
        Task AtualizarSenha(UsuarioModel model);
        Task Deletar(UsuarioModel model);
        Task<UsuarioModel> Obter(int? id, string? usuario);
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
)
OUTPUT INSERTED.ID
VALUES (
@NomeCompleto,
@Usuario,
@Email,
@Senha,
@Tipo,
@Genero,
@DataCriacao
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
                throw ex;
            }
        }

        public async Task Atualizar(UsuarioModel model)
        {
            try
            {
                string sql = @"
UPDATE dbo.Usuarios
SET Email = COALESCE(@Email, Email),
Senha = COALESCE(@Senha, Senha),
Tipo = COALESCE(@Tipo, Tipo),
NomeCompleto = COALESCE(@NomeCompleto, NomeCompleto),
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

        public async Task<List<UsuarioModel>> Listar(int? id, string? nome, string? usuario, string? email)
        {
            try
            {
                string sql = @"
SELECT ID
,NomeCompleto
,Usuario
,Email
,Senha
,Tipo
,Genero
,DataCriacao
,DataDeletado
FROM dbo.Usuarios WITH(NOLOCK)
WHERE DataDeletado IS NULL
AND (@id IS NULL OR ID = @id)
AND (@nome IS NULL OR NomeCompleto LIKE CONCAT(@nome, '%'))
AND (@usuario IS NULL OR Usuario LIKE CONCAT(@usuario, '%'))
AND (@email IS NULL OR Email LIKE CONCAT(@email, '%'))
";

                var obj = new
                {
                    id = id,
                    nome = nome,
                    usuario = usuario,
                    email = email
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
SET Senha = @Senha
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

        public async Task<UsuarioModel> Obter(int? id, string? usuario)
        {
            try
            {
                string sql = @"
SELECT ID
,NomeCompleto
,Usuario
,Email
,Senha
,Tipo
,Genero
,DataCriacao
,DataDeletado
FROM dbo.Usuarios WITH(NOLOCK)
WHERE DataDeletado IS NULL
AND (@id IS NULL OR ID = @id)
AND ((Usuario = @usuario)
OR (Email = @usuario))
";

                var obj = new
                {
                    id = id,
                    usuario = usuario
                };
                return await _dapper.QueryFirstOrDefaultAsync<UsuarioModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}