using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReservaEspectaculo.Models
{
    public class Pelicula
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaLanzamiento { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [MaxLength(300, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [ForeignKey(nameof(Genero))]
        [Display(Name = nameof(Genero))]
        public int GeneroId { get; set; }
        public Genero Genero { get; set; }

        public List<Funcion> Funciones { get; set; }
    }
}
