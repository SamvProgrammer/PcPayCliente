using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PcPay.Code.Clases
{
     public class MITWebService
    {
        public string WS_Params { get; set; }
        public string WS_nbParams { get; set; }
        public string WS_Method { get; set; }
        public string WS_Response { get; set; }
        public string WS_Action { get; set; } // = http: //wscontrol.mitec.com
        public string WS_Url { get; set; }//= // https:// www.controldeproductividad.com.mx/MITAccessControl/service/MITAccessCon trolService
        public void ClearWS()
        {
            WS_Params = "";
            WS_nbParams = "";
            WS_Method = "";
            WS_Response = "";
        }
        public void MITWebServices(string Params, string nbParams, string Method)
        {
            try
            {
                string strXml = string.Empty;
                string M = string.Empty;
                string strAux = string.Empty;
                string Strx = string.Empty;
                string StrxParams = string.Empty;
                string[] P;
                string strCadena = string.Empty;

                string strAuxNP = string.Empty;
                string StrxNP = string.Empty;
                string StrxParamsNP = string.Empty;
                string[] NP;
                string strCadenaNP = string.Empty;
                string PAR = string.Empty;
                int i, K, k, l = 0;



                M = Method;
                StrxParams = Params + ",";
                strCadena = Params;
                StrxParamsNP = nbParams + ",";
                strCadenaNP = nbParams;


                P = strCadena.Split(',');
                NP = strCadenaNP.Split(',');



                //Faltan poner bien las cosas del webService pero lo vi inecesario..
                for (int x = 0; x < NP.Length; x++)
                {
                    if (!string.IsNullOrWhiteSpace(NP[x]))
                    {
                        PAR = PAR + "<q0:" + NP[x] + ">" + P[x] + "</q0:" + NP[x] + ">";
                    }
                }
                Method = "q0:" + Method;
                strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<soapenv:Envelope xmlns:soapenv=\"http:" + "//schemas.xmlsoap.org/soap/envelope/\" xmlns:q0=\"" + WS_Action + "\" xmlns:xsd=\"http://" + "www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://" + "www.w3.org/2001/XMLSchema-instance\">" +
                    "<soapenv:Body><" + Method + ">" + PAR + "</" + Method + "></soapenv:Body>" +
                    "</soapenv:Envelope>";
                
                WS_Response = PostWebservice(WS_Url, WS_Action, strXml, M);
            }
            catch
            {

            }
        }

        public string PostWebservice(string AsmlUrl, string SoapActionUrl, string xmlBody, string Method)
        {
            byte[] respuestaWeb;
            string respuesta = string.Empty;
            try
            {
                WebClient cliente = new WebClient();
                byte[] datoEnviar = Encoding.UTF8.GetBytes(xmlBody);
                cliente.Headers.Add(HttpRequestHeader.ContentType, "text/xml;charset=utf-8");
                cliente.Headers.Add("SOAPAction", SoapActionUrl);
                respuesta = cliente.UploadString(AsmlUrl, xmlBody);
                //XmlDocument n = new XmlDocument();
                //n.LoadXml(respuesta);
                //var nsmgr = new XmlNamespaceManager(n.NameTable);
                //nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
               // respuesta = n.SelectSingleNode("/soap:Envelope/soap:Body/ns1:"+Method+"Response"+"/ns1:out",nsmgr).InnerText;
                //respuesta = Encoding.Default.GetString(respuestaWeb);
            }
            catch
            {

            }
            return respuesta;
        }

        internal string PostWebserviceSinSOAPAction(string XmlBody, string AsmxUrl)
        {
            try {
                byte[] respuestaWeb;
                string respuesta = string.Empty;
                int intentos = 0;
                try
                {
                    reintenta:
                    WebClient cliente = new WebClient();
                    byte[] datoEnviar = Encoding.UTF8.GetBytes(XmlBody);
                    //cliente.Headers.Add(HttpRequestHeader.ContentType, "text/xml;charset=utf-8");
                    //cliente.Headers.Add("SOAPAction",SoapActionUrl);
                    
                    if (Globales.isQualitas)
                    {
                        cliente.UseDefaultCredentials = true;
                        cliente.Credentials = new NetworkCredential(Globales.userQualitasWS,Globales.passQualitasWS);
                        respuesta = cliente.UploadString(AsmxUrl, XmlBody);
                    }
                    else {
                        respuesta = cliente.UploadString(AsmxUrl, XmlBody);
                    }
                    if(respuesta == "" || respuesta.Contains("Error")){
                        intentos++;
                        if(intentos <= 2){
                            goto reintenta;
                        }
                    }
                    //respuesta = Encoding.Default.GetString(respuestaWeb);
                }
                catch
                {

                }
                return respuesta;
            }
            catch
            {

                return "Error en la petición";
            }
        }
    }
}
