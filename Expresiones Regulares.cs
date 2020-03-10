using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class Expresiones_Regulares
    {
        LinkedList<Token> tabla_Tokens;
        LinkedList<Token> expresion_Regular;

        //VA A FUNCIONAR COMO UN MINI ANALIZADOR SINTACTICO PARA OBTENER LAS EXPRESIONES REGULARES

        public void generarArbol()  //GENERA EL ARBOL PARA METER THOMPSON DESPUES
        {

        }
        public void busqueda_expresion() // es el metodo emparejar
        {
            Token actual;
            for (int i = 0; i < tabla_Tokens.Count; i++)
            {
                actual = tabla_Tokens.ElementAt(i);
                if (actual.GetTipoToken() == Token.Tipo.IDENTIFICADOR)
                {
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
                                    expresion_Regular.AddLast(actual);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        
    }
}
