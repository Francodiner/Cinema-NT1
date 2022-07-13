using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReservaEspectaculo.Models
{
    public class Funcion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        public String Descripcion { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Las {0} admiten un máximo de {2} y un mínimo de {1}")]
        public int ButacarDisponibles { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        public Boolean Confirmada { get; set; }

        [Required(ErrorMessage = "{0} es requerida")]
        [ForeignKey(nameof(Pelicula))]
        [Display(Name = "Pelicula")]
        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; }

        [Required(ErrorMessage = "{0} es requerida")]
        [ForeignKey(nameof(Sala))]
        [Display(Name = nameof(Sala))]
        public int SalaId { get; set; }
        public Sala Sala { get; set; }

        public bool Reservable { get { return ((Fecha - DateTime.Now).TotalDays <= 7 && (Fecha - DateTime.Now).TotalDays > 0 && ButacarDisponibles > 0); } }

        public List<Reserva> Reservas { get; set; }

    }
}
