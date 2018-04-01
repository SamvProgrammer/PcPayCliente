using PcPay.Code.Configuracion;
using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using PcPay.Forms.Formularios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PcPay.Forms.Pages
{
    /// <summary>
    /// Lógica de interacción para frmCheckIn.xaml
    /// </summary>
    public partial class frmCheckIn : Page
    {
        public string numeroTarjeta;
        public string _mes { get; set; }
        public string _anio { get; set; }
        public Window propietario { get; set; }
        public string Voucher { get; set; }
        private const string NOMBRE_GENERAL = "frmBandaAgencia";
        private string strNoEconomico { set; get; }
        private string strNoServicio { set; get; }
        private string strProveedor { set; get; }
        private bool StatusCmd { set; get; }
        public abrirFrm abrir;
        public cerrarVentana cerrar;
        public frmCheckIn()
        {
            InitializeComponent();
            Carga();
            Globales.csvAmexenBanda = string.Empty;

            this.NumCuarto.ToolTip = "Introduzca el Dato Adicional.";
            this.Importe.ToolTip = Globales.MsjImporte;
            this.NumOrden.ToolTip = Globales.MsjReferencia;
            this.CmdLeer.ToolTip = "Haga clic en el botón Activar Lector e inserte chip o deslice la tarjeta";


            this.NumOrden.GotFocus += Globales.setFocusMit2;
            this.NumCuarto.GotFocus += Globales.setFocusMit2;
            this.Importe.GotFocus += Globales.setFocusMit2;
            this.NumTdc.GotFocus += Globales.setFocusMit2;
            this.TFECHA.GotFocus += Globales.setFocusMit2;
            this.NomTdc.GotFocus += Globales.setFocusMit2;

            this.NumOrden.LostFocus += Globales.lostFocusMit2;
            this.NumCuarto.LostFocus += Globales.lostFocusMit2;
            this.Importe.LostFocus += Globales.lostFocusMit2;
            this.NumTdc.LostFocus += Globales.lostFocusMit2;
            this.TFECHA.LostFocus += Globales.lostFocusMit2;
            this.NomTdc.LostFocus += Globales.lostFocusMit2;
            Importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;

            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;
            this.BENVIAMAIL.MouseDown += Globales.sendMail_GridMouseDown;
            this.imgEmailFirmaPanel.MouseDown += Globales.sendMailFirmaPanel_GridMouseDown;
        }


        private void Carga()
        {
            lblMoneda.Content = "";
            Globales.strNombreFP = NOMBRE_GENERAL + ".Form_Load()";
            limpia();
            StatusCmd = true;
            if (Globales.bolActivaMotoP)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                limpia();
                Mouse.OverrideCursor = null;
                CmdLeer.Visibility = Visibility.Visible;
                CmdEnviar.Visibility = Visibility.Hidden;

            }
         }
        private void cargandoFormulario(object sender, RoutedEventArgs e)
        {
            if (TypeUsuario.ShowMsg)
            {
                frmAvisoBanda frm = new frmAvisoBanda();
                frm.Owner = this.propietario;
                frm.ShowDialog();

            }
            imgEmailFirmaPanel.Visibility = Visibility.Hidden;
        }
        private void CmdEnviar_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdEnviar_Click()";
            CmdEnviar.IsEnabled = false;
            if (string.IsNullOrWhiteSpace(NumTdc.Text) || string.IsNullOrWhiteSpace(this._mes) || string.IsNullOrWhiteSpace(this._anio))
            {
                Globales.MessageBoxMit("Deslice la tarjeta bancaria.");
                CmdEnviar.IsEnabled = true;
            }
            else if (string.IsNullOrWhiteSpace(NumOrden.Text))
            {
                Globales.MessageBoxMit("Introduzca " + TypeUsuario.reference + ".");
                NumOrden.Focus();
                CmdEnviar.IsEnabled = true;
            }
            else if (string.IsNullOrWhiteSpace(NumCuarto.Text))
            {
                Globales.MessageBoxMit("Introduzca el Dato Adicional. ");
                NumCuarto.Focus();
                CmdEnviar.IsEnabled = true;
            }
            else if (string.IsNullOrWhiteSpace(Importe.Text))
            {
                Globales.MessageBoxMit("Introduzca el importe.");
                Importe.Focus();
                CmdEnviar.IsEnabled = true;
            }
            else if ((Utils.Mid(Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML), 1, 1) == "1") && (Globales.GetDataXml("tarjeta", Globales.cpHTTP_sResult) != NumTdc.Text))
            {
                Globales.MessageBoxMit("La Tarjeta Deslizada no corresponde con el Servicio a cobrar");
                CmdEnviar.IsEnabled = true;
            }

            else if (TypeUsuario.Id_Company == "0059")
            {
                if (NumOrden.Text.Length != 28)
                {
                    Globales.MessageBoxMit("El no. de REFERENCIA debe ser de 28 posiciones");
                    NumOrden.Focus();
                    CmdEnviar.IsEnabled = true;
                    return;
                }
            }

            if (!Globales.IsNumeric(Importe.Text))
            {
                Globales.MessageBoxMit("El importe debe ser numérico.");
                Importe.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(Globales.merchantBanda))
            {
                Globales.MessageBoxMit("No hay planes de pago para esta tarjeta, por favor cambie la tarjeta.");
                CmdLeer.Visibility = Visibility.Visible;
                CmdLeer.IsEnabled = true;
                CmdEnviar.IsEnabled = false;
                CmdEnviar.Visibility = Visibility.Hidden;
                CmdNuevo.Visibility = Visibility.Hidden;
                CmdNuevo.IsEnabled = false;
                Importe.IsEnabled = true;
                NumOrden.IsEnabled = true;
                NumCuarto.IsEnabled = true;
                NumOrden.IsEnabled = true;
                Importe.IsEnabled = true;
                StatusCmd = true;
                NumTdc.Text = "";
                this.TFECHA.Text = string.Empty;
                NomTdc.Text = "";
                Globales.cpIntegraEMV.dbgCancelOperation();
                return;
            }
            string strTypeC = string.Empty;
            string strCadEncriptar = string.Empty;
            Voucher = string.Empty;

            if (Globales.isAmex)
            {
                strTypeC = "AMEX";
            }
            else
            {
                strTypeC = "V/MC";
            }


            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globales.cpIntegraEMV.dbgSetCurrencyType(lblMoneda.Content.ToString());
            Globales.cpIntegraEMV.sndCheckInEMV(TypeUsuario.usu, TypeUsuario.Pass, "", TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
            TypeUsuario.country, strTypeC, Globales.merchantBanda, NumOrden.Text,
            "9", lblMoneda.Content.ToString(), NumCuarto.Text, Globales.csvAmexenBanda);
            Globales.csvAmexenBanda = "";
            string mensaje = string.Empty;
            string caso = Globales.cpIntegraEMV.getRspDsResponse();
            Mouse.OverrideCursor = null;
            switch (caso)
            {

                case "approved"://'Transacción Aprobada
                    NumOrden.IsEnabled = false;
                    Importe.IsEnabled = false;
                    NumCuarto.IsEnabled = false;
                    mensaje =  Globales.cpIntegraEMV.getRspAuth();
                    if (TypeUsuario.Id_Company == Globales.EMPREF && TypeUsuario.Id_Branch == Globales.EMPREF2)
                    {
                        frmReferencia refe = new frmReferencia();
                        refe.ShowDialog();
                        if (!refe.cancelar)
                        {
                            Globales.cpIntegraEMV.sndReimpresion(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country, Globales.cpIntegraEMV.getRspOperationNumber());
                            Mouse.OverrideCursor = null;
                        }
                    }
                    Globales.MessageBoxMitApproved(mensaje);
                    TypeUsuario.strVoucherCoP = Globales.cpIntegraEMV.getRspVoucher().Trim();
                    CmdNuevo.Visibility = Visibility.Visible;
                    CmdEnviar.Visibility = Visibility.Hidden;
                    this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;
                    this.BENVIAMAIL.Tag = this.NomTdc.Text;
                    //'************************************************************************************************
                    //        ''********************************** FIRMA EN PANEL *********************************************
                    //        'VALIDACIONES PARA LA FIRMA EN PANEL
                    string textoAgua = string.Empty;
                    textoAgua = "Folio: " + Globales.cpIntegraEMV.getRspOperationNumber() + System.Environment.NewLine;
                    textoAgua += "Auth: " + Globales.cpIntegraEMV.getRspAuth() + System.Environment.NewLine;
                    textoAgua += "Importe: " + Globales.cpIntegraEMV.getTx_Amount() + System.Environment.NewLine;
                    textoAgua += "Fecha: " + Globales.cpIntegraEMV.getRspDate() + System.Environment.NewLine;
                    textoAgua += "Merchant: " + Globales.cpIntegraEMV.getRspDsMerchant() + System.Environment.NewLine;
                    textoAgua += " " + System.Environment.NewLine;
                    textoAgua += "___________________" + System.Environment.NewLine;
                    textoAgua += "FIRMA DIGITALIZADA:" + System.Environment.NewLine;
                    //'************************************************************************************************
                    //'valida si la tarjeta es Chip and Pin
                    bool IsChipAndPin = false; bool esQPS = false;
                    string cadenaHexFirma = string.Empty;
                    int tipoVta = 1;
                    //'valida si es chip and pin
                    if (Globales.cpIntegraEMV.chkCc_IsPin() == "01")
                    {
                        IsChipAndPin = true;
                    }

                    //'************************************************************************************************
                    //'valida si la venta es QPS
                    if (Globales.cpIntegraEMV.getRspVoucher().Contains("@cnn Autorizado sin Firma ") && tipoVta == 1 && !IsChipAndPin)
                    {
                        esQPS = true;
                    }

                    //'************************************************************************************************
                    // 'Si la PinPad no soporta Firma en Panel y no es touch, sigue el flujo normal de PcPay
                    if (!Globales.cpIntegraEMV.EsTouch() && !Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                    {
                        goto GoImpresion;
                    }

                    //'************************************************************************************************
                    //'Si la PinPad Soporta Firma en Panel y no es touch,
                    if (!Globales.cpIntegraEMV.EsTouch() && Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                    {

                        //'Llama a la función de obtener la firma en Panel
                        cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(false, textoAgua, Globales.cpIntegraEMV.getRspVoucher(),
                                                                        IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), (short)tipoVta, esQPS);

                        if (!cadenaHexFirma.Contains("Error"))
                        {
                            // 'Llama a la funcion de Guardar la firma
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Program.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                            {
                                imgEmailFirmaPanel.Visibility = Visibility.Visible;
                            }
                            Mouse.OverrideCursor = null;
                        }
                        else
                        {
                            Globales.MessageBoxMit("No se pudo obtener la imagen del PinPad" + "\n" + cadenaHexFirma);
                        }

                        goto GoImpresion;

                    }



                    //'************************************************************************************************
                    //'SI EL DISPOSITIVO TIENE CAPACIDAD TOUCH
                    if (Globales.cpIntegraEMV.EsTouch())
                    {

                        if (TypeUsuario.UtilizarCapacidadTouch && TypeUsuario.EnviarVoucherMail)
                        {
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            if (!Globales.ObtieneFirmaPanel(Program.ipFirmaPanel, textoAgua, (short)tipoVta, IsChipAndPin, esQPS))
                            {
                                goto GoImpresion;
                            }
                            else
                            {
                                goto finaliza;
                            }
                            Mouse.OverrideCursor = null;
                        }


                        if (TypeUsuario.UtilizarCapacidadTouch && !TypeUsuario.EnviarVoucherMail)
                        {

                            // 'Llama a la función de obtener la firma en Panel
                            cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(true, textoAgua,
                                                                                Globales.cpIntegraEMV.getRspVoucher(), IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), (short)tipoVta, esQPS);

                            if (!cadenaHexFirma.Contains("Error"))
                            {
                                // 'Llama a la funcion de Guardar la firma
                                if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Program.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                {
                                    imgEmailFirmaPanel.Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                Globales.MessageBoxMit("No se pudo obtener la imagen del PinPad" + "\n" + cadenaHexFirma);
                            }

                            goto GoImpresion;

                        }


                        if (!TypeUsuario.UtilizarCapacidadTouch && !TypeUsuario.EnviarVoucherMail)
                        {

                            if (Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                            {

                                //'Llama a la función de obtener la firma en Panel
                                cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(false, textoAgua,
                                                                                    Globales.cpIntegraEMV.getRspVoucher(), IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), (short)tipoVta, esQPS);

                                if (!cadenaHexFirma.Contains("Error"))
                                {
                                    //'Llama a la funcion de Guardar la firma
                                    if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Program.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                    {
                                        imgEmailFirmaPanel.Visibility = Visibility.Visible;
                                    }
                                }
                                else
                                {

                                    Globales.MessageBoxMit("No se pudo obtener la imagen del PinPad" + "\n" + cadenaHexFirma);
                                }

                            }

                            goto GoImpresion;

                        }
                    }
                    if(imgEmailFirmaPanel.Visibility == Visibility.Hidden){
                        BENVIAMAIL.Visibility = (TypeUsuario.enviaCorreo) ? Visibility.Visible : Visibility.Hidden;
                    }
                GoImpresion:
                    Voucher = Globales.cpIntegraEMV.getRspVoucher();
                    CmdVoucher.IsEnabled = true;
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    Globales.PrintOptions(Voucher, Globales.cpIntegraEMV.getRspOperationNumber());//, Impresora
                    Mouse.OverrideCursor = null;

                finaliza:
                    break;
                case "denied":
                    if (Utils.Mid(Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML), 1, 1) == "1")
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        Globales.cpHTTP_Clear();
                        Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
                        strCadEncriptar = "&op=facileasingcobrorechazada" +
                                        "&nueconomico=" + strNoEconomico +
                                        "&nuservicio=" + strNoServicio +
                                        "&nuproveedor=" + strProveedor +
                                        "&transaccion=" + Globales.cpIntegraEMV.getRspOperationNumber() +
                                        "&importe=" + Importe +
                                        "&numtdc=" + Utils.Mid(NumTdc.Text, 13, 4) +
                                        "&auth=" + "" +
                                        "&response=" + Globales.cpIntegraEMV.getRspCdResponse();
                        Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
                        if (Globales.cpHTTP_SendcpCUCT())
                        {
                        }
                        Mouse.OverrideCursor = null;

                    }
                    Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
                    mensaje = Globales.msjRech + "\n" + Globales.cpIntegraEMV.getRspCdResponse() + " " + Globales.cpIntegraEMV.getRspFriendlyResponse();
                    Globales.MessageBoxMitDenied(mensaje);
                    CmdNuevo.Visibility = Visibility.Visible;
                    CmdEnviar.Visibility = Visibility.Hidden;
                    break;
                case "error":
                    CmdLeer.Visibility = Visibility.Hidden;
                    CmdLeer.IsEnabled = false;
                    CmdEnviar.IsEnabled = false;
                    CmdEnviar.Visibility = Visibility.Hidden;
                    mensaje = Globales.cpIntegraEMV.getRspDsError();
                    Globales.MessageBoxMitError(mensaje);
                    CmdNuevo.Visibility = Visibility.Visible;
                    break;
                default:
                    if (Globales.EMVLector == "1" && (TypeUsuario.strTipoLector == "3" || TypeUsuario.strTipoLector == "4"))
                    {
                        CmdEnviar.IsEnabled = false;
                        CmdLeer.Visibility = Visibility.Visible;
                        CmdLeer.IsEnabled = true;
                    }
                    else
                    {
                        CmdEnviar.Visibility = Visibility.Visible;
                    }
                    mensaje = "Error de conexión, verifique su reporte.";
                    Globales.MessageBoxMitError(mensaje);
                    CmdNuevo.Visibility = Visibility.Hidden;
                    break;
            }
            Mouse.OverrideCursor = null;
        }
        private void CmdLeer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NumOrden.Text))
            {
                Globales.MessageBoxMit("Ingresa Referencia");
                NumOrden.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(NumCuarto.Text))
            {
                Globales.MessageBoxMit("Ingresa Cuarto");
                NumCuarto.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(Importe.Text))
            {
                Globales.MessageBoxMit("Ingresa Importe");
                Importe.Focus();
                return;
            }

            realizaOp();

        }
        private void realizaOp()
        {
            if (string.IsNullOrWhiteSpace(NumOrden.Text))
            {
                Globales.MessageBoxMit("Introduzca" + TypeUsuario.reference + ".");
                NumOrden.Focus();
            }
            else if (string.IsNullOrWhiteSpace(Importe.Text))
            {

                Globales.MessageBoxMit("Introduzca el importe.");
                Importe.Focus();
            }
            else if (string.IsNullOrWhiteSpace(NumCuarto.Text))
            {

                Globales.MessageBoxMit("Introduzca el Dato Adicional.");
                NumCuarto.Focus();
            }
            else if (TypeUsuario.Id_Company == "0059")
            {
                if (NumOrden.Text.Length != 28)
                {

                    Globales.MessageBoxMit("El no. de REFERENCIA debe ser de 28 posiciones");
                    NumOrden.Focus();
                    return;
                }
            }
            if (!Globales.IsNumeric(Importe.Text))
            {
                Globales.MessageBoxMit("El importe debe ser numérico." + Globales.NOMBRE_APP);
                Importe.Focus();

                return;
            }
            CmdLeer.IsEnabled = false;
            FormaPago.IsEnabled = false;
            NumOrden.IsEnabled = false;
            Importe.IsEnabled = false;
            NumCuarto.IsEnabled = false;
            StatusCmd = false;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
            Globales.cpIntegraEMV.dbgStartTxEMV(Importe.Text);
            CmdLeer.IsEnabled = false;
            if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()) && !string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkCc_Number()))
            {
                NumTdc.Text = Globales.cpIntegraEMV.chkCc_Number();
                NomTdc.Text = Globales.cpIntegraEMV.chkCc_Name();
                numeroTarjeta = NomTdc.Text;


                this._mes = Globales.cpIntegraEMV.chkCc_ExpMonth();
                this._anio = Globales.cpIntegraEMV.chkCc_ExpYear();
                this.TFECHA.Text = string.Format("{0}/{1}", this._mes, this._anio);
                Globales.merchantBanda = Globales.cpIntegraEMV.dbgGetMerchantBanda("9");
                if (Globales.merchantBanda != "00000")
                {
                    Globales.isAmex = Globales.cpIntegraEMV.dbgGetisAmex();
                    FormaPago_Click();
                    CmdLeer.Visibility = Visibility.Hidden;
                    CmdEnviar.IsEnabled = true;
                    CmdEnviar.Visibility = Visibility.Visible;

                    if (!Globales.cpIntegraEMV.dbgTxMonitor())
                    {
                        Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspDsError());
                        CmdLeer.Visibility = Visibility.Visible;
                        CmdLeer.IsEnabled = true;
                        CmdEnviar.IsEnabled = false;
                        CmdEnviar.Visibility = Visibility.Hidden;
                        FormaPago.Visibility = Visibility.Hidden;
                        NumTdc.Text = "";
                        NomTdc.Text = "";

                        this._anio = string.Empty;
                        this._mes = string.Empty;
                        this.TFECHA.Text = string.Empty;

                        StatusCmd = true;
                    }
                }
                else
                {
                    Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspDsError());
                    CmdNuevo_Click();
                }

            }
            Mouse.OverrideCursor = null;
        }
        private void CmdNuevo_Click()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".CmdNuevo_Click()";
            lblMoneda.Content = "";
            Globales.csvAmexenBanda = "";

            StatusCmd = true;
            CmdLeer.IsEnabled = true;
            FormaPago.IsEnabled = true;
            NumOrden.IsEnabled = true;
            NumCuarto.IsEnabled = true;
            Importe.IsEnabled = true;
            limpia();
            TypeUsuario.strVoucherCoP = "";
            CmdLeer.Visibility = Visibility.Visible;
            CmdEnviar.IsEnabled = false;

            this.BENVIAMAIL.Tag = string.Empty;
            this.BENVIAMAIL.Visibility = Visibility.Hidden;


            if (Utils.Mid(Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML), 1, 1) == "1")
            {
                frmFacileasing facil = new frmFacileasing();
                facil.abrir = abrir;
                facil.cerrar = cerrar;
                abrir(facil);
            }
            Globales.cpIntegraEMV.dbgEndOperation();

        }
        private void FormaPago_Click()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".FormaPago_Click";
            if (Globales.GetDataXml("MXN", TypeUsuario.CadenaXML).Contains(Globales.merchantBanda))
            {
                lblMoneda.Content = "MXN";
            }
            else
            {
                lblMoneda.Content = "USD";
            }
        }
        private void CmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".CmdNuevo_Click()";
            lblMoneda.Content = "";
            Globales.csvAmexenBanda = "";
            StatusCmd = true;
            CmdLeer.IsEnabled = true;
            NumOrden.IsEnabled = true;
            NumCuarto.IsEnabled = true;
            Importe.IsEnabled = true;
            limpia();
            TypeUsuario.strVoucherCoP = "";
            CmdLeer.Visibility = Visibility.Visible;
            CmdEnviar.IsEnabled = false;
            this.BENVIAMAIL.Tag = string.Empty;
            this.BENVIAMAIL.Visibility = Visibility.Hidden;

            if (Utils.Mid(Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML), 1, 1) == "1")
            {
                frmFacileasing facil = new frmFacileasing();
                facil.abrir = abrir;
                facil.cerrar = cerrar;
                abrir(facil);
            }
            Globales.cpIntegraEMV.dbgEndOperation();

        }
        private void limpia()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".Limpia()";
            NumTdc.Text = "";
            NomTdc.Text = "";
            NumCuarto.Text = "";
            NumOrden.Text = "";
            Importe.Text = "";
            CmdVoucher.IsEnabled = false;
            this.TFECHA.Text = string.Empty;
            this._mes = string.Empty;
            this._anio = string.Empty;



            CmdEnviar.Visibility = Visibility.Visible;
            CmdNuevo.Visibility = Visibility.Hidden;
            if (TypeUsuario.Id_Company == "0059")
            {
                NumOrden.MaxLength = 28;
            }
            else
            {
                NumOrden.MaxLength = Globales.MAXCAR;
            }
            imgEmailFirmaPanel.Visibility = Visibility.Hidden;
            BENVIAMAIL.Visibility = Visibility.Hidden;
        }
        private void AlfaNume(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }
        private void Numeros(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }
        private void Importe_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);

        }
        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }
        private void NumCuarto_GotFocus(object sender, RoutedEventArgs e)
        {
        }
        private void Importe_GotFocus(object sender, RoutedEventArgs e)
        {
        }
        private void CmdLeer_GotFocus(object sender, RoutedEventArgs e)
        {
        }
        private void NumOrden_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TypeUsuario.Id_Company == "0059" && NumOrden.IsEnabled)
            {
                TypeUsuario.strRefReaDig = "MA";
                Importe.Focus();
                frmReaderDigest reader = new frmReaderDigest();
                reader.ShowDialog();
                NumOrden.Text = Globales.referenciaAux;
            }
        }
        private void CmdVoucher_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            CmdVoucher.IsEnabled = false;
            Globales.PrintOptions(Voucher, Globales.cpIntegraEMV.getRspOperationNumber());//, Impresora
            CmdVoucher.IsEnabled = true;
            Mouse.OverrideCursor = null;
        }
        private void Importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }
        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir(true);
        }

        private void imgEmailFirmaPanel_MouseDown(object sender, MouseButtonEventArgs e)
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
                        if (Globales.GetDataXml("response", Globales.cpIntegraEMV.getRspXML()).ToLower() == "approved")
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
    }
}


