using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservaEspectaculo.Models
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        public List<Pelicula> Peliculas { get; set; }
    }
}
