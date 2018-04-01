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
using PcPay.Forms.Formularios;
using System.Threading;
using System.Text.RegularExpressions;
using PcPay.Forms.Formularios.MessagesW;
using PcPay.Forms.formularios;

namespace PcPay.Forms.Pages
{
    /// <summary>
    /// Interaction logic for frmBandaEMV.xaml
    /// </summary>
    public partial class frmBandaEMV : Page
    {
        public Window propietario { get; set; }
        //frmPlanePagos frmPlanPagos = new frmPlanePagos();
        frmBoletosAerolinea frmBoletosAerolinea { get; set; }
        public abrirFrm abrirFrmNuevo { get; set; }
        public cerrarVentana cerrar { get; set; }

        internal string _anio { get; set; }
        internal string _mes { get; set; }
        public bool cobroVtaNormalBool { get; set; }



        public frmBandaEMV()
        {
            InitializeComponent();
            this.NumOrden.ToolTip = Globales.MsjReferencia;
            this.Importe.ToolTip = Globales.MsjImporte;
            this.lblMensaje.Text = string.Empty;

            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.txtVenc2.Visibility = Visibility.Hidden;

            this.BENVIAMAIL.MouseDown += Globales.sendMailFirmaPanel_GridMouseDown;
            cmdLeer.Click += new RoutedEventHandler(cmdLeerBoton);
            #region"**********Efectos**********"
            this.NumOrden.GotFocus += Globales.setFocusMit2;
            this.Importe.GotFocus += Globales.setFocusMit2;

            this.NumOrden.LostFocus += Globales.lostFocusMit2;
            this.Importe.LostFocus += Globales.lostFocusMit2;
            #endregion
        }
        private void cargandoFormulario(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "Form_load()";
            imgEmailFirmaPanel.Visibility = Visibility.Hidden;
            lblMoneda.Content = "MXN";
            if (Globales.isVentasPropias)
            {

            }
            StatusCmd = true;
            if (TypeUsuario.Id_Company == Globales.EMPREF && TypeUsuario.Id_Branch == Globales.EMPREF2)
            {
                numOrden2.Visibility = Visibility.Hidden;
                label_7.Visibility = Visibility.Hidden;
            }


           

            cmdLeer.Visibility = Visibility.Visible;
            //imgFirma.Visibility = Visibility.Hidden;
            cmdActualizaPago.Visibility = Visibility.Hidden;
            txtEmail2.Visibility = Visibility.Hidden;
            lblEmail.Visibility = Visibility.Hidden;
            cmdActualizaPago.Visibility = Visibility.Hidden;
            txtRemesa.Visibility = Visibility.Hidden;
            cmdSalirQualitas.Visibility = Visibility.Hidden;
            cmdQualitasOtroCobro.Visibility = Visibility.Hidden;
            //imgQualitasMail.Visibility = Visibility.Hidden;

            lblAfiliacionQ.Visibility = Visibility.Hidden;
            //lblAfiliacionQ.Left = 228;
            lblTipoCobroQ.Visibility = Visibility.Hidden;
            //lblTipocobroQ.Left = 228;

            //Si eres de Qualitas

            if (Globales.bolActivaMotoP)
            {
                limpia();
                cmdLeer.Visibility = Visibility.Visible;
                CmdEnviar.Visibility = Visibility.Hidden;
                label_7.Content = TypeUsuario.reference;
                if (TypeUsuario.ShowMsg)
                {
                    frmAvisoBanda avisoBanda = new frmAvisoBanda();
                    avisoBanda.Owner = this.propietario;
                    avisoBanda.ShowDialog();
                }
            }

            if (Globales.isQualitas)
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
                    }
                    else
                    {
                        typeUsuarioQualitas.DeducibleCoberturaCodigo = Globales.nodeListXML.Item(Globales.contCoberturas).ChildNodes[1].InnerText;
                        typeUsuarioQualitas.DeducibleCoberturaDescripcion = Globales.nodeListXML.Item(Globales.contCoberturas).ChildNodes[2].InnerText;
                        Globales.MessageBoxMit("Cobertura sin deducible \nCódigo:" + typeUsuarioQualitas.DeducibleCoberturaCodigo + "\nDescripción:" + typeUsuarioQualitas.DeducibleCoberturaDescripcion);
                        NumOrden.IsEnabled = false;
                        Importe.IsEnabled = false;
                        realizaCoberturaMultiple();
                    }
                }

                if (typeUsuarioQualitas.TipoCobro.ToUpper() != "OTROS")
                {
                    Globales.isQualitasCierraForm = true;
                    NumOrden.IsEnabled = false;
                    Importe.IsEnabled = false;
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
                    NumOrden.Text = typeUsuarioQualitas.PolizaReciboNumero;
                    Importe.Text = typeUsuarioQualitas.PolizaReciboImporte;
                    numOrden2.Width = 135;

                    lblVenc.Visibility = Visibility.Visible;
                    txtVenc2.Visibility = Visibility;
                    txtVenc.Text = typeUsuarioQualitas.PolizaReciboVencimiento;
                }

                if (typeUsuarioQualitas.TipoCobro.ToUpper() == "DEDUCIBLE")
                {
                    label_7.Content = "Siniestro";
                    NumOrden.Text = typeUsuarioQualitas.DeducibleSiniestro;
                    Importe.Text = typeUsuarioQualitas.DeducibleCoberturaMonto;
                }
            }
            Globales.ReseteaVariablesRecompensas();
            Globales.isVentanaQualitas = true;
            Globales.isQualitasTrxValida = false;
            cmdNuevoClick();
        }
        private void limpia()
        {
            int i = 0;
            this.imgEmailFirmaPanel.Visibility = Visibility.Hidden;
            Globales.strNombreFP = NOMBRE_GENERAL + "Limpia";
            NomTdc.Text = "";
            NumTdc.Text = "";
            NumOrden.Text = "";
            Importe.Text = "";

            this.TFECHAVENC.Text = string.Empty;


            this._mes = string.Empty;
            this._anio = string.Empty;

            //lblTError.Text = "";
            //lblAuth.Text = "";
            cmdVoucher.IsEnabled = false;
            //lblAprob.Visibility = Visibility.Hidden;
            //lblAuth.Visibility = Visibility.Hidden;
            //lblTError.Visibility = Visibility.Visible;
            //lblDenied.Visibility = Visibility.Hidden;
            this.lblMensaje.Text = string.Empty;
            this.lblMensaje.Foreground = Brushes.Black;
            this.lblMensaje.FontSize = 15;

            cboBanco = Globales.obtieneBancos(cboBanco, Globales.GetDataXml("catbancos", TypeUsuario.CadenaXML));

            CmdEnviar.Visibility = Visibility.Visible;
            CmdNuevo.Visibility = Visibility.Hidden;
            //Cambio facilising predefine santande y lo oculta
            if (Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML).Contains("1"))
            {

                //Este metodo todavia hay que verificarlo
                int contador = 0;
                foreach (var item in cboBanco.Items)
                {
                    itemCombobox items = new itemCombobox();
                    items = (itemCombobox)item;
                    if (items.item == "49")
                    {//Si es santander
                        cboBanco.SelectedIndex = contador;
                        cboBanco.Visibility = Visibility.Hidden;
                        FormaPago.Visibility = Visibility.Hidden;
                        break;
                    }

                    contador++;
                }
            }
            if (TypeUsuario.Id_Company == "0059")
            {
                NumOrden.MaxLength = 20;
            }
            else
            {
                NumOrden.MaxLength = Globales.MAXCAR;
            }

        }
        public string NOMBRE_GENERAL = "Formulario Banda EMV";
        public bool StatusCmd { get; set; }
        //Este procedimiento se llama en caso que se realice una transaccion con EMV para 
        //validar que no falle nungún dato necesario para la transacción.
        private void realizaOp()
        {
            try
            {
                double numero = 0;
              

                if (FormaPago.SelectedIndex > 0)
                {
                    //Falta código aqu de forma pago pero esto es porque es facileasing..
                }

                cmdLeer.IsEnabled = false;
                cboBanco.IsEnabled = false;
                FormaPago.IsEnabled = false;
                NumOrden.IsEnabled = false;
                Importe.IsEnabled = false;
                StatusCmd = false;
                //se hace el if si importe es numerido if(isnumeric(importe))
                Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                var respuesta = Globales.cpIntegraEMV.dbgStartTxEMV(Importe.Text);
                
                cmdLeer.IsEnabled = false;
                Mouse.OverrideCursor = null;
                if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()) && !string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkCc_Number()))
                {
                    NumTdc.Text = Globales.cpIntegraEMV.chkCc_Number();
                    NomTdc.Text = Globales.cpIntegraEMV.chkCc_Name();
                    this._mes = Globales.cpIntegraEMV.chkCc_ExpMonth();
                    this._anio = Globales.cpIntegraEMV.chkCc_ExpYear();

                    this.TFECHAVENC.Text = string.Format("{0}/{1}", Globales.cpIntegraEMV.chkCc_ExpMonth(), Globales.cpIntegraEMV.chkCc_ExpYear());

                    //Se cambia el metodo para llamar los merchant
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    if (!(Globales.cpIntegraEMV.AplicaCobroRecompensas(Globales.cpIntegraEMV.chkCc_Number().Substring(0, 6))))
                        Globales.merchantBanda = Globales.cpIntegraEMV.dbgGetMerchantBanda("9");
                    else
                        if (Globales.cpIntegraEMV.GetTipoMoneda().ToUpper() == "USD")
                            Globales.merchantBanda = Globales.cpIntegraEMV.dbgGetMerchantBanda("9");
                        else
                            Globales.merchantBanda = "-1";

                    Mouse.OverrideCursor = null;

                    if (!string.IsNullOrWhiteSpace(Globales.merchantBanda))
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        Globales.isAmex = Globales.cpIntegraEMV.dbgGetisAmex();
                        Mouse.OverrideCursor = null;

                        //Se sustituye para dejar el seteo de la moneda
                        FormaPagoClick();
                        cmdLeer.Visibility = Visibility.Hidden;
                        CmdEnviar.IsEnabled = true;
                        CmdEnviar.Visibility = Visibility.Visible;
                        Mouse.OverrideCursor = null;

                        if (Globales.isQualitas)
                        {
                            lblAfiliacionQ.Visibility = Visibility.Visible;
                            lblAfiliacionQ.Content = "Afiliación: " + Convert.ToString(Globales.cpIntegraEMV.strAfiliacionQualitas());
                            lblTipoCobroQ.Visibility = Visibility.Visible;
                            lblTipoCobroQ.Content = "Tipo cobro: " + Globales.cpIntegraEMV.strTipoCobroQualitas() + "   /   Moneda: " + Globales.cpIntegraEMV.GetMonedaQualitas();
                        }

                        if (!Globales.cpIntegraEMV.dbgTxMonitor())
                        {
                            Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspDsError());
                            FormaPago.Text = "";
                            FormaPago.Items.Clear();

                            cmdLeer.Visibility = Visibility.Visible;
                            cmdLeer.IsEnabled = true;
                            CmdEnviar.IsEnabled = false;
                            CmdEnviar.Visibility = Visibility.Hidden;
                            FormaPago.Visibility = Visibility.Hidden;
                            label_6.Visibility = Visibility.Hidden;
                            NumTdc.Text = "";
                            NomTdc.Text = "";

                            TFECHAVENC.Text = string.Empty;
                            this._mes = string.Empty;
                            this._anio = string.Empty;

                            StatusCmd = true;
                        }
                        Mouse.OverrideCursor = null;
                    }
                    else
                    {
                        Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspDsError());
                        //Cuando no hay una afiliacion válido

                        if (Globales.isQualitas)
                            this.cerrar();
                        else
                            this.cmdNuevoClick();
                    }
                    Mouse.OverrideCursor = null;
                }
                else
                {
                    cmdLeer.IsEnabled = true;
                    //ESTE IF ES RELACIONADO CON UN CÓDIGO DE ERRO QUE SE TENGA
                    if (!string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()))
                    {
                        Importe.IsEnabled = true;
                        cmdLeer.IsEnabled = true;
                        NumOrden.IsEnabled = true;
                        NumOrden.Text = "";

                        this._mes = string.Empty;
                        this._anio = string.Empty;

                        Importe.Text = "";
                        Globales.MessageBoxMitError(Globales.cpIntegraEMV.chkPp_DsError());
                        Mouse.OverrideCursor = null;
                        return;
                    }
                }

            }
            catch
            {
                cmdLeer.IsEnabled = true;
                cmdLeer.Visibility = Visibility.Visible;
                StatusCmd = true;
                Mouse.OverrideCursor = null;
            }
            Mouse.OverrideCursor = null;
        }
        //*********************************************************************
        //                    Limpia los controles y prepara al sistema    ****
        //                    para realizar una nueva transacción          ****
        //*****************
        //****************************************************


        private void FormaPagoClick()
        {
            if (Globales.GetDataXml("MXN", TypeUsuario.CadenaXML).Contains(Globales.merchantBanda))
                lblMoneda.Content = "MXN";
            else
                lblMoneda.Content = "USD";

            //Aplica para recompensas santander
            if (Globales.cpIntegraEMV.AplicaCobroRecompensas(Globales.cpIntegraEMV.chkCc_Number().Substring(0, 6)) && Globales.merchantBanda == "-1")
                lblMoneda.Content = "MXN";
        }
        public string SetMensaje { get; set; }
        public bool Visible { get; set; }

        private void imprimirVoucherClick(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdVoucher_click";

            //Recompensas
            int contadorRecom = 0;
            if (Globales.hayDobleVoucherRecom)
            {
                TypeUsuario.strVoucher = Globales.strCadenaVoucherVtaDirecta;
                TypeUsuario.strVoucherCoP = Globales.strCadenaVoucherVtaDirecta;
            }
        imprimeRecompensas:

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (TypeUsuario.TipoImpresora == "1")
            {

                
                Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                  
            }
            else if (TypeUsuario.TipoImpresora == "2")
            {
              
            }
            else if (TypeUsuario.TipoImpresora == "3")
            {
                Globales.imprimirEpson();
            }   
            else if (TypeUsuario.TipoImpresora == "4")
            {             
                Globales.Imprimir_HTML(TypeUsuario.strVoucher);
            }
            else if (TypeUsuario.TipoImpresora == "5")
            {
               
            }
            else if (TypeUsuario.TipoImpresora == "6")
            {
                CmdNuevo.IsEnabled = false;
                cmdVoucher.IsEnabled = false;
                Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                CmdNuevo.IsEnabled = true;
                cmdVoucher.IsEnabled = true;
            }
            else
            {
                Globales.MessageBoxMit("No ha definido un tipo de impresora\nPara seleccionarla vaya al menu de administración." + NOMBRE_GENERAL);
            }

            if (Globales.hayDobleVoucherRecom)
            {
                if (contadorRecom == 0)
                {
                    contadorRecom++;
                    TypeUsuario.strVoucher = Globales.strCadenaVoucherVtaRecomensa;
                    TypeUsuario.strVoucherCoP = Globales.strCadenaVoucherVtaRecomensa;
                    Thread.Sleep(2000);
                    goto imprimeRecompensas;
                }
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
        private void continuarFliujoQPS()
        {
            throw new NotImplementedException();
        }
        private void realizarLecturaRecom(string monto, bool esCobroRecompensa)
        {
            if (string.IsNullOrWhiteSpace(NumOrden.Text))
            {
                Globales.MessageBoxMit("Introduzca " + TypeUsuario.reference);
                NumOrden.Focus();
                // return;
            }
            else if (string.IsNullOrWhiteSpace(Importe.Text))
            {
                Globales.MessageBoxMit("Introduzca importe");
                Importe.Focus();
                // return;
            }
            else
            {
                if (TypeUsuario.Id_Company == "0059")
                {
                    if (NumOrden.Text.Length != 28)
                    {
                        Globales.MessageBoxMit("El no. de REFERENCIA debe ser de 28 posiciones");
                        NumOrden.Focus();
                        return;
                    }
                }

                int numero = 0;
                if (int.TryParse(Importe.Text, out numero))
                {
                    Globales.MessageBoxMit("El importe debe ser numerico");
                    Importe.Focus();
                    return;
                }
                if (!string.IsNullOrWhiteSpace(FormaPago.Text))
                {
                    //En esta parte del código va sobre lo menor del importe 
                }
                cmdLeer.IsEnabled = false;
                cboBanco.IsEnabled = false;
                FormaPago.IsEnabled = false;
                NumOrden.IsEnabled = false;
                Importe.IsEnabled = false;
                StatusCmd = false;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                //Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);

                Globales.cpIntegraEMV.dbgStartTxEMV(monto);
                Mouse.OverrideCursor = null;

                if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()) && !string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkCc_Number()))
                {
                    NumTdc.Text = Globales.cpIntegraEMV.chkCc_Number();
                    NomTdc.Text = Globales.cpIntegraEMV.chkCc_Name();

                    this._mes = Globales.cpIntegraEMV.chkCc_ExpMonth();
                    this._anio = Globales.cpIntegraEMV.chkCc_ExpYear();
                    //Se cambia el método para llamar a los merchant

                    if (Globales.sinSaldoRecomDirecto)
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        Globales.merchantBanda = Globales.cpIntegraEMV.dbgGetMerchantBanda("9");
                        Mouse.OverrideCursor = null;
                    }
                    if (!string.IsNullOrWhiteSpace(Globales.merchantBanda))
                    {
                        Globales.isAmex = Globales.cpIntegraEMV.dbgGetisAmex();
                        //Se sustituye para dejar el seteo de la moneda.
                        FormaPagoClick();

                        CmdEnviar.Visibility = Visibility.Visible;
                        CmdEnviar.IsEnabled = false;

                        if (Globales.isQualitas)
                        {
                            lblAfiliacionQ.Visibility = Visibility.Visible;
                            lblAfiliacionQ.Content = "Afiliación: "+Globales.cpIntegraEMV.strAfiliacionQualitas();

                            lblTipoCobroQ.Visibility = Visibility.Visible;
                            lblTipoCobroQ.Content = "Tipo cobro: "+Globales.cpIntegraEMV.strTipoCobroQualitas()+" / Moneda: "+Globales.cpIntegraEMV.GetMonedaQualitas();
                        }

                        //Recompensas

                        if (esCobroRecompensa)
                        {
                            this.realizaCobroRecom(monto);
                        }
                        else
                        {
                            this.enviarCobro(CmdEnviar, new RoutedEventArgs());
                        }

                        if (!(Globales.cpIntegraEMV.dbgTxMonitor()))
                        {
                            Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspDsError());
                            FormaPago.Items.Clear();
                            cmdLeer.Visibility = Visibility.Visible;
                            cmdLeer.IsEnabled = true;
                            CmdEnviar.IsEnabled = false;
                            CmdEnviar.Visibility = Visibility.Hidden;
                            FormaPago.Visibility = Visibility.Hidden;
                            NumTdc.Text = "";
                            NomTdc.Text = "";
                            this._mes = string.Empty;
                            this._anio = string.Empty;
                            StatusCmd = true;
                            label_6.Visibility = Visibility.Hidden;
                        }

                    }
                    else
                    {
                        Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspDsError());

                        //Cuando no hay una afiliación valida.
                        if (Globales.isQualitas)
                        {
                            Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            cmdNuevoClick();
                        }
                    }

                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()))
                    {
                        //Aqui va a ir demas cosas del formulario.
                        Globales.MessageBoxMit(Globales.cpIntegraEMV.chkPp_DsError());
                        FormaPago.Items.Clear();
                        cmdLeer.IsEnabled = true;
                        cmdLeer.Visibility = Visibility.Visible;
                        StatusCmd = true;
                        CmdEnviar.IsEnabled = false;
                        CmdEnviar.Visibility = Visibility.Hidden;
                        cmdNuevoClick();
                    }
                }
            }

        }
        private void realizaCobroRecom(string monto)
        {
            string Voucher = "";
            string strCadEncriptar = "";
            string erroVtaRecom = "";

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                #region enviar venta recompensas...
                Globales.cpIntegraEMV.sndVtaRecompensasDirecto(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
                    TypeUsuario.country, NumOrden.Text, monto, "102");
                string seleccion = Globales.cpIntegraEMV.getRspCd_StatusRecom();
                switch (seleccion)
                {
                    case "2":
                        cboBanco.IsEnabled = false;
                        FormaPago.IsEnabled = false;
                        NumOrden.IsEnabled = false;
                        Importe.IsEnabled = false;
                        Importe.IsEnabled = false;
                        //lblAprob.Visibility = Visibility.Visible;
                        //lblTError.Visibility = Visibility.Hidden;
                        //lblDenied.Visibility = Visibility.Hidden;
                        TypeUsuario.strVoucherCoP = Globales.cpIntegraEMV.getRspVoucher().Trim();
                        CmdNuevo.Visibility = Visibility.Visible;
                        CmdNuevo.IsEnabled = true;
                        CmdEnviar.Visibility = Visibility.Hidden;

                        //Ocultar o no el shepe
                        Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();

                        Voucher = Globales.cpIntegraEMV.getRspVoucher();
                        Voucher = (string.IsNullOrWhiteSpace(Voucher)) ? "" : Voucher;
                        if (Voucher.Contains("@cnb -C-O-P-I-A- "))
                        {
                            Voucher = Voucher.Replace("@cnb -C-O-P-I-A- ", "");
                        }
                        //Se salta el flujo de la firma en el panel y la impresion del voucher

                        Globales.printVoucherRecompensasDirecto(Voucher);
                        cmdVoucher.IsEnabled = true;
                        break;
                    case "1": //Denieds
                        if (Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML).Substring(0, 1) == "1")
                        {
                            Globales.cpHTTP_Clear();
                            Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;

                            strCadEncriptar = "&op=facileasingcobrorechazada" +
                                              "&nueconomico=" + strNoEconomico +
                                              "&nuservicio=" + strNoServicio +
                                              "&nuproveedor=" + strProveedor +
                                              "&transaccion=" + Globales.cpIntegraEMV.getRspOperationNumber() +
                                              "&importe=" + Importe +
                                              "&numtdc=" + NumTdc.Text.Substring(12, 4) +
                                              "&auth=" + "" +
                                              "&response=" + Globales.cpIntegraEMV.getRspCdResponse();

                            Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
                            if (Globales.cpHTTP_SendcpCUCT())
                            {

                            }
                        }

                        Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);


                        var mensaje = Globales.msjRech + "\n" + Globales.cpIntegraEMV.getRspCdResponse() + " " + Globales.cpIntegraEMV.getRspFriendlyResponse();
                        Globales.MessageBoxMitDenied(mensaje);
                        CmdNuevo.Visibility = Visibility.Visible;
                        CmdNuevo.IsEnabled = true;
                        CmdEnviar.Visibility = Visibility.Hidden;
                        cmdVoucher.IsEnabled = false;
                        cmdVoucher.Visibility = Visibility.Hidden;


                        break;
                    case "0":
                        cmdLeer.Visibility = Visibility.Visible;
                        cmdLeer.IsEnabled = false;
                        CmdEnviar.IsEnabled = false;
                        CmdEnviar.Visibility = Visibility.Hidden;
                        CmdNuevo.Visibility = Visibility.Visible;
                        CmdNuevo.IsEnabled = true;
                        cboBanco.IsEnabled = true;
                        FormaPago.IsEnabled = true;
                        Importe.IsEnabled = false;
                        NumOrden.IsEnabled = false;

                        var mensajeE = Globales.cpIntegraEMV.getRspDsError();
                        Globales.MessageBoxMitError(mensajeE);

                        if (!string.IsNullOrWhiteSpace(Globales.errorRecom))
                        {
                            Globales.MessageBoxMitError(Globales.errorRecom);
                            Thread.Sleep(2000);
                            Globales.cpIntegraEMV.dbgCancelOperation();
                        }
                        break;
                    default:
                        CmdEnviar.IsEnabled = false;
                        cmdLeer.Visibility = Visibility.Visible;
                        cmdLeer.IsEnabled = true;
                        cboBanco.IsEnabled = true;
                        FormaPago.IsEnabled = true;
                        Importe.IsEnabled = true;
                        NumOrden.IsEnabled = true;

                        CmdNuevo.IsEnabled = false;
                        cmdLeer.IsEnabled = true;
                        StatusCmd = true;

                        if (!string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.getRspDsError()))
                        {
                            Globales.MessageBoxMitError(Globales.cpIntegraEMV.getRspDsError());
                        }
                        else
                        {
                            Globales.MessageBoxMitError("Error de conexión, verifique su reporte.");
                        }
                        break;
                }

                #endregion
            }
            catch { 
            
            }
            Mouse.OverrideCursor = null;

            //GloboPcPay me.lblTerro,melblAprob,me.lblDenied,me.lblAuth.
        }

        public string strNoEconomico { get; set; }
        public string strNoServicio { get; set; }
        public string strProveedor { get; set; }

        private void NumOrden_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {

                if (TypeUsuario.Id_Company == "0059" && NumOrden.IsEnabled)
                {
                    TypeUsuario.strRefReaDig = "MA";
                    Importe.Focus();
                    frmReaderDigest reader = new frmReaderDigest();
                    reader.ShowDialog();
                    NumOrden.Text = Globales.referenciaAux;
                }
            }
            catch
            {

            }
        }
        private void textoNumero(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9A-Za-z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void numOrden_keyUp(object sender, KeyEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".numOrden_keyUp();";
            try
            {
                if (Globales.GetDataXml("id_company", TypeUsuario.CadenaXML) == Globales.RGEV)
                {//prueba GEV
                    Globales.RefGEV(Globales.GetDataXml("refgev", TypeUsuario.CadenaXML), Convert.ToString(lstMerchant.SelectedItem));
                    if (NumOrden.Text.Length == Globales.LCodGEV || NumOrden.Text.Length > Globales.LCodGEV)
                    {
                        if (NumOrden.Text.Substring(0, Globales.LCodGEV) != Globales.CodGEV)
                        {
                            Globales.MessageBoxMit("Referencia invalida");
                            NumOrden.Text = "";
                        }
                        if (!Globales.RefGEV(Globales.GetDataXml("refgev", TypeUsuario.CadenaXML), Convert.ToString(lstMerchant.SelectedItem), NumOrden.Text))
                        {
                            Globales.MessageBoxMit("Referencia invalida para la afiliación seleccionada");
                            NumOrden.Text = "";
                        }
                    }
                }
                if (Globales.GetDataXml("id_company", TypeUsuario.CadenaXML) == "0533")
                {
                    if (NumOrden.Text.Length > 30)
                    {
                        NumOrden.Text = NumOrden.Text.Substring(1, 30);
                        Importe.Focus();
                    }
                    if (NumOrden.Text.Length == 30)
                    {
                        Globales.CFEImporte(NumOrden.Text.Trim());
                        Importe.Text = CFE.Importe;
                        Importe.Focus();
                    }
                }
            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
            }
        }
        private void NumOrden_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".NumOrdes_LostFocus();";
            try
            {
                if (Globales.GetDataXml("id_company", TypeUsuario.CadenaXML) == "0533")
                {
                    if (NumOrden.Text.Length == 30)
                    {
                        Globales.CFEImporte(NumOrden.Text);
                        Importe.Text = CFE.Importe;
                    }
                }
                NumOrden.Text = NumOrden.Text.ToUpper().Trim();
            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
            }
        }
        private void Importe_GotFocus(object sender, RoutedEventArgs e)
        {
        
        }
        private void numero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }
        private void importe_lostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);

            if (Importe.Text != string.Empty && !string.IsNullOrWhiteSpace(NumOrden.Text))
            {
                if (!StatusCmd)
                {
                    cmdLeer.IsEnabled = true; //Antes era falso
                }
                else
                {
                    CmdEnviar.IsEnabled = true;
                    if (cmdLeer.Visibility == Visibility.Visible)
                    {
                        if (cmdLeer.IsEnabled)
                        {
                            cmdLeer.Focus();
                            cmdLeer.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            else
            {
                if (Importe.Text != "")
                {
                    if (TypeUsuario.Id_Company == Globales.EMPREF && TypeUsuario.Id_Branch == Globales.EMPREF2)
                    {
                        cmdLeer.IsEnabled = true;
                        if (cmdLeer.Visibility == Visibility.Visible)
                        {
                            cmdLeer.Focus();
                        }
                    }
                }
            }
            Importe.Text = LimpiarCampo(Importe.Text);
        }
        private string LimpiarCampo(string p)
        {
            return modMIT.LimpiarCampo(p);
        }
        private void formaPagoClick(object sender, SelectionChangedEventArgs e)
        {
            FormaPagoClick();
        }



        private void soloTexto(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void mayuscula(object sender, RoutedEventArgs e)
        {
            TextBox texto = (TextBox)sender;
            texto.Text = texto.Text.ToUpper();
        }
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

                    aux = NumTdc.Text.Substring(NumTdc.Text.Length - 4);
                    strXML += " <numeroTarjeta>" + aux + "</numeroTarjeta>";

                    strXML += " <poliza>" + typeUsuarioQualitas.PolizaNumero + "</poliza>";
                    strXML += " <recibo>" + typeUsuarioQualitas.PolizaReciboNumero + "</recibo>";
                    strXML += " <tarjetaHabiente>" + NomTdc.Text + "</tarjetaHabiente>";
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
                    aux = NumTdc.Text.Substring(NumTdc.Text.Length - 4);
                    strXML += " <numeroTarjeta>" + aux + "</numeroTarjeta>";

                    strXML += " <poliza>" + typeUsuarioQualitas.DeduciblePoliza + "</poliza>";
                    //'strXml = strXml & " <siniestro>" & typeUsuarioQualitas.DeducibleSiniestro & "</siniestro>"
                    strXML += " <tarjetaHabiente>" + NomTdc.Text + "</tarjetaHabiente>";
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
        private void cmdQualitasOtroCobro_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
            Globales.bolActivaMotoP = true;
            Globales.isVentasPropias = true;
            abrirFrmNuevo(this);
        }
        private void cmdSalirQualitas_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
        }
        private void cmdActualizaPago_Click_1(object sender, RoutedEventArgs e)
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

                    aux = NumTdc.Text.Substring(NumTdc.Text.Length - 4);
                    strXML += " <numeroTarjeta>" + aux + "</numeroTarjeta>";

                    strXML += " <poliza>" + typeUsuarioQualitas.PolizaNumero + "</poliza>";
                    strXML += " <recibo>" + typeUsuarioQualitas.PolizaReciboNumero + "</recibo>";
                    strXML += " <tarjetaHabiente>" + NomTdc.Text + "</tarjetaHabiente>";
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
                    aux = NumTdc.Text.Substring(NumTdc.Text.Length - 4);
                    strXML += " <numeroTarjeta>" + aux + "</numeroTarjeta>";

                    strXML += " <poliza>" + typeUsuarioQualitas.DeduciblePoliza + "</poliza>";
                    //'strXml = strXml & " <siniestro>" & typeUsuarioQualitas.DeducibleSiniestro & "</siniestro>"
                    strXML += " <tarjetaHabiente>" + NomTdc.Text + "</tarjetaHabiente>";
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

        private void Importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }




        #region "*************************************Botones*************************************"

        private void cmdLeerBoton(object sender, RoutedEventArgs e)
        {
            //ANTES DE ACTIVAR PINPAD REVISA QUE SEA CORRECTA LA REFERENCIA GEV

            string xml = TypeUsuario.CadenaXML;
            if (Globales.GetDataXml("id_company", xml) == Globales.RGEV)
            {//Prueba GEV
                int indice = FormaPago.SelectedIndex;
                lstMerchant.SelectedIndex = indice;

                Globales.RefGEV(Globales.GetDataXml("refgev", xml), Convert.ToString(lstMerchant.SelectedItem));
                if (!NumOrden.Text.Contains(Globales.CodGEV))
                {
                    Globales.MessageBoxMit("Referencia invalida");
                    NumOrden.Focus();
                    NumOrden.Text = "";
                    return;
                }
                if (!Globales.RefGEV(Globales.GetDataXml("refgev", xml), Convert.ToString(lstMerchant.SelectedItem), NumOrden.Text))
                {
                    Globales.MessageBoxMit("Referencia invalida para la afiliacion seleccionada");
                    NumOrden.Focus();
                    NumOrden.Text = "";
                    return;
                }
            }

            if (TypeUsuario.Id_Company == Globales.EMPREF && TypeUsuario.Id_Branch == Globales.EMPREF2)
                NumOrden.Text = "REFTEMP" + Importe.Text;


            if (Globales.isQualitas)
                if (typeUsuarioQualitas.TipoCobro.ToUpper() != "OTROS" && !string.IsNullOrWhiteSpace(typeUsuarioQualitas.TipoCobro))
                {
                    if (string.IsNullOrWhiteSpace(txtEmail.Text))
                    {
                        Globales.MessageBoxMit("Introduzca Correo Electrónico.");
                        txtEmail.Text = "";
                        txtEmail.Focus();
                        return;
                    }
                    else
                        txtEmail.IsEnabled = false;
                }


            NumTdc.Text = "";
            NomTdc.Text = "";

            this._mes = string.Empty;
            this._anio = string.Empty;
            TFECHAVENC.Text = "";
            cboBanco.IsEnabled = true;
            FormaPago.IsEnabled = true;
            NumOrden.IsEnabled = true;
            Importe.IsEnabled = true;
            this.BENVIAMAIL.Visibility = Visibility.Hidden;

            //Recompensas directo
            Globales.ReseteaVariablesRecompensas();


            if (string.IsNullOrWhiteSpace(this.NumOrden.Text))
            {
                Globales.MessageBoxMit(string.Format("Introduzca {0}.", TypeUsuario.reference));
                NumOrden.Focus();
                return;
            }


            if (string.IsNullOrWhiteSpace(Importe.Text))
            {
                Globales.MessageBoxMit("Introduzca el importe");
                Importe.Focus();
                return;
            }

            if (TypeUsuario.Id_Company == "0059")
                if (NumOrden.Text.Length != 28)
                {
                    Globales.MessageBoxMit("El número de referencia debe de ser de 28 posiciones");
                    NumOrden.Focus();
                    return;
                }
            this.realizaOp();
        }
        private void enviarCobro(object sender, RoutedEventArgs e)
        {
            if (TypeUsuario.Id_Company == Globales.EMPREF && TypeUsuario.Id_Branch == Globales.EMPREF2)
            {
                NumOrden.Text = "REFTEMP" + Importe.Text;
            }

            CmdEnviar.IsEnabled = false;
            label_6.IsEnabled = false;
            FormaPago.IsEnabled = false;
            //lblTError.Text = "";
            this.lblMensaje.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(NumTdc.Text) || string.IsNullOrWhiteSpace(this._mes) || string.IsNullOrWhiteSpace(this._anio))
            {

                Globales.MessageBoxMit("Deslice la tarjeta bancaria");
                CmdEnviar.IsEnabled = true;
            }
            else if (string.IsNullOrWhiteSpace(NumOrden.Text))
            {
                Globales.MessageBoxMit("Introduzca " + TypeUsuario.reference);
                NumOrden.Focus();
                CmdEnviar.IsEnabled = true;
            }
            else if (string.IsNullOrWhiteSpace(Importe.Text))
            {
                Globales.MessageBoxMit("Introduzca el importe.");
                Importe.Focus();
                CmdEnviar.IsEnabled = true;
            }
            else if (Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML).Contains("1") && Globales.GetDataXml("tarjeta", Globales.cpHTTP_sResult) != NumTdc.Text)
            {
                Globales.MessageBoxMit("La tarjeta deslizada no corresponde con el Servicio a cobrar.");
                CmdEnviar.IsEnabled = true;
            }
            else
            {
                if (TypeUsuario.Id_Company == "0059")
                {
                    if (NumOrden.Text.Length != 28)
                    {
                        Globales.MessageBoxMit("El no. de REFERENCIA debe ser de 28 posiciones");
                        NumOrden.Focus();
                        CmdEnviar.IsEnabled = true;
                        return;
                    }
                }

                double resultado = 0;
                if (!(double.TryParse(Importe.Text, out resultado)))
                {
                    Globales.MessageBoxMit("El importe debe ser numérico.");
                    Importe.Focus();
                    Globales.cpIntegraEMV.dbgCancelOperation();
                    CmdEnviar.IsEnabled = true;
                    return;
                }


                //EMV full
                string strTypeC = string.Empty;
                string strCadEncriptar = string.Empty;
                string Voucher = string.Empty;

                CmdEnviar.IsEnabled = false;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                //Se agrega validación para no permitir enviar la transacción sin contar con merchant
                if (string.IsNullOrWhiteSpace(Globales.merchantBanda))
                {
                    Globales.MessageBoxMit("No hay planes de pago para esta tarjeta, porfavor cambie la tarjeta.");
                    cmdLeer.Visibility = Visibility.Visible;
                    cmdLeer.IsEnabled = true;
                    CmdEnviar.IsEnabled = false;
                    CmdEnviar.Visibility = Visibility.Hidden;
                    CmdNuevo.IsEnabled = false;
                    CmdNuevo.Visibility = Visibility.Hidden;
                    cboBanco.IsEnabled = true;
                    FormaPago.IsEnabled = true;
                    Importe.IsEnabled = true;
                    NumOrden.IsEnabled = true;

                    this._mes = string.Empty;
                    this._anio = string.Empty;

                    NumTdc.Text = "";
                    NomTdc.Text = "";
                    StatusCmd = true;
                    Globales.cpIntegraEMV.dbgCancelOperation();
                    Mouse.OverrideCursor = null;
                    return;
                }



                Globales.cpIntegraEMV.dbgSetCurrencyType(Convert.ToString(lblMoneda.Content));

                if (Globales.isAerolinea && !Globales.isVentasPropias)
                {
                    Globales.cpIntegraEMV.sndCheckInEMV(TypeUsuario.usu, TypeUsuario.Pass, "", TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
                        TypeUsuario.country, strTypeC, Globales.merchantBanda, NumOrden.Text,
                        "9", Convert.ToString(lblMoneda.Content), "", Globales.csvAmexenBanda);
                }
                else
                {
                    #region delimitador
                    //Se valida el fluijo para comprar con puntos de Recomensas santander
                    //Si no cumple con condiciones de cobro recompensas, sigue con el flujo normal de cobro
                    if (Globales.sinSaldoRecomDirecto)
                    {
                        //goto cobro nomrmal
                        cobroVtaNormalBool = true;
                        goto ir;
                    }

                    var _aplicaRecompensas = Globales.cpIntegraEMV.AplicaCobroRecompensas(Globales.cpIntegraEMV.chkCc_Number().Substring(0, 6));
                    var _tipoMoneda = Globales.cpIntegraEMV.GetTipoMoneda();

                    if (_aplicaRecompensas &&
                        !(_tipoMoneda == "USD"))
                    {
                        #region "Recompensas santander"

                        Globales.errorRecom = "";
                        //Se hace un cobro para conocer el saldo en puntos

                        Globales.cpIntegraEMV.sndVtaRecompensasDirecto(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
                            TypeUsuario.country, NumOrden.Text, "0.01", "101");

                        string errorRecom =Globales.cpIntegraEMV.getRspCdError();



                        if (errorRecom.Equals("92"))
                        {
                            Globales.cpIntegraEMV.dbgEndOperation();
                            goto irApprob;
                        } 

                        Globales.saldoRecomDirecto = Globales.cpIntegraEMV.getRspSaldoRecom();

                        

                        //Valida si alcanzan los puntos
                        double contenido = 0;
                        double.TryParse(Globales.saldoRecomDirecto,out contenido);
                        if (contenido > 0)
                        {
                            string puntosPesos = string.Empty;
                            Globales.sinSaldoRecomDirecto = false;
                            if (Convert.ToDouble(Globales.saldoRecomDirecto) >= Convert.ToDouble(Importe.Text))
                            {
                                puntosPesos = "Saldo recompensas: $" + Globales.saldoRecomDirecto + "\n";
                                puntosPesos = puntosPesos + "¿Deseas pagar $" + Importe.Text + "  con Puntos recompensas? ";
                            }
                            else
                            {
                                Globales.diferenciaRecomDirecto = Convert.ToString(Convert.ToDouble(Importe.Text) - Convert.ToDouble(Globales.saldoRecomDirecto));
                                puntosPesos = "Saldo recompensas: $" + Globales.saldoRecomDirecto + "\n";
                                puntosPesos += "¿Deseas pagar $" + Globales.saldoRecomDirecto + " con Puntos Recompensas \n y $" + Globales.diferenciaRecomDirecto + " con tarjeta Bancaria?";

                            }
                            Mouse.OverrideCursor = null;
                            var dialogo = Globales.MessageConfirm(puntosPesos);
                            if (dialogo)
                            {
                                Globales.sinSaldoRecomDirecto = false;
                                if (Convert.ToDouble(Globales.saldoRecomDirecto) >= Convert.ToDouble(Importe.Text))
                                {
                                    Globales.cpIntegraEMV.dbgHidePopUp(true);
                                    if (Globales.cpIntegraEMV.chkPp_soportaDUKPT())
                                    {
                                        Globales.cpIntegraEMV.dbgCancelOperation();
                                        Globales.MessageBoxMit("Para continuar con el proceso, favor de retirar la tarjeta.");

                                        realizarLecturaRecom(Importe.Text, true);
                                    }
                                    else
                                    {
                                        this.realizaCobroRecom(Importe.Text);
                                    }

                                    Globales.sinSaldoRecomDirecto = false;

                                    Globales.cpIntegraEMV.dbgHidePopUp(false);
                                    return;
                                }
                                else
                                {
                                    Globales.diferenciaRecomDirecto = Convert.ToString(Convert.ToDouble(Importe.Text) - Convert.ToDouble(Globales.saldoRecomDirecto));
                                    //Se cobra con tarjeta bancaria primero
                                    Globales.sinSaldoRecomDirecto = true;
                                    Globales.hayTrxRecompensa = true;
                                    Globales.cpIntegraEMV.dbgCancelOperation();
                                    Globales.MessageBoxMit("Para continuar con el proceso, porfavor de retirar la tarjeta.");

                                    Globales.cpIntegraEMV.dbgHidePopUp(true);
                                    this.realizarLecturaRecom(Globales.diferenciaRecomDirecto, false);
                                    Globales.cpIntegraEMV.dbgHidePopUp(false);
                                    Mouse.OverrideCursor = null;
                                    return;
                                }
                            }
                            else
                            {
                                //Cobro con tarjeta bancaria, sin puntos de recompensas
                                if (Globales.cpIntegraEMV.chkPp_soportaDUKPT())
                                {
                                    Globales.sinSaldoRecomDirecto = true;
                                    Globales.cpIntegraEMV.dbgCancelOperation();

                                    Globales.MessageBoxMit("Para continuar con el proceso, favor de retirar la tarjeta.");
                                   // Globales.cpIntegraEMV.dbgHidePopUp(true);
                                    this.realizarLecturaRecom(Importe.Text, false);
                                    Globales.cpIntegraEMV.dbgHidePopUp(false);
                                    Mouse.OverrideCursor = null;
                                    return;
                                }
                                else
                                {
                                    if (Globales.merchantBanda == "-1")
                                    {
                                        Globales.merchantBanda = Globales.cpIntegraEMV.dbgGetMerchantBanda("9");
                                    }
                                    cobroVtaNormalBool = true;
                                    goto ir;

                                }
                            }
                        }
                        else
                        {
                            Globales.sinSaldoRecomDirecto = true;
                            Globales.errorRecom = "Saldo en Puntos Recompensas Santander en $ 0.00/ Volver a deslizar la tarjeta para cobro bancario.";
                            if (Globales.cpIntegraEMV.chkPp_soportaDUKPT())
                            {
                                Globales.sinSaldoRecomDirecto = true;
                                Globales.cpIntegraEMV.dbgCancelOperation();

                                Globales.MessageBoxMit("Saldo en puntos de Recompensas Santander en $ 0.00\nFavor de retirar la tarjeta.");
                                Globales.cpIntegraEMV.dbgHidePopUp(true);
                                this.realizarLecturaRecom(Importe.Text, false);
                                Globales.cpIntegraEMV.dbgHidePopUp(false);
                                Globales.sinSaldoRecomDirecto = false;
                                Mouse.OverrideCursor = null;
                                return;
                            }
                            else
                            {
                                Globales.MessageBoxMit("Saldo en Puntos Recompensas Sandander en $ 0.00");
                                if (Globales.merchantBanda == "-1")
                                {
                                    Globales.merchantBanda = Globales.cpIntegraEMV.dbgGetMerchantBanda("9");
                                }
                                cobroVtaNormalBool = true;
                                goto ir;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Globales.sinSaldoRecomDirecto = false;
                        Globales.errorRecom = "";
                        Globales.cpIntegraEMV.sndVtaDirectaEMV(TypeUsuario.usu, TypeUsuario.Pass, "", TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
                                TypeUsuario.country, strTypeC, Globales.merchantBanda, NumOrden.Text, "9", Convert.ToString(lblMoneda.Content), Globales.cvsAmexEnBanda);

                    }
                ir:

                    if (cobroVtaNormalBool)
                    {
                        Globales.sinSaldoRecomDirecto = false;
                        Globales.errorRecom = "";
                        Globales.cpIntegraEMV.sndVtaDirectaEMV(TypeUsuario.usu, TypeUsuario.Pass, "", TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
                                TypeUsuario.country, strTypeC, Globales.merchantBanda, NumOrden.Text, "9", Convert.ToString(lblMoneda.Content), Globales.cvsAmexEnBanda);
                        cobroVtaNormalBool = false;
                    }
                    #endregion
                }
                Globales.csvAmexenBanda = "";
                Mouse.OverrideCursor = null;
            irApprob:
                switch (Globales.cpIntegraEMV.getRspDsResponse())
                {

                    case "approved"://Transacción Aprobada
                        #region"Aprovado"
                        cboBanco.IsEnabled = false;
                        FormaPago.IsEnabled = false;
                        NumOrden.IsEnabled = false;
                        Importe.IsEnabled = false;
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
                        string mensaje = Globales.cpIntegraEMV.getRspAuth();
                        Globales.MessageBoxMitApproved(mensaje);
                        TypeUsuario.strVoucherCoP = Globales.cpIntegraEMV.getRspVoucher().Trim();
                        //**************************************AEREOLINEA****************************
                        if (Globales.isAerolinea && !Globales.isVentasPropias)
                        {
                            //Ira todo sobre formularios boletos
                            frmBoletosAerolinea = new frmBoletosAerolinea();
                            frmBoletosAerolinea.Importe = Importe.Text;
                            frmBoletosAerolinea.auth = Globales.cpIntegraEMV.getRspAuth();
                            frmBoletosAerolinea.ShowDialog();
                        }
                        else
                        {

                            Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();
                            //Se salta el fluijo de la firma en el panel y la impresion del voucher

                            if (Globales.cpIntegraEMV.dbgGetEsImprimibleVoucher() == "0")
                            {
                                cmdVoucher.Visibility = Visibility.Hidden;
                                goto continuaFlujoQPS;
                            }
                            else
                            {
                                cmdVoucher.Visibility = Visibility.Visible;
                            }

                            //*****************Firma en panel *************************+
                            //Validaciones para la firma en el panel


                            //Marca de agua ceriroji
                            string textoAgua = this.getMarcaAgua();
                            //Valida si la tarjeta es Chip y Pin
                            bool IsChipAndPin, esQPS;
                            string cadenaHexFirma = "";
                            int tipoVta = 0;
                            IsChipAndPin = false;
                            esQPS = false;
                            tipoVta = 1;

                            //Valida si es chipp and pin
                            if (Globales.cpIntegraEMV.chkCc_IsPin() == "01")
                            {
                                IsChipAndPin = true;
                            }

                            //Valida si la venta es QPS

                            if (Globales.cpIntegraEMV.getRspVoucher().Contains("@cnn Autorizado sin Firma ") && !IsChipAndPin && tipoVta == 1)
                            {
                                esQPS = true;
                            }

                            if (!Globales.cpIntegraEMV.EsTouch() && !Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                            {
                                goto GoImpresion;
                                //GoImpresion();
                            }
                            //Si la pinpad no soporta firma en panel y no es touch, sigue el flujo normal de programa de pcpay
                            if (!Globales.cpIntegraEMV.EsTouch() && Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                            {
                                //LLama a la funcion de obtener la firma en el panel

                                cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(false, textoAgua, Globales.cpIntegraEMV.getRspVoucher(), IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), Convert.ToInt16(tipoVta), esQPS);
                                //Otras cosas del firma panel que es touch o no se va a checar despuest.....
                                if (!cadenaHexFirma.Contains("Error"))
                                {
                                    if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Globales.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                    {
                                        this.imgEmailFirmaPanel.Visibility = Visibility.Visible;
                                    }
                                }
                                else
                                    Globales.MessageBoxMit("No se pudo obtener la imagen del pinpad\n" + cadenaHexFirma);

                                goto GoImpresion;
                            }
                            //Si el dispositivo tiene capacidad Tuch

                            //NOTA: AQUI FALTA MÁS COSAS EN EL CÓDIGO.....
                            if (Globales.cpIntegraEMV.EsTouch())
                            {
                                //MUCHO CÓDIGO POR SI ES TOUCH
                                if (TypeUsuario.UtilizarCapacidadTouch && TypeUsuario.EnviarVoucherMail)
                                {
                                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                    if (Globales.ObtieneFirmaPanel(Globales.ipFirmaPanel, textoAgua, (short)tipoVta, IsChipAndPin, esQPS))
                                    {
                                        Mouse.OverrideCursor = null;
                                        goto GoImpresion;
                                    }
                                    else {
                                        Mouse.OverrideCursor = null;
                                        goto finaliza;
                                    }
                                }
                                if (TypeUsuario.UtilizarCapacidadTouch && !TypeUsuario.EnviarVoucherMail)
                                {
                                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                    cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(true, textoAgua, Globales.cpIntegraEMV.getRspVoucher(), IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), Convert.ToInt16(tipoVta), esQPS);
                                    Mouse.OverrideCursor = null;
                                    if (!cadenaHexFirma.Contains("Error"))
                                    {
                                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                    var boleano = Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Globales.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial());
                                    Mouse.OverrideCursor = null;
                                        if (boleano)
                                            this.imgEmailFirmaPanel.Visibility = Visibility.Visible;
                                        else
                                            Globales.MessageBoxMit("¡No se pudo obtener la imagen del PinPad!");
                                    goto GoImpresion;
                                    }
                                }
                                if (!TypeUsuario.UtilizarCapacidadTouch && !TypeUsuario.EnviarVoucherMail)
                                {
                                    if (Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                                    {
                                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                        cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(false, textoAgua, Globales.cpIntegraEMV.getRspVoucher(), IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), Convert.ToInt16(tipoVta), esQPS);
                                        Mouse.OverrideCursor = null;
                                        if (!cadenaHexFirma.Contains("Error"))
                                        {
                                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                            var variable = Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Globales.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial());
                                            Mouse.OverrideCursor = null;
                                            if (variable)
                                                this.imgEmailFirmaPanel.Visibility = Visibility.Visible;
                                            else
                                                Globales.MessageBoxMit("¡No se pudo obtener la imagen del PinPad!");
                                            goto GoImpresion;
                                        }
                                    }
                                }
                            }
                        GoImpresion:
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            Voucher = Globales.cpIntegraEMV.getRspVoucher();
                            if (Voucher.Contains("@cnb -C-O-P-I-A- "))
                            {
                                Voucher.Replace("@cnb -C-O-P-I-A- ", "");
                            }
                            System.Windows.Forms.PrintDialog impresora = new System.Windows.Forms.PrintDialog();
                            Globales.PrintOptions(Voucher, Globales.cpIntegraEMV.getRspOperationNumber(), impresora);
                            cmdVoucher.IsEnabled = true;

                            //Recompensas
                            Globales.strCadenaVoucherVtaDirecta = TypeUsuario.strVoucher;
                            Mouse.OverrideCursor = null;
                        }
                    continuaFlujoQPS:
                        if (Globales.hayTrxRecompensa)
                        {
                            Globales.MessageBoxMit("Procesando la transacción con Puntos Recompensas Santander");
                            Globales.hayTrxRecompensa = false;

                            //SE cobran los puntos 

                            Globales.sinSaldoRecomDirecto = false;

                            Globales.cpIntegraEMV.dbgHidePopUp(true);
                            this.realizarLecturaRecom(Globales.saldoRecomDirecto, true);

                            Globales.hayDobleVoucherRecom = true;
                            Thread.Sleep(2000);
                            Globales.cpIntegraEMV.dbgHidePopUp(false);
                        }
                    finaliza:
                         if (!(Globales.hayTrxRecompensa))
                        {
                            CmdNuevo.Visibility = Visibility.Visible;
                            CmdNuevo.IsEnabled = true;
                        }

                        CmdEnviar.Visibility = Visibility.Hidden;
                        if (Globales.FacturaE == "1")
                        {
                            var dialogo = Globales.MessageConfirm("¿Desea factura electrónica?");
                            if (dialogo)
                            {
                                frmPreguntaFactura factura = new frmPreguntaFactura();
                                abrirFrmNuevo(new frmPreguntaFactura()
                                {
                                    abrirFrmAhora = abrirFrmNuevo,
                                    cerraPage = cerrar
                                });
                                Mouse.OverrideCursor = null;
                                return;
                            }
                            else
                                Globales.XMLFacturaE = "";

                        }
                        //****************Si eres qualitas***************+

                        if (Globales.isQualitas)
                        {
                            //Cosas y opciones de qualitas.
                            if (typeUsuarioQualitas.TipoCobro.ToUpper() != "OTROS" && !string.IsNullOrWhiteSpace(typeUsuarioQualitas.TipoCobro))
                            {
                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                try
                                {
                                    cmdActualizaPago_Click(null, null);
                                }
                                catch { }
                                Mouse.OverrideCursor = null;
                                CmdNuevo.Visibility = Visibility.Hidden;
                                //imgEmail.Visibility = Visibility.Hidden;
                            }
                        }
                        #endregion
                        break;
                    case "denied":
                        #region "Denegado"
                        if (Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML).Substring(0, 1) == "1")
                        {
                            Globales.cpHTTP_Clear();
                            Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
                            strCadEncriptar = string.Format("&op=facileasingcobrorechazada&nueconomico={0}&nuservicio={1}&nuproveedor={2}&transaccion={3}}&importe={4}&numtdc={5}&auth={6}&response={7}", strNoEconomico, strNoServicio, strProveedor, Globales.cpIntegraEMV.getRspOperationNumber(), Importe.Text, NumTdc.Text.Substring(12, 4), "", Globales.cpIntegraEMV.getRspCdResponse());
                            Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);

                            if (Globales.cpHTTP_SendcpCUCT())
                            {

                            }
                        }
                        Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
                        //lblAprob.Visibility = Visibility.Hidden;
                        //lblAuth.Visibility = Visibility.Hidden;

                        //lblTError.Visibility = Visibility.Hidden;
                        //lblDenied.Visibility = Visibility.Visible;
                        string mensajeD = Globales.msjRech + "\n" + Globales.cpIntegraEMV.getRspCdResponse() + " " + Globales.cpIntegraEMV.getRspFriendlyResponse();
                        CmdNuevo.Visibility = Visibility.Visible;
                        CmdNuevo.IsEnabled = true;
                        CmdEnviar.Visibility = Visibility.Hidden;
                        cmdVoucher.IsEnabled = false;
                        cmdVoucher.Visibility = Visibility.Visible;
                        Globales.MessageBoxMitDenied(mensajeD);
                        if (Globales.isQualitas)
                        {
                            CmdNuevo.Visibility = Visibility.Hidden;
                            cmdLeer.Visibility = Visibility.Visible;
                            cmdLeer.IsEnabled = false;
                            cboBanco.IsEnabled = false;
                            FormaPago.IsEnabled = false;
                            Importe.IsEnabled = false;
                            NumOrden.IsEnabled = false;

                            //lblAprob.Visibility = Visibility.Hidden;
                            //lblAuth.Visibility = Visibility.Hidden;
                            //lblDenied.Visibility = Visibility.Visible;
                            mensajeD = Globales.msjRech + "\n" + Globales.cpIntegraEMV.getRspCdResponse() + " " + Globales.cpIntegraEMV.getRspFriendlyResponse();
                            StatusCmd = false;
                            Globales.isQualitasCierraForm = true;
                            realizaCoberturaMultiple();
                        }
                        #endregion
                        break;
                    case "error":
                        #region "error"
                    veErrorRecom:
                        cmdLeer.Visibility = Visibility.Visible;
                        cmdLeer.IsEnabled = true;
                        CmdEnviar.IsEnabled = false;
                        CmdEnviar.Visibility = Visibility.Visible;
                        CmdNuevo.Visibility = Visibility.Hidden;
                        CmdNuevo.IsEnabled = false;
                        cboBanco.IsEnabled = true;
                        FormaPago.IsEnabled = true;
                        Importe.IsEnabled = true;
                        NumOrden.IsEnabled = true;
                        var mensajeE = Globales.cpIntegraEMV.getRspDsError();
                        if (!mensajeE.Contains("La transaccion ya fue aprobada"))
                        {
                            StatusCmd = true;

                            if (!string.IsNullOrWhiteSpace(Globales.errorRecom))
                            {
                                Globales.MessageBoxMitError(Globales.errorRecom);
                                Thread.Sleep(2000);
                                Globales.cpIntegraEMV.dbgCancelOperation();
                            }

                            //Si eres Qualitas abra otro código pero esto todavía me falta...
                            if (Globales.isQualitas)
                            {
                                CmdEnviar.IsEnabled = false;
                                cmdLeer.Visibility = Visibility.Visible;
                                cmdLeer.IsEnabled = false;
                                cboBanco.IsEnabled = false;
                                FormaPago.IsEnabled = false;
                                Importe.IsEnabled = false;
                                NumOrden.IsEnabled = false;
                                StatusCmd = false;
                                Globales.isQualitasCierraForm = true;
                                realizaCoberturaMultiple();
                            }
                        }
                        else {
                            NumOrden.IsEnabled = false;
                            Importe.IsEnabled = false;
                            cmdLeer.Visibility = Visibility.Hidden;
                            CmdEnviar.Visibility = Visibility.Hidden;
                            CmdNuevo.Visibility = Visibility.Visible;
                            CmdNuevo.IsEnabled = true;
                        }
                        Globales.MessageBoxMitError(mensajeE);
                        #endregion
                        break;
                    default:
                        #region "Default answer"
                    veLeerTarjetaRecom:
                        CmdEnviar.IsEnabled = false;
                        cmdLeer.Visibility = Visibility.Visible;
                        cmdLeer.IsEnabled = true;
                        cboBanco.IsEnabled = true;
                        FormaPago.IsEnabled = true;
                        Importe.IsEnabled = true;
                        NumOrden.IsEnabled = true;
                        CmdNuevo.IsEnabled = false;
                        CmdNuevo.Visibility = Visibility.Hidden;

                        //lblAprob.Visibility = Visibility.Hidden;
                        //lblAuth.Visibility = Visibility.Hidden;

                        //lblTError.Visibility = Visibility.Visible;
                        var mensajeF = "Error de conexión, verifique su reporte";
                        Globales.MessageBoxMitError(mensajeF);
                        //lblDenied.Visibility = Visibility.Hidden;
                        StatusCmd = true;

                        //Error de recompensas

                        if (!string.IsNullOrWhiteSpace(Globales.errorRecom))
                        {
                            Globales.MessageBoxMitError(Globales.errorRecom);
                            Thread.Sleep(2000);
                            Globales.cpIntegraEMV.dbgCancelOperation();
                        }

                        //Falta el código si eres Qualitas
                        if (Globales.isQualitas)
                        {
                            CmdEnviar.IsEnabled = false;
                            cmdLeer.Visibility = Visibility.Visible;
                            cmdLeer.IsEnabled = false;
                            cboBanco.IsEnabled = false;
                            FormaPago.IsEnabled = false;
                            Importe.IsEnabled = false;
                            NumOrden.IsEnabled = false;
                            StatusCmd = false;
                            realizaCoberturaMultiple();
                            Globales.isQualitasCierraForm = true;
                        }
                        #endregion
                        break;
                }

                //Si eres Qualitas

                if (Globales.isQualitas)
                {
                    CmdNuevo.Visibility = Visibility.Hidden;
                    cmdLeer.Visibility = Visibility.Visible;
                    cmdLeer.IsEnabled = false;
                    cboBanco.IsEnabled = false;
                    FormaPago.IsEnabled = false;
                    Importe.IsEnabled = false;
                    NumOrden.IsEnabled = false;

                    //lblAprob.Visibility = Visibility.Hidden;
                    //lblAuth.Visibility = Visibility.Hidden;

                    //lblDenied.Visibility = Visibility.Visible;
                    var mensajeD = Globales.msjRech + "\n" + Globales.cpIntegraEMV.getRspCdResponse() + " " + Globales.cpIntegraEMV.getRspFriendlyResponse();
                    Globales.MessageBoxMitDenied(mensajeD);
                    StatusCmd = false;
                    Globales.isQualitasCierraForm = true;
                    Globales.cpIntegraEMV.dbgHidePopUp(true);
                    realizaCoberturaMultiple();
                    Globales.cpIntegraEMV.dbgHidePopUp(false);
                }

                //Globales.GloboPcPay(out lblTError,out lblAprob,out lblDenied,out lblAuth);
            }
            Mouse.OverrideCursor = null;
        }
        private void otroCobro(object sender, RoutedEventArgs e)
        {
            cmdNuevoClick();
        }
        private void cmdNuevoClick()
        {
            Globales.strNombreFP = "CmdNuevo";
            //frmPlanPagos.merchant = "";
            //frmPlanPagos.TipoTarjeta = "";
            //frmPlanPagos.Bin = "";

            Globales.csvAmexenBanda = "";
            StatusCmd = true;

            cboBanco.IsEnabled = true;
            FormaPago.IsEnabled = true;
            NumOrden.IsEnabled = true;
            Importe.IsEnabled = true;
            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.imgEmailFirmaPanel.Visibility = Visibility.Hidden;

            this.limpia();

            TypeUsuario.strVoucherCoP = "";
            cmdLeer.Visibility = Visibility.Visible;
            this.cmdLeer.IsEnabled = true;
            TFECHAVENC.Text = "";

            this.CmdEnviar.Visibility = Visibility.Hidden;
            this.CmdEnviar.IsEnabled = false;
            //imgEmail.Visibility = Visibility.Hidden;

            label_6.Visibility = Visibility.Hidden;
            FormaPago.Visibility = Visibility.Hidden;
            lblMoneda.Content = "MXN";

            if (Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML).Substring(0, 1) == "1")
            {
                //Se aprieta el boton del menu de la ventana. no se porque jajajajajaj
                //MDImit.mnu2_2_click
            }

            Globales.cpIntegraEMV.dbgEndOperation();
            cmdLeer.Visibility = Visibility.Visible;
            //imgFirma.Visibility = Visibility.Hidden;

            //Recompensas directo
            Globales.ReseteaVariablesRecompensas();

        }
        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir(true);
        }

        private string getMarcaAgua()
        {
            string textoAgua = string.Empty;

            textoAgua = "Folio: " + Globales.cpIntegraEMV.getRspOperationNumber() + "\n";
            textoAgua += "Auth: " + Globales.cpIntegraEMV.getRspAuth() + "\n";
            textoAgua += "Importe: " + Globales.cpIntegraEMV.getTx_Amount() + "\n";
            textoAgua += "Fecha: " + Globales.cpIntegraEMV.getRspDate() + "\n";
            textoAgua += "Merchant: " + Globales.cpIntegraEMV.getRspDsMerchant() + "\n";
            textoAgua += " " + "\n";
            textoAgua += "_____________" + "\n";
            textoAgua += "FIRMA DIGITALIZADA" + "\n";

            return textoAgua;
        }


        #endregion

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender,e);
        }

        private void imgEmailFirmaPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Globales.sendMailFirmaPanel_MouseDown(sender,e);
        }
    }


}
