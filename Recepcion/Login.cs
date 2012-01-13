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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            if (String.IsNullOrEmpty(textBox1.Text)) { errorProvider1.SetError(textBox1, "Nombre de Usuario no puede estar vacio"); } else { errorProvider1.SetError(textBox1, ""); }
            if (String.IsNullOrEmpty(textBox2.Text)) { errorProvider1.SetError(textBox2, "Contraseña no puede estar vacio"); } else { errorProvider1.SetError(textBox2, ""); }

            if (!String.IsNullOrEmpty(textBox1.Text) && !String.IsNullOrEmpty(textBox2.Text))
            {
                Rules logica = new Rules();;
                
                Usuario user = new Usuario();
                
                user = logica.validateUser(textBox1.Text, textBox2.Text);

                if (user.Logged)
                {
                    switch (user.RolUser)
                    {
                        case "Administrador":
                            Administracion admin = new Administracion(user);
                            admin.WindowState = FormWindowState.Maximized;
                            this.Hide();
                            admin.ShowDialog();
                            this.Close();
                            break;
                        default:
                            MessageBox.Show("Usted no esta autorizado");
                            break;
                    }
                }
                else
                { 
                    label3.Text = "Nombre de Usuario/Contraseña Incorrectos";
                }
            }
        }
    }
}
