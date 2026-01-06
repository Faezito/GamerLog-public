using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z1.Model;
using Z3.DataAccess;

namespace Z2.Services
{
    public interface IRegistroJogoServicos
    {
        Task Inserir(RegistroJogoModel model);
        Task Deletar(int registroId, int usuarioId);
        Task<List<RegistroJogoModel>> Listar(int jogoId, int usuarioId);
        Task<RegistroJogoModel> Obter(int jogoId, int usuarioId);
        Task<List<RegistroJogoModel>> ListarRecentes(int usuarioId);
    }

    public class RegistroJogoServicos : IRegistroJogoServicos
    {
        private readonly IRegistroJogoDataAccess _daReg;

        public RegistroJogoServicos(IRegistroJogoDataAccess daReg)
        {
            _daReg = daReg;
        }

        public async Task Deletar(int registroId, int usuarioId)
        {
            await _daReg.Deletar(registroId, usuarioId);
        }

        public async Task Inserir(RegistroJogoModel model)
        {
            await _daReg.Inserir(model);
        }

        public async Task<List<RegistroJogoModel>> Listar(int jogoId, int usuarioId)
        {
            return await _daReg.Listar(jogoId, usuarioId);
        }

        public async Task<List<RegistroJogoModel>> ListarRecentes(int usuarioId)
        {
            return await _daReg.ListarRecentes(usuarioId);
        }

        public async Task<RegistroJogoModel> Obter(int jogoId, int usuarioId)
        {
            return await _daReg.Obter(jogoId, usuarioId);
        }
    }
}
