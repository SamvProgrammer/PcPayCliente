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
    /// Interaction logic for frmMotoAgencia.xaml
    /// </summary>
    public partial class frmMotoAgencia : Page
    {
        public cerrarVentana cierra;
        public string num_tarjeta { get; set; }

        public string caption = "Ventas propias";
        public abrirFrm abrirFrmNuevo;
        public cerrarVentana cerrando;
        private DispatcherTimer tiempo;
        public frmMotoAgencia()
        {
            InitializeComponent();

            #region"Efectos"
            this.numTdc.GotFocus += Globales.setFocusMit2;

            this.Mes.GotFocus += Globales.setFocusMit2;
            this.Anio.GotFocus += Globales.setFocusMit2;
            this.nomTdc.GotFocus += Globales.setFocusMit2;
            this.numCvv.GotFocus += Globales.setFocusMit2;
            this.numOrden.GotFocus += Globales.setFocusMit2;
            this.importe.GotFocus += Globales.setFocusMit2;

            this.numTdc.LostFocus += Globales.lostFocusMit2;

            this.Mes.LostFocus += Globales.lostFocusMit2; ;
            this.Anio.LostFocus += Globales.lostFocusMit2;
            this.nomTdc.LostFocus += Globales.lostFocusMit2;
            this.numCvv.LostFocus += Globales.lostFocusMit2;
            this.numOrden.LostFocus += Globales.lostFocusMit2;
            this.importe.LostFocus += Globales.lostFocusMit2;

            #endregion

            this.numTdc.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            this.numCvv.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;

            tiempo = new DispatcherTimer();

            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;
            this.BENVIAMAIL.MouseDown += Globales.sendMail_GridMouseDown;
        }

        private void numTdcLostFocus(object sender, RoutedEventArgs e)
        {
            if (Globales.noMostrarMensajes) return;
            if (!string.IsNullOrWhiteSpace(numTdc.Text))
            {
                if (numTdc.Text.Length >= 14)
                {
                    this.num_tarjeta = numTdc.Text;
                    this.numTdc.Text = string.Format("{0}******{1}", numTdc.Text.Substring(0, 6), numTdc.Text.Substring(numTdc.Text.Length - 4));
                    this.Mes.Focus();
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
                this.numCvv.Password = string.Empty;
                return;
            }

            if (!string.IsNullOrWhiteSpace(this.num_tarjeta))
            {
                if (!caption.Contains("operativa"))
                {
                    Globales.merchantMoto = Globales.cpIntegraEMV.dbgGetMerchantMoto(this.num_tarjeta, "10");
                }
                else
                {
                    Globales.cpIntegraEMV.dbgSetTipoPago(1);
                    Globales.merchantMoto = Globales.cpIntegraEMV.dbgGetMerchantOpManual(this.num_tarjeta);
                }
                Globales.isAmex = Globales.cpIntegraEMV.dbgGetisAmex();
                //Forma de pago click
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

            if (Globales.isQualitas && !string.IsNullOrWhiteSpace(Globales.merchantMoto) && isVentaSinPresencia)
            {
                lblAfiliacionQ.Visibility = Visibility.Visible;
                lblAfiliacionQ.Content = lblAfiliacionQ.Content.ToString() + Globales.cpIntegraEMV.strAfiliacionQualitas();

                lblTipocobroQ.Visibility = Visibility.Visible;
                lblTipocobroQ.Content = "Tipo cobro:" + Globales.cpIntegraEMV.strTipoCobroQualitas() + "  /   Moneda:" + lblMoneda.Content.ToString();
            }
        }
        private void numTdcGotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.num_tarjeta))
                this.numTdc.Text = this.num_tarjeta;
            else
                this.numTdc.Text = string.Empty;
        }

        private void FormaPago_click()
        {
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

                if (Globales.isQualitas)
                {
                    if (typeUsuarioQualitas.TipoCobro.ToUpper() == "DEDUCIBLE")
                    {
                        lblMoneda.Content = typeUsuarioQualitas.DeducibleMoneda;
                    }
                    if (typeUsuarioQualitas.TipoCobro.ToUpper() == "POLIZA")
                    {
                        lblMoneda.Content = typeUsuarioQualitas.PolizaMoneda;
                    }

                    if (lblMoneda.Content.ToString() == "DLS")
                    {
                        lblMoneda.Content = "USD";
                    }
                }
            }
            catch
            {

            }
        }

        private void cmdNuevoClick(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".EnviarClick()";
            try
            {
                lblMoneda.Content = "";
                if (Globales.IsOM)
                {
                    if (caption.Contains("BOLETOS"))
                    {
                        cerrando();
                        //abrir el menu1_3Click --RECORDAR
                    }
                    else
                    {
                        cerrando();
                        //Abrir el mnu2_3_click
                    }
                }
                else
                {
                    numTdc.IsEnabled = true;
                    numTdc.Visibility = Visibility.Visible;

                    Mes.IsEnabled = true;
                    Anio.IsEnabled = true;
                    nomTdc.IsEnabled = true;
                    cboBanco.IsEnabled = true;
                    numCvv.IsEnabled = true;

                    label_6.Visibility = Visibility.Hidden;
                    formaPago.IsEnabled = false;
                    formaPago.Visibility = Visibility.Hidden;

                    numOrden.IsEnabled = true;
                    importe.IsEnabled = true;
                    limpia();
                    TypeUsuario.strVoucherCoP = string.Empty;
                    this.BENVIAMAIL.Visibility = Visibility.Hidden;
                    this.BENVIAMAIL.Tag = string.Empty;

                }
            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
            }
        }

        private bool isCvv = false;
        private Control control;
        private void cargandoVentana(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".CargandoVentana()";

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


            try
            {
                lblMoneda.Content = "";
                Globales.cpIntegraEMV.dbgEndOperation();
                label_8.Content = TypeUsuario.reference;
                limpia();

                
                if (Globales.isAerolinea && !Globales.isVentasPropias && Globales.IsOM)
                {
                    LTITULO.Content = "VENTA DE BOLETOS: Cargo a tarjeta bancaria sin presencia de tarjeta.";
                }
                else if (Globales.IsOM && Globales.isAerolinea && !Globales.isVentasPropias)
                {
                    LTITULO.Content = "VENTAS DE BOLETOS: Cargo a tarjeta bancaria vía operativa manual";
                }
                else
                {
                    //Código inecesario...
                }
                //Falta checkom
                //Controles para qualitas

                cmdActualizaPago.Visibility = Visibility.Hidden;
                cmdQualitasOtroCobro.Visibility = Visibility.Hidden;
                lblEmail.Visibility = Visibility.Hidden;
                txtRemesa.Visibility = Visibility.Hidden;
                cmdSalirQualitas.Visibility = Visibility.Hidden;
                imgQualitasMail.Visibility = Visibility.Hidden;
                lblAfiliacionQ.Visibility = Visibility.Hidden;
                txtEmail2.Visibility = Visibility.Hidden;
                lblEmail.Visibility = Visibility.Hidden;
                //lblAfiliacionQ.MouseLeftButtonDown = 240
                lblTipocobroQ.Visibility = Visibility.Hidden;
                if (Globales.isQualitas && isVentaSinPresencia)
                {
                    //FALTA MUCHO CÓDIGO EN EL CASO DE SER QUALITAS
                    if (Globales.isCoberturaMultiple)
                    {
                        if (Globales.nodeListXML.Item(Globales.contCoberturas).ChildNodes[0].InnerText.ToUpper() == "S")
                        {
                            typeUsuarioQualitas.DeducibleCoberturaAplicaDeducible = Globales.nodeListXML.Item(Globales.contCoberturas).ChildNodes[0].InnerText;
                            typeUsuarioQualitas.DeducibleCoberturaCodigo = Globales.nodeListXML.Item(Globales.contCoberturas).ChildNodes[1].InnerText;
                            typeUsuarioQualitas.DeducibleCoberturaDescripcion = Globales.nodeListXML.Item(Globales.contCoberturas).ChildNodes[2].InnerText;
                            typeUsuarioQualitas.DeducibleCoberturaMonto = Globales.nodeListXML.Item(Globales.contCoberturas).ChildNodes[3].InnerText;

                            //    Margin.Left = 10;
                            //    Margin.Top = 29;
                            //    Margin.Right = 0;
                            //    Margin.Bottom = 0;
                        }
                        else
                        {
                            typeUsuarioQualitas.DeducibleCoberturaCodigo = Globales.nodeListXML.Item(Globales.contCoberturas).ChildNodes[1].InnerText;
                            typeUsuarioQualitas.DeducibleCoberturaDescripcion = Globales.nodeListXML.Item(Globales.contCoberturas).ChildNodes[2].InnerText;
                            Globales.MessageBoxMit("Cobertura sin deducible \nCódigo:" + typeUsuarioQualitas.DeducibleCoberturaCodigo + "\nDescripción:" + typeUsuarioQualitas.DeducibleCoberturaDescripcion);
                            numOrden.IsEnabled = false;
                            importe.IsEnabled = false;

                            realizaCoberturaMultiple();
                        }
                    }
                    if (typeUsuarioQualitas.TipoCobro.ToUpper() != "OTROS")
                    {
                        Globales.isQualitasCierraForm = true;
                        numOrden.IsEnabled = false;
                        importe.IsEnabled = false;
                        txtEmail2.Visibility = Visibility.Visible;
                        lblEmail.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        typeUsuarioQualitas.TipoCobro = "";
                        label_8.Content = "Referencia";
                    }

                    if (typeUsuarioQualitas.TipoCobro.ToUpper() == "POLIZA")
                    {
                        label_8.Content = "No. Recibo";
                        numOrden.Text = typeUsuarioQualitas.PolizaReciboNumero;
                        importe.Text = typeUsuarioQualitas.PolizaReciboImporte;
                        numOrden2.Width = 135;


                        lblVenc.Visibility = Visibility.Visible;
                        txtVenc2.Visibility = Visibility.Visible;
                        txtVenc.Text = typeUsuarioQualitas.PolizaReciboVencimiento;
                    }

                    if (typeUsuarioQualitas.TipoCobro.ToUpper() == "DEDUCIBLE")
                    {
                        label_8.Content = "Siniestro";
                        numOrden.Text = typeUsuarioQualitas.DeducibleSiniestro;
                        importe.Text = typeUsuarioQualitas.DeducibleCoberturaMonto;
                    }
                }
            }

            catch
            {
                Globales.MessageBoxMit("Error al cargar formulario frmMotoAgencia");
            }
            //abrirFrmNuevo(new frmPreguntaFactura()
            //{
            //    abrirFrmAhora = abrirFrmNuevo
            //});
            Globales.isVentanaQualitas = true;
            Globales.isQualitasTrxValida = false;
        }

        public bool isVentaSinPresencia { get; set; }

        public void limpia()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".limpia()";
            try
            {
                numTdc.Text = "";
                this.num_tarjeta = string.Empty;

                numCvv.Password = "";
                nomTdc.Text = "";


                if (!Globales.isQualitas)
                {
                    numOrden.Text = "";
                    importe.Text = "";
                }
                //lblTError.Text = "";
                //lblAuth.Text = "";
                cmdVoucher.IsEnabled = false;
                //lblAprob.Visibility = Visibility.Hidden;
                //lblAuth.Visibility = Visibility.Hidden;
                //lblTError.Visibility = Visibility.Hidden;
                //lblDenied.Visibility = Visibility.Hidden;

                //lblSucursarl usuario.idbranch y nb_branch
                //lblUsu usuario.usu
                cboBanco.Items.Clear();
                Anio.Items.Clear();
                Mes.Items.Clear();
                cboBanco = Globales.obtieneBancos(cboBanco, Globales.GetDataXml("catbancos", TypeUsuario.CadenaXML));
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

        public static string NOMBRE_GENERAL = "FrmMotoAgencia";

        public bool seguir { get; set; }


        private void antesEscribirLetra(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

        private void numOrdenGotFocus(object sender, RoutedEventArgs e)
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

        private void importeLostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
        }

        private void ConvertirMayusculas(object sender, RoutedEventArgs e)
        {
            TextBox caja = sender as TextBox;
            caja.Text = caja.Text.ToUpper();
        }

        private void cvvGotFOcus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.num_tarjeta))
            {
                Globales.MessageBoxMit("Ingrese número de tarjeta.");
                numTdc.Focus();
            }
        }
        private void cvvLostFocus(object sender, RoutedEventArgs e)
        {

            if (Globales.noMostrarMensajes) return;
            if (string.IsNullOrWhiteSpace(this.num_tarjeta))
                return;


            if (!(Globales.isAerolinea || Globales.isAgencias))
            {
                if (numCvv.Password != "000" || numCvv.Password != "0000")
                {
                    if (Globales.isAmex && numCvv.Password.Length != 4 && numCvv.Password.Length != 0)
                    {
                        Globales.MessageBoxMit("El código de seguridad debe ser de 4 digitos para AMEX");
                        isCvv = true;
                        control = sender as PasswordBox;
                        return;
                    }
                    else if (!Globales.isAmex && numCvv.Password.Length != 3 && numCvv.Password.Length != 0)
                    {
                        Globales.MessageBoxMit("El código de seguridad debe ser de 3 dígitos para V/MC");
                        isCvv = true;
                        control = sender as PasswordBox;
                        return;
                    }

                }
                else
                {
                    Globales.MessageBoxMit("El cvv es incorrecto, favor de validar.");
                    isCvv = true;
                    control = sender as PasswordBox;
                    return;
                }
            }
        }

        private void cmdEnviarClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bool bolOperacion = false;

                //cmd.Enabled = false;
                cmdEnviar.IsEnabled = true;
                if (numTdc.Text == string.Empty)
                {
                    numTdc.Focus();
                    Globales.MessageBoxMit("Ingrese número de tarjeta.");
                    return;
                }
                if (this.Mes.SelectedIndex == -1)
                {
                    this.Mes.Focus();
                    Globales.MessageBoxMit("Seleccione el mes de vencimiento de la tarjeta.");
                    return;
                }
                if (this.Anio.SelectedIndex == -1)
                {
                    this.Anio.Focus();
                    Globales.MessageBoxMit("Seleccione el año de vencimiento de la tarjeta.");
                    return;
                }
                if (this.nomTdc.Text == string.Empty)
                {
                    this.nomTdc.Focus();
                    Globales.MessageBoxMit("Introduza el nombre del titular.");
                    return;
                }
                if ((string.IsNullOrWhiteSpace(numCvv.Password) && numcvv2.Visibility == Visibility.Visible) && !(Globales.isAgencias || Globales.isAerolinea))
                {
                    this.numCvv.Focus();
                    Globales.MessageBoxMit("Ingrese código  de seguridad de la tarjeta.");
                    return;
                }
                if ((numCvv.Password == "000" || numCvv.Password == "0000") && numcvv2.Visibility == Visibility.Visible)
                {
                    Globales.MessageBoxMit("Código de seguridad invalido");
                    numCvv.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Globales.merchantMoto))
                {
                    Globales.MessageBoxMit("No hay planes de pago para esta tarjeta, por favor cambie la tarjeta");
                    if (Globales.isQualitas)
                        cerrando();
                    else
                        cmdEnviar.IsEnabled = true;
                    return;
                }



                if (string.IsNullOrWhiteSpace(numOrden.Text))
                {
                    this.numOrden.Focus();
                    Globales.MessageBoxMit("Introduzca " + TypeUsuario.reference);
                    return;
                }
                if (this.importe.Text == string.Empty)
                {
                    this.importe.Focus();
                    Globales.MessageBoxMit("Ingrese el importe.");
                    return;
                }

                if (TypeUsuario.Id_Company == "0059")
                {
                    if (numOrden.Text.Length != 28)
                    {
                        Globales.MessageBoxMit("El no. de REFERENCIA debe ser de 28 posiciones.");
                        numOrden.Focus();
                        cmdEnviar.IsEnabled = true;
                        return;
                    }
                }

                if (Convert.ToInt16(Anio.Text.Substring(2, 2)) < Convert.ToInt16(DateTime.Now.Year.ToString().Substring(2, 2)))
                {
                    Globales.MessageBoxMit("Tarjeta vencida.");
                    cmdEnviar.IsEnabled = true;
                    return;
                }
                else if ((Convert.ToInt16(Anio.Text.Substring(2, 2)) == Convert.ToInt16(DateTime.Now.Year.ToString().Substring(2, 2)))
                    && Convert.ToInt16(Mes.Text) < Convert.ToInt16(DateTime.Now.Month.ToString()))
                {
                    Globales.MessageBoxMit("Tarjeta vencida");
                    cmdEnviar.IsEnabled = true;
                    return;
                }

                if (Globales.isAmex)
                {
                    //Checkom 4
                    if (numCvv.Password.Length != 4 && !(Globales.isAgencias || Globales.isAerolinea))
                    {
                        Globales.MessageBoxMit("Introduzca el Cod Sed de la tarjeta");
                        numCvv.Focus();
                        cmdEnviar.IsEnabled = true;
                        return;
                    }
                }
                else
                {
                    //CheckOm 3
                    if (numCvv.Password.Length != 3 && numcvv2.Visibility == Visibility.Visible && !(Globales.isAgencias || Globales.isAerolinea))
                    {
                        Globales.MessageBoxMit("Introduzca el Cod de seguridad de la tarjeta");
                        numCvv.Focus();
                        cmdEnviar.IsEnabled = true;
                        return;
                    }
                }

                double auxNumero = 0;
                if (!(double.TryParse(importe.Text, out auxNumero)))
                {
                    Globales.MessageBoxMit("El importe debe ser numérico.");
                    importe.Focus();
                    return;
                }

                //Solo para qualitas
                if (Globales.isQualitas)
                {
                    if (typeUsuarioQualitas.TipoCobro.ToUpper() != "OTROS" && !string.IsNullOrWhiteSpace(typeUsuarioQualitas.TipoCobro))
                    {
                        if (string.IsNullOrWhiteSpace(txtEmail.Text))
                        {
                            Globales.MessageBoxMit("Introduzca Correo Electrónico");
                            this.numTdc.Focus();
                            cmdEnviar.IsEnabled = true;
                            return;
                        }
                        else if (!Globales.ValidaEmail(txtEmail.Text))
                        {
                            Globales.MessageBoxMit("Correo electrónico no válido.");
                            txtEmail.Focus();
                            cmdEnviar.IsEnabled = true;
                            return;
                        }
                        else
                        {
                            txtEmail.IsEnabled = false;
                        }
                    }
                }

                cmdEnviar.IsEnabled = false;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                string strTypeC = string.Empty;
                Globales.cpIntegracion_Clear();

                if (Globales.isAmex)
                    strTypeC = "AMEX";
                else
                    strTypeC = "V/MC";


                Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL;
                if (string.IsNullOrWhiteSpace(Globales.merchantMoto))
                {
                    //Se cambia el método para llamar los merhcant para MCI
                    string strTpOperAfi = string.Empty;
                    if (caption.Contains("operativa"))
                    {
                        Globales.merchantMoto = Globales.cpIntegraEMV.dbgGetMerchantMoto(this.num_tarjeta, strTpOperAfi);
                    }
                    else
                    {
                        Globales.merchantMoto = Globales.cpIntegraEMV.dbgGetMerchantOpManual(this.num_tarjeta);
                    }
                }
                if ((Globales.isAgencias || Globales.isAerolinea) && string.IsNullOrWhiteSpace(numCvv.Password) && TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
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
                string strTpOperacion = string.Empty;
                if (!caption.Contains("operativa"))
                    strTpOperacion = "10";
                else
                    strTpOperacion = "14";

                Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                if (Globales.isAerolinea && !Globales.isVentasPropias)
                {
                    Globales.cpIntegraEMV.sndCheckInMOTO(
                        TypeUsuario.usu,
                        TypeUsuario.Pass,
                        "",
                        TypeUsuario.Id_Company,
                        TypeUsuario.Id_Branch,
                        TypeUsuario.country,
                        Globales.merchantMoto,
                        numOrden.Text,
                        strTpOperacion,
                        importe.Text,
                        Convert.ToString(lblMoneda.Content),
                        "",
                        strTypeC,
                        nomTdc.Text,
                       this.num_tarjeta,
                        Convert.ToString(Mes.SelectedItem),
                        Convert.ToString(Anio.SelectedItem).Substring(2, 2),
                        numCvv.Password
                        );
                    Mouse.OverrideCursor = null;
                }
                else
                {
                    if(string.IsNullOrWhiteSpace(numCvv.Password)){
                        numCvv.Password = (!Globales.isAmex) ? "000" : "0000";
                    }
                    Globales.cpIntegraEMV.sndVtaMOTO(TypeUsuario.usu,
                     TypeUsuario.Pass,
                     string.Empty,
                     TypeUsuario.Id_Company,
                     TypeUsuario.Id_Branch,
                     TypeUsuario.country,
                     Globales.merchantMoto,
                     numOrden.Text,
                     strTpOperacion,
                     importe.Text,
                     Convert.ToString(lblMoneda.Content),
                     strTypeC,
                      nomTdc.Text,
                     this.num_tarjeta,
                     Mes.SelectedItem.ToString(),
                     Anio.SelectedItem.ToString().Substring(2, 2),
                     numCvv.Password
                     );
                    Mouse.OverrideCursor = null;
                }
                Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();

                var respuestaVerificar = Globales.cpIntegracion_sResult;

                bolOperacion = true;

                if (bolOperacion)
                {
                    //-----------------------------Venta forzada-----------------------------------
                    //Se agrega validacion para no lanzar venta forzada!!.. 
                    string strCadEncriptar = string.Empty;
                    if ((Globales.isAgencias || Globales.isAerolinea) && !Globales.isVentasPropias)
                    {
                        goto ejecutaCobro;
                    }

                    if (Globales.GetDataXml("cd_response", Globales.cpIntegracion_sResult) == Globales.COD_VF)
                    {
                        if (Globales.MessageConfirm(Globales.GetDataXml("msgautvf", TypeUsuario.CadenaXML)))
                        {
                            string strNumAut = string.Empty;
                            while (strNumAut.Length != 6)
                            {
                                inputBox input = new inputBox();
                                input.setTitulo("Introduzca el número de autorización (6 caracteres) o cancelar para anular la operación");
                                input.setMax(6);
                                input.ShowDialog();
                                if (input.cancelarActivo)
                                {
                                    strNumAut = "";
                                    break;
                                }
                                else
                                {
                                    strNumAut = input.getValor();
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(strNumAut))
                            {
                                string strFolioC = string.Empty;
                                strFolioC = Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult);
                                Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL_VF;
                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                if (Globales.cpIntegracion_cpVtaForzadaM(
                                       TypeUsuario.Id_Company,
                                       TypeUsuario.Id_Branch,
                                       TypeUsuario.country,
                                       TypeUsuario.usu,
                                       TypeUsuario.Pass,
                                       Globales.merchantMoto,
                                       numOrden.Text,
                                       strTpOperacion,
                                       strTypeC,
                                       nomTdc.Text,
                                       this.num_tarjeta,
                                       Convert.ToString(Mes.SelectedItem),
                                       Convert.ToString(Anio.SelectedItem).Substring(2, 2),
                                       numCvv.Password,
                                       importe.Text,
                                       Convert.ToString(lblMoneda.Content),
                                       strFolioC,
                                       strNumAut
                                    ))
                                {
                                    Mouse.OverrideCursor = null;
                                    switch (Globales.GetDataXml("response", Globales.cpIntegracion_sResult).ToLower())
                                    {
                                        case "approved":
                                            cboBanco.IsEnabled = false;
                                            formaPago.IsEnabled = false;
                                            numOrden.IsEnabled = false;
                                            importe.IsEnabled = false;

                                            string message = Globales.GetDataXml("auth", Globales.cpIntegracion_sResult);
                                            Globales.MessageBoxMitApproved(message);

                                            escogerImpresora();


                                            cmdVoucher.IsEnabled = true;
                                            cmdNuevo.Visibility = Visibility.Visible;
                                            cmdEnviar.Visibility = Visibility.Hidden;
                                            this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;

                                            if (Globales.FacturaE == "1")
                                            {
                                                System.Windows.Forms.DialogResult p = Globales.mensajeQuestion("¿Desea factura electrónica?", "");
                                                if (p == System.Windows.Forms.DialogResult.Yes)
                                                {
                                                    frmPreguntaFactura frmPregunta = new frmPreguntaFactura();
                                                    abrirFrmNuevo(new frmPreguntaFactura()
                                                    {
                                                        abrirFrmAhora = abrirFrmNuevo,
                                                        cerraPage = cierra
                                                    });
                                                    Mouse.OverrideCursor = null;
                                                    return;
                                                }
                                                else
                                                {
                                                    Globales.XMLFacturaE = "";
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

                                            string cadena = string.Format("{0}\n{1} {2}", Globales.msjRech, Globales.GetDataXml("cd_response", Globales.cpIntegracion_sResult.Trim()),
                               Globales.GetDataXml("friendly_response", Globales.cpIntegracion_sResult.Trim()));
                                            Globales.MessageBoxMitDenied(cadena);


                                            cmdNuevo.Visibility = Visibility.Visible;
                                            cmdEnviar.Visibility = Visibility.Hidden;
                                            break;
                                        case "error":
                                            string mensaje = Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult);
                                            Globales.MessageBoxMitError(mensaje);
                                            if (mensaje.Contains("La transaccion ya fue aprobada"))
                                            {
                                                numTdc.IsEnabled = false;

                                                Mes.IsEnabled = false;
                                                Anio.IsEnabled = false;
                                                nomTdc.IsEnabled = false;
                                                numCvv.IsEnabled = false;
                                                cboBanco.IsEnabled = false;
                                                formaPago.IsEnabled = false;
                                                numOrden.IsEnabled = false;
                                                importe.IsEnabled = false;
                                                cmdNuevo.Visibility = Visibility.Visible;
                                                cmdEnviar.Visibility = Visibility.Hidden;

                                            }
                                            else
                                            {
                                                cmdNuevo.Visibility = Visibility.Hidden;
                                                cmdEnviar.Visibility = Visibility.Visible;
                                            }
                                            break;
                                        default:
                                            Globales.MessageBoxMitError("Verifique su conexión de internet.");

                                            cmdNuevo.Visibility = Visibility.Visible;
                                            cmdEnviar.Visibility = Visibility.Hidden;
                                            cmdVoucher.IsEnabled = false;
                                            break;
                                    }
                                    cmdEnviar.IsEnabled = true;
                                }
                                else
                                {
                                    Globales.MessageBoxMitError("Verifique su conexión de internet.");
                                    cmdNuevo.Visibility = Visibility.Visible;
                                    cmdEnviar.Visibility = Visibility.Hidden;
                                    cmdVoucher.IsEnabled = false;
                                    cmdEnviar.IsEnabled = true;
                                }
                            }
                            else
                            {
                                cmdNuevoClick(null, null);
                                cmdEnviar.IsEnabled = true;
                                Mouse.OverrideCursor = null;
                                return;
                            }
                        }
                        else
                        {
                            cmdNuevoClick(null, null);
                            cmdEnviar.IsEnabled = true;
                            Mouse.OverrideCursor = null;
                            return;
                        }
                        Mouse.OverrideCursor = null;
                        return;
                    }
                //------------------------------------FIN DE VENTA FORZADA---------------------------------
                ejecutaCobro:
                    switch (Globales.GetDataXml("response", Globales.cpIntegracion_sResult).ToLower())
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


                            string message = Globales.GetDataXml("auth", Globales.cpIntegracion_sResult);
                            Globales.MessageBoxMitApproved(message);
                            if (TypeUsuario.Id_Company == Globales.EMPREF && TypeUsuario.Id_Branch == Globales.EMPREF2)
                            {
                                frmReferencia refe = new frmReferencia();
                                refe.ShowDialog();
                                if (!refe.cancelar)
                                {
                                    Globales.cpIntegraEMV.sndReimpresion(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country, Globales.cpIntegraEMV.getRspOperationNumber());
                                    Mouse.OverrideCursor = null;
                                }
                            }
                            if (Globales.isAerolinea && !Globales.isVentasPropias)
                            {
                                frmBoletosAerolinea boletos = new frmBoletosAerolinea();
                                boletos.Importe = importe.Text;
                                boletos.auth = Globales.cpIntegraEMV.getRspAuth();
                                boletos.ShowDialog();
                            }
                            else
                            {
                                Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();
                                TypeUsuario.strVoucherCoP = Globales.cpIntegraEMV.getRspVoucher();
                                TypeUsuario.strVoucherCoP = (TypeUsuario.strVoucherCoP == null) ? "" : TypeUsuario.strVoucherCoP;
                                if (TypeUsuario.strVoucherCoP.Contains("@cnb -C-O-P-I-A- "))
                                {
                                    TypeUsuario.strVoucherCoP.Replace("@cnb -C-O-P-I-A- ", "");
                                }
                                escogerImpresora();

                                #region
                                cmdVoucher.IsEnabled = true;
                                cmdNuevo.Visibility = Visibility.Visible;
                                cmdEnviar.Visibility = Visibility.Hidden;
                                this.BENVIAMAIL.Visibility = (TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden);
                                this.BENVIAMAIL.Tag = this.nomTdc.Text;
                                if (Globales.FacturaE == "1")
                                {
                                    bool respuesta = Globales.MessageConfirm("¿Desea factura electrónica?");
                                    if (respuesta)
                                    {
                                        frmPreguntaFactura frmPregunta = new frmPreguntaFactura();
                                        frmPregunta.abrirFrmAhora = abrirFrmNuevo;
                                        frmPregunta.cerraPage = cierra;
                                        abrirFrmNuevo(frmPregunta);
                                        Mouse.OverrideCursor = null;
                                        return;
                                    }
                                    else
                                    {
                                        Globales.XMLFacturaE = "";
                                    }

                                }

                                #endregion
                                if (Globales.isQualitas && isVentaSinPresencia)
                                {
                                    if (typeUsuarioQualitas.TipoCobro.ToUpper() != "OTROS" && !string.IsNullOrWhiteSpace(typeUsuarioQualitas.TipoCobro))
                                    {
                                        cmdActualizaPago_Click(null, null);
                                        this.BENVIAMAIL.Visibility = Visibility.Hidden;
                                        this.BENVIAMAIL.Tag = string.Empty;
                                    }
                                }
                            }
                            //Si eres qualitas
                            if (Globales.isQualitas && isVentaSinPresencia)
                            {
                                if (typeUsuarioQualitas.TipoCobro.ToUpper() != "OTROS" && !string.IsNullOrWhiteSpace(typeUsuarioQualitas.TipoCobro.ToUpper()))
                                {
                                    cmdActualizaPago_Click(null, null);
                                    this.BENVIAMAIL.Visibility = Visibility.Visible;
                                    this.BENVIAMAIL.Tag = this.nomTdc.Text;
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

                            string cadena = string.Format("{0} {1}", Globales.GetDataXml("cd_response", Globales.cpIntegracion_sResult.Trim()),
                                Globales.GetDataXml("friendly_response", Globales.cpIntegracion_sResult.Trim()));
                            Globales.MessageBoxMitDenied(cadena);

                            cmdNuevo.Visibility = Visibility.Visible;
                            cmdEnviar.Visibility = Visibility.Hidden;
                            break;
                        case "error":
                            string mensaje = Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult);
                            Globales.MessageBoxMitError(mensaje);
                            if (mensaje.Contains("La transaccion ya fue aprobada"))
                            {
                                numTdc.IsEnabled = false;

                                Mes.IsEnabled = false;
                                Anio.IsEnabled = false;
                                nomTdc.IsEnabled = false;
                                numCvv.IsEnabled = false;
                                cboBanco.IsEnabled = false;
                                formaPago.IsEnabled = false;
                                numOrden.IsEnabled = false;
                                importe.IsEnabled = false;
                                cmdNuevo.Visibility = Visibility.Visible;
                                cmdEnviar.Visibility = Visibility.Hidden;

                            }
                            else
                            {
                                cmdNuevo.Visibility = Visibility.Hidden;
                                cmdEnviar.Visibility = Visibility.Visible;
                            }
                            break;
                        default:
                            Globales.MessageBoxMitError("Verifique su conexión de internet.");
                            cmdNuevo.Visibility = Visibility.Hidden;
                            cmdEnviar.Visibility = Visibility.Visible;
                            break;
                    }

                }
                else
                {
                    Globales.MessageBoxMitError(Globales.cpIntegracion_sError);
                    cmdNuevo.Visibility = Visibility.Hidden;
                    cmdEnviar.Visibility = Visibility.Visible;
                }
                cmdEnviar.IsEnabled = true;
                Mouse.OverrideCursor = null;
                //Si eres qualitas 

                if (Globales.isQualitas && isVentaSinPresencia)
                {
                    cmdNuevo.Visibility = Visibility.Hidden;
                    cmdEnviar.IsEnabled = false;
                    //Cosas más de qualitas!! Por Santiago Antonio Mariscal Veásquez.
                    if (Globales.cpIntegraEMV.getRspDsResponse().ToUpper() != "APPROVED")
                    {
                        cmdEnviar.IsEnabled = false;
                        cboBanco.IsEnabled = false;
                        formaPago.IsEnabled = false;
                        importe.IsEnabled = false;
                        numOrden.IsEnabled = false;
                        StatusCmd = false;
                        Globales.isQualitasCierraForm = true;
                        numTdc.IsEnabled = false;
                        nomTdc.IsEnabled = false;
                        realizaCoberturaMultiple();
                    }
                }
            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
                cmdEnviar.IsEnabled = true;
            }
            Mouse.OverrideCursor = null;
        }

        private void escogerImpresora()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
               // TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult).Trim();
                switch (TypeUsuario.TipoImpresora)
                {
                    case "1":
                        if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult),
                                                      TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
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
                        if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult),
                                                      TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                        {
                            TypeUsuario.strVoucher = Globales.VoucherHtml1(Globales.cpHTTP_sResult);
                            Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        }
                        cmdVoucher.IsEnabled = true;
                        cmdNuevo.IsEnabled = true;
                        break;
                    case "5":
                        break;
                    case "6":
                        Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                        cmdVoucher.IsEnabled = true;
                        cmdNuevo.IsEnabled = true;
                        break;
                    default:
                        Globales.MessageBoxMit("No hay una impresora seleccionada, porfavor ir a la parte de administracion a seleccionar una.");
                        break;
                }
                if (TypeUsuario.IsAQ)
                {
                    Globales.VerificaVoucher();
                }

            }
            catch
            {

            }
            Mouse.OverrideCursor = null;
        }

        private void realizaCoberturaMultiple()
        {
            if (Globales.isCoberturaMultiple)
            {
                Globales.contCoberturas = Globales.contCoberturas + 1;
                if (Globales.contCoberturas < Globales.numTotalCoberturas)
                {
                    Globales.MessageBoxMit("Quedan " + (Globales.numTotalCoberturas - Globales.contCoberturas) + " Coberturas por cobrar");
                    cmdSalirQualitas.IsEnabled = false;
                    cmdSalirQualitas.Visibility = Visibility.Visible;
                    cmdQualitasOtroCobro.IsEnabled = true;
                    cmdQualitasOtroCobro.Visibility = Visibility.Visible;
                }
                else
                {
                    cmdQualitasOtroCobro.IsEnabled = false;
                    cmdQualitasOtroCobro.Visibility = Visibility.Visible;
                }
            }
        }

        private void cmdVoucher_click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "cmdVoucherClick()";
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                if (TypeUsuario.IsAQ)
                {
                    Globales.VerificaVoucher();
                    return;
                }

                escogerImpresora();
            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
            }
        }

        frmReferencia referenciaFormulario;

        private void cmdActualizaPago_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "cmdActualizaPago_Click();";
            try
            {
                string strXML, aux;
                string resultado;
                strXML = "";

                if (typeUsuarioQualitas.TipoCobro.ToUpper() == "POLIZA")
                {
                    strXML = "<?xml version=" + Convert.ToChar(34) + "1.0" + Convert.ToChar(34) + " encoding=" + Convert.ToChar(34) + "UTF-8" + Convert.ToChar(34) + "?>";
                    strXML += " <SOAP-ENV:Envelope xmlns:SOAP-ENV=" + Convert.ToChar(34) + "http://schemas.xmlsoap.org/soap/envelope/" + Convert.ToChar(34) + " xmlns:ns1=" + Convert.ToChar(34) + "http://service.pcpay.qualitas.smartkode.mx/" + Convert.ToChar(34) + ">";
                    strXML += " <SOAP-ENV:Body>";
                    strXML += " <ns1:aplicaPagoPoliza>";
                    strXML += "  <AplicacionPoliza>";
                    strXML += "  <autpago>" + Globales.cpIntegraEMV.getRspAuth() + "</autpago>";
                    //Valida el tipo de tarjeta
                    if (Globales.cpIntegraEMV.dbgGetisAmex())
                    {
                        aux = "AMEX";
                    }
                    else if (Globales.cpIntegraEMV.getCc_Type().Contains("Visa"))
                    {
                        aux = "V";
                    }
                    else if (Globales.cpIntegraEMV.getCc_Type().Contains("MasterCard"))
                    {
                        aux = "MC";
                    }
                    else
                    {
                        aux = "O";
                    }
                    strXML += " <canalPago>" + aux + "</canalPago>";
                    strXML += " <fecha>" + Globales.cpIntegraEMV.getRspDate() + "</fecha>";

                    strXML += " <financiamiento>" + Globales.cpIntegraEMV.dbgGetQualitasFinanciamiento() + "</financiamiento>";
                    strXML += " <folioPago>" + Globales.cpIntegraEMV.getRspOperationNumber() + "</folioPago>";
                    strXML += " <hora>" + Globales.cpIntegraEMV.getRspTime() + "</hora>";

                    aux = "TDC";
                    if (Globales.cpIntegraEMV.getCc_Type().Contains("DEBITO"))
                    {
                        aux = "TDB";
                    }

                    strXML += " <metodoPago>" + aux + "</metodoPago>";

                    strXML += " <moneda>" + typeUsuarioQualitas.PolizaMoneda + "</moneda>";
                    strXML += " <monto>" + Globales.cpIntegraEMV.getTx_Amount() + "</monto>";

                    aux = this.num_tarjeta.Substring(this.num_tarjeta.Length - 4);
                    strXML += " <numeroTarjeta>" + aux + "</numeroTarjeta>";

                    strXML += " <poliza>" + typeUsuarioQualitas.PolizaNumero + "</poliza>";
                    strXML += " <recibo>" + typeUsuarioQualitas.PolizaReciboNumero + "</recibo>";
                    strXML += " <tarjetaHabiente>" + nomTdc.Text + "</tarjetaHabiente>";
                    strXML += " <tipoFinan>" + Globales.cpIntegraEMV.dbgGetQualitasTipoFinanciamiento() + "</tipoFinan>";

                    strXML += " <usuarioPcPay>" + Globales.cpIntegraEMV.dbgGetUser() + "</usuarioPcPay>";
                    strXML += " <usuarioSise>APL.PCPAY</usuarioSise>";
                    strXML += " <correoElectronico>" + txtEmail.Text + "</correoElectronico>";

                    strXML += " </AplicacionPoliza>";
                    strXML += " </ns1:aplicaPagoPoliza>";
                    strXML += " </SOAP-ENV:Body>";
                    strXML += " </SOAP-ENV:Envelope>";
                }//FIN DE LA PILIZA

                if (typeUsuarioQualitas.TipoCobro.ToUpper() == "DEDUCIBLE")
                {
                    strXML = "<?xml version=" + Convert.ToChar(34) + "1.0" + Convert.ToChar(34) + " encoding=" + Convert.ToChar(34) + "UTF-8" + Convert.ToChar(34) + "?>";
                    strXML += " <SOAP-ENV:Envelope xmlns:SOAP-ENV=" + Convert.ToChar(34) + "http://schemas.xmlsoap.org/soap/envelope/" + Convert.ToChar(34) + " xmlns:ns1=" + Convert.ToChar(34) + "http://service.pcpay.qualitas.smartkode.mx/" + Convert.ToChar(34) + ">";
                    strXML += " <SOAP-ENV:Body>";
                    strXML += " <ns1:aplicaPagoDeducible>";
                    strXML += "  <AplicacionDeducible>";
                    strXML += "  <autpago>" + Globales.cpIntegraEMV.getRspAuth() + "</autpago>";

                    //'Valida el tipo de tarjeta
                    if (Globales.cpIntegraEMV.dbgGetisAmex())
                    {
                        aux = "AMEX";
                    }
                    else if (Globales.cpIntegraEMV.getCc_Type().Contains("Visa"))
                    {
                        aux = "V";
                    }
                    else if (Globales.cpIntegraEMV.getCc_Type().Contains("MasterCard"))
                    {
                        aux = "MC";
                    }
                    else
                    {
                        aux = "O";
                    }

                    strXML += " <canalPago>" + aux + "</canalPago>";


                    //'strXml = strXml & " <claveProveedor>1</claveProveedor>"
                    //'strXml = strXml & " <cobertura>" & typeUsuarioQualitas.DeducibleCoberturaCodigo & "</cobertura>"

                    strXML += " <fecha>" + Globales.cpIntegraEMV.getRspDate() + "</fecha>";

                    strXML += " <financiamiento>" + Globales.cpIntegraEMV.dbgGetQualitasFinanciamiento() + "</financiamiento>";
                    strXML += " <folioPago>" + Globales.cpIntegraEMV.getRspOperationNumber() + "</folioPago>";
                    strXML += " <hora>" + Globales.cpIntegraEMV.getRspTime() + "</hora>";

                    //'strXml = strXml & " <importeDeducible>" & cpIntegraEMV.getTx_Amount & "</importeDeducible>"

                    aux = "TDC";
                    if (Globales.cpIntegraEMV.getCc_Type().Contains("DEBITO"))
                    {
                        aux = "TDB";
                    }
                    strXML += " <metodoPago>" + aux + "</metodoPago>";

                    strXML += " <moneda>" + typeUsuarioQualitas.DeducibleMoneda + "</moneda>";

                    //'valida el numero de tarjeta
                    //'If cpIntegraEMV.dbgGetisAmex Then
                    //'    aux = Mid(cpIntegraEMV.chkCc_Number, 12)
                    //'Else
                    //'    aux = Mid(cpIntegraEMV.chkCc_Number, 13)
                    //'End If
                    aux = this.num_tarjeta.Substring(this.num_tarjeta.Length - 4);
                    strXML += " <numeroTarjeta>" + aux + "</numeroTarjeta>";

                    strXML += " <poliza>" + typeUsuarioQualitas.DeduciblePoliza + "</poliza>";
                    //'strXml = strXml & " <siniestro>" & typeUsuarioQualitas.DeducibleSiniestro & "</siniestro>"
                    strXML += " <tarjetaHabiente>" + nomTdc.Text + "</tarjetaHabiente>";
                    strXML += " <tipoFinan>" + Globales.cpIntegraEMV.dbgGetQualitasTipoFinanciamiento() + "</tipoFinan>";

                    strXML += " <usuarioPcPay>" + Globales.cpIntegraEMV.dbgGetUser() + "</usuarioPcPay>";
                    strXML += " <usuarioSise>APL.PCPAY</usuarioSise>";

                    strXML += " <claveProveedor>1</claveProveedor>";
                    strXML += " <cobertura>" + typeUsuarioQualitas.DeducibleCoberturaCodigo + "</cobertura>";
                    strXML += " <idAsegurado>" + typeUsuarioQualitas.DeducibleIdAsegurado + "</idAsegurado>";
                    strXML += " <importeDeducible>" + Globales.cpIntegraEMV.getTx_Amount() + "</importeDeducible>";
                    strXML += " <siniestro>" + typeUsuarioQualitas.DeducibleSiniestro + "</siniestro>";
                    strXML += " <correoElectronico>" + txtEmail.Text + "</correoElectronico>";

                    strXML += " </AplicacionDeducible>";
                    strXML += " </ns1:aplicaPagoDeducible>";
                    strXML += " </SOAP-ENV:Body>";
                    strXML += " </SOAP-ENV:Envelope>";
                }

                resultado = Globales.WSSoap(strXML, Globales.ipQualitas);
                //Se llann los campos de acuerdo a la respúesta del WS
                if (!string.IsNullOrWhiteSpace(resultado) && resultado.Contains("Error"))
                {
                    typeUsuarioQualitas.RespuestaCodigo = Globales.GetDataXml("codigo", resultado);
                    typeUsuarioQualitas.RespuestaMensaje = Globales.GetDataXml("mensaje", resultado);
                    if (typeUsuarioQualitas.RespuestaCodigo == "1")
                    {
                        txtRemesa.Visibility = Visibility.Visible;
                        if (typeUsuarioQualitas.TipoCobro.ToUpper() == "POLIZA")
                        {
                            aux = "\nPago actualizado.\nRemesa No." + Globales.GetDataXml("remesa", resultado);
                        }
                        else
                        {
                            aux = "\nCobertura:" + typeUsuarioQualitas.DeducibleCoberturaCodigo + " Descripción: " + typeUsuarioQualitas.DeducibleCoberturaDescripcion;
                            aux += "\nPago actulizado\nRemesa No." + Globales.GetDataXml("remesa", resultado);
                        }
                        txtRemesa.Text = "";
                        Globales.isQualitasActualizado = true;
                        Globales.isQualitasTrxValida = false;
                        Globales.isQualitasCierraForm = true;
                        cmdSalirQualitas.Visibility = Visibility.Visible;
                        cmdSalirQualitas.IsEnabled = true;
                        Globales.MessageBoxMit("Pago actualizado correctamente");
                    }
                    else
                    {
                        cmdSalirQualitas.Visibility = Visibility.Visible;
                        cmdSalirQualitas.IsEnabled = true;
                        txtRemesa.Visibility = Visibility.Visible;

                        aux = "Error al aplicar el Pago en Sistema, por favor apliquelo de forma manual.";
                        aux += "\nError" + typeUsuarioQualitas.RespuestaMensaje;
                        txtRemesa.Text += aux;

                        Globales.isQualitasActualizado = true;
                        Globales.isQualitasCierraForm = true;
                        Globales.isQualitasTrxValida = false;
                        Globales.MessageBoxMit("No se pudo Actualizar el pago correctamente.");
                    }
                }
                else
                {
                    cmdSalirQualitas.Visibility = Visibility.Visible;
                    cmdSalirQualitas.IsEnabled = true;

                    txtRemesa.Visibility = Visibility.Visible;
                    aux = "Error al aplicar el Pago en sistema, por favor apliquelo de forma manual.";
                    txtRemesa.Text = aux;

                    Globales.isQualitasActualizado = true;
                    Globales.isQualitasTrxValida = false;
                    Globales.isQualitasCierraForm = true;
                }
                realizaCoberturaMultiple();
            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
            }
        }

        public bool StatusCmd { get; set; }

        private void cmdQualitasOtroCobro_Click(object sender, RoutedEventArgs e)
        {
            cerrando();
            abrirFrmNuevo(this);
        }

        private void cmdSalirQualitas_Click(object sender, RoutedEventArgs e)
        {
            cerrando();
        }

        private void importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void nomTdc_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTexto(sender, e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir(false);
        }

        private void previewKeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.num_tarjeta))
            {
                Globales.MessageBoxMit("Ingrese número de tarjeta.");
                e.Handled = true;
            }
        }
        private void numTdc_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }


    }
}
