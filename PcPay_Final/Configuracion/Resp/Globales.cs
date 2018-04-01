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
using System.Windows;
using System.Windows.Media;

class Globales
{
    //************************************************************
    //**            DEFINICION DE VARIABLES                    ***
    //************************************************************
    public static List<Key> NUMBERKEYS
    {
        get
        {
            return new Key[] { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9 }.ToList();
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

    public static cpIntegracionEMV.clsPrePagoTrx PPOperacion
    {
        get
        {
            return modMIT.PPOperacion;
        }
        set
        {
            modMIT.PPOperacion = value;
        }
    }
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
    public static cpIntegracionEMV.clsServicios TiempoAire
    {
        get
        {
            return modMIT.TiempoAire;
        }
        set
        {
            modMIT.TiempoAire = value;
        }
    }
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
        return System.Windows.Forms.MessageBox.Show(p1, p2, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

    internal static string EstableceLector()
    {
        return modMIT.EstableceLector();
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

    public static bool isAereolinea
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

    internal static System.Windows.Forms.DialogResult mensajeAlerta(string p1, string p2 = "")
    {
        return System.Windows.Forms.MessageBox.Show(p1, p2, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
    }
    internal static void MessageBoxMit(string message, Window owner)
    {
        new PcPay.Forms.Formularios.MessagesW.frmMessageBox(message, owner).ShowDialog();
    }
    internal static void MessageBoxMitApproved(string message, Window owner, bool isCancel = false)
    {
        new PcPay.Forms.Formularios.MessagesW.frmVtaAprobada(message, owner, isCancel).ShowDialog();
    }
    internal static void MessageBoxMitDenied(string message, Window owner)
    {
        new PcPay.Forms.Formularios.MessagesW.frmVtaDenegada(message, owner).ShowDialog();
    }
    internal static void MessageBoxMitError(string message, Window owner)
    {
        new PcPay.Forms.Formularios.MessagesW.frmVtaError(message, owner).ShowDialog();
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

    internal static void Imprimir_HTML(string p)
    {
        modMIT.Imprimir_HTML(p);
    }

    public static string KEY_RC4
    {
        get
        {
            return modMIT.KEY_RC4;
        }
    }

    internal static void PrintOptions(string Voucher, string NumOpe = "", PrintDialog Impresora = null)
    {
        modMIT.PrintOptions(Voucher, NumOpe, Impresora);
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
                else if (cpIntegraEMV.chkPp_Trademark.Trim() == "INGENICO" && cpIntegraEMV.chkPp_Printer == "1")
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

    public static bool vacio(string cadena)
    {
        return string.IsNullOrEmpty(cadena);
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


    public static pcpay.clsCpIntegracionEMV CpCobro
    {
        get
        {
            return modMIT.CpCobro;
        }
    }
    public static string strHexFirmaPanel { get; set; }

    public static bool ObtieneFirmaPanel(MITEncrypt.cpIntegracionFirmaPanel dllFirma, string urlFirma, string textoAgua, short tipoVta, bool IsChipAndPin, bool esQPS)
    {
        bool answer = false;
        strHexFirmaPanel = string.Empty;

        if (dllFirma.EsTouch())
        {
            if (!(IsChipAndPin && tipoVta == 1) || esQPS)
            {
                dllFirma.ObtieneFirmaPanel(textoAgua);
                if (string.IsNullOrWhiteSpace(dllFirma.Error))
                    strHexFirmaPanel = dllFirma.TextoHEXFirmaPanel;
                else
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("No se pudo obtener la firma desde el dispositivo \n{0}", dllFirma.Error));
                    return answer;
                }
            }

            string val1 = cpIntegraEMV.getRspOperationNumber();
            string val2 = cpIntegraEMV.chkPp_Serial;
            string val3 = cpIntegraEMV.dbgGetId_Company();
            string val4 = cpIntegraEMV.dbgGetId_Branch();
            string val5 = cpIntegraEMV.dbgGetCountry();
            string val6 = cpIntegraEMV.dbgGetUser();
            string val7 = cpIntegraEMV.dbgGetPass();
            string val8 = cpIntegraEMV.getRspVoucher();
            string val9 = cpIntegraEMV.chkPp_Trademark;


            if (cpIntegraEMV.sndFirmaEnPanel(true, strHexFirmaPanel, urlFirma, textoAgua,
                val1, val2, val3, val4, val5, val6, val7, val8, IsChipAndPin, val9, tipoVta))
                answer = true;
        }
        else
        {
            if (cpIntegraEMV.getRspSoportaFirmaPanel())
            {
                if (cpIntegraEMV.sndFirmaEnPanel(true, string.Empty, urlFirma, textoAgua, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial,
                                       Globales.cpIntegraEMV.dbgGetId_Company(), Globales.cpIntegraEMV.dbgGetId_Branch(), Globales.cpIntegraEMV.dbgGetCountry(), cpIntegraEMV.dbgGetUser(), Globales.cpIntegraEMV.dbgGetPass(),
                                       Globales.cpIntegraEMV.getRspVoucher(), IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark, tipoVta))
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
    public static MITEncrypt.cpIntegracionFirmaPanel dllFirmaPanel
    {
        get
        {
            return modMIT.dllFirmaPanel;
        }
        set
        {
            modMIT.dllFirmaPanel = value;
        }
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



    internal static void cierraFormas()
    {
        //throw new NotImplementedException();

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
                strCadaux = GetDataXml("response", cpHTTP_sResult).ToLower();
                if (!vacio(strCadaux))
                {
                    strCadaux = GetDataXml("desc", cpHTTP_sResult);
                    if (!vacio(strCadaux))
                    {
                        // MsgBoxEx strCadaux, , , , vbExclamation, NOMBRE_APP
                        mensajeAlerta(strCadaux, NOMBRE_APP);
                    }
                    insertaBoletoAgencia = true;
                }
                else
                {
                    strCadaux = GetDataXml("desc", cpHTTP_sResult);
                    if (!vacio(strCadaux))
                    {
                        mensajeAlerta(strCadaux, NOMBRE_APP);
                    }
                    insertaBoletoAgencia = false;
                }
            }

        }
        catch (Exception Err)
        {

            //  MsgBoxEx strNombreFP & vbCrLf & "Error: " & Err.Number & vbCrLf & "Descripcion: " & Err.Description, , , , vbCritical, NOMBRE_APP
            mensajeAlerta("Descripcion:" + Err.Message);
        }
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
            if (!vacio(f_retorno))
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
            if (cpHTTP_SendcpCUCT())
            {
                strCadaux = GetDataXml("cd_status", cpHTTP_sResult).ToLower();
                if (!vacio(strCadaux))
                {


                    modMIT.TGate.dbgSetTGURL(URL_3GATE);
                    string fecha1 = DateTime.Now.Date.ToString("DDMMyyyy", CultureInfo.InvariantCulture);
                    string fecha2 = DateTime.Now.Date.ToString("DD/MM/yyyy", CultureInfo.InvariantCulture);
                    // TGate.snd3GateInsertReference .usu, .Pass, Empresa, res, Format(Now, "DDMMYYYY"), Importe, Format(Now, "DD/MM/YYYY")
                    modMIT.TGate.snd3GateInsertReference(TypeUsuario.usu, TypeUsuario.Pass, Empresa, res, fecha1, Importe, fecha2);

                    mensajeAlerta("Reservación guardada con éxito.");
                    insertaReservacion = true;
                }
                else
                {
                    strCadaux = GetDataXml("nb_error", cpHTTP_sResult);
                    if (!Globales.vacio(strCadaux))
                    {
                        mensajeAlerta(strCadaux);
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
        numBoletoCompleto = Convert.ToDouble(cveAerolinea + numBoleto);
        if (numBoleto.Length == 10)
        {
            numBoletoCompleto = Convert.ToDouble(cveAerolinea + numBoleto);
        }
        else
        {
            numBoletoCompleto = Convert.ToDouble(cveAerolinea + Utils.Mid(numBoleto, 1, 10));
        }

        if (!(numBoletoCompleto / 7).ToString().Contains("."))
        {
            DV = numBoletoCompleto / 7;
        }
        else
        {
            string aux = Convert.ToString(numBoletoCompleto / 7);
            int posicion = Convert.ToInt32(aux.IndexOf("."));
            DV = Convert.ToDouble(Utils.Mid(aux, 1, posicion));
        }

        mult = DV * 7;
        digv = mult - numBoletoCompleto;
        digv = digv * -1;
        if (numBoleto.Length == 10)
        {
            DigitoVerificador = Convert.ToInt32(digv);
        }
        else
        {
            if (digv == Convert.ToDouble(Utils.Mid(numBoleto, 11, 1)))
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
                if (!Globales.vacio(strCadaux))
                {
                    strCadaux = Globales.GetDataXml("desc", Globales.cpHTTP_sResult);
                    if (!Globales.vacio(strCadaux))
                    {
                        Globales.mensajeAlerta(strCadaux, Globales.NOMBRE_APP);
                    }
                    insertaBoleto = true;
                }
                else
                {
                    strCadaux = Globales.GetDataXml("desc", Globales.cpHTTP_sResult);
                    if (!Globales.vacio(strCadaux))
                    {
                        Globales.mensajeAlerta(strCadaux, Globales.NOMBRE_APP);
                    }
                    insertaBoleto = false;
                }
            }



        }
        catch (Exception Err)
        {
            Globales.mensajeAlerta(strNombreFP + "\n" + "Descripcion: " + Err.Message, Globales.NOMBRE_APP);

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
        Globales.mensajeAlerta("Falta printOptionPagueDirecto");
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
                    double valor1 = Math.Round(valor, 2);
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

    public static void setFocusMit(object sender, RoutedEventArgs e)
    {
        System.Windows.Controls.Border border = null;
        System.Windows.Media.Effects.DropShadowEffect dropShadowEffect = new System.Windows.Media.Effects.DropShadowEffect();
        dropShadowEffect.Opacity = 1;
        dropShadowEffect.ShadowDepth = 1;
        dropShadowEffect.BlurRadius = 8;
        dropShadowEffect.Color = Colors.Firebrick;
        dropShadowEffect.Direction = 0;

        if (sender.GetType() == typeof(System.Windows.Controls.TextBox))
            border = (((System.Windows.Controls.Grid)((System.Windows.Controls.TextBox)sender).Parent)).Parent as System.Windows.Controls.Border;
        else if (sender.GetType() == typeof(System.Windows.Controls.ComboBox))
            border = (((System.Windows.Controls.Grid)((System.Windows.Controls.ComboBox)sender).Parent)).Parent as System.Windows.Controls.Border;
        else if (sender.GetType() == typeof(System.Windows.Controls.PasswordBox))
            border = (((System.Windows.Controls.Grid)((System.Windows.Controls.PasswordBox)sender).Parent)).Parent as System.Windows.Controls.Border;
        border.Effect = dropShadowEffect;
    }
    public static void lostFocusMit(object sender, RoutedEventArgs e)
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
}

