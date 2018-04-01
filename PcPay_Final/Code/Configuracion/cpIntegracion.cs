using PcPay.Code.Usuario;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PcPay.Code.Configuracion
{
    class cpIntegracion
    {
        public static string strXML { get; set; }
        public static string cpIntegracion_sResult { get; set; }
        public static string cpIntegracion_sError { get; set; }
        internal static void cpIntegracion_Clear()
        {
            try
            {
                cpIntegracion_sResult = "";
                cpIntegracion_sError = "";                
            }
            catch { 
            
            }
        }

        public static string sURL_cpINTEGRA { get; set; }

        internal static bool cpIntegracion_EnviarcpINTEGRA()
        {
            bool valor = false;
            try
            {
                WebClient cliente2 = new WebClient();
                NameValueCollection coleccion = new NameValueCollection();
                coleccion.Add("enc",strXML);
                byte[] resultado = cliente2.UploadValues(Globales.cpIntegracion_sURL_cpINTEGRA,coleccion);
                if(resultado.Length == 0){
                    return false;
                }
                string resultado2 = Encoding.Default.GetString(resultado);
                resultado2 = resultado2.Replace("aaa", "á");
                resultado2 = resultado2.Replace("eee", "é");
                resultado2 = resultado2.Replace("iii", "í");
                resultado2 = resultado2.Replace("ooo", "ó");
                resultado2 = resultado2.Replace("uuu", "ú");
                resultado2 = resultado2.Replace("nnn", "ñ");
                resultado2 = resultado2.Replace("ÑÑÑ", "Ñ");

                Globales.cpIntegracion_sResult = resultado2;
                string voucher, voucherAct;
                if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("", Globales.cpIntegracion_sResult)))
                {
                    string comercio, cliente;
                    comercio = Globales.CheckVoucher(Globales.GetDataXml("voucher_comercio", Globales.cpIntegracion_sResult));
                    cliente = Globales.CheckVoucher(Globales.GetDataXml("voucher_cliente", Globales.cpIntegracion_sResult));
                    voucher = "<voucher_comercio>" + comercio + "</voucher_comercio><voucher_cliente>" + cliente + "</voucher_cliente>  @";
                    voucherAct = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult);
                    if (TypeUsuario.TipoImpresora == "6")
                    {
                        Globales.cpIntegracion_sResult = Globales.cpIntegracion_sResult.Replace(voucherAct, voucher);
                    }
                    valor = true;
                }
                else {
                    valor = false;
                }

            }
            catch {
                valor = false;
            }
            return valor;
        }
    }
}
