using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z1.Model
{
    public class JogoModel
    {
        public int? ID { get; set; }
        public string? Titulo { get; set; }
        public string? Genero {  get; set; }
        public int? GeneroID {  get; set; }
        public DateTime? DataLancamento { get; set; }
        public string? Publisher { get; set; }
        public int? PublisherID { get; set; }
        public int? Metacritic {  get; set; }
        public string? CaminhoImagem { get; set; }
        public string MetacriticCor => Metacritic switch
        {
            >= 61 => "badge text-bg-success",
            >= 40 and <= 60 => "badge text-bg-warning",
            <= 39 => "badge text-bg-danger",
            _ => ""
        };
    }
}
