using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OLC1_Proyecto_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            richTextBox1.Name = "rtb";
        }
        //VARIABLES GLOBALES
        static int contPes = 1;
        LinkedList<Token> ListaTokens;
        LinkedList<Token> ListaErrores;
        Analizador_Lexico Hola;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void nuevaPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contPes++;
            TabPage newPestaña = new TabPage("Pestaña " + contPes);
            RichTextBox textBox = new RichTextBox();
            textBox.Width = 600;
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
            //pictureBox1.Image = null;
            //pictureBox2.Image = null;

            //LEE LA PESTAÑA SELECCIONADA
            TabPage selectedTab = editor.SelectedTab;
            if (selectedTab.Controls.ContainsKey("rtb"))
            {
                RichTextBox rtb = (RichTextBox)selectedTab.Controls["rtb"];
                string text = rtb.Text;

                Hola = new Analizador_Lexico();
                Hola.separarLineas(text);//mandar a analizar
                ListaTokens = Hola.getlistaTokens();
                ListaErrores = Hola.getlistaErrores();
            }
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
            if (ListaTokens.Count == 0)
            {
                Hola.GenerarXML(ListaTokens);
            }
            else
            {
                MessageBox.Show("Errores Lexicos encontrados");
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
            string hola = "Nombre del Estudiante: Juan Marcos Ibarra \n\n Carnet: 201801345 \n\n Curso: Compiladores 1";
            MessageBox.Show(hola);
        }
    }
}
