using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MySql.Data;
using MySql.Data.MySqlClient;

using Recepcion.Common;

namespace Recepcion.Data
{
    public class DB
    {
        public Usuario validateUser(string user, string password)
        {
            
            string consulta = "select * from usuarios where usuario = '"+user+"' and password = md5('"+password+"') limit 1";
            MySqlDataReader reader = Ejecutar.ExecuteSQL(consulta);

            Usuario usuario = new Usuario();

            while (reader.Read())
            {
                usuario.idUsuario = (int)reader["id"];
                usuario.User = reader["usuario"].ToString();
                usuario.Password = reader["password"].ToString();
                usuario.Nombre = reader["nombre"].ToString();
                usuario.ApellidoPaterno = reader["apellidoPaterno"].ToString();
                usuario.ApellidoMaterno = reader["apellidoMaterno"].ToString();
                usuario.LugarNacimiento = reader["lugarNacimiento"].ToString();
                usuario.RolUser = reader["rol"].ToString();
                usuario.Activo = (bool)reader["activo"];
                usuario.Logged = true;

            }

            return usuario;
        }

        public List<Usuario> getUsers()
        {
            string consulta = "select * from usuarios";
            MySqlDataReader reader = Ejecutar.ExecuteSQL(consulta);

            List<Usuario> usuarios = new List<Usuario>();
            

            while (reader.Read())
            {
                Usuario usuario = new Usuario();
                usuario.idUsuario = (int)reader["id"];
                usuario.User = reader["usuario"].ToString();
                usuario.Password = reader["password"].ToString();
                usuario.Nombre = reader["nombre"].ToString();
                usuario.ApellidoPaterno = reader["apellidoPaterno"].ToString();
                usuario.ApellidoMaterno = reader["apellidoMaterno"].ToString();
                usuario.LugarNacimiento = reader["lugarNacimiento"].ToString();
                usuario.NoEmpleado = reader["no_empleado"].ToString();
                usuario.Rfc = reader["rfc"].ToString();
                usuario.FechaNacimiento = reader["fechaNacimiento"].ToString();
                usuario.Direccion = reader["direccion"].ToString();
                usuario.Colonia = reader["colonia"].ToString();
                usuario.CP = reader["cp"].ToString();
                usuario.Municipio = reader["municipio"].ToString();
                usuario.Telefono = reader["telefono"].ToString();
                usuario.Celular = reader["celular"].ToString();
                usuario.TelefonoAdicional = reader["telefonoAdicional"].ToString();
                usuario.Foto = reader["foto"].ToString();
                usuario.RolUser = reader["rol"].ToString();
                usuario.Activo = (bool)reader["activo"];

                usuarios.Add(usuario);

            }

            return usuarios;
        }

        public List<Usuario> getUsers(string usr)
        {
            string consulta = "select * from usuarios where nombre like '%"+usr+"%' or apellidoPaterno like '%"+usr+"%' or apellidoMaterno like '%"+usr+"%'";
            MySqlDataReader reader = Ejecutar.ExecuteSQL(consulta);

            List<Usuario> usuarios = new List<Usuario>();


            while (reader.Read())
            {
                Usuario usuario = new Usuario();
                usuario.idUsuario = (int)reader["id"];
                usuario.User = reader["usuario"].ToString();
                usuario.Password = reader["password"].ToString();
                usuario.Nombre = reader["nombre"].ToString();
                usuario.ApellidoPaterno = reader["apellidoPaterno"].ToString();
                usuario.ApellidoMaterno = reader["apellidoMaterno"].ToString();
                usuario.LugarNacimiento = reader["lugarNacimiento"].ToString();
                usuario.FechaNacimiento = reader["fechaNacimiento"].ToString();
                usuario.NoEmpleado = reader["no_empleado"].ToString();
                usuario.Rfc = reader["rfc"].ToString();
                usuario.Direccion = reader["direccion"].ToString();
                usuario.Colonia = reader["colonia"].ToString();
                usuario.CP = reader["cp"].ToString();
                usuario.Municipio = reader["municipio"].ToString();
                usuario.Telefono = reader["telefono"].ToString();
                usuario.Celular = reader["celular"].ToString();
                usuario.TelefonoAdicional = reader["telefonoAdicional"].ToString();
                usuario.Foto = reader["foto"].ToString();
                usuario.RolUser = reader["rol"].ToString();
                usuario.Activo = (bool)reader["activo"];

                usuarios.Add(usuario);

            }

            return usuarios;
        }

        public List<Reporte> getReporte(string fechaInicial, string fechaFinal, List<int> usuarios)
        {
            string consulta = "select * from usuarios";
            MySqlDataReader reader = Ejecutar.ExecuteSQL(consulta);

            List<Usuario> usuarios = new List<Usuario>();


            while (reader.Read())
            {
                Usuario usuario = new Usuario();
                usuario.idUsuario = (int)reader["id"];
                usuario.User = reader["usuario"].ToString();
                usuario.Password = reader["password"].ToString();
                usuario.Nombre = reader["nombre"].ToString();
                usuario.ApellidoPaterno = reader["apellidoPaterno"].ToString();
                usuario.ApellidoMaterno = reader["apellidoMaterno"].ToString();
                usuario.LugarNacimiento = reader["lugarNacimiento"].ToString();
                usuario.NoEmpleado = reader["no_empleado"].ToString();
                usuario.Rfc = reader["rfc"].ToString();
                usuario.FechaNacimiento = reader["fechaNacimiento"].ToString();
                usuario.Direccion = reader["direccion"].ToString();
                usuario.Colonia = reader["colonia"].ToString();
                usuario.CP = reader["cp"].ToString();
                usuario.Municipio = reader["municipio"].ToString();
                usuario.Telefono = reader["telefono"].ToString();
                usuario.Celular = reader["celular"].ToString();
                usuario.TelefonoAdicional = reader["telefonoAdicional"].ToString();
                usuario.Foto = reader["foto"].ToString();
                usuario.RolUser = reader["rol"].ToString();
                usuario.Activo = (bool)reader["activo"];

                usuarios.Add(usuario);

            }
            return new List<Reporte>();
        }


        public string isInOut(string usr)
        {
            string consulta = "select id, fecha, timestamp, fecha_salida, salida, tipo, usuario from visitas v where fecha = '"+this.getFecha()+"' and usuario = "+usr;
            MySqlDataReader reader = Ejecutar.ExecuteSQL(consulta);

            string result = "In";
            
            while (reader.Read())
            {
                string salida = reader["fecha_salida"].ToString();
                if (String.IsNullOrEmpty(salida))
                {
                    result = "Out";
                }
            }

            return result;
        }

        public bool eliminarUsuario(string id)
        { 
            string query = "delete from usuarios where id = "+id;
            if (Ejecutar.ExecuteNonSQL(query) > 0)
                return true;
            else
                return false;
        }

        private string getFecha()
        {
            string[] fechaA = DateTime.Now.ToShortDateString().Split(new char[] { '/' });
            string dia = fechaA[0].ToString();
            string mes = fechaA[1].ToString();
            string ano = fechaA[2].ToString().Replace("20", "");
            string fecha = dia + "/" + mes + "/" + ano;
            return fecha;
        }

        private string getFechaUSD()
        {
            string[] fechaA = DateTime.Now.ToShortDateString().Split(new char[] { '/' });
            string dia = fechaA[0].ToString();
            string mes = fechaA[1].ToString();
            string ano = fechaA[2].ToString();
            string fecha = ano + "-" + mes + "-" + dia + " " + DateTime.Now.ToString("HH:mm:ss");
            return fecha;
        }

        public bool registrarEntrada(List<int> horarios, string usuario)
        {
            string fecha = this.getFecha();
            int count = 0;
            foreach (int id in horarios)
            {
                string insert = "insert into visitas (horario, fecha, usuario) values ("+id+",'"+fecha+"',"+usuario+")";
                
                if(Ejecutar.ExecuteNonSQL(insert) > 0)
                    count++;
            }

            if (count > 0)
                return true;
            else
                return false;
        }

        public bool registrarEntrada(string usuario)
        {
            string fecha = this.getFecha();
            
            string insert = "insert into visitas (horario, fecha, usuario) values (3,'" + fecha + "'," + usuario + ")";

            if (Ejecutar.ExecuteNonSQL(insert) > 0)
                return true;
            else
                return false;
        }

        public bool registrarSalida(string usuario)
        {
            string insert = "update visitas set fecha_salida = '" + this.getFecha() + "', salida = '" + this.getFechaUSD() + "' where usuario = " + usuario + " and fecha = '" + this.getFecha() + "' and fecha_salida is null";
            if (Ejecutar.ExecuteNonSQL(insert) > 0)
                return true;
            else
                return false;
        }
    }
}
    