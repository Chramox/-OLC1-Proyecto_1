using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Build.BuildEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Image = System.Drawing.Image;

namespace _OLC1_Proyecto_1
{
    public partial class Form1 : Form
    {
        public static int contadorIMG = 0;
        public static string rutaAFN = "";
        public static string rutaAFD = "";
        //public static string rutaAFN_Tabla = "";
        public static string rutaAFD_Tabla = "";
        public static string textConsola = "";
        //public static List<Expresiones_Regulares> cadenasEvaluar;
        List<string> listaAutomatas = new List<string>();
        //List<string> listaAFD = new List<string>();
        List<string> listaTablas = new List<string>();
        public Form1()
        {
            InitializeComponent();
            richTextBox1.Name = "rtb";
        }
        //VARIABLES GLOBALES
        static int contPes = 1;
        int contador = 0;
        int contadorTablas = 0;
        LinkedList<Token> ListaTokens;
        LinkedList<Token> ListaErrores;
        Analizador_Lexico Analizador;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void nuevaPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contPes++;
            TabPage newPestaña = new TabPage("Pestaña " + contPes);
            RichTextBox textBox = new RichTextBox();
            textBox.Width = 688;
            textBox.Height = 574;
            newPestaña.Controls.Add(textBox);
            textBox.Name = "rtb";
            editor.TabPages.Add(newPestaña);
        }

        private void cargarTomhsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListaTokens = new LinkedList<Token>();
            ListaErrores = new LinkedList<Token>();
            //FALTA AGREGAR LAS IMAGENES
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            textConsola = "";
            richTextBox2.Text = textConsola;
            //LEE LA PESTAÑA SELECCIONADA
            TabPage selectedTab = editor.SelectedTab;
            if (selectedTab.Controls.ContainsKey("rtb"))
            {
                RichTextBox rtb = (RichTextBox)selectedTab.Controls["rtb"];
                string text = rtb.Text;

                Analizador = new Analizador_Lexico();
                Analizador.separarLineas(text);//mandar a analizar
                ListaTokens = Analizador.getlistaTokens();
                ListaErrores = Analizador.getlistaErrores();

                Console.WriteLine("PRUEBA THOMPSON------------------------------------------------------------");
                AFN Thompson = new AFN(Analizador.GetCadenasEv(), Analizador.GetConjuntos());
                foreach (var item in Analizador.GetListaER())
                {
                    Expresiones_Regulares expresion = item.Value;

                    Thompson.GenerarAFN(expresion);
                    rutaAFN = Thompson.GenerarGraphviz();
                    if (System.IO.File.Exists(rutaAFN))
                    {
                        listaAutomatas.Add(rutaAFN);
                        //Image image = Image.FromFile(rutaAFN);
                        //pictureBox1.Image = image;
                    }
                    //GENERANDO AFD APARTIR DE UN AFN
                    string[] imagenesAFD = Thompson.CrearAFD(); //posicion 0 = afd; posicion 1 = tabla afd
                    rutaAFD_Tabla = imagenesAFD[1];
                    if (System.IO.File.Exists(rutaAFD_Tabla))
                    {
                        listaTablas.Add(rutaAFD_Tabla);
                        System.Drawing.Image image = System.Drawing.Image.FromFile(rutaAFD_Tabla);
                        pictureBox2.Image = image;
                    }
                    if (System.IO.File.Exists(imagenesAFD[0]))
                    {
                        listaAutomatas.Add(imagenesAFD[0]);
                        System.Drawing.Image image = System.Drawing.Image.FromFile(imagenesAFD[0]);
                        pictureBox1.Image = image;
                    }
                    contadorIMG++;
                }
            }
            richTextBox2.Text = textConsola;
            contador = listaAutomatas.Count - 1;
            contadorTablas = listaTablas.Count - 1;
        }

        private void abrirArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                TabPage selectedTab = editor.SelectedTab;
                //AQUI ABRIR Y LEER EL ARCHIVO
                if (selectedTab.Controls.ContainsKey("rtb")) {
                    RichTextBox box = (RichTextBox)selectedTab.Controls["rtb"];
                    box.AppendText("\t+\t");
                    string text = System.IO.File.ReadAllText(openFileDialog1.FileName);
                    box.Text = text;
                    editor.SelectedTab.Text = openFileDialog1.SafeFileName;
                }   
            }
        }
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            if (ListaTokens.Count != 0)
            {
                Analizador.GenerarXML(ListaTokens);
            }
            else
            {
                MessageBox.Show("Lista Vacia");
            }
        }

        private void guardarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                TabPage selectedTab = editor.SelectedTab;

                if (selectedTab.Controls.ContainsKey("rtb"))
                {
                    RichTextBox rtb = (RichTextBox)selectedTab.Controls["rtb"];
                    string text = rtb.Text;

                    // WriteAllText creates a file, writes the specified string to the file,
                    // and then closes the file.    You do NOT need to call Flush() or Close().
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, text);
                }

            }
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Analizador = "Nombre del Estudiante: Juan Marcos Ibarra \n\n Carnet: 201801345 \n\n Curso: Compiladores 1";
            MessageBox.Show(Analizador);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            contador--;
            if (contador > -1)
            {
                string imagen = listaAutomatas.ElementAt(contador);
                if (System.IO.File.Exists(imagen))
                {
                    //listaAutomatas.Add(imagen);
                    System.Drawing.Image image = System.Drawing.Image.FromFile(imagen);
                    pictureBox1.Image = image;
                }
            }
            else
            {
                contador = 0;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void btn_derecha_Click(object sender, EventArgs e)
        {
            contador++;
            if (contador < listaAutomatas.Count-1)
            {
                string imagen = listaAutomatas.ElementAt(contador);
                if (System.IO.File.Exists(imagen))
                {
                    //listaAutomatas.Add(imagen);
                    Image image = Image.FromFile(imagen);
                    pictureBox1.Image = image;
                }
            }
            else
            {
                contador = listaAutomatas.Count -1;
            }
        }

        private void reporteDeErroresLexicosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(@"Reporte.pdf"))
                {
                    File.Delete(@"Reporte.pdf");
                }
                Document doc = new Document(PageSize.LETTER, 10, 10, 42, 35);
                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("Reporte.pdf", FileMode.Create));
                doc.Open();


                PdfPTable table = new PdfPTable(3);

                PdfPCell cell = new PdfPCell(new Phrase("Header spanning 3 columns"));
                cell.Colspan = 3;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
                table.AddCell("VALOR");
                table.AddCell("COLUMNA");
                table.AddCell("FILA");
               
                foreach (var error in ListaErrores)
                {
                    table.AddCell(error.GetValorToken());
                    table.AddCell(error.GetColumna().ToString());
                    table.AddCell(error.GetFila().ToString());
                }

                doc.Add(new Paragraph("\n\n\n\n"));

                if (ListaErrores.Count == 0)
                {
                    doc.Add(new Paragraph("\nNO HAY ERRORES LEXICOS\n"));
                }
                else
                {
                    doc.Add(table);
                }
                doc.Close();


             //   MessageBox.Show("PDF creado con éxito");
                System.Diagnostics.Process.Start(@"Reporte.pdf");
            }
            catch (Exception exe)
            {

                MessageBox.Show("No se pudo crear el archivo PDF " + exe);
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            contadorTablas--;
            if (contadorTablas > -1)
            {
                string imagen = listaTablas.ElementAt(contadorTablas);
                if (System.IO.File.Exists(imagen))
                {
                   // listaTablas.Add(imagen);
                    System.Drawing.Image image = System.Drawing.Image.FromFile(imagen);
                    pictureBox1.Image = image;
                }
            }
            else
            {
                contadorTablas = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            contadorTablas++;
            if (contadorTablas < listaTablas.Count - 1)
            {
                string imagen = listaTablas.ElementAt(contadorTablas);
                if (System.IO.File.Exists(imagen))
                {
                   // listaTablas.Add(imagen);
                    Image image = Image.FromFile(imagen);
                    pictureBox1.Image = image;
                }
            }
            else
            {
                contadorTablas = listaAutomatas.Count - 1;
            }
        }

        private void guardarErroresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ListaTokens.Count != 0)
            {
                Analizador.GenerarXML_Error(ListaTokens);
            }
            else
            {
                MessageBox.Show("Lista Vacia");
            }
        }
    }
}
