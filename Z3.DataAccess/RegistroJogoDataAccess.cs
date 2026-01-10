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
                string sql = @"
SELECT 
R.[ID]
,[UsuarioID]
,[JogoID]
,[PlataformaID]
,Plataforma
,[TempoJogado]
,[DataAdicionado]
,[Nota]
,Status
,CASE 
	WHEN Status = 0 THEN 'Abandonado'
	WHEN Status = 1 THEN 'Jogando'
	WHEN Status = 2 THEN 'História principal concluída'
	WHEN Status = 3 THEN 'História principal e Missões secundárias concluídas'
	WHEN Status = 9 THEN 'Todas as conquistas obtidas (Platinado)'
 END AS StatusTexto
,[Obs]
FROM [dbo].[RegistrosJogos] R  WITH(NOLOCK)
INNER JOIN Plataformas P ON P.ID = R.PlataformaID
WHERE UsuarioID = @usuarioId
AND JogoID = @jogoId
";
                var obj = new
                {
                    jogoId = jogoId,
                    usuarioId = usuarioId
                };
                return await _dapper.QueryAsync<RegistroJogoModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
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
--COALESCE(
--      LEFT([DataZerado], 4)
--      ,LEFT([DataPlatinado], 4)
--      ,LEFT([UltimaSessao], 4)
--      )
    LEFT([UltimaSessao], 4)AS ANO
  FROM [db36109].[dbo].[RegistrosJogos] WITH(NOLOCK)
  WHERE UsuarioID = @usuarioId
)
SELECT * FROM CTE_BASE WITH(NOLOCK)
WHERE ANO IS NOT NULL

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
