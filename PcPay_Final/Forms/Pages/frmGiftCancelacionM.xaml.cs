using PcPay.Code.Usuario;
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
    /// Interaction logic for frmGiftCancelacionM.xaml
    /// </summary>
    public partial class frmGiftCancelacionM : Page
    {
        public cerrarVentana cerrar;
        public frmGiftCancelacionM()
        {
            InitializeComponent();
            txtNumTarjeta.GotFocus += Globales.setFocusMit2;
            txtNumOperacion.GotFocus += Globales.setFocusMit2;
            txtNumAut.GotFocus += Globales.setFocusMit2;
            numOrden.GotFocus += Globales.setFocusMit2;
            importe.GotFocus += Globales.setFocusMit2;

            txtNumTarjeta.LostFocus += Globales.lostFocusMit2;
            txtNumOperacion.LostFocus += Globales.lostFocusMit2;
            txtNumAut.LostFocus += Globales.lostFocusMit2;
            numOrden.LostFocus += Globales.lostFocusMit2;
            importe.LostFocus += Globales.lostFocusMit2;

            importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    cmdActual(cmdLeer);
            //    limpia();
            //    lblCard.Content = "Introduzca datos y haga clic en el botón Enviar.";
            //    Globales.cpIntegraEMV.dbgEndOperation();
            //    if (!Globales.PPOperacion.dbgSetLector())
            //    {
            //        Globales.mensajeAlerta("Imposible conectarse con el lector, configure su lector y vuelva a intentarlo");
            //        cerrar();
            //        return;
            //    }

            //    //Menu 7 = true;
            //}
            //catch
            //{

            //}
        }
        private void limpia()
        {
            //importe.Text = "";
            //lblTInfo.Text = "";
            //lblAuth.Text = "";
            //txtNumOperacion.Text = "";
            //cmdVoucher.IsEnabled = false;
            //lblAprob.Visibility = Visibility.Hidden;
            //lblAuth.Visibility = Visibility.Hidden;
            //lblTInfo.Visibility = Visibility.Hidden;
            //lblTInfo.Visibility = Visibility.Visible;
            //lblDenied.Visibility = Visibility.Hidden;
            //cmdNuevo.Visibility = Visibility.Hidden;
            //gcTracks = "";
        }

        private void cmdActual(Button cmd)
        {
            cmdLeer.IsEnabled = false;
            cmdNuevo.IsEnabled = false;
            cmdLeer.Visibility = Visibility.Hidden;
            cmdNuevo.Visibility = Visibility.Hidden;
            cmd.Visibility = Visibility.Visible;
            cmd.IsEnabled = true;

        }

        private void txtNumTarjeta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void numero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender,e);
        }

        private void cmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            Page_Loaded(null,null);
        }

        private void cmdVoucher_Click(object sender, RoutedEventArgs e)
        {
            VoucherGift();
        }
        private void VoucherGift()
        {
            //try
            //{

            //    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            //    string GCpaper = "";
            //    TypeUsuario.strVoucherCoP = VoucherGC;
            //    if (TypeUsuario.strTipoLector == "3" && TypeUsuario.TipoImpresora == "6")
            //    {
            //        if (!Globales.PPOperacion.dbgPrintTicket(VoucherGC))
            //        {
            //            Globales.mensajeAlerta("Imposible imprimir verifique impresora.");
            //        }
            //    }

            //    else
            //    {
            //        switch (TypeUsuario.TipoImpresora.Substring(0, 1))
            //        {
            //            case "1":
            //                break;
            //            case "2":
            //                break;
            //            case "3":
            //                break;
            //            case "4":
            //                break;
            //            case "5":
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}
            //catch
            //{
            //    Mouse.OverrideCursor = null;
            //}
            //Mouse.OverrideCursor = null;
        }

        public string VoucherGC { get; set; }

        private void cmdLeer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumOperacion.Text))
            {
                Globales.MessageBoxMit("Introduzca el número de operacion");
                txtNumOperacion.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(importe.Text))
            {
                Globales.MessageBoxMit("Introduzca el importe.");
                importe.Focus();
                return;
            }
            else if (txtNumOperacion.Text.Length != 9)
            {
                Globales.MessageBoxMit("El número de operación debe ser de 9 dígitos.");
                txtNumOperacion.Focus();
                return;
            }

            readGiftCard();
        }
        private void readGiftCard()
        {
            //try
            //{
            //    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            //    importe.IsEnabled = false;
            //    txtNumOperacion.IsEnabled = false;
            //    lblAprob.Visibility = Visibility.Hidden;
            //    lblAuth.Visibility = Visibility.Hidden;
            //    lblDenied.Visibility = Visibility.Hidden;
            //    cmdLeer.IsEnabled = false;
                
            //    Globales.PPOperacion.dbgSetUrl(Globales.URL_GIFT);

            //    Globales.PPOperacion.sndPrePagoCancelacionM(TypeUsuario.usu,TypeUsuario.Pass,TypeUsuario.Id_Company,TypeUsuario.Id_Branch,"MX",
            //        txtNumTarjeta.Text,"","",numOrden.Text,"9",txtNumAut.Text,importe.Text,
            //        txtNumOperacion.Text,"MONEDA","");

            //    Mouse.OverrideCursor = null;
            //    switch (Globales.PPOperacion.getRspDsResponse())
            //    {
            //        case "approved":
            //            VoucherGC = Globales.PPOperacion.getRspTicket();
            //            if(Globales.cpIntegraEMV.dbgSetReader()){
            //                if(Globales.cpIntegraEMV.chkPp_Printer == "1"){
            //                    TypeUsuario.TipoImpresora = "6";
            //                }
            //            }
            //            VoucherGift();
            //            lblAprob.Visibility = Visibility.Visible;
            //            lblAuth.Visibility = Visibility.Visible;
            //            lblAuth.Text = "Auth.\n" + Globales.PPOperacion.getRspAuth();
            //            lblTInfo.Visibility = Visibility.Hidden;
            //            lblDenied.Visibility = Visibility.Hidden;
            //            cmdVoucher.IsEnabled = true;
            //            cmdVoucher.Visibility = Visibility.Hidden;
            //            importe.IsEnabled = false;
            //            txtNumOperacion.IsEnabled = false;
            //            cmdActual(cmdNuevo);
            //            break;
            //        case "denied":
            //            lblAprob.Visibility = Visibility.Hidden;
            //            lblAuth.Visibility = Visibility.Hidden;
            //            lblTInfo.Visibility = Visibility.Hidden;
            //            lblDenied.Visibility = Visibility.Visible;
            //            cmdActual(cmdLeer);
            //            break;
            //        case "error":
            //            lblAprob.Visibility = Visibility.Hidden;
            //            lblAuth.Visibility = Visibility.Hidden;
            //            lblTInfo.Visibility = Visibility.Visible;
            //            lblTInfo.Text = Globales.PPOperacion.getRspDsError();
            //            lblDenied.Visibility = Visibility.Hidden;
            //            cmdActual(cmdLeer);
            //            importe.IsEnabled = true;
            //            txtNumOperacion.IsEnabled = true;
            //            break;
            //        default:
            //            lblAprob.Visibility = Visibility.Hidden;
            //            lblAuth.Visibility = Visibility.Hidden;
            //            lblTInfo.Visibility = Visibility.Visible;
            //            lblTInfo.Text = "Verifique su conexión de internet.";
            //            cmdActual(cmdLeer);
            //            break;
            //    }
            //    lblCard.Content = "Saldo Actual: " + Globales.PPOperacion.getRspBalance();
            //}
            //catch
            //{
            //    Mouse.OverrideCursor = null;
            //}
            //Mouse.OverrideCursor = null;
        }

        public string gcTracks { get; set; }

        private void importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender,e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender,e);
        }

        private void numOrden_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }
    }
}
