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
    /// Interaction logic for frmCorteRecompensas.xaml
    /// </summary>
    /// 
    public partial class frmCorteRecompensas : Page
    {
        public cerrarVentana cerrar;
        public frmCorteRecompensas()
        {
            InitializeComponent();
            this.fraP1.Visibility = Visibility.Hidden;
        }


        private void cmdAceptarRecom_Click(object sender, RoutedEventArgs e)
        {
            string tipoReporte = "";
            if (Convert.ToBoolean(optReservacion.IsChecked))
            {
                tipoReporte = "I02";
            }
            else
            {
                tipoReporte = "I01";
            }
            cmdAceptar1.IsEnabled = false;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (TypeUsuario.TipoImpresora == "6")
            {
                Globales.cpHTTP_cadena1 = Globales.cpIntegraEMV.GetCorteRecompensas(Globales.cpIntegraEMV.dbgGetId_Company(), Globales.cpIntegraEMV.dbgGetId_Branch(), Globales.cpIntegraEMV.dbgGetCountry(),
                    Globales.cpIntegraEMV.dbgGetUser(), Globales.cpIntegraEMV.dbgGetPass(), tipoReporte, "impreso");
            }
            else
            {
                Globales.cpHTTP_cadena1 = Globales.cpIntegraEMV.GetCorteRecompensas(Globales.cpIntegraEMV.dbgGetId_Company(), Globales.cpIntegraEMV.dbgGetId_Branch(), Globales.cpIntegraEMV.dbgGetCountry(),
                    Globales.cpIntegraEMV.dbgGetUser(), Globales.cpIntegraEMV.dbgGetPass(), tipoReporte, "html");
            }
            escogerImpresora();
            Mouse.OverrideCursor = null;
            cerrar();
        }

        private void escogerImpresora()
        {
            switch (TypeUsuario.TipoImpresora)
            {
                case "1":
                    Globales.Imprimir_HTML(Globales.cpHTTP_cadena1, true);
                    break;
                case "2":
                    break;
                case "3":
                    Globales.imprimirEpson();
                    break;
                case "4":
                    Globales.Imprimir_HTML(Globales.cpHTTP_cadena1, true);
                    break;
                case "5":
                    break;
                case "6":
                    Globales.ImprimeCorteRecomensas(Globales.cpHTTP_cadena1);
                    break;
                default:
                    break;
            }
        }

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
          //s  this.LMENSAJE.Content = "Presentará las transacciones del cierre del corte anterior hasta este momento";
        }

        private void optReservacion_Checked(object sender, RoutedEventArgs e)
        {
          //  this.LMENSAJE.Content = "Imprimirá el último corte realizado.";
        }
    }
}
