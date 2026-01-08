using GenerativeAI;
using Org.BouncyCastle.Crypto;
using System.Web.Mvc;
using Z1.Model;
using Z3.DataAccess;

namespace Z2.Services
{
    public interface IUsuarioServicos
    {
        Task<List<UsuarioModel>> Listar(int? id, string? nome, string? email, string? GoogleId, string? usuario);
        Task<int?> Cadastrar(UsuarioModel model);
        Task Deletar(UsuarioModel model);
        Task<UsuarioModel> Obter(int? id, string? GoogleId, string? email);
        Task<UsuarioModel> Login(string? GoogleId, string? usuario, string Senha);
        Task AtualizarSenha(UsuarioModel model);
    }

    public class UsuarioServicos : IUsuarioServicos
    {
        private readonly IUsuarioDataAccess _daUsuario;

        public UsuarioServicos(IUsuarioDataAccess daUsuario)
        {
            _daUsuario = daUsuario;
        }

        public async Task AtualizarSenha(UsuarioModel model)
        {
            await _daUsuario.AtualizarSenha(model);
        }

        public async Task<int?> Cadastrar(UsuarioModel model)
        {
            if (model.ID.HasValue)
            {
                await _daUsuario.Atualizar(model);
                return model.ID;
            }

            model.DataCriacao = DateTime.Now;

            return await _daUsuario.Adicionar(model);
        }

        public async Task Deletar(UsuarioModel model)
        {
            await _daUsuario.Deletar(model);
        }

        public async Task<List<UsuarioModel>> Listar(int? id, string? nome, string? GoogleId, string? email, string? usuario)
        {
            List<UsuarioModel> lst = await _daUsuario.Listar(id, nome, GoogleId, email, usuario);
            return lst;
        }

        public async Task<UsuarioModel> Login(string? GoogleId, string? usuario, string Senha)
        {
            var user = await _daUsuario.Obter(null, usuario, GoogleId);

            if (user != null)
            {
                if (user.Senha == Senha)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<UsuarioModel> Obter(int? id, string? GoogleId, string? email)
        {
            var lst = await _daUsuario.Listar(id, null, GoogleId, null, email);
            return lst.SingleOrDefault();
        }
    }
}