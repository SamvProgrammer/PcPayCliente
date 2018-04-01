using PcPay.Code.Configuracion;
using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using PcPay.Forms.formularios;
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
    /// Lógica de interacción para frmToken.xaml
    /// </summary>
    public partial class frmToken : Page
    {
        public frmToken()
        {
            InitializeComponent();
            this.txtReferencia.GotFocus += Globales.setFocusMit2;
            this.txtReferencia.LostFocus += Globales.lostFocusMit2;
            lenghtTarjeta = "16";
        }
        public string NOMBRE_GENERAL { get; set; }

        public string TDCOriginal { get; set; }

        public string lenghtTarjeta { get; set; }
        public string respuesta { get; set; }

        private void btnObtener_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".btnObtener_Click";
            string Mensaje = string.Empty;
            string respuesta = string.Empty;
            Mensaje = "Digite el numero de Tarjeta";
            string tarjetaHEX = "";
            string tarjetaMascara = "";
            if (string.IsNullOrWhiteSpace(txtReferencia.Text))
            {
                Globales.MessageBoxMit("Falta Referencia");
                txtReferencia.Focus();
                return;
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globales.cpIntegraEMV.dbgSetUrl(Program.ipKeyWeb);
            respuesta = Globales.cpIntegraEMV.dbgSetCmd62(Mensaje, lenghtTarjeta);
            
            if(string.IsNullOrWhiteSpace(respuesta)){
                Globales.MessageBoxMit("No se pudo obtener el número de tarjeta.");
                return;
            }


            Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
            Mouse.OverrideCursor = null;
            if (respuesta == "0006P83E180/" || respuesta == "104" || respuesta == "CANCELADO")
            {
                txtNumTDC.Text = "CANCELADO";
                goto Cancel;
            }

            //int posSeparador;
            //posSeparador = respuesta.IndexOf("|") + 1;

            var arrayRespuesta = respuesta.Split('|').ToArray();
            tarjetaHEX = arrayRespuesta[0];
            tarjetaMascara = arrayRespuesta[1];
            txtNumTDC.Text = tarjetaMascara;
            Thread.Sleep(1000);
        Cancel:
            if (txtNumTDC.Text == "CANCELADO")
            {
                Globales.MessageBoxMit("El proceso fue cancelado.");
                txtReferencia.Text = "";
                txtReferencia.Focus();
                Mouse.OverrideCursor = null;
                txtNumTDC.Text = "";
                return;
            }
            //  'tarjeta original
            TDCOriginal = tarjetaHEX;
            txtNumTDC.Text = tarjetaMascara;
            Mouse.OverrideCursor = null;
            //   'En tarjetas AMEX trae un caracter extra
            if (!string.IsNullOrWhiteSpace(txtNumTDC.Text))
            {
                btnObtener.IsEnabled = false;
                btnToken.IsEnabled = true;
                txtReferencia.IsEnabled = false;
            }

        }



        private void btnToken_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".btnToken_Click";
            string Msj = string.Empty;

            if (string.IsNullOrWhiteSpace(txtNumTDC.Text))
            {
                Globales.MessageBoxMit("Falta Número de Tarjeta");
                limpia();
                return;
            }

            btnObtener.IsEnabled = false;
            btnToken.IsEnabled = false;
            btnNuevo.IsEnabled = true;

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            string Referencia = string.Empty;
            Referencia = txtReferencia.Text;
            string textDecode = string.Empty;

            textDecode = Globales.cpIntegraEMV.ConvierteHEX_To_BASE64(TDCOriginal);
            respuesta = Globales.cpIntegraEMV.sndObtieneToken(TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
                                                            TypeUsuario.usu, TypeUsuario.Pass, textDecode, Referencia);
            Mouse.OverrideCursor = null;

            if (Globales.GetDataXml("nbResponse", respuesta).ToUpper() == "SUCCESS")
            {
                string token = string.Empty;
                token = Globales.GetDataXml("token", respuesta);
                frmTokenValido frmToken = new frmTokenValido(token);
                frmToken.strToken = token;
                //Globales.MessageBoxMitApproved("Token válido");
                frmToken.ShowDialog();
            }
            else
            {

                if (string.IsNullOrWhiteSpace(respuesta))
                    Globales.MessageBoxMitError("Error en el Servicio, intente nuevamente.");
                else
                    Globales.MessageBoxMitError(Globales.GetDataXml("nbResponse", respuesta));
            }
        }

        private void limpia()
        {

            btnObtener.IsEnabled = true;
            btnToken.IsEnabled = false;
            btnNuevo.IsEnabled = false;
            txtNumTDC.Text = "";

            optVMC.IsChecked = true;
            optAmex.IsChecked = false;
            lenghtTarjeta = "16";

            txtReferencia.IsEnabled = true;
            txtReferencia.Focus();
            txtReferencia.Text = "";
            this.txtReferencia.IsEnabled = true;
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".btnNuevo_Click";
            this.limpia();
            lenghtTarjeta = "16";
            optVMC.IsChecked = true;
        }


        private void Cargar()
        {

            Globales.strNombreFP = NOMBRE_GENERAL + ".Form_Load()";
            lenghtTarjeta = "16";
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void txtReferencia_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }

        private void SELECCION(object sender, RoutedEventArgs e)
        {
            if(Convert.ToBoolean(optVMC.IsChecked)){
                lenghtTarjeta = "16";
            }
            else if (Convert.ToBoolean(optAmex.IsChecked))
            {
                lenghtTarjeta = "15";
            }
            else {
                lenghtTarjeta = "14";
            }
        }
    }
}
