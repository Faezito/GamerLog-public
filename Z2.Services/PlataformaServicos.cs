using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z1.Model;
using Z3.DataAccess;

namespace Z2.Services
{
    public interface IPlataformaServicos
    {
        Task<List<PlataformaModel>> Listar(int? id, string? plataforma);
        Task<PlataformaModel> Obter(int id);
    }

    public class PlataformaServicos : IPlataformaServicos
    {
        private readonly IPlataformaDataAccess _daPlataforma;
        public PlataformaServicos(IPlataformaDataAccess daPlataforma)
        {
            _daPlataforma = daPlataforma;
        }
        public async Task<List<PlataformaModel>> Listar(int? id, string? plataforma)
        {
            return await _daPlataforma.Listar(id, plataforma);
        }

        public async Task<PlataformaModel> Obter(int id)
        {
            return await _daPlataforma.Obter(id);
        }
    }
}
