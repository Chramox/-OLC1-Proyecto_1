using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class Mueve
    {
        EstadoAFD inicial;
        EstadoAFD llegada;
        string simboloTransicion;
        public Mueve(EstadoAFD ini, EstadoAFD llega, string simbolo)
        {
            inicial = ini;
            llegada = llega;
            simboloTransicion = simbolo;
        }
        public Mueve() { }
        public Mueve(EstadoAFD ini, string simbolo) 
        {
            inicial = ini;
            simboloTransicion = simbolo;
        }
        public EstadoAFD GetEstadoInicial() { return inicial; }
        public EstadoAFD GetEstadoLlegada() { return llegada; }
        public string GetInicialString() { return inicial.GetNombreEstadoAFD().ToString(); }
        public string GetLlegadaString() { return llegada.GetNombreEstadoAFD().ToString(); }
        public string GetSimbolo() { return simboloTransicion; }
        public void SetLlegada(EstadoAFD llegada) { this.llegada = llegada; }
        public void SetSimboloTransicion(string simbolo) { simboloTransicion = simbolo; }
    }
}
