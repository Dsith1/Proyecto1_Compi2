using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Xml;
using System.Xml.Linq;

namespace Proyecto1_Compi2.Elementos
{
    class Manejo
    {
        Maestro maestro;

        public string usuario;

        public Manejo()
        {
            string activeDir = @"c:\";
            string newPath = System.IO.Path.Combine(activeDir, "DBMS");

            if (Directory.Exists(newPath) == false) { }
            {
                System.IO.Directory.CreateDirectory(newPath);
            }

        }

        public string Crear_Maestro()
        {
            maestro = new Maestro();
            

            return "Archivo Maestro Creado";

            //string activeDir = @"c:\DBMS";

            //string newPath = System.IO.Path.Combine(activeDir, "Maestro.usac");

            //if (File.Exists(newPath) == false)
            //{
            //    System.IO.FileStream fs = System.IO.File.Create(newPath);

            //    fs.Close();
            //    return "Archivo Maestro Creado" ;

            //    //Crear_Base("BMaestra") + "\r\n" + Crear_Tabla("Usuario", "BMaestra")


            //}
            //else
            //{
            //    return "";
            //}


        }
        
        public string Crear_Usuario(string nombre,string contra)
        {

            if (usuario.Equals("Admin"))
            {
                if (maestro.usuarios.existe(nombre))
                {
                    return "El Usuario " + nombre + " Ya Existe";
                }
                else
                {
                    Usuario nuevo = new Usuario(nombre, contra);

                    maestro.usuarios.Insertar(nuevo);

                    return "Se Ha Creado el Usuario " + nombre;
                }
            }
            else
            {
                return "Usted No puede Crear Usuarios";
            }

            

            
        }

        public string Modificar_Usuario(string nombre,string contra)
        {
            string respuesta = "";

            if (maestro.usuarios.existe(nombre))
            {
                maestro.usuarios.aux.Contraseña = contra;

                respuesta = "\r\nUsuario: " + nombre+" Actualizado";
            }
            else
            {
                respuesta = "\r\nNo existe el Usuario: " + nombre;
            }

            return respuesta;
        }

        public string Eliminar_Usuario(string nombre)
        {
            string respuesta = "";

            if (maestro.usuarios.existe(nombre))
            {
                maestro.usuarios.Eliminar(nombre);
                

                respuesta = "\r\nUsuario: " + nombre + " Eliminado";
            }
            else
            {
                respuesta = "\r\nNo existe el Usuario: " + nombre;
            }

            return respuesta;
        }

        public string Crear_Base(string nombre)
        {

            if (maestro.bases.existe(nombre))
            {
                return "La Base de Datos " + nombre + " Ya Existe";
            }
            else
            {
                Base nuevo = new Base(nombre);

                maestro.bases.Insertar(nuevo);

                if (usuario.Equals("Admin"))
                {
                    maestro.usuarios.existe(usuario);

                    maestro.usuarios.aux.permiso += nombre;
                }
                else
                {
                    maestro.usuarios.existe(usuario);

                    maestro.usuarios.aux.permiso += nombre;

                    maestro.usuarios.existe("Admin");

                    maestro.usuarios.aux.permiso += nombre;
                }

                return "Se Ha Creado La Base de Datos " + nombre;
            
            }
            
        }

        public string Eliminar_Base(string nombre)
        {
            string respuesta = "";

            if (maestro.bases.existe(nombre))
            {
                maestro.bases.Eliminar(nombre);


                respuesta = "\r\nUsuario: " + nombre + " Eliminado";
            }
            else
            {
                respuesta = "\r\nNo existe el Usuario: " + nombre;
            }

            return respuesta;
        }

        public string Crear_Tabla(string nombre,string Base,string campos)
        {

            maestro.bases.existe(Base);

            if (GetPermiso(usuario, Base)){

                if (maestro.bases.aux.tablas.existe(nombre))
                {
                    return "Ya Existe la Tabla " + nombre + " En la Base " + Base;
                }
                else
                {

                    string tipos = "";
                    string fields = "";
                    string atrib = "";

                    string[] campo = campos.Split(';');

                    for (int x = 0; x < campo.Length; x++)
                    {
                        string[] dato = campo[x].Split(',');

                        tipos += dato[0] + ",";
                        fields += dato[1] + ",";

                        for(int y = 2; y < dato.Length; y++)
                        {
                            atrib += dato[y] + ";";
                        }
                        atrib = atrib.Trim(';');
                        atrib += ",";
                    }

                    tipos = tipos.Trim(',');
                    fields = fields.Trim(',');
                    atrib = atrib.Trim(',');

                    if (atrib.Contains("Llave_Foranea"))
                    {
                        string[] atributos = atrib.Split(',');

                        int ba = 0;
                        bool seguir = true;

                        while (seguir)
                        {
                            if (atributos[ba].Contains("Llave_Foranea"))
                            {
                                seguir = false;
                            }
                            else
                            {
                                ba++;
                            }
                        }

                        string[] subatrib = atributos[ba].Split(' ');

                        string baseR = subatrib[subatrib.Length - 1];

                        if (maestro.bases.aux.tablas.existe(baseR))
                        {
                            int pllave = getPLlave(maestro.bases.aux.tablas.aux);

                            string tipoR = getTipoLlave(maestro.bases.aux.tablas.aux, pllave);

                            if (tipos.Split(',')[ba].Equals(tipoR))
                            {
                                Tabla nuevo = new Tabla(nombre, tipos, fields);

                                nuevo.atributos = atrib;

                                nuevo.SetRuta(System.IO.Path.Combine(@"c:\DBMS", Base + "_" + nombre + ".usac"));

                                maestro.bases.aux.tablas.Insertar(nuevo);

                                return "Se Ha Creado La Tabla " + nombre + " En la Base de Datos " + Base;
                            }
                            else
                            {
                                return "La Llave foranea es de distinto tipo de dato";
                            }

                            
                        }
                        else
                        {
                            return "No Existe la Base "+baseR+" para hacer referencia";
                        }

                        
                    }
                    else
                    {
                        Tabla nuevo = new Tabla(nombre, tipos, fields);

                        nuevo.atributos = atrib;

                        nuevo.SetRuta(System.IO.Path.Combine(@"c:\DBMS", Base + "_" + nombre + ".usac"));

                        maestro.bases.aux.tablas.Insertar(nuevo);

                        return "Se Ha Creado La Tabla " + nombre + " En la Base de Datos " + Base;
                    }

                  
                }
            }
            else
            {
                return "Usted no tiene Permisos sobre la Base:" + Base;
            }

            


        }

        public string Eliminar_Tabla(string nombre,string Base)
        {
            maestro.bases.existe(Base);
            string respuesta = "";

            if (maestro.bases.aux.tablas.existe(nombre))
            {
                maestro.bases.aux.tablas.Eliminar(nombre);


                respuesta = "\r\nUsuario: " + nombre + " Eliminado";
            }
            else
            {
                respuesta = "\r\nNo existe el Usuario: " + nombre;
            }

            return respuesta;
        }

        public string Insertar_Tabla(string valores,string Base, string Tabla)
        {
            maestro.bases.existe(Base);

            if (maestro.bases.aux.tablas.existe(Tabla))
            {

                valores = valores.Replace("entero", "INTEGER");
                valores = valores.Replace("Cadena", "TEXT");

                string[] val = valores.Split(',');

                string subt="";
                string subv = "";

                for (int x = 0; x < val.Length; x++)
                {
                    string[] aux = val[x].Split(';');

                    subt += aux[0] + ",";
                    subv += aux[1] + ",";
                }

                subt = subt.Trim(',');
                subv = subv.Trim(',');

                subv = subv.Replace("\"", "");


                string tipos = maestro.bases.aux.tablas.aux.tipos;
                string atrib = maestro.bases.aux.tablas.aux.atributos;

                if (tipos.Equals(subt))
                {
                    Registro nuevo = new Registro(subv);

                    maestro.bases.aux.tablas.aux.Insertar(nuevo);

                    return "1 Fila Insertada";
                }
                else if (atrib.Contains("Autoincrementable"))
                {

                    int pos = possIncrementable(atrib);

                    int r = getSiguiente(maestro.bases.aux.tablas.aux, pos);

                    string[] vals = subv.Split(',');
                    string[] tips = subt.Split(',');

                    subv = "";
                    subt = "";

                    for (int x = 0; x < pos; x++)
                    {
                        subv += vals[x]+",";
                        subt += tips[x] + ",";
                    }

                    subv += r + ",";
                    subt += "INTEGER,";

                    for (int x = pos+1; x <= vals.Length; x++)
                    {
                        subv += vals[x-1] + ",";
                        subt += tips[x - 1] + ",";
                    }

                    subv = subv.Trim(',');
                    subt = subt.Trim(',');

                    if (subv.Split(',').Length ==subt.Split(',').Length)
                    {
                        Registro nuevo = new Registro(subv);

                        maestro.bases.aux.tablas.aux.Insertar(nuevo);

                        return "1 Fila Insertada";
                    }
                    else
                    {
                        return "Faltan Argumentos";
                    }

                   
                }
                else
                {


                    return "Error de Tipos se Esperaba(" + tipos + ") Se Encontro(" + subt + ")";
                }

                

                
            }
            else
            {

                return "No Existe la Tabla " + Tabla + " en La Base " + Base;
            }

            
        }

        public string Insertar_Tabla_v(string valores,string parametros, string Base, string Tabla)
        {

            maestro.bases.existe(Base);

            if (maestro.bases.aux.tablas.existe(Tabla))
            {

                string[] parametro = parametros.Split(',');

                string campos= maestro.bases.aux.tablas.aux.campos;

                bool encontrado = false;

                for (int x = 0; x < parametro.Length; x++)
                {

                    if (campos.Contains(parametro[x]))
                    {
                        encontrado = true;
                    }
                    else
                    {
                        encontrado = false;
                        break;
                    }
                }

                if (encontrado)
                {
                    string[] campo = campos.Split(',');

                    string[] val = valores.Split(',');

                    string tipos = maestro.bases.aux.tablas.aux.tipos;

                    string atributos = maestro.bases.aux.tablas.aux.atributos;

                    string[] tipo = tipos.Split(',');

                    string subval = "";

                    int auxF = 0;

                    for (int x = 0; x < parametro.Length; x++)
                    {
                        for(int y = auxF; y < campo.Length; y++)
                        {
                            if (parametro[x].Equals(campo[y])){
                                subval += val[x]+",";

                                auxF = y+1;
                                break;
                            }
                            else
                            {
                                subval +=  "null;null,";
                            }
                        }
                    }


                    valores = subval.Trim(',');

                    valores = valores.Replace("entero", "INTEGER");
                    valores = valores.Replace("Cadena", "TEXT");

                    

                    if (auxF < campo.Length)
                    {
                        bool seguir = true;

                        while (seguir)
                        {

                            if (auxF < campo.Length)
                            {
                                valores += "," + tipo[auxF] + ";null";
                                auxF++;
                            }
                            else
                            {
                                seguir = false;
                            }
                        }
                    }

                    val = valores.Split(',');

                    string subt = "";
                    string subv = "";

                    for (int x = 0; x < val.Length; x++)
                    {
                        string[] aux = val[x].Split(';');

                        subt += aux[0] + ",";
                        subv += aux[1] + ",";
                    }

                    subt = subt.Trim(',');
                    subv = subv.Trim(',');

                    

                    if (tipos.Equals(subt))
                    {
                        Registro nuevo = new Registro(subv);

                        maestro.bases.aux.tablas.aux.Insertar(nuevo);

                        return "1 Fila Insertada";
                    }
                    else if (atributos.Contains("Autoincrementable"))
                    {

                        int pos = possIncrementable(atributos);

                        int r = getSiguiente(maestro.bases.aux.tablas.aux, pos);

                        string[] vals = subv.Split(',');

                        subv = "";

                        for (int x = 0; x < pos; x++)
                        {
                            subv += vals[x] + ",";
                        }

                        subv += r + ",";

                        for (int x = pos+1; x < vals.Length; x++)
                        {
                            subv += vals[x] + ",";
                        }

                        subv = subv.Trim(',');

                        Registro nuevo = new Registro(subv);

                        maestro.bases.aux.tablas.aux.Insertar(nuevo);

                        return "1 Fila Insertada";
                    }
                    else
                    {
                        return "Error de Tipos se Esperaba(" + tipos + ") Se Encontro(" + subt + ")";
                    }
                }
                else
                {
                    return "Error Campos inexistentes en la Tabla " + Tabla;
                }


                




            }
            else
            {

                return "No Existe la Tabla " + Tabla + " en La Base " + Base;
            }


        }

        public string Actualizar_Tabla(string Base,string Tabla,string campos,string valores,string condicion)
        {

            int contador = 0;

            maestro.bases.existe(Base);
            maestro.bases.aux.tablas.existe(Tabla);

            string encabezados = getcamposTabla(Tabla);
            string posiciones = "";

            string[] campo = campos.Split(',');

            string[] encabezado = encabezados.Split(',');

            for(int x = 0; x < encabezado.Length; x++)
            {
                for(int y = 0; y < campo.Length; y++)
                {
                    if (encabezado[x].Equals(campo[y])){
                        posiciones += x+",";
                    }
                }
            }

            posiciones = posiciones.Trim(',');

            string[] posicion = posiciones.Split(',');

            int[] pos = Array.ConvertAll(posicion, int.Parse);

            string[] nuevov = valores.Split(',');

            if (condicion.Equals(""))
            {
                bool seguir = true;

                maestro.bases.aux.tablas.aux.aux = maestro.bases.aux.tablas.aux.cabeza;


                while (seguir)
                {

                    string registro = maestro.bases.aux.tablas.aux.aux.valor;

                    string[] valor = registro.Split(',');

                    string insercion = "";

                    for(int x = 0; x < valor.Length; x++)
                    {
                        if (pos.Contains(x))
                        {
                            int lugar = Array.IndexOf(pos, x);

                            insercion += nuevov[lugar].Split(';')[1] + ",";
                        }
                        else
                        {
                            insercion += valor[x] + ",";
                        }

                        maestro.bases.aux.tablas.aux.aux.valor = insercion.Trim(',');
                        
                    }
                    contador++;

                    if (maestro.bases.aux.tablas.aux.aux.siguiente != null)
                    {
                        maestro.bases.aux.tablas.aux.aux = maestro.bases.aux.tablas.aux.aux.siguiente;
                    }
                    else
                    {
                        seguir = false;
                    }
                    

                }

            }
            else
            {
                string evaluarcond = Select(Base, Tabla, "*", "", condicion);

                string[] beta = evaluarcond.Split(new string[] { ";;" }, StringSplitOptions.None);

                string[] filas = beta[1].Split(';');

                string[] bakcup = beta[1].Split(';');

                for(int y = 0; y < filas.Length-1; y++)
                {
                    string registro = filas[y];

                    string[] valor = registro.Split(',');

                    string insercion = "";

                    for (int x = 0; x < valor.Length; x++)
                    {
                        if (pos.Contains(x))
                        {
                            int lugar = Array.IndexOf(pos, x);

                            insercion += nuevov[lugar].Split(';')[1] + ",";
                        }
                        else
                        {
                            insercion += valor[x] + ",";
                        }

                        

                    }
                    filas[y] = insercion.Trim(',');
                    contador++;
                }


                maestro.bases.aux.tablas.aux.aux = maestro.bases.aux.tablas.aux.cabeza;
                bool seguir = true;

                while (seguir)
                {

                    for (int x = 0; x < filas.Length - 1; x++)
                    {
                        if (maestro.bases.aux.tablas.aux.aux.valor.Contains(bakcup[x]))
                        {
                            maestro.bases.aux.tablas.aux.aux.valor = filas[x];
                        }

                    }


                    if (maestro.bases.aux.tablas.aux.aux.siguiente != null)
                    {
                        maestro.bases.aux.tablas.aux.aux = maestro.bases.aux.tablas.aux.aux.siguiente;
                    }
                    else
                    {
                        seguir = false;
                    }
                }
            }

            return "Se Modificaron "+ contador+" filas";
        }

        public string Crear_Objeto(string nombre, string Base, string campos)
        {
            maestro.bases.existe(Base);

            if (GetPermiso(usuario, Base))
            {

                if (maestro.bases.aux.objetos.existe(nombre))
                {
                    return "Ya Existe el Objeto " + nombre + " En la Base " + Base;
                }
                else
                {

                    string tipos = "";
                    string fields = "";

                    string[] campo = campos.Split(';');

                    for (int x = 0; x < campo.Length; x++)
                    {
                        string[] dato = campo[x].Split(',');

                        tipos += dato[1] + ",";
                        fields += dato[0] + ",";
                    }

                    tipos = tipos.Trim(',');
                    fields = fields.Trim(',');

                    Objeto nuevo = new Objeto(nombre, tipos, fields);

                    maestro.bases.aux.objetos.Insertar(nuevo);

                    return "Se Ha Creado La Tabla " + nombre + " En la Base de Datos " + Base;
                }
            }
            else
            {
                return "Usted no tiene Permisos sobre la Base:" + Base;
            }

        }

        public string Eliminar_Objeto(string nombre, string Base)
        {
            maestro.bases.existe(Base);
            string respuesta = "";

            if (maestro.bases.aux.objetos.existe(nombre))
            {
                maestro.bases.aux.objetos.Eliminar(nombre);


                respuesta = "\r\nObjeto: " + nombre + " Eliminado";
            }
            else
            {
                respuesta = "\r\nNo existe el Usuario: " + nombre;
            }

            return respuesta;
        }

        public bool Buscar_Objeto(string nombre,string Base)
        {
            maestro.bases.existe(nombre);

            return maestro.bases.aux.objetos.existe(nombre);
        }

        public string Quitar_Objeto(string nombre, string Base, string campo)
        {
            maestro.bases.existe(Base);

            if (maestro.bases.aux.objetos.existe(nombre))
            {
                string campos = maestro.bases.aux.objetos.aux.campos;
                string tipos = maestro.bases.aux.objetos.aux.tipos;

                string[] subcampo = campos.Split(',');
                string[] subtipos = tipos.Split(',');

                string ncampos = "";
                string ntipos = "";





                    for (int x = 0; x < subcampo.Length; x++)
                    {
                        if (subcampo[x].Equals(campo))
                        {

                        }
                        else
                        {
                            ncampos += subcampo[x]+",";
                            ntipos += subtipos[x] + ",";
                        }
                    }
                

                ncampos = ncampos.Trim(',');
                ntipos = ntipos.Trim(',');

                maestro.bases.aux.objetos.aux.campos = ncampos;
                maestro.bases.aux.objetos.aux.tipos = ntipos;

                return "Objeto:"+nombre+" modificado";
            }
            else
            {
                return "NO Existe el Objeto:"+nombre;
            }

            
        }

        public string Agregar_Objeto(string nombre, string Base, string campo)
        {
            maestro.bases.existe(Base);

            if (maestro.bases.aux.objetos.existe(nombre))
            {
                string campos = maestro.bases.aux.objetos.aux.campos;
                string tipos = maestro.bases.aux.objetos.aux.tipos;

                string[] ncampos = campo.Split(';');

                for (int x = 0; x < ncampos.Length; x++)
                {
                    string[] valor = ncampos[x].Split(',');

                    campos += "," + valor[1];
                    tipos += "," + valor[0];
                }

                campos = campos.Trim(',');
                tipos = tipos.Trim(',');

                maestro.bases.aux.objetos.aux.campos = campos;
                maestro.bases.aux.objetos.aux.tipos = tipos;

                return "Objeto:" + nombre + " modificado";
            }
            else
            {
                return "NO Existe el Objeto:" + nombre;
            }


        }

        public string Crear_Procedimiento(string nombre, string Base, string campos, string instrucciones)
        {

            maestro.bases.existe(Base);

            if (GetPermiso(usuario, Base))
            {

                if (maestro.bases.aux.objetos.existe(nombre))
                {
                    return "Ya Existe el Objeto " + nombre + " En la Base " + Base;
                }
                else
                {

                    if (maestro.bases.aux.procedimientos.existe(nombre))
                    {
                        return "\r\nYa Existe el Procedimiento:" + nombre;
                    }
                    else
                    {

                        if (campos.Equals(""))
                        {
                            Procedimiento nuevo = new Procedimiento(nombre, instrucciones);

                            maestro.bases.aux.procedimientos.Insertar(nuevo);


                            return "\r\nSe ha Creado el Procedimiento:" + nombre.Trim('_');

                        }
                        else
                        {

                            string[] campo = campos.Split(';');

                            string tipos = "";
                            string parametros = "";

                            for (int x = 0; x < campo.Length; x++)
                            {
                                string[] val = campo[x].Split(',');
                                tipos += val[0] + ",";
                                parametros += val[1] + ",";

                            }

                            tipos = tipos.Trim(',');
                            parametros = parametros.Trim(',');

                            string subnombre = nombre + tipos;

                            subnombre = subnombre.Replace(',', '_');


                            Procedimiento nuevo = new Procedimiento(subnombre, instrucciones);

                            nuevo.tipos = tipos;
                            nuevo.campos = parametros;

                            maestro.bases.aux.procedimientos.Insertar(nuevo);


                            return "\r\nSe ha Creado el Procedimiento:" + nombre.Trim('_');
                        }


                    }
                }
            }
            else
            {
                return "Usted no tiene Permisos sobre la Base:" + Base;
            }

            

        }

        public string Crear_funcion(string nombre, string Base, string campos, string instrucciones,string tipo)
        {

            maestro.bases.existe(Base);

            if (GetPermiso(usuario, Base))
            {

                if (maestro.bases.aux.objetos.existe(nombre))
                {
                    return "Ya Existe el Objeto " + nombre + " En la Base " + Base;
                }
                else
                {

                    if (maestro.bases.aux.procedimientos.existe(nombre))
                    {
                        return "\r\nYa Existe el Procedimiento:" + nombre;
                    }
                    else
                    {

                        if (campos.Equals(""))
                        {
                            Procedimiento nuevo = new Procedimiento(nombre, instrucciones);
                            nuevo.funcion = true;
                            nuevo.tipo = tipo;


                            maestro.bases.aux.procedimientos.Insertar(nuevo);


                            return "\r\nSe ha Creado el Procedimiento:" + nombre.Trim('_');

                        }
                        else
                        {

                            string[] campo = campos.Split(';');

                            string tipos = "";
                            string parametros = "";

                            for (int x = 0; x < campo.Length; x++)
                            {
                                string[] val = campo[x].Split(',');
                                tipos += val[0] + ",";
                                parametros += val[1] + ",";

                            }

                            tipos = tipos.Trim(',');
                            parametros = parametros.Trim(',');
                        
                            string subnombre = nombre +"_"+ tipos;

                            subnombre = subnombre.Replace(',', '_');


                            Procedimiento nuevo = new Procedimiento(subnombre, instrucciones);

                            nuevo.tipos = tipos;
                            nuevo.campos = parametros;

                            nuevo.funcion = true;
                            nuevo.tipo = tipo;

                            maestro.bases.aux.procedimientos.Insertar(nuevo);


                            return "\r\nSe ha Creado el Procedimiento:" + nombre.Trim('_');
                        }

                    }
                }
            }
            else
            {
                return "Usted no tiene Permisos sobre la Base:" + Base;
            }



            

        }

        public string EjecutarProcedimeinto(string nombre,string Base)
        {
            maestro.bases.existe(Base);

            if (maestro.bases.aux.procedimientos.existe(nombre))
            {
                return maestro.bases.aux.procedimientos.aux.instrucciones;
            }
            else
            {
                return "\r\nNa Existe el Procedimiento:" + nombre;
            }

        }

        public string ProcedimientoParam(string nombre, string Base) 
        {
            maestro.bases.existe(Base);

            if (maestro.bases.aux.procedimientos.existe(nombre))
            {
                return maestro.bases.aux.procedimientos.aux.campos+";"+ maestro.bases.aux.procedimientos.aux.tipos;
            }
            else
            {
                return "\r\nNa Existe el Procedimiento:" + nombre;
            }

        }

        public bool EsFuncion(string nombre,string Base)
        {
            maestro.bases.existe(Base);

            if (maestro.bases.aux.procedimientos.existe(nombre))
            {
                return maestro.bases.aux.procedimientos.aux.funcion;
            }
            else
            {
                return false;
            }

        }

        public bool EsFuncion(string nombre, string Base, string parametros)
        {
            maestro.bases.existe(Base);

            if (maestro.bases.aux.procedimientos.existe(nombre))
            {
                return maestro.bases.aux.procedimientos.aux.funcion;
            }
            else
            {
                return false;
            }

        }

        public string getTipoF(string nombre, string Base)
        {
            maestro.bases.existe(Base);

            if (maestro.bases.aux.procedimientos.existe(nombre))
            {
                return maestro.bases.aux.procedimientos.aux.tipo;
            }
            else
            {
                return null;
            }
        }

        public bool GetPermiso(string usuario, string objeto)
        {

            maestro.usuarios.existe(usuario);

            if (maestro.usuarios.aux.permiso.Contains(objeto))
            {
                return true;
            }
            else
            {
                return false;
            }

            
        }

        public int possIncrementable(string ATabla)
        {
            string[] atrib = ATabla.Split(',');

            int x = 0;
            bool seguir = true;

            while (seguir)
            {
                if (atrib[x].Contains("Autoincrementable"))
                {
                    seguir = false;
                }
                else
                {
                    x++;
                }
            }

            return x;
        }

        public int getSiguiente(Tabla aux,int pos)
        {
            if (aux.cabeza == null)
            {
                return 1;
            }
            else if(aux.ultimo==null)
            {
                return 2;
            }
            else
            {
                string atribs = aux.ultimo.valor;

                string[] atrib = atribs.Split(',');

                int t = Convert.ToInt32(atrib[pos]);

                t++;

                return t;
            }
        }

        int getPLlave(Tabla aux)
        {
            string atributos = aux.atributos;

            string[] atributo = atributos.Split(',');

            int x = 0;
            bool seguir = true;

            while (seguir)
            {
                if (atributo[x].Contains("Llave_Primaria"))
                {
                    seguir = false;
                }
                else
                {
                    if (x < atributo.Length)
                    {
                        x++;
                    }
                    else
                    {
                        x = -1;
                        seguir = false;
                    }
                }
            }

            return x;

        }

        string getTipoLlave(Tabla aux,int pos)
        {
            string[] tipos = aux.tipos.Split(',');

            return tipos[pos];
        }

        public string Select(string Base, string Tablas, string campos, string orden, string condicion)
        {
            maestro.bases.existe(Base);
            string registros="";
            string encabezado="";

            if (Tablas.Contains(','))
            {
                string[] subT = Tablas.Split(',');
                registros = getRegistrosTabla(subT[0]);

                for(int x = 1; x < subT.Length; x++)
                {
                    string[] subR1 = registros.Split(';');

                    string registros2= getRegistrosTabla(subT[x]);

                    string[] subR2 = registros2.Split(';');

                    registros = "";

                    for(int y = 0; y < subR1.Length - 1; y++)
                    {
                        for (int j = 0; j < subR2.Length - 1; j++)
                        {
                            registros += subR1[y] + "," + subR2[j] + ";";
                        }

                    }

                }


                for (int x = 0; x < subT.Length; x++)
                {
                    encabezado += getcamposTabla(subT[x]) + ",";
                }

                encabezado = encabezado.Trim(',');


            }
            else
            {

                encabezado = getcamposTabla(Tablas);
                registros = getRegistrosTabla(Tablas);
                
            }

            //condicion
            if(condicion.Contains("||") || condicion.Contains("&&"))
            {
                if (condicion.Contains("||") && condicion.Contains("&&")==false){

                    string[] condiciones = condicion.Split(new string[] { "||" }, StringSplitOptions.None);

                    string subregistros;

                    string resultado = "";


                    for(int y = 0; y < condiciones.Length; y++)
                    {
                        subregistros = registros;

                        if (condiciones[y].Contains("=="))
                        {
                            string[] datos = condiciones[y].Split(new string[] { "==" }, StringSplitOptions.None);

                            int pc = 0;

                            

                            for (int x = 0; x < datos.Length; x++)
                            {
                                if (encabezado.Contains(datos[x]))
                                {
                                    pc = x;
                                    break;
                                }
                            }

                            datos[pc] = datos[pc].Trim();

                            int pc2 = datos.Length - 1 - pc;

                            string[] ordenE = encabezado.Split(',');

                            int po = 0;

                            for (int x = 0; x < ordenE.Length; x++)
                            {
                                if (ordenE[x].Equals(datos[pc]))
                                {
                                    po = x;
                                    break;
                                }
                            }


                            if (encabezado.Contains(datos[pc2]))
                            {
                                datos[pc2] = datos[pc2].Trim();

                                int po2 = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(datos[pc2]))
                                    {
                                        po2 = x;
                                        break;
                                    }
                                }

                                subregistros = filtro_join(datos[pc], po, subregistros, 1, po2);

                            }
                            else
                            {
                                subregistros = filtro(datos[pc], po, subregistros, 1, datos[pc2]);
                            }



                        }
                        else if (condiciones[y].Contains("!="))
                        {
                            string[] datos = condiciones[y].Split(new string[] { "!=" }, StringSplitOptions.None);

                            int pc = 0;

                            for (int x = 0; x < datos.Length; x++)
                            {
                                if (encabezado.Contains(datos[x]))
                                {
                                    pc = x;
                                    break;
                                }
                            }

                            datos[pc] = datos[pc].Trim();

                            int pc2 = datos.Length - 1 - pc;

                            string[] ordenE = encabezado.Split(',');

                            int po = 0;

                            for (int x = 0; x < ordenE.Length; x++)
                            {
                                if (ordenE[x].Equals(datos[pc]))
                                {
                                    po = x;
                                    break;
                                }
                            }


                            if (encabezado.Contains(datos[pc2]))
                            {
                                datos[pc2] = datos[pc2].Trim();

                                int po2 = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(datos[pc2]))
                                    {
                                        po2 = x;
                                        break;
                                    }
                                }

                                subregistros = filtro_join(datos[pc], po, subregistros, 2, po2);

                            }
                            else
                            {
                                subregistros = filtro(datos[pc], po, subregistros, 2, datos[pc2]);
                            }
                        }
                        else if (condiciones[y].Contains("<="))
                        {
                            string[] datos = condiciones[y].Split(new string[] { "<=" }, StringSplitOptions.None);

                            int pc = 0;

                            for (int x = 0; x < datos.Length; x++)
                            {
                                if (encabezado.Contains(datos[x]))
                                {
                                    pc = x;
                                    break;
                                }
                            }

                            datos[pc] = datos[pc].Trim();

                            int pc2 = datos.Length - 1 - pc;

                            string[] ordenE = encabezado.Split(',');

                            int po = 0;

                            for (int x = 0; x < ordenE.Length; x++)
                            {
                                if (ordenE[x].Equals(datos[pc]))
                                {
                                    po = x;
                                    break;
                                }
                            }


                            if (encabezado.Contains(datos[pc2]))
                            {
                                datos[pc2] = datos[pc2].Trim();

                                int po2 = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(datos[pc2]))
                                    {
                                        po2 = x;
                                        break;
                                    }
                                }

                                subregistros = filtro_join(datos[pc], po, subregistros, 3, po2);

                            }
                            else
                            {
                                subregistros = filtro(datos[pc], po, subregistros, 3, datos[pc2]);
                            }
                        }
                        else if (condiciones[y].Contains(">="))
                        {
                            string[] datos = condiciones[y].Split(new string[] { ">=" }, StringSplitOptions.None);

                            int pc = 0;

                            for (int x = 0; x < datos.Length; x++)
                            {
                                if (encabezado.Contains(datos[x]))
                                {
                                    pc = x;
                                    break;
                                }
                            }

                            datos[pc] = datos[pc].Trim();

                            int pc2 = datos.Length - 1 - pc;

                            string[] ordenE = encabezado.Split(',');

                            int po = 0;

                            for (int x = 0; x < ordenE.Length; x++)
                            {
                                if (ordenE[x].Equals(datos[pc]))
                                {
                                    po = x;
                                    break;
                                }
                            }




                            if (encabezado.Contains(datos[pc2]))
                            {
                                datos[pc2] = datos[pc2].Trim();

                                int po2 = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(datos[pc2]))
                                    {
                                        po2 = x;
                                        break;
                                    }
                                }

                                subregistros = filtro_join(datos[pc], po, subregistros, 4, po2);

                            }
                            else
                            {
                                subregistros = filtro(datos[pc], po, subregistros, 4, datos[pc2]);
                            }
                        }
                        else if (condiciones[y].Contains(">"))
                        {
                            string[] datos = condiciones[y].Split('>');

                            int pc = 0;

                            for (int x = 0; x < datos.Length; x++)
                            {
                                if (encabezado.Contains(datos[x]))
                                {
                                    pc = x;
                                    break;
                                }
                            }

                            datos[pc] = datos[pc].Trim();

                            int pc2 = datos.Length - 1 - pc;

                            string[] ordenE = encabezado.Split(',');

                            int po = 0;

                            for (int x = 0; x < ordenE.Length; x++)
                            {
                                if (ordenE[x].Equals(datos[pc]))
                                {
                                    po = x;
                                    break;
                                }
                            }


                            if (encabezado.Contains(datos[pc2]))
                            {
                                datos[pc2] = datos[pc2].Trim();

                                int po2 = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(datos[pc2]))
                                    {
                                        po2 = x;
                                        break;
                                    }
                                }

                                subregistros = filtro_join(datos[pc], po, subregistros, 5, po2);

                            }
                            else
                            {
                                subregistros = filtro(datos[pc], po, subregistros, 5, datos[pc2]);
                            }
                        }
                        else if (condiciones[y].Contains("<"))
                        {
                            string[] datos = condiciones[y].Split('<');

                            int pc = 0;

                            for (int x = 0; x < datos.Length; x++)
                            {
                                if (encabezado.Contains(datos[x]))
                                {
                                    pc = x;
                                    break;
                                }
                            }

                            datos[pc] = datos[pc].Trim();

                            int pc2 = datos.Length - 1 - pc;

                            string[] ordenE = encabezado.Split(',');

                            int po = 0;

                            for (int x = 0; x < ordenE.Length; x++)
                            {
                                if (ordenE[x].Equals(datos[pc]))
                                {
                                    po = x;
                                    break;
                                }
                            }


                            if (encabezado.Contains(datos[pc2]))
                            {
                                datos[pc2] = datos[pc2].Trim();

                                int po2 = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(datos[pc2]))
                                    {
                                        po2 = x;
                                        break;
                                    }
                                }

                                subregistros = filtro_join(datos[pc], po, subregistros, 6, po2);

                            }
                            else
                            {
                                subregistros = filtro(datos[pc], po, subregistros, 6, datos[pc2]);
                            }
                        }

                        resultado += subregistros;
                    }

                    registros = resultado;

                    string[] registro = registros.Split(';');

                    registros = "";

                    for(int x = 0; x < registro.Length; x++)
                    {
                        if (registros.Contains(registro[x]))
                        {

                        }
                        else
                        {
                            registros += registro[x]+";";
                        }
                    }


                }
                else if (condicion.Contains("||") == false && condicion.Contains("&&"))
                {
                    string[] condiciones = condicion.Split(new string[] { "&&" }, StringSplitOptions.None);


                    for (int y = 0; y < condiciones.Length; y++)
                    {

                        if (condiciones[y].Contains("=="))
                        {
                            string[] datos = condiciones[y].Split(new string[] { "==" }, StringSplitOptions.None);

                            int pc = 0;



                            for (int x = 0; x < datos.Length; x++)
                            {
                                if (encabezado.Contains(datos[x]))
                                {
                                    pc = x;
                                    break;
                                }
                            }

                            datos[pc] = datos[pc].Trim();

                            int pc2 = datos.Length - 1 - pc;

                            string[] ordenE = encabezado.Split(',');

                            int po = 0;

                            for (int x = 0; x < ordenE.Length; x++)
                            {
                                if (ordenE[x].Equals(datos[pc]))
                                {
                                    po = x;
                                    break;
                                }
                            }

                            if (encabezado.Contains(datos[pc2]))
                            {
                                datos[pc2] = datos[pc2].Trim();

                                int po2 = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(datos[pc2]))
                                    {
                                        po2 = x;
                                        break;
                                    }
                                }

                                registros = filtro_join(datos[pc], po, registros, 1, po2);

                            }
                            else
                            {
                                registros = filtro(datos[pc], po, registros, 1, datos[pc2]);
                            }

                            


                        }
                        else if (condiciones[y].Contains("!="))
                        {
                            string[] datos = condiciones[y].Split(new string[] { "!=" }, StringSplitOptions.None);

                            int pc = 0;

                            for (int x = 0; x < datos.Length; x++)
                            {
                                if (encabezado.Contains(datos[x]))
                                {
                                    pc = x;
                                    break;
                                }
                            }

                            datos[pc] = datos[pc].Trim();

                            int pc2 = datos.Length - 1 - pc;

                            string[] ordenE = encabezado.Split(',');

                            int po = 0;

                            for (int x = 0; x < ordenE.Length; x++)
                            {
                                if (ordenE[x].Equals(datos[pc]))
                                {
                                    po = x;
                                    break;
                                }
                            }


                            if (encabezado.Contains(datos[pc2]))
                            {
                                datos[pc2] = datos[pc2].Trim();

                                int po2 = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(datos[pc2]))
                                    {
                                        po2 = x;
                                        break;
                                    }
                                }

                                registros = filtro_join(datos[pc], po, registros, 2, po2);

                            }
                            else
                            {
                                registros = filtro(datos[pc], po, registros, 2, datos[pc2]);
                            }
                        }
                        else if (condiciones[y].Contains("<="))
                        {
                            string[] datos = condiciones[y].Split(new string[] { "<=" }, StringSplitOptions.None);

                            int pc = 0;

                            for (int x = 0; x < datos.Length; x++)
                            {
                                if (encabezado.Contains(datos[x]))
                                {
                                    pc = x;
                                    break;
                                }
                            }

                            datos[pc] = datos[pc].Trim();

                            int pc2 = datos.Length - 1 - pc;

                            string[] ordenE = encabezado.Split(',');

                            int po = 0;

                            for (int x = 0; x < ordenE.Length; x++)
                            {
                                if (ordenE[x].Equals(datos[pc]))
                                {
                                    po = x;
                                    break;
                                }
                            }


                            if (encabezado.Contains(datos[pc2]))
                            {
                                datos[pc2] = datos[pc2].Trim();

                                int po2 = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(datos[pc2]))
                                    {
                                        po2 = x;
                                        break;
                                    }
                                }

                                registros = filtro_join(datos[pc], po, registros, 3, po2);

                            }
                            else
                            {
                                registros = filtro(datos[pc], po, registros, 3, datos[pc2]);
                            }
                        }
                        else if (condiciones[y].Contains(">="))
                        {
                            string[] datos = condiciones[y].Split(new string[] { ">=" }, StringSplitOptions.None);

                            int pc = 0;

                            for (int x = 0; x < datos.Length; x++)
                            {
                                if (encabezado.Contains(datos[x]))
                                {
                                    pc = x;
                                    break;
                                }
                            }

                            datos[pc] = datos[pc].Trim();

                            int pc2 = datos.Length - 1 - pc;

                            string[] ordenE = encabezado.Split(',');

                            int po = 0;

                            for (int x = 0; x < ordenE.Length; x++)
                            {
                                if (ordenE[x].Equals(datos[pc]))
                                {
                                    po = x;
                                    break;
                                }
                            }


                            if (encabezado.Contains(datos[pc2]))
                            {
                                datos[pc2] = datos[pc2].Trim();

                                int po2 = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(datos[pc2]))
                                    {
                                        po2 = x;
                                        break;
                                    }
                                }

                                registros = filtro_join(datos[pc], po, registros, 4, po2);

                            }
                            else
                            {
                                registros = filtro(datos[pc], po, registros, 4, datos[pc2]);
                            }
                        }
                        else if (condiciones[y].Contains(">"))
                        {
                            string[] datos = condiciones[y].Split('>');

                            int pc = 0;

                            for (int x = 0; x < datos.Length; x++)
                            {
                                if (encabezado.Contains(datos[x]))
                                {
                                    pc = x;
                                    break;
                                }
                            }

                            datos[pc] = datos[pc].Trim();

                            int pc2 = datos.Length - 1 - pc;

                            string[] ordenE = encabezado.Split(',');

                            int po = 0;

                            for (int x = 0; x < ordenE.Length; x++)
                            {
                                if (ordenE[x].Equals(datos[pc]))
                                {
                                    po = x;
                                    break;
                                }
                            }


                            if (encabezado.Contains(datos[pc2]))
                            {
                                datos[pc2] = datos[pc2].Trim();

                                int po2 = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(datos[pc2]))
                                    {
                                        po2 = x;
                                        break;
                                    }
                                }

                                registros = filtro_join(datos[pc], po, registros, 5, po2);

                            }
                            else
                            {
                                registros = filtro(datos[pc], po, registros, 5, datos[pc2]);
                            }
                        }
                        else if (condiciones[y].Contains("<"))
                        {
                            string[] datos = condiciones[y].Split('<');

                            int pc = 0;

                            for (int x = 0; x < datos.Length; x++)
                            {
                                if (encabezado.Contains(datos[x]))
                                {
                                    pc = x;
                                    break;
                                }
                            }

                            datos[pc] = datos[pc].Trim();

                            int pc2 = datos.Length - 1 - pc;

                            string[] ordenE = encabezado.Split(',');

                            int po = 0;

                            for (int x = 0; x < ordenE.Length; x++)
                            {
                                if (ordenE[x].Equals(datos[pc]))
                                {
                                    po = x;
                                    break;
                                }
                            }


                            if (encabezado.Contains(datos[pc2]))
                            {
                                datos[pc2] = datos[pc2].Trim();

                                int po2 = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(datos[pc2]))
                                    {
                                        po2 = x;
                                        break;
                                    }
                                }

                                registros = filtro_join(datos[pc], po, registros, 6, po2);

                            }
                            else
                            {
                                registros = filtro(datos[pc], po, registros, 6, datos[pc2]);
                            }
                        }

                        
                    }
                }
                else
                {
                    string[] datos = condicion.Split(new string[] { "||" }, StringSplitOptions.None);

                    string subregistros = registros;
                    string resultado = registros;
                    string[] resultados = new string[datos.Length];

                    for(int j = 0; j < datos.Length; j++)
                    {
                        if (datos[j].Contains("&&"))
                        {

                            string[] subdatos = datos[j].Split(new string[] { "&&" }, StringSplitOptions.None);

                            for (int y = 0; y < subdatos.Length; y++)
                            {

                                if (subdatos[y].Contains("=="))
                                {
                                    string[] Odatos = subdatos[y].Split(new string[] { "==" }, StringSplitOptions.None);

                                    int pc = 0;



                                    for (int x = 0; x < Odatos.Length; x++)
                                    {
                                        if (encabezado.Contains(Odatos[x]))
                                        {
                                            pc = x;
                                            break;
                                        }
                                    }

                                    Odatos[pc] = Odatos[pc].Trim();

                                    int pc2 = Odatos.Length - 1 - pc;

                                    string[] ordenE = encabezado.Split(',');

                                    int po = 0;

                                    for (int x = 0; x < ordenE.Length; x++)
                                    {
                                        if (ordenE[x].Equals(Odatos[pc]))
                                        {
                                            po = x;
                                            break;
                                        }
                                    }

                                    if (encabezado.Contains(Odatos[pc2]))
                                    {
                                        Odatos[pc2] = Odatos[pc2].Trim();

                                        int po2 = 0;

                                        for (int x = 0; x < ordenE.Length; x++)
                                        {
                                            if (ordenE[x].Equals(Odatos[pc2]))
                                            {
                                                po2 = x;
                                                break;
                                            }
                                        }

                                        subregistros = filtro_join(Odatos[pc], po, subregistros, 1, po2);

                                    }
                                    else
                                    {
                                        subregistros = filtro(Odatos[pc], po, subregistros, 1, Odatos[pc2]);
                                    }

                                    


                                }
                                else if (subdatos[y].Contains("!="))
                                {
                                    string[] Odatos = subdatos[y].Split(new string[] { "==" }, StringSplitOptions.None);

                                    int pc = 0;



                                    for (int x = 0; x < Odatos.Length; x++)
                                    {
                                        if (encabezado.Contains(Odatos[x]))
                                        {
                                            pc = x;
                                            break;
                                        }
                                    }

                                    Odatos[pc] = Odatos[pc].Trim();

                                    int pc2 = Odatos.Length - 1 - pc;

                                    string[] ordenE = encabezado.Split(',');

                                    int po = 0;

                                    for (int x = 0; x < ordenE.Length; x++)
                                    {
                                        if (ordenE[x].Equals(Odatos[pc]))
                                        {
                                            po = x;
                                            break;
                                        }
                                    }


                                    if (encabezado.Contains(Odatos[pc2]))
                                    {
                                        Odatos[pc2] = Odatos[pc2].Trim();

                                        int po2 = 0;

                                        for (int x = 0; x < ordenE.Length; x++)
                                        {
                                            if (ordenE[x].Equals(Odatos[pc2]))
                                            {
                                                po2 = x;
                                                break;
                                            }
                                        }

                                        subregistros = filtro_join(Odatos[pc], po, subregistros, 2, po2);

                                    }
                                    else
                                    {
                                        subregistros = filtro(Odatos[pc], po, subregistros, 2, Odatos[pc2]);
                                    }
                                }
                                else if (subdatos[y].Contains("<="))
                                {
                                    string[] Odatos = subdatos[y].Split(new string[] { "==" }, StringSplitOptions.None);

                                    int pc = 0;



                                    for (int x = 0; x < Odatos.Length; x++)
                                    {
                                        if (encabezado.Contains(Odatos[x]))
                                        {
                                            pc = x;
                                            break;
                                        }
                                    }

                                    Odatos[pc] = Odatos[pc].Trim();

                                    int pc2 = Odatos.Length - 1 - pc;

                                    string[] ordenE = encabezado.Split(',');

                                    int po = 0;

                                    for (int x = 0; x < ordenE.Length; x++)
                                    {
                                        if (ordenE[x].Equals(Odatos[pc]))
                                        {
                                            po = x;
                                            break;
                                        }
                                    }


                                    if (encabezado.Contains(Odatos[pc2]))
                                    {
                                        Odatos[pc2] = Odatos[pc2].Trim();

                                        int po2 = 0;

                                        for (int x = 0; x < ordenE.Length; x++)
                                        {
                                            if (ordenE[x].Equals(Odatos[pc2]))
                                            {
                                                po2 = x;
                                                break;
                                            }
                                        }

                                        subregistros = filtro_join(Odatos[pc], po, subregistros, 3, po2);

                                    }
                                    else
                                    {
                                        subregistros = filtro(Odatos[pc], po, subregistros, 3, Odatos[pc2]);
                                    }
                                }
                                else if (subdatos[y].Contains(">="))
                                {
                                    string[] Odatos = subdatos[y].Split(new string[] { "==" }, StringSplitOptions.None);

                                    int pc = 0;



                                    for (int x = 0; x < Odatos.Length; x++)
                                    {
                                        if (encabezado.Contains(Odatos[x]))
                                        {
                                            pc = x;
                                            break;
                                        }
                                    }

                                    Odatos[pc] = Odatos[pc].Trim();

                                    int pc2 = Odatos.Length - 1 - pc;

                                    string[] ordenE = encabezado.Split(',');

                                    int po = 0;

                                    for (int x = 0; x < ordenE.Length; x++)
                                    {
                                        if (ordenE[x].Equals(Odatos[pc]))
                                        {
                                            po = x;
                                            break;
                                        }
                                    }


                                    if (encabezado.Contains(Odatos[pc2]))
                                    {
                                        Odatos[pc2] = Odatos[pc2].Trim();

                                        int po2 = 0;

                                        for (int x = 0; x < ordenE.Length; x++)
                                        {
                                            if (ordenE[x].Equals(Odatos[pc2]))
                                            {
                                                po2 = x;
                                                break;
                                            }
                                        }

                                        subregistros = filtro_join(Odatos[pc], po, subregistros, 4, po2);

                                    }
                                    else
                                    {
                                        subregistros = filtro(Odatos[pc], po, subregistros, 4, Odatos[pc2]);
                                    }
                                }
                                else if (subdatos[y].Contains(">"))
                                {
                                    string[] Odatos = subdatos[y].Split(new string[] { "==" }, StringSplitOptions.None);

                                    int pc = 0;



                                    for (int x = 0; x < Odatos.Length; x++)
                                    {
                                        if (encabezado.Contains(Odatos[x]))
                                        {
                                            pc = x;
                                            break;
                                        }
                                    }

                                    Odatos[pc] = Odatos[pc].Trim();

                                    int pc2 = Odatos.Length - 1 - pc;

                                    string[] ordenE = encabezado.Split(',');

                                    int po = 0;

                                    for (int x = 0; x < ordenE.Length; x++)
                                    {
                                        if (ordenE[x].Equals(Odatos[pc]))
                                        {
                                            po = x;
                                            break;
                                        }
                                    }


                                    if (encabezado.Contains(Odatos[pc2]))
                                    {
                                        Odatos[pc2] = Odatos[pc2].Trim();

                                        int po2 = 0;

                                        for (int x = 0; x < ordenE.Length; x++)
                                        {
                                            if (ordenE[x].Equals(Odatos[pc2]))
                                            {
                                                po2 = x;
                                                break;
                                            }
                                        }

                                        subregistros = filtro_join(Odatos[pc], po, subregistros, 5, po2);

                                    }
                                    else
                                    {
                                        subregistros = filtro(Odatos[pc], po, subregistros, 5, Odatos[pc2]);
                                    }
                                }
                                else if (subdatos[y].Contains("<"))
                                {
                                    string[] Odatos = subdatos[y].Split(new string[] { "==" }, StringSplitOptions.None);

                                    int pc = 0;



                                    for (int x = 0; x < Odatos.Length; x++)
                                    {
                                        if (encabezado.Contains(Odatos[x]))
                                        {
                                            pc = x;
                                            break;
                                        }
                                    }

                                    Odatos[pc] = Odatos[pc].Trim();

                                    int pc2 = Odatos.Length - 1 - pc;

                                    string[] ordenE = encabezado.Split(',');

                                    int po = 0;

                                    for (int x = 0; x < ordenE.Length; x++)
                                    {
                                        if (ordenE[x].Equals(Odatos[pc]))
                                        {
                                            po = x;
                                            break;
                                        }
                                    }


                                    if (encabezado.Contains(Odatos[pc2]))
                                    {
                                        Odatos[pc2] = Odatos[pc2].Trim();

                                        int po2 = 0;

                                        for (int x = 0; x < ordenE.Length; x++)
                                        {
                                            if (ordenE[x].Equals(Odatos[pc2]))
                                            {
                                                po2 = x;
                                                break;
                                            }
                                        }

                                        subregistros = filtro_join(Odatos[pc], po, subregistros, 6, po2);

                                    }
                                    else
                                    {
                                        subregistros = filtro(Odatos[pc], po, subregistros, 6, Odatos[pc2]);
                                    }
                                }


                            }

                            resultados[j] = subregistros;
                        }
                        else
                        {

                            if (datos[j].Contains("=="))
                            {
                                string[] Odatos = datos[j].Split(new string[] { "==" }, StringSplitOptions.None);

                                int pc = 0;



                                for (int x = 0; x < Odatos.Length; x++)
                                {
                                    if (encabezado.Contains(Odatos[x]))
                                    {
                                        pc = x;
                                        break;
                                    }
                                }

                                Odatos[pc] = Odatos[pc].Trim();

                                int pc2 = Odatos.Length - 1 - pc;

                                string[] ordenE = encabezado.Split(',');

                                int po = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(Odatos[pc]))
                                    {
                                        po = x;
                                        break;
                                    }
                                }


                                if (encabezado.Contains(Odatos[pc2]))
                                {
                                    Odatos[pc2] = Odatos[pc2].Trim();

                                    int po2 = 0;

                                    for (int x = 0; x < ordenE.Length; x++)
                                    {
                                        if (ordenE[x].Equals(Odatos[pc2]))
                                        {
                                            po2 = x;
                                            break;
                                        }
                                    }

                                    resultado = filtro_join(Odatos[pc], po, resultado, 1, po2);

                                }
                                else
                                {
                                    resultado = filtro(Odatos[pc], po, resultado, 1, Odatos[pc2]);
                                }


                            }
                            else if (datos[j].Contains("!="))
                            {
                                string[] Odatos = datos[j].Split(new string[] { "==" }, StringSplitOptions.None);

                                int pc = 0;



                                for (int x = 0; x < Odatos.Length; x++)
                                {
                                    if (encabezado.Contains(Odatos[x]))
                                    {
                                        pc = x;
                                        break;
                                    }
                                }

                                Odatos[pc] = Odatos[pc].Trim();

                                int pc2 = Odatos.Length - 1 - pc;

                                string[] ordenE = encabezado.Split(',');

                                int po = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(Odatos[pc]))
                                    {
                                        po = x;
                                        break;
                                    }
                                }


                                if (encabezado.Contains(Odatos[pc2]))
                                {
                                    Odatos[pc2] = Odatos[pc2].Trim();

                                    int po2 = 0;

                                    for (int x = 0; x < ordenE.Length; x++)
                                    {
                                        if (ordenE[x].Equals(Odatos[pc2]))
                                        {
                                            po2 = x;
                                            break;
                                        }
                                    }

                                    resultado = filtro_join(Odatos[pc], po, resultado, 2, po2);

                                }
                                else
                                {
                                    resultado = filtro(Odatos[pc], po, resultado, 2, Odatos[pc2]);
                                }
                            }
                            else if (datos[j].Contains("<="))
                            {
                                string[] Odatos = datos[j].Split(new string[] { "<=" }, StringSplitOptions.None);

                                int pc = 0;



                                for (int x = 0; x < Odatos.Length; x++)
                                {
                                    if (encabezado.Contains(Odatos[x]))
                                    {
                                        pc = x;
                                        break;
                                    }
                                }

                                Odatos[pc] = Odatos[pc].Trim();

                                int pc2 = Odatos.Length - 1 - pc;

                                string[] ordenE = encabezado.Split(',');

                                int po = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(Odatos[pc]))
                                    {
                                        po = x;
                                        break;
                                    }
                                }


                                if (encabezado.Contains(Odatos[pc2]))
                                {
                                    Odatos[pc2] = Odatos[pc2].Trim();

                                    int po2 = 0;

                                    for (int x = 0; x < ordenE.Length; x++)
                                    {
                                        if (ordenE[x].Equals(Odatos[pc2]))
                                        {
                                            po2 = x;
                                            break;
                                        }
                                    }

                                    resultado = filtro_join(Odatos[pc], po, resultado, 3, po2);

                                }
                                else
                                {
                                    resultado = filtro(Odatos[pc], po, resultado, 3, Odatos[pc2]);
                                }
                            }
                            else if (datos[j].Contains(">="))
                            {
                                string[] Odatos = datos[j].Split(new string[] { "==" }, StringSplitOptions.None);

                                int pc = 0;



                                for (int x = 0; x < Odatos.Length; x++)
                                {
                                    if (encabezado.Contains(Odatos[x]))
                                    {
                                        pc = x;
                                        break;
                                    }
                                }

                                Odatos[pc] = Odatos[pc].Trim();

                                int pc2 = Odatos.Length - 1 - pc;

                                string[] ordenE = encabezado.Split(',');

                                int po = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(Odatos[pc]))
                                    {
                                        po = x;
                                        break;
                                    }
                                }


                                if (encabezado.Contains(Odatos[pc2]))
                                {
                                    Odatos[pc2] = Odatos[pc2].Trim();

                                    int po2 = 0;

                                    for (int x = 0; x < ordenE.Length; x++)
                                    {
                                        if (ordenE[x].Equals(Odatos[pc2]))
                                        {
                                            po2 = x;
                                            break;
                                        }
                                    }

                                    resultado = filtro_join(Odatos[pc], po, resultado, 4, po2);

                                }
                                else
                                {
                                    resultado = filtro(Odatos[pc], po, resultado, 4, Odatos[pc2]);
                                }
                            }
                            else if (datos[j].Contains(">"))
                            {
                                string[] Odatos = datos[j].Split(new string[] { "==" }, StringSplitOptions.None);

                                int pc = 0;



                                for (int x = 0; x < Odatos.Length; x++)
                                {
                                    if (encabezado.Contains(Odatos[x]))
                                    {
                                        pc = x;
                                        break;
                                    }
                                }

                                Odatos[pc] = Odatos[pc].Trim();

                                int pc2 = Odatos.Length - 1 - pc;

                                string[] ordenE = encabezado.Split(',');

                                int po = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(Odatos[pc]))
                                    {
                                        po = x;
                                        break;
                                    }
                                }


                                if (encabezado.Contains(Odatos[pc2]))
                                {
                                    Odatos[pc2] = Odatos[pc2].Trim();

                                    int po2 = 0;

                                    for (int x = 0; x < ordenE.Length; x++)
                                    {
                                        if (ordenE[x].Equals(Odatos[pc2]))
                                        {
                                            po2 = x;
                                            break;
                                        }
                                    }

                                    resultado = filtro_join(Odatos[pc], po, resultado, 5, po2);

                                }
                                else
                                {
                                    resultado = filtro(Odatos[pc], po, resultado, 5, Odatos[pc2]);
                                }
                            }
                            else if (datos[j].Contains("<"))
                            {
                                string[] Odatos = datos[j].Split(new string[] { "==" }, StringSplitOptions.None);

                                int pc = 0;



                                for (int x = 0; x < Odatos.Length; x++)
                                {
                                    if (encabezado.Contains(Odatos[x]))
                                    {
                                        pc = x;
                                        break;
                                    }
                                }

                                Odatos[pc] = Odatos[pc].Trim();

                                int pc2 = Odatos.Length - 1 - pc;

                                string[] ordenE = encabezado.Split(',');

                                int po = 0;

                                for (int x = 0; x < ordenE.Length; x++)
                                {
                                    if (ordenE[x].Equals(Odatos[pc]))
                                    {
                                        po = x;
                                        break;
                                    }
                                }


                                if (encabezado.Contains(Odatos[pc2]))
                                {
                                    Odatos[pc2] = Odatos[pc2].Trim();

                                    int po2 = 0;

                                    for (int x = 0; x < ordenE.Length; x++)
                                    {
                                        if (ordenE[x].Equals(Odatos[pc2]))
                                        {
                                            po2 = x;
                                            break;
                                        }
                                    }

                                    resultado = filtro_join(Odatos[pc], po, resultado, 6, po2);

                                }
                                else
                                {
                                    resultado = filtro(Odatos[pc], po, resultado, 6, Odatos[pc2]);
                                }
                            }

                            resultados[j] = resultado;
                        }


                    }

                    registros = resultados[0];

                    for(int o = 1; o < resultados.Length; o++)
                    {
                        string[] tt = resultados[o].Split(';');

                        for(int t = 0; t < tt.Length; t++)
                        {
                            if (registros.Contains(tt[t]))
                            {

                            }
                            else
                            {
                                registros += tt[t] + ";";
                            }
                        }

                    }

                    
                    //string[] datos = condicion.Split(new string[] { "==" }, StringSplitOptions.None);
                }
            }
            else
            {
                if (condicion.Contains("=="))
                {
                    string[] datos = condicion.Split(new string[] { "==" }, StringSplitOptions.None);

                    int pc=0;

                    for(int x = 0; x < datos.Length; x++)
                    {
                        if (encabezado.Contains(datos[x]))
                        {
                            pc = x;
                            break;
                        }
                    }

                    int pc2 = datos.Length - 1 - pc;

                    string[] ordenE = encabezado.Split(',');

                    int po = 0;

                    for (int x = 0; x < ordenE.Length; x++)
                    {
                        if (ordenE[x].Equals(datos[pc]))
                        {
                            po = x;
                            break;
                        }
                    }

                    datos[pc2] = datos[pc2].Replace("\"", "");

                    datos[pc2] = datos[pc2].Trim();

                    if (encabezado.Contains(datos[pc2]))
                    {
                        datos[pc2] = datos[pc2].Trim();

                        int po2 = 0;

                        for (int x = 0; x < ordenE.Length; x++)
                        {
                            if (ordenE[x].Equals(datos[pc2]))
                            {
                                po2 = x;
                                break;
                            }
                        }

                        registros = filtro_join(datos[pc], po, registros, 1, po2);

                    }
                    else
                    {
                        registros = filtro(datos[pc], po, registros, 1, datos[pc2]);
                    }


                }
                else if (condicion.Contains("!="))
                {
                    string[] datos = condicion.Split(new string[] { "!=" }, StringSplitOptions.None);

                    int pc = 0;

                    for (int x = 0; x < datos.Length; x++)
                    {
                        if (encabezado.Contains(datos[x]))
                        {
                            pc = x;
                            break;
                        }
                    }

                    int pc2 = datos.Length - 1 - pc;

                    string[] ordenE = encabezado.Split(',');

                    int po = 0;

                    for (int x = 0; x < ordenE.Length; x++)
                    {
                        if (ordenE[x].Equals(datos[pc]))
                        {
                            po = x;
                            break;
                        }
                    }


                    if (encabezado.Contains(datos[pc2]))
                    {
                        datos[pc2] = datos[pc2].Trim();

                        int po2 = 0;

                        for (int x = 0; x < ordenE.Length; x++)
                        {
                            if (ordenE[x].Equals(datos[pc2]))
                            {
                                po2 = x;
                                break;
                            }
                        }

                        registros = filtro_join(datos[pc], po, registros, 1, po2);

                    }
                    else
                    {
                        registros = filtro(datos[pc], po, registros, 1, datos[pc2]);
                    }
                }
                else if (condicion.Contains("<="))
                {
                    string[] datos = condicion.Split(new string[] { "<=" }, StringSplitOptions.None);

                    int pc = 0;

                    for (int x = 0; x < datos.Length; x++)
                    {
                        if (encabezado.Contains(datos[x]))
                        {
                            pc = x;
                            break;
                        }
                    }

                    int pc2 = datos.Length - 1 - pc;

                    string[] ordenE = encabezado.Split(',');

                    int po = 0;

                    for (int x = 0; x < ordenE.Length; x++)
                    {
                        if (ordenE[x].Equals(datos[pc]))
                        {
                            po = x;
                            break;
                        }
                    }


                    if (encabezado.Contains(datos[pc2]))
                    {
                        datos[pc2] = datos[pc2].Trim();

                        int po2 = 0;

                        for (int x = 0; x < ordenE.Length; x++)
                        {
                            if (ordenE[x].Equals(datos[pc2]))
                            {
                                po2 = x;
                                break;
                            }
                        }

                        registros = filtro_join(datos[pc], po, registros, 1, po2);

                    }
                    else
                    {
                        registros = filtro(datos[pc], po, registros, 1, datos[pc2]);
                    }
                }
                else if (condicion.Contains(">="))
                {
                    string[] datos = condicion.Split(new string[] { ">=" }, StringSplitOptions.None);

                    int pc = 0;

                    for (int x = 0; x < datos.Length; x++)
                    {
                        if (encabezado.Contains(datos[x]))
                        {
                            pc = x;
                            break;
                        }
                    }

                    int pc2 = datos.Length - 1 - pc;

                    string[] ordenE = encabezado.Split(',');

                    int po = 0;

                    for (int x = 0; x < ordenE.Length; x++)
                    {
                        if (ordenE[x].Equals(datos[pc]))
                        {
                            po = x;
                            break;
                        }
                    }


                    if (encabezado.Contains(datos[pc2]))
                    {
                        datos[pc2] = datos[pc2].Trim();

                        int po2 = 0;

                        for (int x = 0; x < ordenE.Length; x++)
                        {
                            if (ordenE[x].Equals(datos[pc2]))
                            {
                                po2 = x;
                                break;
                            }
                        }

                        registros = filtro_join(datos[pc], po, registros, 1, po2);

                    }
                    else
                    {
                        registros = filtro(datos[pc], po, registros, 1, datos[pc2]);
                    }
                }
                else if (condicion.Contains(">"))
                {
                    string[] datos = condicion.Split('>');

                    int pc = 0;

                    for (int x = 0; x < datos.Length; x++)
                    {
                        if (encabezado.Contains(datos[x]))
                        {
                            pc = x;
                            break;
                        }
                    }

                    int pc2 = datos.Length - 1 - pc;

                    string[] ordenE = encabezado.Split(',');

                    int po = 0;

                    for (int x = 0; x < ordenE.Length; x++)
                    {
                        if (ordenE[x].Equals(datos[pc]))
                        {
                            po = x;
                            break;
                        }
                    }


                    if (encabezado.Contains(datos[pc2]))
                    {
                        datos[pc2] = datos[pc2].Trim();

                        int po2 = 0;

                        for (int x = 0; x < ordenE.Length; x++)
                        {
                            if (ordenE[x].Equals(datos[pc2]))
                            {
                                po2 = x;
                                break;
                            }
                        }

                        registros = filtro_join(datos[pc], po, registros, 1, po2);

                    }
                    else
                    {
                        registros = filtro(datos[pc], po, registros, 1, datos[pc2]);
                    }
                }
                else if (condicion.Contains("<"))
                {
                    string[] datos = condicion.Split('<');

                    int pc = 0;

                    for (int x = 0; x < datos.Length; x++)
                    {
                        if (encabezado.Contains(datos[x]))
                        {
                            pc = x;
                            break;
                        }
                    }

                    int pc2 = datos.Length - 1 - pc;

                    string[] ordenE = encabezado.Split(',');

                    int po = 0;

                    for (int x = 0; x < ordenE.Length; x++)
                    {
                        if (ordenE[x].Equals(datos[pc]))
                        {
                            po = x;
                            break;
                        }
                    }


                    if (encabezado.Contains(datos[pc2]))
                    {
                        datos[pc2] = datos[pc2].Trim();

                        int po2 = 0;

                        for (int x = 0; x < ordenE.Length; x++)
                        {
                            if (ordenE[x].Equals(datos[pc2]))
                            {
                                po2 = x;
                                break;
                            }
                        }

                        registros = filtro_join(datos[pc], po, registros, 1, po2);

                    }
                    else
                    {
                        registros = filtro(datos[pc], po, registros, 1, datos[pc2]);
                    }
                }
            }

            //orden

            if (orden.Equals(""))
            {

            }
            else
            {

            
            string[] Porden = orden.Split(' ');

            if (Porden[1].Equals("ASC"))
            {

                string[] ordenE = encabezado.Split(',');

                int po = 0;

                for(int x = 0; x < ordenE.Length; x++)
                {
                    if (ordenE[x].Equals(Porden[0]))
                    {
                        po = x;
                        break;
                    }
                }

                registros = ordenar_Asc(Porden[0],po,registros);

            }
            else
            {

                string[] ordenE = encabezado.Split(',');

                int po = 0;

                for (int x = 0; x < ordenE.Length; x++)
                {
                    if (ordenE[x].Equals(Porden[0]))
                    {
                        po = x;
                        break;
                    }
                }

                registros = ordenar_Desc(Porden[0], po, registros);
            }
            }

            //filtro
            if (campos.Equals("*"))
            {
                if (Tablas.Contains(','))
                {
                    string[] subT = Tablas.Split(',');

                    for (int x = 0; x < subT.Length; x++)
                    {
                        encabezado += getcamposTabla(subT[x])+",";
                    }

                    encabezado = encabezado.Trim(',');
                }
                else
                {
                    encabezado = getcamposTabla(Tablas);
                }
            }
            else
            {
                if (Tablas.Contains(','))
                {
                    string[] subT = Tablas.Split(',');

                    for (int x = 0; x < subT.Length; x++)
                    {
                        encabezado += getcamposTabla(subT[x]) + ",";
                    }

                    encabezado = encabezado.Trim(',');

                    string[] subE = encabezado.Split(',');

                    string[] subc = campos.Split(',');

                    string pos = "";

                    for (int x = 0; x < subE.Length; x++)
                    {
                        for (int y = 0; y < subc.Length; y++)
                        {
                            if (subE[x].Equals(subc[y]))
                            {
                                pos += x + ",";
                            }
                        }
                    }

                    encabezado = campos;

                    pos = pos.Trim(',');

                    string[] subr = registros.Split(';');

                    registros = "";

                    int[] posiciones = Array.ConvertAll(pos.Split(','), int.Parse);

                    for (int x = 0; x < subr.Length - 1; x++)
                    {
                        string[] subsubr = subr[x].Split(',');

                        for (int y = 0; y < posiciones.Length; y++)
                        {

                            registros += subsubr[posiciones[y]] + ",";
                        }

                        registros = registros.Trim(',');
                        registros += ";";
                    }
                }
                else
                {
                    encabezado = getcamposTabla(Tablas);

                    string[] subE = encabezado.Split(',');

                    string[] subc = campos.Split(',');

                    string pos = "";

                    for (int x = 0; x < subE.Length; x++)
                    {
                        for(int y = 0; y < subc.Length; y++)
                        {
                            if (subE[x].Equals(subc[y]))
                            {
                                pos += x + ",";
                            }
                        }
                    }

                    encabezado = campos;

                    pos = pos.Trim(',');

                    string[] subr = registros.Split(';');

                    registros = "";

                    int[] posiciones = Array.ConvertAll(pos.Split(','), int.Parse);

                    for(int x = 0; x < subr.Length-1; x++)
                    {
                        string[] subsubr = subr[x].Split(',');

                        for (int y = 0; y < posiciones.Length; y++)
                        {
                            
                            registros += subsubr[posiciones[y]] + ",";
                        }

                        registros = registros.Trim(',');
                        registros += ";";
                    }

                }
            }


            return encabezado+";;"+registros;
        }

        string getRegistrosTabla(string Tabla)
        {
            string respuesta="";

            maestro.bases.aux.tablas.existe(Tabla);

            Tabla aux = maestro.bases.aux.tablas.aux;

            if (aux.cabeza == null)
            {
                string campos = aux.campos;
                string[] campo = campos.Split(',');

                for (int x = 0; x < campo.Length; x++)
                {
                    respuesta += "null,";
                }

                respuesta = respuesta.Trim(',');

            }
            else if (aux.ultimo == null)
            {
                respuesta = aux.cabeza.valor;
            }
            else
            {
                aux.aux = aux.cabeza;
                bool seguir = true;

                while (seguir)
                {
                    respuesta += aux.aux.valor + ";";

                    if (aux.aux.siguiente != null)
                    {
                        aux.aux = aux.aux.siguiente;
                    }
                    else
                    {
                        seguir = false;
                    }
                }
                
            }

            return respuesta;
        }

        string getcamposTabla(string Tabla)
        {

            maestro.bases.aux.tablas.existe(Tabla);

            return maestro.bases.aux.tablas.aux.campos;
        }

        string ordenar_Asc(string campo, int pos, string registros)
        {
            string[] campos = registros.Split(';');

            string ordenar = "";

            for(int x = 0; x < campos.Length-1; x++)
            {
                ordenar += campos[x].Split(',')[pos] + ";";
            }


            string[] orden = ordenar.Split(';');

            string t;
            for (int a = 1; a < orden.Length-1; a++)
                for (int b = orden.Length -2; b >= a; b--)
                {
                    if (String.Compare(orden[b - 1], orden[b])==1)
                    {
                        t = orden[b - 1];
                        orden[b - 1] = orden[b];
                        orden[b] = t;

                        t = campos[b - 1];
                        campos[b - 1] = campos[b];
                        campos[b] = t;
                    }
                }

            string respuesta = "";

            for (int x = 0; x < campos.Length - 1; x++)
            {
                respuesta += campos[x] + ";";
            }

            //Compare(A, B): -1
            //Compare(B, A):  1

            //Compare(A, A):  0
            //Compare(B, B):  0

            return respuesta;
        }

        string ordenar_Desc(string campo, int pos, string registros)
        {
            string[] campos = registros.Split(';');

            string ordenar = "";

            for (int x = 0; x < campos.Length - 1; x++)
            {
                ordenar += campos[x].Split(',')[pos] + ";";
            }


            string[] orden = ordenar.Split(';');

            string t;
            for (int a = 1; a < orden.Length - 1; a++)
                for (int b = orden.Length - 2; b >= a; b--)
                {
                    if (String.Compare(orden[b - 1], orden[b]) == -1)
                    {
                        t = orden[b - 1];
                        orden[b - 1] = orden[b];
                        orden[b] = t;

                        t = campos[b - 1];
                        campos[b - 1] = campos[b];
                        campos[b] = t;
                    }
                }

            string respuesta = "";

            for (int x = 0; x < campos.Length - 1; x++)
            {
                respuesta += campos[x] + ";";
            }

          

            return respuesta;
        }

        string filtro(string campo, int pos, string registros,int tipo,string condicion)
        {
            string[] campos = registros.Split(';');

            string ordenar = "";

            string respuesta="";

            condicion = condicion.Trim();

            condicion = condicion.Trim(new char[] { '"' ,'\\',});

            condicion = condicion.Trim();


            for (int x = 0; x < campos.Length - 1; x++)
            {
                ordenar = campos[x].Split(',')[pos];

                if (tipo == 1)
                {
                    if (ordenar.Equals(condicion))
                    {
                        respuesta += campos[x] + ";";
                    }
                }
                else if (tipo == 2)
                {
                    if (ordenar.Equals(condicion))
                    {
                        
                    }
                    else
                    {
                        respuesta += campos[x] + ";";
                    }
                }
                else if (tipo == 3) //<=
                {
                    if(String.Compare(ordenar, condicion) == -1|| String.Compare(ordenar, condicion) == 0) 
                    {
                        respuesta += campos[x] + ";";
                    }
                }
                else if (tipo == 4) //>=
                {
                    if (String.Compare(ordenar, condicion) == 1 || String.Compare(ordenar, condicion) == 0)
                    {
                        respuesta += campos[x] + ";";
                    }
                }
                else if (tipo == 5) //>
                {
                    if (String.Compare(ordenar, condicion) == 1)
                    {
                        respuesta += campos[x] + ";";
                    }
                }
                else if (tipo == 6) //<
                {
                    if (String.Compare(ordenar, condicion) == -1 )
                    {
                        respuesta += campos[x] + ";";
                    }
                }

            }

            return respuesta;
        }

        string filtro_join(string campo, int pos, string registros, int tipo, int campo2)
        {
            string[] campos = registros.Split(';');

            string ordenar = "";

            string respuesta = "";

            string condicion;


            for (int x = 0; x < campos.Length - 1; x++)
            {
                ordenar = campos[x].Split(',')[pos];
                condicion= campos[x].Split(',')[campo2];


                if (tipo == 1)
                {
                    if (ordenar.Equals(condicion))
                    {
                        respuesta += campos[x] + ";";
                    }
                }
                else if (tipo == 2)
                {
                    if (ordenar.Equals(condicion))
                    {

                    }
                    else
                    {
                        respuesta += campos[x] + ";";
                    }
                }
                else if (tipo == 3) //<=
                {
                    if (String.Compare(ordenar, condicion) == -1 || String.Compare(ordenar, condicion) == 0)
                    {
                        respuesta += campos[x] + ";";
                    }
                }
                else if (tipo == 4) //>=
                {
                    if (String.Compare(ordenar, condicion) == 1 || String.Compare(ordenar, condicion) == 0)
                    {
                        respuesta += campos[x] + ";";
                    }
                }
                else if (tipo == 5) //>
                {
                    if (String.Compare(ordenar, condicion) == 1)
                    {
                        respuesta += campos[x] + ";";
                    }
                }
                else if (tipo == 6) //<
                {
                    if (String.Compare(ordenar, condicion) == -1)
                    {
                        respuesta += campos[x] + ";";
                    }
                }

            }

            return respuesta;
        }
    }
    
}