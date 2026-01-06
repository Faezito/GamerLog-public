using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z1.Model;
using Z3.DataAccess.Database;

namespace Z3.DataAccess
{
    public interface IPlataformaDataAccess
    {
        Task<List<PlataformaModel>> Listar(int? id, string? plataforma);
        Task<PlataformaModel> Obter(int id);
    }

    public class PlataformaDataAccess : IPlataformaDataAccess
    {
        private readonly IDapper _dapper;
        public PlataformaDataAccess(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<PlataformaModel>> Listar(int? id, string? plataforma)
        {
            try
            {
                string sql = @"
SELECT 
[ID]
,Plataforma
FROM [dbo].[Plataformas] WITH(NOLOCK)
WHERE (@id IS NULL OR ID = @id)
AND (@plataforma IS NULL OR Plataforma = @plataforma)
";
                var obj = new
                {
                    id = id,
                    plataforma = plataforma
                };
                return await _dapper.QueryAsync<PlataformaModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PlataformaModel> Obter(int id)
        {
            try
            {
                string sql = @"
SELECT 
[ID]
,Plataforma
FROM [dbo].[Plataformas] WITH(NOLOCK)
WHERE ID = @id
";
                var obj = new
                {
                    id = id
                };
                return await _dapper.QuerySingleOrDefaultAsync<PlataformaModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
