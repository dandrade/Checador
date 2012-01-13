using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Recepcion.Common;
using Recepcion.Logic;

namespace Recepcion
{
    public partial class Usuarios : Form
    {
        Rules logica = new Rules();
        List<Usuario> users;
        string buscar = String.Empty;
        Usuario usuario;


        public Usuarios(Usuario user)
        {
            InitializeComponent();
            cargarUsuarios();
            usuario = user;
        }

        public void cargarUsuarios()
        {
            users = new List<Usuario>();
            users = logica.getUsers();
            dataGridView1.DataSource = users;
        }

        public void cargarUsuarios(string buscar)
        {
            users = new List<Usuario>();
            users = logica.getUsers(buscar);
            dataGridView1.DataSource = users;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buscar = textBox1.Text;
            if (!String.IsNullOrEmpty(buscar))
            {
                cargarUsuarios(buscar);
            }
            else
            {
                MessageBox.Show("Ingrese el nombre a buscar");
            }
            
            //if (userResult.Count != 0)
            //{
            //    dataGridView1.DataSource = userResult;
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AltaHuella alta = new AltaHuella(usuario.idUsuario.ToString());
            alta.MdiParent = this.MdiParent;
            alta.WindowState = FormWindowState.Maximized;
            alta.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);

            if (selectedRowCount > 0)
            {
                for (int i = 0; i < selectedRowCount; i++)
                {
                    string id = dataGridView1.SelectedRows[i].Cells["idUsuario"].Value.ToString();

                    if (logica.eliminarUsuario(id))
                    {
                        MessageBox.Show("Usuario eliminado");
                        cargarUsuarios();
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Seleccione un usuario");
            }
           
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);

            if (selectedRowCount > 0)
            {
                for (int i = 0; i < selectedRowCount; i++)
                {
                    int id = (int)dataGridView1.SelectedRows[i].Cells["idUsuario"].Value;

                    foreach (Usuario usr in users)
                    {
                        if (usr.idUsuario == id)
                        {
                            AltaHuella alta = new AltaHuella(usr);
                            alta.MdiParent = this.MdiParent;
                            alta.WindowState = FormWindowState.Maximized;
                            alta.Show();            
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("Seleccione un usuario");
            }


            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cargarUsuarios();
        }

        
    }
}
