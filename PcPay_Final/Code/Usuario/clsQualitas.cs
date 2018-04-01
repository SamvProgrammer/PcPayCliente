using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcPay.Code.Usuario
{
    public static class typeUsuarioQualitas
    {

        public static bool isQualitas;                 //ES UN USUARIO DE QUALITAS
        public static bool isQualitasTrxValida;        //REALIZO UNA CONSULTA PARA QUALITAS
        public static bool isQualitasActualizado;      //SI FUE ACTUALIZADO EL PAGO
        public static bool isQualitasCierraForm;       //DESCARGA EL FORMULARIO, YA QUE SE ACTUALIZO
        public static bool isCoberturaMultiple;        //CUANDO EL DEDUCIBLE TIENE MULTIPLES COBERTURAS
        public static int numTotalCoberturas;
        public static int contCoberturas;




        //Public docXML As New MSXML2.DOMDocument
        //Public nodeListXML As MSXML2.IXMLDOMNodeList
        //Public nodeXML As MSXML2.IXMLDOMNode

        //Public Type usuarioQualitas
        //    TipoCobro As String
        //    NumPoliza As String
        //    NumSiniestro As String

        //    'Propiedades para Deducible y Póliza
        //    RespuestaCodigo As String
        //    RespuestaMensaje As String
        //    TipoPagosContado As String
        //    TipoPagosMSI As String
        //    TipoPagosMSIPlan As String

        //    '***************************************************
        //    PolizaAsegurado As String
        //    PolizaMoneda As String
        //    PolizaNumero As String

        //    PolizaReciboEndoso As String
        //    PolizaReciboEstatus As String
        //    PolizaReciboImporte As String
        //    PolizaReciboNumero As String
        //    PolizaReciboVencimiento As String


        //    PolizaVehiculoDescripcion As String
        //    PolizaVehiculoModelo As String
        //    PolizaVehiculoSerie As String

        //    '***************************************************
        //    'DeducibleAsegurado As String
        //    DeducibleIdAsegurado As String
        //    DeducibleNombreAsegurado As String
        //    DeducibleEndoso As String
        //    DeducibleInciso As String
        //    DeducibleMoneda As String
        //    DeduciblePoliza As String
        //    DeducibleReporte As String
        //    DeducibleSiniestro As String



        //    DeducibleValoracion As String
        //    DeducibleCoberturaAplicaDeducible As String
        //    DeducibleCoberturaCodigo As String
        //    DeducibleCoberturaDescripcion As String
        //    DeducibleCoberturaMonto As String


        //    DeducibleVehiculoDescripcion As String
        //    DeducibleVehiculoModelo As String
        //    DeducibleVehiculoSerie As String




        //End Type

        //Public typeUsuarioQualitas As usuarioQualitas

        public const string userQualitasWS = "userPcP4y";
        public const string passQualitasWS = "1dF23.-o/";
        public const string userQualitas = "0029";
        public const string sucursalQualitas = "-666-";

        public const string ipQualitas = "https://www.qualitas.com.mx/agentes/pcpay-ws/servicios/PcPayService?wsdl";  //PUBLICA QA





        public static string TipoCobro { get; set; }

        public static string NumPoliza { get; set; }

        public static string NumSiniestro { get; set; }

        public static string RespuestaCodigo { get; set; }

        public static string RespuestaMensaje { get; set; }

        public static string TipoPagosContado { get; set; }

        public static string TipoPagosMSIPlan { get; set; }

        public static string TipoPagosMSI { get; set; }

        public static string PolizaAsegurado { get; set; }

        public static string PolizaMoneda { get; set; }

        public static string PolizaNumero { get; set; }

        public static string PolizaReciboEndoso { get; set; }

        public static string DeducibleInciso { get; set; }

        public static string DeducibleMoneda { get; set; }

        public static string DeduciblePoliza { get; set; }

        public static string DeducibleReporte { get; set; }

        public static string DeducibleSiniestro { get; set; }

        public static string DeducibleValoracion { get; set; }

        public static string DeducibleCoberturaAplicaDeducible { get; set; }

        public static string DeducibleCoberturaCodigo { get; set; }

        public static string DeducibleCoberturaDescripcion { get; set; }

        public static string DeducibleCoberturaMonto { get; set; }

        public static string DeducibleVehiculoDescripcion { get; set; }

        public static string DeducibleVehiculoModelo { get; set; }

        public static string DeducibleVehiculoSerie { get; set; }

        public static string PolizaReciboEstatus { get; set; }

        public static string PolizaReciboImporte { get; set; }

        public static string PolizaReciboNumero { get; set; }

        public static string PolizaReciboVencimiento { get; set; }

        public static string PolizaVehiculoDescripcion { get; set; }

        public static string PolizaVehiculoModelo { get; set; }

        public static string PolizaVehiculoSerie { get; set; }

        public static string DeducibleEndoso { get; set; }

        public static string DeducibleNombreAsegurado { get; set; }

        public static string DeducibleIdAsegurado { get; set; }
    }
}
