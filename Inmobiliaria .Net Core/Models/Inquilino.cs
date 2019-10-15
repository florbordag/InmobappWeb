﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public class Inquilino
	{
        [Key]
        [Display(Name = "Código")]
        public int IdInquilino { get; set; }
		[Required]
		public string Nombre { get; set; }
		[Required]
		public string Apellido { get; set; }
		[Required]
		public string Dni { get; set; }
		public string Telefono { get; set; }
		[Required, EmailAddress]
		public string Email { get; set; }
        [Required]
        [Display(Name = "Direccion de Trabajo")]
        public String DireccionTrabajo { get; set; }
        [Required]
        [Display(Name = "Nombre garante")]
        public String NombreGarante { get; set; }
        [Required]
        [Display(Name = "Dni garante")]
        public String DniGarante { get; set; }
        [Required]
        [Display(Name = "Tel. Garante")]
        public String TelGarante { get; set; }
    }
}
