using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PcPay.Code.Usuario;
using PcPay.Code.Configuracion;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;

using PcPay.Forms;
using System.Threading;
using PcPay.Code.Clases;
using System.Net;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Drawing.Printing;

namespace PcPay.Code.Utilidades
{
    public static class modMIT
    {
        public static cpIntegracionEMV.clsServicios CpCobro3G = new cpIntegracionEMV.clsServicios();
        //public static cpIntegracionEMV.clsServicios TiempoAire = new cpIntegracionEMV.clsServicios();
        public static Clases.clsMIT3Gate TGate = new Clases.clsMIT3Gate();

        public static Clases.clsMITVtaServicios VServicios = new Clases.clsMITVtaServicios();
        public static MITWebService WS = new MITWebService();
        public static string URLLOGIN = Globales.ip + "/pgs/pcpayLogin8";
        private const string NOMBRE_GENERAL = "modMIT";

        //public static pcpay.clsCpIntegracionEMV cpCobro = new pcpay.clsCpIntegracionEMV();
        public static Font printFont;
        public static StreamReader streamToPrint;
        public static System.Windows.Forms.PrintDialog printDialog1 = new System.Windows.Forms.PrintDialog();
        public static PrintDocument printDocument1 = new PrintDocument();

        public static string NOMBRE_APP
        {
            get { return Program.NOMBRE_APP; }
            set
            {
            Program.NOMBRE_APP = value;
        } }
        public static string Update;
        public static string FacturaE;
        public static bool NoReverso;
        public static string MerchantGC;
        public static string MsjReferencia;
        public static string MsjImporte;
        public static string Msjtarjeta;
        public static string MsjMes;
        public static string MsjAnyo;
        public static string MsjNombre;
        public static string MsjCvv;

        //Instancia para cobrar en 3Gate y CobroServicios
        //public static pcpay.clsCpIntegracionEMV CpCobro = new pcpay.clsCpIntegracionEMV();


        public static bool isAgencias;
        public static bool isAerolinea;
        public static int intKindofImage;
        public static int BanImg;

        //Llaves
        public static string KEY_CP_DYNAMIC;
        public const string KEY_RC4_CP = "KEY CREDIT CARD KEY";
        public const string KEY_RC4 = "KEY AEROPCPAY KEY";


        public static string URL_REF;
        public static string URL_REVERSO;
        public static string URL_DLL;
        public static string URL_DLL_VF;
        public static string URL_DLL_RA;
        public static string URL_DLL_CANC;

        public static string URL_DLL_CHECKIN;
        public static string URL_DLL_CHECKOUT;
        public static string URL_DLL_PREVENTA;
        public static string URL_DLL_PROPINA;
        public static string URL_DLL_CIERREPREVENTA;

        public static string URL_GIFT;
        public static string URL_3GATE;
        public static string URL_VTASERV;
        public static string URL_PUBLICIDAD;
        public static string URL_POINTS2;
        public static string URL_LOGININSTALADOR;

        public static string URL_REPORTECYC;


        //***************** PAGO FACIL ***********************
        public static bool esPagoFacil;
        public static string IdAdquiriente;


        //*************** USUARIO RECOMPENSAS SANTANDER SIN PRESENCIA ********************
        public const string userSantanderVta = "H363";
        //Public Const userSantanderVta = "A415"
        //********************************************************************************

        //*************** USUARIO TOKENIZACION ********************
        public static bool userTokenizacion;
        public static bool setReader;
        //********************************************************************************

        public static bool cpIntegracion_cpAVSs2(string Id_Company, string Id_Branch, string country, string User,
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

            Globales.cpIntegraEMV.dbgClearDLL();
            //Globales.cpIntegracion_Clear();
            bool cpIntegracion_cpAVSs2 = false;
            string strXML;
            strXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            strXML = strXML + "<PAYMENTSDOS>";
            strXML = strXML + "<business>";
            strXML = strXML + "<bs_idCompany>" + Id_Company + "</bs_idCompany>";
            strXML = strXML + "<bs_idBranch>" + Id_Branch + "</bs_idBranch>";
            strXML = strXML + "<bs_country>" + country + "</bs_country>";
            strXML = strXML + "<bs_user>" + User + "</bs_user>";
            strXML = strXML + "<bs_pwd>" + Pwd + "</bs_pwd>";
            strXML = strXML + "</business>";
            strXML = strXML + "<transaction>";
            strXML = strXML + "<tx_merchant>" + merchant + "</tx_merchant>";
            strXML = strXML + "<tx_reference>" + reference + "</tx_reference>";
            strXML = strXML + "<tx_tpOperation>" + tp_operation + "</tx_tpOperation>";
            strXML = strXML + "<creditcard>";
            strXML = strXML + "<cc_type>" + typeC + "</cc_type>";
            strXML = strXML + "<cc_name>" + nameC + "</cc_name> ";
            strXML = strXML + "<cc_number>" + numberC + "</cc_number>";
            strXML = strXML + "<cc_expMonth>" + expmonthC + "</cc_expMonth> ";
            strXML = strXML + "<cc_expYear>" + expyearC + "</cc_expYear>";
            strXML = strXML + "<cc_cvv>" + cvvcscC + "</cc_cvv>";
            strXML = strXML + "</creditcard>";
            strXML = strXML + "<tx_amount>" + Amount + "</tx_amount>";
            strXML = strXML + "<tx_currency>" + currencyC + "</tx_currency>";
            strXML = strXML + "<tx_userTransaction></tx_userTransaction>";
            strXML = strXML + "<tx_version>pcpay " + "Version" + "</tx_version>";


            if (Tx_isCheckin == "1")
            {
                strXML = strXML + "<tx_ischeckin>" + Tx_isCheckin + "</tx_ischeckin>";
            }

            if (!string.IsNullOrWhiteSpace(Tx_boletos))
            {
                strXML = strXML + "<tx_ticketNumber>" + Tx_boletos + "</tx_ticketNumber>";
            }

            if (!string.IsNullOrWhiteSpace(Tx_fechaSalida))
            {
                strXML = strXML + "<tx_departureDate>" + Tx_fechaSalida + "</tx_departureDate>";
            }

            if (!string.IsNullOrWhiteSpace(Tx_fechaRetorno))
            {
                strXML = strXML + "<tx_completionDate>" + Tx_fechaRetorno + "</tx_completionDate>";
            }

            strXML = strXML + "</transaction>";
            strXML = strXML + "<sdos>";
            strXML = strXML + "<sdos_billingname>" + nombreC + "</sdos_billingname>";
            strXML = strXML + "<sdos_billingStreet>" + direccion + "</sdos_billingStreet>";
            strXML = strXML + "<sdos_billingNeighborhood>" + colonia + "</sdos_billingNeighborhood>";
            strXML = strXML + "<sdos_billingMunicipality>" + delegacion + "</sdos_billingMunicipality>";
            strXML = strXML + "<sdos_billingCity>" + ciudad + "</sdos_billingCity>";
            strXML = strXML + "<sdos_billingState>" + Estado + "</sdos_billingState>";
            strXML = strXML + "<sdos_billingZipCode>" + cp + "</sdos_billingZipCode>";
            strXML = strXML + "<sdos_billingCountry>" + PaisC + "</sdos_billingCountry>";
            strXML = strXML + "<sdos_billingPhone>" + TelefonoC + "</sdos_billingPhone>";
            strXML = strXML + "<sdos_billingEmail>" + CorreoC + "</sdos_billingEmail>";

            strXML = strXML + "<sdos_billingStreetNumber>" + NumExt + "</sdos_billingStreetNumber>";

            if (!string.IsNullOrWhiteSpace(NumInt))
            {
                strXML = strXML + "<sdos_billingStreetNumberInt>" + NumInt + "</sdos_billingStreetNumberInt>";
            }

            strXML = strXML + "</sdos>";

            if (!string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.dbgGetPlazoMCI()))
            {
                strXML = strXML + "<mci><plazomci>" + Globales.cpIntegraEMV.dbgGetPlazoMCI() + "</plazomci></mci>";
            }

            strXML = strXML + "</PAYMENTSDOS>";

            strXML = Globales.EncryptTripleDES(strXML, EncryptC.generarSemilla("PCPAY" + Id_Company + merchant));

            WS.WS_Url = Program.ip + "/wscobroSdos/services/wscobroSdos"; //' ?wsdl'cpIntegracion_sURL_cpINTEGRA + "/wscobroSdos/services/wscobroSdos"
            WS.WS_Action = "http://wscobrosdos.mit.com";
            WS.ClearWS();
            WS.WS_Params = Id_Company + "," + merchant + "," + strXML;
            WS.WS_nbParams = "strIdCompany,strIdMerchant,strXml";
            WS.WS_Method = "cobroSdos";
            WS.MITWebServices(WS.WS_Params, WS.WS_nbParams, WS.WS_Method);
            WS.WS_Response = WS.WS_Response.Replace("&lt;", "<");
            //     WS.WS_Response = Globales.DecryptString(WS.WS_Response);
            if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("response", WS.WS_Response)))
            {
                Globales.cpIntegracion_sResult = WS.WS_Response;
            }
            string Voucher = string.Empty;
            string VoucherAct = string.Empty;
            var temporal = Globales.GetDataXml("voucher_comercio", Globales.cpIntegracion_sResult);
            if (!string.IsNullOrWhiteSpace(temporal))
            {
                string comercio = string.Empty;
                string cliente = string.Empty;
                comercio = Globales.CheckVoucher(Globales.GetDataXml("voucher_comercio", Globales.cpIntegracion_sResult));
                cliente = Globales.CheckVoucher(Globales.GetDataXml("voucher_cliente", Globales.cpIntegracion_sResult));
                Voucher = "<voucher_comercio>" + comercio + "</voucher_comercio>" + "<voucher_cliente>" + cliente + "</voucher_cliente>    @";
                if (!Globales.dbgGetPrintBC())
                {
                    Voucher = Voucher.Replace("@bc " + Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult), "");
                }
                else if (Globales.cpIntegraEMV.chkPp_Trademark().ToUpper() == "INGENICO" && Globales.cpIntegraEMV.chkPp_Printer() == "1")
                {
                    Voucher = Voucher.Replace(Utils.Mid(Voucher, Voucher.IndexOf("@bc ") + 1, 17), Utils.Mid(Voucher, Voucher.IndexOf("@bc ") + 1, 13) + "@br");
                }
                VoucherAct = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult);
                if (TypeUsuario.TipoImpresora == "6")
                {
                    Globales.cpIntegracion_sResult = Globales.cpIntegracion_sResult.Replace(VoucherAct, Voucher);
                }
                cpIntegracion_cpAVSs2 = true;
            }

            else
            {
                // Globales.cpIntegracion_sResult = "";
                cpIntegracion_cpAVSs2 = false;
            }

            return cpIntegracion_cpAVSs2;
        }
        public static bool LoginUser(string strUsr, string strPwd, out string answer)
        {
          //  generateLogs.LogsApp.setToolLogs("modMitLoginUser()");

            bool login = false;
            answer = string.Empty;

            try
            {
                Program.NombreWebForm = NOMBRE_GENERAL + " - LoginUser()";

                TypeUsuario.SaveLogTransaction = true;
                cpHTTP.cpHTTP_Clear();
                cpHTTP.cpHTTP_sURL_cpCUCT = URLLOGIN;

                Program.cpIntegraEMV.dbgSetUrl(Program.ip);
              //  generateLogs.LogsApp.writeLog("Establecee: dbgSetUrl");
                string serie = "";
                serie = Utils.ObtieneParametrosMIT("SERIE");
              //  generateLogs.LogsApp.writeLog("ObtieneParametrosMIT");
                TypeUsuario.SerialReader = serie;

                Program.VersionPcPay = "pcpay " + Program.VersionApp;
                Utils.GuardaParametrosMIT("VERSION", Program.VersionPcPay);
             //   generateLogs.LogsApp.writeLog("GuardaParametrosMIT");
                if (Program.cpIntegraEMV.dbgLoginUser(strUsr, strPwd))
                {
                //    generateLogs.LogsApp.writeLog("Se ha logeado....");
                    TypeUsuario.CadenaXML = Program.cpIntegraEMV.dbgGetXMLUser();
                    TypeUsuario.bolCambiaPwd = Utils.GetDataXML(TypeUsuario.CadenaXML, "cambiaPwd").Equals("false");

                    //*****************************************************************************
                    //SE OBTIENE LA LLAVE RSA PARA LA NUEVA FORMA DE ENCRIPTAR
                    Program.cpIntegraEMV.dbgSetUrlIpKeyWeb(Program.ipKeyWeb);
                    Program.cpIntegraEMV.ObtieneLlavePublicaRSA();

                    login = true;

                    if (Utils.GetDataXML(Program.cpIntegraEMV.dbgGetXMLUser(), "response").Equals("false"))
                    {
                        TypeUsuario.CadenaXML = Program.cpIntegraEMV.dbgGetXMLUser();
                        login = false;
                    }
                }
                else
                {
                    TypeUsuario.CadenaXML = Program.cpIntegraEMV.dbgGetXMLUser();
                    login = false;
                }
                TypeUsuario.SaveLogTransaction = false;
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                //generateLogs.LogsApp.writeLog(ex.StackTrace);
                //generateLogs.LogsApp.writeLog(ex.Source);
                //generateLogs.LogsApp.writeLog(ex.Message);

                answer = Program.NombreWebForm + "\r\nError: " + ex.Message + " " + Program.NOMBRE_APP;
            }

            return login;
        }
        public static bool MatarProceso(string StrNombreProceso)
        {
            bool killproceso;

            try
            {
                Program.NombreWebForm = NOMBRE_GENERAL + " - MatarProceso()";

                foreach (Process proceso in Process.GetProcesses())
                {
                    if (proceso.ProcessName.Equals(StrNombreProceso))
                        proceso.Kill();
                }

                killproceso = true;

            }
            catch (Exception ex)
            {
                killproceso = false;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                MessageBox.Show(Program.NombreWebForm + "\r\nError: " + ex.Message, Program.NOMBRE_APP);
            }

            return killproceso;


        }
        public static int banImp { get; set; }
        public static int banNoVchr { get; set; }
        public static string KEYCARD = "KEY CREDIT CARD KEY";
        internal static void BeginAplicacion()
        {
            #region inicia la aplicacion
            try
            {
                strNombreFP = NOMBRE_GENERAL + ".Main()";
                string chkShwMsg = string.Empty;

                TypeUsuario.UserApp = Globales.DecryptString(Globales.GetSettingString("AUTHOR"), Globales.KEYCARD, true);//Falta la parte de que desencripta el autor
                TypeUsuario.PwdApp = Globales.DecryptString(Globales.GetSettingString("AUTHORID"), Globales.KEYCARD, true);

                chkShwMsg = Globales.GetSettingString("ShwMsg");
                TypeUsuario.IsADO = Globales.GetSettingString("ISADO");
                if (chkShwMsg == "")
                {
                    Globales.SaveSettingString("ShwMsg", "1");
                    TypeUsuario.ShowMsg = true;
                }
                else
                {
                    TypeUsuario.ShowMsg = (chkShwMsg == "0") ? false : true;
                }

                ///Verifica que el instalador no exista en el programa.
                ///if path -> archivoInstalador -> existe entonces se borra


                ///Codigo relacionado con la impresora, se debe escribir código de impresora.
                Globales.clsImpr.ASPPrinter = new ASPPrinterCOM.ASPPrinter();
                Globales.clsImpr.ASPPrinter.Key = "516750B8AFEE9BAA7211DADFACD4170E3437DBAF52A875AAEA12C88773D075BBCF46B9864767025404C";
                Globales.clsImpr.RDP = new RawDataPrinter.Printer();
                Globales.clsImpr.RDP.Key = "63FD42DCCB6175998CAEE8BA16AE17BA9B88638816FD24815379CB158E8D61DFEC99B8FD57C2DE110297050";

                //Codigo paara el dllFirmaPanel = new MITEncrypt.cpIntegracionFirmaPanel

                if (Globales.GetSettingString("CAPACIDAD_TOUCH") == "")
                {
                    if (Globales.cpIntegraEMV.EsTouch())
                    {
                        Globales.SaveSettingString("CAPACIDAD_TOUCH", "1");
                        Globales.SaveSettingString("CAPACIDAD_TOUCH_MAIL", "1");
                        TypeUsuario.EnviarVoucherMail = true;
                        TypeUsuario.UtilizarCapacidadTouch = true;

                    }
                    else
                    {
                        Globales.SaveSettingString("CAPACIDAD_TOUCH", "0");
                        Globales.SaveSettingString("CAPACIDAD_TOUCH_MAIL", "0");
                        TypeUsuario.EnviarVoucherMail = false;
                        TypeUsuario.UtilizarCapacidadTouch = false;
                    }
                }

                if (Globales.STD_getLicence() == "")
                {
                    Globales.STD_GeneratePrint();
                    //STD_Class_Initialize --> falta poner esta clase!!.. para ver se debe visualziar el código de VS.
                    if (Globales.GetSettingString("Instalador") == "1" && !string.IsNullOrWhiteSpace(Globales.GetSettingString("AUTHOR")) && !string.IsNullOrWhiteSpace(Globales.GetSettingString("AUTHOR_ID")))
                    {

                        nextFlujo();
                    }
                    else
                    {
                        frmLicencia licencia = new frmLicencia();
                        licencia.Show();
                    }
                }
                else
                {

                    nextFlujo();
                }

            }
            catch (Exception e)
            {

                mensajeError = strNombreFP + "Error: " + e.Message;

            }
            #endregion
        }
        public static Window formulario { get; set; }
        public static void nextFlujo()
        {
            
            
            if (isCDP)
            {
                //Visualizar frmLicencia
                formulario = new frmLicencia();
            }

            //STD_Class_Initalize -->//Recupera la huella del sistema
            if (TypeUsuario.IsADO == "1")
            {
                if (TypeUsuario.UserApp == "")
                {
                    formulario = new frmLogin();

                }
                else
                {
                    frmLogin logeo = new frmLogin();

                    logeo.txtPwd.Password = TypeUsuario.PwdApp;
                    logeo.txtUser.Text = TypeUsuario.UserApp;
                    logeo.frmLogin1_Loaded(null,null);
                    logeo.cmdAceptar_Click(null,null);
                    if(!Globales.mostrar){
                        return;
                    }
                    logeo.txtPwd.Password = "";
                    logeo.txtUser.Text = "";
                    logeo.TMENSAJE.Text = "Usuario o Contraseña invalido.";
                    logeo.Show();
                    return;
                }
            }
            else
            {
                if (Globales.GetSettingString("Instalador") != "1")
                {
                    isCDP = true;
                }
                formulario = new frmLogin();
            }

            formulario.Show();
            

        }
        public static string EstableceLector(bool lector = false)
        {
            string aux = "";
            Globales.cpIntegraEMV.dbgEndOperation();
            string resultado = "";
            if (!Program.cpIntegraEMV.dbgSetReader())
            {
                TypeUsuario.strTipoLector = "-1";
                resultado = "No hay ningun lector conectado.";
            }
            else
            {
                TypeUsuario.SerialReader = Program.cpIntegraEMV.chkPp_Serial();
                Globales.SaveSettingString("SERIE", TypeUsuario.SerialReader);
                string COM = Globales.cpIntegraEMV.chkPp_Com();
                Globales.SaveSettingString("COM",COM);
                if (lector)
                {
                    resultado = "Lector:\nEMvFull: " + Program.cpIntegraEMV.chkPp_EMVFull() + "\nModelo: " + Program.cpIntegraEMV.chkPp_Model() + "\nSerie: " + Program.cpIntegraEMV.chkPp_Serial() + "\nMarca: " + Program.cpIntegraEMV.chkPp_Trademark() + "\nVersión: " + Program.cpIntegraEMV.chkPp_Version() + "\nCom: " + COM.Replace("COM","");
                }
                else {
                    resultado = "Lector:\n" + Program.cpIntegraEMV.chkPp_EMVFull() + "\nModelo: "  + Program.cpIntegraEMV.chkPp_Model() + "\nSerie: " + Program.cpIntegraEMV.chkPp_Serial() + "\nMarca: " + Program.cpIntegraEMV.chkPp_Trademark() + "\nVersión: " + Program.cpIntegraEMV.chkPp_Version() + "\nCom: " + COM;
                }
                TypeUsuario.IsAQ = false;
                if (Program.cpIntegraEMV.chkPp_Printer() == "1")
                {
                    TypeUsuario.strTipoLector = "3";
                    TypeUsuario.TipoImpresora = "6";
                }
                else if (Program.cpIntegraEMV.chkPp_Printer() == "0")
                {
                    TypeUsuario.strTipoLector = "2";

                }
                else
                {
                    TypeUsuario.strTipoLector = "0";
                }
                EMVLector = "1";
                Globales.SaveSettingString("COM", COM);
                Globales.SaveSettingString("EMV", EMVLector);
                Globales.SaveSettingString("TYPE", TypeUsuario.strTipoLector);
            }



            return resultado;
        }
        public static string strNombreFP { get; set; }
        public static string mensajeError { get; set; }
        public static bool isCDP { get; set; }
        public static string EMVLector { get; set; }
        internal static void setConfiguracion(string xml)
        {
            string aq = string.Empty;
            try
            {

                TypeUsuario.Id_Company = Globales.GetDataXml("id_company", xml);
                TypeUsuario.nb_company = Globales.GetDataXml("nb_company", xml);
                TypeUsuario.nb_companystreet = Globales.GetDataXml("nb_companystreet", xml);
                TypeUsuario.Id_Branch = Globales.GetDataXml("id_branch", xml);
                TypeUsuario.nb_branch = Globales.GetDataXml("nb_branch", xml);
                TypeUsuario.reference = Globales.GetDataXml("reference", xml);
                TypeUsuario.country = Globales.GetDataXml("country", xml);

                Globales.cpIntegraEMV.dbgSetTrxData(TypeUsuario.usu, TypeUsuario.Pass, "", TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country);

                aq = Globales.GetSettingString("IsAQ");
                if (aq != "")
                {
                    TypeUsuario.IsAQ = true;
                }
                TypeUsuario.enviaCorreo = Globales.GetDataXml("correo", TypeUsuario.CadenaXML).Equals("0") ? false : true;

                TypeUsuario.strTipoLector = Globales.GetSettingString("TYPE");
                TypeUsuario.strSoloUnaHoja = Globales.GetSettingString("HOJA");
                string cport, serie;
                cport = Globales.GetSettingString("COM");
                serie = Globales.GetSettingString("SERIE");

                Globales.EMVLector = Globales.GetSettingString("EMV");
                Globales.BandImpHTML = Globales.GetSettingString("AUTOMATICO");
                Globales.BandImpManual = Globales.GetSettingString("VOUCHERPORHOJA");
            }
            catch
            {

            }
        }
        public static string BandImpHTML { get; set; }
        public static string BandImpManual { get; set; }
        public static string sPathUserLog { get; set; }
        internal static bool Imprimir_HTML(string html_code, bool isWallets = false)
        {
            bool answer = false;
            html_code.Replace("<head>", "<head><HTA:APPLICATION ID=\"oMyApp\" ICON=\"mit.ico\">");
            html_code.Replace("<body>", "<body onload=\"self.print();\">");
            if (html_code != string.Empty)
            {
                if (Globales.GetDataXml("response", html_code) == "false")
                {

                }
                else
                {
                    frmReporte reporte = new frmReporte();
                    if (TypeUsuario.TipoImpresora == "4" && !html_code.Contains("promo.bmp"))
                    {
                        html_code = Globales.VoucherHtml1(html_code);
                    }
                    reporte.imprimirHtml(html_code);
                    reporte.Show();
                    answer = true;
                    //formularios.Reportes.FReporte fReporte = new formularios.Reportes.FReporte();
                    //fReporte.setReporteWeb(html_code);
                    //fReporte.ShowDialog();
                    //answer = true;
                }
            }
            else
            {
                if (isWallets)
                    System.Windows.Forms.MessageBox.Show("No se tiene información para imprimir el documento \n ó ya se realizó la impresión.",
                        "Aviso.",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Exclamation);

                else
                    Globales.MessageBoxMitError("No se tiene información para imprimir el documento.");
            }
            return answer;
        }
        internal static void PrintOptions(string VxVoucher, string NumOpe, System.Windows.Forms.PrintDialog Impresora, bool SoloUnaHoja = false, bool isFAE = false)
        {
            #region printoptions
            try
            {
                string strCadEncriptar = string.Empty;
                string AuxUnaHoja = string.Empty;
                string GImp = string.Empty;
                string cport = string.Empty;


                // 'SE SALE DE LA FUNCION CUANDO EL VALOR DE dbgGetEsImprimibleVoucher ES CERO
                if (Program.cpIntegraEMV.dbgGetEsImprimibleVoucher() == "0")
                {
                    // Exit Sub
                    return;
                }


                if (!VxVoucher.Contains("@"))
                { // 'El voucher esta encriptado

                    if (Program.cpIntegraEMV.getRspVoucher().Contains("<voucher_comercio>"))
                    { //Then 'Es nuevo voucher
                        VxVoucher = Program.cpIntegraEMV.getRspVoucher();
                    }
                    else
                    {
                        VxVoucher = Globales.DecryptString(VxVoucher, "KEY CREDIT CARD KEY", true);
                    }
                }

                TypeUsuario.strVoucher = VxVoucher;
                TypeUsuario.strVoucherCoP = VxVoucher;


                if (Program.cpIntegraEMV.getRspVoucher() == "")
                {
                    Globales.VerificaVoucher();
                }
                VxVoucher = TypeUsuario.strVoucherCoP;

                if (TypeUsuario.IsAQ)
                {
                    //Exit Sub
                    return;
                }

                if (SoloUnaHoja && TypeUsuario.strSoloUnaHoja == "0")
                {
                    AuxUnaHoja = TypeUsuario.strSoloUnaHoja;
                    TypeUsuario.strSoloUnaHoja = "1";
                    GImp = "1";
                }
                TypeUsuario.strTipoLector = "3";
                if ((TypeUsuario.strTipoLector == "3" || TypeUsuario.strTipoLector == "4") && TypeUsuario.TipoImpresora == "6")
                {
                    if (TypeUsuario.Publicidad == "1")
                    {
                        string pub = string.Empty;

                        pub = getPublicidad(TypeUsuario.Estado, TypeUsuario.Mcc, Globales.GetDataXml("foliocpagos", cpIntegracion.cpIntegracion_sResult));
                        if (!string.IsNullOrEmpty(pub))
                        {
                            Program.cpIntegraEMV.dbgPrintVoucher(VxVoucher);
                            Program.cpIntegraEMV.dbgPrint(pub);
                        }
                        else
                        {
                            Program.cpIntegraEMV.dbgPrintVoucher(VxVoucher);
                        }
                    }
                    else
                    {

                        if (Program.cpIntegraEMV.chkPp_Trademark() == "VERIFONE")
                        {
                            Thread.Sleep(1000);
                        }
                        Program.cpIntegraEMV.dbgPrintVoucher(VxVoucher);
                    }

                    if (TypeUsuario.strTipoLector == "3")
                    {
                        // CpCobro.dbgSetReader( "3", cport);
                    }
                }
                else
                {
                    string evalua = TypeUsuario.TipoImpresora.Substring(0, 1);


                    // Select Case Mid(TypeUsuario.TipoImpresora, 1, 1)

                    switch (evalua)
                    {
                        case "1":
                            {
                                if (isFAE)
                                {
                                    VxVoucher = VxVoucher.Replace("@cnn", "");
                                    VxVoucher = VxVoucher.Replace("@cnb", "");
                                    VxVoucher = VxVoucher.Replace("@cbb", "");
                                    VxVoucher = VxVoucher.Replace("@lnn", "");
                                    VxVoucher = VxVoucher.Replace("@br", "");
                                    // MsgBoxEx VxVoucher, , , , vbOKOnly, "Factura Electrónica"
                                    System.Windows.Forms.MessageBox.Show(VxVoucher, "Factura Electronica", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                                }


                                else if (isSmart)
                                {
                                    VxVoucher = VxVoucher.Replace("@cnn", "");
                                    VxVoucher = VxVoucher.Replace("@cnb", "");
                                    VxVoucher = VxVoucher.Replace("@cbb", "");
                                    VxVoucher = VxVoucher.Replace("@lnn", "");
                                    VxVoucher = VxVoucher.Replace("@br", "");
                                    System.Windows.Forms.MessageBox.Show(VxVoucher, "Centro de Pago", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                                }
                                else
                                {
                                    Globales.cpHTTP_Clear();
                                    Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
                                    strCadEncriptar = "&folio=" + NumOpe +
                                                      "&empresa=" + TypeUsuario.Id_Company +
                                                      "&sucursal=" + TypeUsuario.Id_Branch +
                                                      "&op=impvouch" +
                                                      "&co=false";
                                    Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, KEY_RC4, true);


                                    if (Globales.cpHTTP_SendcpCUCT())
                                    {
                                        TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                                        modMIT.Imprimir_HTML(Globales.cpHTTP_sResult);
                                    }
                                }

                            }
                            break;


                        case "2":
                            {
                                if (TypeUsuario.strVoucherCoP.Contains("voucher_comercio"))
                                {
                                    TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher_comercio", VxVoucher);
                                    TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("-C-O-M-E-R-C-I-O-", "");
                                    TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("@cnb logo_cpagos", "");
                                    TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("@cnn ver_app", "");
                                }

                                if (IniciaImpresoraForma(Impresora))
                                {
                                    Imprimir_Voucher_Termico(Impresora, GImp);
                                    cierraImpresora(Impresora);
                                }
                                else
                                {
                                    //'MsgBox "No se pudo inicializar la impresora térmica.", vbExclamation, NOMBRE_APP
                                    //MsgBoxEx "No se pudo inicializar la impresora térmica.", , , , vbExclamation, NOMBRE_APP
                                    System.Windows.Forms.MessageBox.Show("No se pudo inicializar la impresora térmica.", NOMBRE_APP, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                                }
                            }
                            break;
                        case "3":
                            {
                                if (TypeUsuario.strVoucherCoP.Contains("voucher_comercio"))
                                {
                                    TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher_comercio", VxVoucher);
                                    TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("-C-O-M-E-R-C-I-O-", "");
                                    TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("@cnb logo_cpagos", "");
                                    TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("@cnn ver_app", "");
                                }
                                Globales.imprimirEpson();
                            }
                            break;

                        case "4":
                            {
                                if (isFAE)
                                {
                                    VxVoucher = VxVoucher.Replace("@cnn", "");
                                    VxVoucher = VxVoucher.Replace("@cnb", "");
                                    VxVoucher = VxVoucher.Replace("@cbb", "");
                                    VxVoucher = VxVoucher.Replace("@lnn", "");
                                    VxVoucher = VxVoucher.Replace("@br", "");


                                    System.Windows.Forms.MessageBox.Show(VxVoucher, "Factura Electronica", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                                }
                                else
                                {
                                    cpHTTP.cpHTTP_Clear();
                                    cpHTTP.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
                                    strCadEncriptar = "&folio=" + NumOpe +
                                                      "&empresa=" + TypeUsuario.Id_Company +
                                                      "&sucursal=" + TypeUsuario.Id_Branch +
                                                      "&op=impvouch" +
                                                      "&co=false";
                                    cpHTTP.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, KEY_RC4, true);

                                    if (cpHTTP.cpHTTP_SendcpCUCT())
                                    {
                                        //'LogError (cpHTTP_sResult)
                                        //'auxCod = Mid(cpHTTP_sResult, 1, InStr(cpHTTP_sResult, "position: absolute; width: 216; height: 32; left: 425;") - 12)
                                        //'auxCod = auxCod & "<!---->" & Mid(cpHTTP_sResult, InStr(cpHTTP_sResult, "table") - 1, Len(cpHTTP_sResult))
                                        //'auxCod = Mid(auxCod, 1, InStr(auxCod, "RESPECTO.") + 8)
                                        //'auxCod = auxCod & "</td></tr></table><!----></td><td width='80'></td><td><img border='0' src='file:c:\pcpay\promo.bmp'></td></body></html>"
                                        cpHTTP.cpHTTP_sResult = cpHTTP.cpHTTP_sResult.Replace("\r", "");
                                        cpHTTP.cpHTTP_sResult = cpHTTP.cpHTTP_sResult.Replace("\n", "");
                                        //   TypeUsuario.strVoucher = voucher1html(cpHTTP.cpHTTP_sResult);
                                        TypeUsuario.strVoucher = "<"+Globales.VoucherHtml1(Globales.cpHTTP_sResult);

                                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                                        //Init_ASPHTML();
                                    }
                                }
                            }
                            break;

                        case "5":
                            {
                                if (TypeUsuario.strVoucherCoP.Contains("voucher_comercio"))
                                {
                                    TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher_comercio", VxVoucher);
                                    TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("-C-O-M-E-R-C-I-O-", "");
                                    TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("@cnb logo_cpagos", "");
                                    TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("@cnn ver_app", "");

                                }
                                Init_Manual();
                            }
                            break;

                    }
                }

                if (!String.IsNullOrEmpty(AuxUnaHoja))
                {
                    TypeUsuario.strSoloUnaHoja = AuxUnaHoja;
                }
                //CpCobro.dbgEndOperation();

            }

            catch (Exception Err)
            {


            }

            #endregion
        }
        private static void Init_Manual()
        {
            throw new NotImplementedException();
        }
        private static string voucher1html(string p)
        {
            return p.Substring(0, p.IndexOf("<td width=\"80\" \">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") - 63) + "</td></tr></table><!----></td><td width=\"80\"></td><td><img border=\"0\" src=\"" + "file:" + "c:\\pcpay\\" + "promo.bmp\"></td></body></html>";
        }
    
        private static void cierraImpresora(System.Windows.Forms.PrintDialog Impresora)
        {
            throw new NotImplementedException();
        }
        private static void Imprimir_Voucher_Termico(System.Windows.Forms.PrintDialog Impresora, string GImp)
        {
            throw new NotImplementedException();
        }
        private static bool IniciaImpresoraForma(System.Windows.Forms.PrintDialog Impresora)
        {
            throw new NotImplementedException();
        }
        public static bool isSmart { get; set; }
        public static string msjRech { get; set; }
        public static bool isVentasPropias { get; set; }
        public const string EMPREF = "0329";
        public const string EMPREF2 = "001";
        public const int MAXCAR = 40;
        public static bool bolActivaModoP { get; set; }//Controla que la venta de desliza no aparezca nuevamente(Propias)
        public static List<string> listaCboBancos = new List<string>();
        internal static System.Windows.Controls.ComboBox obtieneBancos(System.Windows.Controls.ComboBox cboBanco, string bancos)
        {
            string strAux = string.Empty;
            string numEmpr = string.Empty;
            if (!string.IsNullOrWhiteSpace(bancos) && bancos != "null")
            {

                string[] split1 = bancos.Split('|');
                foreach (var item in split1)
                {
                    cboBanco.Items.Add(item);
                }
            }
            return cboBanco;
        }
        internal static void ReseteaVariablesRecompensas()
        {
            sinSaldoRecomDirecto = false;
            errorRecom = "";
            hayTrxRecompensa = false;
            saldoRecomDirecto = "0.00";
            saldoExito = false;
            diferenciaRecomDirecto = "0.00";
            strCadenaVoucherVtaDirecta = "";
            strCadenaVoucherVtaRecompensas = "";
            hayDoubleVoucherRecom = false;
        }
        public static bool isQualitas { get; set; }
        public static bool isCoberturaMultiple { get; set; }
        public static bool sinSaldoRecomDirecto { get; set; }
        public static string errorRecom { get; set; }
        public static bool hayTrxRecompensa { get; set; }
        public static string saldoRecomDirecto { get; set; }
        public static bool saldoExito { get; set; }
        public static string diferenciaRecomDirecto { get; set; }
        public static string strCadenaVoucherVtaDirecta { get; set; }
        public static string strCadenaVoucherVtaRecompensas { get; set; }
        public static bool hayDoubleVoucherRecom { get; set; }
        public const string RGEV = "0355";
        internal static bool RefGEV(string strDatos, string codFPago, string Ref)
        {
            string strCod = string.Empty;
            string strRef = string.Empty;
            bool RefGEVariable = false;
            string[] split = strDatos.Split('|');
            foreach (string item in split)
            {
                string[] split2 = item.Split(',');
                if (split2.Length < 2) continue;
                strCod = split2[1];
                strRef = split2[0];
                if (strRef == codFPago)
                {
                    LCodGEV = strCod.Length;
                    CodGEV = strCod;
                    FPGEV = strRef;
                    RefGEVariable = true;
                    break;
                }
            }
            return RefGEVariable;
        }
        public static int LCodGEV { get; set; }
        public static string CodGEV { get; set; }
        public static string FPGEV { get; set; }
        public static string merchantBanda { get; set; }
        public static bool IsAmex { get; set; }
        public static string csvAmexenBanda { get; set; }
        internal static void printVoucherRecompensasDirecto(string Voucher)
        {
            TypeUsuario.TipoImpresora = (string.IsNullOrWhiteSpace(TypeUsuario.TipoImpresora)) ? "" : TypeUsuario.TipoImpresora;
            var aux = TypeUsuario.TipoImpresora;
            switch (TypeUsuario.TipoImpresora.Substring(0, 1))
            {
                case "1":
                    Globales.cpHTTP_cadena1 = Globales.cpIntegraEMV.GetVoucherRecompensas(Globales.cpIntegraEMV.dbgGetId_Company(), Globales.cpIntegraEMV.dbgGetId_Branch(), Globales.cpIntegraEMV.dbgGetCountry(),
                          Globales.cpIntegraEMV.dbgGetUser(), Globales.cpIntegraEMV.dbgGetPass(), "impticket", Globales.cpIntegraEMV.getRspOperationNumber());
                    TypeUsuario.strVoucher = Globales.cpHTTP_cadena1;
                    modMIT.Imprimir_HTML(TypeUsuario.strVoucher);
                    break;
                case "4":
                    Globales.cpHTTP_cadena1 = Globales.cpIntegraEMV.GetVoucherRecompensas(Globales.cpIntegraEMV.dbgGetId_Company(), Globales.cpIntegraEMV.dbgGetId_Branch(), Globales.cpIntegraEMV.dbgGetCountry(),
                        Globales.cpIntegraEMV.dbgGetUser(), Globales.cpIntegraEMV.dbgGetPass(), "impticket", Globales.cpIntegraEMV.getRspOperationNumber());
                    TypeUsuario.strVoucher = Globales.cpHTTP_cadena1;
                    modMIT.Imprimir_HTML(Globales.cpHTTP_cadena1);

                    break;
                default:
                    //Voucher.............printoptions
                    Globales.PrintOptions(Voucher);
                    break;

            }
            Globales.strCadenaVoucherVtaRecomensa = Globales.cpHTTP_cadena1;
        }
        public static string XMLFacturaE { get; set; }
        public static bool isQualitasCierraForm { get; set; }
        public static string merchantMoto { get; set; }
        public static string plazoMCI { get; set; }
        public static bool isC85 { get; set; }
        public const string ipFirmaPanel = "https://" + "dev3.mitec.com.mx"; // Firma en panel, publica desarrollo
        public static bool IsOM { get; set; }

        public const string COD_VF = "99";

        public const string CRYPTO = "2";
        internal static void CFEImporte(string referencia)
        {
            string AuxImporte = "";
            int i = 0;
            CFE.Importe = "";
            CFE.ValorFijoCFE = referencia.Substring(0, 2);
            CFE.RPU = referencia.Substring(2, 12);
            CFE.FechaVenc = referencia.Substring(14, 6);
            AuxImporte = referencia.Substring(20, 9);
            for (int x = 0; x < 9; x++)
            {
                if (AuxImporte.Substring(0, 1) == "0")
                {
                    AuxImporte = AuxImporte.Substring(1);
                }
                else
                {
                    CFE.Importe = AuxImporte;
                    break;
                }
            }
            CFE.DV = referencia.Substring(29, 1);
        }
        internal static string LimpiarCampo(string p)
        {
            p = p.Replace("-", "");
            p = p.Replace(",", "");
            p = p.Replace("$", "");
            p = p.Replace("'", "");
            p = p.Replace("!", "");
            p = p.Replace("/", "");
            p = p.Replace("#", "");
            p = p.Replace("" + Convert.ToChar(34), "");
            return p;
        }
        public static string ip { get; set; }
        public static string ipvip { get; set; }
        public static string drpPROD { get; set; }
        public static string drpVIP { get; set; }
        public static string getRuta()
        {
            string ruta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);//Se saca la ruta de la carpeta de usuario.
            return ruta;
        }
        internal static void GeneraFacturaElectronica(string Importe, string operacion, string fecha, string Propina)
        {
            XMLFacturaE = "<importe>" + Importe.Replace("$$", "$") + "</importe>";
            XMLFacturaE += "<operacion>" + operacion + "</operacion>";
            if (string.IsNullOrWhiteSpace(fecha))
            {
                string fechaUx = DateTime.Now.ToString("u");
                fechaUx = fechaUx.Replace("Z", "");
                XMLFacturaE += "<fecha>" + fechaUx + ".000" + "</fecha>";
            }
            else
            {
                XMLFacturaE += "<fecha>" + fecha.Trim() + ".000" + "</fecha>";
            }
            XMLFacturaE += "<propina>" + Propina + "</propina>";
            //frmFacturaDatos.show();
        }
        internal static void FacturaElectronica(string Importe, string Operacion, string Fecha, string propina)
        {
            XMLFacturaE = "<importe>" + Importe.Replace("$$", "$") + "</importe>";
            XMLFacturaE += "<operacion>" + Operacion + "</operacion>";
            XMLFacturaE += "<fecha>" + Fecha + "</fecha>";
            XMLFacturaE += "<propina>" + propina + "</propina>";
            //frmFacturaE.show();
        }
        public static string InfoCheckOut { get; set; }
        public static bool CloseFr { get; set; }
        internal static void PrintOptionsTAE(string VxVoucher, string NumOpe, string opcionImpresion, System.Windows.Controls.PrintDialog printDialog, bool soloUnaHoja = false)
        {
            string strCadEncriptar = string.Empty;
            string auxUnaHoja = string.Empty;
            string GImp = string.Empty;
            string cport = string.Empty;
            auxUnaHoja = "";
            TypeUsuario.strVoucher = VxVoucher;
            TypeUsuario.strVoucherCoP = VxVoucher;
            if (TypeUsuario.IsAQ) return;
            cport = Globales.GetSettingString("COM");

            if (TypeUsuario.strTipoLector == "3")
            {
                //Globales.CpCobro.dbgSetReader("3", cport);

            }
            if (soloUnaHoja == true && TypeUsuario.strSoloUnaHoja == "0")
            {
                auxUnaHoja = TypeUsuario.strSoloUnaHoja;
                TypeUsuario.strSoloUnaHoja = "1";
                GImp = "1";
            }

            if (TypeUsuario.strTipoLector == "3" && TypeUsuario.TipoImpresora == "6")
            {
                //Globales.CpCobro.dbgEndOperation();
                if (!(Globales.cpIntegraEMV.dbgSetReader()))
                {
                    Globales.MessageBoxMit("Existe un porblema con la impresón.");
                }
                else
                {
                    if (TypeUsuario.Publicidad == "1")
                    {
                        string pub = string.Empty;
                        pub = Globales.getPublicidad(TypeUsuario.Estado, TypeUsuario.Mcc, Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult));
                        if (!string.IsNullOrWhiteSpace(pub))
                        {
                            Globales.cpIntegraEMV.dbgPrintVoucher(VxVoucher);
                            Globales.cpIntegraEMV.dbgPrint(pub);
                        }
                        else
                        {
                            Globales.cpIntegraEMV.dbgPrintVoucher(VxVoucher);
                        }
                    }
                    else
                    {
                        if (!soloUnaHoja)
                        {
                            Globales.cpIntegraEMV.dbgPrintVoucher(VxVoucher);
                        }
                        else
                        {
                            Globales.cpIntegraEMV.dbgPrintVoucher(VxVoucher + "@br @br @br @br @br @br");
                        }
                    }
                    if (TypeUsuario.strTipoLector == "3")
                    {
                        //Globales.CpCobro.dbgSetReader("3", cport);
                    }
                }
            }
            else
            {

            }
            if (auxUnaHoja != "")
            {
                TypeUsuario.strSoloUnaHoja = auxUnaHoja;
            }
            //Globales.CpCobro.dbgEndOperation();
        }
        internal static string getPublicidad(string nbCodigoEstado, string nbCodigogiro, string cdOperacion)
        {
            string strSoap = string.Empty;
            string valor = "";
            string strSOAPAction = string.Empty;
            string strWsdl = string.Empty;
            string llave = string.Empty;
            llave = "'?>#S>E=ZAkLD:3X/rMM";
            strSoap = "<com.mit.promopay.beans.BeanEntrada>";
            strSoap += "<nbCodigoestado>" + nbCodigoEstado + "</nbCodigoestado>";
            strSoap += "<nbCodigogiro>" + nbCodigogiro + "</nbCodigogiro>";
            strSoap += "<nbOperacion>" + cdOperacion + "</nbOperacion>";
            strSoap += "</com.mit.promopay.beans.BeanEntrada>";

            strSoap = Globales.EncryptTripleDES(strSoap, llave);

            strSoap = "<soap:Envelope xmlns:soap=" + Convert.ToChar(34) + "http://schemas.xmlsoap.org/soap/envelope/" + Convert.ToChar(34) + "><soap:Body><ns1:extraePublicidadXML xmlns:ns1=" + Convert.ToChar(34) + "http://services.promopay.mit.com/" + Convert.ToChar(34) + "><strEntrada>" + strSoap + "</strEntrada></ns1:extraePublicidadXML></soap:Body></soap:Envelope>";


            strSOAPAction = "extraePublicidadXML/extraePublicidadXML";
            strWsdl = Globales.URL_PUBLICIDAD + "/promopay/webservices/PublicidadActivaService?wsdl";
            string respuesta = InvokeWebService(strSoap, strSOAPAction, strWsdl);
            if (respuesta.Contains("200"))
            {
                string response = "";
                response = Globales.DecryptTripleDES(Globales.GetDataXml("return", respuesta), llave);
                if (Globales.GetDataXml("existeError", response) == "false")
                {
                    valor = Globales.GetDataXml("txPublicidad", response);
                }
            }
            return valor;
        }
        internal static string InvokeWebService(string strSoap, string strSOAPAction, string strWsdl)
        {
            bool blnSuccess = false;

            try
            {
                WebClient cliente = new WebClient();
                byte[] datoEnviar = Encoding.UTF8.GetBytes(strSoap);
                cliente.Headers.Add(HttpRequestHeader.ContentType, "text/xml;charset=utf-8");
                cliente.Headers.Add("SOAPAction", strSOAPAction);
                string respuesta = cliente.UploadString(strWsdl, strSoap);

                return respuesta;
            }
            catch
            {
                return "";
            }


        }
        internal static void PrintOptions2(string VxVoucher, string NumOpe, bool soloUnaHoja)
        {
            string strCadEncriptar = "";
            string auxUnaHoja = "";
            string GImp = "";
            auxUnaHoja = "";
            string cport = "";
            TypeUsuario.strVoucher = VxVoucher;
            TypeUsuario.strVoucherCoP = VxVoucher;
            //Verifica voucher

            if (TypeUsuario.IsAQ)
            {
                return;
            }
            cport = Globales.GetSettingString("COM");
            if (TypeUsuario.strTipoLector == "3")
            {
                //Globales.CpCobro.dbgSetReader("3", cport);
            }
            if (soloUnaHoja && TypeUsuario.strSoloUnaHoja == "0")
            {
                auxUnaHoja = TypeUsuario.strSoloUnaHoja;
                TypeUsuario.strSoloUnaHoja = "1";
                GImp = "1";
            }
            if (TypeUsuario.strTipoLector == "3" && TypeUsuario.TipoImpresora == "6")
            {
                //Globales.CpCobro.dbgEndOperation();
                if (!Globales.cpIntegraEMV.dbgSetReader())
                {
                    Globales.MessageBoxMit("Existe un problema con la impresión");
                }
                else
                {
                    if (TypeUsuario.Publicidad == "1")
                    {
                        string pub;
                        pub = Globales.getPublicidad(TypeUsuario.Estado, TypeUsuario.Mcc, Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult));
                        if (!string.IsNullOrWhiteSpace(pub))
                        {
                            Globales.cpIntegraEMV.dbgPrintVoucher(VxVoucher);
                            Globales.cpIntegraEMV.dbgPrint(pub);
                        }
                        else
                        {
                            Globales.cpIntegraEMV.dbgPrintVoucher(VxVoucher);
                        }
                    }
                    else
                    {
                        if (soloUnaHoja)
                        {
                            Globales.cpIntegraEMV.dbgPrintVoucher(VxVoucher);
                        }
                        else
                        {
                            Globales.cpIntegraEMV.dbgPrint(VxVoucher + "@br @br @br @br @br @br ");
                        }
                    }

                    if (TypeUsuario.strTipoLector == "3")
                    {
                        //Globales.CpCobro.dbgSetReader("3", cport);
                    }
                }
            }
            else
            {
                //Aqui va el switch de tipos de impresoras

            }
            if (auxUnaHoja != "")
            {
                TypeUsuario.strSoloUnaHoja = auxUnaHoja;
            }
            //Globales.CpCobro.dbgEndOperation();
        }
        internal static void frmImprimeReporteDiario()
        {
            string strAux = "";
            strAux = TypeUsuario.strVoucherCoP;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            string strCadEncriptar = "&usuario=" + TypeUsuario.usu + "&op=minireporte";
            Globales.cpHTTP_Clear();
            Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
            Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
            if (Globales.cpHTTP_SendcpCUCT())
            {
                TypeUsuario.strVoucherCoP = Globales.cpHTTP_sResult;
                if (!TypeUsuario.strVoucherCoP.Contains("@"))
                {
                    Globales.MessageBoxMit("No existe transacciones para este usuario.");
                    return;
                }
                int indice = TypeUsuario.strVoucherCoP.IndexOf("@");
                int longitud = TypeUsuario.strVoucherCoP.Substring(indice).Length;
                TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Substring(TypeUsuario.strVoucherCoP.IndexOf("@"), longitud);
                switch (TypeUsuario.TipoImpresora)
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    case "6":
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        Globales.cpIntegraEMV.dbgPrint(Globales.cpHTTP_sResult + "@br @br @br @br @br @br @br @br @br ");
                        Mouse.OverrideCursor = null;
                        break;
                    default:
                        Globales.MessageBoxMit("No se ha definido un tipo de impresora.\nPara seleccionarla vaya al menu de administracion.");
                        break;
                }
                Mouse.OverrideCursor = null;
            }
            TypeUsuario.strVoucherCoP = strAux;
        }
        public static bool HabilitaNumCuenta(string FormaPago)
        {
            string textTemp = string.Empty;
            textTemp = FormaPago.ToUpper();
            bool HabilitaNumCuenta = false;


            switch (textTemp)
            {

                case "EFECTIVO":
                    {
                        HabilitaNumCuenta = false;
                    }
                    break;
                case "CHEQUE NOMINATIVO":
                    HabilitaNumCuenta = true;
                    break;
                case "TRANSF. ELECTRÓNICA":
                    HabilitaNumCuenta = true;
                    break;
                case "TARJETA DE CRÉDITO":
                    HabilitaNumCuenta = true;
                    break;
                case "MONEDERO ELECTRÓNICO":
                    HabilitaNumCuenta = false;
                    break;
                case "DINERO ELECTRÓNICO":
                    HabilitaNumCuenta = false;
                    break;
                case "VALES DE DESPENSA":
                    HabilitaNumCuenta = false;
                    break;
                case "TARJETA DE DÉBITO":
                    HabilitaNumCuenta = true;
                    break;
                case "TARJETA DE SERVICIO":
                    HabilitaNumCuenta = true;
                    break;

                case "OTROS":
                    HabilitaNumCuenta = false;
                    break;
                default:
                    HabilitaNumCuenta = false;
                    break;

            }
            return HabilitaNumCuenta;

        }
        public static bool EsTarjetaCredito()
        {
            string cc_type = string.Empty;
            bool EsTarjetaCredito = true;
            cc_type = Program.cpIntegraEMV.getCc_TypeTRX().ToUpper();

            if (cc_type.Contains("CREDITO") || cc_type.Contains("CRÉDITO"))
            {
                EsTarjetaCredito = true;
            }

            if (cc_type.Contains("DEBITO") || cc_type.Contains("DÉBITO"))
            {
                EsTarjetaCredito = false; ;
            }
            return EsTarjetaCredito;
        }
        //public static cpIntegracionEMV.clsPrePagoTrx PPOperacion = new cpIntegracionEMV.clsPrePagoTrx();
        internal static System.Windows.Controls.ComboBox llenaImp(System.Windows.Controls.ComboBox combo1)
        {
            dynamic myprinters;
            int cont;
            int prnCount = 0;
            myprinters = Globales.clsImpr.ASPPrinter.GetPrinters(prnCount);
            foreach (var item in myprinters)
            {
                combo1.Items.Add(item);
            }
            combo1.SelectedIndex = 0;
            return combo1;
        }
        internal static void ImprimeCorteRecomensas(string p)
        {
            Globales.cpIntegraEMV.dbgImprimeCorteRecom(p);
        }
        internal static void cleanValoresQualitas()
        {
            isCoberturaMultiple = false;
            numTotalCoberturas = 0;
            contCoberturas = 0;
            docXML = null;
            nodeListXML = null;
            nodeXML = null;
            typeUsuarioQualitas.TipoCobro = "";
            typeUsuarioQualitas.NumPoliza = "";
            typeUsuarioQualitas.NumSiniestro = "";
            typeUsuarioQualitas.RespuestaCodigo = "";
            typeUsuarioQualitas.RespuestaMensaje = "";
            typeUsuarioQualitas.TipoPagosContado = "";
            typeUsuarioQualitas.TipoPagosMSI = "";
            typeUsuarioQualitas.TipoPagosMSIPlan = "";

            typeUsuarioQualitas.PolizaAsegurado = "";
            typeUsuarioQualitas.PolizaMoneda = "";
            typeUsuarioQualitas.PolizaNumero = "";
            typeUsuarioQualitas.PolizaReciboEndoso = "";
            typeUsuarioQualitas.DeducibleInciso = "";
            typeUsuarioQualitas.DeducibleMoneda = "";
            typeUsuarioQualitas.DeduciblePoliza = "";
            typeUsuarioQualitas.DeducibleReporte = "";
            typeUsuarioQualitas.DeducibleSiniestro = "";

            typeUsuarioQualitas.DeducibleValoracion = "";
            typeUsuarioQualitas.DeducibleCoberturaAplicaDeducible = "";
            typeUsuarioQualitas.DeducibleCoberturaCodigo = "";
            typeUsuarioQualitas.DeducibleCoberturaDescripcion = "";
            typeUsuarioQualitas.DeducibleCoberturaMonto = "";

            typeUsuarioQualitas.DeducibleVehiculoDescripcion = "";
            typeUsuarioQualitas.DeducibleVehiculoModelo = "";
            typeUsuarioQualitas.DeducibleVehiculoSerie = "";

            //Globales.cpIntegraEMV.dbgSetQualitasTipoPagosContado("");
            //Globales.cpIntegraEMV.dbgSetQualitasTipoPagosMSI("");
            //Globales.cpIntegraEMV.dbgSetQualitasMoneda("");
            //Globales.cpIntegraEMV.dbgSetQualitasPlanPagosMSI("");
            //Globales.cpIntegraEMV.dbgSetQualitasTipocobro("");

        }
        public static int numTotalCoberturas { get; set; }
        public static int contCoberturas { get; set; }
        public static XmlDocument docXML { get; set; }
        public static XmlNodeList nodeListXML { get; set; }
        public static XmlNode nodeXML { get; set; }
        public const string ipQualitas = "https://www.qualitas.com.mx/agentes/pcpay-ws/servicios/PcPayService?wsdl";
        public const string userQualitasWS = "userPcP4y";
        public const string passQualitasWS = "1dF23.-o/";
        public static bool isQualitasActualizado { get; set; }
        public static bool consultaBoleto(string res)
        {
            bool consultaBoleto = false;
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                string strCadEncriptar, strCadaux, myxml = string.Empty;
                myxml = "xml=<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                myxml += "<reg_agencia><agencia>";
                myxml += "<nb_reservacion>" + res + "</nb_reservacion>";
                myxml += "</agencia></reg_agencia></xml>";
                strNombreFP = NOMBRE_GENERAL + ".insertaReservacion()";

                strCadEncriptar = "&" + myxml + "&op=consRes";

                //  DoEvents
                Globales.cpHTTP_Clear();
                Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
                Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
                if (Globales.cpHTTP_SendcpCUCT())
                {
                    Mouse.OverrideCursor = null;
                    strCadaux = Globales.GetDataXml("nb_status", Globales.cpHTTP_sResult).ToLower();
                    if (strCadaux == "exito")
                    {

                        consultaBoleto = true;
                    }
                    else
                    {
                        strCadaux = Globales.GetDataXml("nb_error", Globales.cpHTTP_sResult);
                        if (!string.IsNullOrWhiteSpace(strCadaux))
                        {
                            Globales.MessageBoxMit(strCadaux);


                        }
                        consultaBoleto = false;
                    }
                }
                Mouse.OverrideCursor = null;
            }

            catch (Exception Err)
            {
                Mouse.OverrideCursor = null;
                Globales.MessageBoxMit(strNombreFP + "Error: " + "\n" + "Descripcion: " + Err.Message);
            }
            return consultaBoleto;

        }
        public static string InfoPNR { get; set; }
        public static bool isVentaForzada { get; set; }
        public static string ultimaTrxPD { get; set; }
        public static string printerName { get; set; }

        public static string ArchivoServidor { get; set; }

        public static string ArchivoInstalador { get; set; }

        public static string ArchivoActualizar { get; set; }

        internal static string TipoUpdate(string actual, string nuevo)
        {
            string[] v1 = actual.Split('.');
            string[] v2 = nuevo.Split('.');
            if (Convert.ToInt32(v1[0]) < Convert.ToInt32(v2[0]))
            {
                return "msi";
            }
            else if (Convert.ToInt32(v1[1]) < Convert.ToInt32(v2[1]))
            {
                return "msi";
            }
            else if (Convert.ToInt32(v1[2]) < Convert.ToInt32(v2[2]))
            {
                return "msi";
            }
            else
            {
                return "no";
            }
        }

        public static int TA { get; set; } //=2, TA=1 mejora y TA=2 cambío de versión

        public static bool desvincularInstaladorLector { get; set; }

        internal static string KEY_RC4_CP_COM(string Bs_Company)
        {
            string valor = string.Empty;
            try
            {
                valor = Globales.encryptString(Bs_Company, Bs_Company, true);
            }
            catch
            {
                valor = "KEY CREDIT CARD KEY";
            }
            return valor;
        }

        internal static bool verificadorBoleto(string TNUMBOLETO)
        {
            bool valor = false;
            if (isAerolinea)
            {
                if (TNUMBOLETO.Length < 10)
                {
                    Globales.MessageBoxMit("¡La longitud del boleto es incorrecta!");
                    valor = true;
                }
            }
            else {
                if (TNUMBOLETO.Length != 11)
                {
                    Globales.MessageBoxMit("¡La longitud del boleto es incorrecta!");
                    valor = true;
                }
            }
            return valor;
        }

        public static string leyendaVoucher = "POR ESTE PAGARE ME OBLIGO INCONDI \n"+
"CIONALMENTE A PAGAR A LA ORDEN DEL\n"+
"BANCO EMISOR EL IMPORTE DE ESTE\n"+
"TITULO EN LOS TERMINOS DEL CONTRA\n"+
"TO SUSCRITO PARA EL USO DE ESTA\n"+
"TARJETA DE CREDITO EN EL CASO DE\n"+
"OPERACIONES CON TARJETA DE DEBITO,\n"+
"EXPRESAMENTE RECONOZCO Y ACEPTO\n"+
"ESTE RECIBO ES EL COMPROBANTE DE\n"+
"LA OPERACION REALIZADA, MISMA QUE\n"+
"SE CONSIGNA  EN EL ANVERSO Y TEN\n"+
"DRA PLENO VALOR PROBATORIO Y FUER\n"+
"ZA LEGAL, EN VIRTUD DE QUE LO FIR\n"+
"ME PERSONALMENTE Y/O DIGITE MI NU\n"+
"MERO DE IDENTIFICACION PERSONAL\n"+
"COMO FIRMA ELECTRONICA EL CUAL ES\n"+
"EXCLUSIVO DE MI RESPONSABILIDAD\n"+
"MANIFESTANDO PLENA CONFORMIDAD\n" +
"AL RESPECTO.";
    }
    public class itemCombobox
    {
        public string item { get; set; }
        public string value { get; set; }
    }
}
