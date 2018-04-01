using PcPay.Code.Usuario;
using PcPay.Forms.formularios;
using PcPay.Forms.Formularios;
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
    /// Interaction logic for frmAVSs2.xaml
    /// </summary>
    public partial class frmAVSs2 : Page
    {
        public string NOMBRE_GENERAL = "Formulario AVSs2";
        public string Caption { get; set; }
        public abrirFrm abrirFrmNuevo;
        public cerrarVentana cerrar;
        public string num_tarjeta { get; set; }
        private DispatcherTimer tiempo;
        public frmAVSs2()
        {
            InitializeComponent();

            #region"**********Datos de persona Efecto**********"
            this.txtNombre.GotFocus += Globales.setFocusMit2;
            this.txtNombre.LostFocus += Globales.lostFocusMit2;

            this.txtPaterno.GotFocus += Globales.setFocusMit2;
            this.txtPaterno.LostFocus += Globales.lostFocusMit2;

            this.txtMaterno.GotFocus += Globales.setFocusMit2;
            this.txtMaterno.LostFocus += Globales.lostFocusMit2;

            this.txtLadaPersonal.GotFocus += Globales.setFocusMit2;
            this.txtLadaPersonal.LostFocus += Globales.lostFocusMit2;

            this.txtTelefonoPersonal.GotFocus += Globales.setFocusMit2;
            this.txtTelefonoPersonal.LostFocus += Globales.lostFocusMit2;

            this.txtEmail.GotFocus += Globales.setFocusMit2;
            this.txtEmail.LostFocus += Globales.lostFocusMit2;
            #endregion
            #region "**********Datos tarjeta Efectos**********"
            this.numTdc.GotFocus += Globales.setFocusMit2;
            this.numTdc.LostFocus += Globales.lostFocusMit2;

            this.Mes.GotFocus += Globales.setFocusMit2;
            this.Mes.LostFocus += Globales.lostFocusMit2;

            this.Anio.GotFocus += Globales.setFocusMit2;
            this.Anio.LostFocus += Globales.lostFocusMit2;

            this.nomTdc.GotFocus += Globales.setFocusMit2;
            this.nomTdc.LostFocus += Globales.lostFocusMit2;

            this.numCvv.GotFocus += Globales.setFocusMit2;
            this.numCvv.LostFocus += Globales.lostFocusMit2;

            this.numOrden.GotFocus += Globales.setFocusMit2;
            this.numOrden.LostFocus += Globales.lostFocusMit2;

            this.importe.GotFocus += Globales.setFocusMit2;
            this.importe.LostFocus += Globales.lostFocusMit2;
            #endregion
            #region"**********Datos direccion Efecto**********"

            this.txtCalle.GotFocus += Globales.setFocusMit2;
            this.txtExt.GotFocus += Globales.setFocusMit2;
            this.txtInt.GotFocus += Globales.setFocusMit2;
            this.cboColonia.GotFocus += Globales.setFocusMit2;
            this.txtCP.GotFocus += Globales.setFocusMit2;
            this.txtDelMun.GotFocus += Globales.setFocusMit2;
            this.txtCiudad.GotFocus += Globales.setFocusMit2;
            this.txtEstado.GotFocus += Globales.setFocusMit2;
            this.txtPais.GotFocus += Globales.setFocusMit2;


            this.txtCalle.LostFocus += Globales.lostFocusMit2;
            this.txtExt.LostFocus += Globales.lostFocusMit2;
            this.txtInt.LostFocus += Globales.lostFocusMit2;
            this.cboColonia.LostFocus += Globales.lostFocusMit2;
            this.txtCP.LostFocus += Globales.lostFocusMit2;
            this.txtDelMun.LostFocus += Globales.lostFocusMit2;
            this.txtCiudad.LostFocus += Globales.lostFocusMit2;
            this.txtEstado.LostFocus += Globales.lostFocusMit2;
            this.txtPais.LostFocus += Globales.lostFocusMit2;

            #endregion


            tiempo = new DispatcherTimer();
            this.txtLadaPersonal.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            this.txtTelefonoPersonal.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            this.numCvv.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            this.numTdc.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            this.importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;

            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;
            this.BENVIAMAIL.MouseDown += Globales.sendMail_GridMouseDown;
        }
        private void numTdc_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.num_tarjeta))
                this.numTdc.Text = this.num_tarjeta;
            else
                this.numTdc.Text = string.Empty;
        }
        private Control control;
        private bool isCvv;
        private void numTdcLostFocus(object sender, RoutedEventArgs e)
        {
            if (Globales.noMostrarMensajes) return;
            if (!string.IsNullOrWhiteSpace(numTdc.Text))
            {
                if (numTdc.Text.Length >= 14)
                {
                    this.num_tarjeta = this.numTdc.Text;
                    this.numTdc.Text = string.Format("{0}******{1}", numTdc.Text.Substring(0, 6), numTdc.Text.Substring(numTdc.Text.Length - 4));
                }
                else
                {
                    Globales.MessageBoxMit("Favor de validar número tarjeta.");
                    numTdc.Text = "";
                    num_tarjeta = "";
                    control = sender as TextBox;
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(this.num_tarjeta))
            {
                Globales.cpIntegraEMV.dbgSetTipoPago(2);
                Globales.merchantMoto = Globales.cpIntegraEMV.dbgGetMerchantAvs(this.num_tarjeta);
                Globales.isAmex = Globales.cpIntegraEMV.dbgGetisAmex();
                FormaPago_Click();
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

        private void FormaPago_Click()
        {
            if (Globales.GetDataXml("MXN", TypeUsuario.CadenaXML).ToUpper().Contains(Globales.merchantMoto.ToUpper()))
            {
                lblMoneda.Content = "MXN";
            }
            else
            {
                lblMoneda.Content = "USD";
            }
        }

        public bool desactivar { get; set; }

        private void cmdFWDClick(object sender, RoutedEventArgs e)
        {
            desactivar = true;
            Globales.strNombreFP = NOMBRE_GENERAL + "CMDfwd()";
            if (string.IsNullOrWhiteSpace(this.num_tarjeta))
            {
                Globales.MessageBoxMit("Introduzca el número de la tarjeta");
            }
            else if (Mes.SelectedIndex == -1)
            {
                Globales.MessageBoxMit("Seleccione el mes de vencimiento de la tarjeta.");
                Mes.Focus();
            }
            else if (Anio.SelectedIndex == -1)
            {
                Globales.MessageBoxMit("Seleccione el año de vencimiento de la tarjeta.");
                Anio.Focus();
            }
            else if (string.IsNullOrWhiteSpace(nomTdc.Text))
            {
                Globales.MessageBoxMit("Introduzca el nombre del titular.");
                nomTdc.Focus();
            }
            else if (string.IsNullOrWhiteSpace(numCvv.Password) && !(Globales.isAgencias || Globales.isAerolinea))
            {
                Globales.MessageBoxMit("Introduzca el Code de la tarjeta");
                this.numCvv.Focus();
            }
            else if ((numCvv.Password == "0000" || numCvv.Password == "000") && numCvv.Visibility == Visibility.Visible)
            {
                Globales.MessageBoxMit("Código de seguridad inválido.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(Globales.merchantMoto))
            {
                Globales.MessageBoxMit("No hay planes de pago para esta tarjeta, por favor cambie la tarjeta.");
                //return;
            }
            else if (string.IsNullOrWhiteSpace(numOrden.Text))
            {
                Globales.MessageBoxMit("Introduzca " + TypeUsuario.reference + ".");
                numOrden.Focus();
            }
            else if (string.IsNullOrWhiteSpace(importe.Text))
            {
                Globales.MessageBoxMit("Introduzca el importe.");
                importe.Focus();
            }
            else
            {
                if (TypeUsuario.Id_Company == "0059")
                {
                    if (numOrden.Text.Length != 28)
                    {
                        Globales.MessageBoxMit("El no. de REFERENCIA debe se de 28 posiciones.");
                        numOrden.Focus();
                        return;
                    }
                }

                if (Convert.ToInt16(Anio.Text) < Convert.ToInt16(DateTime.Now.Year.ToString().Substring(2, 2)))
                {
                    Globales.MessageBoxMit("Tarjeta vencida.");
                    return;
                }
                else if (Convert.ToInt16(Anio.Text) == Convert.ToInt16(DateTime.Now.Year.ToString().Substring(2, 2)) && Convert.ToInt16(Mes.Text) < Convert.ToInt16(DateTime.Now.Month))
                {
                    Globales.MessageBoxMit("Tarjeta vencida.");
                    return;
                }

                if (Globales.isAmex)
                {
                    if (numCvv.MaxLength != 4 && !(Globales.isAgencias || Globales.isAerolinea))
                    {
                        Globales.MessageBoxMit("Introduzca el Code de la tarjea");
                        numCvv.Focus();
                        return;
                    }
                }
                else
                {
                    if (numCvv.MaxLength != 3 && !(Globales.isAgencias || Globales.isAerolinea))
                    {
                        Globales.MessageBoxMit("Introduzca el Code de la tarjea");
                        numCvv.Focus();
                        return;
                    }
                }
                double esNumero = 0;
                if (!double.TryParse(importe.Text, out esNumero))
                {
                    Globales.MessageBoxMit("El importe debe ser numérico");
                    importe.Focus();
                    return;
                }
                Globales.MessageBoxMit("Introduzca el domicilio tal como aparece en el estado de cuenta.");
                fraDireccion.Visibility = Visibility.Visible;
                fraCliente.Visibility = Visibility.Hidden;
                txtCalle.Focus();
            }
        }

        private void cmdRevClick(object sender, RoutedEventArgs e)
        {
            desactivar = false;
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdRev_click";
            fraCliente.Visibility = Visibility.Visible;
            fraDireccion.Visibility = Visibility.Hidden;
        }

        private void cmdEnviarClick(object sender, RoutedEventArgs e)
        {
            cmdEnviar.IsEnabled = false;
            cmdEnviar.IsEnabled = true;
            string isCheckin = "";
            if (string.IsNullOrWhiteSpace(txtCalle.Text))
            {
                Globales.MessageBoxMit("Introduzca la calle del domicilio");
                txtCalle.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtExt.Text))
            {
                Globales.MessageBoxMit("Introduzca el número exterior del domicilio.");
                txtExt.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtCP.Text))
            {
                Globales.MessageBoxMit("Introduzca el código postal del domicilio");
                txtCP.Focus();
            }
            else if (string.IsNullOrWhiteSpace(cboColonia.Text))
            {
                Globales.MessageBoxMit("Introduzca la colonia del domicilio.");
                cboColonia.Focus();

            }
            else if (string.IsNullOrWhiteSpace(txtDelMun.Text))
            {
                Globales.MessageBoxMit("Introduzca la delefación o municipio del domicilio.");
                txtDelMun.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtCiudad.Text))
            {
                Globales.MessageBoxMit("Introduzca la ciudad del domicilio.");
                txtCiudad.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtEstado.Text))
            {
                Globales.MessageBoxMit("Introduzca el estado del domicilio.");
                txtEstado.Focus();
            }
            else
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                cmdEnviar.IsEnabled = false;
                string strTypeC = "";
                //Cpintegracion_clear
                if (Globales.isAmex)
                {
                    strTypeC = "AMEX";
                }
                else
                {
                    strTypeC = "V/MC";
                }

                Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL;
                //cpIntegracion_sURL_cpIntegra = url_Dll
                string strTpIperacion = "17";
                if (Globales.isAerolinea && !Globales.isVentasPropias)
                {
                    isCheckin = "1";
                }
                else
                {
                    isCheckin = "";
                }
                //Se agrega validacion para poner como opcionar el CVV para agencias y aerepñomeas AG
                if ((Globales.isAgencias || Globales.isAerolinea) && numCvv.Password == "" && TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
                {
                    if (strTypeC == "V/MC")
                    {
                        numCvv.Password = "000";
                    }
                    else
                    {
                        numCvv.Password = "0000";
                    }
                }
                bool operacion = true;
                //bool operacion = 

                string strCadEncriptar = "";

                //operacion = Globales.cpIntegraEMV.cpIntegracion_cpAVSs2(TypeUsuario.Id_Company,TypeUsuario.Id_Branch,TypeUsuario.country,TypeUsuario.usu,TypeUsuario.Pass,Globales.merchantMoto,
                //    numOrden.Text, strTpIperacion, strTypeC, nomTdc.Text, num_tarjeta, Mes.Text, Anio.Text, numCvv.Password, importe.Text, Convert.ToString(lblMoneda.Content), txtCalle.Text, txtInt.Text, txtExt.Text, txtDelMun.Text, txtCiudad.Text, txtEstado.Text, txtCP.Text, cboColonia.Text, txtNombre.Text + " " + txtPaterno.Text + " " + txtMaterno.Text, txtPais.Text,
                //    txtLadaPersonal.Text+txtTelefonoPersonal.Text,txtEmail.Text,isCheckin);
                var xml = Globales.cpIntegracon_cpAVSs2(TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country,
                     TypeUsuario.usu,
                     TypeUsuario.Pass,
                     Globales.merchantMoto,
                     numOrden.Text,
                     strTpIperacion,
                     strTypeC,
                     nomTdc.Text,
                     this.num_tarjeta,
                     Mes.Text,
                     Anio.Text.Substring(2, 2),
                     numCvv.Password,
                     importe.Text,
                     Convert.ToString(lblMoneda.Content),
                     txtCalle.Text,
                     txtInt.Text,
                     txtExt.Text,
                     txtDelMun.Text,
                     txtCiudad.Text,
                     txtEstado.Text,
                     txtCP.Text,
                     cboColonia.Text,
                     txtNombre.Text + " " + txtPaterno.Text + " " + txtMaterno.Text,
                     txtPais.Text, txtLadaPersonal.Text + "" + txtTelefonoPersonal.Text, txtEmail.Text, isCheckin);
                if (operacion)
                {
                    cmdRev.IsEnabled = false;
                    xml = xml.Replace("&lt;", "<");
                    Mouse.OverrideCursor = null;
                    switch (Globales.GetDataXml("response", xml))
                    {
                        case "approved":
                            numTdc.IsEnabled = false;
                            Mes.IsEnabled = false;
                            Anio.IsEnabled = false;
                            nomTdc.IsEnabled = false;
                            numCvv.IsEnabled = false;
                            cboBanco.IsEnabled = false;
                            formaPago.IsEnabled = false;
                            numOrden.IsEnabled = false;
                            importe.IsEnabled = false;
                            txtCalle.IsEnabled = false;
                            txtExt.IsEnabled = false;
                            txtInt.IsEnabled = false;
                            txtCP.IsEnabled = false;
                            cboColonia.IsEnabled = false;

                            txtDelMun.IsEnabled = false;
                            txtCiudad.IsEnabled = false;
                            txtEstado.IsEnabled = false;
                            txtPais.IsEnabled = false;

                            Globales.MessageBoxMitApproved(Globales.GetDataXml("auth", xml));
                            //Se agrega la validación para chek out
                            if (Globales.isAerolinea && !Globales.isVentasPropias)
                            {
                                frmBoletosAerolinea frmBoletosAereolinea = new frmBoletosAerolinea();
                                frmBoletosAereolinea.Importe = Globales.GetDataXml("amount", xml);
                                frmBoletosAereolinea.Importe = Globales.GetDataXml("auth", xml);
                                frmBoletosAereolinea.Importe = Globales.GetDataXml("foliocpagos", xml);
                                frmBoletosAereolinea.ShowDialog();
                            }
                            else
                            {
                                TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher", xml);
                                string tipoImpresora = TypeUsuario.TipoImpresora;
                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                switch (tipoImpresora)
                                {
                                    case "1":
                                        if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", xml),
                                            TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                                        {
                                            TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                                            Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                                            cmdVoucher.IsEnabled = true;
                                            cmdNuevo.IsEnabled = true;
                                        }
                                        break;
                                    case "3":
                                        Globales.imprimirEpson();
                                        break;
                                    case "4":
                                        if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", xml),
                                           TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                                        {
                                            TypeUsuario.strVoucher = Globales.VoucherHtml1(Globales.cpHTTP_sResult);
                                            Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                                            cmdVoucher.IsEnabled = true;
                                            cmdNuevo.IsEnabled = true;
                                        }
                                        break;
                                    case "6":
                                        Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                                        cmdVoucher.IsEnabled = true;
                                        cmdNuevo.IsEnabled = true;
                                        break;
                                    default:
                                        Globales.MessageBoxMit("No existe opción de impresora.");
                                        break;
                                }
                                Mouse.OverrideCursor = null;
                                if (TypeUsuario.IsAQ)
                                {
                                    Globales.VerificaVoucher();
                                }

                                cmdVoucher.IsEnabled = true;
                                cmdNuevo.Visibility = Visibility.Visible;
                                cmdEnviar.Visibility = Visibility.Hidden;

                                this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;
                                this.BENVIAMAIL.Tag = this.nomTdc.Text;

                                if (Globales.FacturaE == "1")
                                {

                                    if (Globales.MessageConfirm("¿Desea factura electrónica?"))
                                    {
                                       
                                        abrirFrmNuevo(new frmPreguntaFactura()
                                        {
                                            abrirFrmAhora = abrirFrmNuevo
                                            ,cerraPage = cerrar
                                        });
                                        Mouse.OverrideCursor = null;
                                        return;
                                    }
                                    else
                                    {
                                        Globales.XMLFacturaE = "";
                                    }
                                }

                            }

                            break;
                        case "denied":
                            numTdc.IsEnabled = false;
                            Mes.IsEnabled = false;
                            Anio.IsEnabled = false;
                            nomTdc.IsEnabled = false;
                            numCvv.IsEnabled = false;
                            cboBanco.IsEnabled = false;
                            formaPago.IsEnabled = false;
                            numOrden.IsEnabled = false;
                            importe.IsEnabled = false;
                            txtCalle.IsEnabled = false;
                            txtExt.IsEnabled = false;
                            txtInt.IsEnabled = false;
                            txtCP.IsEnabled = false;
                            cboColonia.IsEnabled = false;
                            txtDelMun.IsEnabled = false;
                            txtCiudad.IsEnabled = false;
                            txtEstado.IsEnabled = false;
                            txtPais.IsEnabled = false;

                            Globales.MessageBoxMitDenied("Cobro rechazado \n" + Globales.GetDataXml("cd_response", xml));
                            cmdNuevo.Visibility = Visibility.Visible;
                            cmdEnviar.Visibility = Visibility.Hidden;
                            break;
                        case "error":
                            Globales.MessageBoxMitError("Error al cobrar");
                            cmdNuevo.Visibility = Visibility.Hidden;
                            cmdEnviar.Visibility = Visibility.Visible;
                            break;
                        default:
                            Globales.MessageBoxMitError("Verifique su conexión de internet.");
                            cmdNuevo.Visibility = Visibility.Hidden;
                            cmdEnviar.Visibility = Visibility.Visible;
                            break;

                    }
                    Mouse.OverrideCursor = null;
                }
                else
                {
                    Globales.MessageBoxMitError(Globales.cpIntegracion_sError);
                    cmdNuevo.Visibility = Visibility.Hidden;
                    cmdEnviar.Visibility = Visibility.Visible;
                    Mouse.OverrideCursor = null;
                }
                Mouse.OverrideCursor = null;
                cmdEnviar.IsEnabled = true;
            }
        }

        private void cargandoVentana(object sender, RoutedEventArgs e)
        {
            lblMoneda.Content = "";
            Globales.cpIntegraEMV.dbgEndOperation();
            Globales.strNombreFP = NOMBRE_GENERAL + "limpia();";
            //Cosas de intkindoImage y un case checar código de visual basic
            if (Globales.isAerolinea && !Globales.isVentasPropias)
            {
                Caption = "VENTA DE BOLETOS: Cargo a tarjeta bancaria vía telefónica.";
            }
            this.limpia();
            fraDireccion.Visibility = Visibility.Hidden;
            fraCliente.Visibility = Visibility.Visible;

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

        private void mayuscula(object sender, RoutedEventArgs e)
        {
            TextBox texto = sender as TextBox;
            texto.Text = texto.Text.ToUpper().Trim();
        }

        private void soloNumero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }

        private void soloLetrasNumeros(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9A-Za-z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void soloNumeroDecimal(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }

        private void importeLostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
        }

        private void soloLetras(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cmdnuevoClick(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdnuevoClick()";
            try
            {
                lblMoneda.Content = "";
                numTdc.Text = "";
                num_tarjeta = "";
                numTdc.IsEnabled = true;
                numTdc.Visibility = Visibility.Visible;
                this.num_tarjeta = string.Empty;

                Mes.IsEnabled = true;
                Anio.IsEnabled = true;
                nomTdc.IsEnabled = true;
                cboBanco.IsEnabled = true;
                numCvv.IsEnabled = true;
                label_6.IsEnabled = false;
                formaPago.IsEnabled = false;
                formaPago.Visibility = Visibility.Hidden;
                numOrden.IsEnabled = true;
                importe.IsEnabled = true;
                txtCalle.IsEnabled = true;
                txtExt.IsEnabled = true;
                txtInt.IsEnabled = true;
                txtCP.IsEnabled = true;
                cboColonia.IsEnabled = true;
                txtDelMun.IsEnabled = true;
                txtCiudad.IsEnabled = true;
                txtEstado.IsEnabled = true;
                txtPais.IsEnabled = true;
                limpia();
                TypeUsuario.strVoucher = "";

                cmdRev.IsEnabled = true;
                cmdRevClick(null, null);

            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
            }

        }

        private void limpia()
        {

            Globales.strNombreFP = NOMBRE_GENERAL + "limpia();";
            try
            {
                txtNombre.Text = "";
                txtPaterno.Text = "";
                txtMaterno.Text = "";
                txtLadaPersonal.Text = "";
                txtTelefonoPersonal.Text = "";
                txtEmail.Text = "";

                numTdc.Text = "";
                num_tarjeta = "";
                this.num_tarjeta = string.Empty;
                this.BENVIAMAIL.Tag = string.Empty;
                this.BENVIAMAIL.Visibility = Visibility.Hidden;
                numCvv.Password = "";
                nomTdc.Text = "";
                importe.Text = "";
                txtCalle.Text = "";
                txtInt.Text = "";
                txtCP.Text = "";
                txtExt.Text = "";
                numOrden.Text = "";

                txtDelMun.Text = "";
                txtEstado.Text = "";
                txtPais.Text = "";
                txtCiudad.Text = "";
                cboColonia.Text = "";
                cmdVoucher.IsEnabled = false;

                label_8.Content = TypeUsuario.reference.Substring(0, 1).Trim().ToUpper() + TypeUsuario.reference.Substring(1).Trim().ToLower();
                formaPago.Items.Clear();
                cboBanco = Globales.obtieneBancos(cboBanco, Globales.GetDataXml("catbancos", TypeUsuario.CadenaXML));




                Anio.Items.Clear();
                Mes.Items.Clear();
                for (int x = 0; x <= 10; x++)
                {
                    Anio.Items.Add((DateTime.Now.Year + x).ToString());
                }
                for (int x = 1; x <= 12; x++)
                {
                    string aux = string.Empty;
                    if (x < 10) aux = "0";
                    aux += Convert.ToString(x);
                    Mes.Items.Add(aux);
                }

                cmdEnviar.Visibility = Visibility.Visible;
                cmdNuevo.Visibility = Visibility.Hidden;

                if (TypeUsuario.Id_Company == "0059")
                    numOrden.MaxLength = 28;
                else
                    numOrden.MaxLength = Globales.MAXCAR;
            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
            }
        }

        private void imprimeVoucherClick(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "ImprimeVoucherClick();";
            try
            {
                if (TypeUsuario.IsAQ)
                {
                    Globales.VerificaVoucher();
                    return;
                }

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                switch (TypeUsuario.TipoImpresora)
                {
                    case "1":
                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        break;
                    case "2":
                        break;
                    case "3":
                        Globales.imprimirEpson();
                        break;
                    case "4":
                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        break;
                    case "5":
                        break;
                    case "6":
                        cmdVoucher.IsEnabled = false;
                        cmdNuevo.IsEnabled = false;
                        Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                        cmdVoucher.IsEnabled = true;
                        cmdNuevo.IsEnabled = true;
                        break;
                    default:
                        Globales.MessageBoxMit("No ha definido un tipo de impresora.\nPara seleccionarla vaya al menú de administración.");
                        break;
                }
            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
            }
            Mouse.OverrideCursor = null;
        }

        private void soloNumero1(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void numOrden_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

        private void importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void txtMascara_GotFocus(object sender, RoutedEventArgs e)
        {
            numTdc.Visibility = Visibility.Visible;
            numTdc.Focus();
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void txtCP_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void nomTdc_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.num_tarjeta))
            {
                Globales.MessageBoxMit("Ingrese número de tarjeta.");
                e.Handled = true;
            }
        }

        private void numero(object sender, TextCompositionEventArgs e)
        {
            System.Windows.Controls.PasswordBox texto = (System.Windows.Controls.PasswordBox)sender;
            Regex es;
            es = new Regex("[^0-9]+");
            e.Handled = es.IsMatch(e.Text);
        }

        private void numOrden_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TypeUsuario.Id_Company == "0059" && numOrden.IsEnabled)
            {
                TypeUsuario.strRefReaDig = "MA";
                importe.Focus();
                frmReaderDigest reader = new frmReaderDigest();
                reader.ShowDialog();
                numOrden.Text = Globales.referenciaAux;
            }
        }

        private void numeros(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender,e);
        }

        private void codeLostFocus(object sender, RoutedEventArgs e)
        {
            if (Globales.noMostrarMensajes) return;
            if(Globales.isAerolinea && string.IsNullOrWhiteSpace(numCvv.Password)){
                return;
            }
            if(string.IsNullOrWhiteSpace(numCvv.Password))return;
            if (numCvv.Password != "000" && numCvv.Password != "0000")
            {
                if (Globales.isAmex && numCvv.Password.Length != 4)
                {
                    Globales.MessageBoxMit("El código de seguridad debe ser de 4 dígitos para AMEX");
                    isCvv = true;
                    control = sender as PasswordBox;
                }
                else if (!Globales.isAmex && numCvv.Password.Length != 3)
                {
                    Globales.MessageBoxMit("El código de seguridad debe ser de 3 dígitos para V/MC");
                    isCvv = true;
                    control = sender as PasswordBox;
                }
            }
            else {
                Globales.MessageBoxMit("El cvv es incorrecto, favor de validar");
                isCvv = true;
                control = sender as PasswordBox;
            }
        }

    }
}
