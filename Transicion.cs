using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class Transicion
    {
        Estado primero;
        Estado segundo;
        string simbolo;

        public Transicion() { }

        public Transicion(Estado first, Estado last, string simboloTransicion) 
        {
            primero = first;
            segundo = last;
            simbolo = simboloTransicion;
        }
        public void SetPrimerEstado(Estado first) 
        {
            primero = first;
        }
        public void SetSegundoEstado(Estado last) 
        {
            segundo = last;
        }
        public Estado GetPrimero() { return primero; }
        public Estado GetSegundo() { return segundo; }
        public string GetTransicionSimbolo() { return simbolo; }

        
    }
}
