using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class AFN
    {
        //AUTOMATA FINITO NO DETERMINISTA
        List<Estado> conjuntoEst_Normales = new List<Estado>();
        List<string> listaTerminales = new List<string>();
        Estado Inicial;
        Estado Final;
        Stack<AFN> stackAFN = new Stack<AFN>();


        public AFN() { }
        public List<string> GetListaTerminales() { return listaTerminales; }
        public Estado GetEstadoInicial() { return Inicial; }
        public Estado GetEstadoFinal() { return Final; }
        public List<Estado> GetListaEstados() { return conjuntoEst_Normales; }
        public void GenerarAFN(Expresiones_Regulares expresion)
        {
            //AFN inicial = Operando("epsilon");
            for (int i = expresion.GetTokens().Count - 1; i >= 0; i--)
            {
                GenerarThompson(expresion.GetTokens().ElementAt(i));
            }
            //ORDENANDO LOS ESTADOS
            AFN final = stackAFN.Peek();
            int contadorEst = 1;
            //final = Concatenacion(inicial, final);
            //stackAFN.Push(final);
            final.Inicial.SetNumeroEstado(0);
            foreach (var item in final.GetListaEstados())
            {
                item.SetNumeroEstado(contadorEst);
                contadorEst++;
            }
            final.Final.SetNumeroEstado(contadorEst);
            foreach (var token in expresion.GetTokens())
            {
                switch (token.GetTipoToken())
                {
                    case Token.Tipo.PUNTO: //CONCATENACION
                        {
                            break;
                        }
                    case Token.Tipo.PALITO_OR:  //ALTERNANCIA
                        {
                            break;
                        }
                    case Token.Tipo.ASTERISCO: //CERRADURA DE KLEEN
                        {
                            break;
                        }
                    case Token.Tipo.OP_SUMA: //CERRADURA POSITIVA
                        {
                            break;
                        }
                    case Token.Tipo.PREGUNTA_CIERRE: //APARICION
                        {
                            break;
                        }
                    default:
                        { //operando
                            if (!final.listaTerminales.Contains(token.GetValorToken()))
                            {
                                final.listaTerminales.Add(token.GetValorToken());
                            }
                            break;
                        }
                }
            }
        }
        private void GenerarThompson(Token token)
        {
            switch (token.GetTipoToken())
            {
                case Token.Tipo.PUNTO: //CONCATENACION
                    {
                        AFN opIzquierdo = stackAFN.Pop();
                        AFN opDerecho = stackAFN.Pop();
                        AFN concat = Concatenacion(opIzquierdo, opDerecho);
                        stackAFN.Push(concat);
                        break;
                    }
                case Token.Tipo.PALITO_OR:  //ALTERNANCIA
                    {
                        AFN opIzquierdo = stackAFN.Pop();
                        AFN opDerecho = stackAFN.Pop();
                        AFN alternancia = Alternancia(opIzquierdo, opDerecho);
                        stackAFN.Push(alternancia);
                        break;
                    }
                case Token.Tipo.ASTERISCO: //CERRADURA DE KLEEN
                    {

                        AFN kleen = CerraduraKleen(stackAFN.Pop());
                        stackAFN.Push(kleen);
                        break;
                    }
                //case Token.Tipo.OP_SUMA: 
                //    {
                //        AFN concat = new AFN();
                //        concat = stackAFN.Peek();
                //        AFN kleen = CerraduraKleen(stackAFN.Pop());
                //        AFN positiva = Concatenacion(concat,kleen);
                //        stackAFN.Push(positiva);
                //        break;
                //    }
                case Token.Tipo.PREGUNTA_CIERRE: //APARICION
                    {
                        AFN aparicion = Aparicion(stackAFN.Pop());
                        stackAFN.Push(aparicion);
                        break;
                    }
                default:
                    { //operando
                        AFN thompson_simple = Operando(token.GetValorToken());
                        stackAFN.Push(thompson_simple);
                        break;
                    }
            }
        }
        //ESTE SE VUELVE UNA HOJA
        private AFN Operando(string simbolo) // S1 --- transicion(simbolo) --- >> S2  
        {
            //ELEMENTOS DEL AFN
            Estado estado1 = new Estado(0);
            Estado estado2 = new Estado(1);
            Transicion transicion = new Transicion(estado1, estado2, simbolo);
            //LLENANDO LISTA DE TERMINALES

            estado1.SetTransicionIzq(transicion); //el primero debe saber a donde jodidos va
            //CREANDO EL AFN
            AFN operando = new AFN();
            operando.Inicial = estado1;
            operando.Final = estado2;
            //operando.conjuntoEst_Aceptacion.Add(estado2);

            return operando;
        }
        private AFN Concatenacion(AFN operando1, AFN operando2) // S1 ---tran1_2--->> S2 ---tran2_3--->> S3
        {

            //MODIFICAR TRANSICION: LOS QUE APUNTAN A ULTIMO YA NO LO HACEN
            Transicion cambioIzq = operando1.GetEstadoInicial().GetTransicionIzq();
            Transicion cambioDer = operando1.GetEstadoInicial().GetTransicionDer();
            if (cambioIzq != null && cambioIzq.GetSegundo() == operando1.GetEstadoFinal())
            {
                cambioIzq.SetSegundoEstado(operando2.GetEstadoInicial());
            }
            if (cambioDer != null && cambioDer.GetSegundo() == operando1.GetEstadoFinal())
            {
                cambioDer.SetSegundoEstado(operando2.GetEstadoInicial());
            }
            foreach (var estado in operando1.GetListaEstados())
            {
                cambioIzq = estado.GetTransicionIzq();
                cambioDer = estado.GetTransicionDer();
                if (cambioIzq != null && cambioIzq.GetSegundo() == operando1.GetEstadoFinal())
                {
                    cambioIzq.SetSegundoEstado(operando2.GetEstadoInicial());
                }
                if (cambioDer != null && cambioDer.GetSegundo() == operando1.GetEstadoFinal())
                {
                    cambioDer.SetSegundoEstado(operando2.GetEstadoInicial());
                }
            }
            //LO UNICO QUE HAY QUE HACER ES UNIR EL ULTIMO DEL PRIMERO CON EL PRIMERO DEL ULTIMO
            operando1.Final = operando2.Inicial;
            AFN concatenacion = new AFN();
            concatenacion.Inicial = operando1.Inicial;
            concatenacion.Final = operando2.Final;

            //METIENDO EL RESTO DE ESTADOS AFN NUMERO 1
            foreach (var estado in operando1.GetListaEstados())
            {
                concatenacion.GetListaEstados().Add(estado);
            }
            concatenacion.GetListaEstados().Add(operando1.Final);
            //METIENDO EL RESTO DE ESTADOS AFN NUMERO 2
            foreach (var estado in operando2.GetListaEstados())
            {
                concatenacion.GetListaEstados().Add(estado);
            }
            return concatenacion;
        }
        private AFN Alternancia(AFN operando1, AFN operando2)
        {
            Estado estadoInicial = new Estado(0);
            Transicion Opcion1 = new Transicion(estadoInicial, operando1.Inicial, "epsilon");
            Transicion Opcion2 = new Transicion(estadoInicial, operando2.Inicial, "epsilon");
            //Metiendo las transiciones
            estadoInicial.SetTransicionDer(Opcion2);
            estadoInicial.SetTransicionIzq(Opcion1);
            Estado estadoFinal = new Estado(6); //6 -> si fuera el mas simple existente
            Transicion Opcion1F = new Transicion(operando1.Final, estadoFinal, "epsilon");
            Transicion Opcion2F = new Transicion(operando2.Final, estadoFinal, "epsilon");
            //Metiendo las transiciones
            operando1.Final.SetTransicionIzq(Opcion1F);
            operando2.Final.SetTransicionIzq(Opcion2F);

            AFN alternancia = new AFN();
            alternancia.Inicial = estadoInicial;
            alternancia.Final = estadoFinal;
            alternancia.Meter_EstNormales(operando1);
            alternancia.Meter_EstNormales(operando2);

            return alternancia;
        }
        //OPERADOR UNARIO
        private AFN CerraduraKleen(AFN operando)
        {
            Estado estadoInicial = new Estado(0);
            Estado estadoFinal = new Estado(3); //3 -> si fuera el mas simple existente
            Transicion transicion = new Transicion(estadoInicial, operando.Inicial, "epsilon");
            Transicion inicial_final = new Transicion(estadoInicial, estadoFinal, "epsilon");
            estadoInicial.SetTransicionIzq(transicion);
            estadoInicial.SetTransicionDer(inicial_final);
            //REPETICION
            Transicion repeticion = new Transicion(operando.Final, operando.Inicial, "epsilon");
            //UNIENDO OPERANDO CON EL FINAL
            Transicion unionFinal = new Transicion(operando.Final, estadoFinal, "epsilon");
            operando.Final.SetTransicionIzq(unionFinal);
            operando.Final.SetTransicionDer(repeticion);
            //CREANDO AFN
            AFN cerraduraKleen = new AFN();
            cerraduraKleen.Inicial = estadoInicial;
            cerraduraKleen.Final = estadoFinal;
            cerraduraKleen.Meter_EstNormales(operando);

            return cerraduraKleen;
        }
        private AFN CerraduraPositiva(AFN operando)
        {
            //Operando(operando.);
            //CerraduraKleen(operando);
            Estado estadoInicial = new Estado(0);
            Estado estadoFinal = new Estado(3); //3 -> si fuera el mas simple existente
            Transicion transicion = new Transicion(estadoInicial, operando.Inicial, "epsilon");
            estadoInicial.SetTransicionIzq(transicion);
            //REPETICION
            Transicion repeticion = new Transicion(operando.Final, operando.Inicial, "epsilon");
            //UNIENDO OPERANDO CON EL FINAL
            Transicion unionFinal = new Transicion(operando.Final, estadoFinal, "epsilon");
            operando.Final.SetTransicionIzq(unionFinal);
            operando.Final.SetTransicionDer(repeticion);
            //CREANDO AFN
            AFN cerraduraPositiva = new AFN();
            cerraduraPositiva.Inicial = estadoInicial;
            cerraduraPositiva.Final = estadoFinal;
            cerraduraPositiva.Meter_EstNormales(operando);

            return cerraduraPositiva;
        }
        private AFN Aparicion(AFN operando)
        {
            Estado estadoInicial = new Estado(0);
            Estado estadoFinal = new Estado(3); //3 -> si fuera el mas simple existente
            Transicion transicion = new Transicion(estadoInicial, operando.Inicial, "epsilon");
            //QUE NO APAREZCA NADA
            Transicion inicial_final = new Transicion(estadoInicial, estadoFinal, "epsilon");
            estadoInicial.SetTransicionIzq(transicion);
            estadoInicial.SetTransicionDer(inicial_final);
            //UNIENDO OPERANDO CON EL FINAL
            Transicion unionFinal = new Transicion(operando.Final, estadoFinal, "epsilon");
            operando.Final.SetTransicionIzq(unionFinal);
            //CREANDO AFN
            AFN aparicion = new AFN();
            aparicion.Inicial = estadoInicial;
            aparicion.Final = estadoFinal;
            aparicion.Meter_EstNormales(operando);

            return aparicion;
        }
        private void Meter_EstNormales(AFN estados)
        { //METE TODOS LOS ESTADOS QUE TIENE UN AFN A LOS ESTADOS NORMALES DE OTRO AFN
            int contador = 2;
            Inicial.SetNumeroEstado(0);
            estados.Inicial.SetNumeroEstado(1);
            conjuntoEst_Normales.Add(estados.Inicial);
            foreach (var item in estados.conjuntoEst_Normales)
            {
                item.SetNumeroEstado(contador);
                contador++;
                conjuntoEst_Normales.Add(item);
            }
            estados.Final.SetNumeroEstado(contador);
            conjuntoEst_Normales.Add(estados.Final);
        }
        public string GenerarGraphviz()
        {
            Graficador graficador = new Graficador();
            return graficador.GraphvizAFN(stackAFN);
        }
        public string[] CrearAFD()
        {
            string[] hola = new string[2];
            AFN automataAF = stackAFN.Peek();
            AFD automata = new AFD(automataAF, automataAF.GetListaTerminales());
            automata.CrearAFD();
            hola[0] = automata.GenerarGraphvizAFD();
            hola[1] = automata.GenerarGraphivizAFDTabla();
            return hola;
        }

    }
}
