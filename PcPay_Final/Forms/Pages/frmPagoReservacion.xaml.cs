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
    /// Lógica de interacción para frmPagoReservacion.xaml
    /// </summary>
    public partial class frmPagoReservacion : Page
    {
        public frmPagoReservacion()
        {
            InitializeComponent();
            Reservacion.GotFocus += Globales.setFocusMit2;
            Reservacion.LostFocus += Globales.lostFocusMit2;
        }

        public abrirFrm abrir;

        private void cmdContinuar_Click(object sender, RoutedEventArgs e)
        {
             Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL;
            
                if( Globales.consultaBoleto(Reservacion.Text.Trim())) {
                    frmBanda frm = new frmBanda();
                    frm.LTITULO.Content = "VENTA DE BOLETOS: Venta con presencia de tarjeta";
                    frm.BELIMINARBOLETO.Visibility = Visibility.Hidden;
                    abrir(frm);
                }
                else{
                    Globales.cpHTTP_sResult = "";
                }
        }

        private void Alfa(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

        private void Reservacion_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = "NOMBRE_GENERAL" + ".Reservacion_LostFocus";
            Reservacion.Text = Reservacion.Text.Trim().ToUpper();
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblReferencia.Content = TypeUsuario.reference;
        }
    }
}
