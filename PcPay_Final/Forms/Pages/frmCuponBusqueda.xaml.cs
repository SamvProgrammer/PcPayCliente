using PcPay.Code.Usuario;
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
    /// Interaction logic for frmCuponBusqueda.xaml
    /// </summary>
    public partial class frmCuponBusqueda : Page
    {

        public string strCodeCupon { get; set; }
        public bool isCuponMultiple { get; set; }

        public frmCuponBusqueda()
        {
            InitializeComponent();

            this.isCuponMultiple = false;

            this.frameCuponUnico.Visibility = Visibility.Hidden;
            this.frameError.Visibility = Visibility.Hidden;
            this.frameCuponMultiples.Visibility = Visibility.Hidden;

            this.txtTelefono.GotFocus += Globales.setFocusMit2;
            this.txtTelefono.LostFocus += Globales.lostFocusMit2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
        }
        public cerrarVentana cerrar { get; set; }
        public bool askRedimir(string cupon)
        {
            Forms.Formularios.MessagesW.frmCuponRedimir confirma = new Formularios.MessagesW.frmCuponRedimir(cupon, "Cupón válido");
            confirma.ShowDialog();
            return confirma.redimirCupon;
        }


        private void cmdValidar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string strCaption;
                if (string.IsNullOrWhiteSpace(txtTelefono.Text))
                {
                    Globales.MessageBoxMit(etiqueta);
                    txtTelefono.Focus();
                    return;
                }
                if (txtTelefono.Text.Length < 10)
                {
                    Globales.MessageBoxMit(etiqueta2);
                    txtTelefono.Focus();
                    return;
                }
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                respuesta = Globales.cpIntegraEMV.sndPayNoPayBusquedaCupon(TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country, TypeUsuario.usu, TypeUsuario.Pass, txtTelefono.Text);
#if DEBUG
              //  respuesta = PcPay.Properties.Resources.MULTICUPONES;
#endif



                Mouse.OverrideCursor = null;
                if (Globales.GetDataXml("nb_status", respuesta).ToUpper() == "SUCCESS")
                {
                    Globales.docXML = new System.Xml.XmlDocument();
                    Globales.docXML.LoadXml(respuesta);
                    Globales.nodeListXML = Globales.docXML.GetElementsByTagName("coupon");
                    if (Globales.nodeListXML.Count == 1)
                    {
                        strCodeCupon = Globales.nodeListXML.Item(0).ChildNodes[0].InnerText;
                        var descripcionCupon = Globales.nodeListXML.Item(0).ChildNodes[1].InnerText.Split('|').ToArray();
                        strCaption = string.Format("{0}\n{1}\nVigencia: {2}", descripcionCupon[0],
                            descripcionCupon[1],
                            Globales.nodeListXML.Item(0).ChildNodes[2].InnerText);

                        //Globales.MessageBoxMitApproved(strCaption, "Cupón válido", PcPay.MITWindowSize.Medium);
                        if (this.askRedimir(strCaption))
                            this.cmdRedimirCU.PerformClick();
                        else
                            this.cmdRegresarCU.PerformClick();
                    }
                    else
                    {
                        int codigoCupon = 0;
                        string respForm;
                        this.frameBuscar.Visibility = Visibility.Hidden;
                        Globales.cpIntegraEMV.ObtieneCupones(respuesta);
                        respForm = Globales.cpIntegraEMV.RespuestaFormCupon();
                        codigoCupon = Globales.cpIntegraEMV.CodigoCupon();

                        if (respForm == "1")
                        {
                            this.frameBuscar.Visibility = Visibility.Visible;
                            cmdRegresarCU_Click(null, null);
                        }

                        if (respForm == "2")
                        {
                            isCuponMultiple = true;
                            limpia();
                            strCodeCupon = Globales.nodeListXML.Item(codigoCupon).ChildNodes[0].InnerText;
                            var descripcionCupon = Globales.nodeListXML.Item(0).ChildNodes[1].InnerText.Split('|').ToArray();
                            strCaption = string.Format("{0}\n{1}\nVigencia: {2}", descripcionCupon[0],
                                descripcionCupon[1],
                                Globales.nodeListXML.Item(0).ChildNodes[2].InnerText);



                            if (this.askRedimir(strCaption))
                                this.cmdRedimirCU.PerformClick();
                            else
                                this.cmdRegresarCU.PerformClick();
                        }
                    }
                }
                else
                {
                    //limpia();
                    mensaje = "Error en el servicio, intente nuevamente.";
                    if (!string.IsNullOrWhiteSpace(respuesta))
                        mensaje = Globales.GetDataXml("nb_error", respuesta);
                    Globales.MessageBoxMitError(mensaje);
                }
            }
            catch
            {
                Globales.MessageBoxMit("Error" + "frmCuponesValidarcmdvalidar");
            }
        }

        private void limpia()
        {
            frameError.Visibility = Visibility.Hidden;
            frameBuscar.Visibility = Visibility.Hidden;
            frameCuponUnico.Visibility = Visibility.Hidden;
            frameCuponMultiples.Visibility = Visibility.Hidden;
            lblError.Content = "Lo sentimos";
            cmdRegresarE.Visibility = Visibility.Visible;
        }

        public string respuesta { get; set; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblEtiqueta.Content = "Cupón o número de celular";
            txtTelefono.MaxLength = 16;
            etiqueta = "Introduzca número de celular o cupón";
            etiqueta2 = "Número de celular o cupón invalido";
            cmdValida.Content = "Validar";
            if (Globales.isCuponTelefonico)
            {
                txtTelefono.MaxLength = 10;
                lblEtiqueta.Content = "Número de celular";
                etiqueta = "Introduzca número de celular";
                etiqueta2 = "Número de celular invalido";
                cmdValida.Content = "Recuperar";
            }
            frameError.Visibility = Visibility.Hidden;
            frameBuscar.Visibility = Visibility.Visible;
            frameCuponUnico.Visibility = Visibility.Hidden;
            frameCuponMultiples.Visibility = Visibility.Hidden;
            lblError.Content = "Lo sentimos";
            cmdRegresarE.Visibility = Visibility.Hidden;
            txtTelefono.Focus();

        }

        private void cmdRegresarCU_Click(object sender, RoutedEventArgs e)
        {
            if (isCuponMultiple)
            {
                isCuponMultiple = false;
                cmdValidar_Click(null, null);
            }
            else
            {
                limpia();
                frameBuscar.Visibility = Visibility.Visible;
                cmdValida.IsEnabled = true;
                txtTelefono.IsEnabled = true;
                txtTelefono.Focus();
                lblCuponU.Visibility = Visibility.Hidden;
                lblCuponU.Text = "";
                isCuponMultiple = false;
            }
        }

        private void cmdRedimirCU_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            respuesta = Globales.cpIntegraEMV.sndPayNoPayRedimirCupon(TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country,
                TypeUsuario.usu, TypeUsuario.Pass, strCodeCupon);
            Mouse.OverrideCursor = null;
            if (Globales.GetDataXml("nb_status", respuesta).ToUpper() == "SUCCESS")
            {
                //limpia();
                //lblError.Content = "Cupón Redimido";

                Globales.MessageBoxMitApproved("Cupón Redimido\nAhora ya se puede ocupar el cupon");

                //frameError.Visibility = Visibility.Visible;
                lblResultadoE.Text = "Ahora ya se puede ocupar el cupon";
                cmdRegresarE.Visibility = Visibility.Hidden;
                frameBuscar.Visibility = Visibility.Visible;
                txtTelefono.Text = "";
            }
            else
            {
                // limpia();
                //frameError.Visibility = Visibility.Visible;
                lblResultadoE.Visibility = Visibility.Visible;
                if (string.IsNullOrWhiteSpace(respuesta))
                {
                    lblResultadoE.Text = "Error en el Servicio, intente nuevamente.";
                    Globales.MessageBoxMitError("Error en el Servicio, intente nuevamente.");
                }
                else
                {
                    lblResultadoE.Text = Globales.GetDataXml("nb_error", respuesta);
                    Globales.MessageBoxMitError(Globales.GetDataXml("nb_error", respuesta));
                    this.frameBuscar.Visibility = Visibility.Visible;
                }
            }
        }

        private void cmdRegresarE_Click(object sender, RoutedEventArgs e)
        {
            limpia();
            frameBuscar.Visibility = Visibility.Visible;
            cmdValida.IsEnabled = true;
            txtTelefono.IsEnabled = true;
            txtTelefono.Focus();
        }

        private void cmdSalirE_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
        }

        private void cmdRegresarCM_Click(object sender, RoutedEventArgs e)
        {
            cmdRedimirCU_Click(null, null);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        public string mensaje { get; set; }

        private void chk1_Click(object sender, RoutedEventArgs e)
        {
            etiqueta = "Introduzca cupón";
            etiqueta2 = "Número de cupón invalido";
            lblEtiqueta.Content = "Cupón";
            txtTelefono.MaxLength = 16;
            if (Convert.ToBoolean(chk1.IsChecked))
            {
                lblEtiqueta.Content = "Número de celular";
                txtTelefono.MaxLength = 10;
                etiqueta = "Introduzca número de celular";
                etiqueta2 = "Número de celular invalido";
            }
            txtTelefono.Text = "";
        }

        public string etiqueta { get; set; }

        public string etiqueta2 { get; set; }

        private void cmdValidar_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.frameCuponMultiples.Visibility = Visibility.Visible;
        }
    }
}
