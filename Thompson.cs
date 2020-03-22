using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class Thompson
    {
        class NodoArbol
        {
            NodoArbol derecho;
            NodoArbol izquierdo;
            public NodoArbol() 
            {
                derecho = null;
                izquierdo = null;
            }
            public NodoArbol GetIzq() { return izquierdo; }
            public NodoArbol GetDer() { return derecho; }
        }

        //ATRIBUTOS
        LinkedList<Token> listaTokens;
        Stack<string> ER_inorder;

        NodoArbol raiz;

        public Thompson() { }
        //METODOS PARA EL ARBOL
        public void insertar() 
        {
            insertarNodo(raiz);
        }
        private void insertarNodo(NodoArbol nodoArbol)
        {
            if (nodoArbol == null)
            {
                NodoArbol nuevo_nodo = new NodoArbol();
                nodoArbol = nuevo_nodo;
            }
            else
            {
                //Primero meter en izquierda luego derecha
                if (nodoArbol.GetIzq() == null)
                {
                    insertarNodo(nodoArbol.GetIzq());
                }
                else
                {
                    insertarNodo(nodoArbol.GetDer());
                }
            }
        }


    }
}
