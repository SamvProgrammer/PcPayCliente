using PcPay.Code.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;

namespace PcPay.Forms.Pages
{
    /// <summary>
    /// Interaction logic for frmVentaForzada.xaml
    /// </summary>
    public partial class frmVentaForzada : Page
    {
        public string caption = "";
        public const string NOMBRE_GENERAL = "frmVentaForzada";
        public abrirFrm abrirFrmNuevo;
        public string num_tarjeta { get; set; }
        private DispatcherTimer tiempo;
        public frmVentaForzada()
        {
            InitializeComponent();
            this.mes.SelectedIndex = -1;
            this.anio.SelectedIndex = -1;

            #region"Efecto en cajas"
            this.numTdc.GotFocus += Globales.setFocusMit2;
            this.mes.GotFocus += Globales.setFocusMit2;
            this.anio.GotFocus += Globales.setFocusMit2;
            this.nomTdc.GotFocus += Globales.setFocusMit2;
            this.numCvv.GotFocus += Globales.setFocusMit2;
            this.numOrden.GotFocus += Globales.setFocusMit2;
            this.importe.GotFocus += Globales.setFocusMit2;
            numAut.GotFocus += Globales.setFocusMit2;

            this.numTdc.LostFocus += Globales.lostFocusMit2;
            this.mes.LostFocus += Globales.lostFocusMit2;
            this.anio.LostFocus += Globales.lostFocusMit2;
            this.nomTdc.LostFocus += Globales.lostFocusMit2;
            this.numCvv.LostFocus += Globales.lostFocusMit2;
            this.numOrden.LostFocus += Globales.lostFocusMit2;
            this.importe.LostFocus += Globales.lostFocusMit2;
            numAut.LostFocus += Globales.lostFocusMit2;
            #endregion

            tiempo = new DispatcherTimer();

            this.numCvv.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            this.numTdc.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            this.numAut.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            this.importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;

            this.LCVV.Visibility = Visibility.Hidden;
            this.GCVV.Visibility = Visibility.Hidden;

            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.MouseDown += Globales.sendMail_GridMouseDown;
            this.BENVIAMAIL.Tag = string.Empty;
        }

        private Control control;
        private bool isCvv = false;
        private void cargandoVentana(object sender, RoutedEventArgs e)
        {
            lblMoneda.Content = "";
            Globales.cpIntegraEMV.dbgEndOperation();
            //switch
            LTITTULO.Content = "Venta Forzada";
            anio.Items.Clear();
            mes.Items.Clear();
            for (int x = 0; x <= 10; x++)
            {
                anio.Items.Add((DateTime.Now.Year + x).ToString());
            }
            for (int x = 1; x <= 12; x++)
            {
                string aux = string.Empty;
                if (x < 10) aux = "0";
                aux += Convert.ToString(x);
                mes.Items.Add(aux);
            }
            limpia();
            //falta bancos ..
            numCvv.Visibility = Visibility.Hidden;
            numCvv.IsEnabled = false;


            TimeSpan t = new TimeSpan(0, 0, 0, 0, 5);
            tiempo.Interval = t;
            tiempo.Tick += (a, b) =>
            {
                if (control != null)
                {
                    if (isCvv)
                    {
                        PasswordBox cvv = control as PasswordBox;
                        isCvv = false;
                        cvv.Focus();
                    }
                    else
                    {
                        TextBox text = control as TextBox;
                        text.Focus();
                    }
                    control = null;
                }
            };
            tiempo.Start();  
        }

        private void limpia()
        {
            Activa();
            importe.Text = "";
            numAut.Text = "";
            numCvv.Password = "";
            numOrden.Text = "";
            nomTdc.Text = "";
            numTdc.Text = "";
            num_tarjeta = "";
            this.mes.SelectedIndex = -1;
            this.anio.SelectedIndex = -1;

            this.num_tarjeta = string.Empty;
            //mes.SelectedIndex = 0;
            //anio.SelectedIndex = 0;

            this.visibles();
        }

        private void visibles()
        {
            cmdVoucher.Visibility = Visibility.Hidden;
            cmdNuevo.Visibility = Visibility.Hidden;
            //Imagen mail
            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;
        }

        private void Activa()
        {
            importe.IsEnabled = true;
            numAut.IsEnabled = true;
            numCvv.IsEnabled = true;
            numOrden.IsEnabled = true;
            nomTdc.IsEnabled = true;
            numTdc.IsEnabled = true;

            mes.IsEnabled = true;
            anio.IsEnabled = true;
            cboBanco.IsEnabled = true;
            formaPago.IsEnabled = true;

            cmdEnviar.IsEnabled = true;
            cmdNuevo.IsEnabled = true;
            cmdVoucher.IsEnabled = true;
        }
        private void desactiva()
        {
            importe.IsEnabled = false;
            numAut.IsEnabled = false;
            numCvv.IsEnabled = false;
            numOrden.IsEnabled = false;
            nomTdc.IsEnabled = false;
            numTdc.IsEnabled = false;

            mes.IsEnabled = false;
            anio.IsEnabled = false;
            cboBanco.IsEnabled = false;
            formaPago.IsEnabled = false;
            cmdEnviar.IsEnabled = false;
            cmdNuevo.IsEnabled = false;
            cmdVoucher.IsEnabled = false;
        }

        private void numTdc_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.num_tarjeta))
                this.numTdc.Text = this.num_tarjeta;
            else
                this.numTdc.Text = string.Empty;
        }
        private void numTdc_lostfocus(object sender, RoutedEventArgs e)
        {
            if(Globales.noMostrarMensajes)return;
            if (!string.IsNullOrWhiteSpace(this.numTdc.Text))
            {
                if (numTdc.Text.Length >= 14)
                {
                    this.num_tarjeta = this.numTdc.Text;
                    this.numTdc.Text = string.Format("{0}******{1}", numTdc.Text.Substring(0, 6), numTdc.Text.Substring(numTdc.Text.Length - 4));
                }
                else
                {
                    Globales.MessageBoxMit("Favor de validar el número de tarjeta");
                    this.num_tarjeta = string.Empty;
                    this.numTdc.Text = string.Empty;
                    this.numCvv.Password = string.Empty;
                    control = sender as TextBox;
                    return;
                }
            }
            else
            {
                this.num_tarjeta = string.Empty;
                this.numTdc.Text = string.Empty;
                return;
            }

            if (!string.IsNullOrWhiteSpace(this.num_tarjeta))
            {
                Globales.cpIntegraEMV.dbgSetTipoPago(3);
                Globales.merchantMoto = Globales.cpIntegraEMV.dbgGetMerchantVtaForzada(this.num_tarjeta);
                Globales.isAmex = Globales.cpIntegraEMV.dbgGetisAmex();
                if (Globales.isAmex)
                {
                    Globales.MessageBoxMit("No aplica venta forzada para American Express");
                    this.numTdc.Text = string.Empty;
                    this.num_tarjeta = string.Empty;
                    return;

                }
                FormaPago_click();
                numCvv.Password = "";
                if (Globales.isAmex)
                {
                    numCvv.MaxLength = 4;
                }
                else
                {
                    numCvv.MaxLength = 3;
                }
            }
        }

        private void FormaPago_click()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".FormaPago_click()";
            try
            {
                if (Globales.GetDataXml("MXN", TypeUsuario.CadenaXML).Contains(Globales.merchantMoto))
                {
                    lblMoneda.Content = "MXN";
                }
                else
                {
                    lblMoneda.Content = "USD";
                }
            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
            }
        }

        private void soloLetra(object sender, TextCompositionEventArgs e)
        {
            Regex es = new Regex("[^a-zA-Z]+");
            e.Handled = es.IsMatch(e.Text);
        }

        private void convertirMayusculas(object sender, RoutedEventArgs e)
        {
            TextBox texto = (TextBox)sender;
            texto.Text = texto.Text.ToUpper();
        }

        private void cvv_lostfocus(object sender, RoutedEventArgs e)
        {
            if (numCvv.Password != "000" || numCvv.Password != "0000")
            {
                if (Globales.isAmex && numCvv.Password.Length != 4)
                {
                    Globales.MessageBoxMit("El código de seguridad debe ser de 4 dígitos");
                    return;
                }
                else if (!Globales.isAmex && numCvv.Password.Length != 3)
                {
                    Globales.MessageBoxMit("El código de seguridad debe ser de 3 dígitos para V/MC");
                    return;
                }
            }
            else
            {
                Globales.MessageBoxMit("El CVV es incorrecto, favor de validar");
                return;
            }
        }

        private void soloLetraNumeros(object sender, TextCompositionEventArgs e)
        {
            Regex es = new Regex("[^a-zA-Z0-9]+");
            e.Handled = es.IsMatch(e.Text);
        }

        private void cmdNuevoClick(object sender, RoutedEventArgs e)
        {
            lblMoneda.Content = "";
            visibles();
            limpia();
            numTdc.Visibility = Visibility.Visible;
            cmdEnviar.Visibility = Visibility.Visible;
            numTdc.Focus();


        }

        private void cmdVoucher_click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "cmdVoucher_click;";
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                if (TypeUsuario.IsAQ)
                {
                    //Verificar voucher 
                    return;
                }

                switch (TypeUsuario.TipoImpresora)
                {
                    case "1":
                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        cmdVoucher.IsEnabled = true;
                        cmdNuevo.IsEnabled = true;
                        break;
                    case "2":
                        break;
                    case "3":
                        Globales.imprimirEpson();
                        break;
                    case "4":
                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        cmdVoucher.IsEnabled = true;
                        cmdNuevo.IsEnabled = true;
                        break;
                    case "5":
                        break;
                    case "6":
                        cmdNuevo.IsEnabled = false;
                        cmdVoucher.IsEnabled = false;
                        Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                        cmdVoucher.IsEnabled = true;
                        cmdNuevo.IsEnabled = true;
                        break;
                    default:
                        Globales.MessageBoxMit("No ha definido un tipo de iompresora\nPara seleccionarla vnaya al menú de administracion.");
                        break;
                }
            }
            catch
            {
                Mouse.OverrideCursor = null;
            }
            Mouse.OverrideCursor = null;
        }

        private void cmdEnviar_click(object sender, RoutedEventArgs e)
        {
            string strTypeC = "";
            string strCadEncriptar = "";
            label_6.Visibility = Visibility.Hidden;
            formaPago.IsEnabled = false;
            formaPago.Visibility = Visibility.Hidden;
            cmdEnviar.IsEnabled = false;
            if (string.IsNullOrWhiteSpace(this.num_tarjeta))
            {
                cmdEnviar.IsEnabled = true;
                Globales.MessageBoxMit("Insertar número de tarjeta.");
                return;
            }

            if (string.IsNullOrWhiteSpace(nomTdc.Text))
            {
                Globales.MessageBoxMit("insertar nombre tarjeta habiente");
                cmdEnviar.IsEnabled = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(nomTdc.Text))
            {
                Globales.MessageBoxMit("Insertar nombre tarjeta habiente");
                cmdEnviar.IsEnabled = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(numOrden.Text))
            {
                Globales.MessageBoxMit("Insertar referencia");
                cmdEnviar.IsEnabled = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(numAut.Text))
            {
                Globales.MessageBoxMit("Insertar número de autorización");
                cmdEnviar.IsEnabled = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(importe.Text))
            {
                Globales.MessageBoxMit("Colocar importe");
                cmdEnviar.IsEnabled = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(mes.Text))
            {
                Globales.MessageBoxMit("Colocar mes");
                cmdEnviar.IsEnabled = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(anio.Text))
            {
                Globales.MessageBoxMit("Colocar año");
                cmdEnviar.IsEnabled = true;
                return;
            }
            if (numAut.Text.Length != 6)
            {
                Globales.MessageBoxMit("EL numero de Autorizacion debe ser de 6 elementos");
                cmdEnviar.IsEnabled = true;
                return;
            }

          

            if (string.IsNullOrWhiteSpace(Globales.merchantMoto))
            {
                Globales.MessageBoxMit("No hay planes de pago para esta tarjeta, porfavor cambie la tarjeta.");
                cmdEnviar.IsEnabled = true;
                return;
            }

            if (Convert.ToInt16(anio.Text) < Convert.ToInt16(DateTime.Now.Year.ToString().Substring(2, 2)))
            {
                Globales.MessageBoxMit("Tarjeta vencida.");
                cmdEnviar.IsEnabled = true;
                return;
            }
            else if ((Convert.ToInt16(anio.Text) == Convert.ToInt16(DateTime.Now.Year.ToString().Substring(2, 2)))
                && Convert.ToInt16(mes.Text) < Convert.ToInt16(DateTime.Now.Month.ToString()))
            {
                Globales.MessageBoxMit("Tarjeta vencida");
                cmdEnviar.IsEnabled = true;
                return;
            }

            string num_cvv = string.Empty;

            if (Globales.isAmex)
            {
                strTypeC = "AMEX";
                num_cvv = "0000";
            }
            else
            {
                strTypeC = "V/MC";
                num_cvv = "000";
            }
            try
            {

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                desactiva();
                Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                Globales.cpIntegraEMV.sndVtaFzadaMOTO(TypeUsuario.usu, TypeUsuario.Pass, "", TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
                    TypeUsuario.country, Globales.merchantMoto, numOrden.Text, "18", importe.Text, Convert.ToString(lblMoneda.Content), numAut.Text,
                    strTypeC, nomTdc.Text, this.num_tarjeta, mes.Text, anio.Text.Substring(2, 2), num_cvv);

                Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();
                Mouse.OverrideCursor = null;
                switch (Globales.cpIntegraEMV.getRspDsResponse())
                {
                    case "approved":
                        visibles();

                        TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult);

                        Globales.MessageBoxMitApproved(Globales.GetDataXml("auth", Globales.cpIntegracion_sResult));

                        this.escogerImpresora();
                        if (TypeUsuario.IsAQ)
                            Globales.VerificaVoucher();

                        cmdVoucher.IsEnabled = true;
                        cmdVoucher.Visibility = Visibility.Visible;
                        cmdNuevo.IsEnabled = true;
                        cmdNuevo.Visibility = Visibility.Visible;
                        cmdEnviar.Visibility = Visibility.Hidden;

                        this.BENVIAMAIL.Visibility = (TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden);
                        this.BENVIAMAIL.Tag = this.nomTdc.Text;
                        //   Globales.setMensaje("approved","Operación exitosa");
                        if (Globales.FacturaE == "1")
                        {
                            bool confirmar = Globales.MessageConfirm("¿Desea Factura Electrónica?");
                            if (confirmar)
                            {
                                abrirFrmNuevo(new frmPreguntaFactura()
                                {
                                    abrirFrmAhora = abrirFrmNuevo
                                });
                                return;
                            }
                            else
                            {
                                Globales.XMLFacturaE = "";
                            }
                        }
                        break;
                    case "denied":
                        visibles();

                        string mensa = Globales.msjRech + "\n" + Globales.GetDataXml("cd_response", Globales.cpIntegracion_sResult);

                        cmdNuevo.IsEnabled = true;
                        cmdEnviar.Visibility = Visibility.Hidden;
                        cmdNuevo.Visibility = Visibility.Visible;
                        Globales.MessageBoxMitDenied("Cobro denegado\n" + Globales.GetDataXml("cd_response", Globales.cpIntegracion_sResult));
                        break;
                    case "error":
                        visibles();

                        string error = Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult);
                        Globales.MessageBoxMitError(Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult));

                        cmdNuevo.IsEnabled = true;
                        cmdNuevo.Visibility = Visibility.Visible;
                        cmdEnviar.Visibility = Visibility.Hidden;
                        cmdVoucher.IsEnabled = false;
                        break;
                    default:
                        Activa();
                        visibles();

                        cmdNuevo.IsEnabled = true;
                        cmdNuevo.Visibility = Visibility.Visible;
                        cmdEnviar.Visibility = Visibility.Hidden;
                        cmdVoucher.IsEnabled = false;
                        Globales.MessageBoxMitError("Verifique su conexión de internet");
                        break;
                }
            }
            catch
            {
                Mouse.OverrideCursor = null;
            }
            Mouse.OverrideCursor = null;
        }

        private void escogerImpresora()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            switch (TypeUsuario.TipoImpresora)
            {
                case "1":
                    if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult),
                                          TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                    {
                        TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        cmdVoucher.IsEnabled = true;
                        cmdNuevo.IsEnabled = true;
                    }
                    break;
                case "2":
                    break;
                case "3":
                    Globales.imprimirEpson();
                    break;
                case "4":
                    if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult),
                                          TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                    {
                        TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        cmdVoucher.IsEnabled = true;
                        cmdNuevo.IsEnabled = true;
                    }
                    break;
                case "5":
                    break;
                case "6":
                    Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                    cmdVoucher.IsEnabled = true;
                    cmdNuevo.IsEnabled = true;
                    break;
                default:
                    break;
            }
            Mouse.OverrideCursor = null;
        }

        private void importe_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void previewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Back)
                return;

            if (string.IsNullOrWhiteSpace(this.num_tarjeta))
            {
                Globales.MessageBoxMit("Ingrese número de tarjeta.");
                e.Handled = true;
            }
        }

        private void numeros(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }
    }
}
