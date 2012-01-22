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
            Usuario usuario = new Usuario();
            try
            {
                MySqlDataReader reader = Ejecutar.ExecuteSQL(consulta);

                

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
            }
            catch
            {
                usuario.Logged = false;
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
                usuario.FechaNacimiento = reader["fechaNacimiento"].ToString();
                usuario.NoEmpleado = reader["id"].ToString();
                usuario.Rfc = reader["rfc"].ToString();
                usuario.Direccion = reader["direccion"].ToString();
                usuario.Colonia = reader["colonia"].ToString();
                usuario.CP = reader["cp"].ToString();
                usuario.Municipio = reader["municipio"].ToString();
                usuario.Telefono = reader["telefono"].ToString();
                usuario.Celular = reader["celular"].ToString();
                usuario.Foto = reader["foto"].ToString();
                usuario.RolUser = reader["rol"].ToString();
                usuario.Activo = (bool)reader["activo"];
                usuario.FechaIngreso = reader["fechaIngreso"].ToString();
                usuario.EstadoCivil = reader["estadoCivil"].ToString();
                usuario.Edad = reader["edad"].ToString();
                usuario.Hijos = reader["hijos"].ToString();
                usuario.GradoEstudio = reader["gradoEstudio"].ToString();
                usuario.Nss = reader["nss"].ToString();

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
                usuario.NoEmpleado = reader["id"].ToString();
                usuario.Rfc = reader["rfc"].ToString();
                usuario.Direccion = reader["direccion"].ToString();
                usuario.Colonia = reader["colonia"].ToString();
                usuario.CP = reader["cp"].ToString();
                usuario.Municipio = reader["municipio"].ToString();
                usuario.Telefono = reader["telefono"].ToString();
                usuario.Celular = reader["celular"].ToString();
                usuario.Foto = reader["foto"].ToString();
                usuario.RolUser = reader["rol"].ToString();
                usuario.Activo = (bool)reader["activo"];
                usuario.FechaIngreso = reader["fechaIngreso"].ToString();
                usuario.EstadoCivil = reader["estadoCivil"].ToString();
                usuario.Edad = reader["edad"].ToString();
                usuario.Hijos = reader["hijos"].ToString();
                usuario.GradoEstudio = reader["gradoEstudio"].ToString();
                usuario.Nss = reader["nss"].ToString();

                usuarios.Add(usuario);

            }

            return usuarios;
        }

        public List<Reporte> getReporte(string fechaInicial, string fechaFinal, List<int> usuarios)
        {
            string consulta = string.Empty;
            if (usuarios.Count > 0)
            {
                string ids = string.Empty;

                foreach (int usuario in usuarios)
                {
                    ids += usuario.ToString() + ",";
                }
                int inicio = ids.Length - 1;
                ids = ids.Remove(inicio, 1);

                consulta = "select v.*, concat_ws(' ', u.nombre, u.apellidoPaterno, u.apellidoMaterno) as empleado from visitas as v join usuarios as u on u.id = v.usuario where v.usuario in (" + ids + ") and v.fecha between '" + fechaInicial + "' and '" + fechaFinal + "' order by u.id, v.id";
            }
            else
            { 
                consulta = "select v.*, concat_ws(' ', u.nombre, u.apellidoPaterno, u.apellidoMaterno) as empleado from visitas as v join usuarios as u on u.id = v.usuario where v.fecha between '" + fechaInicial + "' and '" + fechaFinal + "' order by u.id, v.id";
            }
            MySqlDataReader reader = Ejecutar.ExecuteSQL(consulta);

            List<Reporte> reportes = new List<Reporte>();


            while (reader.Read())
            {
                Reporte reporte = new Reporte();

                reporte.Entrada = reader["timestamp"].ToString();
                reporte.Salida = formatFecha(reader["salida"].ToString(), new char[] { '-' });

                string fechaBD = reader["fecha"].ToString();
                string[] fechaBDA = fechaBD.Split(new char[] { '-' });

                DateTime fecha = new DateTime(int.Parse(fechaBDA[0].ToString()),
                    int.Parse(fechaBDA[1]), 
                    int.Parse(fechaBDA[2]));
                
                reporte.Dia = dayOfWeek(fecha);
                reporte.TotalHoras = TotalHoras(reporte.Entrada, reader["salida"].ToString());
                reporte.NombreCompleto = reader["empleado"].ToString();

                reportes.Add(reporte);

            }
            return reportes;
        }

        public string formatFecha(string fecha, char[] delimiter)
        {
            string result = "";
            if (!string.IsNullOrEmpty(fecha))
            {
                string[] fechaDias = fecha.Split(new char[] { ' ' })[0].Split(delimiter);
                if (fecha.Split(new char[] { ' ' }).Count() > 1)
                {
                    string[] fechaHoras = fecha.Split(new char[] { ' ' })[1].Split(new char[] { ':' });
                    result = fechaDias[2].ToString() + "/" + fechaDias[1].ToString() + "/" + fechaDias[0].ToString() + " " + fecha.Split(new char[] { ' ' })[1] + " " + fecha.Split(new char[] { ' ' })[2];
                }
                else
                {
                    result = fechaDias[2].ToString() + "/" + fechaDias[1].ToString() + "/" + fechaDias[0].ToString();
                }
                
            }
            return result;
        }

        public String TotalHoras(string fechaI, string fechaF)
        {
            string difference = "No Registro salida";
            if (!string.IsNullOrEmpty(fechaF))
            {
                string fechaBD = fechaI;
                string fechaBDS = fechaF;

                string[] fechaBDA = fechaBD.Split(new char[] { ' ' })[0].Split(new char[] { '/' });
                string[] fechaBDH = fechaBD.Split(new char[] { ' ' })[1].Split(new char[] { ':' });

                DateTime old = new DateTime(int.Parse(fechaBDA[2].ToString()),
                    int.Parse(fechaBDA[1]),
                    int.Parse(fechaBDA[0]), int.Parse(fechaBDH[0].ToString()), int.Parse(fechaBDH[1].ToString()), int.Parse(fechaBDH[2].ToString()));


                string[] fechaBDSA = fechaBDS.Split(new char[] { ' ' })[0].Split(new char[] { '-' });
                string[] fechaBDSH = fechaBDS.Split(new char[] { ' ' })[1].Split(new char[] { ':' });

                DateTime nueva = new DateTime(int.Parse(fechaBDSA[0].ToString()),
                    int.Parse(fechaBDSA[1]),
                    int.Parse(fechaBDSA[2]), int.Parse(fechaBDSH[0].ToString()), int.Parse(fechaBDSH[1].ToString()), int.Parse(fechaBDSH[2].ToString()));



                // Difference in days, hours, and minutes.
                TimeSpan ts = nueva - old;

                // Difference in days.
                difference = ts.Hours.ToString() + " Hora(s) y " + ts.Minutes.ToString() + " Minuto(s)";
            }
            return difference;
            
        }


        public String dayOfWeek(DateTime? date)
        {
            return date.Value.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));
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
            string[] fechaA = DateTime.Now.ToString("dd/MM/yyyy").Split(new char[] { '/' });
            string dia = fechaA[0].ToString();
            string mes = fechaA[1].ToString();
            string ano = fechaA[2].ToString();
            string fecha = ano + "-" + mes + "-" + dia;
            //string fecha = dia + "/" + mes + "/" + ano;
            return fecha;
        }

        private string getFechaUSD()
        {
            string[] fechaA = DateTime.Now.ToShortDateString().Split(new char[] { '/' });
            string dia = fechaA[0].ToString();
            string mes = fechaA[1].ToString();
            string ano = fechaA[2].ToString();
            string fecha = ano + "-" + mes + "-" + dia + " " + DateTime.Now.ToString("T");
            return fecha;
        }

        

        public bool registrarEntrada(string usuario)
        {
            string fecha = this.getFecha();

            string insert = "insert into visitas (usuario, fecha) values (" + usuario + ",'" + fecha + "')";

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
    