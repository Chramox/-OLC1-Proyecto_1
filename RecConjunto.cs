using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class RecConjunto
    {
        

        //variables para el reconocimiento
        static int controlToken = 0;
        Token tokenActual;
        LinkedList<Token> listaTokens;
        Dictionary<String,Conjunto> conjuntos = new Dictionary<string, Conjunto>();
        Dictionary<String,Expresiones_Regulares> listaER = new Dictionary<string, Expresiones_Regulares>();
        List<Expresiones_Regulares> listaCadenasEv = new List<Expresiones_Regulares>();
        String inicio, final;
        Conjunto nuevo_conjunto;
        bool dentroExpresion = true;
        public RecConjunto() { } //CONSTRUCTOR VACIO

        public Dictionary<string,Conjunto> GetConjuntos() 
        {
            return conjuntos;
        }
        public Dictionary<string,Expresiones_Regulares> GetExpresiones() 
        {
            return listaER;
        }
        public List<Expresiones_Regulares> GetCadenasEvaluacion() 
        {
            return listaCadenasEv;
        }
        //MINI ANALIZADOR SINTACTICO PARA RECONCER LOS CONJUNTOS Y TAMBIEN VA A RECONOCER LAS EXPRESIONES REGULARES
        public void ReconocerConjuntos(LinkedList<Token> lista) 
        {
            dentroExpresion = true;
            this.listaTokens = lista;
            for (int i = 0; i < listaTokens.Count; i++)
            {
                tokenActual = listaTokens.ElementAt(i);
                if (tokenActual.GetTipoToken() == Token.Tipo.CONJUNTO)
                {
                    controlToken = i;
                    CONJUNTO();
                    i = controlToken-1;
                    if (!conjuntos.ContainsKey(nuevo_conjunto.GetNombre()))
                    {
                        conjuntos.Add(nuevo_conjunto.GetNombre(), nuevo_conjunto);
                    }
                   
                }
                else if (tokenActual.GetTipoToken() == Token.Tipo.IDENTIFICADOR)
                {
                    controlToken = i;
                    EXPRESION();
                    i = controlToken-1;
                }
                
            }
        }
        private void EXPRESION() 
        {
            Expresiones_Regulares expresion = new Expresiones_Regulares(tokenActual.GetValorToken());
            emparejar(Token.Tipo.IDENTIFICADOR);
            if (tokenActual.GetTipoToken() == Token.Tipo.GUION)
            {
                emparejar(Token.Tipo.GUION);
                emparejar(Token.Tipo.MAYOR);
                while (tokenActual.GetTipoToken() != Token.Tipo.PUNTO_COMA && dentroExpresion==true)
                {
                    expresion.AñadirElemento(tokenActual);
                    controlToken++;
                    tokenActual = listaTokens.ElementAt(controlToken);
                }
                emparejar(Token.Tipo.PUNTO_COMA);
                if (!listaER.ContainsKey(expresion.GetNombre()))
                {
                    listaER.Add(expresion.GetNombre(), expresion);
                }
            }
            else if (tokenActual.GetTipoToken() == Token.Tipo.DOS_PUNTOS)
            {
                emparejar(Token.Tipo.DOS_PUNTOS);
                while (tokenActual.GetTipoToken() != Token.Tipo.PUNTO_COMA)
                {
                    expresion.AñadirElemento(tokenActual);
                    controlToken++;
                    tokenActual = listaTokens.ElementAt(controlToken);
                }
                emparejar(Token.Tipo.PUNTO_COMA);
                if (listaER.ContainsKey(expresion.GetNombre()))
                {
                    listaCadenasEv.Add(expresion);
                }
            }
        }

        private void CONJUNTO() 
        {
            emparejar(Token.Tipo.CONJUNTO);
            emparejar(Token.Tipo.DOS_PUNTOS);
            nuevo_conjunto = new Conjunto(tokenActual.GetValorToken());
            emparejar(Token.Tipo.IDENTIFICADOR);
            emparejar(Token.Tipo.GUION);
            emparejar(Token.Tipo.MAYOR);
            CONJUNTO2();
            nuevo_conjunto.AgregarSimbolo(inicio);
            nuevo_conjunto.AgregarSimbolo(final);
            emparejar(Token.Tipo.PUNTO_COMA); //final del conjunto
        }
        private void CONJUNTO2()
        {
            inicio = tokenActual.GetValorToken();
            emparejar(Token.Tipo.IDENTIFICADOR);
            CONJUNTO3();
        }
        private void CONJUNTO3() 
        {
            if (tokenActual.GetTipoToken() == Token.Tipo.COMA)
            {
                nuevo_conjunto.AgregarSimbolo(inicio);
                emparejar(Token.Tipo.COMA);
                inicio = tokenActual.GetValorToken();
                emparejar(Token.Tipo.IDENTIFICADOR);
                CONJUNTO3();
            }
            else if (tokenActual.GetTipoToken() == Token.Tipo.GUION_CURVO)
            {
                emparejar(Token.Tipo.GUION_CURVO);

                final = tokenActual.GetValorToken();
                nuevo_conjunto.definirRango(inicio, final);
                emparejar(Token.Tipo.IDENTIFICADOR);
            }
            //si no entra a nada es epsilon
        }
        private void emparejar(Token.Tipo tipo) //recibe el tipo para no perderme en el codigo
        {
            controlToken++;
            if (controlToken < listaTokens.Count)
            {
                tokenActual = listaTokens.ElementAt(controlToken);
            }
            else
            {
                dentroExpresion = false;
            }
        }
        /*                      GRAMATICA
         *      <CONJUNTO> ::= CONJ : Identificador -> <CONJUNTO2> ;
         *      <CONJUNTO2> ::= <TERMINAL><CONJUNTO3>
         *      <CONJUNTO3> ::= , <TERMINAL><CONJUNTO3>
         *                    | ~ <TERMINAL><CONJUNTO3>
         *                    | epsilon
         */

    }
}
