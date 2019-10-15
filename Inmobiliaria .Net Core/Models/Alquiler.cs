using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Inmobiliaria_.Net_Core.Models
{
    public class Alquiler
    {
        public int IdContrato { get; set; }
        [Required, Display(Name = "Locador")]
        public Inmueble Inmueble { get; set; }
        public int IdInmueble { get; set; }
        [Required, Display(Name = "Locatario")]
        public Inquilino Inquilino { get; set; }
        public int IdInquilino { get; set; }
        [Required, Display(Name = "Fecha Inicio")]
        public String FechaInicio { get; set; }
        [Display(Name = "Fecha Fin")]
        public String FechaFin { get; set; }
        [Required, Display(Name = "Monto Total")]
        public decimal MontoTotal { get; set; }
        public Nullable<Decimal> Multa { get; set; }
        public String ProximoFin { get; set; }
        public Boolean Vigente { get; set; }
        public Boolean vigente()
        {
            
            DateTime hoy = DateTime.Now;
            DateTime fin = Convert.ToDateTime(FechaFin);
            DateTime ini = Convert.ToDateTime(FechaInicio);

            if (hoy < fin) { return true; } else { return false; }

        }
       public decimal calcularMulta()
        {
            Decimal multa= MontoTotal;

            DateTime hoy = DateTime.Now; DateTime fin;
            if (ProximoFin != null) { fin = Convert.ToDateTime(ProximoFin); } else { fin = Convert.ToDateTime(FechaFin); }
            
            DateTime ini = Convert.ToDateTime(FechaInicio);
            if (fin < Convert.ToDateTime(FechaFin))
            {
                int añoini = ini.Year;
                int diaini = ini.DayOfYear;
                int añofin = fin.Year;
                int diafin = fin.DayOfYear;
                int diamid = diafin / 2;
                int añomid = ((añofin - añoini) / 2);
                DateTime fechamedia = ini.AddDays(diamid);
                fechamedia = fechamedia.AddYears(añomid);
                
                if (fin < fechamedia) { multa = MontoTotal * 2; }
            }
    
            
            return multa;
        }
   
    }
}
