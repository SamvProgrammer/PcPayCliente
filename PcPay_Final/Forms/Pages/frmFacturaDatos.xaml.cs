using PcPay.Code.Configuracion;
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
    /// Interaction logic for frmFacturaDatos.xaml
    /// </summary>
    public partial class frmFacturaDatos : Page
    {
        public frmFacturaDatos()
        {
            InitializeComponent();
            this.cargar();
            
            this.txtRFC1.GotFocus += Globales.setFocusMit2;
            this.txtNombre1.GotFocus += Globales.setFocusMit2;
            this.TxtEMail.GotFocus += Globales.setFocusMit2;
            this.txtCalle.GotFocus += Globales.setFocusMit2;
            this.txtExt.GotFocus += Globales.setFocusMit2;
            this.txtInt.GotFocus += Globales.setFocusMit2;
            this.txtCP.GotFocus += Globales.setFocusMit2;
            this.TxtColonia.GotFocus += Globales.setFocusMit2;
            this.txtDelMun.GotFocus += Globales.setFocusMit2;
            this.txtCiudad.GotFocus += Globales.setFocusMit2;
            this.txtEstado.GotFocus += Globales.setFocusMit2;
            this.TxtTicket.GotFocus += Globales.setFocusMit2;
            this.TxtImporte.GotFocus += Globales.setFocusMit2;
            this.txtNoCuenta.GotFocus += Globales.setFocusMit2;
            this.cboConcepto.GotFocus += Globales.setFocusMit2;

            this.txtRFC1.LostFocus += Globales.lostFocusMit2;
            this.txtNombre1.LostFocus += Globales.lostFocusMit2;
            this.TxtEMail.LostFocus += Globales.lostFocusMit2;
            this.txtCalle.LostFocus += Globales.lostFocusMit2;
            this.txtExt.LostFocus += Globales.lostFocusMit2;
            this.txtInt.LostFocus += Globales.lostFocusMit2;
            this.txtCP.LostFocus += Globales.lostFocusMit2;
            this.TxtColonia.LostFocus += Globales.lostFocusMit2;
            this.txtDelMun.LostFocus += Globales.lostFocusMit2;
            this.txtCiudad.LostFocus += Globales.lostFocusMit2;
            this.txtEstado.LostFocus += Globales.lostFocusMit2;
            this.TxtTicket.LostFocus += Globales.lostFocusMit2;
            this.TxtImporte.LostFocus += Globales.lostFocusMit2;
            this.txtNoCuenta.LostFocus += Globales.lostFocusMit2;
            this.cboConcepto.LostFocus += Globales.lostFocusMit2;


        }
        public cerrarVentana cerrar;
        // public setTitulo titulo;


        private void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            string RFC = string.Empty;
            string nombre = string.Empty;
            string fac_elec = string.Empty;
            string idFormaPago = string.Empty;
            string FormaPago = string.Empty;

            RFC = txtRFC1.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtRFC1.Text))
            {
                Globales.MessageBoxMit("El campor RFC no es correcto");
                return;
            }
            if (txtRFC1.Text.Length < 12)
            {
                Globales.MessageBoxMit("La longitud del campo RFC es incorrecta");
                return;
            }
            if (!txtNombre1.Text.Contains(" "))
            {
                //
                Globales.MessageBoxMit("El campo Nombre o Razón Social no es correcto");
                return;
            }
            else
            {

                nombre = txtNombre1.Text;
            }

            if (string.IsNullOrWhiteSpace(TxtEMail.Text.Trim()))
            {
                Globales.MessageBoxMit("El campo Correo Electrónico no puede estar vacío");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCalle.Text.Trim()))
            {
                Globales.MessageBoxMit("El campo Calle no puede estar vacío");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtExt.Text.Trim()))
            {
                Globales.MessageBoxMit("El campo Número Exterior no puede estar vacío");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCP.Text.Trim()))
            {
                Globales.MessageBoxMit("El campo Código Postal no puede estar vacío");
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtColonia.Text.Trim()))
            {
                Globales.MessageBoxMit("El campo Colonia no puede estar vacío");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDelMun.Text.Trim()))
            {
                Globales.MessageBoxMit("El campo Delegación o Municipio no puede estar vacío");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCiudad.Text.Trim()))
            {
                Globales.MessageBoxMit("El campo Ciudad no puede estar vacío");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtEstado.Text.Trim()))
            {
                Globales.MessageBoxMit("El campo Estado no puede estar vacío");
                return;
            }

            string Tarjeta = string.Empty;
            if (TxtImporte.Visibility == Visibility.Visible)
            {// = True Then
                if (string.IsNullOrWhiteSpace(TxtImporte.Text))
                {
                    Globales.MessageBoxMit("El campo Estado no puede estar vacío");
                    return;
                }

                if (Globales.HabilitaNumCuenta(cbxFormaPago.Text))
                {
                    if (string.IsNullOrWhiteSpace(txtNombre1.Text))
                    {//Trim(txtNoCuenta.Text) = "" Then

                        Globales.MessageBoxMit("El campo No. Cuenta no puede estar vacío");
                        return;

                    }
                }
                if (cbxFormaPago.SelectedIndex <= 0)
                {
                    Globales.MessageBoxMit("Seleccione el tipo de pago");
                    return;
                }
                else
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                    idFormaPago = Globales.cpIntegraEMV.ObtieneIDFacturaElectronica(cbxFormaPago.Text);
                    FormaPago = cbxFormaPago.Text;
                    Globales.XMLFacturaE = Globales.XMLFacturaE.Replace("<importe></importe>", "<importe>" + TxtImporte.Text + "</importe><metodoPago>" + FormaPago + "</metodoPago>");
                    Mouse.OverrideCursor = null;
                }
            }

            string metodoPago = string.Empty;

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            idFormaPago = Globales.cpIntegraEMV.ObtieneIDFacturaElectronica(cbxFormaPago.Text);
            FormaPago = cbxFormaPago.Text;
            Globales.cpIntegraEMV.dbgSetUrl(Program.ipfe);
            fac_elec = Globales.cpIntegraEMV.sndFacturaElectronicaDatos(TypeUsuario.usu, Globales.GetDataXml("fecha", Globales.XMLFacturaE), "", TypeUsuario.Id_Branch + "," + TypeUsuario.Id_Company, "", "1", idFormaPago, FormaPago, Globales.GetDataXml("importe", Globales.XMLFacturaE), TxtTicket.Text, RFC.Trim(), Globales.GetDataXml("operacion", Globales.XMLFacturaE), TxtEMail.Text, Globales.GetDataXml("propina", Globales.XMLFacturaE), cboConcepto.Text, txtNoCuenta.Text.Trim(), "MXN", "", "", "", nombre, txtCP.Text, txtInt.Text, "MEXICO", txtEstado.Text, txtCiudad.Text, TxtColonia.Text, txtExt.Text, txtDelMun.Text, txtCalle.Text);
            Mouse.OverrideCursor = null;
            if (Globales.GetDataXml("cd_response", fac_elec).Trim() != "100" && fac_elec.Trim() != "error")
            {

                Globales.MessageBoxMit(Globales.GetDataXml("nb_response", fac_elec));

            }


            else
            {
                if (fac_elec.Trim() != "error")
                {
                    Globales.MessageBoxMit("La factura se ha generado y enviado exitosamente.");
                }
            }
            if (fac_elec.Trim() == "error")
            {
                Globales.MessageBoxMit("Ha ocurrido un error, por favor vuelve a intentarlo");
            }
            else
            {
                
                cerrar();
            }

        }

        public void cargar()
        {
            ObtieneConceptos(Globales.GetDataXml("conceptos", TypeUsuario.CadenaXML));
            if (string.IsNullOrWhiteSpace(Globales.GetDataXml("importe", Globales.XMLFacturaE)))
            {
                txtImporte2.Visibility = Visibility.Visible;
                lbObImporte.Visibility = Visibility.Visible;
                lbImporte.Visibility = Visibility.Visible;
                lblNoCuenta.Visibility = Visibility.Visible;
                txtNoCuenta2.Visibility = Visibility.Visible;
                cbxFormaPago2.Visibility = Visibility.Visible;
            }
            else
            {
                txtNoCuenta.Text = Globales.GetDataXml("cc_number", Globales.cpIntegracion_sResult);

                if (Globales.EsTarjetaCredito())
                {
                    cbxFormaPago.SelectedIndex = 4;
                }
                else
                {
                    cbxFormaPago.SelectedIndex = 8;
                }

            }

        }

        private void ObtieneConceptos(string conceptos)
        {
            // Definir codigo para llenar el cbo
            int i = 1;
            while (!string.IsNullOrWhiteSpace(Globales.GetDataXml("concepto" + i, conceptos)))
            {
                cboConcepto.Items.Add(Globales.GetDataXml("concepto" + i, conceptos));
                i++;
            }


            if (cboConcepto.Items.Count > 1)
            {
                cboConcepto.IsEnabled = true;
            }
            else {
                cboConcepto.IsEnabled = false;
            }

            cboConcepto.SelectedIndex = 0;
            cbxFormaPago.SelectedIndex = 0;

        }

        private void cbxFormaPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void AlfaNumerico(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

        private void TxtEMail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Globales.ValidaEmail(TxtEMail.Text))
            {
                Globales.MessageBoxMit("Email no valido, ingrese de nuevo");
                TxtEMail.Text = "";
            }
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void TxtImporte_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender,e);
        }

        private void soloNumero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender,e);
        }

        private void solonumeroLetra(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }

        private void TxtImporte_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender,e);
        }

        private void perdioFoco(object sender, RoutedEventArgs e)
        {
            ComboBox aux = sender as ComboBox;

            string aux2 = aux.Text;


            if (!Globales.HabilitaNumCuenta(aux2))
            {
                txtNoCuenta.Text = "";
                txtNoCuenta.IsEnabled = false;
            }
            else
            {
                txtNoCuenta.IsEnabled = true;
            }           
        }


    }
}
