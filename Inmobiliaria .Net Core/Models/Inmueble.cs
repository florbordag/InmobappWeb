using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public class Inmueble
	{
        [Display(Name = "Código")]
        public int Id { get; set; }
		[Required]
		public string Direccion { get; set; }
		[Required]
		public int Ambientes { get; set; }
		[Required]
        public decimal Precio { get; set; }
        public string Tipo { get; set; }
        public string Uso { get; set; }
        public int Disponible { get; set; }
        public int PropietarioId { get; set; }
        [ForeignKey("PropietarioId")]
        public Propietario Duenio { get; set; }
    }
}
