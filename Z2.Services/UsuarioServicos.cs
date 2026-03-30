using System.Transactions;
using Z1.Model;
using Z3.DataAccess;
using Z4.Lib;

namespace Z2.Services
{
    public interface IUsuarioServicos
    {
        Task<List<UsuarioModel>> Listar(int? id, string? nome, string? email, string? GoogleId, string? usuario);
        Task<int?> Cadastrar(UsuarioModel model);
        Task Deletar(UsuarioModel model);
        Task<UsuarioModel> Obter(int? id, string? GoogleId, string? email);
        Task<UsuarioModel> ObterPorSteam(string? steamId);
        Task<UsuarioModel> Login(string? GoogleId, string? usuario, string Senha);
        Task AtualizarSenha(UsuarioModel model);
        Task<int?> AdicionarSteam(UsuarioModel model);
    }

    public class UsuarioServicos : IUsuarioServicos
    {
        private readonly IUsuarioDataAccess _daUsuario;

        public UsuarioServicos(IUsuarioDataAccess daUsuario)
        {
            _daUsuario = daUsuario;
        }

        public async Task<int?> AdicionarSteam(UsuarioModel model)
        {
            return await _daUsuario.AdicionarSteam(model);
        }

        public async Task AtualizarSenha(UsuarioModel model)
        {
            model.Senha = PasswordHasher.Hash(model.Senha);
            model.SenhaTemporaria = false; // TODO: Método para validar formato de senha
            await _daUsuario.AtualizarSenha(model);
        }

        public async Task<int?> Cadastrar(UsuarioModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Senha))
                model.Senha = PasswordHasher.Hash(model.Senha);

            if (model.ID.HasValue)
            {
                await _daUsuario.Atualizar(model);
                return model.ID;
            }
            else
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        model.DataCriacao = DateTime.Now;
                        model.ID = await _daUsuario.Adicionar(model);

                        if (!string.IsNullOrWhiteSpace(model.steamid))
                            await AdicionarSteam(model);

                        scope.Complete();
                        return model.ID;
                    }
                    catch (Exception)
                    {
                        scope.Dispose();
                        throw;
                    }
                }
            }
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
                bool valido = PasswordHasher.Autenticar(Senha, user.Senha);
                if (valido)
                    return user;
            }
            return null;
        }

        public async Task<UsuarioModel> Obter(int? id, string? GoogleId, string? email)
        {
            var lst = await _daUsuario.Listar(id, null, GoogleId, null, email);
            return lst.SingleOrDefault();
        }

        public async Task<UsuarioModel> ObterPorSteam(string? steamId)
        {
            return await _daUsuario.ObterPorSteam(steamId);
        }
    }
}