using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using PcPay.Forms.formularios;
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
    /// Lógica de interacción para frmPreventa.xaml
    /// </summary>
    public partial class frmPreventa : Page
    {

        private const string NOMBRE_GENERAL = "frmBandaAgencia";

        public string strProveedor { get; set; }
        public string strNoServicio { get; set; }
        public string strNoEconomico { get; set; }
        public string Voucher { get; set; }
        public string mes { get; set; }
        public string anio { get; set; }

        public frmPreventa()
        {
            InitializeComponent();
            this.Cargar();
            Importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;

            NumOrden.GotFocus += Globales.setFocusMit2;
            NumMesero.GotFocus += Globales.setFocusMit2;
            NumTurno.GotFocus += Globales.setFocusMit2;
            Importe.GotFocus += Globales.setFocusMit2;

            NumOrden.LostFocus += Globales.lostFocusMit2;
            NumMesero.LostFocus += Globales.lostFocusMit2;
            NumTurno.LostFocus += Globales.lostFocusMit2;
            Importe.LostFocus += Globales.lostFocusMit2;


            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;
            this.BENVIAMAIL.MouseDown += Globales.sendMail_GridMouseDown;
        }

        private void CmdLeer_Click(object sender, RoutedEventArgs e)
        {
            realizaOp();
        }

        private void realizaOp()
        {
            if (string.IsNullOrWhiteSpace(NumOrden.Text))
            {
                Globales.MessageBoxMit("Introduzca" + TypeUsuario.reference);
                NumOrden.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(NumMesero.Text))
            {

                Globales.MessageBoxMit("Introduzca el Número de Mesero.");
                NumMesero.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(NumTurno.Text))
            {

                Globales.MessageBoxMit("Introduzca el Turno");
                NumTurno.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(Importe.Text))
            {

                Globales.MessageBoxMit("Introduzca el importe.");
                Importe.Focus();
                return;
            }
            else if ((Utils.Mid(Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML), 1, 1) == "1") && (Globales.GetDataXml("tarjeta", Globales.cpHTTP_sResult) != NumTdc.Text))
            {

                Globales.MessageBoxMit("La Tarjeta Deslizada no corresponde con el Servicio a cobrar");
            }
            else
                if (TypeUsuario.Id_Company == "0059")
                {
                    if (NumOrden.Text.Length != 28)
                    {
                        Globales.MessageBoxMit("El no. de REFERENCIA debe ser de 28 posiciones");
                        NumOrden.Focus();
                        return;
                    }
                }

            if (!Globales.isNumeric(Importe.Text))
            {
                Globales.MessageBoxMit("El importe debe ser numérico.");
                Importe.Focus();
                return;
            }
            CmdLeer.IsEnabled = false;
            cboBanco.IsEnabled = false;
            FormaPago.IsEnabled = false;
            NumOrden.IsEnabled = false;
            Importe.IsEnabled = false;
            NumMesero.IsEnabled = false;
            NumTurno.IsEnabled = false;

            Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
            //   // 'Prepara PinPad y DLL, para EMV
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globales.cpIntegraEMV.dbgStartTxEMV(Importe.Text);
            CmdLeer.IsEnabled = false;
            Mouse.OverrideCursor = null;
            if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()) && !string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkCc_Number()))
            {
                //       // 'Extrae Datos de la tarjeta
                NumTdc.Text = Globales.cpIntegraEMV.chkCc_Number();
                NomTdc.Text = Globales.cpIntegraEMV.chkCc_Name();

                this.mes = Globales.cpIntegraEMV.chkCc_ExpMonth();
                this.anio = Globales.cpIntegraEMV.chkCc_ExpYear();

                this.TFECHAVENC.Text = string.Format("{0}/{1}", this.mes, this.anio);
                Globales.merchantBanda = Globales.cpIntegraEMV.dbgGetMerchantBanda("9");

                if (Globales.merchantBanda != "00000")
                {
                    Globales.isAmex = Globales.cpIntegraEMV.dbgGetisAmex();
                    this.formaPago();
                    CmdLeer.Visibility = Visibility.Hidden;
                    CmdEnviar.IsEnabled = true;
                    CmdEnviar.Visibility = Visibility.Visible;

                    if (!Globales.cpIntegraEMV.dbgTxMonitor())
                    {
                        Globales.MessageBoxMitError(Globales.cpIntegraEMV.getRspDsError());

                        CmdLeer.Visibility = Visibility.Visible;
                        CmdLeer.IsEnabled = true;
                        CmdEnviar.IsEnabled = false;
                        CmdEnviar.Visibility = Visibility.Hidden;
                        FormaPago.Visibility = Visibility.Hidden;
                        //  Label(6).Visibility = Visibility.Hidden;
                        NumTdc.Text = "";
                        NomTdc.Text = "";
                        this.TFECHAVENC.Text = string.Empty;


                        this.mes = string.Empty;
                        this.anio = string.Empty;
                    }
                }
                else
                    Globales.MessageBoxMitError(Globales.cpIntegraEMV.getRspDsError());
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()))
                {
                    Globales.MessageBoxMitError("Error en leer la tarjeta...");
                }
                CmdLeer.IsEnabled = true;
            }
        }


        private void formaPago()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".formaPago()";
            this.lblMoneda.Content = (Globales.GetDataXml("MXN", TypeUsuario.CadenaXML).Contains(Globales.merchantBanda) ? "MXN" : "USD");
        }

        private void CmdEnviar_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdEnviar_Click()";
            string mensaje = string.Empty;

            CmdEnviar.IsEnabled = false;
            FormaPago.IsEnabled = false;

            if (string.IsNullOrWhiteSpace(NumTdc.Text) || string.IsNullOrWhiteSpace(this.mes) || string.IsNullOrWhiteSpace(this.anio))
            {

                Globales.MessageBoxMit("Deslice la tarjeta bancaria");
                CmdEnviar.IsEnabled = true;
                return;
            }
            else if (string.IsNullOrWhiteSpace(NumOrden.Text))
            {

                Globales.MessageBoxMit("Introduzca" + TypeUsuario.reference + "");
                NumOrden.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            else if (string.IsNullOrWhiteSpace(NumMesero.Text))
            {

                Globales.MessageBoxMit("Introduzca el Número de Mesero.");
                NumMesero.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            else if (string.IsNullOrWhiteSpace(NumTurno.Text))
            {

                Globales.MessageBoxMit("Introduzca el Turno.");
                NumTurno.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            else if (string.IsNullOrWhiteSpace(Importe.Text))
            {

                Globales.MessageBoxMit("Introduzca el Turno.");
                Importe.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            else if (Utils.Mid(Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML), 1, 1) == "1" && (Globales.GetDataXml("tarjeta", Globales.cpHTTP_sResult) != NumTdc.Text))
            {

                Globales.MessageBoxMit("La Tarjeta Deslizada no corresponde con el Servicio a cobrar");
                CmdEnviar.IsEnabled = true;
                return;
            }
            else
                if (TypeUsuario.Id_Company == "0059")
                {
                    if (NumOrden.Text.Length != 28)
                    {

                        Globales.MessageBoxMit("El no. de REFERENCIA debe ser de 28 posiciones");
                        NumOrden.Focus();
                        CmdEnviar.IsEnabled = true;
                        return;
                    }
                }


            // 'EMV FULL
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

            // 'Se agrega validación para no permitir enviar la transacción sin contar con merchant
            if (string.IsNullOrWhiteSpace(Globales.merchantBanda))
            {
                Globales.MessageBoxMit("No hay planes de pago para esta tarjeta, por favor cambie la tarjeta.");
                CmdEnviar.IsEnabled = true;
                return;
            }

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            string moneda = lblMoneda.Content.ToString();
            Globales.cpIntegraEMV.dbgSetCurrencyType(moneda);
            Globales.cpIntegraEMV.sndPreventaEMV(TypeUsuario.usu, TypeUsuario.Pass, "", TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
            TypeUsuario.country, strTypeC, Globales.merchantBanda, NumOrden.Text,
            "9", moneda, NumMesero.Text, NumTurno.Text, Globales.csvAmexenBanda);
            Mouse.OverrideCursor = null;
            //frmPlanPagos.merchant = ""
            //frmPlanPagos.TipoTarjeta = ""
            //frmPlanPagos.Bin = ""
            Globales.csvAmexenBanda = "";
            string temporal = Globales.cpIntegraEMV.getRspXML();

            string aux = Globales.cpIntegraEMV.getRspDsResponse();
            aux = aux.ToLower();
            switch (aux)
            {
                case "approved":
                    cboBanco.IsEnabled = false;
                    FormaPago.IsEnabled = false;
                    NumOrden.IsEnabled = false;
                    Importe.IsEnabled = false;
                    NumMesero.IsEnabled = false;
                    NumTurno.IsEnabled = false;
                    mensaje = Globales.cpIntegraEMV.getRspAuth();
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

                    TypeUsuario.strVoucherCoP = Globales.cpIntegraEMV.getRspVoucher();
                    CmdNuevo.Visibility = Visibility.Visible;
                    CmdEnviar.Visibility = Visibility.Hidden;
                    cmdVoucher.IsEnabled = true;

                    this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;
                    this.BENVIAMAIL.Tag = this.NomTdc.Text;

                    Voucher = Globales.cpIntegraEMV.getRspVoucher();

                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    Globales.PrintOptions(Voucher, Globales.cpIntegraEMV.getRspOperationNumber());
                    Mouse.OverrideCursor = null;
                    break;
                case "denied":
                    //label4.Text = "La transacción ha finalizado, si deseas hacer otra transacción haz  clic en el botón Otro Cobro.";
                    if (Utils.Mid(Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML), 1, 1) == "1")
                    {
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
                    }
                    Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
                    mensaje = Globales.msjRech + " " + Globales.cpIntegraEMV.getRspCdResponse() + " " + Globales.cpIntegraEMV.getRspFriendlyResponse();
                    Globales.MessageBoxMitDenied(mensaje);
                    CmdNuevo.Visibility = Visibility.Visible;
                    CmdEnviar.Visibility = Visibility.Hidden;
                    break;
                case "error":
                    if (Globales.EMVLector == "1" && (TypeUsuario.strTipoLector == "3" || TypeUsuario.strTipoLector == "4"))
                    {
                        CmdLeer.Visibility = Visibility.Visible;
                        CmdLeer.IsEnabled = true;
                        CmdEnviar.IsEnabled = false;
                        CmdEnviar.Visibility = Visibility.Hidden;
                        cboBanco.IsEnabled = true;
                        FormaPago.IsEnabled = true;
                        Importe.IsEnabled = true;
                        NumOrden.IsEnabled = true;
                        NumMesero.IsEnabled = true;
                        NumTurno.IsEnabled = true;
                        NumTdc.Text = "";
                        TFECHAVENC.Text = "";
                        NomTdc.Text = "";
                    }
                    else
                    {
                        CmdEnviar.Visibility = Visibility.Visible;
                        CmdEnviar.IsEnabled = true;
                    }
                    mensaje = Globales.cpIntegraEMV.getRspDsError();
                    Globales.MessageBoxMitError(mensaje);
                    if (mensaje.Contains("La transaccion ya fue aprobada")) {
                        CmdNuevo.Visibility = Visibility.Visible;
                        CmdLeer.Visibility = Visibility.Hidden;
                        cboBanco.IsEnabled = false;
                        FormaPago.IsEnabled = false;
                        NumOrden.IsEnabled = false;
                        Importe.IsEnabled = false;
                        NumMesero.IsEnabled = false;
                        NumTurno.IsEnabled = false;
                        CmdEnviar.Visibility = Visibility.Hidden;
                        CmdEnviar.IsEnabled = false;
                    }
                    else {
                        CmdNuevo.Visibility = Visibility.Hidden;
                    }
                    Globales.cpIntegraEMV.dbgCancelOperation();
                    break;
                default:
                    //label4.Text = "La transacción ha finalizado, si deseas intentar nuevamente haz  clic en el botón Activar Lector.";
                    if (Globales.EMVLector == "1" && (TypeUsuario.strTipoLector == "3" || TypeUsuario.strTipoLector == "4"))
                    {
                        CmdEnviar.IsEnabled = false;
                        CmdLeer.Visibility = Visibility.Visible;
                        CmdLeer.IsEnabled = true;
                        cboBanco.IsEnabled = true;
                        FormaPago.IsEnabled = true;
                        Importe.IsEnabled = true;
                        NumOrden.IsEnabled = true;
                        NumMesero.IsEnabled = true;
                        NumTurno.IsEnabled = true;
                    }
                    else
                    {
                        CmdEnviar.Visibility = Visibility.Visible;
                        CmdEnviar.IsEnabled = true;
                    }
                    mensaje = "Error de conexión, verifique su reporte.";
                    Globales.MessageBoxMitError(mensaje);
                    CmdNuevo.Visibility = Visibility.Hidden;


                    break;
            }
            Mouse.OverrideCursor = null;
        }

        private void cmdVoucher_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                escogerImpresora();
            }
            catch
            {

            }

        }

        private void escogerImpresora()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                cmdVoucher.IsEnabled = false;
                CmdNuevo.IsEnabled = false;
                Globales.PrintOptions(Voucher, Globales.cpIntegraEMV.getRspOperationNumber());
                cmdVoucher.IsEnabled = true;
                CmdNuevo.IsEnabled = true;
                if (TypeUsuario.IsAQ)
                {
                    Globales.VerificaVoucher();
                }

            }
            catch
            {

            }
            Mouse.OverrideCursor = null;
        }
        // GloboPcPay Me.LblTError, Me.LblAprob, Me.LblDenied, Me.LblAuth

        private void Cargar()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".Cargar()";
            lblMoneda.Content = "";
            this.TFECHAVENC.Text = string.Empty;


            if (TypeUsuario.Id_Company == Globales.EMPREF && TypeUsuario.Id_Branch == Globales.EMPREF2)
            {
                NumOrden.Visibility = Visibility.Hidden;
                label7.Visibility = Visibility.Hidden;
            }

            if (Globales.bolActivaMotoP)
            {
                limpia();
                Mouse.OverrideCursor = null;
                CmdLeer.Visibility = Visibility.Visible;
                CmdEnviar.Visibility = Visibility.Hidden;
            }
            else
            {
                // Unload Me

            }
            label7.Content = TypeUsuario.reference;
            if (TypeUsuario.ShowMsg)
            {

                frmAvisoBanda frmBanda = new frmAvisoBanda();
                frmBanda.Owner = Globales.principal;
                frmBanda.ShowDialog();
            }
        }

        private void limpia()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".Limpia()";
            NumTdc.Text = "";
            NomTdc.Text = "";
            NumMesero.Text = "";
            NumTurno.Text = "";
            NumOrden.Text = "";
            this.TFECHAVENC.Text = string.Empty;

            this.mes = string.Empty;
            this.anio = string.Empty;

            Importe.Text = "";
            cmdVoucher.IsEnabled = false;
            FormaPago.Items.Clear();

            CmdEnviar.Visibility = Visibility.Hidden;
            CmdNuevo.Visibility = Visibility.Hidden;
            CmdLeer.Visibility = Visibility.Visible;
            // 'Cambio Facileasing predefine Santander y lo oculta
            if (Utils.Mid(Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML), 1, 1) == "1")
            {
                for (int i = 0; i <= cboBanco.Items.Count - 1; i++)
                {
                    //if (cboBanco.ItemData(i) == 1)
                    //{// 'Si es Santander
                    //    cboBanco.ListIndex = i;
                    //    cboBanco.Visibility = Visibility.Hidden;
                    //    FormaPago.Visibility = Visibility.Hidden;

                    //    break;
                    //}
                }
            }
            //'Fin cambio Facileasing 22092006
            if (TypeUsuario.Id_Company == "0059")
            {
                NumOrden.MaxLength = 28;
            }
            else
            {
                NumOrden.MaxLength = Globales.MAXCAR;
            }

        }

        private void ObtieneBancos(string strBancos)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".ObtieneBancos()";
            if (!string.IsNullOrWhiteSpace(strBancos))
            {
                string strAux = strBancos;
                string numEmpr = string.Empty;
                int i = 1;
                do
                {
                    int pos = strBancos.IndexOf("|");
                    if (pos == -1)
                    {
                        pos = 0;
                    }
                    pos++;
                    strAux = Utils.Mid(strBancos, 1, pos - 1);

                    cboBanco.Items.Add(Utils.Mid(strAux, strAux.IndexOf(",") + 1));
                    strBancos = Utils.Mid(strBancos, strBancos.IndexOf("|") + 1);
                    i++;
                } while (strBancos.Length > 0);
            }
        }

        private void numero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }

        private void alfaNumerico(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

        private void Importe_LostFocus(object sender, RoutedEventArgs e)
        {

            Globales.formatoMoneda(sender, e);

        }

        private void CmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".CmdNuevo_Click()";
            lblMoneda.Content = "";
            Globales.csvAmexenBanda = "";

            Globales.cpIntegraEMV.dbgEndOperation();
            CmdLeer.IsEnabled = true;
            cboBanco.IsEnabled = true;
            FormaPago.IsEnabled = true;
            NumOrden.IsEnabled = true;

            NumMesero.IsEnabled = true;
            NumTurno.IsEnabled = true;
            Importe.IsEnabled = true;
            limpia();
            TypeUsuario.strVoucherCoP = "";
            CmdLeer.Visibility = Visibility.Visible;
            CmdEnviar.IsEnabled = false;

            FormaPago.Visibility = Visibility.Hidden;

            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;
            if (Utils.Mid(Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML), 1, 1) == "1")
            {
                //frmBandaEMV.Visible = False
                //Load frmfacileasing
            }

        }

        private void NumOrden_LostFocus(object sender, RoutedEventArgs e)
        {
            NumOrden.Text = NumOrden.Text.ToUpper();
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void NumOrden_GotFocus(object sender, RoutedEventArgs e)
        {

            if (TypeUsuario.Id_Company == "0059" && NumOrden.IsEnabled)
            {
                TypeUsuario.strRefReaDig = "BA";
                Importe.Focus();
                frmReaderDigest frm = new frmReaderDigest();
                frm.Show();
            }
        }

        private void SetMensaje(string msj)
        {
            //label4.Text = msj;
            //label4.Foreground = Brushes.Blue;
        }

        private void NumMesero_GotFocus(object sender, RoutedEventArgs e)
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

        private void NumTurno_GotFocus(object sender, RoutedEventArgs e)
        {
            SetMensaje("Ingrese turno");
        }

        private void Importe_GotFocus(object sender, RoutedEventArgs e)
        {
            SetMensaje(Globales.MsjImporte);

        }

        private void CmdLeer_GotFocus(object sender, RoutedEventArgs e)
        {
            SetMensaje("Haga clic en el botón Activar Lector e inserte chip o deslice la tarjeta");
        }


        private void NumeroPunto(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir(true);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NumOrden.Focus();
        }

    }
}



