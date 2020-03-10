using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class Conjunto
    {
        String nombre;
        List<char> elementos;

        public Conjunto() { } //CONSTRUCTOR VACIO
        public Conjunto(String nombre) 
        {
            this.nombre = nombre;
        }
        public void definirRango(char inicio, char final) 
        {
            int size, letra;
            size = inicio - final;
            letra = inicio;

            for (byte i = 0; i < size; i++)//los mete en orden
            {
                elementos.Add((char)letra);
                letra++;
            }
            
        }
        public void agregarSimbolo(char a) 
        {
            elementos.Add(a);
            if (elementos.Count > 0)
            {
                elementos.Sort(); //para tener todo en orden en su codigo ASCII para mas placer
            }
        }


    }
}
