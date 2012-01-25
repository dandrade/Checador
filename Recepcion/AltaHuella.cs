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
    public partial class AltaHuella : Form
    {
        GriauleFingerprintLibrary.FingerprintCore core;
        GriauleFingerprintLibrary.DataTypes.FingerprintRawImage huella;
        GriauleFingerprintLibrary.DataTypes.FingerprintTemplate template;

        private static MySqlCommand command;
        private static Conexion_MySQL Conexion;

        Rules logica = new Rules();

        
        
        public Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        string RutaFotos = string.Empty;

        Usuario usuario;
        bool isEdit = false;

        public AltaHuella()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            

            try
            {
                core = new GriauleFingerprintLibrary.FingerprintCore();

                core.onStatus += new GriauleFingerprintLibrary.StatusEventHandler(core_onStatus);
                core.onImage += new GriauleFingerprintLibrary.ImageEventHandler(core_onImage);
                core.Initialize();
                core.CaptureInitialize();

                RutaFotos = config.AppSettings.Settings["RutaFotos"].Value.ToString();
            }
            catch
            {
            }

            
        }

        public AltaHuella(Usuario usr)
        {
            InitializeComponent();
            label15.Visible = true;
            no_empleado.Visible = true;
            usuario = usr;
            comboBox1.SelectedIndex = 0;
            asignarValores();
            button1.Text = "Editar Usuario";
            this.Text = "Editar Usuario";
            isEdit = true;
            
            RutaFotos = config.AppSettings.Settings["RutaFotos"].Value.ToString();

            fotoEditar.ImageLocation = @RutaFotos + usuario.Foto;
            fotoEditar.Show();
            groupBox1.Text = "Fotografia";
            
            pictureBox1.Hide();
            
        }

        private void asignarValores()
        {
            nombre.Text = usuario.Nombre;
            apellidoPaterno.Text = usuario.ApellidoPaterno;
            apellidoMaterno.Text = usuario.ApellidoMaterno;
            lugarNacimiento.Text = usuario.LugarNacimiento;
            fechaNacimiento.Text = usuario.FechaNacimiento;
            no_empleado.Text = usuario.NoEmpleado;
            rfc.Text = usuario.Rfc;
            direccion.Text = usuario.Direccion;
            colonia.Text = usuario.Colonia;
            CP.Text = usuario.CP;
            municipio.Text = usuario.Municipio;
            telefono.Text = usuario.Telefono;
            celular.Text = usuario.Celular;
            comboBox1.SelectedItem = usuario.RolUser;
            fechaIngreso.Text = usuario.FechaIngreso;
            estadoCivil.SelectedItem = usuario.EstadoCivil;
            edad.Text = usuario.Edad;
            hijos.Text = usuario.Hijos;
            gradoEstudios.SelectedItem = usuario.GradoEstudio;
            nss.Text = usuario.Nss;
            
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            //if (!isEdit)
            //{
            //    try
            //    {
            //        core = new GriauleFingerprintLibrary.FingerprintCore();

            //        core.onStatus += new GriauleFingerprintLibrary.StatusEventHandler(core_onStatus);
            //        core.onImage += new GriauleFingerprintLibrary.ImageEventHandler(core_onImage);
            //        core.Initialize();
            //        core.CaptureInitialize();
            //    }
            //    catch
            //    {
            //    }
            //}
        }


        bool HuellaOK = false;
        void core_onImage(object source, GriauleFingerprintLibrary.Events.ImageEventArgs ie)
        {
            try 
            {
                huella = ie.RawImage;
                core.Extract(huella, ref template);

                pictureBox1.Image = huella.Image;
                
                switch (template.Quality)
                {
                    case 0:
                        MessageBox.Show("Huella de mala calidad favor de volver a poner el dedo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case 1:
                        MessageBox.Show("La huella es de una calidad media, intente nuevamente", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case 2:
                        MessageBox.Show("Huella con buena calidad", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        HuellaOK = true;
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void core_onStatus(object source, GriauleFingerprintLibrary.Events.StatusEventArgs se)
        {
            if (se.StatusEventType == GriauleFingerprintLibrary.Events.StatusEventType.SENSOR_PLUG)
            {
                core.StartCapture(source);
                mensajeBarraEstado.Text = "Lector Conectado";
            }
            else
            {
                mensajeBarraEstado.Text = "Lector Desconectado";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isEdit)
            {
                editarUsuarioClick();
            }
            else
            {
                agregarUsuarioClick();
            }

        }

        private void agregarUsuarioClick()
        {
            if (!String.IsNullOrEmpty(nombre.Text) && HuellaOK)
            {
                try
                {
                    if (!String.IsNullOrEmpty(textBox1.Text))
                    {
                        try
                        {
                            Fotografia = GetFileName(textBox1.Text).Replace(".", DateTime.Now.Ticks.ToString() + ".");
                            System.IO.File.Copy(textBox1.Text, @RutaFotos + Fotografia);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
                catch
                {
                }

                Conexion = new Conexion_MySQL();

                string query = @"INSERT INTO usuarios (
                                    nombre, 
                                    apellidoPaterno, 
                                    apellidoMaterno,
                                    rfc,
                                    lugarNacimiento,
                                    fechaNacimiento,
                                    direccion,
                                    colonia,
                                    cp,
                                    municipio,
                                    telefono,
                                    celular,
                                    foto,
                                    rol,
                                    activo,
                                    template,
                                    calidad_template,
                                    fechaIngreso,
                                    estadoCivil,
                                    edad,
                                    hijos,
                                    gradoEstudio,
                                    nss
                                )
                                VALUES 
                                (
                                    ?nombre, 
                                    ?apellidoPaterno, 
                                    ?apellidoMaterno,
                                    ?rfc,
                                    ?lugarNacimiento,
                                    ?fechaNacimiento,
                                    ?direccion,
                                    ?colonia,
                                    ?cp,
                                    ?municipio,
                                    ?telefono,
                                    ?celular,
                                    ?foto,
                                    ?rol,
                                    ?activo,
                                    ?template,
                                    ?calidad_template,
                                    ?fechaIngreso,
                                    ?estadoCivil,
                                    ?edad,
                                    ?hijos,
                                    ?gradoEstudio,
                                    ?nss
                                ); select LAST_INSERT_ID()";

                MySqlCommand cmd = new MySqlCommand(query, Conexion.Cnx);

                cmd.Parameters.AddWithValue("?nombre", nombre.Text);
                cmd.Parameters.AddWithValue("?apellidoPaterno", apellidoPaterno.Text);
                cmd.Parameters.AddWithValue("?apellidoMaterno", apellidoMaterno.Text);
                cmd.Parameters.AddWithValue("?rfc", rfc.Text);
                cmd.Parameters.AddWithValue("?lugarNacimiento", lugarNacimiento.Text);
                cmd.Parameters.AddWithValue("?fechaNacimiento", fechaNacimiento.Text);
                cmd.Parameters.AddWithValue("?direccion", direccion.Text);
                cmd.Parameters.AddWithValue("?colonia", colonia.Text);
                cmd.Parameters.AddWithValue("?cp", CP.Text);
                cmd.Parameters.AddWithValue("?municipio", municipio.Text);
                cmd.Parameters.AddWithValue("?telefono", telefono.Text);
                cmd.Parameters.AddWithValue("?celular", celular.Text);
                
                cmd.Parameters.AddWithValue("?foto", Fotografia);
                cmd.Parameters.AddWithValue("?rol", comboBox1.SelectedItem);
                cmd.Parameters.AddWithValue("?activo", "1");
                cmd.Parameters.AddWithValue("?calidad_template", template.Quality.ToString());
                cmd.Parameters.AddWithValue("?fechaIngreso", fechaIngreso.Text);
                cmd.Parameters.AddWithValue("?estadoCivil", estadoCivil.SelectedItem);
                cmd.Parameters.AddWithValue("?edad", edad.Text);
                cmd.Parameters.AddWithValue("?hijos", hijos.Text);
                cmd.Parameters.AddWithValue("?gradoEstudio", gradoEstudios.SelectedItem);
                cmd.Parameters.AddWithValue("?nss", nss.Text);
                


                MySqlParameter templateParam = cmd.Parameters.Add("?template", MySqlDbType.Blob);
                templateParam.Value = (object)template.Buffer;


                try
                {
                    int newID = Convert.ToInt32(cmd.ExecuteScalar());

                    if (newID > 0)
                    {
                        MessageBox.Show("Usuario Agregado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clean();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo agregar el usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (MySqlException ex)
                {

                }
            }
            else
            {
                MessageBox.Show("Por favor capture su huella", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void editarUsuarioClick()
        {
            if (!String.IsNullOrEmpty(nombre.Text))
            {

                try
                {
                    if (!String.IsNullOrEmpty(textBox1.Text))
                    {
                        Fotografia = GetFileName(textBox1.Text).Replace(".", DateTime.Now.Ticks.ToString() + ".");
                        System.IO.File.Copy(textBox1.Text, @RutaFotos + Fotografia);
                    }
                    else
                    {
                        Fotografia = usuario.Foto;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                Conexion = new Conexion_MySQL();

                string query = @"UPDATE usuarios set
                                    nombre = ?nombre, 
                                    apellidoPaterno = ?apellidoPaterno, 
                                    apellidoMaterno = ?apellidoMaterno,
                                    rfc = ?rfc,
                                    lugarNacimiento = ?lugarNacimiento,
                                    fechaNacimiento = ?fechaNacimiento,
                                    direccion = ?direccion,
                                    colonia = ?colonia,
                                    cp = ?cp,
                                    municipio = ?municipio,
                                    telefono = ?telefono,
                                    celular = ?celular,
                                    foto = ?foto,
                                    rol = ?rol,
                                    fechaIngreso = ?fechaIngreso,
                                    estadoCivil = ?estadoCivil,
                                    edad = ?edad,
                                    hijos = ?hijos,
                                    gradoEstudio = ?gradoEstudio,
                                    nss = ?nss
                                    where id = ?id
                                ";

                MySqlCommand cmd = new MySqlCommand(query, Conexion.Cnx);

                cmd.Parameters.AddWithValue("?nombre", nombre.Text);
                cmd.Parameters.AddWithValue("?apellidoPaterno", apellidoPaterno.Text);
                cmd.Parameters.AddWithValue("?apellidoMaterno", apellidoMaterno.Text);
                cmd.Parameters.AddWithValue("?rfc", rfc.Text);
                cmd.Parameters.AddWithValue("?lugarNacimiento", lugarNacimiento.Text);
                cmd.Parameters.AddWithValue("?fechaNacimiento", fechaNacimiento.Text);
                cmd.Parameters.AddWithValue("?direccion", direccion.Text);
                cmd.Parameters.AddWithValue("?colonia", colonia.Text);
                cmd.Parameters.AddWithValue("?cp", CP.Text);
                cmd.Parameters.AddWithValue("?municipio", municipio.Text);
                cmd.Parameters.AddWithValue("?telefono", telefono.Text);
                cmd.Parameters.AddWithValue("?celular", celular.Text);

                cmd.Parameters.AddWithValue("?foto", Fotografia);
                cmd.Parameters.AddWithValue("?rol", comboBox1.SelectedItem);
                cmd.Parameters.AddWithValue("?activo", "1");
                cmd.Parameters.AddWithValue("?fechaIngreso", fechaIngreso.Text);
                cmd.Parameters.AddWithValue("?estadoCivil", estadoCivil.SelectedItem);
                cmd.Parameters.AddWithValue("?edad", edad.Text);
                cmd.Parameters.AddWithValue("?hijos", hijos.Text);
                cmd.Parameters.AddWithValue("?gradoEstudio", gradoEstudios.SelectedItem);
                cmd.Parameters.AddWithValue("?nss", nss.Text);
                
                cmd.Parameters.AddWithValue("?id", usuario.idUsuario);

                try
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Usuario Editado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //clean();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo editar el usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("El nombre/huella no puede estar en blanco.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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


        private void clean()
        {
            no_empleado.Text = "";
            rfc.Text = "";
            nombre.Text = ""; 
            apellidoPaterno.Text = ""; 
            apellidoMaterno.Text = "";
            lugarNacimiento.Text = "";
            fechaNacimiento.Text = "";
            direccion.Text = "";
            colonia.Text = "";
            CP.Text = "";
            municipio.Text = "";
            telefono.Text = "";
            celular.Text = "";
            textBox1.Text = "";
            
             
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void AltaHuella_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                core.CaptureFinalize();
                //core.Finalizer();
            }
            catch (Exception ex)
            {
                
            }
        }

        private void AltaHuella_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                core.CaptureFinalize();
               // core.Finalizer();
            }
            catch (Exception ex)
            {
                
            }
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        string Fotografia;
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult resultado = openFileDialog1.ShowDialog();

            if (resultado == DialogResult.OK)
            {

                textBox1.Text = openFileDialog1.FileName;

                
            }
        }

        public string GetFileName(string path)
        {
            string ruta, nombre;
            ruta = System.IO.Path.GetDirectoryName(path);
            nombre = path.ToString().Replace(ruta, "");
            return nombre.Replace("\\", "");
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        
        private void descuento_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void descuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }
    }
}
