using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class Mueve
    {
        string inicial;
        string llegada;
        string simboloTransicion;
        public Mueve(string ini, string llega, string simbolo)
        {
            inicial = ini;
            llegada = llega;
            simboloTransicion = simbolo;
        }
        public Mueve() { }
        public Mueve(string ini, string simbolo) 
        {
            inicial = ini;
            simboloTransicion = simbolo;
        }
        public string GetInicial() { return inicial; }
        public string GetLlegada() { return llegada; }
        public string GetSimbolo() { return simboloTransicion; }
        public void SetLlegada(string llegada) { this.llegada = llegada; }
        public void SetSimboloTransicion(string simbolo) { simboloTransicion = simbolo; }
    }
}
