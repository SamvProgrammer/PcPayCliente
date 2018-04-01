using PcPay.Code.Usuario;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PcPay.Code.Utilidades
{
    public class cpIntegracion
    {
        public static void cpIntegracion_cpAVSs2(string Id_Company, string Id_Branch, string country, string User,
                                    string Pwd,
                                    string merchant,
                                    string reference,
                                    string tp_operation,
                                    string typeC,
                                    string nameC,
                                    string numberC,
                                    string expmonthC,
                                    string expyearC,
                                    string cvvcscC,
                                    string Amount,
                                    string currencyC,
                                    string direccion,
                                    string NumInt,
                                    string NumExt,
                                    string delegacion,
                                    string ciudad,
                                    string Estado,
                                    string cp,
                                    string colonia,
                                    string nombreC,
                                    string PaisC,
                                    string TelefonoC,
                                    string CorreoC, string Tx_isCheckin, string Tx_boletos = "", string Tx_fechaSalida = "", string Tx_fechaRetorno = "")
        {


            cpIntegracion_Clear();

            try
            {
                string strXML = string.Empty;
                strXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                strXML += "<PAYMENTSDOS>";
                strXML += "<business>";
                strXML += "<bs_idCompany>" + Id_Company + "</bs_idCompany>";
                strXML += "<bs_idBranch>" + Id_Branch + "</bs_idBranch>";
                strXML += "<bs_country>" + country + "</bs_country>";
                strXML += "<bs_user>" + User + "</bs_user>";
                strXML += "<bs_pwd>" + Pwd + "</bs_pwd>";
                strXML += "</business>";
                strXML += "<transaction>";
                strXML += "<tx_merchant>" + merchant + "</tx_merchant>";
                strXML += "<tx_reference>" + reference + "</tx_reference>";
                strXML += "<tx_tpOperation>" + tp_operation + "</tx_tpOperation>";
                strXML += "<creditcard>";
                strXML += "<cc_type>" + typeC + "</cc_type>";
                strXML += "<cc_name>" + nameC + "</cc_name> ";
                strXML += "<cc_number>" + numberC + "</cc_number>";
                strXML += "<cc_expMonth>" + expmonthC + "</cc_expMonth> ";
                strXML += "<cc_expYear>" + expyearC + "</cc_expYear>";
                strXML += "<cc_cvv>" + cvvcscC + "</cc_cvv>";
                strXML += "</creditcard>";
                strXML += "<tx_amount>" + Amount + "</tx_amount>";
                strXML += "<tx_currency>" + currencyC + "</tx_currency>";
                strXML += "<tx_userTransaction></tx_userTransaction>";
                strXML += "<tx_version>pcpay " + 9 + "." + 1 + "." + 0 + "</tx_version>";


                if (Tx_isCheckin == "1")
                {
                    strXML += "<tx_ischeckin>" + Tx_isCheckin + "</tx_ischeckin>";
                }

                if (!vacio(Tx_boletos))
                {
                    strXML += "<tx_ticketNumber>" + Tx_boletos + "</tx_ticketNumber>";
                }

                if (!vacio(Tx_fechaSalida))
                {
                    strXML += "<tx_departureDate>" + Tx_fechaSalida + "</tx_departureDate>";
                }

                if (!vacio(Tx_fechaRetorno))
                {
                    strXML += "<tx_completionDate>" + Tx_fechaRetorno + "</tx_completionDate>";
                }

                strXML += "</transaction>";
                strXML += "<sdos>";
                strXML += "<sdos_billingname>" + nombreC + "</sdos_billingname>";
                strXML += "<sdos_billingStreet>" + direccion + "</sdos_billingStreet>";
                strXML += "<sdos_billingNeighborhood>" + colonia + "</sdos_billingNeighborhood>";
                strXML += "<sdos_billingMunicipality>" + delegacion + "</sdos_billingMunicipality>";
                strXML += "<sdos_billingCity>" + ciudad + "</sdos_billingCity>";
                strXML += "<sdos_billingState>" + Estado + "</sdos_billingState>";
                strXML += "<sdos_billingZipCode>" + cp + "</sdos_billingZipCode>";
                strXML += "<sdos_billingCountry>" + PaisC + "</sdos_billingCountry>";
                strXML += "<sdos_billingPhone>" + TelefonoC + "</sdos_billingPhone>";
                strXML += "<sdos_billingEmail>" + CorreoC + "</sdos_billingEmail>";

                strXML += "<sdos_billingStreetNumber>" + NumExt + "</sdos_billingStreetNumber>";

                if (!vacio(NumInt))
                {
                    strXML = strXML + "<sdos_billingStreetNumberInt>" + NumInt + "</sdos_billingStreetNumberInt>";
                }

                strXML += "</sdos>";

                // 'Se agrega para enviar la bandarea de MCI. PcPay 7.2.1. */ AGG \*
                if (!vacio(dbgGetPlazoMCI()))
                {
                    strXML = strXML + "<mci><plazomci>" + dbgGetPlazoMCI() + "</plazomci></mci>";
                }

                strXML += "</PAYMENTSDOS>";

                strXML = Globales.EncryptTripleDES(strXML, Globales.generarSemilla("PCPAY" + Id_Company + merchant));

                Globales.WS.WS_Url = Globales.ip + "/wscobroSdos/services/wscobroSdos";//' ?wsdl'cpIntegracion_sURL_cpINTEGRA & "/wscobroSdos/services/wscobroSdos"
                Globales.WS.WS_Action = "http" + "://wscobrosdos.mit.com";
                Globales.WS.ClearWS();
                Globales.WS.WS_Params = Id_Company + "," + merchant + "," + strXML;
                Globales.WS.WS_nbParams = "strIdCompany,strIdMerchant,strXml";
                Globales.WS.WS_Method = "cobroSdos";
                Globales.WS.MITWebServices(Globales.WS.WS_Params, Globales.WS.WS_nbParams, Globales.WS.WS_Method);
                cpIntegracion_sResult = Globales.WS.WS_Response;
                if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("response", Globales.WS.WS_Response)))
                {
                    cpIntegracion_sResult = Globales.WS.WS_Response;
                }

            }

            catch (Exception Err)
            {

                //    cpIntegracion_sError = "cpIntegracion_cpAVSM(" + ": " + Err.Message + ")";

            }
        }
        public static bool vacio(string texto)
        {
            return String.IsNullOrEmpty(texto);
        }
        public static string dbgGetPlazoMCI()
        {
            return Globales.cpIntegraEMV.dbgGetPlazoMCI();
        }

        public static string cpIntegracion_sResult { get; set; }

        internal static void cpIntegracion_Clear()
        {
            cpIntegracion_sResult = "";
            cpIntegracion_sError = "";
            strXML = "";
        }

        public static string strXML { get; set; }

        public static string cpIntegracion_sError { get; set; }

        public static string cpIntegracion_sURL_cpINTEGRA { get; set; }

        public static string sURL_cpINTEGRA { get; set; }

        internal static bool cpIntegracion_cpVtaForzadaM(string Id_Company,
            string Id_Branch,
            string country,
            string User,
            string Pwd,
            string merchant,
            string reference,
            string tp_operacion,
            string typeC,
            string nameC,
            string numberC,
            string expMonthC,
            string expyearC,
            string cvvcscC,
            string Amount,
            string currencyC,
            string no_operacion,
            string auth,
            string iCFA = "")
        {
            bool valor = false;
            try
            {
                Globales.cpIntegraEMV.dbgClearDLL();
                Globales.cpIntegracion_Clear();
                strXML += "<VMCAMEXMFORZADA>";
                strXML += "<business>";
                strXML += "<id_company>" + Id_Company + "</id_company>";
                strXML += "<id_branch>" + Id_Branch + "</id_branch>";
                strXML += "<country>" + country + "</country>";
                strXML += "<user>" + User + "</user>";
                strXML += "<pwd>" + Globales.encryptString(Pwd, Globales.KEY_RC4_CP, true) + "</pwd>";
                strXML += "</business>";
                strXML += "<transacction>";
                strXML += "<merchant>" + merchant + "</merchant>";
                strXML += "<reference>" + reference + "</reference>";
                strXML += "<tp_operation>" + tp_operacion + "</tp_operation>";
                strXML += "<creditcard>";
                strXML += "<crypto>" + Globales.CRYPTO + "</crypto>";
                strXML += "<type>" + typeC + "</type>";
                strXML += "<name>" + Globales.encryptString(nameC, Globales.KEY_RC4_CP, true) + "</name> ";
                strXML += "<number>" + Globales.encryptString(numberC, Globales.KEY_RC4_CP, true) + "</number>";
                strXML += "<expmonth>" + Globales.encryptString(expMonthC, Globales.KEY_RC4_CP, true) + "</expmonth> ";
                strXML += "<expyear>" + Globales.encryptString(expyearC, Globales.KEY_RC4_CP, true) + "</expyear>";
                strXML += "<cvv-csc>" + Globales.encryptString(cvvcscC, Globales.KEY_RC4_CP, true) + "</cvv-csc>";
                strXML += "</creditcard>";
                strXML += "<amount>" + Amount + "</amount>";
                strXML += "<currency>" + currencyC + "</currency>";
                if (no_operacion.Length == 9)
                {
                    strXML += "<no_operacion>" + no_operacion + "</no_operacion>";
                }
                strXML += "<auth>" + auth + "</auth>";

                //'Nuevo Tag en el esquema que le informa al cp con que aplicación
                //'y que versión se mando la transacción.
                strXML += "<version>" + "9.1.0" + "</version>";

                strXML += "</transacction>";

                //'Se agrega para enviar la bandarea de MCI. PcPay 7.2.1. */ AGG \*
                if (!string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.dbgGetPlazoMCI()))
                {
                    strXML += "<mci><plazomci>" + Globales.cpIntegraEMV.dbgGetPlazoMCI() + "</plazomci></mci>";
                }




                strXML += "</VMCAMEXMFORZADA>";
                valor = cpIntegracion_EnviarcpINTEGRA();
            }
            catch
            {
                valor = false;
                cpIntegracion_sError = "cpIntegracion_cpVtaForzadaM(Error)";
            }
            return valor;
        }

        private static bool cpIntegracion_EnviarcpINTEGRA()
        {
            bool valor = false;
            try
            {
                string pagLink = string.Empty;
                string respuesta = string.Empty;
                //Cosas del wlog
                valor = false;

                pagLink = cpIntegracion_sURL_cpINTEGRA;
                //cpintegracion_wlog/(;)
                WebClient peticion = new WebClient();
                NameValueCollection valores = new NameValueCollection();
              //  strXML = strXML.Replace("%", "%25");
              //  strXML = strXML.Replace(" ", "%20");
                string[] splits1;
                splits1 = strXML.Split('=');
                valores.Add(splits1[0], splits1[1]);
                byte[] respuestaweb = peticion.UploadValues(pagLink,"POST", valores);
                respuesta = Encoding.Default.GetString(respuestaweb);
                respuesta = respuesta.Replace("aaa", "á");
                respuesta = respuesta.Replace("eee", "é");
                respuesta = respuesta.Replace("iii", "í");
                respuesta = respuesta.Replace("ooo", "ó");
                respuesta = respuesta.Replace("uuu", "ú");
                respuesta = respuesta.Replace("NNNN", "Ñ");
                respuesta = respuesta.Replace("nnn", "ñ");
                cpIntegracion_sResult = respuesta;

                string Voucher = string.Empty;
                string VoucherAct = string.Empty;
                if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("voucher_comercio", cpIntegracion_sResult)))
                {
                    string comercio;
                    string cliente;
                    comercio = Globales.CheckVoucher(Globales.GetDataXml("voucher_comercio", cpIntegracion_sResult));
                    cliente = Globales.CheckVoucher(Globales.GetDataXml("voucher_cliente", cpIntegracion_sResult));
                    Voucher = "<voucher_comercio>" + comercio + "</voucher_comercio><voucher_cliente>" + cliente + "</voucher_cliente>    @";
                    VoucherAct = Globales.GetDataXml("voucher", cpIntegracion_sResult);
                    if (TypeUsuario.TipoImpresora == "6")
                    {
                        cpIntegracion_sResult = cpIntegracion_sResult.Replace(VoucherAct, Voucher);
                    }
                }
                valor = true;
            }
            catch
            {
                cpIntegracion_sError = "Verifique su conexión a internet antes de continuar";
            }


            return valor;
        }

        internal static string CheckVoucher(string strVoucherCop)
        {
            string strc, strcRef;
            string valor = "";
            if (strVoucherCop == null)
            {
                return "";
            }
            if (!string.IsNullOrWhiteSpace(strVoucherCop))
            {
                if (strVoucherCop.Contains(":"))
                {
                    strVoucherCop = Globales.DecryptString(strVoucherCop, "KEY CREDIT CARD KEY", true);
                }
                if (strVoucherCop.Contains("@lsn POR ESTE PAGARE ME OBLIGO INCONDI"))
                {
                    strVoucherCop = strVoucherCop.Substring(0, strVoucherCop.IndexOf("@lsn POR ESTE PAGARE ME OBLIGO INCONDI"));
                }
                //Falta cositas del voucher checar en cpIntegracion.bas cliente

                strVoucherCop = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(strVoucherCop));
                strVoucherCop = strVoucherCop.Replace("MXN", "MXN  ");
                valor = strVoucherCop;
            }


            return valor;
        }
    }
}
