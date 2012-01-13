using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Recepcion.Logic;
using Recepcion.Common;

namespace Recepcion
{
    public partial class Reportes : Form
    {

        Rules logica = new Rules();
        List<Usuario> users;
        Usuario usuario;

        public Reportes()
        {
            InitializeComponent();
            this.cargarUsuarios();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        public void cargarUsuarios()
        {
            users = new List<Usuario>();
            users = logica.getUsers();
            dataGridView1.DataSource = users;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string fechaInicial = this.fechaInicial.Text;
            string fechaFinal = this.fechaFinal.Text;

            List<int> usuarios = new List<int>();
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                try
                {
                    bool isSelected = (bool)item.Cells["Seleccionar"].Value;
                    if (isSelected)
                    {
                        int idUsuario = (int)item.Cells["idUsuario"].Value;

                        usuarios.Add(idUsuario);
                    }
                }
                catch (Exception ex)
                {
                }
            }

            if (usuarios.Count > 0)
            {

            }
            else
            { 
            
            }
        }
    }
}
