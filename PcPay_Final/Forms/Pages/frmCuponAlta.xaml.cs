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
    /// Interaction logic for frmCuponAlta.xaml
    /// </summary>
    public partial class frmCuponAlta : Page
    {
        private string mensaje = string.Empty;
        public frmCuponAlta()
        {
            InitializeComponent();
            this.txtTelefono.GotFocus += Globales.setFocusMit2;
            this.txtTelefono.LostFocus += Globales.lostFocusMit2;

        }

        private void cmdSalir_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
        }
        public cerrarVentana cerrar;

        private void cmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            limpia();
            txtTelefono.Focus();
        }

        private void limpia()
        {
            txtTelefono.Text = "";
            txtTelefono.IsEnabled = true;

            cmdAlta.Visibility = Visibility.Visible;
            cmdAlta.IsEnabled = true;

            cmdNuevo.IsEnabled = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            limpia();

            Mouse.OverrideCursor = null;

            txtTelefono.Focus();
            txtMail2.Visibility = Visibility.Hidden;
            lblEmail.Visibility = Visibility.Hidden;
            return;
        }

        private void cmdAlta_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(txtTelefono.Text))
                {
                    Globales.MessageBoxMit("Introduzca número Celular");
                    txtTelefono.Focus();
                    return;
                }
                if (txtTelefono.Text.Length < 10)
                {
                    Globales.MessageBoxMit("Número celular incompleto");
                    txtTelefono.Focus();
                    return;
                }
                cmdAlta.Visibility = Visibility.Hidden;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                txtTelefono.IsEnabled = false;
                cmdNuevo.IsEnabled = true;
                respuesta = Globales.cpIntegraEMV.sndPayNoPayAltaCliente(TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country,
                    TypeUsuario.usu, TypeUsuario.Pass, txtTelefono.Text);

                Mouse.OverrideCursor = null;
                if (Globales.GetDataXml("nb_status", respuesta).ToUpper() == "SUCCESS")
                {
                    mensaje = Globales.GetDataXml("respuesta", respuesta);
                    ;
                    Globales.MessageBoxMitApproved(mensaje, "Alta exitosa");
                }
                else
                {
                    StringBuilder mensaje = new StringBuilder();
                    mensaje.AppendLine("Error en el Servicio, intente nuevamente.");

                    if (!string.IsNullOrWhiteSpace(respuesta))
                        mensaje.AppendLine(Globales.GetDataXml("nb_error", respuesta));
                    Globales.MessageBoxMitError(mensaje.ToString(), "Lo sentimos");
                }
            }
            catch
            {

            }
            Mouse.OverrideCursor = null;
        }

        public string respuesta { get; set; }

        private void txtTelefono_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }
    }
}
