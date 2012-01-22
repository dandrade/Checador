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
    public partial class Administracion : Form
    {
        Usuario usuario;
        public Administracion(Usuario user)
        {
            InitializeComponent();
            usuario = user;
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Usuarios usuarios = new Usuarios(usuario);
            usuarios.MdiParent = this;
            usuarios.WindowState = FormWindowState.Maximized;
            usuarios.Show();
        }

        private void cascadaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void organizarIconosToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void Administracion_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void Administracion_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reportes reportes = new Reportes();
            reportes.MdiParent = this;
            reportes.WindowState = FormWindowState.Maximized;
            reportes.Show();
        }

        private void Administracion_Load(object sender, EventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.MdiParent = this;
            inicio.WindowState = FormWindowState.Maximized;
            inicio.Show();
        }
    }
}
