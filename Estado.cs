using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    [Serializable]
    class Estado //ESTADOS DEL AUTOMATA FINITO
    {
        int noEstado;
        Tipo tipoEstado;
        //TRANSICIONES DEPENDIENDO DE LOS HIJOS
        Transicion T_izquierda; // TRANSICION HACIA HIJO IZQUIERDO
        Transicion T_derecha;   // TRANSICION HACIA HIJO DERECHO
       

        public enum Tipo
        {
            ACEPTACION,
            NORMAL
        }
        public Estado() { }
        public Estado(int no)  //LO PONE COMO UN ESTADO DEL TIPO NORMAL
        {
            noEstado = no;
            tipoEstado = Tipo.NORMAL;
            T_derecha = null;
            T_izquierda = null;
        }
        public void SetTransicionIzq(Transicion transicion) 
        {
            T_izquierda = transicion;
        }
        public void SetTransicionDer(Transicion transicion)
        {
            T_derecha = transicion;
        }
        public void SetNumeroEstado(int numero) 
        {
            noEstado = numero;
        }
        public Tipo GetTipoEstado()
        {
            return tipoEstado;
        }

        public int GetNoEstado() 
        {
            return noEstado;
        }
        public Transicion GetTransicionIzq() 
        {
            return T_izquierda;
        }
        public Transicion GetTransicionDer() 
        {
            return T_derecha;
        }
    }
}
