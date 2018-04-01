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
    /// Interaction logic for frmGiftACT.xaml
    /// </summary>
    public partial class frmGiftACT : Page
    {
        private string voucherGC { get; set; }
        private string gcTracks { get; set; }
        public cerrarVentana cerrando;
        public frmGiftACT()
        {
            InitializeComponent();
            numOrden.GotFocus += Globales.setFocusMit2;
            numOrden.LostFocus += Globales.lostFocusMit2;
            importe.GotFocus += Globales.setFocusMit2;
            
            importe.LostFocus += Globales.lostFocusMit2;
            importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;
        }

        private void letrasNumeros(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }

        private void numero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender,e);

        }

        private void cargandoVentana(object sender, RoutedEventArgs e)
        {
            try
            {
                cmdActual(cmdLeer);
                limpia();
                //lblCard.Text = "Introduzca datso y haga clic en el botón Activar";
                Globales.cpIntegraEMV.dbgEndOperation();
                //if(!Globales.PPOperacion.dbgSetLector()){
                //    Globales.mensajeAlerta("Imposible conectarse con el lector, configure su lector y vuelva a intentarlo");
                //    cerrando();
                //}
            }
            catch { 
               
            }
        }

        private void limpia()
        {
            //importe.Text = "";
            //lblTInfo.Text = "";
            //lblAuth.Text = "";
            //cmdVoucher.IsEnabled = false;
            //lblAprob.Visibility = Visibility.Hidden;
            //lblAuth.Visibility = Visibility.Hidden;
            //lblDenied.Visibility = Visibility.Hidden;
            //lblTInfo.Visibility = Visibility.Visible;
            //cmdNuevo.Visibility = Visibility.Hidden;
            //gcTracks = "";
        }

        private void cmdActual(Button cmd)
        {
            cmdLeer.IsEnabled = true;
            cmdNuevo.IsEnabled = true;
            cmdLeer.Visibility = Visibility.Hidden;
            cmdNuevo.Visibility = Visibility.Hidden;
            cmd.Visibility = Visibility.Visible;
            cmd.IsEnabled = true;
        }

        private void cerrandoVentana(object sender, RoutedEventArgs e)
        {
           // Globales.PPOperacion.dbgCancelOperation();
        }

        private void cmdNuevoClick(object sender, RoutedEventArgs e)
        {
            cerrandoVentana(null,null);
        }

        private void cmdLeerClick(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(numOrden.Text)){
                Globales.MessageBoxMit("Introduzca referencia");
                numOrden.Focus();
                return;
            }

            if(string.IsNullOrWhiteSpace(importe.Text)){
                Globales.MessageBoxMit("Introduzca el importe");
                importe.Focus();
                return;
            }
            ReadGiftCard();
        }

        private void ReadGiftCard()
        {
            try
            {
                //importe.IsEnabled = false;
                //numOrden.IsEnabled = false;
                //lblTInfo.Text = "";
                //lblAprob.Visibility = Visibility.Hidden;
                //lblAuth.Visibility = Visibility.Hidden;
                //lblDenied.Visibility = Visibility.Hidden;
                //cmdLeer.IsEnabled = false;
                //lblCard.Text = "Deslice mitTarjeta";
                //Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                //lblCard.Text = "Procesando transacción espere un momento.";
                //Globales.PPOperacion.dbgSetUrl(Globales.URL_GIFT);
                //Globales.PPOperacion.setTrxAmount(importe.Text);
                //if (Globales.PPOperacion.dbgActivaLector())
                //{
                //    if (Globales.PPOperacion.getPPTracks() != "")
                //    {
                //        gcTracks = Globales.PPOperacion.getPPTracks();
                //        Globales.PPOperacion.sndPrePagoActivacion(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "MX", numOrden.Text, "11", gcTracks, importe.Text, "MONEDA", "USR");
                //        Mouse.OverrideCursor = null;
                //        switch (""/*Globales.PPOperacion.getRspDsResponse()*/)
                //        {
                //            case "approved":
                //                voucherGC = "";// Globales.PPOperacion.getRspTicket();
                //                voucherGift();
                //                lblAprob.Visibility = Visibility.Visible;
                //                lblAuth.Visibility = Visibility.Visible;
                //                lblAuth.Text = "Aut.\n" + Globales.PPOperacion.getRspCdActivacion();
                //                lblTInfo.Visibility = Visibility.Hidden;
                //                lblDenied.Visibility = Visibility.Hidden;
                //                cmdVoucher.IsEnabled = true;
                //                cmdVoucher.Visibility = Visibility.Visible;
                //                numOrden.IsEnabled = false;
                //                importe.IsEnabled = false;
                //                cmdActual(cmdNuevo);
                //                break;
                //            case "denied":
                //                lblAprob.Visibility = Visibility.Hidden;
                //                lblAuth.Visibility = Visibility.Hidden;
                //                lblDenied.Visibility = Visibility.Visible;
                //                lblTInfo.Visibility = Visibility.Hidden;
                //                cmdActual(cmdLeer);
                //                break;
                //            case "error":
                //                lblAprob.Visibility = Visibility.Hidden;
                //                lblAuth.Visibility = Visibility.Hidden;
                //                lblDenied.Visibility = Visibility.Hidden;
                //                lblTInfo.Visibility = Visibility.Visible;
                //                lblTInfo.Text = Globales.PPOperacion.getRspDsError();
                //                cmdActual(cmdLeer);
                //                importe.IsEnabled = true;
                //                break;
                //            default:
                //                lblAprob.Visibility = Visibility.Hidden;
                //                lblAuth.Visibility = Visibility.Hidden;
                //                lblTInfo.Visibility = Visibility.Visible;
                //                lblTInfo.Text = "Verifique su conexión de internet...";
                //                cmdActual(cmdLeer);
                //                break;
                //        }
                //        lblCard.Text = "Saldo Actual: " + Globales.PPOperacion.getRspBalance();
                //    }
                //}
            }
            catch {
                Mouse.OverrideCursor = null;
            }
            Mouse.OverrideCursor = null;
        }

        private void voucherGift()
        {
            TypeUsuario.strVoucherCoP = voucherGC;
            if(TypeUsuario.TipoImpresora == "6" && TypeUsuario.strTipoLector == "3"){
                //if (!Globales.PPOperacion.dbgPrintTicket(voucherGC))
                //{
                //    Globales.MessageBoxMit("Imposible de imprimir, verifique impresora.");
                //}
            }
            switch(TypeUsuario.TipoImpresora){
                case "1":                    
                    break;
                case "4":
                    break;
                case "6":
                    break;
                default:
                    Globales.MessageBoxMit("Seleccione un tipo de impresora en el menu de administración..");
                    break;
            }
        }

        private void cmdVoucher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                voucherGift();
            }
            catch { 
            
            }
        }

        private void cmdLeer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            cargandoVentana(null,null);
        }

        private void importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender,e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }
    }
}
