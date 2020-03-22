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
        List<String> elementos = new List<String>();

        public Conjunto() { }
        public Conjunto(String nombre)
        {
            this.nombre = nombre;
        }
        public String GetNombre()
        {
            return nombre;
        }
        public List<String> GetElementos()
        {
            return elementos;
        }
        public void definirRango(String inicio, String final)
        {
            char letra;
            int start, end;
            // Hay dos casos: 1. Letras (usar ASCII) 2. Digitos (Alli miramos que pedo)
            if (int.TryParse(inicio, out start)) //significa que son numeros
            {
                end = int.Parse(final);
                //definiendo rango de numeros
                for (int i = start; i < end; i++)//los mete en orden
                {
                    elementos.Add(i.ToString());
                }
            }
            else //son letras, usar codigo ASCII
            {
                start = inicio[0];
                end = final[0];
                for (int i = start; i < end; i++)
                {
                    letra = (char)i;
                    elementos.Add(letra.ToString());
                }
            }


        }
        public void AgregarSimbolo(String a)
        {
            if (!elementos.Contains(a) && a != null)
            {
                elementos.Add(a);
                if (elementos.Count > 0)
                {
                    elementos.Sort(); //para tener todo en orden en su codigo ASCII para mas placer
                }
            }
        }

    }
}
