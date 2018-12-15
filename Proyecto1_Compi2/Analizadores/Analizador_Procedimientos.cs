using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Proyecto1_Compi2.Analizadores
{
    class Analizador_Procedimientos:Grammar
    {

        public Analizador_Procedimientos() : base(false)
        {
            CommentTerminal comentarioSimple = new CommentTerminal("comentarioSimple", "#", "\n", "\r\n");
            CommentTerminal comentarioMulti = new CommentTerminal("comentarioMulti", "#*", "*#");

            base.NonGrammarTerminals.Add(comentarioMulti);
            base.NonGrammarTerminals.Add(comentarioSimple);

            //Reservadas

            RegexBasedTerminal PUNTOCOMA = new RegexBasedTerminal("PUNTOCOMA", ";");
            RegexBasedTerminal COMA = new RegexBasedTerminal("COMA", ",");
            RegexBasedTerminal PUNTO = new RegexBasedTerminal("PUNTO", "\\.");
            RegexBasedTerminal RUSAR = new RegexBasedTerminal("RUSAR", "USAR ");
            RegexBasedTerminal RCREAR = new RegexBasedTerminal("RCREAR", "CREAR ");
            RegexBasedTerminal RBASE = new RegexBasedTerminal("RBASE", "BASE_DATOS");
            RegexBasedTerminal RTABLA = new RegexBasedTerminal("RTABLA", "TABLA");
            RegexBasedTerminal ROBJETO = new RegexBasedTerminal("ROBJETO", "OBJETO");
            RegexBasedTerminal RPROCEDIMIENTO = new RegexBasedTerminal("RPROCEDIMIENTO", "PROCEDIMIENTO");
            RegexBasedTerminal RFUNCION = new RegexBasedTerminal("RFUNCION", "FUNCION ");
            RegexBasedTerminal RRETORNO = new RegexBasedTerminal("RRETORNO", "RETORNO ");
            RegexBasedTerminal RUSUARIO = new RegexBasedTerminal("RUSUARIO", "USUARIO ");
            RegexBasedTerminal RCOLOCAR = new RegexBasedTerminal("RCOLOCAR", "COLOCAR");
            RegexBasedTerminal RPAS = new RegexBasedTerminal("RPAS", "password");
            RegexBasedTerminal RIMPRIMIR = new RegexBasedTerminal("RIMPRIMIR", "IMPRIMIR ");
            RegexBasedTerminal RINSERTAR = new RegexBasedTerminal("RINSERTAR", "INSERTAR ");
            RegexBasedTerminal REN = new RegexBasedTerminal("REN", "EN");
            RegexBasedTerminal RVALORES = new RegexBasedTerminal("RVALORES", "VALORES");
            RegexBasedTerminal RACTUALIZAR = new RegexBasedTerminal("RACTUALIZAR", "ACTUALIZAR");
            RegexBasedTerminal RDONDE = new RegexBasedTerminal("RDONDE", "DONDE");
            RegexBasedTerminal RBORRAR = new RegexBasedTerminal("RBORRAR", "BORRAR");
            RegexBasedTerminal RSELECCIONAR = new RegexBasedTerminal("RSELECCIONAR", "SELECCIONAR");
            RegexBasedTerminal RDE = new RegexBasedTerminal("RDE", "DE");
            RegexBasedTerminal RORDENAR = new RegexBasedTerminal("RORDENAR", "ORDENAR_POR");
            RegexBasedTerminal RASC = new RegexBasedTerminal("RASC", "ASC");
            RegexBasedTerminal RDESC = new RegexBasedTerminal("RDESC", "DESC");
            RegexBasedTerminal ROTORGAR = new RegexBasedTerminal("ROTORGAR", "OTORGAR");
            RegexBasedTerminal RPERMISOS = new RegexBasedTerminal("RPERMISOS", "PERMISOS");
            RegexBasedTerminal RDENEGAR = new RegexBasedTerminal("RDENEGAR", "DENEGAR");
            RegexBasedTerminal RBACKUP = new RegexBasedTerminal("RBACKUP", "BACKUP");
            RegexBasedTerminal RUSQLDUMP = new RegexBasedTerminal("RUSQLDUMP", "USQLDUMP");
            RegexBasedTerminal RCOMPLETO = new RegexBasedTerminal("RCOMPLETO", "COMPLETO");
            RegexBasedTerminal RRESTAURAR = new RegexBasedTerminal("RRESTAURAR", "RESTAURAR");
            RegexBasedTerminal RALTERAR = new RegexBasedTerminal("RALTERAR", "ALTERAR ");
            RegexBasedTerminal RAGREGAR = new RegexBasedTerminal("RAGREGAR", "AGREGAR");
            RegexBasedTerminal RQUITAR = new RegexBasedTerminal("RQUITAR", "QUITAR");
            RegexBasedTerminal RCAMBIAR = new RegexBasedTerminal("RCAMBIAR", "CAMBIAR");
            RegexBasedTerminal RELIMINAR = new RegexBasedTerminal("RELIMINAR", "ELIMINAR");
            RegexBasedTerminal RDECLARAR = new RegexBasedTerminal("RDECLARAR", "DECLARAR ");
            RegexBasedTerminal RSI = new RegexBasedTerminal("RSI", "SI ");
            RegexBasedTerminal RSINO = new RegexBasedTerminal("RSINO", "SINO ");
            RegexBasedTerminal RSELECCIONA = new RegexBasedTerminal("RSELECCIONA", "SELECCIONA ");
            RegexBasedTerminal RCASO = new RegexBasedTerminal("RCASO", "CASO ");
            RegexBasedTerminal RDEFECTO = new RegexBasedTerminal("RDEFECTO", "DEFECTO ");
            RegexBasedTerminal RPARA = new RegexBasedTerminal("RPARA", "PARA ");
            RegexBasedTerminal RMIENTRAS = new RegexBasedTerminal("RMIENTRAS", "MIENTRAS ");
            RegexBasedTerminal RDETENER = new RegexBasedTerminal("RDETENER", "DETENER ");
            RegexBasedTerminal RFECHA = new RegexBasedTerminal("RFECHA", "FECHA");
            RegexBasedTerminal RFECHA_HORA = new RegexBasedTerminal("RFECHA_HORA", "FECHA_HORA");
            RegexBasedTerminal RCONTAR = new RegexBasedTerminal("RCONTAR", "CONTAR");

            //tipos
            IdentifierTerminal ID = new IdentifierTerminal("ID");
            RegexBasedTerminal Variable = new RegexBasedTerminal("Variable", "@[a-zA-Z]+([a-zA-Z0-9_])*");

            RegexBasedTerminal RINTEGER = new RegexBasedTerminal("RINTEGER", "INTEGER ");
            NumberLiteral Entero = new NumberLiteral("entero");


            RegexBasedTerminal RDOUBLE = new RegexBasedTerminal("RDOUBLE", "DOUBLE ");
            RegexBasedTerminal Doble = new RegexBasedTerminal("Doble", "[0-9]+\\.[0-9]{6}");

            RegexBasedTerminal RBOOL = new RegexBasedTerminal("RBOOL", "BOOL ");
            //RegexBasedTerminal Verdadero = new RegexBasedTerminal("verdadero", "verdadero|true");
            //RegexBasedTerminal Falso = new RegexBasedTerminal("falso", "falso|false");

            RegexBasedTerminal RTEXT = new RegexBasedTerminal("RTEXT", "TEXT ");
            StringLiteral Cadena = new StringLiteral("Cadena", "\"");

            RegexBasedTerminal RDATE = new RegexBasedTerminal("RDATE", "DATE ");
            RegexBasedTerminal tfecha = new RegexBasedTerminal("tfecha", "[1-3]*[0-9]-[0-1][0-9]-[1-2][0-9]{3}");

            RegexBasedTerminal RDATETIME = new RegexBasedTerminal("RDATETIME", "DATETIME ");
            RegexBasedTerminal tfechahora = new RegexBasedTerminal("tfechahora", "[1-3]*[0-9]-[0-1][0-9]-[1-2][0-9]{3} [1-2]*[0-9]:[0-9]{2}:[0-9]{2}");

            //Atributos de campos de tablas
            RegexBasedTerminal RNO = new RegexBasedTerminal("RNO", "NO|no|No");
            RegexBasedTerminal RNULO = new RegexBasedTerminal("RNULO", "NULO|Nulo|nulo");
            RegexBasedTerminal RAUTOINCREMENTABLE = new RegexBasedTerminal("RAUTOINCREMENTABLE", "Autoincrementable");
            RegexBasedTerminal RPK = new RegexBasedTerminal("RPK", "Llave_Primaria");
            RegexBasedTerminal RFK = new RegexBasedTerminal("RFK", "Llave_Foranea");
            RegexBasedTerminal RUNICO = new RegexBasedTerminal("RUNICO", "UNICO");

            //Operadores aritmeticos
            RegexBasedTerminal SUMA = new RegexBasedTerminal("SUMA", "\\+");
            RegexBasedTerminal RESTA = new RegexBasedTerminal("RESTA", "-");
            RegexBasedTerminal MULTI = new RegexBasedTerminal("MULTI", "\\*");
            RegexBasedTerminal DIV = new RegexBasedTerminal("DIV", "\\/");
            RegexBasedTerminal POTENCIA = new RegexBasedTerminal("POTENCIA", "\\^");
            RegexBasedTerminal INCREMENTAR = new RegexBasedTerminal("INCREMENTAR", "\\+\\+");
            RegexBasedTerminal DISMINUIR = new RegexBasedTerminal("DISMINUIR", "--");
            RegexBasedTerminal I_ASIGNAR = new RegexBasedTerminal("I_ASIGNAR", "=");


            //Operadores Relacionales

            RegexBasedTerminal IGUAL = new RegexBasedTerminal("IGUAL", "==");
            RegexBasedTerminal DISTINTO = new RegexBasedTerminal("DISTINTO", "!=");
            RegexBasedTerminal MENOR = new RegexBasedTerminal("MENOR", "<");
            RegexBasedTerminal MAYOR = new RegexBasedTerminal("MAYOR", "> ");
            RegexBasedTerminal MENOR_IGUAL = new RegexBasedTerminal("MENOR_IGUAL", "<=");
            RegexBasedTerminal MAYOR_IGUAL = new RegexBasedTerminal("MAYOR_IGUAL", ">=");


            //Operadores Logicos
            RegexBasedTerminal OR = new RegexBasedTerminal("OR", "\\|\\|");
            RegexBasedTerminal AND = new RegexBasedTerminal("AND", "&&");
            RegexBasedTerminal NOT = new RegexBasedTerminal("NOT", "!");


            //Presedencia

            this.RegisterOperators(0, Associativity.Left, SUMA, RESTA);
            this.RegisterOperators(1, Associativity.Left, MULTI, DIV);
            this.RegisterOperators(2, Associativity.Right, POTENCIA);
            this.RegisterOperators(3, IGUAL, DISTINTO, MENOR, MAYOR, MENOR_IGUAL, MAYOR_IGUAL);
            this.RegisterOperators(4, Associativity.Left, OR);
            this.RegisterOperators(5, Associativity.Left, AND);
            this.RegisterOperators(6, Associativity.Left, NOT);


            NonTerminal S = new NonTerminal("S"),
                inicio = new NonTerminal("inicio"),
                sentencias = new NonTerminal("sentencias"),
                sentencia = new NonTerminal("sentencia"),
                usar = new NonTerminal("usar"),
                crear = new NonTerminal("crear"),
                imprimir = new NonTerminal("imprimir"),
                insertar = new NonTerminal("insertar"),
                actualizar = new NonTerminal("actualizar"),
                borrar = new NonTerminal("borrar"),
                seleccionar = new NonTerminal("seleccionar"),
                seleccionarf = new NonTerminal("seleccionarf"),
                otorgar = new NonTerminal("otorgar"),
                denegar = new NonTerminal("denegar"),
                back = new NonTerminal("back"),
                restaurar = new NonTerminal("restaurar"),
                alterar = new NonTerminal("alterar"),
                eliminar = new NonTerminal("eliminar"),
                declarar = new NonTerminal("declarar"),
                contar = new NonTerminal("contar"),
                contarAsig = new NonTerminal("contarAsig"),
                opciones_crear = new NonTerminal("opciones_crear"),
                c_base = new NonTerminal("c_base"),
                c_tabla = new NonTerminal("c_tabla"),
                c_objeto = new NonTerminal("c_objeto"),
                c_pro = new NonTerminal("c_pro"),
                c_funcion = new NonTerminal("c_funcion"),
                c_usuario = new NonTerminal("c_usuario"),
                campos_tabla = new NonTerminal("campos_tabla"),
                campo_tabla = new NonTerminal("campo_tabla"),
                tipo_dato = new NonTerminal("tipo_dato"),
                complementos = new NonTerminal("complementos"),
                complemento = new NonTerminal("complemento"),
                parametros = new NonTerminal("parametros"),
                parametro = new NonTerminal("parametro"),
                instrucciones = new NonTerminal("instrucciones"),
                instruccionesR = new NonTerminal("instruccionesR"),
                instruccion = new NonTerminal("instruccion"),
                asignacion = new NonTerminal("asignacion"),
                Tif = new NonTerminal("if"),
                Tswitch = new NonTerminal("switch"),
                Tfor = new NonTerminal("for"),
                Twhile = new NonTerminal("while"),
                fecha = new NonTerminal("fecha"),
                fecha_hora = new NonTerminal("fecha_hora"),
                retorno = new NonTerminal("retorno"),
                expresion = new NonTerminal("expresion"),
                aritemtica = new NonTerminal("aritemtica"),
                logica = new NonTerminal("logica"),
                relacional = new NonTerminal("relacional"),
                campos = new NonTerminal("campos"),
                valores = new NonTerminal("valores"),
                tipoins = new NonTerminal("tipoins"),
                condicion = new NonTerminal("condicion"),
                logica_consultas = new NonTerminal("logica_consultas"),
                logica_consulta = new NonTerminal("logica_consulta"),
                orden = new NonTerminal("orden"),
                rutaB = new NonTerminal("rutaB"),
                alterartabla = new NonTerminal("alterartabla"),
                alterarobjeto = new NonTerminal("alterarobjeto"),
                variables = new NonTerminal("variables"),
                sino = new NonTerminal("sino"),
                casos = new NonTerminal("casos"),
                caso = new NonTerminal("caso"),
                defecto = new NonTerminal("defecto"),
                opciones_for = new NonTerminal("opciones_for"),
                llamada = new NonTerminal("llamada");




            S.Rule = inicio;//Ya

            inicio.Rule = instrucciones;//Ya

            sentencias.Rule = sentencia + sentencias
                | sentencia;//Ya

            sentencia.Rule = usar
                            | crear
                            | imprimir
                            | insertar
                            | actualizar
                            | borrar
                            | seleccionar
                            | otorgar
                            | denegar
                            | back
                            | restaurar
                            | alterar
                            | eliminar
                            | declarar
                            | llamada + PUNTOCOMA;//Ya



            usar.Rule = RUSAR + ID + PUNTOCOMA;//Ya

            crear.Rule = RCREAR + opciones_crear;//Ya

            opciones_crear.Rule = c_base
                                | c_tabla
                                | c_objeto
                                | c_pro
                                | c_funcion
                                | c_usuario;//Ya

            c_base.Rule = RBASE + ID + PUNTOCOMA;//Ya

            c_tabla.Rule = RTABLA + ID + "(" + campos_tabla + ")" + PUNTOCOMA;//Ya

            campos_tabla.Rule = campo_tabla + COMA + campos_tabla
                              | campo_tabla;//Ya

            campo_tabla.Rule = tipo_dato + ID + complementos
                              | tipo_dato + ID
                              | ID + ID + complementos
                              | ID + ID;//Ya

            complementos.Rule = complemento + complementos
                            | complemento;//Ya

            complemento.Rule = RNO
                            | RNULO
                            | RAUTOINCREMENTABLE
                            | RPK
                            | RFK + ID
                            | RUNICO;

            c_objeto.Rule = ROBJETO + ID + "(" + parametros + ")" + PUNTOCOMA
                        | ROBJETO + ID + "(" + ")" + PUNTOCOMA;//Ya

            parametros.Rule = parametros + COMA + parametro
                            | parametro;//Ya

            parametro.Rule = tipo_dato + ID
                             | ID + ID
                             | ID + Variable
                             | tipo_dato + Variable;//Ya

            c_pro.Rule = RPROCEDIMIENTO + ID + "(" + parametros + ")" + "{" + instrucciones + "}"
                        | RPROCEDIMIENTO + ID + "(" + ")" + "{" + instrucciones + "}";//.5

            instrucciones.Rule = instruccion + instrucciones
                                | instruccion;//.5

            instruccion.Rule = asignacion
                                | Tif
                                | Tswitch
                                | Tfor
                                | Twhile
                                | sentencia
                                | llamada + PUNTOCOMA
                                | RDETENER + PUNTOCOMA
                                | retorno;//.5

            c_funcion.Rule = RFUNCION + ID + "(" + parametros + ")" + tipo_dato + "{" + instrucciones + "}"//9
                           | RFUNCION + ID + "(" + parametros + ")" + ID + "{" + instrucciones + "}"//9
                           | RFUNCION + ID + "(" + ")" + tipo_dato + "{" + instrucciones + "}"//8
                           | RFUNCION + ID + "(" + ")" + ID + "{" + instrucciones + "}";//8


            instruccionesR.Rule = instrucciones + retorno;

            retorno.Rule = RRETORNO + aritemtica + PUNTOCOMA;


            c_usuario.Rule = RUSUARIO + ID + RCOLOCAR + RPAS + I_ASIGNAR + Cadena + PUNTOCOMA;//Ya

            imprimir.Rule = RIMPRIMIR + "(" + valores + ")" + PUNTOCOMA; //Ya

            insertar.Rule = RINSERTAR + REN + RTABLA + ID + tipoins;//ya

            tipoins.Rule = "(" + campos + ")" + RVALORES + "(" + valores + ")" + PUNTOCOMA
                        | "(" + valores + ")" + PUNTOCOMA;//ya

            campos.Rule = ID + COMA + campos
                        | ID;//ya

            valores.Rule = aritemtica + COMA + valores
                        | aritemtica;//ya

            actualizar.Rule = RACTUALIZAR + RTABLA + ID + "(" + campos + ")" + RVALORES + "(" + valores + ")" + condicion + PUNTOCOMA
                            | RACTUALIZAR + RTABLA + ID + "(" + campos + ")" + RVALORES + "(" + valores + ")" + PUNTOCOMA;

            condicion.Rule = RDONDE + logica_consultas;

            borrar.Rule = RBORRAR + REN + RTABLA + ID + condicion + PUNTOCOMA
                        | RBORRAR + REN + RTABLA + ID + PUNTOCOMA;

            seleccionar.Rule = RSELECCIONAR + campos + RDE + campos + condicion + orden + PUNTOCOMA
                             | RSELECCIONAR + MULTI + RDE + campos + condicion + orden + PUNTOCOMA
                             | RSELECCIONAR + campos + RDE + campos + condicion + PUNTOCOMA
                             | RSELECCIONAR + MULTI + RDE + campos + condicion + PUNTOCOMA
                             | RSELECCIONAR + campos + RDE + campos + orden + PUNTOCOMA
                             | RSELECCIONAR + MULTI + RDE + campos + orden + PUNTOCOMA
                             | RSELECCIONAR + campos + RDE + campos + PUNTOCOMA
                             | RSELECCIONAR + MULTI + RDE + campos + PUNTOCOMA;

            seleccionarf.Rule = RSELECCIONAR + campos + RDE + campos + condicion + orden
                             | RSELECCIONAR + MULTI + RDE + campos + condicion + orden
                             | RSELECCIONAR + campos + RDE + campos + condicion
                             | RSELECCIONAR + MULTI + RDE + campos + condicion
                             | RSELECCIONAR + campos + RDE + campos + orden
                             | RSELECCIONAR + MULTI + RDE + campos + orden
                             | RSELECCIONAR + campos + RDE + campos
                             | RSELECCIONAR + MULTI + RDE + campos;

            orden.Rule = RORDENAR + ID + RASC
                    | RORDENAR + ID + RDESC
                    | RORDENAR + ID;

            otorgar.Rule = ROTORGAR + RPERMISOS + ID + COMA + rutaB + PUNTOCOMA;

            rutaB.Rule = rutaB + PUNTO + ID
                    | Variable
                    | ID
                    | MULTI;

            denegar.Rule = RDENEGAR + RPERMISOS + ID + COMA + rutaB + PUNTOCOMA;

            back.Rule = RBACKUP + RUSQLDUMP + ID + ID + PUNTOCOMA
                    | RBACKUP + RCOMPLETO + ID + ID + PUNTOCOMA;

            restaurar.Rule = RRESTAURAR + RUSQLDUMP + Cadena + PUNTOCOMA
                    | RRESTAURAR + RCOMPLETO + Cadena + PUNTOCOMA;


            alterar.Rule = RALTERAR + RTABLA + ID + alterartabla + PUNTOCOMA
                        | RALTERAR + ROBJETO + ID + alterarobjeto + PUNTOCOMA
                        | RALTERAR + RUSUARIO + ID + RCAMBIAR + RPAS + I_ASIGNAR + Cadena + PUNTOCOMA;

            alterartabla.Rule = RAGREGAR + "(" + campos_tabla + ")"
                            | RQUITAR + campos;

            alterarobjeto.Rule = RAGREGAR + "(" + parametros + ")"
                            | RQUITAR + campos;


            eliminar.Rule = RELIMINAR + RTABLA + ID + PUNTOCOMA
                         | RELIMINAR + RBASE + ID + PUNTOCOMA
                         | RELIMINAR + ROBJETO + ID + PUNTOCOMA
                         | RELIMINAR + RUSUARIO + ID + PUNTOCOMA;

            declarar.Rule = RDECLARAR + variables + tipo_dato + I_ASIGNAR + expresion + PUNTOCOMA
                        | RDECLARAR + variables + ID + PUNTOCOMA
                        | RDECLARAR + variables + tipo_dato + PUNTOCOMA;

            variables.Rule = Variable + COMA + variables
                      | Variable;

            asignacion.Rule = rutaB + I_ASIGNAR + aritemtica + PUNTOCOMA
                        | Variable + I_ASIGNAR + aritemtica + PUNTOCOMA;


            Tif.Rule = RSI + "(" + logica + ")" + "{" + instrucciones + "}" + sino
                    | RSI + "(" + logica + ")" + "{" + instrucciones + "}";

            sino.Rule = RSINO + "{" + instrucciones + "}";


            Tswitch.Rule = RSELECCIONA + "(" + logica + ")" + "{" + casos + "}";

            casos.Rule = caso + casos
                    | caso
                    | defecto;

            caso.Rule = RCASO + expresion + ":" + instrucciones;


            defecto.Rule = RDEFECTO + ":" + instrucciones;

            Tfor.Rule = RPARA + "(" + RDECLARAR + Variable + RINTEGER + I_ASIGNAR + expresion + PUNTOCOMA + logica + PUNTOCOMA + opciones_for + ")" + "{" + instrucciones + "}";

            opciones_for.Rule = INCREMENTAR
                              | DISMINUIR;

            Twhile.Rule = RMIENTRAS + "(" + logica + ")" + "{" + instrucciones + "}";

            contar.Rule = RCONTAR + "(" + "<<" + seleccionarf + ">>" + ")" + PUNTOCOMA;

            contarAsig.Rule = RCONTAR + "(" + "<<" + seleccionarf + ">>" + ")";

            tipo_dato.Rule = RINTEGER
                          | RTEXT
                          | RDOUBLE
                          | RBOOL
                          | RDATE
                          | RDATETIME;//Ya

            logica.Rule = logica + OR + logica
                        | logica + AND + logica
                        | NOT + logica
                        | "(" + logica + ")"
                        | relacional;//ya

            relacional.Rule = relacional + IGUAL + aritemtica
                            | relacional + DISTINTO + aritemtica
                            | relacional + MAYOR_IGUAL + aritemtica
                            | relacional + MENOR_IGUAL + aritemtica
                            | relacional + MAYOR + aritemtica
                            | relacional + MENOR + aritemtica
                            | "(" + relacional + ")"
                            | aritemtica;//ya

            aritemtica.Rule = aritemtica + SUMA + expresion
                           | aritemtica + RESTA + expresion
                           | aritemtica + MULTI + expresion
                           | aritemtica + DIV + expresion
                           | aritemtica + POTENCIA + expresion
                           | "(" + aritemtica + ")"
                           | RESTA + expresion
                           | expresion;//ya

            expresion.Rule = Entero
                           | Doble
                           | Cadena
                           | tfecha
                           | tfechahora
                           | llamada
                           | contarAsig//ya
                           | rutaB;

            llamada.Rule = RFECHA + "(" + ")"
                          | RFECHA_HORA + "(" + ")"
                          | rutaB + "(" + ")"
                          | rutaB + "(" + valores + ")";

            logica_consultas.Rule = logica_consulta + REN + logica_consultas
                                  | logica_consulta;

            logica_consulta.Rule = "(" + seleccionar + ")"
                                  | logica;



            this.Root = S;
        }
    }
}
