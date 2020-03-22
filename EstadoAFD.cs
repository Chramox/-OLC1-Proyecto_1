using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class EstadoAFD
    {
        char nombre;
        List<int> noEstados;
        List<Estado> estadosAlcanzados;
      
        public EstadoAFD() { }
        public EstadoAFD(char nombre,List<int>noEstados, List<Estado> estados) 
        {
            this.nombre = nombre;
            this.noEstados = noEstados;
            estadosAlcanzados = estados;
        }
        public char GetNombreEstadoAFD() { return nombre; }
        public List<int> GetNoEstados() { return noEstados; }
        public List<Estado> GetEstadosAlcanzados() { return estadosAlcanzados; }
        public void SetNombreEstadoAFD(char nombre) { this.nombre = nombre; }
        public void SetNoEstadosAFN(List<int> estados) { this.noEstados = estados; }
        public void SetEstadosAlcanzados(List<Estado> alcanzados) { estadosAlcanzados = alcanzados; }
    }
}
