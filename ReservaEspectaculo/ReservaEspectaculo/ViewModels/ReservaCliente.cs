using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReservaEspectaculo.ViewModels
{
    public class ReservaCliente
    {
        public int PeliculaId { get; set; }
        public string PeliculaTitulo { get; set; }

        [Display(Name = "Fecha para seleccionar funciones")]
        [Required(ErrorMessage = "{0} es requerido")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Entre un valor mayor a {1}")]
        public int CantidadButacas { get; set; }
    }
}
