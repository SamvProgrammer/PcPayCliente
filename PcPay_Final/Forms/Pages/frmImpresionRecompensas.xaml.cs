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
    /// Interaction logic for frmImpresionRecompensas.xaml
    /// </summary>
    public partial class frmImpresionRecompensas : Page
    {
        public cerrarVentana cerrar;
        public frmImpresionRecompensas()
        {
            InitializeComponent();
            this.txtReservacion.GotFocus += Globales.setFocusMit2;
            this.txtReservacion.LostFocus += Globales.lostFocusMit2;

            this.GRESERVACION.Visibility = Visibility.Hidden;
            this.LNUM.Visibility = Visibility.Hidden;
        }

         private void cmdAceptarRecom_Click(object sender, RoutedEventArgs e)
        {
            string opcionImp = "reimpticket";
            if (Convert.ToBoolean(optFolio.IsChecked))
            {
                if (string.IsNullOrWhiteSpace(txtReservacion.Text))
                {
                    Globales.MessageBoxMit("Introduzca el No. de transacción");
                    txtReservacion.Focus();
                    return;
                }
            }

            strFolioReImpresion = ((bool)this.optReservacion.IsChecked ? "" : this.txtReservacion.Text);
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            if (TypeUsuario.TipoImpresora == "6")
            {
                Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                TypeUsuario.strVoucherCoP = Globales.cpIntegraEMV.sndReimpresionRecompensas(
                      TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country, strFolioReImpresion
                    );
            }
            else
                Globales.cpHTTP_cadena1 = Globales.cpIntegraEMV.GetVoucherRecompensas(Globales.cpIntegraEMV.dbgGetId_Company(), Globales.cpIntegraEMV.dbgGetId_Branch(),
                    Globales.cpIntegraEMV.dbgGetCountry(), Globales.cpIntegraEMV.dbgGetUser(), Globales.cpIntegraEMV.dbgGetPass(), opcionImp, strFolioReImpresion);

            this.escogerImpresora();

            if (TypeUsuario.IsAQ)
            {
                Globales.VerificaVoucher();
            }
            Mouse.OverrideCursor = null;
            cerrar();
        }

        private void escogerImpresora()
        {
            switch (TypeUsuario.TipoImpresora)
            {
                case "1":
                    Globales.Imprimir_HTML(Globales.cpHTTP_cadena1, false);
                    break;
                case "2":
                    //Impresora térmica....
                    break;
                case "3":
                    break;
                case "4":
                    Globales.Imprimir_HTML(Globales.cpHTTP_cadena1, false);
                    break;
                case "5":
                    break;
                case "6":
                    string Rsp = "";
                    Rsp = Globales.cpIntegraEMV.getRspDsResponse();
                    switch (Rsp)
                    {
                        case "error":
                            Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspDsError());
                            break;
                        case "denied":
                            Globales.MessageBoxMit("No es posible realizar la reimpresión");
                            break;
                        case "approved":
                            Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public string strFolioImpresion { get; set; }

        public string strFolioReImpresion { get; set; }

        private void cmdCancelarRecom_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void optFolio_Checked(object sender, RoutedEventArgs e)
        {
            this.GRESERVACION.Visibility = Visibility.Visible;
            this.LNUM.Visibility = Visibility.Visible;
            this.txtReservacion.Text = string.Empty;
        }

        private void optFolio_Unchecked(object sender, RoutedEventArgs e)
        {
            this.GRESERVACION.Visibility = Visibility.Hidden;
            this.LNUM.Visibility = Visibility.Hidden;
            this.txtReservacion.Text = string.Empty;
        }

        private void optReservacion_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void txtReservacion_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }
    }
}
