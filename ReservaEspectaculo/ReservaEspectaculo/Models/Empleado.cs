using ReservaEspectaculo.Validaciones;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReservaEspectaculo.Models
{
    public class Empleado : Usuario
    {
        [Required(ErrorMessage = "El {0} es requerido")]
        [MaxLength(7, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        [StringRange("A000000", "A999999", ErrorMessage = "El {0} se debe encontrar entre A000000 y A999999")]
        public string Legajo { get; set; }
    }
}
