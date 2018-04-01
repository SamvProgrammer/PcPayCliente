using PcPay.Code.Usuario;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for frmCancelacion.xaml
    /// </summary>
    public partial class frmCancelacion : Page
    {
        public Window propietario { get; set; }
        public frmCancelacion()
        {
            InitializeComponent();

            #region"Efectos"
            this.txtNumAut.GotFocus += Globales.setFocusMit2;
            this.txtNumOper.GotFocus += Globales.setFocusMit2;
            this.Importe.GotFocus += Globales.setFocusMit2;

            this.txtNumAut.LostFocus += Globales.lostFocusMit2;
            this.txtNumOper.LostFocus += Globales.lostFocusMit2;
            this.Importe.LostFocus += Globales.lostFocusMit2;
            #endregion
            txtNumOper.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            Importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;


            this.imgEmail.Visibility = Visibility.Hidden;
            this.imgEmail.Tag = string.Empty;
            this.imgEmail.MouseDown += Globales.sendMailFirmaPanel_MouseDown;
        }
        private void loadPage(object sender, RoutedEventArgs e)
        {
            limpia();
            imgEmail.Visibility = Visibility.Hidden;
        }
        private void limpia()
        {
            txtNumOper.Text = "";
            txtNumOper.IsEnabled = true;
            txtNumAut.Text = "";
            txtNumAut.IsEnabled = true;
            Importe.Text = "";
            Importe.IsEnabled = true;
            cmdVoucher.IsEnabled = false;

            cmdEnviar.Visibility = Visibility.Visible;
            cmdNuevo.Visibility = Visibility.Hidden;
        }

        private void imprimirVooucher(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (TypeUsuario.TipoImpresora == "6")
            {
                cmdVoucher.IsEnabled = false;
                cmdNuevo.IsEnabled = false;
                Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                cmdVoucher.IsEnabled = true;
                cmdNuevo.IsEnabled = true;
            }
            else
            {
                switch (TypeUsuario.TipoImpresora)
                {
                    case "1":
                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        break;
                    case "4":
                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        break;
                    case "3":
                        Globales.imprimirEpson();
                        break;
                    default:
                        Globales.MessageBoxMit("No ha definido un tipo de impresora.\nPara seleccionarla, vaya al menu de administración.");
                        break;
                }
            }

            if (TypeUsuario.IsAQ)
            {
                Globales.VerificaVoucher();
            }
            Mouse.OverrideCursor = null;
        }

        private void Nuevo(object sender, RoutedEventArgs e)
        {
            limpia();
            imgEmail.Visibility = Visibility.Hidden;
        }

        private void Procesar(object sender, RoutedEventArgs e)
        {
            if (txtNumOper.Text.Trim().Length != 9)
            {
                //Globales.MessageBoxMit("El número de operación debe ser de 9 digitos.");
                Globales.MessageBoxMit("El número de operación debe ser de 9 digitos.");
                txtNumOper.Focus();
                return;
            }
            else if (txtNumAut.Text.Trim().Length != 6)
            {
                //Globales.MessageBoxMit("El número de autorización debe ser de 6 digitos.");
                Globales.MessageBoxMit("El número de autorización debe ser de 6 digitos.");
                txtNumAut.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNumOper.Text) || txtNumOper.Text.Length != 9)
            {
                Globales.MessageBoxMit("Introduzca el número de operación.");
                txtNumOper.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtNumAut.Text))
            {
                Globales.MessageBoxMit("Introduzca el número de autorizacion");
                txtNumAut.Focus();
            }
            else if (string.IsNullOrWhiteSpace(Importe.Text))
            {
                Globales.MessageBoxMit("Introduzca el importe");
                Importe.Focus();
            }
            else
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                cmdEnviar.IsEnabled = false;
                Globales.cpIntegracion_Clear();
                Globales.sURL_cpINTEGRA = Globales.URL_DLL_CANC;
                Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                Globales.cpIntegraEMV.sndCancelacion(TypeUsuario.usu,
                    TypeUsuario.Pass, "", TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
                    TypeUsuario.country, Importe.Text, txtNumOper.Text, txtNumAut.Text);
                Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();

                string strCadEncriptar = "";
                string mensaje = string.Empty;
                Mouse.OverrideCursor = null;
                switch (Globales.GetDataXml("response", Globales.cpIntegracion_sResult))
                {
                    case "approved":
                        txtNumOper.IsEnabled = false;
                        txtNumAut.IsEnabled = false;
                        Importe.IsEnabled = false;

                        mensaje =  Globales.GetDataXml("auth", Globales.cpIntegracion_sResult);
                        //FIRMA EN PANEL

                        //texto de marca de agu


                        Globales.MessageBoxMitApproved(mensaje,true);
                #region firma en panel
                          string textoAgua = string.Empty;
                        textoAgua += "Folio:" + Globales.cpIntegraEMV.getRspOperationNumber() + "\n";
                        textoAgua += "Auth:" + Globales.cpIntegraEMV.getRspAuth() + "\n";
                        textoAgua += "Importe:" + Globales.cpIntegraEMV.getTx_Amount() + "\n";
                        textoAgua += "Fecha" + Globales.cpIntegraEMV.getRspDate() + "\n";
                        textoAgua += "Merchant" + Globales.cpIntegraEMV.getRspDsMerchant() + "\n";
                        textoAgua += " \n";
                        textoAgua += "_______________________\n";
                        textoAgua += "FIRMA DIGITALIZADA:\n";

                        bool isChipAndPind, esQPS;
                        string cadenaHexFirma = string.Empty;
                        int tipoVta;
                        esQPS = false;
                        tipoVta = 1;
                        isChipAndPind = false;
                        //Valida si el chip and pind

                        if(Globales.cpIntegraEMV.chkCc_IsPin() == "01"){
                            isChipAndPind = true;
                        }
                        if(Globales.cpIntegraEMV.getRspVoucher().Contains("@cnn Autorizado sin Firma ") && tipoVta == 1 && !isChipAndPind){
                            esQPS = true;
                        }


                        //Si pinpad soporta firma en panel y no es touch
                        if (!Globales.cpIntegraEMV.EsTouch() && Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                        {
                            cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(false, textoAgua, Globales.cpIntegraEMV.getRspVoucher(), isChipAndPind, Globales.cpIntegraEMV.chkPp_Trademark(), tipoVta, esQPS);


                            if (!cadenaHexFirma.Contains("Error"))
                            {
                                if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Globales.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                {
                                    imgEmail.Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                Globales.MessageBoxMit("No se pudo obtener la imagen del PinPad.");
                            }
                            goto GoImpresion;
                        }
                        //Sí el dispositivo tiene capacidad touch...


                        if(Globales.cpIntegraEMV.EsTouch()){
                            if(TypeUsuario.UtilizarCapacidadTouch && TypeUsuario.EnviarVoucherMail){
                               if(!Globales.ObtieneFirmaPanel(Globales.ipFirmaPanel,textoAgua,tipoVta,isChipAndPind,esQPS)){
                                   goto GoImpresion;
                               }else{
                                 goto finaliza;
                               }
                            }

                            if(TypeUsuario.UtilizarCapacidadTouch && TypeUsuario.EnviarVoucherMail){
                                 //Llama a la función de obtener la firma en panel
                                cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(true, textoAgua, Globales.cpIntegraEMV.getRspVoucher(), isChipAndPind, Globales.cpIntegraEMV.chkPp_Trademark(),tipoVta,esQPS);
                                if (!cadenaHexFirma.Contains("Error"))
                                {
                                    if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Globales.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                    {
                                        imgEmail.Visibility = Visibility.Visible;
                                    }
                                }
                                else {
                                    Globales.MessageBoxMit("No se pudo obtener la imagen del pinpad");
                                }
                                goto GoImpresion;
                            }

                            if(!TypeUsuario.UtilizarCapacidadTouch && !TypeUsuario.EnviarVoucherMail){
                                if(Globales.cpIntegraEMV.getRspSoportaFirmaPanel()){
                                    cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(false,textoAgua,Globales.cpIntegraEMV.getRspVoucher(),isChipAndPind,Globales.cpIntegraEMV.chkPp_Trademark(),tipoVta,esQPS);
                                    if (!cadenaHexFirma.Contains("Error"))
                                    {
                                        if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Globales.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                        {
                                            imgEmail.Visibility = Visibility.Visible;
                                        }
                                    }
                                    else {
                                        Globales.MessageBoxMit("No s pudo obtener la imagen del PinPad\n"+cadenaHexFirma);
                                    }
                                }
                                goto GoImpresion;
                                }
                        }//Fin de si eres Touch

#endregion
                    GoImpresion:
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult);
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
                                cmdNuevo.IsEnabled = true;
                                break;
                            case "4":
                                if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult),
                                                        TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                                {
                                    TypeUsuario.strVoucher = Globales.VoucherHtml1(Globales.cpHTTP_sResult);
                                    Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                                }
                                cmdVoucher.IsEnabled = true;
                                cmdNuevo.IsEnabled = true;
                                break;
                            case "3":
                                Globales.imprimirEpson();
                                break;
                            case "6":
                                Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                                cmdVoucher.IsEnabled = true;
                                cmdNuevo.IsEnabled = true;

                                break;
                            default:
                                break;
                        }

                finaliza:
                        cmdNuevo.Visibility = Visibility.Visible;
                        cmdEnviar.Visibility = Visibility.Visible;
                        Mouse.OverrideCursor = null;
                        break;
                    case "denied":
                        txtNumOper.Visibility = Visibility.Visible;
                        txtNumAut.IsEnabled = false;
                        Importe.IsEnabled = false;

                        mensaje =  Globales.GetDataXml("cd_response", Globales.cpIntegracion_sResult);
                        Globales.MessageBoxMitDenied("Cancelación denegada\n"+mensaje);
                        cmdNuevo.Visibility = Visibility.Visible;
                        cmdEnviar.Visibility = Visibility.Hidden;
                        break;
                    case "error":

                        mensaje = Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult);
                        Globales.MessageBoxMitError(mensaje);
                        cmdNuevo.Visibility = Visibility.Hidden;
                        cmdEnviar.Visibility = Visibility.Visible;
                        break;
                    default:
                        mensaje = "Verifique su conexión de internet.";
                        this.setMensaje("none", mensaje);

                        cmdNuevo.Visibility = Visibility.Hidden;
                        cmdEnviar.Visibility = Visibility.Visible;
                        Globales.MessageBoxMitError(mensaje);
                        break;
                }
                cmdEnviar.IsEnabled = true;
                Mouse.OverrideCursor = null;
            }

        }

        private void Importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void Importe_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
        }

        private void numero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }

        private void txtNumAut_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }
        public void setMensaje(string typeMessage, string message)
        {
            switch (typeMessage)
            {
                case "approved":
                    Globales.MessageBoxMitApproved(message, true);
                    //this.lblMensaje.FontSize = 25;
                    break;
                case "denied":
                    Globales.MessageBoxMitDenied(message);
                    break;
                case "error":
                    Globales.MessageBoxMitError(message);
                    break;
                case "none":
                    break;
            }
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender,e);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
