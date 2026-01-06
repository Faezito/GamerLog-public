using Newtonsoft.Json;
using Z1.Model;
namespace GameDB_v3.Libraries.Login
{
    public class LoginUsuario
    {
        private Sessao.Sessao _sessao;
        private string Key = "Login.Usuario";
        public LoginUsuario(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }

        public void Login(UsuarioModel model)
        {
            string userJSONstring = JsonConvert.SerializeObject(model);
            _sessao.Cadastrar(Key, userJSONstring);
        }

        public UsuarioModel GetCliente()
        {
            if (_sessao.Existe(Key))
            {
                string clienteJSONstring = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<UsuarioModel>(clienteJSONstring);
            }
            return null;
        }
        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}
