using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Z1.Model;
using Z3.DataAccess.Database;

namespace Z3.DataAccess
{
    public interface IRegistroJogoDataAccess
    {
        Task Inserir(RegistroJogoModel model);
        Task Deletar(int registroId, int usuarioId);
        Task<List<RegistroJogoModel>> Listar(int jogoId, int usuarioId);
        Task<RegistroJogoModel> Obter(int jogoId, int usuarioId);
        Task<List<RegistroJogoModel>> ListarRecentes(int usuarioId);
        Task<List<int?>> ListarAnos(int usuarioId);
    }

    public class RegistroJogoDataAccess : IRegistroJogoDataAccess
    {
        private readonly IDapper _dapper;

        public RegistroJogoDataAccess(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task Deletar(int registroId, int usuarioId)
        {
            try
            {
                string sql = @"
DELETE FROM dbo.RegistrosJogos
WHERE ID = @id
AND UsuarioID = @usuarioId
";
                var obj = new
                {
                    id = registroId,
                    usuarioId = usuarioId
                };
                await _dapper.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Inserir(RegistroJogoModel model)
        {
            try
            {
                string sql = @"
INSERT INTO dbo.RegistrosJogos (
UsuarioID
,JogoID
,PlataformaID
,TempoJogado
,DataAdicionado
,DataZerado
,DataPlatinado
,UltimaSessao
,Nota
,Status
,Obs
)
OUTPUT INSERTED.ID
VALUES (
@UsuarioID
,@JogoID
,@PlataformaID
,@TempoJogado
,@DataAdicionado
,@DataZerado
,@DataPlatinado
,@UltimaSessao
,@Nota
,@Status
,@Obs
)
";
                await _dapper.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<RegistroJogoModel>> Listar(int jogoId, int usuarioId)
        {
            try
            {
                string sql = @"usp_listar_registros";
                var obj = new
                {
                    jogoId = jogoId,
                    usuarioId = usuarioId
                };
                return await _dapper.QueryAsync<RegistroJogoModel>(sql: sql, commandType: System.Data.CommandType.StoredProcedure, param: obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<RegistroJogoModel> Obter(int jogoId, int usuarioId)
        {
            try
            {
                string sql = @"
SELECT TOP 1 R.[ID]
      ,[UsuarioID]
      ,[JogoID]
      ,[PlataformaID]
      ,[TempoJogado]
      ,[DataAdicionado]
      ,[Nota]
      ,Status
      ,[Obs]
  FROM [dbo].[RegistrosJogos] R WITH(NOLOCK)
    WHERE UsuarioID = @usuarioId
    AND JogoID = @jogoId
";
                var obj = new
                {
                    jogoId = jogoId,
                    usuarioId = usuarioId
                };
                return await _dapper.QuerySingleOrDefaultAsync<RegistroJogoModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RegistroJogoModel>> ListarRecentes(int usuarioId)
        {
            try
            {
                string sql = @"
SELECT TOP 10 R.[ID]
      ,[UsuarioID]
      ,[JogoID]
      ,[PlataformaID]
      ,[TempoJogado]
      ,[DataAdicionado]
      ,[Nota]
      ,Status
      ,[Obs]
  FROM [dbo].[RegistrosJogos] R WITH(NOLOCK)
    WHERE UsuarioID = @usuarioId
    ORDER BY DataAdicionado DESC
";
                var obj = new
                {
                    usuarioId = usuarioId
                };
                return await _dapper.QueryAsync<RegistroJogoModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<int?>> ListarAnos(int usuarioId)
        {
            try
            {
                string sql = @"
WITH CTE_BASE AS (
SELECT DISTINCT
    LEFT([UltimaSessao], 4) AS ANO
  FROM [db36109].[dbo].[RegistrosJogos] WITH(NOLOCK)
  WHERE UsuarioID = @usuarioId
)
SELECT ANO FROM CTE_BASE WITH(NOLOCK)
WHERE ANO IS NOT NULL
ORDER BY ANO DESC

";
                var obj = new
                {
                    usuarioId = usuarioId
                };
                return await _dapper.QueryAsync<int?>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
