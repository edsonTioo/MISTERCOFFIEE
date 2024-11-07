using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISTERCOFFIEE.MVVM.MODEL
{
    public class Empleado
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public string Estado { get; set; } = "Activo";
        public int Telefono { get; set; }
        public int Pago { get; set; }
        public string Cedula { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Rol { get; set; }
    }
}
