using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z1.Model.APIs;
using Z1.Model.Lang;

namespace Z1.Model
{
    public class RegistroJogoModel : JogoModel
    {
        public int UsuarioID { get; set; }
        public int JogoID { get; set; }
        public int PlataformaID { get; set; }
        public string? Plataforma { get; set; }
        public decimal TempoJogado {  get; set; }
        public string? TempoJogadoString {  get; set; }
        public decimal? TempoJogadoTotal {  get; set; }
        public DateTime DataAdicionado { get; set; }
        public DateTime? UltimaSessao { get; set; }
        public DateTime? DataPlatinado { get; set; }
        public DateTime? DataZerado { get; set; }
        public int? Nota {  get; set; }
        public int Status { get; set; } // 0 - abandonado, 1 - jogando, 2 - finalizado, 3 - 100%, 9 - platinado

        [StringLength(255, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "E_MAXCARACTERES")]
        public string? Obs {  get; set; }
        public string? StatusTexto { get; set; }
        public string? StatusCor { get; set; }


    }
}
