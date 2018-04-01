using PcPay.Code.Configuracion;
using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
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
    /// Lógica de interacción para frmFacturaE.xaml
    /// </summary>
    public partial class frmFacturaE : Page
    {
        public frmFacturaE()
        {
            InitializeComponent();
            carga();
            cbxFormaPago.SelectionChanged += new SelectionChangedEventHandler(cambia);

            this.TxtTicket.GotFocus += Globales.setFocusMit2;
            this.Importe.GotFocus += Globales.setFocusMit2;
            this.txtNoCuenta.GotFocus += Globales.setFocusMit2;
            this.cbxFormaPago.GotFocus += Globales.setFocusMit2;
            this.cboConcepto.GotFocus += Globales.setFocusMit2;

            this.TxtTicket.LostFocus += Globales.lostFocusMit2;
            this.Importe.LostFocus += Globales.lostFocusMit2;
            this.txtNoCuenta.LostFocus += Globales.lostFocusMit2;
            this.cbxFormaPago.LostFocus += Globales.lostFocusMit2;
            this.cboConcepto.LostFocus += Globales.lostFocusMit2;

        }

        private void cambia(object sender, SelectionChangedEventArgs e)
        {
            txtNoCuenta.IsEnabled = true;
            string text = Convert.ToString((sender as ComboBox).SelectedItem);
            text = text.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            if (!Globales.HabilitaNumCuenta(text))
            {
                txtNoCuenta.Text = "";
                txtNoCuenta.IsEnabled = false;
            }
        }

        public cerrarVentana cerrar;


        private void cmdAcepEmp_Click(object sender, RoutedEventArgs e)
        {
            //string fac_elec = string.Empty;
            string fac_elec = string.Empty;
            string idFormaPago = string.Empty;
            string FormaPago = string.Empty;

            if (cbxFormaPago.SelectedIndex == 0)
            {
                Globales.MessageBoxMit("Seleccione el tipo de pago.");
                return;
            }


            if (Globales.HabilitaNumCuenta(cbxFormaPago.Text))
            {
                if (string.IsNullOrEmpty(txtNoCuenta.Text))
                {
                    Globales.MessageBoxMit("Ingrese el no. de cuenta");
                    return;
                }
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (!string.IsNullOrWhiteSpace(TxtTicket.Text) && cboConcepto.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(Importe.Text))
            {
                //   TxtTicket.IsEnabled = false;
               
                TxtOperacion.IsEnabled = false;
                TxtFecha.IsEnabled = false;
                Importe.IsEnabled = false;
                cmdAcepEmp.IsEnabled = false;
                TxtTicket.IsEnabled = false;
                //    cmdNuevo.IsEnabled = true;
                cbxFormaPago.IsEnabled = false;
                txtNoCuenta.IsEnabled = false;

                Globales.cpIntegraEMV.dbgSetUrl(Program.ipfe);

                idFormaPago = Globales.cpIntegraEMV.ObtieneIDFacturaElectronica(cbxFormaPago.Text);
                FormaPago = cbxFormaPago.Text;
                if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("operacion", Globales.XMLFacturaE)))
                {
                    fac_elec = Globales.cpIntegraEMV.sndFacturaElectronica(TypeUsuario.usu, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, Importe.Text.Trim(), Propina, TxtTicket.Text.Trim(), TxtOperacion.Text, TxtFecha.Text, idFormaPago, FormaPago, cboConcepto.Text.Trim(), txtNoCuenta.Text.Trim());
                }
                else
                {
                        fac_elec = Globales.cpIntegraEMV.sndFacturaElectronica(TypeUsuario.usu, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, Importe.Text.Trim(), "", TxtTicket.Text.Trim(), "", "", idFormaPago, FormaPago, cboConcepto.Text.Trim(),txtNoCuenta.Text.Trim());
                }

                Mouse.OverrideCursor = null;

                if (Globales.cpIntegraEMV.getRspFeCdResponse() == "0")
                {
                    Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspFeDsResponse());
                }
                else
                {
                    string imprime = string.Empty;
                    imprime = Globales.cpIntegraEMV.getRspFeTxLeyenda();
                    string otro = Globales.cpIntegraEMV.getRspFeTxLeyenda();
                    otro = otro.Replace("@br", "\n");
                    otro = otro.Replace("@cnb", "");
                    otro = otro.Replace("@cnn", "");
                    otro = otro.Replace("@lnn", "");
                    otro = otro.Replace("@cbb", "");

                   // System.Windows.Forms.MessageBox.Show(otro);
                    imprime = imprime.Replace("cbb", "cnb");
                    imprime = imprime.Replace("á", "a");
                    imprime = imprime.Replace("é", "e");
                    imprime = imprime.Replace("í", "i");
                    imprime = imprime.Replace("ó", "o");
                    imprime = imprime.Replace("ú", "u");

                    imprime = imprime.Replace("Á", "A");
                    imprime = imprime.Replace("É", "E");
                    imprime = imprime.Replace("Í", "I");
                    imprime = imprime.Replace("Ó", "O");
                    imprime = imprime.Replace("Ú", "U");

                    imprime = imprime.Replace("$$", "$");
                    imprime = imprime.Replace("@cnb Genere su Factura Electronica", "@cnb Genere su Factura @cnb Electronica");
                    imprime = imprime.Replace("@lnn 1.Ingrese a www.mitfacturaelectronica.com.mx", "@lnn 1.Ingrese a @lnn www.mitfacturaelectronica.com.mx");
                    imprime = imprime.Replace("@lnn 2.Introduzca el numero de operacion y monto.", "@lnn 2.Introduzca el numero de operacion y @lnn monto.");
                    imprime = imprime.Replace("@cnb Tiene 3 dias naturales a partir de:", "@cnb Tiene 3 dias @cnb naturales a partir de:");


                    imprime = imprime.Replace(System.Environment.NewLine, " ");
                    imprime = imprime.Replace("\r", " ");


                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    if (TypeUsuario.TipoImpresora != "6")
                    {
                         Globales.PrintOptions(imprime,"",null,true,true);
                    }
                    else
                    {
                        Globales.cpIntegraEMV.dbgPrint(imprime + "@br @br @br");
                    }
                    Mouse.OverrideCursor = null;
                }
                CmdNuevo.IsEnabled = true;
            }
            else
            {
                Mouse.OverrideCursor = null;
                Globales.MessageBoxMit("Favor de llenar todos los campos");

                TxtTicket.IsEnabled = true;
                TxtOperacion.IsEnabled = true;
                TxtFecha.IsEnabled = true;
                txtNoCuenta.IsEnabled = true;
                Importe.IsEnabled = true;
                cmdAcepEmp.IsEnabled = true;
                
            }
            Mouse.OverrideCursor = null;

        }






        public string Propina { get; set; }



        private void CmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            ObtieneConceptos(Globales.GetDataXml("conceptos", TypeUsuario.CadenaXML));
            cboConcepto.SelectedIndex = 0;
            TxtTicket.Text = "";
            TxtOperacion.Text = "";
            TxtFecha.Text = "";
            TxtTicket.IsEnabled= true;
            Importe.Text = "";
            Propina = "";
            Importe.IsEnabled = true;
            cmdAcepEmp.IsEnabled = true;
            CmdNuevo.IsEnabled = false;
            TxtTicket.IsEnabled = true;
            //  cmdNuevo.IsEnabled = false;
            txtNoCuenta.Text = "";

            txtnocuenta2.Visibility = Visibility.Visible;
            lblNoCuenta.Visibility = Visibility.Visible;

            cbxFormaPago.SelectedIndex = 0;
            cbxFormaPago2.Visibility = Visibility.Visible;
            cbxFormaPago.IsEnabled = true;
            TxtTicket.Focus();
        }

        public void carga()
        {

            ObtieneConceptos(Globales.GetDataXml("conceptos", TypeUsuario.CadenaXML));

            if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("importe", Globales.XMLFacturaE)))
            {
                Importe.Text = Globales.GetDataXml("importe", Globales.XMLFacturaE).Trim();
                TxtOperacion.Text = Globales.GetDataXml("operacion", Globales.XMLFacturaE).Trim();
                TxtFecha.Text = Globales.GetDataXml("fecha", Globales.XMLFacturaE).Trim();
                Propina = Globales.GetDataXml("propina", Globales.XMLFacturaE).Trim();
                Importe.IsEnabled = false;
                txtNoCuenta.Text = Globales.GetDataXml("cc_number", Globales.cpIntegracion_sResult);


                if (Globales.EsTarjetaCredito())
                {
                    cbxFormaPago.SelectedIndex = 4;
                }
                else
                {
                    cbxFormaPago.SelectedIndex = 8;
                }


                lblNoCuenta.Visibility = Visibility.Hidden;
                txtnocuenta2.Visibility = Visibility.Visible;
                cbxFormaPago2.Visibility = Visibility.Hidden;
            }


        }

        private void ObtieneConceptos(string conceptos)
        {
            // Definir codigo para llenar el cbo
            int i = 1;
            cboConcepto.Items.Clear();
            while (!string.IsNullOrWhiteSpace(Globales.GetDataXml("concepto" + i, conceptos)))
            {
                cboConcepto.Items.Add(Globales.GetDataXml("concepto" + i, conceptos));
                i++;
            }
            if (i <= 2)
            {
                cboConcepto.IsEnabled = false;
            }

            cboConcepto.SelectedIndex = 0;
            cbxFormaPago.SelectedIndex = 0;

        }



        private void cmdCancEmp_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = ".cmdCancEmp_Click()";
        }



        private void Numero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }

        private void AlfaNumerico(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

        private void Importe_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void NumeroPunto(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void txtNoCuenta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ObtieneConceptos(Globales.GetDataXml("conceptos", TypeUsuario.CadenaXML));
                cboConcepto.SelectedIndex = 0;
                cbxFormaPago.SelectedIndex = 0;
                if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("importe", Globales.XMLFacturaE)))
                {
                    string xmlFactura = Globales.XMLFacturaE;
                    Importe.Text = Globales.GetDataXml("importe", xmlFactura).Trim();
                    TxtOperacion.Text = Globales.GetDataXml("operacion", xmlFactura).Trim();
                    TxtFecha.Text = Globales.GetDataXml("fecha", xmlFactura).Trim();
                    Propina = Globales.GetDataXml("propina", xmlFactura).Trim();
                    Importe.IsEnabled = false;
                    txtNoCuenta.Text = Globales.GetDataXml("cc_number", Globales.cpIntegracion_sResult);

                    if (Globales.EsTarjetaCredito())
                    {
                        cbxFormaPago.SelectedIndex = 4;
                    }
                    else
                    {
                        cbxFormaPago.SelectedIndex = 8;
                    }
                    lblNoCuenta.Visibility = Visibility.Hidden;
                    txtnocuenta2.Visibility = Visibility.Hidden;
                    cbxFormaPago2.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                Globales.MessageBoxMit("Error en cargar la pagina");
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Globales.XMLFacturaE = "";
            Globales.cpIntegracion_sResult = "";
        }









    }

}





