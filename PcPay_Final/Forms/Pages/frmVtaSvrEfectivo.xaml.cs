using PcPay.Code.Usuario;
using PcPay.Forms.formularios;
using PcPay.Forms.Formularios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for frmVtaSvrEfectivo.xaml
    /// </summary>
    public partial class frmVtaSvrEfectivo : Page
    {

       public string idCategoria = string.Empty;
        public string idproducto = "";
        public string idProveedor = "";
        public string st_capt_tel_imp = "";
        public abrirFrm abriFrmNuevo;
        public frmVtaSvrEfectivo()
        {
            InitializeComponent();
        }

        private void cargandoVentana(object sender, RoutedEventArgs e)
        {
            if(TypeUsuario.ShowMsg){
                frmAvisoBanda banda = new frmAvisoBanda();
                banda.ShowDialog();
            }
            if(st_capt_tel_imp == "1"){
                lblReferencia.Content = "Número telefónico";
                importe.IsEnabled = true;
                txtReferencia.IsEnabled = true;
                importe.IsEnabled = true;
                lbl044.Visibility = Visibility.Visible;
            }
            else if (st_capt_tel_imp == "2")
            {
                lblReferencia.Content = "Número telefónico";
                txtReferencia.IsEnabled = true;
                txtReferencia.MaxLength = 10;
                importe.Text = Globales.VServicios.getRspVsImporte();

                lbl044.Visibility = Visibility.Visible;
            }
            else {
                importe.Text = Globales.VServicios.getRspVsReferencia();
                lbl044.Visibility = Visibility.Hidden;
            }
        }

        private void importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender,e);
        }

        private void importe_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender,e);
        }

        private void txtReferencia_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (st_capt_tel_imp == "1" || st_capt_tel_imp == "2")
            {
                Globales.soloNumero(sender, e);
            }
            else {
                Globales.soloTNumeroTexto(sender,e);
            }
        }

        private void txtReferencia_LostFocus(object sender, RoutedEventArgs e)
        {
            if (st_capt_tel_imp == "1" || st_capt_tel_imp == "2")
            {
              if(txtReferencia.Text.Length < 10){
                  Globales.MessageBoxMit("La longitus debe ser mayor de 10 dígitos");
                  return;
              }

              string tmpRef = "";
              tmpRef = txtReferencia.Text;
              frmConfirmaTel confirma = new frmConfirmaTel();
              confirma.ShowDialog();
              string confnum = confirma.GetConfirmacionTel();
                if(confnum != tmpRef){
                    Globales.MessageBoxMit("El número no concide vuelva a ingresarlo");
                    txtReferencia.Text = tmpRef;
                    txtReferencia.Focus();
                    return;
                }
                txtReferencia.Text = tmpRef;


            }

            txtReferencia.Text = txtReferencia.Text.ToUpper().Trim();
        }

        private void cmdVoucher_Click(object sender, RoutedEventArgs e)
        {
            //cmdVoucher.IsEnabled = false;
            //cmdNuevo.IsEnabled = false;
            //if (Globales.TiempoAire.chkPp_Printer == "1")
            //{
            //    Globales.TiempoAire.dbgPrintVoucher();
            //}
            //else {
            //    Globales.PrintOptions(Globales.TiempoAire.getRspVoucher(),Globales.TiempoAire.getRspOperationNumber());
            //    Thread.Sleep(4000);
            //    Globales.PrintOptions2(Globales.TiempoAire.getRspTicket(),strNumOpe);
            //}

            //cmdVoucher.IsEnabled = true;
            //cmdNuevo.IsEnabled = true;

        }

        public string strNumOpe { get; set; }

        private void cmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            voucherTrx = "";
            
            abriFrmNuevo(new frmVtaSrvSeleccionar(),"Productos");
        }

        public string voucherTrx { get; set; }
        public cerrarVentana cerrando;
        public abrirFrm abrir;

        private void cmdEnviar_Click(object sender, RoutedEventArgs e)
        {
            cmdEnviar.IsEnabled = false;
            enviar();
        }

        private void enviar()
        {
            //lblInfo.Text = "";
            //cmdEnviar.IsEnabled = false;
            //importe.IsEnabled = false;
            //txtReferencia.IsEnabled = false;
            //Globales.TiempoAire.dbgSetUrl(Globales.URL_VTASERV);
            //if (st_capt_tel_imp == "1" || st_capt_tel_imp == "2")
            //{
            //    Globales.TiempoAire.sndVtaTiempoAire(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country,
            //        "9", "MXN", "", txtReferencia.Text, txtReferencia.Text, idProveedor, idCategoria, idproducto, true, "");
            //}//En el caso de lo que se venda sea un servicio.
            //else {
            //    Globales.TiempoAire.sndVentaServicios(TypeUsuario.usu,TypeUsuario.Pass,TypeUsuario.Id_Company,TypeUsuario.Id_Branch,TypeUsuario.country,"9","MXN","",
            //        idProveedor,idCategoria,idproducto);
            //}
            //Globales.csvAmexenBanda = "";
            //switch(Globales.TiempoAire.getRspDsResponse()){
            //    case "approved":
            //        lblInfo.Visibility = Visibility.Hidden;
            //        lblDenied.Visibility = Visibility.Hidden;
            //        lblAprob.Visibility = Visibility.Visible;

            //        strNumOpe = Globales.TiempoAire.getRspOperationNumber();
            //        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            //        voucherTrx = Globales.TiempoAire.getRspVoucher();

            //        if (!Globales.TiempoAire.dbgSetReader())
            //        {
            //            Globales.PrintOptions(Globales.TiempoAire.getRspVoucher(), Globales.TiempoAire.getRspOperationNumber());
            //            Thread.Sleep(4000);
            //            Globales.PrintOptions2(Globales.TiempoAire.getRspTicket(), strNumOpe);
            //        }
            //        else {
            //            if (Globales.TiempoAire.chkPp_Printer == "1")
            //            {
            //                Globales.TiempoAire.dbgPrintVoucher();
            //            }
            //            else {
            //                Globales.PrintOptions(Globales.TiempoAire.getRspVoucher(), Globales.TiempoAire.getRspOperationNumber());
            //                Thread.Sleep(2000);
            //                Globales.PrintOptions2(Globales.TiempoAire.getRspTicket(),strNumOpe);

            //            }
            //        }

            //        cmdVoucher.IsEnabled = true;
            //        cmdVoucher.Visibility = Visibility.Visible;
            //        cmdActual(cmdNuevo);
            //        return;
            //        break;
            //    case "denied":
            //        lblAprob.Visibility = Visibility.Hidden;
            //        lblAprob.Visibility = Visibility.Hidden;
            //        lblInfo.Visibility = Visibility.Hidden;

            //        lblDenied.Text = Globales.TiempoAire.getRspCdResponse();
            //        lblDenied.Visibility = Visibility.Visible;
            //        cmdActual(cmdEnviar);
            //        break;
            //    case "error":
            //        lblAprob.Visibility = Visibility.Hidden;
            //        lblAuth.Visibility = Visibility.Hidden;
            //        lblInfo.Visibility = Visibility.Visible;
            //        lblInfo.Text = Globales.TiempoAire.getRspDsError();
            //        lblDenied.Visibility = Visibility.Hidden;
            //        cmdActual(cmdEnviar);
            //        break;
            //    default:
            //        lblAprob.Visibility = Visibility.Hidden;
            //        lblAuth.Visibility = Visibility.Hidden;
            //        lblInfo.Visibility = Visibility.Visible;
            //        lblInfo.Text = "Verifique su conexion de internet.";
            //        cmdActual(cmdEnviar);
            //        break;
            //}

            //Mouse.OverrideCursor = null;
        }

        private void cmdActual(Button cmd)
        {
            cmdEnviar.IsEnabled = true;
            cmdNuevo.IsEnabled = true;
            cmdEnviar.Visibility = Visibility.Hidden;
            cmdNuevo.Visibility = Visibility.Hidden;
            cmd.Visibility = Visibility.Visible;
            cmd.IsEnabled = true;
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender,e);
        }
    }
}
