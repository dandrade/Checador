using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recepcion.Common
{
    public class Usuario
    {
        public string NoEmpleado { get; set; }
        public int idUsuario { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Rfc { get; set; }
        public string LugarNacimiento { get; set; }
        public string FechaNacimiento { get; set; }
        
        
        public string Direccion { get; set; }
        public string Colonia { get; set; }
        public string CP { get; set; }
        public string Municipio { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string TelefonoAdicional { get; set; }
        public string Foto { get; set; }
        public string RolUser { get; set; }
        public bool Activo { get; set; }
        public bool Logged { get; set; }
    }
}
