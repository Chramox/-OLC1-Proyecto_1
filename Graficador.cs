using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class Graficador
    {
        //String ruta;
        StringBuilder grafo;
        String ruta1;
        String text = "";
        String ruta;
        public Graficador()
        {
            ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        private void generardot(String rdot, String rpng)
        {
            File.WriteAllText(rdot, grafo.ToString());
            String comandoDot = "dot -Tpng " + rdot + " -o " + rpng;
            Console.WriteLine(comandoDot);
            var comando = string.Format(comandoDot);
            var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/C" + comando);
            var procedimiento = new System.Diagnostics.Process();

            procedimiento.StartInfo = procStart;
            procedimiento.Start();
            procedimiento.WaitForExit();

        }
        public string GraphvizAFN(Stack<AFN> stackAFN)
        {
            
            AFN Thompson = stackAFN.Peek();
            EnOrder(Thompson.GetEstadoInicial().GetTransicionIzq());
            EnOrder(Thompson.GetEstadoInicial().GetTransicionDer());
            //  EnOrderDer(Thompson.GetEstadoInicial().GetTransicionIzq());
            return Graficar(text);
        }
        public string GraphvizAFDTabla(string text) 
        {
            return GraficarAFDTabla(text);
        }
        public string GraphvizAFD(string text) 
        {
            return GraficarAFD(text);
        }
        private string GraficarAFD(string text) 
        {
            grafo = new StringBuilder();
            String rdot = ruta + "\\GrafoImagenAFD.dot";
            String rpng = ruta + "\\GrafoImagenAFD.png";
            Form1.rutaAFD = rpng;
            grafo.Append("digraph G{ \nrankdir = LR\nnode[shape = circle];");
            grafo.Append(text);
            grafo.Append("}");
            ruta1 = ruta + "\\GrafoImagenAFD.png";

            generardot(rdot, rpng);
            return rpng;
        }
        private string GraficarAFDTabla(String text)//falta definir parametros
        {
            grafo = new StringBuilder();
            String rdot = ruta + "\\GrafoImagenAFDTabla.dot";
            String rpng = ruta + "\\GrafoImagenAFDTabla.png";
            Form1.rutaAFD_Tabla = rpng;
            grafo.Append("digraph G  {");
            grafo.Append(text);
            grafo.Append("}");
            ruta1 = ruta + "\\GrafoImagenAFNTabla.png";

            generardot(rdot, rpng);
            return rpng;
        }
        private string Graficar(String text)//falta definir parametros
        {
            grafo = new StringBuilder();
            String rdot = "GrafoImagenAFN.dot";
            String rpng = "GrafoImagenAFN.png";
            Form1.rutaAFN = rpng;
            grafo.Append("digraph G{ \nrankdir = LR\nnode[shape = circle];");
            grafo.Append(text);
            grafo.Append("}");
            ruta1 = "GrafoImagenAFN.png";

            generardot(rdot, rpng);
            return rpng;
        }
        
        public void EnOrder(Transicion transicion)
        {
           bool repeticion = false;
            string textoAlt;
            if (transicion!=null)
            {
                textoAlt = transicion.GetPrimero().GetNoEstado() + " -> " + transicion.GetSegundo().GetNoEstado() + "[label=\"" + transicion.GetTransicionSimbolo() + "\"]";
                
                if (text.Contains(textoAlt))
                {
                    repeticion = true;
                }
                else
                {
                    text += textoAlt;
                }
                EnOrder(transicion.GetSegundo().GetTransicionIzq());
                if (!repeticion)
                {
                    EnOrder(transicion.GetSegundo().GetTransicionDer());
                }
            }
        }
        public void EnOrderDer(Transicion transicion)
        {
            Transicion transicionDerecha, transicion1;
            string textDerecha;
            while (transicion != null)
            {
                transicionDerecha = transicion.GetPrimero().GetTransicionDer();
                if (transicionDerecha!=null)
                {
                    transicion1 = transicionDerecha.GetSegundo().GetTransicionIzq();
                    text += transicionDerecha.GetPrimero().GetNoEstado() + " -> " + transicionDerecha.GetSegundo().GetNoEstado() + "[label=\"" + transicionDerecha.GetTransicionSimbolo() + "\"]";
                    while (transicion1 != null)
                    {
                        textDerecha = transicion1.GetPrimero().GetNoEstado() + " -> " + transicion1.GetSegundo().GetNoEstado() + "[label=\"" + transicion1.GetTransicionSimbolo() + "\"]";
                        if (text.Contains(textDerecha))
                        {
                            break;
                        }
                        else
                        {
                            text += textDerecha;
                        }
                        transicion1 = transicion1.GetSegundo().GetTransicionIzq();
                    }
                }
                transicion = transicion.GetSegundo().GetTransicionIzq();
            }
        }

        public string getRutaIMG()
        {
            return ruta1;
        }
    }
}
