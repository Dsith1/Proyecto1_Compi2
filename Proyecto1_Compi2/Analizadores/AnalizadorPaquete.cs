using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Proyecto1_Compi2.Analizadores
{
    class AnalizadorPaquete:Grammar
    {

        public AnalizadorPaquete() : base(false)
        {
            //Reservadas

            RegexBasedTerminal INICIO = new RegexBasedTerminal("Inicio", "[");
            RegexBasedTerminal FIN = new RegexBasedTerminal("Fin", "]");
            RegexBasedTerminal COMA = new RegexBasedTerminal("COMA", ",");

            RegexBasedTerminal PAQUETE = new RegexBasedTerminal("PAQUETE", "\"paquete\"");
            RegexBasedTerminal USQL = new RegexBasedTerminal("USQL  ", "\"usql\"");
            RegexBasedTerminal INSTRUCCION = new RegexBasedTerminal("INSTRUCCION  ", "\"instrucción\"");
            RegexBasedTerminal REPORTE = new RegexBasedTerminal("REPORTE", "\"reporte\"");
            RegexBasedTerminal COMANDO = new RegexBasedTerminal("COMANDO", "\"comando\"");
            RegexBasedTerminal VALIDAR = new RegexBasedTerminal("VALIDAR", "\"validar\"");
            RegexBasedTerminal RFIN = new RegexBasedTerminal("RFIN", "\"fin\"");
            RegexBasedTerminal LOGIN = new RegexBasedTerminal("LOGIN", "\"login\"");
            RegexBasedTerminal FLECHA = new RegexBasedTerminal("FLECHA", "=>");


            //Datos
            StringLiteral DATOS = new StringLiteral("DATOS", "\'");
            NumberLiteral Entero = new NumberLiteral("entero");

            //No Terminales
            NonTerminal S = new NonTerminal("S"),
                        inicio = new NonTerminal("inicio"),
                        cuerpo = new NonTerminal("cuerpo"),
                        login = new NonTerminal("login"),
                        paquete = new NonTerminal("paquete"),
                        sublogin = new NonTerminal("sublogin"),
                        fin = new NonTerminal("fin"),
                        usql = new NonTerminal("usql");

            S.Rule=inicio;

            inicio.Rule = INICIO + cuerpo + FIN;

            cuerpo.Rule = login
                        | paquete;

            login.Rule = VALIDAR + ":" + Entero + COMA + sublogin;

            sublogin.Rule = LOGIN + ":" + INICIO + COMANDO + FLECHA + DATOS + FIN;


            paquete.Rule = PAQUETE + ":" + fin
                         | PAQUETE + ":" + usql;

            fin.Rule = FIN;

            usql.Rule = USQL + COMA + INSTRUCCION + ":" + DATOS + COMA;

        }
    }
}
