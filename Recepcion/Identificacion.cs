using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

using Recepcion.Common;
using Recepcion.Logic;
using System.Configuration;

namespace Recepcion
{
    public partial class Identificacion : Form
    {
        GriauleFingerprintLibrary.FingerprintCore core;
        GriauleFingerprintLibrary.DataTypes.FingerprintRawImage huella;
        GriauleFingerprintLibrary.DataTypes.FingerprintTemplate template;

        private static MySqlCommand command;
        private static Conexion_MySQL Conexion;

        public string Usuario { get; set; }
        public string Rol = String.Empty;

        public Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        string RutaFotos = string.Empty;
        

        private string getDay(string day)
        {
            switch (day)
            {
                case "Sunday":
                    return "dom";
                case "Monday":
                    return "lun";
                case "Tuesday":
                    return "mar";
                case "Wednesday":
                    return "mie";
                case "Thursday":
                    return "jue";
                case "Friday":
                    return "vie";
                case "Saturday":
                    return "sab";
                default:
                    break;
            }
            return "";
        }

        public Identificacion()
        {
            InitializeComponent();
        }

        private void WriteLog(string log) 
        {
            string dir = config.AppSettings.Settings["LogPath"].Value.ToString();
            string fileName = @dir + "log_" + DateTime.Now.Date.ToShortDateString().Replace('/','-') + ".txt";
            System.IO.StreamWriter writer = System.IO.File.AppendText(fileName);
            writer.WriteLine(DateTime.Now.ToString());
            writer.WriteLine(log);
            writer.Close();
        }

        private void Identificacion_Load(object sender, EventArgs e)
        {

            WriteLog("Iniciando Aplicacion.");
            CheckForIllegalCrossThreadCalls = false;
            core = new GriauleFingerprintLibrary.FingerprintCore();

            core.onStatus += new GriauleFingerprintLibrary.StatusEventHandler(core_onStatus);
            core.onImage += new GriauleFingerprintLibrary.ImageEventHandler(core_onImage);
            
            RutaFotos = config.AppSettings.Settings["RutaFotos"].Value.ToString();
            try
            {
                core.Initialize();
                core.CaptureInitialize();
                WriteLog("Lector Iniciado.");
            }
            catch(Exception ex){
                WriteLog("Error al iniciar el lector - Error: "+ex.Message);
            }
            
        }

        void core_onImage(object source, GriauleFingerprintLibrary.Events.ImageEventArgs ie)
        {
            try
            {
                WriteLog("Obteniendo Huella.");
                huella = ie.RawImage;
                core.Extract(huella, ref template);
                string consulta;
                byte[] dataTemp;
                GriauleFingerprintLibrary.DataTypes.FingerprintTemplate templateTemp;
                int precision, calidad;

                // selecciono 
                consulta = "select id, id as no_empleado, telefono, rfc, rol, concat_ws(' ', nombre, apellidoPaterno, apellidoMaterno) as nombreCompleto, template, calidad_template, foto from usuarios where template is not null and 1 = 1";

                MySqlDataReader reader = this.EjecutarQuery(consulta);
                core.IdentifyPrepare(template);

                while (reader.Read())
                {
                    dataTemp = (byte[])reader["template"];
                    calidad = (int)reader["calidad_template"];
                    templateTemp = new GriauleFingerprintLibrary.DataTypes.FingerprintTemplate();
                    templateTemp.Buffer = dataTemp;
                    templateTemp.Size = dataTemp.Length;
                    templateTemp.Quality = calidad;

                    int result = core.Identify(templateTemp, out precision);
                    WriteLog("Resultado: "+result.ToString() + " Precision: " +  precision.ToString());
                    
                    
                    if (result == 1)
                    {
                        
                        //
                        Usuario = reader["id"].ToString();
                        string no_Empleado = reader["no_empleado"].ToString();
                        string nombreCompleto = reader["nombreCompleto"].ToString();
                        string rfc = reader["rfc"].ToString();
                        string telefono = reader["telefono"].ToString();
                        string rol = reader["rol"].ToString();
                        string foto = reader["foto"].ToString();
                        Rol = rol;

                        WriteLog("Huella encontrada.");
                        WriteLog("Empleado: " + no_Empleado);

                        string inout = new Rules().isInOut(Usuario);
                        if (inout == "In")
                        {
                            if (new Rules().registrarEntrada(Usuario))
                            {
                                this.no_personal.Text = no_Empleado;
                                this.nombre_completo.Text = nombreCompleto;
                                this.rfc.Text = rfc;
                                this.telefono.Text = telefono;
                                label1.Text = "Entrada";

                                if (!String.IsNullOrEmpty(foto))
                                {
                                    fotoGrafia.ImageLocation = @RutaFotos + foto;
                                }

                                this.WindowState = FormWindowState.Normal;
                                WriteLog("Entrada Registrada. " + no_Empleado);
                            }
                            else
                            {
                                MessageBox.Show("No hemos podido registrar su entrada");
                                WriteLog("No hemos podido registrar su entrada" + no_Empleado);
                            }
                        }
                        else
                        {
                            if (new Rules().registrarSalida(Usuario))
                            {
                                this.no_personal.Text = no_Empleado;
                                this.nombre_completo.Text = nombreCompleto;
                                this.rfc.Text = rfc;
                                this.telefono.Text = telefono;

                                if (!String.IsNullOrEmpty(foto))
                                {
                                    fotoGrafia.ImageLocation = @RutaFotos + foto;
                                }

                                this.no_personal.Text = no_Empleado;
                                this.nombre_completo.Text = nombreCompleto;
                                this.rfc.Text = rfc;
                                this.telefono.Text = telefono;
                                label1.Text = "Salida";

                                if (!String.IsNullOrEmpty(foto))
                                {
                                    fotoGrafia.ImageLocation = @RutaFotos + foto;
                                }

                                this.WindowState = FormWindowState.Normal;
                                WriteLog("Salida Registrada. " + no_Empleado);
                            }

                        }

                        break;
                    }
                    else
                    {
                        WriteLog("No concuerda"); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clean()
        {
            int miliseconds = int.Parse(string.IsNullOrEmpty(config.AppSettings.Settings["Clean"].Value.ToString()) ? "8" : config.AppSettings.Settings["Clean"].Value.ToString());
            System.Threading.Thread.Sleep(miliseconds * 1000);
            fotoGrafia.ImageLocation = null;
            this.no_personal.Text = "";
            this.nombre_completo.Text = "";
            this.rfc.Text = "";
            this.telefono.Text = "";
            label1.Text = "";
            WriteLog("Limpiando datos.");
        }

        void core_onStatus(object source, GriauleFingerprintLibrary.Events.StatusEventArgs se)
        {
            if (se.StatusEventType == GriauleFingerprintLibrary.Events.StatusEventType.SENSOR_PLUG)
            {
                core.StartCapture(source);
               
            }
            else
            {
               
            }
        }

        private MySqlDataReader EjecutarQuery(string query)
        {
            Conexion = new Conexion_MySQL();

            MySqlCommand cmd = new MySqlCommand(query, Conexion.Cnx);

            try
            {
                return cmd.ExecuteReader();
            }
            catch (MySqlException ex)
            {
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Identificacion_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                core.CaptureFinalize();
            }
            catch
            { 
                
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

    }
}
