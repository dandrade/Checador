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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;

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

            List<Reporte> reportes = logica.getReporte(this.fechaInicial.Text, this.fechaFinal.Text, usuarios);

            crearPDF(reportes);
            
        }

        private void crearPDF(List<Reporte> reportes)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string pdfPath = config.AppSettings.Settings["PdfPath"].Value.ToString();
            
            Document documento = new Document();
            string nombreArchivo = @pdfPath + "Reporte_" + DateTime.Now.Ticks.ToString() + ".pdf";
            PdfWriter.GetInstance(documento, new System.IO.FileStream(nombreArchivo, System.IO.FileMode.Create));

            documento.Open();

            Phrase salto = new Phrase("\n");



            PdfPTable tabla = new PdfPTable(5);
            Paragraph titulo = new Paragraph("Reporte de Entradas y Salidas del " + this.fechaInicial.Text + " al " + this.fechaFinal.Text);
            titulo.Alignment = 1;

            documento.Add(titulo);
            documento.Add(salto);

            PdfPCell nombreCell = new PdfPCell(new Phrase("Empleado"));
            nombreCell.HorizontalAlignment = 1;
            PdfPCell diaCell = new PdfPCell(new Phrase("Dia"));
            diaCell.HorizontalAlignment = 1;
            PdfPCell entradaCell = new PdfPCell(new Phrase("Entrada"));
            entradaCell.HorizontalAlignment = 1;
            PdfPCell salidaCell = new PdfPCell(new Phrase("Salida"));
            salidaCell.HorizontalAlignment = 1;
            PdfPCell totalCell = new PdfPCell(new Phrase("Total HH"));
            totalCell.HorizontalAlignment = 1;

            //tabla.AddCell(header);
            tabla.AddCell(nombreCell);
            tabla.AddCell(diaCell);
            tabla.AddCell(entradaCell);
            tabla.AddCell(salidaCell);
            tabla.AddCell(totalCell);

            tabla.TotalWidth = documento.PageSize.Width - documento.LeftMargin - documento.RightMargin;

            foreach (Reporte reporte in reportes)
            {
                PdfPCell nombreCellB = new PdfPCell(new Phrase(reporte.NombreCompleto, FontFactory.GetFont("Arial", 10)));
                nombreCellB.HorizontalAlignment = 1;
                PdfPCell diaCellB = new PdfPCell(new Phrase(reporte.Dia, FontFactory.GetFont("Arial", 10)));
                diaCellB.HorizontalAlignment = 1;
                PdfPCell entradaCellB = new PdfPCell(new Phrase(reporte.Entrada, FontFactory.GetFont("Arial", 10)));
                entradaCellB.HorizontalAlignment = 1;
                PdfPCell salidaCellB = new PdfPCell(new Phrase(reporte.Salida, FontFactory.GetFont("Arial", 10)));
                salidaCellB.HorizontalAlignment = 1;
                PdfPCell totalCellB = new PdfPCell(new Phrase(reporte.TotalHoras, FontFactory.GetFont("Arial", 10)));
                totalCellB.HorizontalAlignment = 1;

                tabla.AddCell(nombreCellB);
                tabla.AddCell(diaCellB);
                tabla.AddCell(entradaCellB);
                tabla.AddCell(salidaCellB);
                tabla.AddCell(totalCellB);
            }


            documento.Add(tabla);

            documento.Close();

            System.Diagnostics.Process.Start(nombreArchivo);
        }
    }
}
