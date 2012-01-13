using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Recepcion.Data
{
    class Conexion_MySQL
    {
        private string conexionString = String.Empty;
        private MySqlConnection conexion = null;
        public Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public void Conectar()
        {

            conexionString = config.AppSettings.Settings["MySQL"].Value;
            conexion = new MySqlConnection(conexionString);
        }

        public MySqlConnection Cnx
        {
            get
            {
                Conectar();
                conexion.Open();
                return conexion;
            }
        }
    }

    class Ejecutar
    {
        private static MySqlCommand command;
        private static Conexion_MySQL Conexion;

        public static MySqlDataReader ExecuteSQL(string Query)
        {
            Conexion = new Conexion_MySQL();
            command = new MySqlCommand(Query, Conexion.Cnx);
            return command.ExecuteReader();
        }

        public static int ExecuteNonSQL(string Query)
        {
            Conexion = new Conexion_MySQL();
            command = new MySqlCommand(Query, Conexion.Cnx);
            try
            {
                return command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                return 0;
            }

        }

        public static void GuardarHuella(string Name, byte[] Template1, byte[] Template2, byte[] Template3)
        {
            Conexion = new Conexion_MySQL();

            string query = @"INSERT INTO usuarios (Name, Template1, Template2, Template3)
                         VALUES (?Name, ?Template1, ?Template2, ?Template3)";

            MySqlCommand cmd = new MySqlCommand(query, Conexion.Cnx);

            cmd.Parameters.AddWithValue("?Name", Name);

            MySqlParameter imageParam1 = cmd.Parameters.Add("?Template1", MySqlDbType.Blob);
            imageParam1.Value = Template1;

            MySqlParameter imageParam2 = cmd.Parameters.Add("?Template2", MySqlDbType.Blob);
            imageParam2.Value = Template2;

            MySqlParameter imageParam3 = cmd.Parameters.Add("?Template3", MySqlDbType.Blob);
            imageParam3.Value = Template3;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {

            }

        }

        public static void GuardarHuella()
        {

        }
    }
}

