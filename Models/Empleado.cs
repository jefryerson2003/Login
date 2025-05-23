using System;

namespace EmpleadosApp.Models
{
    public class Empleado
    {
        public int EmpleadoID { get; set; }

        // inicializador = null! para quitar la warning CS8618
        public string Nombre { get; set; } = null!;
        public DateTime FechaIngreso { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }
}
