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
    /// Interaction logic for frmGiftCancelacion.xaml
    /// </summary>
    public partial class frmGiftCancelacion : Page
    {
        public cerrarVentana cerrar;
        public frmGiftCancelacion()
        {
            InitializeComponent();
            txtNumOper.GotFocus += Globales.setFocusMit2;
            txtNumOper.LostFocus += Globales.lostFocusMit2;
            importe.GotFocus += Globales.setFocusMit2;

            importe.LostFocus += Globales.lostFocusMit2;
            importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
                       
        }

        private void cmdNuevo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
                
        }

        private void importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender,e);
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

            //    else { 
            //       switch(TypeUsuario.TipoImpresora.Substring(0,1)){
            //           case "1":
            //               break;
            //           case "2":
            //               break;
            //           case "3":
            //               break;
            //           case "4":
            //               break;
            //           case "5":
            //               break;
            //           default:
            //               break;
            //       }
            //    }
            //}
            //catch
            //{
            //    Mouse.OverrideCursor = null;
            //}
            //Mouse.OverrideCursor = null;
        }

        public string VoucherGC { get; set; }
        public string gcTracks { get; set; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //menu7_0.enable = false;
            //try
            //{
            //    cmdActual(cmdLeer);
            //    limpia();
            //    lblCard.Content = "Introduzca datos y haga clic en el botón Enviar.";
            //    Globales.cpIntegraEMV.dbgEndOperation();
            //    if(!Globales.PPOperacion.dbgSetLector()){
            //        Globales.mensajeAlerta("Imposible conectarse con el lector, configure su lector y vuelva a intentarlo");
            //        cerrar();
            //        return;
            //    }

            //    //Menu 7 = true;
            //}
            //catch { 
            
            //}
        }

        private void limpia()
        {
            //importe.Text = "";
            //lblTInfo.Text = "";
            //lblAuth.Text = "";
            //txtNumOper.Text = "";
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

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Globales.cpIntegraEMV.dbgEndOperation();
        }

        private void cmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            Page_Loaded(null,null);        
        }

        private void cmdLeer_Click(object sender, RoutedEventArgs e)
        {
            //if(string.IsNullOrWhiteSpace(txtNumOper.Text)){
            //    Globales.mensajeAlerta("Introduzca el número de operacion");
            //    txtNumOper.Focus();
            //    return;
            //}else if(string.IsNullOrWhiteSpace(importe.Text)){
            //    Globales.mensajeAlerta("Introduzca el importe.");
            //    importe.Focus();
            //    return;
            //}else if(txtNumOper.Text.Length != 9){
            //    Globales.mensajeAlerta("El número de operación debe ser de 9 dígitos.");
            //    txtNumOper.Focus();
            //    return;
            //}

            //readGiftCard();
        }

        private void readGiftCard()
        {
            //try
            //{
            //    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            //    importe.IsEnabled = false;
            //    txtNumOper.IsEnabled = false;
            //    lblAprob.Visibility = Visibility.Hidden;
            //    lblAuth.Visibility = Visibility.Hidden;
            //    lblDenied.Visibility = Visibility.Hidden;
            //    cmdLeer.IsEnabled = false;
            //    lblCard.Content = "Deslice MItTarjeta";

            //    lblCard.Content = "Procesando Transacción, espere un momento...";
            //    Globales.PPOperacion.dbgSetUrl(Globales.URL_GIFT);
            //    Globales.PPOperacion.setTrxAmount(importe.Text);
            //    if (Globales.PPOperacion.dbgActivaLector())
            //    {
            //        gcTracks = Globales.PPOperacion.getPPTracks();
            //        Globales.PPOperacion.sndPrePagoCancelacion(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
            //            "MX", "11", gcTracks, importe.Text, txtNumOper.Text, "MONEDA", "");

            //        Mouse.OverrideCursor = null;
            //        switch (Globales.PPOperacion.getRspTicket())
            //        {
            //            case "approved":
            //                VoucherGC = Globales.PPOperacion.getRspTicket();
            //                VoucherGift();
            //                lblAprob.Visibility = Visibility.Visible;
            //                lblAuth.Visibility = Visibility.Visible;
            //                lblAuth.Text = "Auth.\n" + Globales.PPOperacion.getRspAuth();
            //                lblTInfo.Visibility = Visibility.Hidden;
            //                lblDenied.Visibility = Visibility.Hidden;
            //                cmdVoucher.IsEnabled = true;
            //                cmdVoucher.Visibility = Visibility.Hidden;
            //                importe.IsEnabled = false;
            //                txtNumOper.IsEnabled = false;
            //                cmdActual(cmdNuevo);
            //                break;
            //            case "denied":
            //                lblAprob.Visibility = Visibility.Hidden;
            //                lblAuth.Visibility = Visibility.Hidden;
            //                lblTInfo.Visibility = Visibility.Hidden;
            //                lblDenied.Visibility = Visibility.Visible;
            //                cmdActual(cmdLeer);
            //                break;
            //            case "error":
            //                lblAprob.Visibility = Visibility.Hidden;
            //                lblAuth.Visibility = Visibility.Hidden;
            //                lblTInfo.Visibility = Visibility.Visible;
            //                lblTInfo.Text = Globales.PPOperacion.getRspDsError();
            //                lblDenied.Visibility = Visibility.Hidden;
            //                cmdActual(cmdLeer);
            //                importe.IsEnabled = true;
            //                txtNumOper.IsEnabled = true;
            //                break;
            //            default:
            //                lblAprob.Visibility = Visibility.Hidden;
            //                lblAuth.Visibility = Visibility.Hidden;
            //                lblTInfo.Visibility = Visibility.Visible;
            //                lblTInfo.Text = "Verifique su conexión de internet.";
            //                cmdActual(cmdLeer);
            //                break;
            //        }
            //        lblCard.Content = "Saldo Actual: " + Globales.PPOperacion.getRspBalance();
            //    }
            //    else {
            //        if (!string.IsNullOrWhiteSpace(Globales.PPOperacion.chkPp_CdError) && Globales.PPOperacion.chkPp_CdError != "PPE01")
            //        {
            //            Globales.mensajeAlerta(Globales.PPOperacion.chkPp_DsError);
            //            lblCard.Content = "";
            //            cmdActual(cmdLeer);
            //            cmdLeer.IsEnabled = true;
            //            Mouse.OverrideCursor = null;
            //        }
            //    }
            //}
            //catch {
            //    Mouse.OverrideCursor = null;
            //}
            //Mouse.OverrideCursor = null;
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }
    }
}
