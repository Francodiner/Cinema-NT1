using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReservaEspectaculo.Models
{
    public class Usuario : IdentityUser<int>
    {
         //public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [RegularExpression(@"[0-9]{2}\.[0-9]{3}\.[0-9]{3}", ErrorMessage = "El {0} debe tener un formato NN.NNN.NNN")]
        public string Dni { get; set; }

        [MaxLength(20, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        [RegularExpression(@"[0-9]{3}\-[0-9]{4}\-[0-9]{4}", ErrorMessage = "El {0} debe tener un formato NNN-NNNN-NNNN")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La {0} es requerida")]
        [MaxLength(100, ErrorMessage = "La {0} admite un máximo de {1} caracteres")]
        public string Direccion { get; set; }

        //[Required(ErrorMessage = "El {0} es requerido")]
        //[DataType(DataType.EmailAddress, ErrorMessage = "El {0} es erroneo")]
        //public string Email { get; set; }

        public DateTime FechaAlta { get; set; }

        //[Required(ErrorMessage = "El {0} es requerido")]
        //[DataType(DataType.Password, ErrorMessage = "El {0} es erroneo")]
        //public string Password { get; set; }

    }
}
