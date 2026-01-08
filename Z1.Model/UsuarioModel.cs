using System.ComponentModel.DataAnnotations;
using Z1.Model.Lang;

namespace Z1.Model
{
    public class UsuarioModel
    {
        public int? ID { get; set; }

        [StringLength(100, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "E_MAXCARACTERES")]
        public string? NomeCompleto { get; set; }

        [StringLength(345, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "E_MAXCARACTERES")]
        public string? Email { get; set; }

        [StringLength(30, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "E_MAXCARACTERES")]
        public string? Usuario { get; set; }

        [StringLength(100, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "E_MAXCARACTERES")]
        public string? Senha { get; set; }
        public string? ConfirmacaoSenha { get; set; }
        public string? Tipo { get; set; }
        public string? Genero { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataDeletado { get; set; }
        public bool SenhaTemporaria { get; set; }
        public string? GoogleId { get; set; }
    }
}
