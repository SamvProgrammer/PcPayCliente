using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PcPay.Code.Utilidades;
using System.Windows.Forms;
using PcPay.Code.Configuracion;
using PcPay.Code.Clases;
using PcPay.Code.Usuario;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using PcPay.Configuracion;
using PcPay.Forms.Formularios.MessagesW;
using System.Drawing;
using System.IO;
using System.Drawing.Printing;

class Globales
{
    public static PcPay.Forms.frmMenuPrincipal principal { get; set; }


    //************************************************************
    //**            DEFINICION DE VARIABLES                    ***
    //************************************************************
    public static List<Key> NUMBERKEYS
    {
        get
        {
            return new Key[] { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, 
                Key.NumPad0, Key.NumPad1,Key.NumPad2,Key.NumPad3,Key.NumPad4,Key.NumPad5,Key.NumPad6,Key.NumPad7,
                Key.NumPad8,Key.NumPad9}.ToList();
        }
    }
    public static string NOMBRE_APP
    {
        get
        {
            return modMIT.NOMBRE_APP;
        }
        set
        {
            modMIT.NOMBRE_APP = value;
        }
    }

    //public static cpIntegracionEMV.clsPrePagoTrx PPOperacion
    //{
    //    get
    //    {
    //        return modMIT.PPOperacion;
    //    }
    //    set
    //    {
    //        modMIT.PPOperacion = value;
    //    }
    //}
    public static cpIntegracionEMV.clsCpIntegracionEMV cpIntegraEMV
    {
        get
        {
            return PcPay.Code.Configuracion.Program.cpIntegraEMV;
        }
        set
        {
            PcPay.Code.Configuracion.Program.cpIntegraEMV = value;
        }
    }
    //public static cpIntegracionEMV.clsServicios TiempoAire
    //{
    //    get
    //    {
    //        return modMIT.TiempoAire;
    //    }
    //    set
    //    {
    //        modMIT.TiempoAire = value;
    //    }
    //}
    public static PcPay.Code.Clases.clsMITVtaServicios VServicios
    {
        get
        {
            return modMIT.VServicios;
        }
        set
        {
            modMIT.VServicios = value;
        }
    }
    public static PcPay.Code.Clases.ClsImp clsImpr = new PcPay.Code.Clases.ClsImp();
    public static string strNombreFP
    {
        get
        {
            return modMIT.strNombreFP;
        }
        set
        {
            modMIT.strNombreFP = value;
        }
    }
   
    public static string centrado { get; set; }
    public static string margenIzq { get; set; }
    public static string configCorte { get; set; }
    public static string configLeyenda { get; set; }
    public static string configCentrado { get; set; }
    public static string config { get; set; }
    public static string URL_DLL_CANC
    {
        get
        {
            return modMIT.URL_DLL_CANC;
        }
        set
        {
            modMIT.URL_DLL_CANC = value;
        }
    }
    //************************************************************
    //**            DEFINICION DE METODOS GLOB                  ***
    //*************************************************************
    public static string GetSettingString(string llave)
    {
        return Utils.ObtieneParametrosMIT(llave);
    }
    public static void SaveSettingString(string llave, string valor)
    {
        Utils.GuardaParametrosMIT(llave, valor);
    }
    public static string GetDataXml(string llave, string xml)
    {
        return Utils.GetDataXML(xml, llave);
    }
    public static string encryptString(string cadena, string llave = "", bool hexadeximal = true)
    {
        return EncryptC.EncryptRC4(cadena, llave, true);
    }
    public static string DecryptString(string valor, string llave = "", bool hexadecimal = true)
    {
        return EncryptC.DecryptRC4(valor, llave, hexadecimal);
    }
    internal static DialogResult mensajeQuestion(string p1, string p2)
    {
        return MessageBox.Show(p1, p2, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }
    public static int banImp
    {
        get
        {
            return modMIT.banImp;
        }
        set
        {
            modMIT.banImp = value;
        }
    }
    public static int banNoVchr
    {
        get
        {
            return modMIT.banNoVchr;
        }
        set
        {
            modMIT.banNoVchr = value;
        }
    }
    public static string KEYCARD
    {
        get
        {
            return modMIT.KEYCARD;
        }
    }
    internal static void BeginAplicacion()
    {
        modMIT.BeginAplicacion();
    }
    internal static string STD_getLicence()
    {
        return cpTdcStd.STD_getLicence();
    }
    internal static void STD_GeneratePrint()
    {
        cpTdcStd.STD_GeneratePrint();
    }
    internal static void dbgEndOperation()
    {

    }
    internal static string EstableceLector(bool lector = false)
    {
        return modMIT.EstableceLector(lector);
    }
    internal static string acentoOnString(string value)
    {
        return Utils.acentoOnString(value);
    }
    internal static string GetVersionApp()
    {
        return Utils.GetVersionApp();
    }
    internal static Dictionary<string, List<MenuOpciones>> getMenu()
    {
        return Ayuda.getMenu();
    }
    public static bool isAerolinea
    {
        get
        {
            return modMIT.isAerolinea;
        }
        set
        {
            modMIT.isAerolinea = value;
        }
    }
    public static bool isAgencias
    {
        get
        {
            return modMIT.isAgencias;
        }
        set
        {
            modMIT.isAgencias = value;
        }
    }

    internal static void setConfiguracion(string p)
    {
        modMIT.setConfiguracion(p);
    }
    public static string EMVLector
    {
        get
        {
            return modMIT.EMVLector;
        }
        set
        {
            modMIT.EMVLector = value;
        }
    }
    public static string BandImpHTML
    {
        get
        {
            return modMIT.BandImpHTML;
        }
        set
        {
            modMIT.BandImpHTML = value;
        }
    }
    public static string BandImpManual
    {
        get
        {
            return modMIT.BandImpManual;
        }
        set
        {
            modMIT.BandImpManual = value;
        }
    }
    public static string sPathUserLog
    {
        get
        {
            return modMIT.sPathUserLog;
        }
        set
        {
            modMIT.sPathUserLog = value;
        }
    }
    internal static void cpHTTP_Clear()
    {
        cpHTTP.cpHTTP_Clear();
    }
    public static string cpHTTP_sURL_cpCUCT
    {
        get
        {
            return cpHTTP.cpHTTP_sURL_cpCUCT;
        }
        set
        {
            cpHTTP.cpHTTP_sURL_cpCUCT = value;
        }
    }
    public static string cpHTTP_cadena1
    {
        get
        {
            return cpHTTP.cpHTTP_cadena1;
        }
        set
        {
            cpHTTP.cpHTTP_cadena1 = value;
        }
    }
    public static string KEY_RC4_CP
    {
        get
        {
            return modMIT.KEY_RC4_CP;
        }

    }
    internal static bool cpHTTP_SendcpCUCT()
    {
        return cpHTTP.cpHTTP_SendcpCUCT();
    }
    public static string cpHTTP_sResult
    {
        get
        {
            return cpHTTP.cpHTTP_sResult;
        }
        set
        {
            cpHTTP.cpHTTP_sResult = value;
        }
    }

    public static bool isReporte { get; set; }
    internal static void Imprimir_HTML(string p, bool _isReporte = false)
    {
        isReporte = _isReporte;
        modMIT.Imprimir_HTML(p);
    }
    public static string KEY_RC4
    {
        get
        {
            return modMIT.KEY_RC4;
        }
    }
    internal static void PrintOptions(string Voucher, string NumOpe = "", PrintDialog Impresora = null, bool SoloUnaHoja = false, bool isFAE = false)
    {
        modMIT.PrintOptions(Voucher, NumOpe, Impresora, SoloUnaHoja,isFAE);
    }
    internal static void VerificaVoucher()
    {

    }
    internal static void cpIntegracion_Clear()
    {
        PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_Clear();
    }
    public static string sURL_cpINTEGRA
    {
        get
        {
            return PcPay.Code.Utilidades.cpIntegracion.sURL_cpINTEGRA;
        }
        set
        {

            PcPay.Code.Utilidades.cpIntegracion.sURL_cpINTEGRA = value;

        }
    }
    public static string URL_3GATE
    {
        get
        {
            return modMIT.URL_3GATE;
        }
        set
        {
            modMIT.URL_3GATE = value;
        }
    }
    public static string cpIntegracion_sResult
    {
        get
        {
            return PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult;
        }
        set
        {
            PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult = value;
        }
    }

    public static string msjRech
    {
        get
        {
            return modMIT.msjRech;
        }
        set
        {
            modMIT.msjRech = value;
        }
    }
    public static bool lectorConectado { get; set; }
    public static bool isVentasPropias
    {
        get
        {
            return modMIT.isVentasPropias;
        }
        set
        {
            modMIT.isVentasPropias = value;
        }
    }
    public static string EMPREF
    {
        get
        {
            return modMIT.EMPREF;
        }
    }
    public static string EMPREF2
    {
        get
        {
            return modMIT.EMPREF2;
        }
    }
    public static bool bolActivaMotoP
    {
        get
        {
            return modMIT.bolActivaModoP;
        }
        set
        {
            modMIT.bolActivaModoP = value;
        }
    }
    internal static System.Windows.Controls.ComboBox obtieneBancos(System.Windows.Controls.ComboBox cboBanco, string p)
    {
        return modMIT.obtieneBancos(cboBanco, p);
    }
    public static int MAXCAR
    {
        get
        {
            return modMIT.MAXCAR;
        }
    }
    public static bool isQualitas
    {
        get
        {
            return modMIT.isQualitas;
        }
        set
        {
            modMIT.isQualitas = value;
        }
    }
    public static bool isCoberturaMultiple
    {
        get
        {
            return modMIT.isCoberturaMultiple;
        }
        set
        {
            modMIT.isCoberturaMultiple = value;
        }
    }
    internal static void ReseteaVariablesRecompensas()
    {
        modMIT.ReseteaVariablesRecompensas();
    }
    public static string RGEV
    {
        get
        {
            return modMIT.RGEV;
        }
    }
    internal static bool RefGEV(string strDatos, string codFPago, string Ref = "")
    {
        return modMIT.RefGEV(strDatos, codFPago, Ref);
    }
    public static string CodGEV
    {
        get
        {
            return modMIT.CodGEV;
        }
        set
        {
            modMIT.CodGEV = value;
        }
    }
    public static string merchantBanda
    {
        get
        {
            return modMIT.merchantBanda;
        }
        set
        {
            modMIT.merchantBanda = value;
        }
    }
    public static bool isAmex
    {
        get
        {
            return modMIT.IsAmex;
        }
        set
        {
            modMIT.IsAmex = value;
        }
    }
    public static string csvAmexenBanda
    {
        get
        {
            return modMIT.csvAmexenBanda;
        }
        set
        {
            modMIT.csvAmexenBanda = value;
        }
    }
    public static bool hayDobleVoucherRecom
    {
        get
        {
            return modMIT.hayDoubleVoucherRecom;
        }
        set
        {
            modMIT.hayDoubleVoucherRecom = value;
        }
    }
    public static string strCadenaVoucherVtaDirecta
    {
        get
        {
            return modMIT.strCadenaVoucherVtaDirecta;
        }
        set
        {
            modMIT.strCadenaVoucherVtaDirecta = value;
        }
    }
    public static string strCadenaVoucherVtaRecomensa
    {
        get
        {
            return modMIT.strCadenaVoucherVtaRecompensas;
        }
        set
        {
            modMIT.strCadenaVoucherVtaRecompensas = value;
        }
    }
    public static string cvsAmexEnBanda { get; set; }
    public static bool sinSaldoRecomDirecto
    {
        get
        {
            return modMIT.sinSaldoRecomDirecto;
        }
        set
        {
            modMIT.sinSaldoRecomDirecto = value;
        }
    }
    public static string errorRecom
    {
        get
        {
            return modMIT.errorRecom;
        }
        set
        {
            modMIT.errorRecom = value;
        }
    }
    public static string saldoRecomDirecto
    {
        get
        {
            return modMIT.saldoRecomDirecto;
        }
        set
        {
            modMIT.saldoRecomDirecto = value;
        }
    }
    public static string diferenciaRecomDirecto
    {
        get
        {
            return modMIT.diferenciaRecomDirecto;
        }
        set
        {
            modMIT.diferenciaRecomDirecto = value;
        }
    }
    internal static void printVoucherRecompensasDirecto(string Voucher)
    {
        modMIT.printVoucherRecompensasDirecto(Voucher);
    }
    public static bool hayTrxRecompensa
    {
        get
        {
            return modMIT.hayTrxRecompensa;
        }
        set
        {
            modMIT.hayTrxRecompensa = value;
        }
    }
    public static string FacturaE
    {
        get
        {
            return modMIT.FacturaE;
        }
        set
        {
            modMIT.FacturaE = value;
        }
    }
    public static string XMLFacturaE
    {
        get
        {
            return modMIT.XMLFacturaE;
        }
        set
        {
            modMIT.XMLFacturaE = value;
        }
    }
    public static bool isQualitasCierraForm
    {
        get
        {
            return modMIT.isQualitasCierraForm;
        }
        set
        {
            modMIT.isQualitasCierraForm = value;
        }
    }
    public static bool isCDP
    {
        get
        {
            return modMIT.isCDP;
        }
        set
        {
            modMIT.isCDP = value;
        }
    }
    internal static string EncryptTripleDES(string Text, string Key)
    {
        return EncryptC.EncryptTripleDES(Text, Key);
    }
    internal static string DecryptTripleDES(string Text, string Key)
    {
        return EncryptC.DecryptTripleDES(Text, Key);
    }
    public static MITWebService WS
    {
        get
        {
            return modMIT.WS;
        }
        set
        {
            modMIT.WS = value;
        }
    }
    public static string URL_VTASERV
    {
        get
        {
            return modMIT.URL_VTASERV;
        }
        set
        {
            modMIT.URL_VTASERV = value;
        }
    }
    public static Dictionary<string, Categoria> getProductos()
    {
        return Ayuda.getProductos();
    }
    public static string merchantMoto
    {
        get
        {
            return modMIT.merchantMoto;
        }
        set
        {
            modMIT.merchantMoto = value;
        }
    }
    internal static string generarSemilla(string p)
    {
        return PcPay.Code.Utilidades.EncryptC.generarSemilla(p);
    }
    public static string cpIntegracon_cpAVSs2(string Id_Company, string Id_Branch, string country, string User,
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
        cpIntegraEMV.dbgClearDLL();
        PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_Clear();
        PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_cpAVSs2(Id_Company,
            Id_Branch,
            country,
            User,
            Pwd,
            merchant,
            reference,
            tp_operation,
            typeC,
            nameC,
            numberC,
            expmonthC,
            expyearC,
            cvvcscC,
            Amount,
            currencyC,
            direccion,
            NumInt,
            NumExt,
            delegacion,
            ciudad,
            Estado,
            cp,
            colonia, nombreC, PaisC, TelefonoC, CorreoC, Tx_isCheckin, Tx_boletos,
            Tx_fechaSalida, Tx_fechaRetorno);
        //cpIntegracion.cpIntegracion_cpAVSs2();
        if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("response", PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult)))
        {
            string Voucher = string.Empty;
            string VoucherAct = string.Empty;
            if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("voucher_comercio", PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult)))
            {
                string comercio = string.Empty;
                string cliente = string.Empty;
                comercio = Globales.CheckVoucher(GetDataXml("voucher_comercio", PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult));
                cliente = Globales.CheckVoucher(GetDataXml("voucher_cliente", PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult));
                Voucher = "<voucher_comercio>" + comercio + "</voucher_comercio>" + "<voucher_cliente>" + cliente + "</voucher_cliente>;    @";
                if (!dbgGetPrintBC())
                {
                    Voucher = Voucher.Replace("@bc " + GetDataXml("foliocpagos", PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult), "");
                }
                else if (cpIntegraEMV.chkPp_Trademark().Trim() == "INGENICO" && cpIntegraEMV.chkPp_Printer() == "1")
                {
                    // Voucher = Voucher.Replace(MetodosCadena.Mid(Voucher, Microsoft.VisualBasic.Strings.InStr(1, Voucher, "@bc "), 17), MetodosCadena.Mid(Voucher, Microsoft.VisualBasic.Strings.InStr(1, Voucher, "@bc "), 13) + "@br");
                }
                VoucherAct = Globales.GetDataXml("voucher", PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult);

                if (TypeUsuario.TipoImpresora == "6")
                {
                    PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult = PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult.Replace(VoucherAct, Voucher);
                }
            }

            else
            {
                PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult = "";
            }
        }
        //trx.XML = PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult;
        PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult = WS.WS_Response;
        return PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sResult;
    }
    public static bool dbgGetPrintBC()
    {
        return modMIT.isC85;
    }
    public static string URL_DLL
    {
        get
        {
            return modMIT.URL_DLL;
        }
        set
        {
            modMIT.URL_DLL = value;
        }
    }
    public static string cpIntegracion_sURL_cpINTEGRA
    {
        get
        {
            return PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sURL_cpINTEGRA;
        }
        set
        {
            PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sURL_cpINTEGRA = value;

        }
    }
    public static string ipFirmaPanel
    {
        get
        {
            return Program.ipFirmaPanel;
        }
    }
    public static bool IsOM
    { //Indica para la configuracipón de mensajes en pantalla
        get
        {
            return modMIT.IsOM;
        }
        set
        {
            modMIT.IsOM = value;
        }
    }
    public static string COD_VF
    {
        get
        {
            return modMIT.COD_VF;
        }
    }
    public static string URL_DLL_VF
    {
        get
        {
            return modMIT.URL_DLL_VF;
        }
        set
        {
            modMIT.URL_DLL_VF = value;
        }
    }
    internal static bool cpIntegracion_cpVtaForzadaM(
        string Id_Company,
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
        return PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_cpVtaForzadaM(
                               Id_Company,
                               Id_Branch,
                               country,
                               User,
                               Pwd,
                               merchant,
                               reference,
                               tp_operacion,
                               typeC,
                               nameC,
                               numberC,
                               expMonthC,
                               expyearC,
                               cvvcscC,
                               Amount,
                               currencyC,
                               no_operacion,
                               auth,
                               iCFA
                            );
    }
    public static string CRYPTO
    {
        get
        {
            return modMIT.CRYPTO;
        }
    }
    internal static string CheckVoucher(string strVoucherCop)
    {
        return PcPay.Code.Utilidades.cpIntegracion.CheckVoucher(strVoucherCop);
    }
    public static string cpIntegracion_sError
    {
        get
        {
            return PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sError;
        }
        set
        {
            PcPay.Code.Utilidades.cpIntegracion.cpIntegracion_sError = value;
        }
    }
    public static string URL_REF
    {
        get
        {
            return modMIT.URL_REF;
        }
        set
        {
            modMIT.URL_REF = value;
        }
    }
    public static string MsjReferencia
    {
        get
        {
            return modMIT.MsjReferencia;
        }
        set
        {
            modMIT.MsjReferencia = value;
        }
    }
    public static int LCodGEV
    {
        get
        {
            return modMIT.LCodGEV;
        }
        set
        {
            modMIT.LCodGEV = value;
        }
    }
    internal static void CFEImporte(string p)
    {
        modMIT.CFEImporte(p);
    }
    public static string MsjImporte
    {
        get
        {
            return modMIT.MsjImporte;
        }
        set
        {
            modMIT.MsjImporte = value;
        }
    }
    public static string ip
    {
        get
        {
            return Program.ip;
        }
        set
        {
            Program.ip = value;
        }
    }
    public static string ipvip
    {
        get
        {
            return modMIT.ipvip;
        }
        set
        {
            modMIT.ipvip = value;
        }
    }
    public static string drpPROD
    {
        get
        {
            return modMIT.drpPROD;
        }
        set
        {
            modMIT.drpPROD = value;
        }
    }
    public static string drpVIP
    {
        get
        {
            return modMIT.drpVIP;
        }
        set
        {
            modMIT.drpVIP = value;
        }
    }
    public static int BanImg
    {
        get
        {
            return modMIT.BanImg;
        }
        set
        {
            modMIT.BanImg = value;
        }
    }
    internal static string getRutaUsuario()
    {
        return modMIT.getRuta();
    }
    public static bool isSmart
    {
        get
        {
            return modMIT.isSmart;
        }

        set
        {
            modMIT.isSmart = value;
        }
    }
    internal static void PrintOptions2(string VxVoucher, string NumOpe, bool soloUnaHoja = false)
    {
        modMIT.PrintOptions2(VxVoucher, NumOpe, soloUnaHoja);
    }
    public static string FormatMoneda(string cantidad)
    {
        foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
        {
            try
            {
                cantidad = cantidad.Replace(new RegionInfo(ci.Name).CurrencySymbol, "");
            }
            catch
            {

            }
        }
        double valor = Convert.ToDouble(cantidad);
        return valor.ToString("0.00", CultureInfo.InvariantCulture);
    }
    public static cpIntegracionEMV.clsServicios CpCobro3G
    {
        get { return modMIT.CpCobro3G; }
    }

    public static string strHexFirmaPanel { get; set; }
    public static bool ObtieneFirmaPanel(string urlFirma, string textoAgua, int tipoVta, bool IsChipAndPin, bool esQPS)
    {
        bool answer = false;
        strHexFirmaPanel = string.Empty;

        if (Globales.cpIntegraEMV.EsTouch())
        {
            if (!(IsChipAndPin && tipoVta == 1) || esQPS)
            {
                Globales.cpIntegraEMV.ObtieneFirmaPanel(textoAgua);
                if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.Error()))
                    strHexFirmaPanel = Globales.cpIntegraEMV.TextoHEXFirmaPanel();
                else
                {
                    MessageBox.Show(string.Format("No se pudo obtener la firma desde el dispositivo \n{0}", Globales.cpIntegraEMV.Error()));
                    return answer;
                }
            }

            string val1 = cpIntegraEMV.getRspOperationNumber();
            string val2 = cpIntegraEMV.chkPp_Serial();
            string val3 = cpIntegraEMV.dbgGetId_Company();
            string val4 = cpIntegraEMV.dbgGetId_Branch();
            string val5 = cpIntegraEMV.dbgGetCountry();
            string val6 = cpIntegraEMV.dbgGetUser();
            string val7 = cpIntegraEMV.dbgGetPass();
            string val8 = cpIntegraEMV.getRspVoucher();
            string val9 = cpIntegraEMV.chkPp_Trademark();


            if (cpIntegraEMV.sndFirmaEnPanel(true, strHexFirmaPanel, urlFirma, textoAgua,
                val1, val2, val3, val4, val5, val6, val7, val8, IsChipAndPin, val9, tipoVta))
                answer = true;
        }
        else
        {
            if (cpIntegraEMV.getRspSoportaFirmaPanel())
            {
                if (cpIntegraEMV.sndFirmaEnPanel(true, string.Empty, urlFirma, textoAgua, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial(),
                                       Globales.cpIntegraEMV.dbgGetId_Company(), Globales.cpIntegraEMV.dbgGetId_Branch(), Globales.cpIntegraEMV.dbgGetCountry(), cpIntegraEMV.dbgGetUser(), Globales.cpIntegraEMV.dbgGetPass(),
                                       Globales.cpIntegraEMV.getRspVoucher(), IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), tipoVta))
                    answer = true;
            }
        }
        return answer;
    }
    internal static void GeneraFacturaElectronica(string Importe, string operacion, string fecha, string Propina = "")
    {
        modMIT.GeneraFacturaElectronica(Importe, operacion, fecha, Propina);
    }
    internal static void FacturaElectronica(string Importe, string Operacion, string Fecha, string propina = "")
    {
        modMIT.FacturaElectronica(Importe, Operacion, Fecha, propina);
    }
    public static bool setReader
    {
        get
        {
            return modMIT.setReader;
        }
        set
        {
            modMIT.setReader = value;
        }
    }
    public static string InfoCheckOut
    {
        get
        {
            return modMIT.InfoCheckOut;
        }
        set
        {
            modMIT.InfoCheckOut = value;
        }
    }
    public static bool CloseFr
    {
        get
        {
            return modMIT.CloseFr;
        }
        set
        {
            modMIT.CloseFr = value;
        }
    }
    internal static void PrintOptionsTAE(string VxVoucher, string NumOpe, string opcionImpresion, System.Windows.Controls.PrintDialog printDialog)
    {


        modMIT.PrintOptionsTAE(VxVoucher, NumOpe, opcionImpresion, printDialog);
    }
    internal static string getPublicidad(string nbCodigoEstado, string nbCodigogiro, string cdOperacion)
    {
        return modMIT.getPublicidad(nbCodigoEstado, nbCodigogiro, cdOperacion);
    }
    public static string URL_PUBLICIDAD
    {
        get
        {
            return modMIT.URL_PUBLICIDAD;
        }
        set
        {
            modMIT.URL_PUBLICIDAD = value;
        }
    }
    internal static string InvokeWebService(string strSoap, string strSOAPAction, string strWsdl)
    {
        return modMIT.InvokeWebService(strSoap, strSOAPAction, strWsdl);
    }
    internal static void frmImprimeReporteDiario()
    {
        modMIT.frmImprimeReporteDiario();
    }
    internal static void soloNumeroConPunto(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        System.Windows.Controls.TextBox texto = (System.Windows.Controls.TextBox)sender;
        Regex es;
        if (texto.Text.Contains(".") && e.Text.Contains("."))
        {
            es = new Regex("[^¡]+");
            e.Handled = es.IsMatch(e.Text);
            return;
        }
        es = new Regex("[^0-9.]+");
        e.Handled = es.IsMatch(e.Text);

    }
    internal static void soloNumero(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        System.Windows.Controls.TextBox texto = (System.Windows.Controls.TextBox)sender;
        Regex es;
        es = new Regex("[^0-9]+");
        e.Handled = es.IsMatch(e.Text);

    }
    internal static void validaCVV(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        // System.Windows.Controls.PasswordBox texto = (System.Windows.Controls.PasswordBox)sender;
        Regex es;
        es = new Regex("[^0-9]+");
        e.Handled = es.IsMatch(e.Text);

    }
    public static void soloTexto(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        Regex es = new Regex("[^A-Za-z]+");
        e.Handled = es.IsMatch(e.Text);
    }
    public static void soloTNumeroTexto(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        Regex es = new Regex("[^A-Za-z0-9]+");
        e.Handled = es.IsMatch(e.Text);
    }
    public static bool HabilitaNumCuenta(string FormaPago)
    {
        return modMIT.HabilitaNumCuenta(FormaPago);

    }
    public static bool EsTarjetaCredito()
    {
        return modMIT.EsTarjetaCredito();
        //string cc_type = string.Empty;
        //bool EsTarjetaCredito = true;
        //cc_type = cpIntegraEMV.getCc_TypeTRX().ToUpper();

        //if (cc_type.Contains("CREDITO") || cc_type.Contains("CRÉDITO"))
        //{
        //    EsTarjetaCredito = true;
        //}

        //if (cc_type.Contains("DEBITO") || cc_type.Contains("DÉBITO"))
        //{
        //    EsTarjetaCredito = false; ;
        //}
        //return EsTarjetaCredito;
    }
    public static string LimpiarCampo(string p)
    {
        return modMIT.LimpiarCampo(p);
    }
    public static string URL_DLL_CIERREPREVENTA
    {
        get


        { return modMIT.URL_DLL_CIERREPREVENTA; }
        set
        {
            modMIT.URL_DLL_CIERREPREVENTA = value;
        }
    }
    public static bool isNumeric(string cadena)
    {
        cadena = cadena.Replace(".", "");
        bool bandera = true;
        char[] aux = cadena.ToCharArray();
        for (int i = 0; i < aux.Length; i++)
        {
            if (!Char.IsNumber(aux[i]))
            {
                bandera = false;
                break;
            }
        }
        return bandera;
    }
    public static string URL_GIFT
    {
        get
        {
            return modMIT.URL_GIFT;
        }
        set
        {
            modMIT.URL_GIFT = value;
        }
    }
    public static bool ValidaEmail(string email)
    {
        string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        bool esValido = false;
        if (Regex.IsMatch(email, expresion))
        {
            if (Regex.Replace(email, expresion, String.Empty).Length == 0)
            {
                esValido = true;
            }
        }
        return esValido;
    }
    public static string msjTarjeta
    {
        get
        {
            return modMIT.Msjtarjeta;
        }
    }
    public static string msjNombre
    {
        get
        {
            return modMIT.MsjNombre;
        }
    }
    public static string URL_DLL_CHECKIN
    {
        get
        {
            return modMIT.URL_DLL_CHECKIN;
        }
        set
        {
            modMIT.URL_DLL_CHECKIN = value;
        }
    }
    public static string URL_DLL_RA
    {
        get
        {
            return modMIT.URL_DLL_RA;
        }
        set
        {
            modMIT.URL_DLL_RA = value;
        }
    }
    internal static System.Windows.Controls.ComboBox llenaImp(System.Windows.Controls.ComboBox combo1)
    {
        return modMIT.llenaImp(combo1);
    }
    internal static void ImprimeCorteRecomensas(string p)
    {
        modMIT.ImprimeCorteRecomensas(p);
    }
    public string NOMBRE_GENERAL { get; set; }
    public static string mynumero { get; set; }
    public static bool insertaBoletoAgencia(string Id_Company, string strfolio, string strBoletos, string Fs, string Fr)
    {

        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
        string strCadEncriptar, strCadaux = string.Empty;

        //  strNombreFP = NOMBRE_GENERAL + ".insertaBoleto()";
        bool insertaBoletoAgencia = false; ;

        try
        {
            strCadEncriptar = "&transaccion=" + strfolio +
                              "&id_company=" + Id_Company +
                              "&boletos=" + strBoletos +
                              "&op=ins" +
                              "&version=" + TypeUsuario.strVersion +
                              "&fh_salida=" + Fs +
                              "&fh_retorno=" + Fr;

            //  DoEvents
            cpHTTP_Clear();
            cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
            cpHTTP_cadena1 = "enc=" + encryptString(strCadEncriptar, KEY_RC4, true);
            if (cpHTTP_SendcpCUCT())
            {
                Mouse.OverrideCursor = null;
                strCadaux = GetDataXml("response", cpHTTP_sResult).ToLower();
                bool respuesta = (strCadaux == "true" ? true : false);

                if (respuesta)
                {
                    strCadaux = Globales.GetDataXml("desc", cpHTTP_sResult);
                    if (!string.IsNullOrWhiteSpace(strCadaux))
                        Globales.MessageBoxMit(strCadaux);
                    insertaBoletoAgencia = true;
                }
                else
                {
                    strCadaux = Globales.GetDataXml("desc", cpHTTP_sResult);
                    if (!string.IsNullOrWhiteSpace(strCadaux))
                        Globales.MessageBoxMit(strCadaux);
                    insertaBoletoAgencia = false;
                }
            }
            Mouse.OverrideCursor = null;
        }
        catch (Exception Err)
        {
            Mouse.OverrideCursor = null;
            //  MsgBoxEx strNombreFP & vbCrLf & "Error: " & Err.Number & vbCrLf & "Descripcion: " & Err.Description, , , , vbCritical, NOMBRE_APP
            MessageBoxMit("Descripcion:" + Err.Message);
        }
        Mouse.OverrideCursor = null;
        return insertaBoletoAgencia;

    }
    public static bool insertaReservacion(string res, string f_salida, string f_retorno, string Importe, string Empresa, string mBoleto)
    {
        //"dd/mm/yyyy"
        bool insertaReservacion = false;
        string strCadEncriptar, strCadaux, myxml = string.Empty;
        try
        {
            myxml = "xml=<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            myxml += "<reg_agencia><agencia>";
            myxml += "<nb_reservacion>" + res + "</nb_reservacion>";
            myxml += "<fh_salida>" + f_salida.Replace("-", "/") + "</fh_salida>";
            if (!string.IsNullOrWhiteSpace(f_retorno))
            {
                myxml = myxml + "<fh_retorno>" + f_retorno.Replace("-", "/") + "</fh_retorno>";
            }
            myxml += "<nu_importe>" + Importe + "</nu_importe>";
            myxml += "<cd_empresa_originante>" + Empresa.Trim() + "</cd_empresa_originante>";
            myxml += "<tx_boleto>" + mBoleto.Trim() + "</tx_boleto>";
            myxml += "</agencia></reg_agencia></xml>";




            // strNombreFP = NOMBRE_GENERAL & ".insertaReservacion()"

            strCadEncriptar = "&" + myxml + "&op=insRes";

            //   DoEvents
            cpHTTP_Clear();
            cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
            cpHTTP_cadena1 = "enc=" + encryptString(strCadEncriptar, KEY_RC4, true);
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (cpHTTP_SendcpCUCT())
            {
                Mouse.OverrideCursor = null;
                strCadaux = GetDataXml("cd_status", cpHTTP_sResult).ToLower();

                strCadaux = strCadaux == "" ? "0" : strCadaux;
                bool respuesta = strCadaux == "0" ? false : true;

                if (respuesta)
                {
                    modMIT.TGate.dbgSetTGURL(URL_3GATE);
                    string fecha1 = DateTime.Now.Date.ToString("DDMMyyyy", CultureInfo.InvariantCulture);
                    string fecha2 = DateTime.Now.Date.ToString("DD/MM/yyyy", CultureInfo.InvariantCulture);
                    // TGate.snd3GateInsertReference .usu, .Pass, Empresa, res, Format(Now, "DDMMYYYY"), Importe, Format(Now, "DD/MM/YYYY")
                    modMIT.TGate.snd3GateInsertReference(TypeUsuario.usu, TypeUsuario.Pass, Empresa, res, fecha1, Importe, fecha2);

                    MessageBoxMit("Reservación guardada con éxito.");
                    insertaReservacion = true;
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    strCadaux = GetDataXml("nb_error", cpHTTP_sResult);
                    if (!string.IsNullOrWhiteSpace(strCadaux))
                    {
                        MessageBoxMit(strCadaux);
                    }

                    insertaReservacion = false;
                }
            }

        }
        catch (Exception Err)
        {

        }
        return insertaReservacion;


    }
    public static bool isQualitasTrxValida
    {
        get
        {
            return typeUsuarioQualitas.isQualitasTrxValida;
        }
        set
        {
            typeUsuarioQualitas.isQualitasTrxValida = value;
        }
    }
    internal static void cleanValoresQualitas()
    {
        modMIT.cleanValoresQualitas();
    }
    public static string ipQualitas
    {
        get
        {
            return modMIT.ipQualitas;
        }
    }
    internal static string WSSoap(string strXML, string strURL)
    {
        return WS.PostWebserviceSinSOAPAction(strXML, strURL);
    }
    public static string userQualitasWS
    {
        get
        {
            return modMIT.userQualitasWS;
        }
    }
    public static string passQualitasWS
    {

        get
        {
            return modMIT.passQualitasWS;
        }
    }
    public static System.Xml.XmlDocument docXML
    {
        get
        {
            return modMIT.docXML;
        }
        set
        {
            modMIT.docXML = value;
        }
    }
    public static System.Xml.XmlNodeList nodeListXML
    {
        get
        {
            return modMIT.nodeListXML;
        }
        set
        {
            modMIT.nodeListXML = value;

        }
    }
    public static System.Xml.XmlNode nodeXML
    {
        get
        {
            return modMIT.nodeXML;
        }
        set
        {
            modMIT.nodeXML = value;
        }
    }
    public static int numTotalCoberturas
    {
        get
        {
            return modMIT.numTotalCoberturas;
        }
        set
        {
            modMIT.numTotalCoberturas = value;
        }
    }
    public static int contCoberturas
    {
        get
        {
            return modMIT.contCoberturas;
        }
        set
        {
            modMIT.contCoberturas = value;
        }
    }
    public static bool isQualitasActualizado
    {
        get
        {
            return modMIT.isQualitasActualizado;
        }
        set
        {
            modMIT.isQualitasActualizado = value;
        }
    }
    internal static void CheckOm(string p1, string p2)
    {
        // throw new NotImplementedException();
    }
    public static bool consultaBoleto(string res)
    {
        return modMIT.consultaBoleto(res);
    }
    public static Boolean IsNumeric(string valor)
    {
        double result;
        return double.TryParse(valor, out result);
    }
    public static string Right(string param, int length)
    {
        int value = param.Length - length;
        string result = param.Substring(value, length);
        return result;

    }
    public static bool isVentaForzada
    {
        get { return modMIT.isVentaForzada; }
        set { modMIT.isVentaForzada = value; }
    }
    public static int DigitoVerificador(string cveAerolinea, string numBoleto)
    {
        double numBoletoCompleto, DV, mult, tmp, digv;
        // Globales.strNombreFP = NOMBRE_GENERAL + ".DigitoVerificador()";
        int DigitoVerificador = -1;
        numBoletoCompleto = Convert.ToDouble((cveAerolinea + numBoleto));
        if (numBoleto.Length == 10)
        {
            numBoletoCompleto = Convert.ToDouble(cveAerolinea + numBoleto);
        }
        else
        {
            numBoletoCompleto = Convert.ToDouble((cveAerolinea + numBoleto.Substring(0, 10)));
        }
        double valor1 = numBoletoCompleto / 7;
        string valor2 = Convert.ToString(valor1);
        if (!valor2.Contains("."))
        {
            DV = Convert.ToDouble(valor2);
        }
        else
        {
            int posicion = valor2.IndexOf(".");
            valor2 = valor2.Substring(0, posicion + 1);
            DV = Convert.ToDouble(valor2);
        }
        // DV = 0;
        mult = DV * 7;
        digv = mult - numBoletoCompleto;
        digv = digv * -1;
        if (numBoleto.Length == 10)
        {
            DigitoVerificador = Convert.ToInt32(digv);
        }
        else
        {
            if (digv == Convert.ToDouble(numBoleto.Substring(10, 1)))
            {
                DigitoVerificador = Convert.ToInt32(digv);
            }
        }
        return DigitoVerificador;
    }
    public static void enviarCorreo(string p)
    {
        throw new NotImplementedException();
    }
    public static bool insertaBoleto(string strfolio, string strBoletos)
    {
        bool insertaBoleto = false;
        try
        {

            string strCadEncriptar, strCadaux = string.Empty;
            strNombreFP = "NOMBRE_GENERAL" + ".insertaBoleto()";

            strCadEncriptar = "&transaccion=" + strfolio +
                              "&boletos=" + strBoletos +
                              "&op=ins" +
                              "&version=" + TypeUsuario.strVersion +
                              "&updateBoletos=true";

            // DoEvents
            Globales.cpHTTP_Clear();
            cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
            cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
            if (Globales.cpHTTP_SendcpCUCT())
            {
                strCadaux = Globales.GetDataXml("response", Globales.cpHTTP_sResult).ToLower();
                if (!string.IsNullOrWhiteSpace(strCadaux))
                {
                    strCadaux = Globales.GetDataXml("desc", Globales.cpHTTP_sResult);
                    if (!string.IsNullOrWhiteSpace(strCadaux))
                    {
                        Globales.MessageBoxMit(strCadaux);
                    }
                    insertaBoleto = true;
                }
                else
                {
                    strCadaux = Globales.GetDataXml("desc", Globales.cpHTTP_sResult);
                    if (!string.IsNullOrWhiteSpace(strCadaux))
                    {
                        Globales.MessageBoxMit(strCadaux);
                    }
                    insertaBoleto = false;
                }
            }



        }
        catch (Exception Err)
        {
            Globales.MessageBoxMit(strNombreFP + "\n" + "Descripcion: " + Err.Message);

        }

        return insertaBoleto;
    }
    public static string InfoPNR
    {
        get { return modMIT.InfoPNR; }
        set { modMIT.InfoPNR = value; }

    }
    internal static void printOptionPagueDirecto()
    {
        Globales.MessageBoxMit("Falta printOptionPagueDirecto");
    }
    public static string numOperaCancel { get; set; }
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
        return modMIT.cpIntegracion_cpAVSs2(Id_Company, Id_Branch, country, User, Pwd,
                                     merchant,
                                     reference,
                                     tp_operation,
                                     typeC,
                                      nameC,
                                     numberC,
                                      expmonthC,
                                      expyearC,
                                     cvvcscC,
                                      Amount,
                                     currencyC,
                                      direccion,
                                      NumInt,
                                      NumExt,
                                      delegacion,
                                     ciudad,
                                      Estado,
                                      cp,
                                      colonia,
                                      nombreC,
                                      PaisC,
                                      TelefonoC,
                                      CorreoC, Tx_isCheckin, Tx_boletos, Tx_fechaSalida, Tx_fechaRetorno);
    }
    public static string ImporteFormato(string importe)
    {
        double valor = 0;
        string ImporteFormato = string.Empty;
        if (double.TryParse(importe, out valor))
        {
            int aux = 0;
            if (int.TryParse(importe, out aux))
            {
                if (aux > 0)
                {
                    ImporteFormato = aux + ".00";
                }
                else
                {
                    ImporteFormato = "0.00";
                }
            }
            else
            {
                if (valor <= 0)
                {
                    ImporteFormato = "0.00";
                }
                else
                {
                    double valor1 = Math.Round(valor, 2);
                    ImporteFormato = valor1.ToString();
                    if (ImporteFormato.Contains(".00"))
                    {
                        ImporteFormato = ImporteFormato + ".00";
                    }
                }

            }
        }
        else
        {
            ImporteFormato = "0.00";
        }
        return ImporteFormato;
    }
    internal static void noCopy(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Command == ApplicationCommands.Copy ||
                    e.Command == ApplicationCommands.Cut ||
                    e.Command == ApplicationCommands.Paste)
        {
            e.Handled = true;
        }
    }
    internal static void formatoMoneda(object sender, System.Windows.RoutedEventArgs e)
    {
        System.Windows.Controls.TextBox importe = (System.Windows.Controls.TextBox)sender;
        double valor = 0;
        if (double.TryParse(importe.Text, out valor))
        {
            int aux = 0;
            if (int.TryParse(importe.Text, out aux))
            {
                if (aux > 0)
                {
                    importe.Text = aux + ".00";
                }
                else
                {
                    importe.Text = "0.00";
                }
            }
            else
            {
                if (valor <= 0)
                {
                    importe.Text = "0.00";
                }
                else
                {
                    string x1 = valor.ToString();
                    double valor1 = 0;
                    if (x1.Contains(".")){
                        string axu1 = x1.Substring((x1.IndexOf(".")+1));
                        string axu2 = string.Empty;
                        if(axu1.Length == 0){
                            axu2 = x1 + "00";
                        }
                        else if (axu1.Length == 1)
                        {
                            axu2 = x1 + "0";
                        }
                        else {
                            axu2 = x1.Substring(0,x1.IndexOf("."))+"."+axu1.Substring(0,2);
                        }
                        valor1 = Convert.ToDouble(axu2);
                    }
                        
                    else
                        valor1 = Convert.ToDouble(x1);

                   // double valor1 = Math.Round(valor, 2);
                    importe.Text = valor1.ToString();
                    if (importe.Text.Contains(".00"))
                    {
                        importe.Text = importe.Text + ".00";
                    }
                }

            }
        }
        else
        {
            importe.Text = "0.00";
        }

        if (!importe.Text.Contains("."))
        {
            importe.Text += ".00";
        }
        else
        {
            int indice = importe.Text.IndexOf('.');
            string cadena = importe.Text.Substring(indice + 1);
            importe.Text += (cadena.Length < 2) ? "0" : "";
        }

        if (importe.Text == "0.00")
        {
            importe.Text = "";
        }
    }
    public static string ipfe
    {
        get
        {
            return Program.ipfe;
        }
    }
    public static string ultimaTrxPD
    {
        get
        {
            return modMIT.ultimaTrxPD;
        }
        set
        {
            modMIT.ultimaTrxPD = value;
        }
    }
    public static string printerName
    {
        get
        {
            return modMIT.printerName;
        }
        set
        {
            modMIT.printerName = value;
        }
    }
    internal static bool VoucherHtml(string folio, string empresa, string sucursal, string op, string co, string nvoucher = "")
    {
        string strCadEncriptar = "";

        cpHTTP_Clear();
        cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
        strCadEncriptar = "&folio=" + folio +
                          "&empresa=" + empresa +
                          "&sucursal=" + sucursal +
                          "&op=" + op +
                          "&co" + co;
        if (!string.IsNullOrWhiteSpace(nvoucher))
        {
            strCadEncriptar += "&nvoucher=" + nvoucher;
        }
        cpHTTP_cadena1 = "enc=" + encryptString(strCadEncriptar, KEY_RC4, true);
        return cpHTTP_SendcpCUCT();
    }
    public static bool bolActivaMoto { get; set; }
    public static int BandPIN { get; set; }
    public static int BandPIN2 { get; set; }
    public static int CuentaBoletos(System.Windows.Controls.Grid gridBoletos)
    {
        int boletos = 0;
        System.Windows.Controls.UIElementCollection cajas = gridBoletos.Children;
        try
        {
            foreach (Object item in cajas)
            {
                if (item.GetType().ToString().Equals("System.Windows.Controls.Border"))
                {
                    System.Windows.Controls.Border borde = (System.Windows.Controls.Border)item;
                    System.Windows.Controls.Grid ob = (System.Windows.Controls.Grid)borde.Child;
                    System.Windows.Controls.UIElementCollection hijos = ob.Children;
                    foreach (Object items in hijos)
                    {
                        if (items.GetType().ToString().Equals("System.Windows.Controls.TextBox"))
                        {
                            System.Windows.Controls.TextBox texto = (System.Windows.Controls.TextBox)items;
                            if (!string.IsNullOrWhiteSpace(texto.Text))
                            {
                                boletos++;
                            }
                        }
                    }
                }
            }

        }
        catch { }
        return boletos;
    }
    public static void EliminaBoletos(ref System.Windows.Controls.Grid gridBoletos)
    {
        System.Windows.Controls.UIElementCollection cajas = gridBoletos.Children;
        try
        {
            foreach (Object item in cajas)
            {
                if (item.GetType().ToString().Equals("System.Windows.Controls.Border"))
                {
                    System.Windows.Controls.Border borde = (System.Windows.Controls.Border)item;
                    System.Windows.Controls.Grid ob = (System.Windows.Controls.Grid)borde.Child;
                    System.Windows.Controls.UIElementCollection hijos = ob.Children;
                    foreach (Object items in hijos)
                    {
                        if (items.GetType().ToString().Equals("System.Windows.Controls.TextBox"))
                        {
                            System.Windows.Controls.TextBox texto = (System.Windows.Controls.TextBox)items;
                            texto.Text = string.Empty;
                        }


                    }
                }
            }
        }
        catch { }
    }
    public static string CadenaBoletos(System.Windows.Controls.Grid gridBoletos, string digitos, bool NoBoletosExtra)
    {
        string CadenaBoletos = string.Empty;
        string strBoletos = string.Empty;
        if (!NoBoletosExtra)
        {


            System.Windows.Controls.UIElementCollection cajas = gridBoletos.Children;
            try
            {
                foreach (Object item in cajas)
                {
                    if (item.GetType().ToString().Equals("System.Windows.Controls.Border"))
                    {
                        System.Windows.Controls.Border borde = (System.Windows.Controls.Border)item;
                        System.Windows.Controls.Grid ob = (System.Windows.Controls.Grid)borde.Child;
                        System.Windows.Controls.UIElementCollection hijos = ob.Children;
                        foreach (Object items in hijos)
                        {
                            if (items.GetType().ToString().Equals("System.Windows.Controls.TextBox"))
                            {
                                System.Windows.Controls.TextBox texto = (System.Windows.Controls.TextBox)items;
                                if (!string.IsNullOrWhiteSpace(texto.Text))
                                {
                                    strBoletos = digitos + texto.Text + "," + strBoletos;

                                }


                            }
                        }

                    }
                }
                if (strBoletos != string.Empty)
                {
                    if (Right(strBoletos, 1) == ",")
                    {
                        CadenaBoletos = Utils.Mid(strBoletos, 1, strBoletos.Length - 1);
                    }
                }
            }
            catch { }
        }
        return CadenaBoletos;

    }
    //    *********************************
    //--Ventanas personalizadas---    

    #region "Mensajes y Eventos"
    internal static void MessageBoxMit(string message)
    {
        new PcPay.Forms.Formularios.MessagesW.frmMessageBox(message).ShowDialog();
    }

    /// <summary>
    /// Muestra ventana que indica que el proceso se ha efectuado exitosamente.
    /// </summary>
    /// <param name="message">Mensaje de éxito a mostrar.</param>
    /// <param name="isCancel">Establece si la operación fué una cancelación.</param>
    internal static void MessageBoxMitApproved(string message, bool isCancel = false)
    {
        new PcPay.Forms.Formularios.MessagesW.frmVtaAprobada(message, isCancel).ShowDialog();
    }
    /// <summary>
    /// Muestra ventana que indica que el proceso se ha efectuado con éxito.
    /// </summary>
    /// <param name="message">Mensaje de éxito a mostrar.</param>
    /// <param name="title">Título a mostrar en la ventana</param>
    internal static void MessageBoxMitApproved(string message, string title)
    {
        new PcPay.Forms.Formularios.MessagesW.frmVtaAprobada(message, title).ShowDialog();
    }
    /// <summary>
    ///  Muestra ventana que indica que el proceso se ha efectuado con éxito.
    /// </summary>
    /// <param name="message">Mensaje de éxito a mostrar.</param>
    /// <param name="title">Título a mostrar en la ventana</param>
    /// <param name="size">El ancho de la ventana</param>
    internal static void MessageBoxMitApproved(string message, string title, PcPay.MITWindowSize size = PcPay.MITWindowSize.Normal)
    {
        new PcPay.Forms.Formularios.MessagesW.frmVtaAprobada(message, title, size).ShowDialog();
    }

    internal static void MessageBoxMitDenied(string message)
    {
        new PcPay.Forms.Formularios.MessagesW.frmVtaDenegada(message).ShowDialog();
    }
    /// <summary>
    /// Muestra ventana que indica que ha ocurrido un error.
    /// </summary>
    /// <param name="message">Mensaje de error</param>
    /// <param name="title">Título a mostrar en la ventana</param>
    internal static void MessageBoxMitError(string message, string title = "")
    {
        new PcPay.Forms.Formularios.MessagesW.frmVtaError(message, title).ShowDialog();
    }
    internal static bool MessageConfirm(string message)
    {
        var ventana = new PcPay.Forms.Formularios.MessagesW.frmConfirma(message);
        ventana.ShowDialog();
        return ventana.answer;
    }
    //****************************************************************
    //--Efecto al establecer foco en los cuadros de text-----------
    /// <summary>
    /// Establece el efecto de sombra sobre el contenedo del componente que dispara este evento.
    /// </summary>
    /// <param name="sender">Control que dispara el evento</param>
    /// <param name="e">Evento efectuado por el control</param>
    public static void setFocusMit(object sender, System.Windows.RoutedEventArgs e)
    {
        System.Windows.Controls.Border border = null;
        System.Windows.Media.Effects.DropShadowEffect dropShadowEffect = new System.Windows.Media.Effects.DropShadowEffect();
        dropShadowEffect.Opacity = 1;
        dropShadowEffect.ShadowDepth = 1;
        dropShadowEffect.BlurRadius = 8;
        dropShadowEffect.Color = System.Windows.Media.Colors.Firebrick;
        dropShadowEffect.Direction = 0;

        if (sender.GetType() == typeof(System.Windows.Controls.TextBox))
            border = (((System.Windows.Controls.Grid)((System.Windows.Controls.TextBox)sender).Parent)).Parent as System.Windows.Controls.Border;
        else if (sender.GetType() == typeof(System.Windows.Controls.ComboBox))
            border = (((System.Windows.Controls.Grid)((System.Windows.Controls.ComboBox)sender).Parent)).Parent as System.Windows.Controls.Border;
        else if (sender.GetType() == typeof(System.Windows.Controls.PasswordBox))
            border = (((System.Windows.Controls.Grid)((System.Windows.Controls.PasswordBox)sender).Parent)).Parent as System.Windows.Controls.Border;
        border.Effect = dropShadowEffect;
    }
    /// <summary>
    /// Quita el efecto de sombra del contenedor que dispara el eventok.
    /// </summary>
    /// <param name="sender">Control que dispara el evento.</param>
    /// <param name="e">Evento efectuado por el control</param>
    public static void lostFocusMit(object sender, System.Windows.RoutedEventArgs e)
    {
        System.Windows.Controls.Border border = null;
        if (sender.GetType() == typeof(System.Windows.Controls.TextBox))
            border = (((System.Windows.Controls.Grid)((System.Windows.Controls.TextBox)sender).Parent)).Parent as System.Windows.Controls.Border;
        else if (sender.GetType() == typeof(System.Windows.Controls.ComboBox))
            border = (((System.Windows.Controls.Grid)((System.Windows.Controls.ComboBox)sender).Parent)).Parent as System.Windows.Controls.Border;
        else if (sender.GetType() == typeof(System.Windows.Controls.PasswordBox))
            border = (((System.Windows.Controls.Grid)((System.Windows.Controls.PasswordBox)sender).Parent)).Parent as System.Windows.Controls.Border;
        border.Effect = null;
    }
    /// <summary>
    /// Establece el efecto de sombra sobre el contenedor del componente que dispara este evento (Versión 2).
    /// </summary>
    /// <param name="sender">Control que dispara el evento</param>
    /// <param name="e">Evento efectuado por el control</param>
    public static void setFocusMit2(object sender, System.Windows.RoutedEventArgs e)
    {
        System.Windows.Controls.Grid border = null;
        System.Windows.Media.Effects.DropShadowEffect dropShadowEffect = new System.Windows.Media.Effects.DropShadowEffect();
        dropShadowEffect.Opacity = 1;
        dropShadowEffect.ShadowDepth = 1;
        dropShadowEffect.BlurRadius = 8;
        dropShadowEffect.Color = System.Windows.Media.Colors.Firebrick;
        dropShadowEffect.Direction = 0;

        if (sender.GetType() == typeof(System.Windows.Controls.TextBox))
            border = (((System.Windows.Controls.Border)((System.Windows.Controls.TextBox)sender).Parent)).Parent as System.Windows.Controls.Grid;

        else if (sender.GetType() == typeof(System.Windows.Controls.ComboBox))
            border = (((System.Windows.Controls.Border)((System.Windows.Controls.ComboBox)sender).Parent)).Parent as System.Windows.Controls.Grid;

        else if (sender.GetType() == typeof(System.Windows.Controls.PasswordBox))
            border = (((System.Windows.Controls.Border)((System.Windows.Controls.PasswordBox)sender).Parent)).Parent as System.Windows.Controls.Grid;

        border.Effect = dropShadowEffect;
    }
    /// <summary>
    /// Quita el efecto de sombra del contenedor que dispara el evento(Versión2).
    /// </summary>
    /// <param name="sender">Control que dispara el evento.</param>
    /// <param name="e">Evento efectuado por el control</param>
    public static void lostFocusMit2(object sender, System.Windows.RoutedEventArgs e)
    {
        System.Windows.Controls.Grid border = null;
        if (sender.GetType() == typeof(System.Windows.Controls.TextBox))
            border = (((System.Windows.Controls.Border)((System.Windows.Controls.TextBox)sender).Parent)).Parent as System.Windows.Controls.Grid;
        else if (sender.GetType() == typeof(System.Windows.Controls.ComboBox))
            border = (((System.Windows.Controls.Border)((System.Windows.Controls.ComboBox)sender).Parent)).Parent as System.Windows.Controls.Grid;
        else if (sender.GetType() == typeof(System.Windows.Controls.PasswordBox))
            border = (((System.Windows.Controls.Border)((System.Windows.Controls.PasswordBox)sender).Parent)).Parent as System.Windows.Controls.Grid;
        border.Effect = null;
    }

    public static void onlyNumbers_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key >= Key.D0 && e.Key <= Key.D9)
            e.Handled = false;
        else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            e.Handled = false;
        else if (e.Key == Key.Escape || e.Key == Key.Return || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Tab || e.Key == Key.Delete)
            e.Handled = false;
        else
            e.Handled = true;
    }
    public static void onlyNumbers_PreviewKeyDownImporte(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key >= Key.D0 && e.Key <= Key.D9)
            e.Handled = false;
        else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            e.Handled = false;
        else if (e.Key == Key.Escape || e.Key == Key.Return || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Tab || e.Key == Key.Delete)
            e.Handled = false;
        else if (e.Key == Key.OemPeriod)
            e.Handled = false;
        else if (e.Key == Key.OemSemicolon)
            e.Handled = false;

        else
            e.Handled = true;
    }
    public static void sendMailFirmaPanel_GridMouseDown(object sender, MouseButtonEventArgs e)
    {
        string strMail = string.Empty;
        string strError = string.Empty;
        int intentos = 0;

        strMail = Globales.cpIntegraEMV.ObtieneFormMail("-1", "");

        if (!string.IsNullOrWhiteSpace(strMail))
        {
            while (intentos <= 3)
            {
                if (intentos == 3)
                {
                    strError = Globales.GetDataXml("nb_error", Globales.cpIntegraEMV.getRspXML());
                    Globales.MessageBoxMit(string.Format("No se pudo enviar el voucher. \n{0}", strError));
                    break;
                }

                bool envio = Globales.cpIntegraEMV.sndEnviaMailFirmaPanel(strMail,
                Globales.cpIntegraEMV.getRspOperationNumber(),
                Globales.cpIntegraEMV.dbgGetId_Company(),
                Globales.cpIntegraEMV.dbgGetId_Branch(),
                Globales.cpIntegraEMV.dbgGetCountry(),
                Globales.cpIntegraEMV.dbgGetUser(),
                Globales.cpIntegraEMV.dbgGetPass(), Globales.ipFirmaPanel);
                if (envio)
                {
                    Globales.cpIntegraEMV.ObtieneFormMail("0", strMail);
                    break;
                }
                else
                    intentos++;
            }
        }
    }
    public static void sendMail_GridMouseDown(object sender, MouseButtonEventArgs e)
    {
        string pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        string strDireccion = string.Empty;
        string strCadEncriptar = string.Empty;
        string strVoucherAux = string.Empty;
        string strNombre = ((System.Windows.Controls.Grid)sender).Tag.ToString();


        DialogResult answer = InputBox.InputBoxQ("Envío de e-mail.", "Introduzca correo electrónico del cliente", ref strDireccion);
        if (answer == DialogResult.OK || answer == DialogResult.Yes)
        {
            Regex reg = new Regex(pattern);
            if (reg.IsMatch(strDireccion))
            {
                if (!TypeUsuario.strVoucherCoP.Contains('@'))
                    strVoucherAux = DecryptString(TypeUsuario.strVoucherCoP, KEY_RC4_CP);
                else
                    strVoucherAux = TypeUsuario.strVoucherCoP;

                Globales.cpHTTP_Clear();
                Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;

                if (!string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.getRspOperationNumber()))
                    strCadEncriptar = string.Format("&direccion={0}&nombrecliente={1}&numoper={2}&empresa={3}&sucursal={4}&op=enviacorreo",
                     strDireccion, strNombre,
                     cpIntegraEMV.getRspOperationNumber(),
                     TypeUsuario.Id_Company,
                     TypeUsuario.Id_Branch);
                else
                {
                    cpIntegracion_sResult = cpIntegracion_sResult.Replace("&lt;", "<");
                    strCadEncriptar = string.Format("&direccion={0}&nombrecliente={1}&numoper={2}&empresa={3}&sucursal={4}&op=enviacorreo",
                      strDireccion, strNombre,
                      GetDataXml("foliocpagos", cpIntegracion_sResult),
                      TypeUsuario.Id_Company,
                      TypeUsuario.Id_Branch);
                }

                cpHTTP_cadena1 = string.Format("enc={0}", encryptString(strCadEncriptar, KEY_RC4, true));
                if (cpHTTP_SendcpCUCT())
                    Globales.MessageBoxMit(GetDataXml("desc", cpHTTP_sResult));
            }
            else
                Globales.MessageBoxMit("Correo inválido.");
        }
    }

    public static void sendMailFirmaPanel_MouseDown(object sender, MouseButtonEventArgs e)
    {
        try
        {
            bool bandera = false;
            string strMail = Globales.cpIntegraEMV.ObtieneFormMail("-1", "");
            if (!string.IsNullOrWhiteSpace(strMail))
            {
                Globales.cpIntegraEMV.sndEnviaMailFirmaPanel(strMail, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.dbgGetId_Company(),
                    Globales.cpIntegraEMV.dbgGetId_Branch(), Globales.cpIntegraEMV.dbgGetCountry(), Globales.cpIntegraEMV.dbgGetUser(), Globales.cpIntegraEMV.dbgGetPass(), Globales.ipFirmaPanel);
                for (int x = 0; x < 2; x++)
                {
                    var valor = Globales.cpIntegraEMV.getRspXML();
                    if (Globales.GetDataXml("response", Globales.cpIntegraEMV.getRspXML()).ToLower() == "true")
                    {
                        Globales.cpIntegraEMV.ObtieneFormMail("0", strMail);
                        bandera = true;
                        break;
                    }
                }
                if (!bandera)
                {
                    string strError = Globales.GetDataXml("nb_error", Globales.cpIntegraEMV.getRspXML());
                    Globales.MessageBoxMit("No se pudo enviar el voucher\n");
                }
                bandera = false;
            }
            else
            {
                Globales.MessageBoxMit("No se pudo enviar el correo eléctronico\nCorreo electrónico vacio..");
            }
        }
        catch
        {
            Globales.MessageBoxMit("Error al mandar el correo electrónico..");
        }
    }

    #endregion

    public static string ArchivoServidor
    {
        get
        {
            return modMIT.ArchivoServidor;
        }
        set
        {
            modMIT.ArchivoServidor = value;
        }
    }
    public static string ArchivoInstalador
    {
        get
        {
            return modMIT.ArchivoInstalador;
        }
        set
        {
            modMIT.ArchivoInstalador = value;
        }
    }
    public static string ArchivoActualizar
    {
        get
        {
            return modMIT.ArchivoActualizar;
        }
        set
        {
            modMIT.ArchivoActualizar = value;
        }

    }
    internal static string TipoUpdate(string versionactual, string strversion)
    {
        return modMIT.TipoUpdate(versionactual, strversion);
    }
    public static int TA
    {
        get
        {
            return modMIT.TA;
        }
        set
        {
            modMIT.TA = value;
        }
    }
    public static bool desvinculaInstaladorLector
    {
        get
        {
            return modMIT.desvincularInstaladorLector;
        }
    }
    internal static void botonSalir(bool lector = false)
    {
        Mouse.OverrideCursor = null;
        Globales.principal.ventanaDisplay.Content = null;
        if (lector) Globales.cpIntegraEMV.dbgCancelOperation();
    }
    public static bool isVentanaQualitas { get; set; }

    public static bool isCuponTelefonico { get; set; }

    internal static string importe(string importe)
    {
        double valor = 0;
        if (double.TryParse(importe, out valor))
        {
            int aux = 0;
            if (int.TryParse(importe, out aux))
            {
                if (aux > 0)
                {
                    importe = aux + ".00";
                }
                else
                {
                    importe = "0.00";
                }
            }
            else
            {
                if (valor <= 0)
                {
                    importe = "0.00";
                }
                else
                {
                    double valor1 = Math.Round(valor, 2);
                    importe = valor1.ToString();
                    if (importe.Contains(".00"))
                    {
                        importe = importe + ".00";
                    }
                }

            }
        }
        else
        {
            importe = "0.00";
        }

        if (!importe.Contains("."))
        {
            importe += ".00";
        }
        else
        {
            int indice = importe.IndexOf('.');
            string cadena = importe.Substring(indice + 1);
            importe += (cadena.Length < 2) ? "0" : "";
        }

        if (importe == "0.00")
        {
            importe = "";
        }
        return importe;
    }

    public static string URL_REPORTECYC
    {
        get
        {
            return modMIT.URL_REPORTECYC;
        }
        set
        {
            modMIT.URL_REPORTECYC = value;
        }
    }
    public static string strXML
    {
        get
        {
            return PcPay.Code.Configuracion.cpIntegracion.strXML;
        }
        set
        {
            PcPay.Code.Configuracion.cpIntegracion.strXML = value;
        }
    }

    internal static bool cpintegracion_cpReporte(string cadena, string url)
    {
        cpIntegracion_Clear();
        bool valor = false;
        cpIntegracion_sURL_cpINTEGRA = url;
        strXML = cadena;
        valor = PcPay.Code.Configuracion.cpIntegracion.cpIntegracion_EnviarcpINTEGRA();
        return valor;
    }

    public static string userSantanderVta
    {
        get
        {
            return modMIT.userSantanderVta;
        }
    }

    internal static string KEY_RC4_CP_COM(string p)
    {
        return modMIT.KEY_RC4_CP_COM(p);
    }


    public static string referenciaAux = string.Empty;

    internal static string digitoVF(string strContrato)
    {
        string valor = string.Empty;
        try
        {
            string contenedora = string.Empty;
            string aux = strContrato.Substring(0, 10);
            int k = 1;
            int operacion1 = 0, operacion2;
            foreach (var x in aux)
            {
                string temporal = string.Empty;
                if (k % 2 == 1)
                {
                    string x2 = System.Text.Encoding.ASCII.GetString(new byte[] { Convert.ToByte(x) });
                    temporal = Convert.ToString(Convert.ToInt32(x2) * 1);

                }
                else
                {
                    string x2 = System.Text.Encoding.ASCII.GetString(new byte[] { Convert.ToByte(x) });
                    temporal = Convert.ToString(Convert.ToInt32(x2) * 2);
                }
                if (temporal.Length == 2)
                {
                    operacion2 = Convert.ToInt32(temporal.Substring(0, 1)) + Convert.ToInt32(temporal.Substring(1, 1));
                }
                else
                {
                    operacion2 = Convert.ToInt32(temporal);
                }
                operacion1 += operacion2;
                k++;
            }
            string strOperacion1 = Convert.ToString(operacion1);
            int v1 = 10 - Convert.ToInt32(strOperacion1.Substring(strOperacion1.Length - 1, 1));
            valor = (Convert.ToString(v1)).Substring((Convert.ToString(v1)).Length - 1, 1);

        }
        catch
        {
            valor = "";
        }
        return valor;
    }


    public const string KTGAPPWEB = "#<?V>E=ZqRmW*:9%rH7";

    public static bool mostrar { get; set; }

    public static bool saltar { get; set; }

    public static bool logeo { get; set; }

    public static string answerLoginUser { get; set; }

    public static bool esSantander { get; set; }

    public static bool sinImagen { get; set; }

    internal static bool verificadorBoleto(string p)
    {
       
        return modMIT.verificadorBoleto(p);
    }

    public static bool noMostrarMensajes { get; set; }

    internal static string VoucherHtml1(string p)
    {
        return p.Substring(1, p.IndexOf("<td width=" + Convert.ToChar(34) + "80" + Convert.ToChar(34) + ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") - 62) + "</td></tr></table><!----></td><td width='80'></td><td><img border='0' src='" + @"C:\Windows\promo.bmp" + "'/></td></body></html>";
    }
    public static Font printFont{
       get{
           return modMIT.printFont;
       }
        set {
            modMIT.printFont = value;
        }
    }
    public static StreamReader streamToPrint {
        get {
            return modMIT.streamToPrint;
        }
        set {
            modMIT.streamToPrint = value;
        }
    }
    public static System.Windows.Forms.PrintDialog printDialog1 {
        get {
            return modMIT.printDialog1;
        }
        set {
            modMIT.printDialog1 = value;
        }
    }
    public static PrintDocument printDocument1 {
        get {
            return modMIT.printDocument1;
        }
        set {
            modMIT.printDocument1 = value;
        }
    }
    private static string mensaje = string.Empty;
    private static float linesPerPage = 0;
    private static float yPos = 0;
    private static  int count = 0;
    private static  int leftMargin = 0;
    private static float topMargin = 0;
    private static string line = null;
    private static bool prueba = false;
    internal static void epsonImprimir(string mensaje, float leftMargin,bool prueba = false)
    {
        Globales.mensaje = mensaje;
        Globales.prueba = prueba;
        var aux = Globales.GetSettingString("leftPrinter");
        Globales.leftMargin = Convert.ToInt32(aux);
        Globales.printDialog1 = new PrintDialog();
        Globales.printDocument1 = new PrintDocument();
        if (Globales.printDialog1.Document == null)
        {
              Globales.printDialog1.Document = Globales.printDocument1;
        }
        
        printFont = new Font("Arial", 8);

        PrintController printController = new StandardPrintController();
        printDocument1.PrintController = printController;
        printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(imprimiendoPage);
        printDocument1.Print();
    }

    private static void imprimiendoPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
    {
        
        linesPerPage = e.MarginBounds.Height /
        printFont.GetHeight(e.Graphics);
        yPos = topMargin + (count * printFont.GetHeight(e.Graphics));
        int justificar = cadenaGrande;
        StringFormat sf = new StringFormat();
        if(Globales.prueba){
            e.Graphics.DrawString(Globales.mensaje, printFont, Brushes.Black,
            Globales.leftMargin, 0, new StringFormat());
            return;
        }

        sf.Alignment = StringAlignment.Center;
        int medida = (justificar * 2)+leftMargin;
        string[] aux = mensaje.Split('@');
        int contador = 0;
        string importe = string.Empty;
        int auxContador = 0;

        foreach(string item in aux){
            try
            {
                if ((aux[auxContador].Contains("br")))
                {
                    if (!aux[auxContador + 1].Contains("br") && !aux[auxContador + 1].ToUpper().Contains("TARJETA"))
                    {
                        aux[auxContador] = "";
                    }

                }

            }
            catch
            {

            }
            auxContador++;
        }
        
        foreach(string item in aux){
           
          if(!string.IsNullOrWhiteSpace(item)){
            string item2 = "@" + item;
            string[] t2 = item2.Split(',');

            if (item2.Contains(",") && !(item2.ToUpper().Contains("IMPORTE") || item.Contains("$") || item.Contains("€")))
            {
               
                for (int x = 0; x < t2.Length-1; x++ )
                {
                    
                    t2[x] = t2[x].Replace("@lnn", "");
                    t2[x] = t2[x].Replace("@cnb", "");
                    t2[x] = t2[x].Replace("@cnn", "");
                    t2[x] = t2[x].Replace("@bc", "");
                   
                    e.Graphics.DrawString(t2[x], printFont, Brushes.Black,
          medida + 5, contador, sf);
                    contador += 16;
                }
                item2 = t2.Last();
            }
            item2 = item2.Trim();
            if(item2.ToUpper().Contains("TARJETA")){
                sf.Alignment = StringAlignment.Near;
                medida = leftMargin;
            }

            if (item2.ToUpper().Contains("FECHA"))
            {
                fechita = true;
            }
            
            if (item2.Contains("@cnb"))
            {
                printFont = new Font("Arial",8, FontStyle.Bold);
                
            }
            else {
                printFont = new Font("Arial", 8, FontStyle.Regular);
            }
            item2 = item2.Replace("@cnb", "");
            item2 = item2.Replace("@cnn", "");
            item2 = item2.Replace("@lnn", "");
            item2 = item2.Replace("@br", "");
            item2 = item2.Replace("@bc", "");
            if(item2.ToUpper().Contains("IMPORTE")){
                e.Graphics.DrawString("IMPORTE", printFont, Brushes.Black,
           medida+5 , contador, sf);
                contador += 16;
                printFont = new Font("Arial", 8, FontStyle.Bold);
                 item2 = item2.ToUpper().Substring(item2.ToUpper().IndexOf("IMPORTE")+"importe".Length);
            }
            bool otravez = false;
            if (item2.Contains("-C-O-M-E-R-C-I-O") || item2.Contains("-C-L-I-E-N-T-E") || item2.Contains("$"))
            {
                sf.Alignment = StringAlignment.Center;
                medida = leftMargin + (justificar * 2);
                otravez = true;
            }
            e.Graphics.DrawString(item2, printFont, Brushes.Black,
            medida, contador, sf);
            contador += 16;
            if(fechita)
            {
                sf.Alignment = StringAlignment.Center;
                medida = leftMargin + (justificar * 2);
                fechita = false;
            }
            if (otravez)
            {
                sf.Alignment = StringAlignment.Near;
                medida = leftMargin;
                otravez = false;
            }
          }
        }
 
    }

    internal static void imprimirEpson()
    {
        string auxVoucher = string.Empty;
        string leyend = string.Empty;
        if(!TypeUsuario.strVoucherCoP.Contains(":")){
            TypeUsuario.strVoucherCoP = Globales.DecryptString(TypeUsuario.strVoucherCoP,Globales.KEY_RC4_CP,true);
        }
        if (!TypeUsuario.strVoucherCoP.Contains("POR MEDIO DE ESTE PAGARE ME OBLIGO INCONDI"))
        {
            leyend = string.Empty;
        }
        else {
            leyend = Globales.leyendaVoucher;
            TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("@ls","");
            TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Substring(0, TypeUsuario.strVoucherCoP.IndexOf("POR MEDIO DE ESTE PAGARE ME OBLIGO INCONDI"));
        }
        Mouse.OverrideCursor = null;
        Globales.MessageBoxMit("Verifique que la impresora esté lista y preparada para imprimir.");
        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
        if (string.IsNullOrWhiteSpace(TypeUsuario.strSoloUnaHoja))
        {
            TypeUsuario.strSoloUnaHoja = Globales.GetSettingString("HOJA");
        }
        string temp1 = TypeUsuario.strVoucherCoP;
        for (int x = 1; x <= 2; x++ )
        {
            TypeUsuario.strVoucherCoP = temp1;
            if (TypeUsuario.strSoloUnaHoja == "1")
            {
                Mouse.OverrideCursor = null;
                Globales.MessageBoxMit("Introduzca la hoja.");
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            }
            else {
                if (x == 1)
                {
                    Mouse.OverrideCursor = null;
                    Globales.MessageBoxMit("Introduzca la hoja que se quedará en el comercio.");
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    TypeUsuario.strVoucherCoP += "***** C O M E R C I O *****" + "\n" + leyend;
                }
                else {
                    Mouse.OverrideCursor = null;
                    Globales.MessageBoxMit("Introduzca la hoja que se dará al cliente.");
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    TypeUsuario.strVoucherCoP += "***** C L I E N T E *****"+"\n"+leyend;
                }
            }


            TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("<voucher_comercio>","");
            TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("</voucher_comercio>", "");
            TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("<voucher_cliente>","");
            TypeUsuario.strVoucherCoP = TypeUsuario.strVoucherCoP.Replace("</voucher_cliente>", "");

            Globales.arregloSplit = TypeUsuario.strVoucherCoP.Split('@');
            int temp = 0;
            foreach (var item in arregloSplit)
            {
                if (temp < item.Length)
                {
                    temp = item.Length;
                }
            }
            Globales.cadenaGrande = temp;
            
            Globales.epsonImprimir(TypeUsuario.strVoucherCoP, 0);

            if (TypeUsuario.strSoloUnaHoja == "1") break;
            
        }

    }

    public static string leyendaVoucher {
        get {
            return modMIT.leyendaVoucher;
        }

        set {
            modMIT.leyendaVoucher = value;
        }
    }

    public static string[] arregloSplit { get; set; }

    public static int cadenaGrande { get; set; }

    public static bool fechita { get; set; }
}

