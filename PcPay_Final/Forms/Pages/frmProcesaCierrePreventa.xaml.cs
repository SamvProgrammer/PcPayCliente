using PcPay.Code.Configuracion;
using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
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
    /// Lógica de interacción para frmProcesaCierrePreventa.xaml
    /// </summary>
    public partial class frmProcesaCierrePreventa : Page
    {
        public double total_importe { get; set; }
        public cerrarVentana cierra;
        public frmProcesaCierrePreventa()
        {
            InitializeComponent();
            Carga();
            this.cmdVoucher.Visibility = System.Windows.Visibility.Hidden;
            this.Propina.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;
        }
        private const string NOMBRE_GENERAL = "frmProcesaCheckout";
        private void CmdEnviar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Globales.strNombreFP = NOMBRE_GENERAL + ".CmdEnviar_Click()";
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                string mensaje = string.Empty;


                dynamic Impte;
                dynamic IsAmex, strCadEncriptar, CadAmex;
                Impte = Globales.FormatMoneda(lblimportepreventa.Content.ToString());

                IsAmex = false;

                Globales.cpHTTP_Clear();
                Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
                strCadEncriptar = "&folio=" + lbloperacion.Content.ToString() +
                                  "&empresa=" + TypeUsuario.Id_Company +
                                  "&sucursal=" + TypeUsuario.Id_Branch +
                                  "&op=reimpvouchtermica" +
                                  "&co=true";
                strCadEncriptar = strCadEncriptar.Replace("\r", "");
                strCadEncriptar = strCadEncriptar.Replace("\t", "");
                Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
                if (Globales.cpHTTP_SendcpCUCT())
                {
                    Mouse.OverrideCursor = null;
                    int InSrt = Globales.cpHTTP_sResult.IndexOf(System.Environment.NewLine);
                    if (InSrt == -1)
                    {
                        InSrt = 0;
                    }
                    InSrt++;

                    string cadena = Utils.Mid(Globales.cpHTTP_sResult, InSrt + 2);
                    cadena = cadena.Replace(System.Environment.NewLine, "");
                    cadena = cadena.Replace("\r", "");
                    cadena = cadena.Replace("\t", "");
                    CadAmex = Globales.DecryptString(cadena, "KEY CREDIT CARD KEY", true);
                }

            Continuar:
                if (string.IsNullOrWhiteSpace(Propina.Text))
                {
                    Globales.MessageBoxMit("Introduzca la propina.");
                    Propina.Focus();
                }
                else
                {
                    CmdEnviar.IsEnabled = false;

                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    string strTypeC = string.Empty;
                    Globales.cpIntegracion_Clear();
                    Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL_CIERREPREVENTA;


                    Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    Globales.cpIntegraEMV.sndCierrePreventaMOTO(TypeUsuario.usu,
                                                    TypeUsuario.Pass,
                                                    "",
                                                    TypeUsuario.Id_Company,
                                                    TypeUsuario.Id_Branch,
                                                    TypeUsuario.country,
                                                    Impte,
                                                    Propina.Text,
                                                    lbloperacion.Content.ToString());

                    Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML() + "<propina>" + Propina.Text + "</propina>";
                    Globales.cpIntegracion_sResult = Globales.cpIntegracion_sResult.Replace("<amount/>", "<amount>" + Impte + "</amount>");

                    Mouse.OverrideCursor = null;


                    string caso = Globales.GetDataXml("response", Globales.cpIntegracion_sResult).ToLower();
                    switch (caso)
                    {
                        case "approved":
                            Propina.IsEnabled = false;

                            mensaje = string.Format("Aut. {0}", Globales.GetDataXml("auth", Globales.cpIntegracion_sResult));
                            Globales.MessageBoxMitApproved(mensaje, "Cierre Preventa Aprobado");

                            TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult).Trim();
                            TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult).Trim();

                            //                 //'************************************************************************************************
                            //                 //   ''********************************** FIRMA EN PANEL *********************************************
                            //                 //   'VALIDACIONES PARA LA FIRMA EN PANEL
                            string textoAgua = string.Empty;
                            textoAgua = "Folio: " + Globales.cpIntegraEMV.getRspOperationNumber() + System.Environment.NewLine;
                            textoAgua += "Auth: " + Globales.cpIntegraEMV.getRspAuth() + System.Environment.NewLine;
                            textoAgua += "Importe: " + Globales.cpIntegraEMV.getTx_Amount() + System.Environment.NewLine;
                            textoAgua += "Fecha: " + Globales.cpIntegraEMV.getRspDate() + System.Environment.NewLine;
                            textoAgua += "Merchant: " + Globales.cpIntegraEMV.getRspDsMerchant() + System.Environment.NewLine;
                            textoAgua += " " + System.Environment.NewLine;
                            textoAgua += ";___________________" + System.Environment.NewLine;
                            textoAgua += "FIRMA DIGITALIZADA:" + System.Environment.NewLine;


                            //                    '************************************************************************************************
                            //                    'valida si la tarjeta es Chip and Pin
                            bool IsChipAndPin = false; bool esQPS = false;
                            string cadenaHexFirma = string.Empty;
                            int tipoVta = 1;
                            var aux = Globales.cpIntegraEMV.chkCc_IsPin();
                            if (Globales.cpIntegraEMV.chkCc_IsPin() == "01")
                            {
                                IsChipAndPin = true;
                            }
                            if (Globales.cpIntegraEMV.getRspVoucher().Contains("@cnn Autorizado sin Firma ") && tipoVta == 1 && !IsChipAndPin)
                            {
                                esQPS = true;
                            }

                            //                    '************************************************************************************************
                            //                    'Si la PinPad no soporta Firma en Panel y no es touch, sigue el flujo normal de PcPay
                            if (!Globales.cpIntegraEMV.EsTouch() && !Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                            {// Then
                                goto GoImpresion;
                            }

                            //                    '************************************************************************************************
                            //                    'Si la PinPad Soporta Firma en Panel y no es touch,
                            if (!Globales.cpIntegraEMV.EsTouch() && Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                            {

                                // 'Llama a la función de obtener la firma en Panel
                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(false, textoAgua, Globales.cpIntegraEMV.getRspVoucher(),
                                                                                IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), (short)tipoVta, esQPS);

                                Mouse.OverrideCursor = null;
                                if (!cadenaHexFirma.Contains("Error"))
                                {
                                    //  'Llama a la funcion de Guardar la firma
                                    if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Globales.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                    {
                                        imgEmailFirmaPanel.Visibility = Visibility.Visible;
                                    }
                                }
                                else
                                {
                                    Globales.MessageBoxMit("No se pudo obtener la imagen del PinPad");
                                }

                                goto GoImpresion;

                            }

                            if (Globales.cpIntegraEMV.EsTouch())
                            {

                                if (TypeUsuario.UtilizarCapacidadTouch && TypeUsuario.EnviarVoucherMail)
                                {
                                    if (!Globales.ObtieneFirmaPanel(Globales.ipFirmaPanel, textoAgua, (short)tipoVta, IsChipAndPin, esQPS))
                                    {
                                        goto GoImpresion;
                                    }
                                    else
                                    {
                                        goto finaliza;
                                    }

                                }


                                if (TypeUsuario.UtilizarCapacidadTouch && !TypeUsuario.EnviarVoucherMail)
                                {

                                    //Llama a la función de obtener la firma en Panel
                                    cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(true, textoAgua,
                                                                                        Globales.cpIntegraEMV.getRspVoucher(), IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), (short)tipoVta, esQPS);

                                    if (!cadenaHexFirma.Contains("Error"))
                                    {
                                        //  'Llama a la funcion de Guardar la firma
                                        if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Globales.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                        {
                                            imgEmailFirmaPanel.Visibility = Visibility.Visible;
                                        }

                                    }
                                    else
                                    {
                                        Globales.MessageBoxMit("No se pudo obtener la imagen del PinPad");
                                    }

                                    goto GoImpresion;

                                }


                                if (!TypeUsuario.UtilizarCapacidadTouch && !TypeUsuario.EnviarVoucherMail)
                                {

                                    if (Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                                    {

                                        // 'Llama a la función de obtener la firma en Panel
                                        cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(false, textoAgua,
                                                                                            Globales.cpIntegraEMV.getRspVoucher(), IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), (short)tipoVta, esQPS);

                                        if (!cadenaHexFirma.Contains("Error"))
                                        {
                                            //   'Llama a la funcion de Guardar la firma
                                            if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Globales.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
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


                            //                '************************************************************************************************************
                        GoImpresion:
                            escogerImpresora();
                        finaliza:
                            //'fin del case

                            if (TypeUsuario.IsAQ)
                            {
                                Globales.VerificaVoucher();
                            }
                            cmdVoucher.Visibility = Visibility.Visible;
                            CmdEnviar.Visibility = Visibility.Hidden;

                            if (Globales.FacturaE == "1")
                            {

                                if (Globales.MessageConfirm("¿Desea factura electrónica?"))
                                {

                                    frmPreguntaFactura frmPregunta = new frmPreguntaFactura();
                                    frmPregunta.abrirFrmAhora = abrirFrmNuevo;
                                    frmPregunta.cerraPage = cierra;
                                    abrirFrmNuevo(frmPregunta, "Factura Electronica");
                                    Mouse.OverrideCursor = null;
                                    return;
                                }
                                else
                                {
                                    Globales.XMLFacturaE = "";
                                }
                            }


                            break;
                        case "denied":
                            Propina.IsEnabled = false;
                            mensaje = Globales.msjRech + ": " + Globales.GetDataXml("cd_response", Globales.cpIntegracion_sResult) + " " + Globales.GetDataXml("friendly_response", Globales.cpIntegracion_sResult);
                            Globales.MessageBoxMitDenied(mensaje);
                            CmdEnviar.Visibility = Visibility.Hidden;
                            break;
                        case "error":
                            mensaje = Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult);
                            Globales.MessageBoxMitError(mensaje);
                            CmdEnviar.Visibility = Visibility.Visible;
                            break;
                        default:
                            mensaje = "Verifique su conexión de Internet.";
                            Globales.MessageBoxMitError(mensaje);
                            CmdEnviar.Visibility = Visibility.Visible;
                            break;
                    }
                    Mouse.OverrideCursor = null;
                    CmdEnviar.IsEnabled = true;
                }
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
                TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult).Trim();
                switch (TypeUsuario.TipoImpresora)
                {
                    case "1":
                        if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult),
                                                      TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                        {
                            TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                            Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        }
                        cmdVoucher.IsEnabled = true;
                        break;
                    case "2":
                        break;
                    case "3":
                        Globales.imprimirEpson();
                        break;
                    case "4":
                        if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult),
                                                      TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                        {
                            TypeUsuario.strVoucher = Globales.VoucherHtml1(Globales.cpHTTP_sResult);
                            Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        }
                        cmdVoucher.IsEnabled = true;
                        break;
                    case "5":
                        break;
                    case "6":
                        Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                        cmdVoucher.IsEnabled = true;
                        break;
                    default:
                        Globales.MessageBoxMit("No hay una impresora seleccionada, porfavor ir a la parte de administracion a seleccionar una.");
                        break;
                }
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

        public abrirFrm abrirFrmNuevo;
        private void Carga()
        {


            Globales.strNombreFP = NOMBRE_GENERAL + ".Form_Load()";
            limpia();
            CargaLabels(Globales.InfoCheckOut);
        }

        private void CargaLabels(string p)
        {

            lblimportepreventa.Content = Globales.GetDataXml("importe" + Globales.InfoCheckOut, Globales.cpHTTP_sResult);
            Total.Text = Globales.GetDataXml("importe" + Globales.InfoCheckOut, Globales.cpHTTP_sResult);
            this.total_importe = double.Parse(this.Total.Text);

            lblFechapreventa.Content = Globales.GetDataXml("fecha" + Globales.InfoCheckOut, Globales.cpHTTP_sResult);
            lbloperacion.Content = Globales.GetDataXml("operacion" + Globales.InfoCheckOut, Globales.cpHTTP_sResult);
            lblMesero.Content = Globales.GetDataXml("mesero" + Globales.InfoCheckOut, Globales.cpHTTP_sResult);
            lblTurno.Content = Globales.GetDataXml("turno" + Globales.InfoCheckOut, Globales.cpHTTP_sResult);
            lblNombreCierrePreventa.Content = Globales.GetDataXml("cc_name" + Globales.InfoCheckOut, Globales.cpHTTP_sResult);
            lbloperacion.Visibility = Visibility.Hidden;
        }

        private void limpia()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".Limpia()";
            Propina.Text = "";
            Propina.IsEnabled = true;
            CmdEnviar.Visibility = Visibility.Visible;
            imgEmailFirmaPanel.Visibility = Visibility.Hidden;
        }


        private void cmdVoucher_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdVoucher_Click()";


            if (TypeUsuario.IsAQ)
            {
                Globales.VerificaVoucher();
                return;
            }
            switch (TypeUsuario.TipoImpresora)
            {
                case "1":
                    Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                    break;
                case "2":

                    break;
                case "3":
                    Globales.imprimirEpson();
                    break;
                case "4":
                    Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                    break;
                case "6":
                    Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                    break;
                default:
                    Globales.MessageBoxMit("No ha definido un tipo de impresora." + "\n" + "Para seleccionarla vaya al menú de administración.");
                    break;
            }

        }

        private void Numero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void Propina_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Propina.Text))
            {
                Total.Text = (Convert.ToDouble(lblimportepreventa.Content.ToString()) + Convert.ToDouble(Propina.Text)).ToString();
                Globales.formatoMoneda(sender, e);
                Total.Text = Globales.ImporteFormato(Total.Text);
            }
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void Propina_KeyDown(object sender, KeyEventArgs e)
        {
            //if (!string.IsNullOrWhiteSpace(Propina.Text))
            //{
            //    //   Propina.Text = Globales.ImporteFormato(Propina.Text);
            //    Total.Text = (Convert.ToDouble(lblimportepreventa.Content.ToString()) + Convert.ToDouble(Propina.Text)).ToString();
            //    Total.Text = Globales.ImporteFormato(Total.Text);
            //}
            if (e.Key == Key.Enter)
            {
                CmdEnviar.Focus();
            }
        }

        private void Propina_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(Propina.Text))
            {
                if (Propina.Text.Substring(0, 1) == "." && Propina.Text.Length == 1)
                {
                    return;
                }
                Total.Text = (this.total_importe + Convert.ToDouble(Propina.Text)).ToString();
                Total.Text = Globales.ImporteFormato(Total.Text);
            }
            else
            {
                this.Total.Text = Globales.ImporteFormato(this.total_importe.ToString());
            }
        }

        private void Propina_LostFocus_1(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
            if (string.IsNullOrWhiteSpace(Propina.Text))
            {
                Propina.Text = "0.00";
            }
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void Total_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Propina.Text = "0.00";
            imgEmailFirmaPanel.Visibility = Visibility.Hidden;
        }

        private void imgEmailFirmaPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Globales.sendMailFirmaPanel_MouseDown(sender,e);
        }
    }
}
