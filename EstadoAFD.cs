using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class EstadoAFD
    {
        int nombre;
        List<int> noEstados;
        List<Estado> estadosAlcanzados;
        Tipo tipoEstado;
        public enum Tipo 
        {
            NORMAL,
            FINAL
        }
        public EstadoAFD() { }
        public EstadoAFD(int nombre,List<int>noEstados, List<Estado> estados) 
        {
            this.nombre = nombre;
            this.noEstados = noEstados;
            estadosAlcanzados = estados;
        }
        public Tipo GetTipo() { return tipoEstado; }
        public void SetTipoEstado(Tipo tipo) { tipoEstado = tipo; }
        public int GetNombreEstadoAFD() { return nombre; }
        public List<int> GetNoEstados() { return noEstados; }
        public List<Estado> GetEstadosAlcanzados() { return estadosAlcanzados; }
        public void SetNombreEstadoAFD(int nombre) { this.nombre = nombre; }
        public void SetNoEstadosAFN(List<int> estados) { this.noEstados = estados; }
        public void SetEstadosAlcanzados(List<Estado> alcanzados) { estadosAlcanzados = alcanzados; }
    }
}
