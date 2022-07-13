using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ReservaEspectaculo.ViewModels
{
    public class RegistroCliente
    {
        const string _errorRequerido = "El campo {0} es requerido";

        [Required(ErrorMessage = _errorRequerido)]
        [MaxLength(50, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        [EmailAddress]
        [Remote(action: "EmailDisponible", controller: "Accounts")]
        public string Email { get; set; }

        [Required(ErrorMessage = _errorRequerido)]
        [MaxLength(50, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = _errorRequerido)]
        [MaxLength(50, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        [Display(Name = "Confirmación de Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La password no coincide.")]
        public string ConfirmarPassword { get; set; }

        [Required(ErrorMessage = _errorRequerido)]
        [MaxLength(50, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = _errorRequerido)]
        [MaxLength(50, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = _errorRequerido)]
        [RegularExpression(@"[0-9]{2}\.[0-9]{3}\.[0-9]{3}", ErrorMessage = "El {0} debe tener un formato NN.NNN.NNN")]
        public string Dni { get; set; }

        [Required(ErrorMessage = _errorRequerido)]
        [MaxLength(100, ErrorMessage = "La {0} admite un máximo de {1} caracteres")]
        public string Direccion { get; set; }

    }
}
