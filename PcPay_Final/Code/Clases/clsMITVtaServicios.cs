using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcPay.Code.Clases
{
    public class clsMITVtaServicios
    {
        private string VsURL { get; set; }
        private const string KTGAPPWEB = "#<?V>E=ZqRmW*:9%rH7";
        public void dbgSetVsUrl(string valor)
        {
            VsURL = valor;
        }
        public string dbgGetVsUrl()
        {
            return VsURL;
        }
        public string getRspVsXML() {
            return VsXml;
        }

        internal void sndVsRequestTx(string Bs_user, string Bs_Pwd, string Bs_Company, string Bs_Branch, string Vs_Proveedor, string Vs_IdCategoria, string Vs_idProducto)
        {
            string strXml = string.Empty;
            dbgClearDLL();
            strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            strXml += "<REQVTASERV>";
            strXml += "<id_company>" + Bs_Company + "</id_company>";
            strXml += "<id_branch>" + Bs_Branch + "</id_branch>";
            strXml += "<user>" + Bs_user + "</user>";
            strXml += "<pwd>" + Bs_Pwd + "</pwd>";
            strXml += "<id_proveedor>" + Vs_Proveedor + "</id_proveedor>";
            strXml += "<id_categoria>" + Vs_IdCategoria + "</id_categoria>";
            strXml += "<id_producto>" + Vs_idProducto + "</id_producto>";
            strXml += "</REQVTASERV>";
            strXml = Globales.EncryptTripleDES(strXml, KTGAPPWEB);
            Globales.WS.WS_Url = VsURL + "/pgs/services/ventaProducto"; ;
            Globales.WS.WS_Action = "http:" + "//server.ventaproducto.webservice.cpagos";
            Globales.WS.ClearWS();
            Globales.WS.WS_Params = strXml;
            Globales.WS.WS_nbParams = "in0";
            Globales.WS.WS_Method = "getVentaProduto";
            Globales.WS.MITWebServices(Globales.WS.WS_Params, Globales.WS.WS_nbParams, Globales.WS.WS_Method);
            string respuesta = Globales.WS.WS_Response;
            respuesta = Globales.GetDataXml("ns1:out", respuesta);
            Globales.WS.WS_Response = Globales.DecryptTripleDES(respuesta, KTGAPPWEB);
            SetRspVsXML(Globales.WS.WS_Response);

            setRspVsIdProducto(Globales.GetDataXml("id_producto", VsXml));
            setRspVsIdProveedor(Globales.GetDataXml("id_proveedor", VsXml));
            setRspVsImporte(Globales.GetDataXml("importe", VsXml));
            setRspVsExistencia(Globales.GetDataXml("existencia", VsXml));
            setRspVsTarjeta(Globales.GetDataXml("tarjeta", VsXml));
            setRspVsEfectivo(Globales.GetDataXml("efectivo", VsXml));
            setRspVsCatBancos(Globales.GetDataXml("catbancos", VsXml));
            setRspVsxmlBancos(Globales.GetDataXml("ventas3gate", VsXml));
            setRspVsReferencias(Globales.GetDataXml("referencia", VsXml));

            setRspVsUser(Globales.GetDataXml("usr", Globales.GetDataXml("ventas3gate", VsXml)));
            setRspVsPassword(Globales.GetDataXml("psw", Globales.GetDataXml("ventas3gate", VsXml)));
            setRspVsCompany(Globales.GetDataXml("company", Globales.GetDataXml("ventas3gate", VsXml)));
            setRspVsBranch(Globales.GetDataXml("branch", Globales.GetDataXml("ventas3gate", VsXml)));
        }

        private void setRspVsBranch(string p)
        {
            VsBranch = p;
        }

        private void setRspVsCompany(string p)
        {
            VsCompany = p;
        }

        private void setRspVsPassword(string p)
        {
            VsPassword = p;
        }

        private void setRspVsUser(string p)
        {
            VsUser = p;
        }

        private void setRspVsReferencias(string p)
        {
            VsReferencia = p;
        }

        private void setRspVsxmlBancos(string p)
        {
            VsXmlBancos = p;
        }

        private void setRspVsCatBancos(string p)
        {
            VsCatBancos = p;
        }

        private void setRspVsEfectivo(string p)
        {
            VsEfectivo = p;
        }

        private void setRspVsTarjeta(string p)
        {
            VsTarjeta = p;
        }

        private void setRspVsExistencia(string p)
        {
            VsExistencia = p;
        }

        private void setRspVsImporte(string p)
        {
            VsImporte = p;
        }

        private void setRspVsIdProveedor(string p)
        {
            VsIdProveedor = p;
        }

        private void setRspVsIdProducto(string p)
        {
            VsIdProducto = p;
        }



        private void SetRspVsXML(string value)
        {
            VsXml = value;
        }

        private void dbgClearDLL()
        {
            VsXml = "";
            VsIdProducto = "";
            VsIdProveedor = "";
            VsImporte = "";
            VsTarjeta = "";
            VsEfectivo = "";
            VsExistencia = "";
            VsCatBancos = "";
            VsXmlBancos = "";
            VsRecibo = "";
            VsReferencia = "";
        }

        public string VsXml { get; set; }

        public string VsIdProducto { get; set; }

        public string VsIdProveedor { get; set; }

        public string VsImporte { get; set; }

        public string VsTarjeta { get; set; }

        public string VsEfectivo { get; set; }

        public string VsExistencia { get; set; }

        public string VsCatBancos { get; set; }

        public string VsXmlBancos { get; set; }

        public string VsRecibo { get; set; }

        public string VsReferencia { get; set; }

        public string VsBranch { get; set; }

        public string VsCompany { get; set; }

        public string VsPassword { get; set; }

        public string VsUser { get; set; }

        internal string getRspVsIdProducto()
        {
            return VsIdProducto;
        }



        internal string getRspVsExistencia()
        {
            return VsExistencia;
        }

        internal string getRspVsEfectivo()
        {
            return VsEfectivo;
        }

        internal string getRspVsTarjeta()
        {
            return VsTarjeta;
        }

        internal string getRspVsImporte()
        {
            return VsImporte;
        }

        internal string getRspVsReferencia()
        {
            return VsReferencia;
        }

        internal string dbgVSHTMLReport(string User, string Pwd, string Id_Branch, string Id_Company)
        {
            string strParmetros = string.Empty;
            strParmetros = "&usuario="+User+
                           "&password="+Pwd+
                           "&sucursal="+Id_Branch+
                           "&cdEmpresa="+Id_Company;
            strParmetros = Globales.EncryptTripleDES(strParmetros,Globales.KTGAPPWEB);
            Globales.WS.WS_Url = VsURL + "/pgs/services/ventaProducto";
            Globales.WS.WS_Action = "http://"+"server.referencias.webservice.cpagos";
            Globales.WS.ClearWS();
            Globales.WS.WS_Params = strParmetros;
            Globales.WS.WS_nbParams = "in0";
            Globales.WS.WS_Method = "getReporte";
            Globales.WS.MITWebServices(Globales.WS.WS_Params,Globales.WS.WS_nbParams,Globales.WS.WS_Method);
            var respustaN = Globales.GetDataXml("ns1:out", Globales.WS.WS_Response);
            Globales.WS.WS_Response = Globales.DecryptTripleDES(respustaN, Globales.KTGAPPWEB);
            return Globales.WS.WS_Response;
        }
    }
}
