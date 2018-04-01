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
    /// Interaction logic for frmVtaSrvBanda.xaml
    /// </summary>
    public partial class frmVtaSrvBanda : Page
    {
        private string VoucherTrx = string.Empty;
        public string idProducto = string.Empty;
        public string idCategoria = string.Empty;
        public string idProveedor = string.Empty;
        public string st_capt_tel_imp = string.Empty;

        public string strNumOpe = string.Empty;
        public string cPort = string.Empty;
        public abrirFrm abrirFrmNuevo;
        public frmVtaSrvBanda()
        {
            InitializeComponent();
        }

        private void cargandoVentana(object sender, RoutedEventArgs e)
        {
            try
            {
                string msg = string.Empty;
                
                //if(!(Globales.TiempoAire.dbgSetReader())){
                //    Globales.MessageBoxMit("Imposible establecer comunicacion con el lector");
                    
                //    return;
                //}
                cboBanco = Globales.obtieneBancos(cboBanco,Globales.GetDataXml("catbancos",Globales.VServicios.getRspVsXML()));
                cboBanco.Focus();
                if(TypeUsuario.ShowMsg){
                    frmAvisoBanda frmAvisoBanda = new frmAvisoBanda();
                    frmAvisoBanda.ShowDialog();
                }
                if(st_capt_tel_imp == "1"){
                    lblReferencia.Content = "Número Telefónico";
                    importe.IsEnabled = true;
                    txtReferencia.IsEnabled = true;
                    txtReferencia.MaxLength = 10;
                    Thickness Margen = new Thickness();
                    Margen.Left = 41;
                    Margen.Right = 0;
                    Margen.Bottom = 0;
                    Margen.Top = 33;
                    txtReferencia.Margin = Margen;
                    lbl044.Visibility = Visibility.Visible;
                    if (!string.IsNullOrWhiteSpace(Globales.VServicios.getRspVsImporte()))
                    {
                        txtReferencia.Focus();
                        importe.IsEnabled = false;
                        importe.Text = Globales.VServicios.getRspVsImporte();
                    }

                }
                else if (st_capt_tel_imp == "2")
                {
                    lblReferencia.Content = "Número Telefónico";
                    txtReferencia.IsEnabled = true;
                    txtReferencia.MaxLength = 10;
                    importe.Text = Globales.VServicios.getRspVsImporte();
                    Thickness Margen = new Thickness();
                    Margen.Left = 41;
                    Margen.Right = 0;
                    Margen.Bottom = 0;
                    Margen.Top = 33;
                    txtReferencia.Margin = Margen;
                    lbl044.Visibility = Visibility.Visible;
                }
                else {
                    importe.Text = Globales.VServicios.getRspVsImporte();
                    txtReferencia.Text = Globales.VServicios.getRspVsReferencia();
                    lbl044.Visibility = Visibility.Hidden;
                }
            }
            catch { 
            
            }
            
        }

        private void cerrandoVentana(object sender, RoutedEventArgs e)
        {
           // Globales.TiempoAire.dbgCancelOperation();
        }

        private void cmdActivarLector_click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if(string.IsNullOrWhiteSpace(txtReferencia.Text)){
            //        Globales.MessageBoxMit("Introduzca una referencia para la activacion MIItTarjeta");
            //        return;
            //    }
            //    if (string.IsNullOrWhiteSpace(importe.Text))
            //    {
            //        Globales.MessageBoxMit("Introduzca el importe MITarjeta");
            //        importe.Focus();
            //        return;
            //    }
            //    formaPago.IsEnabled = false;
            //    cboBanco.IsEnabled = false;
            //    importe.IsEnabled = false;
            //    txtReferencia.IsEnabled = false;
            //    cmdLeer.IsEnabled = false;

            //    numTdc.Text = "";
            //    nomTdc.Text = "";
            //    mes.Text = "";
            //    anio.Text = "";

            //    lblInfo.Text = "Inserta el chip o desliza tarjeta y Espera un momento...\nSigue las intrucciones del lector.";
            //    Globales.TiempoAire.dbgSetUrl(Globales.URL_VTASERV);
            //    Globales.TiempoAire.setTrxAmount(importe.Text);
            //    if (Globales.TiempoAire.dbgActivaLector())
            //    {
            //        if (!string.IsNullOrWhiteSpace(Globales.TiempoAire.chkCc_Number))
            //        {
            //            numTdc.Text = Globales.TiempoAire.chkCc_Number;
            //            nomTdc.Text = Globales.TiempoAire.chkCc_Name;
            //            mes.Text = Globales.TiempoAire.chkCc_ExpMonth;
            //            anio.Text = Globales.TiempoAire.chkCc_ExpYear;
            //            if (Globales.GetDataXml("csvamexenbanda", TypeUsuario.CadenaXML) == "1" && Globales.TiempoAire.chkCc_Number.Length == 15)
            //            {
            //                frmCsvAMEX frmCsvAMEX = new frmCsvAMEX();
            //                frmCsvAMEX.ShowDialog();
            //            }
            //            cmdActual(ref cmdEnviar);
            //            cmdEnviar.Focus();
            //        }
            //    }
            //    else { 
            //        if(Globales.TiempoAire.getRspDsError() != ""){
            //            cmdActual(ref cmdLeer);
            //            lblInfo.Text = "Error = " + Globales.TiempoAire.getRspDsError()+" CodError = "+Globales.TiempoAire.getRspCdError();
            //        }
            //    }
            //}
            //catch { 
            
            //}
        }

        private void cmdActual(ref Button cmd)
        {
            cmdLeer.IsEnabled = true;
            cmdEnviar.IsEnabled = true;
            cmdNuevo.IsEnabled = true;
            cmdLeer.Visibility = Visibility.Hidden;
            cmdEnviar.Visibility = Visibility.Hidden;
            cmdNuevo.Visibility = Visibility.Hidden;
            cmd.Visibility = Visibility.Visible;
            cmd.IsEnabled = true;
        }

        private void cmdNuevo_click(object sender, RoutedEventArgs e)
        {
            lblMoneda.Content = "";
            VoucherTrx = "";
            frmVtaSrvSeleccionar seleccionar = new frmVtaSrvSeleccionar();
            seleccionar.abrirFrmNuevo = abrirFrmNuevo;
            abrirFrmNuevo(seleccionar);
        }

        private void cmdEnviarClick(object sender, RoutedEventArgs e)
        {
            cmdEnviar.IsEnabled = false;
            enviar();
        }

        private void enviar()
        {
            //Globales.strNombreFP = "frmGiftVenta.cmdEvniar_click";
            //try
            //{
            //    lblInfo.Text = "";
            //    cmdEnviar.IsEnabled = false;
            //    importe.IsEnabled = false;
            //    txtReferencia.IsEnabled = false;
            //    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            //    string strTypeC = string.Empty;
            //    Globales.cpIntegracion_Clear();
            //    switch(numTdc.Text.Substring(0,1)){
            //        case "3":
            //            strTypeC = "AMEX";
            //            break;
            //        default:
            //            strTypeC = "V/MC";
            //            break;
            //    }
            //    if (st_capt_tel_imp == "1" || st_capt_tel_imp == "2")
            //    {
            //        Globales.TiempoAire.sndVtaTiempoAire(TypeUsuario.usu,TypeUsuario.Pass,TypeUsuario.Id_Company,TypeUsuario.Id_Branch,TypeUsuario.country,"9","MXN",strTypeC,
            //            txtReferencia.Text,txtReferencia.Text,idProveedor,idCategoria,idProducto,false,Globales.csvAmexenBanda);
            //    }
            //    else {
            //        Globales.TiempoAire.sndVentaServicios(TypeUsuario.usu,TypeUsuario.Pass,TypeUsuario.Id_Company,TypeUsuario.Id_Branch,TypeUsuario.country,"9","MXN",strTypeC,
            //            idProveedor,idCategoria,idProducto);
            //    }
            //    Globales.csvAmexenBanda = "";
            //    var ghola = Globales.TiempoAire.getRspXML();
            //    string respuestaAux = Globales.TiempoAire.getRspDsResponse();
            //    switch (respuestaAux)
            //    {
            //        case "approved":
            //            imgMail.Visibility = Visibility.Hidden;
            //            if(TypeUsuario.enviaCorreo){
            //                imgMail.Visibility = Visibility.Visible;
            //            }
            //            lblInfo.Visibility = Visibility.Hidden;
            //            lblDenied.Visibility = Visibility.Hidden;
            //            lblAprob.Visibility = Visibility.Visible;
            //            lblAuth.Text = Globales.TiempoAire.getRspAuth();

            //            picResult.Visibility = Visibility.Visible;
            //            strNumOpe = Globales.TiempoAire.getRspOperationNumber();
            //            Mouse.OverrideCursor = null;
            //            VoucherTrx = Globales.TiempoAire.getRspVoucher();
            //            Globales.PrintOptionsTAE(Globales.TiempoAire.getRspVoucher(),Globales.TiempoAire.getRspOperationNumber(),"1",new PrintDialog());
            //            if(TypeUsuario.strTipoLector == "3" && TypeUsuario.TipoImpresora == "6"){
            //                Thread.Sleep(2000);
            //                Globales.PrintOptions2(Globales.TiempoAire.getRspTicket(),strNumOpe);
            //            }
            //            cmdVoucher.IsEnabled = true;
            //            cmdVoucher.Visibility = Visibility.Visible;
            //            cmdActual(ref cmdNuevo);
            //            return;
            //            break;
            //        case "denied":
            //            lblAprob.Visibility = Visibility.Hidden;
            //            lblAuth.Visibility = Visibility.Hidden;
            //            lblInfo.Visibility = Visibility.Hidden;
            //            lblDenied.Text = Globales.TiempoAire.getRspCdResponse()+" "+Globales.msjRech;
            //            lblDenied.Visibility = Visibility.Visible;
            //            cmdActual(ref cmdLeer);
            //            break;
            //        case "error":
            //            lblAprob.Visibility = Visibility.Hidden;
            //            lblAuth.Visibility = Visibility.Hidden;
            //            lblInfo.Visibility = Visibility.Visible;
            //            lblInfo.Text = Globales.TiempoAire.getRspDsError();
            //            lblDenied.Visibility = Visibility.Hidden;
            //            picResult.Visibility = Visibility.Visible;
            //            cboBanco.IsEnabled = true;
            //            formaPago.IsEnabled = true;
            //            cmdActual(ref cmdLeer);
            //            break;
            //        default:
            //            lblAprob.Visibility = Visibility.Hidden;
            //            lblAuth.Visibility = Visibility.Hidden;
            //            lblInfo.Visibility = Visibility.Visible;
            //            lblInfo.Text = "Verifique su conexión de internet.";
            //            cmdActual(ref cmdLeer);
            //            break;
            //    }
            //    picResult.Visibility = Visibility.Visible;
            //    Mouse.OverrideCursor = null;
            //}
            //catch {
            //    Mouse.OverrideCursor = null;
            //    Globales.MessageBoxMit(Globales.strNombreFP + " Error en la exception");
            //}
        }

        private void referenciaLostFocus(object sender, RoutedEventArgs e)
        {
            string confnum = string.Empty;
            string tmpRef = string.Empty;
            if(st_capt_tel_imp =="1" || st_capt_tel_imp == "2"){
               if(txtReferencia.Text.Length < 10){
                   
                   return;
               }
                tmpRef = string.Empty;
               tmpRef = txtReferencia.Text;
               txtReferencia.Text = "**********";
               frmConfirmaTel confirma = new frmConfirmaTel();
               confirma.ShowDialog();
               confnum = confirma.GetConfirmacionTel();
               if(confnum != tmpRef){
                   Globales.MessageBoxMit("El número no concide vuelva a ingresarlo");
                   txtReferencia.Text = tmpRef;
                   return;
               }
               txtReferencia.Text = tmpRef;
            }
            txtReferencia.Text = tmpRef;
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir(true);
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender,e);
        }
    }
}
