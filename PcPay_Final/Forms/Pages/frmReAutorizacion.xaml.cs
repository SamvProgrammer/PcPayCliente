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
    /// Interaction logic for frmReAutorizacion.xaml
    /// </summary>
    public partial class frmReAutorizacion : Page
    {
        private const string NOMBRE_GENERAL = "frmCheckout";
        string strImprimeVouhcer, strCadaux;
        double sum;
        public cerrarVentana cerrar;
        int alto, alto2;
        public frmReAutorizacion()
        {
            InitializeComponent();
            txtImporte.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;
        }

        private void cargandoVentana(object sender, RoutedEventArgs e)
        {
            this.G2.Visibility = Visibility.Hidden;
            this.G1.Visibility = Visibility.Visible;
            this.txtCuarto.GotFocus += Globales.setFocusMit2;
            this.txtCuarto.LostFocus += Globales.lostFocusMit2;

            this.txtImporte.GotFocus += Globales.setFocusMit2;
            this.txtImporte.GotFocus += Globales.lostFocusMit2;
            limpia();
            txtCuarto.Focus();
        }

        private void limpia()
        {
            txtImporte.Text = "";
            cmdAceptar2.Content = "Aceptar";
            cmdAceptar2.IsEnabled = true;
            cmdCancelar2.Content = "Cancelar";
            cmdCancelar2.IsEnabled = true;
        }

        private void ClickRadio1(object sender, RoutedEventArgs e)
        {
            this.txtCuarto.Text = string.Empty;

            if (Convert.ToBoolean(numRef1.IsChecked))
            {
                txtCuarto.MaxLength = 40;
                txtCuarto.Focus();
            }
            else
            {
                txtCuarto.MaxLength = 9;
                txtCuarto.Focus();
            }
        }

        private void lostfocustxt(object sender, RoutedEventArgs e)
        {
            txtCuarto.Text = txtCuarto.Text.ToUpper();
        }

        private void cmdCancelar1_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
        }

        private void cmdAceptar1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string strCadEncriptar = "";

                string strParam = string.Empty;
                if (Convert.ToBoolean(numRef1.IsChecked))
                {
                    strParam = "referencia";
                }
                else
                {
                    strParam = "cuarto";
                }
                if (string.IsNullOrWhiteSpace(txtCuarto.Text))
                {
                    Globales.MessageBoxMit("Introduzca Dato Adicional");
                    return;
                }
                strCadEncriptar = "&idcompany=" + TypeUsuario.Id_Company +
                                  "&idbranch=" + TypeUsuario.Id_Branch +
                                  "&usuario=" + TypeUsuario.usu +
                                  "&buscar=" + txtCuarto.Text.Trim() +
                                  "&por=" + strParam +
                                  "&op=consultareautorizacion";
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                Globales.cpHTTP_Clear();
                Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
                Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
                int i = 0;
                int sum = 0;

                if (Globales.cpHTTP_SendcpCUCT())
                {
                    Mouse.OverrideCursor = null;
                    strCadaux = Globales.cpHTTP_sResult;
                    if (Globales.GetDataXml("referencia" + i, strCadaux) == "")
                    {
                        G1.Visibility = Visibility.Visible;
                        G2.Visibility = Visibility.Hidden;
                        txtCuarto.Focus();
                        Globales.MessageBoxMit("No existe información asociada.");

                    }
                    else
                    {
                        int j = 0;
                        lblReservacion.Content = "Reserv:" + Globales.GetDataXml("referencia0", strCadaux);
                        lblCuarto.Content = "Dato Adicional:" + Globales.GetDataXml("room0", strCadaux);
                        strCadaux = strCadaux.Replace("-", "");
                        data.Items.Clear();

                        while (!string.IsNullOrWhiteSpace(Globales.GetDataXml(string.Format("referencia{0}", i), strCadaux)))
                        {
                            modelitoSimple modelo = new modelitoSimple()
                            {
                                referencia = Globales.GetDataXml("referencia" + i, strCadaux),
                                fecha = Globales.GetDataXml("fecha" + i, strCadaux),
                                importe = Globales.GetDataXml("importe" + i, strCadaux),
                                operacion = Globales.GetDataXml("operacion" + i, strCadaux),
                                cliente = Globales.GetDataXml("cc_name" + i, strCadaux),
                                tarjeta = Globales.GetDataXml("cc_tarjeta" + i, strCadaux),
                                datoAdicional = Globales.GetDataXml("room" + i, strCadaux),
                                tipoOperacion = Globales.GetDataXml("tp_op" + i, strCadaux)
                            };
                            if (modelo.tipoOperacion == "CHECKIN")
                                if (!string.IsNullOrWhiteSpace(strCadaux))
                                {
                                    data.Items.Add(modelo);
                                    i++;
                                }
                                else
                                    i++;

                            G1.Visibility = Visibility.Hidden;
                            G2.Visibility = Visibility.Visible;

                            lblTotal.Content = "Total: " + sum;
                        }
                    }
                }
            }
            catch
            {
                Globales.MessageBoxMit(NOMBRE_GENERAL + "Clickl1");
            }
            Mouse.OverrideCursor = null;
        }

        private void txtImporte_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
        }

        private void txtImporte_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void cmdCancelar2_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(cmdCancelar2.Content).Contains("Imprime Voucher"))
            {
                imprimir();
            }
            else
            {
                cerrar();
            }
        }

        private void imprimir()
        {
            switch (TypeUsuario.TipoImpresora)
            {
                case "1":
                    if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult), TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                    {
                        TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                    }
                    break;
                case "2":
                    break;
                case "3":
                    Globales.imprimirEpson();
                    break;
                case "4":
                    if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult), TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                    {
                        TypeUsuario.strVoucher = Globales.VoucherHtml1(Globales.cpHTTP_sResult);
                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                    }
                    break;
                case "5":
                    break;
                case "6":
                    Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                    break;
            }
        }

        private void cmdAceptar2_Click(object sender, RoutedEventArgs e)
        {
            string strCadEncritpar = "";
            string mensaje = string.Empty;

            cmdAceptar2.IsEnabled = false;
            cmdCancelar2.IsEnabled = false;

            if (Convert.ToString(cmdAceptar2.Content).Contains("Otro"))
            {
                limpia();
                txtCuarto.Text = "";
                this.G1.Visibility = Visibility.Visible;
                this.G2.Visibility = Visibility.Hidden;
                Globales.InfoCheckOut = "";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Globales.InfoCheckOut))
                {
                    Globales.MessageBoxMit("Debe seleccionar una opreación");
                    cmdAceptar2.IsEnabled = false;
                    cmdCancelar2.IsEnabled = true;
                }
                else if (string.IsNullOrWhiteSpace(txtImporte.Text) || txtImporte.Text.Equals("0.00", StringComparison.InvariantCulture))
                {
                    Globales.MessageBoxMit("Introduzca el importe");
                    txtImporte.Focus();
                    cmdAceptar2.IsEnabled = true;
                    cmdCancelar2.IsEnabled = true;
                }
                else
                {


                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                    string strTypeC = "";
                    Globales.cpIntegracion_Clear();
                    Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL_RA;
                    Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                    Globales.cpIntegraEMV.sndReautorizacionMOTO(
                          TypeUsuario.usu,
                          TypeUsuario.Pass,
                          "",
                          TypeUsuario.Id_Company,
                          TypeUsuario.Id_Branch,
                          TypeUsuario.country,
                          txtImporte.Text,
                          Globales.InfoCheckOut
                        );
                    Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();
                    Mouse.OverrideCursor = null;

                    switch (Globales.GetDataXml("response", Globales.cpIntegracion_sResult).ToLower())
                    {
                        case "approved":
                            //lblAut.Visibility = Visibility.Hidden;
                            //lblautorizacion.Visibility = Visibility.Visible;
                            //lblrechazado.Visibility = Visibility.Hidden;
                            //lblTError.Visibility = Visibility.Hidden;
                            //lblAut.Visibility = Visibility.Visible;
                            //lblautorizacion.Visibility = Visibility.Visible;
                            mensaje = string.Format(Globales.GetDataXml("auth", Globales.cpIntegracion_sResult));
                            Globales.MessageBoxMitApproved(mensaje);
                            TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult).Trim();
                            imprimir();
                            if (TypeUsuario.IsAQ)
                            {
                                Globales.VerificaVoucher();
                            }
                            txtImporte.IsEnabled = false;
                            cmdAceptar2.Content = "Otro";
                            cmdCancelar2.Content = "Imprime Voucher";
                            cerrar();
                            break;
                        case "denied":
                            Globales.MessageBoxMitDenied("Autorización denegada");
                            cmdAceptar2.Visibility = Visibility.Visible;
                            break;
                        case "error":
                            mensaje = Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult);
                            Globales.MessageBoxMitError(mensaje);
                            cmdAceptar2.Visibility = Visibility.Visible;
                            break;
                        default:
                            mensaje = "Verifique su conexión de internet";
                            Globales.MessageBoxMitError(mensaje);
                            cmdAceptar2.Visibility = Visibility.Visible;
                            break;
                    }
                    Mouse.OverrideCursor = null;
                    cmdAceptar2.IsEnabled = true;
                    cmdCancelar2.IsEnabled = true;
                }
            }

        }

        private void data_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void escogerTransaccion(object sender, SelectionChangedEventArgs e)
        {
            DataGrid tablita = sender as DataGrid;
            modelitoSimple datos = tablita.SelectedItem as modelitoSimple;
            Globales.InfoCheckOut = datos.operacion;
            cmdAceptar2.IsEnabled = true;
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void txtCuarto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }


    }
    internal class modelitoSimple
    {
        public string referencia { get; set; }
        public string fecha { get; set; }
        public string importe { get; set; }
        public string operacion { get; set; }
        public string cliente { get; set; }
        public string tarjeta { get; set; }
        public string datoAdicional { get; set; }
        public string tipoOperacion { get; set; }
    }
}
