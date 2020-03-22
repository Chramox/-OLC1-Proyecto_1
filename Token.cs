using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto_1
{
    class Token
    {
        //ATRIBUTOS DE LA CLASE
        private Tipo tipoToken;
        private String valorToken;
        private int ID; //ID DE LOS TOKEN VA A SER SU CODIGO ASCII
        private int columna;
        private int fila;

        public void SetValor(string val) { valorToken = val; }
        public void SetTipo(Tipo tipo) { tipoToken = tipo; }
        public int GetColumna() 
        {
            return columna;
        }
        public int GetFila() 
        {
            return fila;
        }
        public enum Tipo
        {
            // SIMBOLOS ESPECIALES POR CODIGO ASCII
            ADMIRACION_CIERRE,  //  !   33
            COMILLA_DOBLE,      //  "   34
            NUMERAL,            //  #   35
            DOLAR,              //  $   36
            PORCENTAJE,         //  %   37
            I_BONITA,           //  &   38
            COMILLA_SIMPLE,     //  '   39
            PARENTESIS_APERTURA,//  (   40
            PARENTESIS_CIERRE,  //  )   41
            ASTERISCO,          //  *   42
            OP_SUMA,            //  +   43
            COMA,               //  ,   44
            GUION,              //  -   45
            PUNTO,              //  .   46
            DIAGONAL,           //  /   47

            DOS_PUNTOS,         //  :   58
            PUNTO_COMA,         //  ;   59
            MENOR,              //  <   60
            IGUAL,              //  =   61
            MAYOR,              //  >   62
            PREGUNTA_CIERRE,    //  ?   63
            ARROBA,             //  @   64

            CORCHETE_APERTURA,  //  [   91
            DIAGONAL_INVERSO,   //  \   92
            CORCHETE_CIERRE,    //  ]   93
            ELEVAR,             //  ^   94
            GUION_BAJO,         //  _   95
            APOSTROFE,          //  `   96

            LLAVE_APERTURA,     //  {   123
            PALITO_OR,          //  |   124              
            LLAVE_CIERRE,       //  }   125
            //TOKENS NECESARIOS PARA QUE EL PROGRAMA FUNCIONE
            NUMERO_ENTERO,
            NUMERO_FLOTANTE,
            CADENA,
            ERROR,
            IDENTIFICADOR,
            // COMENTARIOS
            COMENTARIO_SIMPLE, // //
            COMENTARIO_MULTILINEA, // <!
                                   // !>
                                   // -----------------------------
                                   // PALABRAS RESERVADAS
            CONJUNTO,
            GUION_CURVO
        }
        public Token() { } // NADA MAS INICIALIZANDO
        public Token(Tipo type, String val, int id, int fila, int columna)
        {
            this.tipoToken = type;
            this.ID = id;
            this.valorToken = val;
            this.fila = fila;
            this.columna = columna;
        }

        public String GetValorToken()
        {
            return valorToken;
        }

        public Tipo GetTipoToken()
        {
            return tipoToken;

        }
    }

}
