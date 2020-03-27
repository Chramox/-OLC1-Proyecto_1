using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OLC1_Proyecto_1
{
    class Analizador_Lexico
    {
        //VARIABLES
        LinkedList<Token> listaTokens = new LinkedList<Token>();
        LinkedList<Token> listaErrores = new LinkedList<Token>();
        Dictionary<string,Conjunto> listaConjuntos;
        Dictionary<string,Expresiones_Regulares> listaER;
        List<Expresiones_Regulares> listaCadenas;
        //STRING es un vector de letras, va a contener todo el conjunto

        int idToken;
        int fila = 1;
        int columna = 1;
        int estado = 0; //variable para ir cambiando de estado
        String auxiliarLexema = ""; //variable donde se guarda el valor del Token
        bool esCadena = false;
        bool esComentarioSimple = false;
        bool esComentarioMultiple = false;
        bool esNumeroFlotante = false;

        public Dictionary<string, Conjunto> GetConjuntos() { return listaConjuntos; }
        public LinkedList<Token> getlistaTokens()
        {
            return listaTokens;
        }
        public LinkedList<Token> getlistaErrores()
        {
            return listaErrores;
        }
        public Dictionary<string,Expresiones_Regulares> GetListaER() 
        {
            return listaER;
        }
        public List<Expresiones_Regulares> GetCadenasEv() { return listaCadenas; }
        public void separarLineas(String Lineas)
        {
            Lineas += "Ç";
            String[] lines = Lineas.Split('\n');
            foreach (var line in lines)
            {
                Leer(line);
            }
            if (listaErrores.Count != 0)
            {
                Console.WriteLine("LISTA DE ERRORES");
                foreach (var Token in listaErrores)
                {
                    Console.WriteLine(Token.GetValorToken() + "<------>" + Token.GetTipoToken());
                }

            }
            if (listaTokens.Count != 0)
            {
                RecConjunto rec = new RecConjunto();
                rec.ReconocerConjuntos(listaTokens);
                listaER = rec.GetExpresiones();
                listaCadenas = rec.GetCadenasEvaluacion();
                Console.WriteLine("LISTA DE EXPRESIONES REGULARES");
                foreach (var elementoG in listaER)
                {
                    Expresiones_Regulares elemento = elementoG.Value;
                    Console.WriteLine("Nombre Expresion: " + elemento.GetNombre());
                    foreach (var item in elemento.GetTokens())
                    {
                        Console.WriteLine("Token: " + item.GetValorToken());
                    }
                }
                listaConjuntos = rec.GetConjuntos();
                Console.WriteLine("LISTA DE CONJUNTOS DECLARADOS");
                foreach (var conjunto in listaConjuntos)
                {
                    Conjunto conjunto1 = conjunto.Value;
                    Console.WriteLine("Nombre Conjunto: " + conjunto1.GetNombre());
                    Console.WriteLine("Elementos: ");
                    byte contador = 0;
                    foreach (var item in conjunto1.GetElementos())
                    {
                        Console.WriteLine(contador +". "+ item);
                        contador++;
                    }
                }
                foreach (var Token in listaTokens)
                {
                    Console.WriteLine(Token.GetValorToken() + "<------>" + Token.GetTipoToken());
                }
                

            }
            
        }
        public void Leer(String Texto)
        {
            char c;
            fila++;
            columna = 1;

            char[] caracteres = Texto.ToCharArray();

            for (int i = 0; i < Texto.Length; i++)
            {
                c = caracteres[i];

                if (c == 'Ç' && i == (caracteres.Length - 1))
                {
                    if (esCadena || esComentarioSimple || esComentarioMultiple)
                    {

                    }
                    else
                    {
                        Console.WriteLine("FIN ANALISIS");
                        foreach (var Token in listaTokens)
                        {
                            Console.WriteLine(Token.GetValorToken() + "<------>" + Token.GetTipoToken());
                        }
                        if (listaErrores.Count != 0)
                        {
                            foreach (var Token in listaErrores)
                            {
                                Console.WriteLine(Token.GetValorToken() + "<------>" + Token.GetTipoToken());
                            }
                        }
                        break;
                    }
                }
                //IMPLEMENTACION DEL AUTOMATA FINITO
                switch (estado)
                {
                    // ESTADO INICIAL
                    case 0:
                        {
                            if (Char.IsDigit(c))
                            {
                                estado = 1;
                                auxiliarLexema += c;
                            }
                            else if (Char.IsLetter(c))
                            {
                                estado = 2;
                                auxiliarLexema += c;
                            }//EMPIEZA COMPARACION PARA LOS SIMBOLOS
                            else if (Char.IsWhiteSpace(c))
                            {
                                estado = 0;
                            }
                            else if (c == '<')
                            { //COMENTARIO MULTIPLE
                                auxiliarLexema += c;
                                estado = 5;
                            }//AÑADIENDO CARACTERES ESPECIALES
                            else if (c == '\\')
                            {
                                auxiliarLexema += c;
                                estado = 6;
                            }
                            else if (c == '[')
                            {
                                auxiliarLexema += c;
                                estado = 7;
                            }
                            else if (c == '\"')
                            { //COMILLAS DOBLES --> CADENA
                                estado = 3;
                            }
                            /* else if (c == '\'') { //COMILLAS DOBLES --> CADENA
                                 estado = 3;
                             }*/
                            else if (c == '/')
                            { //COMENTARIO SIMPLE
                                auxiliarLexema += c;
                                estado = 5;
                            }
                            else if (c == '!')
                            {
                                auxiliarLexema += c;
                                estado = 5;
                            }
                            else if (reconocerSimbolos(c.ToString()))
                            {
                                //ADENTRO DEL METODO SE ENCARGA DE TODO EL MANEJO
                            }
                            else
                            {
                                if (c == 'Ç' && i == (caracteres.Length - 1))
                                {
                                    foreach (var Token in listaTokens)
                                    {
                                        Console.WriteLine(Token.GetValorToken() + "<------>" + Token.GetTipoToken());
                                    }
                                    if (listaErrores.Count != 0)
                                    {
                                        foreach (var Token in listaErrores)
                                        {
                                            Console.WriteLine(Token.GetValorToken() + "<------>" + Token.GetTipoToken());
                                        }
                                    }
                                }
                            }

                        }
                        break;
                    case 1://NUMEROS, SI Y SOLO SI, NUMEROS
                        {
                            if (Char.IsDigit(c))
                            {
                                estado = 1;
                                auxiliarLexema += c;
                            }
                            else if (c == '.')
                            {
                                esNumeroFlotante = true;
                                estado = 1;
                                auxiliarLexema += c;
                            }
                            else if (Char.IsLetter(c))// SE CONVIERTE EN ERROR (ID`S NO PUEDEN EMPEZAR CON NUMERO)
                            {
                                estado = 4;
                                auxiliarLexema += c;
                            }
                            else
                            {
                                //idToken = 40;
                                i--;
                                if (esNumeroFlotante == false)
                                {
                                    agregarToken(Token.Tipo.NUMERO_ENTERO);
                                }
                                else // esNumeroFlotante == true -> se agreaga un numero flotante
                                {
                                    agregarToken(Token.Tipo.NUMERO_FLOTANTE);
                                }
                            }
                        }
                        break;
                    case 2://RESERVADAS Y LETRAS
                        {
                            if (Char.IsLetter(c))
                            {
                                estado = 2;
                                auxiliarLexema += c;
                            }
                            else if (Char.IsDigit(c))// ID`S PUEDEN TERMINAR CON NUMERO
                            {
                                estado = 2;
                                auxiliarLexema += c;
                            }
                            else if (c == '_')
                            {
                                estado = 2;
                                auxiliarLexema += c;
                            }
                            else
                            {
                                i--;
                                auxiliarLexema = auxiliarLexema.Trim();
                                palabrasReservadas(auxiliarLexema);  //COMPARAR PARA VER SI COINCIDE CON ALGUAN PALABRA RESERVADA
                                                                     //DE LO CONTRARIO ES UN IDENTIFICADOR
                            }
                            break;
                        }
                    case 3: //CADENAS
                        {
                            if (c == '"' && esCadena == false)
                            {
                                estado = 3;
                                esCadena = true;
                            }

                            if (c == '"' && esCadena == true)
                            {
                                esCadena = false;
                                idToken = 41;
                                agregarToken(Token.Tipo.CADENA);
                                estado = 0;
                            }
                            else //SIGUE CONCATENANDO HASTA QUE ENCUENTRA LA SEGUNDA COMIILA DOBLE
                            {
                                auxiliarLexema += c;
                                estado = 3;
                                esCadena = true;
                            }
                        }
                        break;
                    case 4: // ESTADO DE ERRORES
                        {
                            if (Char.IsLetterOrDigit(c))//para que reconozca todo el error
                            {
                                auxiliarLexema += c;
                                estado = 4;
                            }
                            else
                            {
                                i--;
                                agregarErrores();
                                estado = 0;
                            }
                            break;
                        }
                    case 5: //COMENTARIOS
                        {
                            // Console.WriteLine(auxiliarLexema);
                            //  Console.WriteLine(c);
                            estado = 5;
                            if ((i + 1) == caracteres.Length && esComentarioSimple)
                            {//FIN COMENTARIO SIMPLE
                                auxiliarLexema += c;
                                esComentarioSimple = false;
                                agregarToken(Token.Tipo.COMENTARIO_SIMPLE);
                            }
                            else if (c == '/' && esCadena == false && esComentarioMultiple == false)
                            {
                                auxiliarLexema += c;
                                esComentarioSimple = true;
                            }
                            else if (c == '!' && esCadena == false && esComentarioSimple == false) // PARA COMENTARIOS <! 
                            {
                                auxiliarLexema += c;
                                esComentarioMultiple = true;
                                //                        reconocerSimbolos(auxiliarLexema);
                            }
                            else if (c == '>' && esCadena == false && auxiliarLexema.EndsWith("!")) // PARA COMENTARIOS !>
                            {
                                auxiliarLexema += c;
                                //reconocerSimbolos(auxiliarLexema);
                                agregarToken(Token.Tipo.COMENTARIO_MULTILINEA);
                                esComentarioMultiple = false;
                            }
                            else if (esComentarioMultiple || esComentarioSimple && c != 'Ç')
                            {
                                auxiliarLexema += c;
                            }
                            else // MANDA LOS OPERADORES DE UN SOLO CARACTER, MANDA EL SEGUNDO CARACTER OTRA VEZ A ANALISIS
                            {
                                i--;
                                if (reconocerSimbolos(auxiliarLexema))
                                {

                                }
                                else { estado = 0; }

                            }
                            break;
                        }
                    case 6: //CARACTERES ESPECIALES: SALTO DE LINEA, TABULACION, COMILLA SIMPLE SOLA, COMILLA DOBLE SOLA
                        {
                            if (c == 'n')
                            {
                                auxiliarLexema += c;
                                idToken = 1;
                                agregarToken(Token.Tipo.SALTO_DE_LINEA);
                            }
                            else if (c == 't')
                            {
                                auxiliarLexema += c;
                                idToken = 2;
                                agregarToken(Token.Tipo.TABULACION);
                            }
                            else if (c == '\'')
                            {
                                auxiliarLexema += c;
                                idToken = 3;
                                agregarToken(Token.Tipo.COMILLA_SIMPLE_SOLA);
                            }
                            else if (c == '\"')
                            {
                                auxiliarLexema += c;
                                idToken = 4;
                                agregarToken(Token.Tipo.COMILLA_DOBLE_SOLA);
                            }
                            else
                            {
                                i--;
                                reconocerSimbolos(c.ToString()); //añadiendo el \
                            }
                            break;
                        }
                    case 7: //CASE SOLO PARA TODO
                        {
                            int temp = i;
                            string temp2 = "[";
                            bool esTODO = true;
                            if (c == ':')
                            {
                                temp++;
                                temp2 += c;
                                if (temp < caracteres.Length)
                                {
                                    c = caracteres[temp];
                                    if (c == 't')
                                    {
                                        temp2 += c;
                                        temp++;
                                        if (temp < caracteres.Length)
                                        {
                                            c = caracteres[temp];
                                            if (c == 'o')
                                            {
                                                temp2 += c;
                                                temp++;
                                                if (temp < caracteres.Length)
                                                {
                                                    c = caracteres[temp];
                                                    if (c == 'd')
                                                    {
                                                        temp2 += c;
                                                        temp++;
                                                        if (temp < caracteres.Length)
                                                        {
                                                            c = caracteres[temp];
                                                            if (c == 'o')
                                                            {
                                                                temp2 += c;
                                                                temp++;
                                                                if (temp < caracteres.Length)
                                                                {
                                                                    c = caracteres[temp];
                                                                    if (c == ':')
                                                                    {
                                                                        temp2 += c;
                                                                        temp++;
                                                                        if (temp < caracteres.Length)
                                                                        {
                                                                            c = caracteres[temp];
                                                                            if (c == ']')
                                                                            {
                                                                                temp2 += c;
                                                                                i = temp;
                                                                                auxiliarLexema = temp2;
                                                                                palabrasReservadas(temp2);
                                                                            }
                                                                            else
                                                                                esTODO = false;
                                                                        }
                                                                        else
                                                                            esTODO = false;
                                                                    }
                                                                    else
                                                                        esTODO = false;
                                                                }
                                                                else
                                                                    esTODO = false;
                                                            }
                                                            else
                                                                esTODO = false;
                                                        }
                                                        else
                                                            esTODO = false;
                                                    }
                                                    else
                                                        esTODO = false;
                                                }
                                                else
                                                    esTODO = false;
                                            }
                                            else
                                                esTODO = false;
                                        }
                                        else
                                            esTODO = false;
                                    }
                                    else
                                        esTODO = false;
                                }
                                else
                                    esTODO = false;
                            }
                            else
                                esTODO = false;
                            if (esTODO == false)
                            { 
                                c = caracteres[i-1];
                                reconocerSimbolos(c.ToString());
                            }
                            break;
                        }
                }
            }
        }

        public void agregarToken(Token.Tipo Tipo)
        {
            Console.WriteLine(auxiliarLexema);
            listaTokens.AddLast(new Token(Tipo, auxiliarLexema, idToken, fila, columna));
            auxiliarLexema = "";
            estado = 0;
        }
        public void palabrasReservadas(String auxiliarLexema)
        {
            //AGREGAR TODAS LAS PALABRAS RESERVADAS
            if (auxiliarLexema.Equals("CONJ"))
            {
                agregarToken(Token.Tipo.CONJUNTO);
            }
            else if (auxiliarLexema.Equals("[:todo:]"))
            {
                agregarToken(Token.Tipo.TODO);
            }
            else
            {
                agregarToken(Token.Tipo.IDENTIFICADOR);
            }

        }
        public void agregarErrores()
        {
            //idToken = 0
            listaErrores.AddLast(new Token(Token.Tipo.ERROR, auxiliarLexema, idToken, fila, columna));
            estado = 0;
        }
        public void reconocerComentarios(String auxiliarLexema)
        {
            auxiliarLexema = auxiliarLexema.Trim();
            Console.WriteLine("Simbolos que pueden ser dobles " + auxiliarLexema);
            if (auxiliarLexema.Equals("<!"))
            { //INICIO COMENTARIO

            }
            else if (auxiliarLexema.Equals("!>"))
            { //FIN DE COMENTARIO MULTILINEA

            }
            else if (auxiliarLexema.Equals("//"))
            { //COMENTARIO DE UNA SOLA LINEA

            }
        }
        private bool reconocerSimbolos(String c)
        {
            auxiliarLexema += c;
            if (null == c) { return false; }
            else // ADMIRACION CIERRE, MENOR QUE Y DIAGONAL CASOS ESPECIALES TAMBIEN CASO ESPECIAL LAS COMILLAS --> CADENAS
                switch (c)
                {
                    case "#":
                        idToken = 35;
                        agregarToken(Token.Tipo.NUMERAL);
                        return true;
                    case "$":
                        idToken = 36;
                        agregarToken(Token.Tipo.DOLAR);
                        return true;
                    case "%":
                        idToken = 37;
                        agregarToken(Token.Tipo.PORCENTAJE);
                        return true;
                    case "&":
                        idToken = 38;
                        agregarToken(Token.Tipo.I_BONITA);
                        return true;
                    case "'":
                        idToken = 39;
                        agregarToken(Token.Tipo.COMILLA_SIMPLE);
                        return true;
                    case "(":
                        idToken = 40;
                        agregarToken(Token.Tipo.PARENTESIS_APERTURA);
                        return true;
                    case ")":
                        idToken = 41;
                        agregarToken(Token.Tipo.PARENTESIS_CIERRE);
                        return true;
                    case "*":
                        idToken = 42;
                        agregarToken(Token.Tipo.ASTERISCO);
                        return true;
                    case "+":
                        idToken = 43;
                        agregarToken(Token.Tipo.OP_SUMA);
                        return true;
                    case ",":
                        idToken = 44;
                        agregarToken(Token.Tipo.COMA);
                        return true;
                    case "-":
                        idToken = 45;
                        agregarToken(Token.Tipo.GUION);
                        return true;
                    case ".":
                        idToken = 46;
                        agregarToken(Token.Tipo.PUNTO);
                        return true;
                    case "/":
                        idToken = 47;
                        agregarToken(Token.Tipo.DIAGONAL);
                        return true;
                    case ":":
                        idToken = 58;
                        agregarToken(Token.Tipo.DOS_PUNTOS);
                        return true;
                    //OMITIENDO MENOR QUE (60) POR QUE SE USA PARA COMENTARIOS (VIENE PRIMERO LUEGO EL SIGNO DE ADMIRACION)
                    case ";":
                        idToken = 59;
                        agregarToken(Token.Tipo.PUNTO_COMA);
                        return true;
                    case "=":
                        idToken = 61;
                        agregarToken(Token.Tipo.IGUAL);
                        return true;
                    case ">":
                        idToken = 62;
                        agregarToken(Token.Tipo.MAYOR);
                        return true;
                    case "?":
                        idToken = 63;
                        agregarToken(Token.Tipo.PREGUNTA_CIERRE);
                        return true;
                    case "@":
                        idToken = 64;
                        agregarToken(Token.Tipo.ARROBA);
                        return true;
                    case "[":
                        idToken = 91;
                        agregarToken(Token.Tipo.CORCHETE_APERTURA);
                        return true;
                    case "\\":
                        idToken = 92;
                        agregarToken(Token.Tipo.DIAGONAL_INVERSO);
                        return true;
                    case "]":
                        idToken = 93;
                        agregarToken(Token.Tipo.CORCHETE_CIERRE);
                        return true;
                    case "^":
                        idToken = 94;
                        agregarToken(Token.Tipo.ELEVAR);
                        return true;
                    case "_":
                        idToken = 95;
                        agregarToken(Token.Tipo.GUION_BAJO);
                        return true;
                    case "`":
                        idToken = 96;
                        agregarToken(Token.Tipo.APOSTROFE);
                        return true;
                    case "{":
                        idToken = 123;
                        agregarToken(Token.Tipo.LLAVE_APERTURA);
                        return true;
                    case "|":
                        idToken = 124;
                        agregarToken(Token.Tipo.PALITO_OR);
                        return true;
                    case "}":
                        idToken = 125;
                        agregarToken(Token.Tipo.LLAVE_CIERRE);
                        return true;
                    case "~":
                        idToken = 0;
                        agregarToken(Token.Tipo.GUION_CURVO);
                        return true;
                    default:
                        return false;
                }
        }
        public void GenerarXML(LinkedList<Token> lista)
        {
            using (StreamWriter writer = new StreamWriter(@"Tokens.txt"))
            {
                //INICIO 
                String doc = "<ListaTokens>\n";
                //AGREGA LOS TOKENS ENCONTRADOS
                foreach (var Token in lista)
                { 
                    doc += "<Token>\n"+"\t<Nombre>" + Token.GetTipoToken() + "</Nombre>\n";
                    doc += "\t<Valor>" + Token.GetValorToken() + "</Valor>\n";
                    doc += "\t<Fila>" + Token.GetFila() + "</Fila>\n";
                    doc += "\t<Columna>" + Token.GetColumna() + "</Columna>\n</Token>";
                }
                //CERRAR HTML
                doc += "</ListaTokens>";
                writer.WriteLine(doc);

            }
            MessageBox.Show("Tokens Guardados", "Listado Tokens");
            System.Diagnostics.Process.Start(@"Tokens.txt");

        }
        public void GenerarXML_Error(LinkedList<Token> lista)
        {
            using (StreamWriter writer = new StreamWriter(@"Errores.txt"))
            {
                //INICIO 
                String doc = "<ListaErrores>\n";
                //AGREGA LOS TOKENS ENCONTRADOS
                foreach (var Token in lista)
                { 
                    doc += "\t<ValorError>" + Token.GetValorToken() + "</ValorError>\n";
                    doc += "\t<Fila>" + Token.GetFila() + "</Fila>\n";
                    doc += "\t<Columna>" + Token.GetColumna() + "</Columna>\n</Token>";
                }
                //CERRAR HTML
                doc += "</ListaTokens>";
                writer.WriteLine(doc);

            }
            MessageBox.Show("Errores Guardados", "Listado Errores");
            System.Diagnostics.Process.Start(@"Errores.txt");

        }
    }
}
