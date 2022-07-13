using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReservaEspectaculo.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaAlta { get; set; }

        [Required(ErrorMessage = "{0} es requerida")]
        [ForeignKey(nameof(Cliente))]
        [Display(Name = nameof(Cliente))]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required(ErrorMessage = "{0} es requerida")]
        [ForeignKey(nameof(Funcion))]
        [Display(Name = nameof(Funcion))]
        public int FuncionId { get; set; }
        public Funcion Funcion { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Las {0} admiten un máximo de {2} y un mínimo de {1}")]
        public int CantidadButacas { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        public bool  Activa { get; set; }
        public bool Cancelable { get { return esCancelable(); } }

        private bool esCancelable()
        {
            bool res = false;
            if (Funcion != null && Activa)
            {
                res = (Funcion.Fecha - DateTime.Now).TotalHours >= 24;
            }
            return res;
        }
    }
}
