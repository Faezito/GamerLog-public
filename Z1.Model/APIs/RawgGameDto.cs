using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z1.Model.APIs
{
    public class RawgGameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Released { get; set; }
        public double Rating { get; set; }
        public int? Metacritic { get; set; }
        public string Background_Image { get; set; }
        public string MetacriticCor
        {
            get
            {
                string cor = "success";
                switch (Metacritic)
                {
                    case > 79:
                        cor = "success";
                        break;
                    case > 40 and <= 79:
                        cor = "warning";
                        break;
                    case <= 39:
                        cor = "danger";
                        break;
                }
                return cor;
            }
        }
    }

}
