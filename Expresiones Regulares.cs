using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;  
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    [Serializable]
    class Expresiones_Regulares
    {

        LinkedList<Token> expresion_Regular = new LinkedList<Token>();
        String nombreExpresion;
        //List<string> cadenasEvaluacion = new List<string>();
        public Expresiones_Regulares() { }
        public Expresiones_Regulares(String nombre) 
        {
            nombreExpresion = nombre;
        }
        public LinkedList<Token> GetTokens() 
        {
            return expresion_Regular;
        }
        public String GetNombre() 
        {
            return nombreExpresion; 
        }
        public void AñadirElemento(Token token) 
        {
            expresion_Regular.AddLast(token);
        }
        //VA A FUNCIONAR COMO UN MINI ANALIZADOR SINTACTICO PARA OBTENER LAS EXPRESIONES REGULARES
        public void generarArbol()  //GENERA EL ARBOL PARA METER THOMPSON DESPUES
        {

        }

      /*  public void busqueda_expresion(LinkedList<Token> tabla_Tokens) // es el metodo emparejar, busca la expresion regular y la guarda
        {
            Token actual;
            for (int i = 0; i < tabla_Tokens.Count; i++)
            {
                actual = tabla_Tokens.ElementAt(i);
                if (actual.GetTipoToken() == Token.Tipo.IDENTIFICADOR)
                {
                    Expresiones_Regulares expresion = new Expresiones_Regulares();
                    expresion.nombreExpresion = actual.GetValorToken();
                    i++;
                    actual = tabla_Tokens.ElementAt(i);
                    if (actual.GetTipoToken() == Token.Tipo.GUION)
                    {
                        i++;
                        actual = tabla_Tokens.ElementAt(i);
                        if (actual.GetTipoToken() == Token.Tipo.MAYOR) //EMPIEZA A GUARDAR LA EXPRESION REGULAR
                        {
                            i++;
                            actual = tabla_Tokens.ElementAt(i);
                            for (int j = i; j < tabla_Tokens.Count; j++)//GUARDAR LA EXPRESION REGULAR HASTA EL PUNTO Y COMA
                            {
                                if (actual.GetTipoToken() != Token.Tipo.PUNTO_COMA)
                                {
                                    expresion.expresion_Regular.AddLast(actual);
                                }
                                else
                                {
                                    listaExpresiones.AddLast(expresion);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        */
    }
}
