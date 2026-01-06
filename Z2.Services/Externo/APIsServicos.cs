using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z1.Model.APIs;
using Z3.DataAccess.Externo;

namespace Z2.Services.Externo
{
    public interface IAPIsServicos
    {
        Task<APIModel> Obter(int id);
        Task<int?> Cadastro(int id);
        Task Deletar(int id);
        Task<List<APIModel>> Listar(int? id);
    }

    public class APIsServicos : IAPIsServicos
    {
        private readonly IAPIsDataAccess _apis;
        public APIsServicos(IAPIsDataAccess apis)
        {
            _apis = apis;
        }

        public async Task<int?> Cadastro(int id)
        {
            APIModel api = await _apis.Obter(id);

            if(api == null)
            {
                int APIid = await _apis.Inserir(id);
                return APIid;
            }
            await _apis.Atualizar(id);
            return id;
        }

        public async Task Deletar(int id)
        {
            await _apis.Deletar(id);
        }

        public async Task<List<APIModel>> Listar(int? id)
        {
            return await _apis.Listar(id);
        }

        public async Task<APIModel> Obter(int id)
        {
            return await _apis.Obter(id);
        }
    }
}
