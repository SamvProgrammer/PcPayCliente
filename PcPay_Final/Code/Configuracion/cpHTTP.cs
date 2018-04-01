using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PcPay.Code.Configuracion
{
    public static class cpHTTP
    {
        public static string cpHTTP_sResult;
        public static string cpHTTP_sError;
        public static string cpHTTP_sURL_cpCUCT;
        private static bool cpHTTP_bEnableLog { set; get; }

        public static void cpHTTP_Clear()
        {

            cpHTTP_ClearVariables();
            cpHTTP_ClearResponse();

        }

        private static void cpHTTP_ClearVariables()
        {

            cpHTTP_sResult = "";
            cpHTTP_sError = "";

        }

        private static void cpHTTP_ClearResponse()
        {

            cpHTTP_sResult = "";
            cpHTTP_sError = "";

        }


        public static string cpHTTP_cadena1 { get; set; }

        internal static bool cpHTTP_SendcpCUCT()
        {
            cpHTTP_ClearResponse();
            bool cpHTTP_SendcpCUCT = false;
            if ((cpHTTP_cadena1.Trim()).Length <= 0)
            {
                cpHTTP_sError = "Cadena Invalida";
                return false;
            }

            cpHTTP_cadena1 = cpHTTP_cadena1.Trim();
            cpHTTP_SendcpCUCT = cpHTTP_EnviarcpCUCT();

            return cpHTTP_SendcpCUCT;
        }

        private static bool cpHTTP_EnviarcpCUCT()
        {
            string pagLink = string.Empty;
            cpHTTP_wlog("Envia cpCUCT");
            string strDataToSend = string.Empty;

            string Respuesta = string.Empty;
            bool cpHTTP_EnviarcpCUCT = false;
         

                pagLink = cpHTTP_sURL_cpCUCT;// Necesario?

                strDataToSend = cpHTTP_cadena1;

                // Hacer peticion POST
                WebClient cliente = new WebClient();//Preparacion del cliente
                NameValueCollection valores = new NameValueCollection();//Preparacion de la coleccion (llave/valor)
                byte[] respuestaWeb;
                cliente.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                try
                {
                    respuestaWeb = cliente.UploadData(pagLink, "POST", System.Text.Encoding.ASCII.GetBytes(strDataToSend));
                    Respuesta = Encoding.Default.GetString(respuestaWeb);
                }
                catch { }

                //Respuesta = Lo que regrese el post
                cpHTTP_wlog(Respuesta);
                cpHTTP_wlog("============================================================");

                if (!Respuesta.Contains("<html>"))
                {
                    //'##### Minúsculas c/ acentos #####
                    Respuesta = Globales.acentoOnString(Respuesta);
                    //'##### Mayúsculas c/ acentos #####
                    //Respuesta = MitTools.acentoOnString(Respuesta,true);
                    Respuesta = Respuesta.Replace("NNN", "Ñ");

                }
                cpHTTP_sResult = Respuesta;
                cpHTTP_EnviarcpCUCT = true;
            
    

            return cpHTTP_EnviarcpCUCT;
        }
        private static void cpHTTP_wlog(string cadena)
        {
            if (cpHTTP_bEnableLog)
            {
                try
                {
                    string ruta = Globales.sPathUserLog + @"LogMITTransac.txt";
                    StreamWriter sw = new StreamWriter(ruta);
                    sw.WriteLine(cadena);

                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
