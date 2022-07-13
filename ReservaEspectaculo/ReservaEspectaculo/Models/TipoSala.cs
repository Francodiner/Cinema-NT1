using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservaEspectaculo.Models
{
    public class TipoSala
    {
       
        public int Id { get; set; }


        [Required(ErrorMessage = "El {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Range(100, 500, ErrorMessage = "Las {0} admiten un máximo de {2} y un mínimo de {1}")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }
    }
}