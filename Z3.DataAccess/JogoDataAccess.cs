using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z1.Model;
using Z3.DataAccess.Database;

namespace Z3.DataAccess
{
    public interface IJogoDataAccess
    {
        Task<List<JogoModel>> Listar(int? id, string? titulo);
        Task<List<RegistroJogoModel>> ListarJogosDoUsuario(int? id, string? titulo, int? status, int usuarioID, int? ano);
        Task<int> Adicionar(JogoModel model);
        Task<int> Atualizar(JogoModel model);
        Task Deletar(int id);
        Task<JogoModel> Obter(int id, int usuarioID);
        Task<JogoModel> Obter(int id);
    }

    public class JogoDataAccess : IJogoDataAccess
    {
        private readonly IDapper _dapper;

        public JogoDataAccess(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<int> Adicionar(JogoModel model)
        {
            try
            {
                string sql = @"
INSERT INTO dbo.Jogos (
ID
,Titulo
,GeneroID
,PublisherID
,DataLancamento
,Metacritic
,CaminhoImagem
)
OUTPUT INSERTED.ID
VALUES (
@ID
,@Titulo
,@GeneroID
,@PublisherID
,@DataLancamento
,@Metacritic
,@CaminhoImagem
)
";
                int? id = await _dapper.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: model);
                return id.Value;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Atualizar(JogoModel model)
        {
            try
            {
                string sql = @"
UPDATE dbo.Jogos
SET 
Titulo = COALESCE(@Titulo, Titulo)
,Genero = COALESCE(@Genero, Genero)
,Publisher = COALESCE(@Publisher, Publisher)
,DataLancamento = COALESCE(@DataLancamento, DataLancamento)
,Metacritic = COALESCE(@Metacritic, Metacritic)
,CaminhoImagem = COALESCE(@CaminhoImagem, CaminhoImagem)
WHERE ID = @id
";
                int? id = await _dapper.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: model);
                return id.Value;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Deletar(int id)
        {
            try
            {
                string sql = @"
DELETE FROM dbo.Jogos
WHERE ID = @Id
";
                await _dapper.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<JogoModel>> Listar(int? id, string? titulo)
        {
            try
            {
                string sql = @"
SELECT 
J.ID
,[Titulo]
,P.ID AS PublisherID
,P.Publisher
,G.ID AS GeneroID
,G.Genero
,[DataLancamento]
,Metacritic
,CaminhoImagem
FROM [dbo].[Jogos] J WITH(NOLOCK)
INNER JOIN [dbo].Generos G ON J.GeneroID = G.ID
INNER JOIN [dbo].Publishers P ON J.PublisherID = P.ID
WHERE (@id IS NULL OR ID = @id)
AND (@titulo IS NULL OR titulo LIKE CONCAT(@titulo, '%'))
";

                var obj = new
                {
                    id = id,
                    titulo = titulo
                };
                return await _dapper.QueryAsync<JogoModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<RegistroJogoModel>> ListarJogosDoUsuario(int? id, string? titulo, int? status, int usuarioID, int? ano)
        {
            try
            {
                string sql = @"[dbo].[listar_jogos_do_usuario]";

                var obj = new
                {
                    id = id,
                    titulo = titulo,
                    usuarioID = usuarioID,
                    status = status,
                    ano = ano
                };
                return await _dapper.QueryAsync<RegistroJogoModel>(sql: sql, commandType: System.Data.CommandType.StoredProcedure, param: obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<JogoModel> Obter(int id)
        {
            try
            {
                string sql = @"
SELECT 
J.ID
,[Titulo]
,[DataLancamento]
,P.Publisher
,P.ID AS PublisherID
,G.Genero
,G.ID AS GeneroID
,Metacritic
,CaminhoImagem
FROM [dbo].[Jogos] J WITH(NOLOCK)
INNER JOIN [dbo].Generos G ON J.GeneroID = G.ID
INNER JOIN [dbo].Publishers P ON J.PublisherID = P.ID
WHERE (@id IS NULL OR J.ID = @id)
";

            var obj = new
                {
                    id = id
                };
                return await _dapper.QueryFirstOrDefaultAsync<JogoModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<JogoModel> Obter(int id, int usuarioID)
        {
            try
            {
                string sql = @"
SELECT J.[ID] AS ID
,[Titulo]
,[DataLancamento]
,UJ.Nota
,UJ.UsuarioID  
,Metacritic
,CaminhoImagem
,P.Publisher
,P.ID AS PublisherID
,G.Genero
,G.ID AS GeneroID
FROM [dbo].[Jogos] J WITH(NOLOCK)
INNER JOIN dbo.RegistrosJogos UJ ON UJ.JogoID = J.ID
INNER JOIN [dbo].Generos G ON J.GeneroID = G.ID
INNER JOIN [dbo].Publishers P ON J.PublisherID = P.ID
WHERE UsuarioID = @usuarioID
AND J.ID = @id
";

                var obj = new
                {
                    id = id,
                    usuarioID = usuarioID
                };
                return await _dapper.QueryFirstOrDefaultAsync<JogoModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}