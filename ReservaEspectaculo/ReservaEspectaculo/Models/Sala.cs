using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservaEspectaculo.Models
{
    public class Sala
    {
       
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "{0} es requerida")]
        [ForeignKey(nameof(TipoSala))]
        [Display(Name = nameof(TipoSala))]
        public int TipoSalaId { get; set; }
        public TipoSala TipoSala { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [Range(10, 100, ErrorMessage = "Las {0} admiten un máximo de {2} y un mínimo de {1}")]
        public int CapacidadButacas { get; set; }

        public List<Funcion> Funciones { get; set; }

    }
}