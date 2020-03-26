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
        EstadoAFD estVacio = new EstadoAFD();
        List<Mueve> mueveSinVacio;
        Dictionary<string, Conjunto> listaConjuntos;
        public static int nombreEstadoAFD = 0;
        public AFD() { }
        public AFD(AFN automata, List<string>listaTerminales, Dictionary<string, Conjunto> listaConjuntos) 
        {
            automataND = automata;
            this.listaTerminales = listaTerminales;
            this.listaConjuntos = listaConjuntos;
        }
        private void DefinirMueves() { }
        public void CrearAFD() 
        {
            estVacio.SetNombreEstadoAFD(-1);
            nombreEstadoAFD = 0;
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
                    mueve = new Mueve(temp,terminal);
                    bool entrar = true;
                    EstadoAFD estadoAFD_nuevo = Cerradura(estados);
                    Console.WriteLine("estados mueves");
                    Console.WriteLine("ESTADO " + estadoAFD_nuevo.GetNombreEstadoAFD());
                    string hola = "";
                    foreach (var item in estadoAFD_nuevo.GetEstadosAlcanzados())
                    {
                        hola += item.GetNoEstado().ToString() + ", ";
                    }
                    Console.WriteLine(hola);
                    foreach (var estado in estados)
                    {
                        entrar = true;
                        
                        Console.WriteLine("estado act: " + temp.GetNombreEstadoAFD() + " estAFD: " +estadoAFD_nuevo.GetNombreEstadoAFD() + " simbolo: " + terminal);
                        for (int i = 0; i < listaEstadosAFD.Count; i++)
                        {
                            
                            if (CompararEstadosAFD(listaEstadosAFD.ElementAt(i).GetEstadosAlcanzados(), estadoAFD_nuevo.GetEstadosAlcanzados()) == false)
                            {
                                entrar = false;
                                mueve.SetLlegada(listaEstadosAFD.ElementAt(i));
                                mueves.Add(mueve);
                                nombreEstadoAFD = listaGeneralEstados.Last().GetNombreEstadoAFD() + 1;
                                break;
                            }
                        }
                        if (entrar)
                        {
                            //llegada
                            mueve.SetLlegada(estadoAFD_nuevo);
                            mueves.Add(mueve);
                            listaGeneralEstados.Add(estadoAFD_nuevo);
                            colaEstadosAFD.Enqueue(estadoAFD_nuevo);
                            listaEstadosAFD.Add(estadoAFD_nuevo);
                        }
                    }
                    //if (agregar)
                    //{
                    //    char a = estadoSal;
                    //    mueve.SetLlegada(a.ToString());
                    //    mueves.Add(mueve);
                    //    agregar = false;
                    //}
                    if (estados.Count == 0)
                    {
                        mueve.SetLlegada(estVacio);
                        mueves.Add(mueve);
                        nombreEstadoAFD = listaGeneralEstados.Last().GetNombreEstadoAFD() + 1;
                    }
                    //else if (entrar == false)
                    //{
                    //    nombreEstadoAFD--;
                    //}
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
                Console.WriteLine("inicio: " + item.GetInicialString() + " llegada: " + item.GetLlegadaString() + " simbolo: " + item.GetSimbolo());
            }
            //GenerarGraphivizAFDTabla();
            //GenerarGraphvizAFD();
        }
        private bool CompararEstadosAFD(List<Estado>listaA, List<Estado>listaB) 
        {
            List<Estado> excepciones = listaB.Except(listaA).ToList();
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
            string text = "A -> 0;";
            List<Mueve> listaMueves = new List<Mueve>();
            for (int i = 0; i < mueves.Count; i++)
            {
                Mueve mueve = mueves.ElementAt(i);

                
                if (!mueve.GetLlegadaString().Equals("-1") && !text.Contains(mueve.GetInicialString() + " -> " + mueve.GetLlegadaString() + "[label=\"" + mueve.GetSimbolo() + "\"]"))
                {
                    text += mueve.GetInicialString() + " -> " + mueve.GetLlegadaString() + "[label=\"" + mueve.GetSimbolo() + "\"]";
                    listaMueves.Add(mueve);
                }
            }
            mueveSinVacio = listaMueves;
            string formas = "A[shape = point]";
            foreach (var item in mueves)
            {
                if (item.GetEstadoInicial().GetNombreEstadoAFD() != -1)
                {
                    if (!formas.Contains(item.GetInicialString() + "[shape = doublecircle];"))
                    {
                        if (item.GetEstadoInicial().GetTipo() == EstadoAFD.Tipo.FINAL)
                        {
                            formas += item.GetInicialString() + "[shape = doublecircle];";
                        }
                    }
                }
            }
            text += formas;
           
            Graficador graficador = new Graficador();
            return graficador.GraphvizAFD(text);
        }
        public string GenerarGraphivizAFDTabla()
        {
            string text = "node[ shape = none, fontname = \"Arial\" ];\n";
            text += "set1[ label=<" + "<TABLE BORDER=\"0\" CELLBORDER=\"1\" CELLSPACING=\"0\" CELLPADDING=\"4\">\n";
            text += " <TR>\n<TD> Estado </TD>";
            List<Mueve> sinRepeticion = new List<Mueve>();

            foreach (var simbolo in listaTerminales)
            {
                text += "<TD>" + simbolo + "</TD>"; 
            }
            text += "</TR>\n";
            bool entrada;
            for (int i = 0; i < mueves.Count-1; i++)
            {
                string estadoAct = mueves.ElementAt(i).GetInicialString();
                //Estado
                if (!text.Contains("<TR>\n<TD>" + estadoAct + "</TD>"))
                {
                    sinRepeticion.Add(mueves.ElementAt(i));
                    text += "<TR>\n<TD>" + estadoAct + "</TD>";
                    entrada = false;
                    for (int j = 0; j < mueves.Count; j++)
                    {
                        if (estadoAct.Equals(mueves.ElementAt(j).GetInicialString()))
                        {
                            if (mueves.ElementAt(j).GetLlegadaString().Equals("-1"))
                            {
                                text += "<TD> - </TD>";
                            }
                            else
                            {
                                text += "<TD>" + mueves.ElementAt(j).GetLlegadaString() + "</TD>";
                            }

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
                else
                {
                    break;
                }
                
            }
            text += "</TABLE>>];";
            mueves = sinRepeticion;
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
            
            EstadoAFD estadoAFD = new EstadoAFD(nombreEstadoAFD++,numeroEstados,listaEstados);
            if (listaEstados.Contains(automataND.GetEstadoFinal()))
            {
                estadoAFD.SetTipoEstado(EstadoAFD.Tipo.FINAL);
            }
            else
            {
                estadoAFD.SetTipoEstado(EstadoAFD.Tipo.NORMAL);
            }
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

        public string validarCadena(Expresiones_Regulares expresionEv) 
        {
            int estadoActual = 0;
            bool valido = false;
            List<int> estadosFinales = new List<int>();
            foreach (var item in mueves)
            {
                EstadoAFD estado = item.GetEstadoInicial();
                if (!estadosFinales.Contains(estado.GetNombreEstadoAFD()))
                {
                    if (estado.GetTipo() == EstadoAFD.Tipo.FINAL )
                    {
                        estadosFinales.Add(estado.GetNombreEstadoAFD());
                    }
                }
                
            }
            string impresion = "";
           // Mueve mueveActual = mueveSinVacio.First();
            string evaluar = expresionEv.GetTokens().Last().GetValorToken();//como es una cadena solo tiene uno

            List<Mueve> listaTemp = new List<Mueve>();
            for (int i = 0; i < evaluar.Length; i++)//evaluando letra por letra
            {
                listaTemp.Clear();
                char letra = evaluar[i];
                valido = false; //reiniciando variable

                foreach (var mov in mueveSinVacio)
                {
                    if (mov.GetEstadoInicial().GetNombreEstadoAFD() == estadoActual)
                    {
                        listaTemp.Add(mov);
                    }
                }
                //obtenemos solo los mueves que tienen de estado inicial al estado actual
                List<Mueve> listOrdernada = listaTemp.OrderByDescending(o=>o.GetSimbolo().Length).ToList();
                Console.WriteLine("LISTA ORDENADA");
                foreach (var item in listOrdernada)
                {
                    Console.WriteLine("estadoInicial " + item.GetInicialString() + "simbolo: " + item.GetSimbolo() + " final: " + item.GetLlegadaString() );
                }
                //los mueves se orden de mayor a menor, segun la longitud de su transicion
                for (int j = 0; j < listOrdernada.Count; j++)
                {
                    Mueve mueve = listOrdernada.ElementAt(j);
                    string simboloTransicion = mueve.GetSimbolo();
                    //bool esConjunto = true;

                    if (simboloTransicion.Length > 1) //cadenas mayores a uno
                    {
                        int longitud = simboloTransicion.Length + i;
                        if (longitud <= evaluar.Length)
                        {
                            longitud -= i;
                            string subCadena = evaluar.Substring(i, longitud);
                            Console.WriteLine("subcadena " + subCadena + " simbolo " + simboloTransicion);
                            if (subCadena.Equals(mueve.GetSimbolo()))
                            {
                                estadoActual = mueve.GetEstadoLlegada().GetNombreEstadoAFD();
                                i += subCadena.Length-1;
                                valido = true;
                                break;
                            }
                        }   
                    }
                    if (listaConjuntos.ContainsKey(simboloTransicion)) //comparando si es un conjunto
                    {
                        Conjunto conjunto = listaConjuntos[simboloTransicion];
                        if (conjunto.GetElementos().Contains(letra.ToString()))//iguala con algun elemento de un conjunto
                        {
                            estadoActual = mueve.GetEstadoLlegada().GetNombreEstadoAFD();
                            valido = true;
                            break;
                        }
                    }
                    else if(simboloTransicion == letra.ToString())
                    {
                        estadoActual = mueve.GetEstadoLlegada().GetNombreEstadoAFD();
                        valido = true;
                        break;
                    }
                   // Console.WriteLine("valida: " + valido);
                }
                Console.WriteLine("valida: " + valido);
                //si sale del segundo for sin encontrar una coincidencia es que no se puede, y la cadena no es valida
                if (valido == false)
                {
                    break; //No vale la pena seguir evaluando la expresion entonces para
                }
            }
            //Al salir la variable estadoActual debe ser un estado final para que la expresion sea valida
            if (valido == false)
            {
                impresion += expresionEv.GetNombre() + ": \"" + evaluar + "\" NO ES VALIDA, para la expresion regular evaluada\n";
            }
            else
            {
                if (estadosFinales.Contains(estadoActual))
                {
                    impresion += expresionEv.GetNombre() + ": \"" + evaluar+ "\" ES VALIDA, para la expresion regular evaluada\n";
                }
                else
                {
                    impresion += expresionEv.GetNombre() + ": \"" + evaluar + "\" NO ES VALIDA, para la expresion regular evaluada\n";
                }
            }
            return impresion;
        }
    }
}
