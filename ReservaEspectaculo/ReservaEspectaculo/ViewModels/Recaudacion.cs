using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservaEspectaculo.ViewModels
{
    public class Recaudacion
    {

        public string TituloPelicula { get; set; }
        public string Mes { get; set; }
        public int? Anio { get; set; }
        public decimal Importe { get; set; }

    }
}
