using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class AFD
    {
        
        AFN automataND;
        List<string> listaTerminales;
        List<EstadoAFD> listaGeneralEstados = new List<EstadoAFD>();
        List<Mueve> mueves = new List<Mueve>();
        public static int nombreEstadoAFD = 65;
        public AFD() { }
        public AFD(AFN automata, List<string>listaTerminales) 
        {
            automataND = automata;
            this.listaTerminales = listaTerminales;
        }
        private void DefinirMueves() { }
        public void CrearAFD() 
        {
            nombreEstadoAFD = 65;
            Queue<EstadoAFD> colaEstadosAFD = new Queue<EstadoAFD>();
            List<Estado> estados1 = new List<Estado>();
            estados1.Add(automataND.GetEstadoInicial());
            EstadoAFD cerradura = Cerradura(estados1);
            listaGeneralEstados.Add(cerradura);
          //  List<Estado> listaCerradura = cerradura.GetEstadosAlcanzados();
            List<EstadoAFD> listaEstadosAFD = new List<EstadoAFD>();
            listaEstadosAFD.Add(cerradura);
            colaEstadosAFD.Enqueue(cerradura);
            
            Mueve mueve;
            while (colaEstadosAFD.Count > 0)
            {

                EstadoAFD temp = colaEstadosAFD.Dequeue();
                foreach (var terminal in listaTerminales)
                {
                    List<Estado> estados = Mueve(temp.GetEstadosAlcanzados(),terminal);
                    //ida y simbolo
                    mueve = new Mueve(temp.GetNombreEstadoAFD().ToString(),terminal);
                    bool entrar = true;
                    int estadoSal = 0;
                    bool agregar = false;
                    foreach (var estado in estados)
                    {
                        entrar = true;
                        EstadoAFD estadoAFD_nuevo = Cerradura(estados);
                        agregar = false;
                        for (int i = 0; i < listaEstadosAFD.Count; i++)
                        {
                            
                            if (CompararEstadosAFD(listaEstadosAFD.ElementAt(i).GetEstadosAlcanzados(),estadoAFD_nuevo.GetEstadosAlcanzados()) == false)
                            {
                                entrar = false;
                                agregar = true;
                                mueve.SetLlegada(listaEstadosAFD.ElementAt(i).GetNombreEstadoAFD().ToString());
                                mueves.Add(mueve);
                                break;
                            }
                            else
                            {
                                agregar = true;
                            }
                            estadoSal = i;
                        }
                        estadoSal += 66;
                        if (entrar)
                        {
                            //llegada
                            mueve.SetLlegada(estadoAFD_nuevo.GetNombreEstadoAFD().ToString());
                            mueves.Add(mueve);
                            listaGeneralEstados.Add(estadoAFD_nuevo);
                            colaEstadosAFD.Enqueue(estadoAFD_nuevo);
                            listaEstadosAFD.Add(estadoAFD_nuevo);
                        }

                    }
                    //if (agregar)
                    //{
                    //    char a = (char)estadoSal;
                    //    mueve.SetLlegada(a.ToString());
                    //    mueves.Add(mueve);
                    //    agregar = false;
                    //}
                    if (estados.Count == 0)
                    {
                        mueve.SetLlegada("");
                        mueves.Add(mueve);
                    }
                    //llegada vacia
                }
                
            }
            string estadosS;
            foreach (var item in listaGeneralEstados)
            {
                estadosS = "";
                Console.WriteLine("ESTADO " + item.GetNombreEstadoAFD());
                foreach (var item2 in item.GetEstadosAlcanzados())
                {
                    estadosS += item2.GetNoEstado().ToString() + ", ";
                }
                Console.WriteLine(estadosS);
            }
            foreach (var item in mueves)
            {
                Console.WriteLine("inicio: " + item.GetInicial() + " llegada: " + item.GetLlegada() + " simbolo: " + item.GetSimbolo());
            }
            //GenerarGraphivizAFDTabla();
            //GenerarGraphvizAFD();
        }
        private bool CompararEstadosAFD(List<Estado>lista1, List<Estado>lista2) 
        {
            List<Estado> excepciones = lista1.Except(lista2).ToList();
            if (excepciones.Count > 0)
            {
                return true; //iguales
            }
            else
            {
                return false; //no iguales
            }
        }
        public string GenerarGraphvizAFD() 
        {
            string text = "";
            for (int i = 0; i < mueves.Count; i++)
            {
                Mueve mueve = mueves.ElementAt(i);
                if (!mueve.GetLlegada().Equals(""))
                {
                    text += mueve.GetInicial() + " -> " + mueve.GetLlegada() + "[label=\"" + mueve.GetSimbolo() + "\"]";
                }
            }
            Graficador graficador = new Graficador();
            return graficador.GraphvizAFD(text);
        }
        public string GenerarGraphivizAFDTabla()
        {
            string text = "node[ shape = none, fontname = \"Arial\" ];\n";
            text += "set1[ label=<" + "<TABLE BORDER=\"0\" CELLBORDER=\"1\" CELLSPACING=\"0\" CELLPADDING=\"4\">\n";
            text += " <TR>\n<TD> Estado </TD>";
            
            foreach (var simbolo in listaTerminales)
            {
                text += "<TD>" + simbolo + "</TD>"; 
            }
            text += "</TR>\n";
            bool entrada;
            for (int i = 0; i < mueves.Count-1; i++)
            {
                string estadoAct = mueves.ElementAt(i).GetInicial();
                //Estado
                text += "<TR>\n<TD>" + estadoAct + "</TD>";
                entrada = false;
                for (int j = 0; j < mueves.Count; j++)
                {
                    if (estadoAct.Equals(mueves.ElementAt(j).GetInicial()))
                    {
                        text += "<TD>" + mueves.ElementAt(j).GetLlegada() + "</TD>";
                        entrada = true;
                    }
                    else if (entrada == true)//ya se encontraron todos
                    {
                        i = j - 1;
                        break;
                    }
                }
                text += "</TR>";
            }
            text += "</TABLE>>];";

            Graficador graficador = new Graficador();
            return graficador.GraphvizAFDTabla(text);
        }
        private EstadoAFD Cerradura(List<Estado> estados) 
        {
            Stack<Estado> pilaEstados = new Stack<Estado>();
            Estado actual;
            //estado AFD
            List<Estado> listaEstados = new List<Estado>();
            List<int> numeroEstados = new List<int>();
            foreach (var estado in estados)
            {
                actual = estado;
                pilaEstados.Push(actual);
                numeroEstados.Add(estado.GetNoEstado());

                while (pilaEstados.Count > 0)
                {
                    actual = pilaEstados.Pop();
                    if (actual != null)
                    {
                        Transicion transicion = actual.GetTransicionDer();
                        if (transicion != null &&
                            transicion.GetTransicionSimbolo().Equals("epsilon")
                            && !listaEstados.Contains(transicion.GetSegundo()))
                        {
                            listaEstados.Add(transicion.GetSegundo());
                            numeroEstados.Add(transicion.GetSegundo().GetNoEstado());
                            pilaEstados.Push(transicion.GetSegundo());
                        }
                        transicion = actual.GetTransicionIzq();
                        if (transicion != null &&
                            transicion.GetTransicionSimbolo().Equals("epsilon")
                            && !listaEstados.Contains(transicion.GetSegundo()))
                        {
                            listaEstados.Add(transicion.GetSegundo());
                            numeroEstados.Add(transicion.GetSegundo().GetNoEstado());
                            pilaEstados.Push(transicion.GetSegundo());
                        }
                    }
                }
                listaEstados.Add(estado);
            }
            
            EstadoAFD estadoAFD = new EstadoAFD((char)nombreEstadoAFD++,numeroEstados,listaEstados);
            //Console.WriteLine("PRUEBA ESTADOS POR " + "epsilon");
            //string alv = "";
            //foreach (Estado est in listaEstados)
            //{
            //    alv += est.GetNoEstado();
            //}
            
            return estadoAFD;
            
        }
        //ESTE METODO DEVUELVE UNA LISTA DE ESTADOS QUE HAY QUE MANDAR A CERRADURA
        private List<Estado> Mueve(List<Estado> estados, string terminal) 
        {
            List<Estado> estadosAlcanzados = new List<Estado>();
            foreach (var actual in estados)
            {
                Transicion transicion = actual.GetTransicionDer();
                if (transicion != null &&
                    transicion.GetTransicionSimbolo().Equals(terminal)
                    && !estadosAlcanzados.Contains(transicion.GetSegundo()))
                {
                    estadosAlcanzados.Add(transicion.GetSegundo());
                }
                transicion = actual.GetTransicionIzq();
                if (transicion != null &&
                    transicion.GetTransicionSimbolo().Equals(terminal)
                    && !estadosAlcanzados.Contains(transicion.GetSegundo()))
                {
                    estadosAlcanzados.Add(transicion.GetSegundo());
                }
            }
            return estadosAlcanzados;
        }
        

    }
}
