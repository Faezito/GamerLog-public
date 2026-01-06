using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z1.Model;
using Z1.Model.APIs;
using Z3.DataAccess.Database;

namespace Z3.DataAccess.Externo  // TODO: CRIAR UMA API E PASSAR TUDO ISSO PARA LÁ
{
    public interface IAPIsDataAccess
    {
        Task<APIModel> Obter(int id);
        Task<int> Inserir(int id);
        Task Atualizar(int id);
        Task Deletar(int id);
        Task<List<APIModel>> Listar(int? id);
    }

    public class APIsDataAccess : IAPIsDataAccess
    {
        private readonly IDapper _dapper;
        public APIsDataAccess(IDapper dapper)
        {
            _dapper = dapper;
        }

        public Task Atualizar(int id)
        {
            throw new NotImplementedException();
        }

        public Task Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Inserir(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<APIModel>> Listar(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<APIModel> Obter(int id)
        {
            try
            {
                string sql = @"
SELECT [ID]
      ,[Descricao]
      ,[Token]
      ,[Modelo]
      ,[Url]
      ,[Usuario]
      ,[Senha] 
FROM dbo.APIs WITH(NOLOCK)
WHERE ID = @id
";
                var obj = new
                {
                    ID = id
                };
                return await _dapper.QueryFirstOrDefaultAsync<APIModel>(sql: sql, commandType: System.Data.CommandType.Text, param: obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
