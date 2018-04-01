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
    /// Interaction logic for frmGiftCobroM.xaml
    /// </summary>
    public partial class frmGiftCobroM : Page
    {
        public frmGiftCobroM()
        {
            InitializeComponent();
            txtNumTarjeta.LostFocus += Globales.lostFocusMit2;
            txtCode.LostFocus += Globales.lostFocusMit2;
            numOrden.LostFocus += Globales.lostFocusMit2;
            importe.LostFocus += Globales.lostFocusMit2;

            txtNumTarjeta.GotFocus += Globales.setFocusMit2;
            txtCode.GotFocus += Globales.setFocusMit2;
            numOrden.GotFocus += Globales.setFocusMit2;
            importe.GotFocus += Globales.setFocusMit2;

            importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;
        }

        private void cmdvoucher_Click(object sender, RoutedEventArgs e)
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Menu7 = false;

                cmdActual(cmdLeer);
                limpia();
            }
            catch
            {

            }
        }

        private void limpia()
        {
            //importe.Text = "";
            //lblTInfo.Text = "";
            //lblAuth.Text = "";
            //txtNumTarjeta.Text = "";
            //cmdvoucher.IsEnabled = false;
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
            //cmdLeer.IsEnabled = false;
            //cmdNuevo.IsEnabled = false;
            //cmdLeer.Visibility = Visibility.Hidden;
            //cmdNuevo.Visibility = Visibility.Hidden;
            //cmd.Visibility = Visibility.Visible;
            //cmd.IsEnabled = true;

        }

        public string gcTracks { get; set; }

        private void cmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            Page_Loaded(null,null);
        }

        private void cmdLeer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumTarjeta.Text))
            {
                Globales.MessageBoxMit("Introduzca el número de operacion");
                txtNumTarjeta.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(importe.Text))
            {
                Globales.MessageBoxMit("Introduzca el importe.");
                importe.Focus();
                return;
            }
            else if (txtNumTarjeta.Text.Length != 16)
            {
                Globales.MessageBoxMit("El número de operación debe ser de 9 dígitos.");
                txtNumTarjeta.Focus();
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
            //    txtNumTarjeta.IsEnabled = false;
            //    lblAprob.Visibility = Visibility.Hidden;
            //    lblAuth.Visibility = Visibility.Hidden;
            //    lblDenied.Visibility = Visibility.Hidden;
            //    cmdLeer.IsEnabled = false;


            //    Globales.PPOperacion.dbgSetUrl(Globales.URL_GIFT);
            //    Globales.PPOperacion.sndPrePagoCobroM(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "MX",
            //        numOrden.Text,"9",txtNumTarjeta.Text,txtCode.Text,importe.Text,"MONEDA","");
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
            //            cmdvoucher.IsEnabled = true;
            //            cmdvoucher.Visibility = Visibility.Hidden;
            //            importe.IsEnabled = false;
            //            txtNumTarjeta.IsEnabled = false;
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
            //            txtNumTarjeta.IsEnabled = true;
            //            break;
            //        default:
            //            lblAprob.Visibility = Visibility.Hidden;
            //            lblAuth.Visibility = Visibility.Hidden;
            //            lblTInfo.Visibility = Visibility.Visible;
            //            lblTInfo.Text = "Verifique su conexión de internet.";
            //            cmdActual(cmdLeer);
            //            break;
            //    }
            //    lblTarjeta.Content = "Saldo Actual: " + Globales.PPOperacion.getRspBalance();
            //}
            //catch
            //{
            //    Mouse.OverrideCursor = null;
            //}
            //Mouse.OverrideCursor = null;
        }

        private void txtNumTarjeta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender,e);
        }

        private void txtCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender,e);
        }

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
