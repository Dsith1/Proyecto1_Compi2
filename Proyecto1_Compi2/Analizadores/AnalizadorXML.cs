using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Proyecto1_Compi2.Analizadores
{
    class AnalizadorXML:Grammar
    {

        public AnalizadorXML() : base(false)
        {
            //tipos
            IdentifierTerminal ID = new IdentifierTerminal("ID");
            StringLiteral Cadena = new StringLiteral("Cadena","@@");
            StringLiteral Atributos = new StringLiteral("Atributos", "\"");

            //Etiquetas
            RegexBasedTerminal ABAse = new RegexBasedTerminal("ABAse", "<DB>");
            RegexBasedTerminal CBAse = new RegexBasedTerminal("CBAse", "</DB>");
            RegexBasedTerminal ANombre = new RegexBasedTerminal("ANombre", "<nombre>");
            RegexBasedTerminal CNombre = new RegexBasedTerminal("CNombre", "</nombre>");
            RegexBasedTerminal ARuta = new RegexBasedTerminal("ARuta", "<path>");
            RegexBasedTerminal CRuta = new RegexBasedTerminal("CRuta", "</path>");
            RegexBasedTerminal AUsuario = new RegexBasedTerminal("AUsuario", "<User>");
            RegexBasedTerminal CUsuario = new RegexBasedTerminal("CUsuario", "</User>");
            RegexBasedTerminal AContra = new RegexBasedTerminal("AContra", "<contra>");
            RegexBasedTerminal CContra = new RegexBasedTerminal("CContra", "</contra>");
            RegexBasedTerminal APermisos = new RegexBasedTerminal("APermisos", "<permisos>");
            RegexBasedTerminal CPermisos = new RegexBasedTerminal("CPermisos", "</permisos>");
            RegexBasedTerminal AObject = new RegexBasedTerminal("AObject", "<Object>");
            RegexBasedTerminal CObject = new RegexBasedTerminal("CObject", "</Object>");
            RegexBasedTerminal AProcedimiento = new RegexBasedTerminal("AProcedimiento", "<Procedure>");
            RegexBasedTerminal CProcedimiento = new RegexBasedTerminal("CProcedimiento", "</Procedure>");
            RegexBasedTerminal ATabla = new RegexBasedTerminal("ATabla", "<Tabla>");
            RegexBasedTerminal CTabla = new RegexBasedTerminal("CTabla", "</Tabla>");
            RegexBasedTerminal Arow = new RegexBasedTerminal("Arow", "<rows>");
            RegexBasedTerminal Crow = new RegexBasedTerminal("Crow", "</rows>");
            RegexBasedTerminal Arows = new RegexBasedTerminal("Arows", "<Row>");
            RegexBasedTerminal Crows = new RegexBasedTerminal("Crows", "</Row>");
            RegexBasedTerminal AProc = new RegexBasedTerminal("AProc", "<Proc>");
            RegexBasedTerminal CProc = new RegexBasedTerminal("CProc", "</Proc>");
            RegexBasedTerminal Asrc = new RegexBasedTerminal("Asrc", "<src>");
            RegexBasedTerminal Csrc = new RegexBasedTerminal("Csrc", "</src>");
            RegexBasedTerminal AObj = new RegexBasedTerminal("AObj", "<Obj>");
            RegexBasedTerminal CObj = new RegexBasedTerminal("CObj", "</Obj>");
            RegexBasedTerminal Aattr = new RegexBasedTerminal("Aattr", "<attr>");
            RegexBasedTerminal Cattr = new RegexBasedTerminal("Cattr", "</attr>");
            RegexBasedTerminal Aparams = new RegexBasedTerminal("Aparams", "<params>");
            RegexBasedTerminal Cparams = new RegexBasedTerminal("Cparams", "</params>");
            RegexBasedTerminal Atipo = new RegexBasedTerminal("Atipo", "<tipo>");
            RegexBasedTerminal Ctipo = new RegexBasedTerminal("Ctipo", "</tipo>");
            RegexBasedTerminal AHistoria = new RegexBasedTerminal("AHistoria", "<Historia>");
            RegexBasedTerminal CHistoria = new RegexBasedTerminal("CHistoria", "</Historia>");
            RegexBasedTerminal Ratributos = new RegexBasedTerminal("Ratributos", "Atributos:");

            NonTerminal S = new NonTerminal("S"),
                inicio = new NonTerminal("inicio"),
                Maestro = new NonTerminal("Maestro"),
                bases = new NonTerminal("bases"),
                usuario = new NonTerminal("usuario"),
                Dobjetos = new NonTerminal("Dobjetos"),
                Dprocedimientos = new NonTerminal("Dprocedimientos"),
                Dtablas = new NonTerminal("Dtablas"),
                cuerpobase = new NonTerminal("cuerpobase"),
                detallesbase = new NonTerminal("detallesbase"),
                cuerpousuario = new NonTerminal("cuerpousuario"),
                cuerpotabla = new NonTerminal("cuerpotabla"),
                nombre = new NonTerminal("nombre"),
                row = new NonTerminal("row"),
                rows = new NonTerminal("rows"),
                campos = new NonTerminal("campos"),
                ruta = new NonTerminal("ruta"),
                contra = new NonTerminal("contra"),
                permiso = new NonTerminal("permiso"),
                procedimientos = new NonTerminal("procedimientos"),
                procedimiento = new NonTerminal("procedimiento"),
                Objetos = new NonTerminal("Objetos"),
                Objeto = new NonTerminal("Objeto"),
                Tablas = new NonTerminal("Tablas"),
                instruccion = new NonTerminal("instruccion"),
                historia = new NonTerminal("historia"),
                tipof = new NonTerminal("tipof"),
                parametros = new NonTerminal("parametros"),
                atrrib = new NonTerminal("atrrib"); ;


            S.Rule = inicio;

            inicio.Rule = Maestro
                        | detallesbase
                        | procedimientos
                        | Objetos
                        | Tablas;

            

            Maestro.Rule = bases + usuario
                     | bases
                     | usuario;

            detallesbase.Rule = historia + Dprocedimientos + Dobjetos + Dtablas
                              | historia + Dprocedimientos + Dobjetos
                              | historia + Dprocedimientos + Dtablas
                              | historia + Dobjetos + Dtablas
                              | historia + Dprocedimientos
                              | historia + Dobjetos
                              | historia + Dtablas;



            bases.Rule = ABAse + cuerpobase + CBAse + bases
                        | ABAse + cuerpobase + CBAse;

            usuario.Rule = AUsuario + cuerpousuario + CUsuario + usuario
                        | AUsuario + cuerpousuario + CUsuario;

            cuerpobase.Rule = nombre + ruta;

            nombre.Rule = ANombre + ID + CNombre;

            ruta.Rule = ARuta + Cadena+ CRuta;

            cuerpousuario.Rule = nombre + contra + permiso;

            contra.Rule = AContra + Cadena + CContra;

            permiso.Rule = APermisos + Cadena + CPermisos;

            Dobjetos.Rule =AObject+ ruta+CObject;

            Dprocedimientos.Rule = AProcedimiento + ruta + CProcedimiento;

            Dtablas.Rule = ATabla + cuerpotabla + CTabla + Dtablas
                        | ATabla + cuerpotabla + CTabla;

            cuerpotabla.Rule = nombre + ruta + row;

            row.Rule = Arow + campos + Crow;

            rows.Rule = Arows + campos + Crows;

            campos.Rule = "<" + ID + Ratributos + Atributos + ">" + Cadena + "</" + ID + ">" + campos//10
                        | "<" + ID + ">" + Cadena + "</" + ID + ">" + campos//8
                        | "<" + ID + Ratributos + Atributos + ">" + Cadena + "</" + ID + ">"//9
                        | "<" + ID + ">" + Cadena + "</" + ID + ">";//7

            procedimientos.Rule = AProc + procedimiento + CProc + procedimientos
                               | AProc + procedimiento + CProc;

            procedimiento.Rule = nombre + tipof + parametros + instruccion 
                               | nombre + tipof + instruccion
                               | nombre + parametros + instruccion
                               | nombre + instruccion;

            Objetos.Rule = AObj + Objeto + CObj + Objetos
                         | AObj + Objeto + CObj;

            Objeto.Rule = nombre + atrrib;

            instruccion.Rule = Asrc + Cadena + Csrc;

            parametros.Rule = Aparams + campos + Cparams;

            tipof.Rule = Atipo + ID + Ctipo;

            atrrib.Rule = Aattr + campos + Cattr;

            Tablas.Rule = rows + Tablas
                        | rows;

            historia.Rule = AHistoria + Cadena + CHistoria;

            this.Root = S;


        }
    }
}
