using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Recepcion
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Usuarios usuarios = new Usuarios();
            usuarios.MdiParent = this.MdiParent;
            usuarios.WindowState = FormWindowState.Maximized;
            usuarios.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reportes reportes = new Reportes();
            reportes.MdiParent = this.MdiParent;
            reportes.WindowState = FormWindowState.Maximized;
            reportes.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
