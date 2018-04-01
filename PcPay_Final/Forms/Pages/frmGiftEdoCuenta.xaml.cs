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
    /// Interaction logic for frmGiftEdoCuenta.xaml
    /// </summary>
    public partial class frmGiftEdoCuenta : Page
    {
        public cerrarVentana cerrar;
        public frmGiftEdoCuenta()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //      //menu7 enable = false;
            //    limpia();
            //   // lblCard.Content = "Haga clic en el botón leer tarjeta para realizar una consulta.";
            //    Globales.cpIntegraEMV.dbgEndOperation();
            //    if (!Globales.PPOperacion.dbgSetLector())
            //    {
            //        Globales.mensajeAlerta("Imposible conectarse con el lector, configure su lector y vuelva a intentalo");
            //        cerrar();
            //    }
            //    else {
            //        cmdActual(cmdLeer);
            //    }
            //    //menu7 = true;
            //}
            //catch { 
            
            //}
        }

        private void cmdActual(Button cmd)
        {
            cmdNuevo.IsEnabled = false;
            cmdNuevo.Visibility = Visibility.Hidden;

            cmdLeer.IsEnabled = true;
            cmd.Visibility = Visibility.Visible;
            cmd.IsEnabled = true;
        }

        private void limpia() {
            cmdVoucher.IsEnabled = false;
            cmdNuevo.Visibility = Visibility.Hidden;
            gcTracks = "";
        }

        public string gcTracks { get; set; }

        private void cmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            Page_Loaded(null,null);
        }

        private void cmdLeer_Click(object sender, RoutedEventArgs e)
        {
            cmdLeer.IsEnabled = false;
            readGiftCard();
        }

        private void readGiftCard()
        {
            //try
            //{
            //   // "Deslice MITarjeta...";
            //    picDetalle.Visibility = Visibility.Visible;
            //    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            //    Globales.PPOperacion.dbgSetUrl(Globales.URL_GIFT);
            //    if (Globales.PPOperacion.dbgActivaLector())
            //    {
            //        if (!string.IsNullOrWhiteSpace(Globales.PPOperacion.getPPTracks()))
            //        {
            //            gcTracks = Globales.PPOperacion.getPPTracks();
            //            Globales.PPOperacion.sndPrePagoBalance(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "MX",
            //                "Balance", "11", gcTracks, "MONEDA", "USUARIO");
            //            Mouse.OverrideCursor = null;

            //            switch (Globales.PPOperacion.getRspDsResponse())
            //            {
            //                case "approved":
            //                    picDetalle.Visibility = Visibility.Visible;
            //                    VoucherGC = Globales.PPOperacion.getRspTicket();
            //                    GCApproved();
            //                    VoucherGift();
            //                    cmdVoucher.IsEnabled = true;
            //                    cmdVoucher.Visibility = Visibility.Visible;
            //                    cmdActual(cmdNuevo);
            //                 //   lblCard.Content = "Saldo Actual: " + Globales.PPOperacion.getRspBalance();
            //                    return;
            //                    break;
            //                case "denied":
            //                    cmdActual(cmdNuevo);
            //                    Globales.mensajeAlerta("No hay información para mostrar.");
            //                    break;
            //                case "error":
            //                    cmdActual(cmdNuevo);
            //                    Globales.mensajeAlerta(Globales.PPOperacion.getRspDsError());
            //                    break;
            //                default:
            //                    cmdActual(cmdNuevo);
            //                  //  lblCard.Content = "Verifique su conexión de internet.";
            //                    break;
            //            }
            //            cmdActual(cmdLeer);
            //           // lblCard.Content = "Haga clic en el botón leer tarjeta para realiar una consulta.";
            //        }
            //    }
            //    else {
            //        if (!string.IsNullOrWhiteSpace(Globales.PPOperacion.chkPp_DsError))
            //        {
            //            Globales.mensajeAlerta(Globales.PPOperacion.chkPp_DsError);
            //            //lblCard.Content = "";
            //            cmdActual(cmdNuevo);
            //            Mouse.OverrideCursor = null;
            //           // lblCard.Content = "Haga clic en el bocón leer tarjeta para realizar una consulta.";
            //         }
            //    }
            //}
            //catch {
            //    Mouse.OverrideCursor = null;
            //}
            //Mouse.OverrideCursor = null;
        }

        private void GCApproved()
        {
            //try
            //{
            //    string[] aux = { "Fecha","Descripción","Importe"};
            //    foreach(string item in aux){
            //        DataGridTextColumn textoCabecera = new DataGridTextColumn();
            //        textoCabecera.Header = item;
            //        textoCabecera.Binding = new Binding(item);
            //        datos.Columns.Add(textoCabecera);
            //    }
            //    int i = 1;
            //    while(i < 6){
            //        datos.Items.Add(new prototipoDato() { 
            //           Fecha = Globales.GetDataXml("fecha",Globales.GetDataXml("trx"+i,Globales.PPOperacion.getRspXML())),
            //           Descripción = Globales.GetDataXml("description", Globales.GetDataXml("trx" + i, Globales.PPOperacion.getRspXML())),
            //           Importe ="$"+Globales.GetDataXml("amount", Globales.GetDataXml("trx" + i, Globales.PPOperacion.getRspXML()))
            //        });
            //        i++;
            //    }
            //}
            //catch { 
            
            //}
        }

        internal class prototipoDato {
            public string Fecha { get; set; }
            public string Descripción { get; set; }
            public string Importe { get; set; }
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

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }
    }
}
