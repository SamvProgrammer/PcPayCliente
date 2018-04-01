using PcPay.Code.Usuario;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace PcPay.Code.Utilidades
{
    static class Ayuda
    {

        private static bool menu1bool { get; set; }
        private static string[] menu4 = { "mnu4_3" };
        private static char[] opcionmenu4 = {'1'};
        private static string[] menu2 = { "mnu2_12" };
        private static char[] opcionmenu2 = { '1' };
        private static string[] menu5 = { "mnu5_2" };
        private static char[] opcionmenu5 = { '1' };
        public static Dictionary<string, List<MenuOpciones>> getMenu()
        {
            //Menus extras;
            XmlNodeList temporalHijos = null;
            string llaveHijoTemporal;
            List<MenuOpciones> auxMenu5 = null;
            List<MenuOpciones> aux;
            bool repetir = true;
            leerXml leer = new leerXml(Globales.GetDataXml("menupcpayagencias", TypeUsuario.CadenaXML));
            var temp = leer.getElemento("menupcpayagencias");
            XmlNode padre = temp[0];
            Dictionary<string, List<MenuOpciones>> menu = new Dictionary<string, List<MenuOpciones>>();
            XmlNode temporalHijo = null;
            if (Globales.GetDataXml("id_company", TypeUsuario.CadenaXML) == Globales.userSantanderVta)
            {
                string nombre2 = "Recompensas,Venta,Venta sin presencia,Reportes,Reimpresión";
                string opcion2 = "10111";
                auxMenu5 = operacionMenu(opcion2, nombre2, "recompensas");
                llaveHijoTemporal = auxMenu5.First().nombre;
                auxMenu5.RemoveAt(0);
                menu.Add(llaveHijoTemporal, auxMenu5);
                if (Globales.GetDataXml("st_tokenizacion", TypeUsuario.CadenaXML).ToUpper() == "1")
                {
                    nombre2 = "Token";
                    opcion2 = "1";
                    auxMenu5 = operacionMenu(opcion2, nombre2, "token");
                    llaveHijoTemporal = auxMenu5.First().nombre;
                    auxMenu5.RemoveAt(0);
                    menu.Add(llaveHijoTemporal, auxMenu5);
                    if (!(Globales.setReader))
                    {
                        Globales.setReader = Globales.cpIntegraEMV.dbgSetReader();
                    }
                }
                if (Globales.GetDataXml("activa_cupones", TypeUsuario.CadenaXML) == "1")
                {
                    nombre2 = "Cupones,Alta cliente,Canjear,Número celular";
                    opcion2 = "1111";
                    auxMenu5 = operacionMenu(opcion2, nombre2, "cupones");
                    llaveHijoTemporal = auxMenu5.First().nombre;
                    auxMenu5.RemoveAt(0);
                    menu.Add(llaveHijoTemporal, auxMenu5);
                }
                if (Globales.GetDataXml("st_pague_directo", TypeUsuario.CadenaXML) == "1")
                {
                    if (!Globales.cpIntegraEMV.chkPp_soportaDUKPT())
                    {
                        nombre2 = "Pague Directo,Ventas,Reimpresión,Reporte";
                        opcion2 = "1111";
                        auxMenu5 = operacionMenu(opcion2, nombre2, "pagueDirecto");
                        llaveHijoTemporal = auxMenu5.First().nombre;
                        auxMenu5.RemoveAt(0);
                        menu.Add(llaveHijoTemporal, auxMenu5);
                    }
                }
            }
            else {
                foreach (XmlNode item in padre.ChildNodes)
                {
                    try
                    {
                        XmlNodeList hijosItem = item.ChildNodes;
                        if (item.Name == "menu5")
                        {
                            temporalHijo = item;
                            //Reacomodar despues


                            continue;
                        }

                        string auxName = item.Name;
                    volver:

                        if (item.Name == "menu10")
                        {
                            if (TypeUsuario.Points2 == "1")
                            {
                                string[] menupoints = Globales.GetDataXml("menu_points2", TypeUsuario.CadenaXML).Split(',');
                                string nombre2 = menupoints[0] + "," + menupoints[1];
                                string opcion2 = "11";
                                auxMenu5 = operacionMenu(opcion2, nombre2, "menuPoints");
                                llaveHijoTemporal = auxMenu5.First().nombre;
                                auxMenu5.RemoveAt(0);
                                menu.Add(llaveHijoTemporal, auxMenu5);
                            }
                            if (Globales.GetDataXml("wallets", TypeUsuario.CadenaXML) == "1")
                            {
                                string nombre2 = "Wallets,Impresión voucher";
                                string opcion2 = "11";
                                auxMenu5 = operacionMenu(opcion2, nombre2, "Wallets");
                                llaveHijoTemporal = auxMenu5.First().nombre;
                                auxMenu5.RemoveAt(0);
                                menu.Add(llaveHijoTemporal, auxMenu5);
                            }
                            if (Globales.cpIntegraEMV.isRecompensas())
                            {
                                string nombre2 = "Recompensas,Venta,Venta sin presencia,Reportes,Reimpresión";
                                string opcion2 = "10011";
                                auxMenu5 = operacionMenu(opcion2, nombre2, "recompensas");
                                llaveHijoTemporal = auxMenu5.First().nombre;
                                auxMenu5.RemoveAt(0);
                                menu.Add(llaveHijoTemporal, auxMenu5);
                            }
                            if (Globales.GetDataXml("st_tokenizacion", TypeUsuario.CadenaXML).ToUpper() == "1")
                            {
                                string nombre2 = "Token";
                                string opcion2 = "1";
                                auxMenu5 = operacionMenu(opcion2, nombre2, "token");
                                llaveHijoTemporal = auxMenu5.First().nombre;
                                auxMenu5.RemoveAt(0);
                                menu.Add(llaveHijoTemporal, auxMenu5);
                                if (!(Globales.setReader))
                                {
                                    Globales.setReader = Globales.cpIntegraEMV.dbgSetReader();
                                }
                            }
                            if (Globales.GetDataXml("conectaycobra", TypeUsuario.CadenaXML) == "1" || Globales.GetDataXml("supernegocio", TypeUsuario.CadenaXML) == "1")
                            {
                                string nombre2 = "Reportes,Reportes";
                                string opcion2 = "11";
                                auxMenu5 = operacionMenu(opcion2, nombre2, "reportes");
                                llaveHijoTemporal = auxMenu5.First().nombre;
                                auxMenu5.RemoveAt(0);
                                menu.Add(llaveHijoTemporal, auxMenu5);
                            }
                            if (Globales.GetDataXml("activa_cupones", TypeUsuario.CadenaXML) == "1")
                            {
                                string nombre2 = "Cupones,Alta cliente,Canjear,Número celular";
                                string opcion2 = "1111";
                                auxMenu5 = operacionMenu(opcion2, nombre2, "cupones");
                                llaveHijoTemporal = auxMenu5.First().nombre;
                                auxMenu5.RemoveAt(0);
                                menu.Add(llaveHijoTemporal, auxMenu5);
                            }
                            if (Globales.GetDataXml("st_pague_directo", TypeUsuario.CadenaXML) == "1")
                            {
                                if (!Globales.cpIntegraEMV.chkPp_soportaDUKPT())
                                {
                                    string nombre2 = "Pague Directo,Ventas,Reimpresión,Reporte";
                                    string opcion2 = "1111";
                                    auxMenu5 = operacionMenu(opcion2, nombre2, "pagueDirecto");
                                    llaveHijoTemporal = auxMenu5.First().nombre;
                                    auxMenu5.RemoveAt(0);
                                    menu.Add(llaveHijoTemporal, auxMenu5);
                                }
                            }
                            temporalHijos = temporalHijo.ChildNodes;
                            auxMenu5 = operacionMenu(temporalHijos[0].InnerText, temporalHijos[1].InnerText, "menu5");
                            llaveHijoTemporal = auxMenu5.First().nombre;
                            auxMenu5.RemoveAt(0);
                            menu.Add(llaveHijoTemporal, auxMenu5);

                            aux = operacionMenu("11", "Salir,Salir", "menu10");

                        }
                        else
                        {
                            if (auxName == "menu6") continue;
                            if(auxName == "menu8"){
                                hijosItem[0].InnerText += "1";
                                hijosItem[1].InnerText += ",Reporte Venta de Servicios";
                            }
                            aux = operacionMenu(hijosItem[0].InnerText, hijosItem[1].InnerText, auxName);
                        }

                        string llave = aux.First().nombre;
                        aux.RemoveAt(0);
                        menu.Add(llave, aux);
                        if (Globales.isAerolinea)
                        {
                            if (repetir)
                            {
                                repetir = false;
                                hijosItem[0].InnerText = hijosItem[0].InnerText.Substring(0, 12) + "00" + hijosItem[0].InnerText.Substring(14);
                                auxName = "menu2";
                                goto volver;
                            }
                        }
                    }
                    catch
                    {
                        string name = string.Empty;
                        string opcion = string.Empty;
                        XmlNode valor = item;
                        if (valor.Name == "menuprepago")
                        {
                            XmlNodeList nodos = valor.ChildNodes;
                            if (nodos.Count > 0)
                            {
                                XmlNode a1 = item.PreviousSibling;
                                XmlNodeList a2 = a1.ChildNodes;
                                name = a2[1].InnerText.Substring(0, a2[1].InnerText.IndexOf(',')) + ",";
                                opcion = "1";
                                foreach (XmlNode nodito in nodos)
                                {
                                    name += nodito.InnerText + ",";
                                    opcion += "1";
                                }
                                name = name.Substring(0, name.Length - 1);
                            }
                        }
                        List<MenuOpciones> temporal = operacionMenu(opcion, name, "menuprepago");
                        string llave = temporal.First().nombre;
                        temporal.RemoveAt(0);
                        menu.Add(llave, temporal);
                    }

                }
            }
            return menu;
        }
        public static List<MenuOpciones> operacionMenu(string opciones, string nombre, string nombreMenuAux = "")
        {
            MenuOpciones menu;
            List<MenuOpciones> menuLista = new List<MenuOpciones>();
            var arregloNombreAux = nombre.Split(',');
            var opcionesListAux = opciones.ToCharArray();
            if(nombreMenuAux == "menu1"){
                menu1bool = true;
                opciones = "";
                nombre = "";
                for (int x = 0; x < arregloNombreAux.Length; x++ )
                {
                    if(x == 7){
                        opciones += opcionesListAux[x];
                        nombre += "Re-Autorización,";
                    }
                    opciones += opcionesListAux[x];
                    nombre += arregloNombreAux[x];
                    if (x < arregloNombreAux.Length - 1)
                        nombre += ",";
                }
            }
            if(nombreMenuAux == "menu2"){
                opciones = "";
                nombre = "";
                for (int x = 0; x < arregloNombreAux.Length; x++)
                {
                    if (x == 7)//
                    {
                        opciones += '0';
                        nombre += "mnu2_15,";
                    }
                    opciones += opcionesListAux[x];
                    nombre += arregloNombreAux[x];
                    if (x < arregloNombreAux.Length - 1)
                        nombre += ",";
                }
                opciones += "0";
                nombre += ",Pago Distancia";
            }
            if (nombreMenuAux == "menu3")
            {
                opciones = "";
                nombre = "";
                for (int x = 0; x < arregloNombreAux.Length; x++)
                {
                   
                    opciones += opcionesListAux[x];
                    nombre += arregloNombreAux[x];
                    if (x < arregloNombreAux.Length - 1)
                        nombre += ",";
                    if (x == 2)
                    {
                        opciones += '1';
                        nombre += "Reenvío de Voucher,";
                    }
                }
               if(Globales.isAerolinea){
                   opciones += "1";
                   nombre += ",Asociación de boletos";
               }
               
            }
            if(nombreMenuAux == "menu4"){
                if (Globales.GetDataXml("stlogin", TypeUsuario.CadenaXML) == "1")
                {
                    opciones += "1111";
                }
                else {
                    opciones += "1101";
                }
                nombre += ",Tipo de impresoras,Lector de tarjetas,Cambiar de usuario,Capacidad touch";
            }
            if(nombreMenuAux == "menu5"){
                opciones += "1111";
                nombre += ",Registrar licencia,Conectividad,Probar Lector,Conversor DCC";
            }

            var arregloNombre = nombre.Split(',');
            var opcionesList = opciones.ToCharArray();
            if(nombreMenuAux == "menu1"){
                arregloNombre[0] = (Globales.isAerolinea) ? "Venta de Boletos" : arregloNombre[0];
                opcionmenu4[0] = opcionesList[2];
                opcionesList[3] = (Globales.isAgencias) ? '0' : opcionesList[3];
                opcionesList[11] = (TypeUsuario.AddTableNum) ? '0' : opcionesList[11];
                if (Globales.isAgencias)
                {
                    if (opcionesList[18] == '1')
                    {
                        if (Globales.cpIntegraEMV.chkPp_Printer() != "1")
                        {
                            opcionmenu2[0] = '0';
                        }
                    }
                }
                else {
                    if (opcionesList[16] == '1')
                    {
                        if (Globales.cpIntegraEMV.chkPp_Printer() != "1")
                        {
                            opcionmenu2[0] = '0';
                        }
                    }                
                }
            }
            if(nombreMenuAux == "menu2"){
                if (menu1bool) opcionesList[16] = opcionmenu2[0];
                arregloNombre[0] = (Globales.isAerolinea)? "Ventas Misceláneos":arregloNombre[0];
                if (opcionesList[2] == '1')
                {
                    opcionmenu4[0] = '1';
                }
                if(opcionesList[6]=='1' || opcionesList[8]=='1'){
                    opcionesList[7] = '1';
                    arregloNombre[7] = "Re-Autorización";
                }
                if(TypeUsuario.AddTableNum){
                    opcionesList[11] = '0';
                    arregloNombre[10] = "Consumo";
                }
                //if(Globales.cpIntegraEMV.dbgGetPagoADistancia()){
                //    opcionesList[17] = '0';
                //}
                
            }
            //No muestra cambio de contraseña para súper pagos
            //En caso de ser MOTO no muetra el menú de lector de tarjetas ni pprueba de lector.
            if(nombreMenuAux == "menu4"){
                
                if (Globales.GetSettingString("Instalador") != "1")
                {

                }
                else {
                    opcionesList[1] = '0';
                    if(!(Globales.GetDataXml("tipopagoVMC",TypeUsuario.CadenaXML) != "") || Globales.GetDataXml("tipopagobAMEX",TypeUsuario.CadenaXML) != ""){
                        opcionesList[3] = '0';
                        opcionmenu5[0] = '0';
                    }
                }

                if (Globales.cpIntegraEMV.EsTouch())
                {
                    opcionesList[5] = '1';
                }
                else {
                    opcionesList[5] = '0';
                }
            }
            if("menu5" == nombreMenuAux){
                opcionesList[2] = opcionmenu5[0];
               if(arregloNombre[1] == "Acerca de EXPRESS"){
                   arregloNombre[1] = "Acerca de AeroPay PC";
               }
               opcionesList[2] = '0';
                if(Globales.isAgencias && TypeUsuario.CadenaXML.Contains("<PCPAYRP3>")){
                    arregloNombre[1] = "Acerca de PcPay";
                }
                if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("DCC_afiliaciones", TypeUsuario.CadenaXML)))
                {
                    opcionesList[5] = '1';
                }
                else {
                    opcionesList[5] = '0';
                }
            }
            
            if (opciones.Length == arregloNombre.Length)
            {
                

                var contador = 0;
                foreach (var item in opcionesList)
                {
                    menu = new MenuOpciones()
                    {
                        activo = (item == '1') ? true : false,
                        nombre = Globales.acentoOnString(arregloNombre[contador]),
                        id = nombreMenuAux + Globales.acentoOnString(arregloNombre[contador]).Replace(" ", "").Replace("-","_").Replace(".","_"),
                        idPadre = nombreMenuAux
                    };
                    contador++;
                    menuLista.Add(menu);
                    //Código parche                    
                }
            }
            return menuLista;
        }

        #region obtener productos del xml
        public static Dictionary<string, Categoria> getProductos()
        {
            leerXml leer = new leerXml(Globales.GetDataXml("RESPRODUCTOS", TypeUsuario.CadenaXML), "RESPRODUCTOS");
            var temp = leer.getElemento("RESPRODUCTOS");
            var padre = temp[0];
            var categorias = padre.ChildNodes;
            Categoria objCategoria;
            Dictionary<string, Categoria> categoriaX = new Dictionary<string, Categoria>();

            foreach (XmlNode item in categorias)
            {
                objCategoria = new Categoria();

                var hijos = item.ChildNodes;
                objCategoria.id_categoria = hijos[0].InnerText;
                objCategoria.descripcion = hijos[1].InnerText;
                objCategoria.st_capt_tel_imp = hijos[2].InnerText;
                for (int x = 3; x < hijos.Count; x++)
                {
                    XmlNodeList hijoProductos = hijos[x].ChildNodes;
                    productoCategoria producto = new productoCategoria();
                    producto.id_proveedor = hijoProductos[0].InnerText;
                    producto.desc_proveedo = hijoProductos[1].InnerText;
                    producto.id = hijoProductos[2].InnerText;
                    producto.descripcion = hijoProductos[3].InnerText;
                    objCategoria.listaProductos.Add(producto);
                }

                string nombreNodo = item.Name;
                categoriaX.Add(hijos[0].InnerText, objCategoria);
            }




            return categoriaX;
        }
        #endregion

    }
    public class MenuOpciones
    {
        public bool activo { get; set; }
        public string nombre { get; set; }

        public string idPadre { get; set; }
        public string id { get; set; }
    }

    public class leerXml {
        private XmlDocument xml { get; set; }
        public leerXml(string cadena,string tipo="") {
            string aux = cadena.Trim();
            aux = aux.Replace("\t","");
            string cadenita = string.Empty;
            for (int x = 0; x < aux.Length; x++ )
            {
                cadenita += aux.Substring(x, 1);
                if(aux.Substring(x,1)=="<"){
                    int numero = 0;
                    if (int.TryParse(aux.Substring((x + 1), 1), out numero))
                    {
                        cadenita += "a";
                        continue;
                    }
                }
                if (aux.Substring(x, 1) == "/")
                {
                    int numero = 0;
                    if (int.TryParse(aux.Substring((x + 1), 1), out numero))
                    {
                        cadenita += "a";
                        continue;
                    }
                }
                
            }
            try
            {
                if (tipo == "")
                {
                    aux = "<padre><menupcpayagencias>" + cadenita + "</menupcpayagencias></padre>";
                }
                else {
                    aux = "<padre><RESPRODUCTOS>" + cadenita + "</RESPRODUCTOS></padre>";
                }
                
                xml = new XmlDocument();
                aux = aux.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>","");
                aux = aux.Trim();
                xml.LoadXml(aux);
            }
            catch { 
               
            }
        }

        public XmlNodeList getElemento(string etiqueta) {
            XmlNodeList aux = null;
            try
            {
              aux =   xml.GetElementsByTagName(etiqueta);
            }
            catch {
                aux = null;
            }
            return aux;
        }
    }
    public class Categoria
    {
        public string descripcion { get; set; }
        public string st_capt_tel_imp { get; set; }
        public string id_categoria { get; set; }


        public List<productoCategoria> listaProductos { get; set; }
        public Categoria()
        {
            listaProductos = new List<productoCategoria>();
        }
    }
    public class productoCategoria
    {
        public string id_proveedor { get; set; }
        public string desc_proveedo { get; set; }
        public string id { get; set; }
        public string descripcion { get; set; }
    }
    
}
