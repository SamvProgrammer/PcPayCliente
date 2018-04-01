using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using PcPay.Forms.Formularios;
using PcPay.Forms.Formularios.MessagesW;
//using PcPay.Forms.formularios;
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
using System.Windows.Threading;

namespace PcPay.Forms.Pages
{
    /// <summary>
    /// Lógica de interacción para frmBanda.xaml
    /// </summary>
    public partial class frmBanda : Page
    {

        public cerrarVentana cerrar;
        public List<string> lista_boletos { get; set; }
        public string _mes { get; set; }
        public string _anio { get; set; }

        private bool YaCobro = false;
        public abrirFrm abrir;
        private string strNoEconomico;
        private string strNoServicio;
        private string strProveedor;
        private string cadenadeboletos;
        private bool banboletosrepetidos;
        public Window propietario { get; set; }

        private string strfolio;
        private bool StatusCmd;
        private const string NOMBRE_GENERAL = "frmBanda";
        private string boletos;
        private string auxiliar;
        private string clave;
        private string caption;
        DispatcherTimer tiempo;

        public frmBanda()
        {
            InitializeComponent();
            
            lista_boletos = new List<string>();
            Globales.csvAmexenBanda = string.Empty;
            this.TFECHAVENC.Text = string.Empty;

            this.fraCubreTodo.Visibility = Visibility.Visible;
            this.frmDatos.Visibility = Visibility.Hidden;
            this.BoletosExtra.Visibility = Visibility.Hidden;

            Cargar();
            tiempo = new DispatcherTimer();
            
            #region"**********Efectos**********"
            this.txtNoReservacion.GotFocus += Globales.setFocusMit2;
            this.Importe.GotFocus += Globales.setFocusMit2;
            this.TNUMBOLETO.GotFocus += Globales.setFocusMit2;

            TNUMBOLETO.GotFocus += Globales.setFocusMit2;
            TNUMBOLETO.LostFocus += Globales.lostFocusMit2;

            this.txtNoReservacion.LostFocus += Globales.lostFocusMit2;
            this.Importe.LostFocus += Globales.lostFocusMit2;
            this.TNUMBOLETO.GotFocus += Globales.lostFocusMit2;

            #endregion
            TNUMBOLETO.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            Importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;

            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;
            this.BENVIAMAIL.MouseDown += Globales.sendMail_GridMouseDown;
        }


        private void LblTError_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void CmdLeer_Click(object sender, RoutedEventArgs e)
        {
            this.NumTdc.Text = "";
            this.NomTdc.Text = "";

            this.TFECHAVENC.Text = string.Empty;
            this._anio = string.Empty;
            this._mes = string.Empty;



            this.realizaOp();
            Mouse.OverrideCursor = null;

        }
        private string CadenaBoletos()
        {
            //return Globales.CadenaBoletos(BoletosExtra, lbl3Digitos.Content.ToString(), NoBoletosExtra);
            string returnValue = string.Empty;

            if (this.lista_boletos == null)
                returnValue = string.Empty;
            else
            {
                if (this.lista_boletos.Count == 0)
                    returnValue = string.Empty;
                else
                    returnValue = string.Join(",", this.lista_boletos.ToArray());
            }
            return returnValue;
        }
        private void realizaOp()
        {

            int intDigVer;
            if (string.IsNullOrWhiteSpace(txtNoReservacion.Text))
            {
                Globales.MessageBoxMit("Introduzca el No. de reservación.");
                txtNoReservacion.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(Importe.Text))
            {
                Globales.MessageBoxMit("Introduzca el importe.");
                Importe.Focus();
                return;
            }
            else
                if (!Globales.IsNumeric(Importe.Text))
                {
                    Globales.MessageBoxMit("El importe debe ser numérico.");
                    Importe.Focus();
                    return;
                }
            if (txtNoReservacion.Text.Length != 6 && Globales.isAgencias && !TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
            {
                Globales.MessageBoxMit("La reservación debe tener 6 caracteres.");
                txtNoReservacion.Focus();
                CmdLeer.IsEnabled = true;
                return;
            }

            boletos = this.CadenaBoletos();

            if (string.IsNullOrWhiteSpace(Fecha.Text) && (Globales.isAgencias || Globales.isAerolinea) && !TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
            {
                Globales.MessageBoxMit("Favor de seleccionar la fecha salida.");
                CmdLeer.IsEnabled = true;
                return;
            }

            if (Globales.isAerolinea)
            {
                                  
                    if (txtNoReservacion.Text.Length != 6)
                    {
                        Globales.MessageBoxMit("El PNR debe tener 6 dígitos");
                        CmdLeer.IsEnabled = true;
                        txtNoReservacion.Focus();
                        return;
                    }
                
            }
            else
            {
                if (string.IsNullOrWhiteSpace(boletos))
                {
                    Globales.MessageBoxMit("Favor de introducir un número de boleto.");
                    this.CmdEnviar.IsEnabled = true;
                    return;
                }
            }


            if (Globales.isAerolinea)
            {
                if (!string.IsNullOrWhiteSpace(boletos))
                {
                    if (!Globales.insertaBoletoAgencia(TypeUsuario.Id_Company, "", boletos, Fecha.Text, FechaR.Text))
                    {
                        CmdLeer.IsEnabled = true;
                        return;
                    }
                }
            }
            else
                if (boletos != "")
                {
                    if (!Globales.insertaBoletoAgencia(TypeUsuario.Id_Company, "", boletos, Fecha.Text, ""))
                    {
                        CmdLeer.IsEnabled = true;
                        return;
                    }
                }


            // CmdBoletosA.IsEnabled = false;
            this.BADDBOLETOS.IsEnabled = false;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);//'se prepara url
            CmdLeer.IsEnabled = false;
            CmdLeer.IsEnabled = false;
            CmdLeer.Visibility = Visibility.Hidden;
            CmdLeer.IsEnabled = false;
            Importe.IsEnabled = false;
            txtNoReservacion.IsEnabled = false;
            StatusCmd = false;
            Fecha.IsEnabled = false;
            FechaR.IsEnabled = false;
            Globales.cpIntegraEMV.dbgStartTxEMV(Importe.Text); //' se prepara lector
            Mouse.OverrideCursor = null;
      
            if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()) && !string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkCc_Number()))
            {
                NumTdc.Text = Globales.cpIntegraEMV.chkCc_Number();
                NomTdc.Text = Globales.cpIntegraEMV.chkCc_Name();
                this._mes = Globales.cpIntegraEMV.chkCc_ExpMonth();
                this._anio = Globales.cpIntegraEMV.chkCc_ExpYear();

                TFECHAVENC.Text = string.Format("{0}/{1}", this._mes, this._anio);

                if (Globales.isAgencias)
                {
                    string numEmpresa = string.Empty;
                    //  numEmpresa = Format(Right(MDImit.ActiveForm.cboEmpresa.Text, 4), "0000")
                    Globales.cpIntegraEMV.dbgSetTipoRP3(true, Globales.mynumero);
                }
                // 'Se agrega para el tipo de afiliación
                string stOpAfi = string.Empty;
                if (Globales.isAgencias)
                {
                    stOpAfi = "8";
                    Globales.merchantBanda = Globales.cpIntegraEMV.dbgGetMerchantBandaRP3(Globales.mynumero);
                }
                else
                {
                    stOpAfi = "9";
                    Globales.merchantBanda = Globales.cpIntegraEMV.dbgGetMerchantBanda(stOpAfi);
                }

                if (Globales.merchantBanda != "00000")
                {
                    Globales.isAmex = Globales.cpIntegraEMV.dbgGetisAmex();
                    FormaPago_Click();
                    CmdEnviar.IsEnabled = true;
                    CmdEnviar.Visibility = Visibility.Visible;
                    if (!Globales.cpIntegraEMV.dbgTxMonitor())
                    {
                        Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspDsError());
                        CmdLeer.Visibility = Visibility.Visible;
                        CmdLeer.IsEnabled = true;
                        CmdEnviar.IsEnabled = false;
                        CmdEnviar.Visibility = Visibility.Hidden;
                        FormaPago.Visibility = Visibility.Hidden;
                        //Label6.Visibility = Visibility.Hidden;
                        NumTdc.Text = "";
                        NomTdc.Text = "";

                        this._mes = string.Empty;
                        this._anio = string.Empty;
                        this.TFECHAVENC.Text = string.Empty;
                        this.BADDBOLETOS.IsEnabled = true;

                        StatusCmd = true;
                    }
                }
                else
                {
                    Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspDsError());                    
                }
            }
            else
            {
                BADDBOLETOS.IsEnabled = true;
                if (!string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()))
                {
                    CmdLeer.IsEnabled = true;
                    CmdLeer.Visibility = Visibility.Visible;
                    CmdLeer.IsEnabled = true;
                    Importe.IsEnabled = true;
                    txtNoReservacion.IsEnabled = true;
                    StatusCmd = true;
                    Fecha.IsEnabled = true;
                    FechaR.IsEnabled = true;
                    Globales.MessageBoxMitError(Globales.cpIntegraEMV.chkPp_DsError());
                    CmdLeer.IsEnabled = true;
                }
                else
                {
                    Globales.MessageBoxMitError("Error en el lector");
                    CmdNuevo_Click(null, null);
                }
            }
        }
        private void FormaPago_Click()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".FormaPago_Click";

            if (Globales.GetDataXml("MXN", TypeUsuario.CadenaXML).Contains(Globales.merchantBanda))
            {
                lblMoneda.Content = "MXN";
            }
            else
            {
                lblMoneda.Content = "USD";
            }
        }
        public void CargaBoletosReservados(string cadena)
        {
            string[] arregloboletos;
            arregloboletos = cadena.Split(',');
            int contador = 1;
            System.Windows.Controls.UIElementCollection cajas = BoletosExtra.Children;
            try
            {
                foreach (Object item in cajas)
                {
                    if (item.GetType().ToString().Equals("System.Windows.Controls.TextBox"))
                    {
                        System.Windows.Controls.TextBox texto = (System.Windows.Controls.TextBox)item;
                        if (arregloboletos.Length > contador)
                        {

                            texto.Text = arregloboletos[contador];
                        }
                        contador++;

                    }
                }
            }
            catch { }
        }
        // '***********************************************************************************
        //'***********************************************************************************
        //'**                          CmdEnviar_Click()                                    **
        //'**                                                                               **
        //'**  Descripción   : Manda la transacción al servidor con ciertos parametros      **
        //'**                  dependiendo si se trata de una transacción c/s EMV           **
        //'**                  si es el caso de una con EMV manda al sub realizaOP y sino   **
        //'**                  manda hace la validaciones correspondientes y manda al       **
        //'**                  sub VentaBanda.                                              **
        //'**                                                                               **
        //'***********************************************************************************
        //'***********************************************************************************
        private void CmdEnviar_Click(object sender, RoutedEventArgs e)
        {

            int intDigVer;
            bool bolBoletoLibre;
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdEnviar_Click";

            CmdEnviar.IsEnabled = false;
            string strTypeC = string.Empty;
            string strCadEncriptar = string.Empty;
            string Voucher = string.Empty;
            string strTpOperacion = string.Empty;


            if (Globales.isAmex)
            {
                strTypeC = "AMEX";
            }
            else
            {
                strTypeC = "V/MC";
            }
            if (Globales.isAgencias)
            {
                strTpOperacion = "8";
            }
            else
            {
                strTpOperacion = "9";
            }

            // 'Se agrega validación para no permitir enviar la transacción sin contar con merchant
            if (string.IsNullOrWhiteSpace(Globales.merchantBanda))
            {
                Globales.MessageBoxMit("No hay planes de pago para esta tarjeta, por favor cambie la tarjeta." );
                CmdLeer.Visibility = Visibility.Visible;
                CmdLeer.IsEnabled = true; ;
                CmdEnviar.IsEnabled = false;
                CmdEnviar.Visibility = Visibility.Hidden;
                CmdNuevo.Visibility = Visibility.Hidden;
                CmdNuevo.IsEnabled = false;
                FormaPago.IsEnabled = true; ;
                Importe.IsEnabled = true; ;
                StatusCmd = true;
                this.BADDBOLETOS.IsEnabled = true;
                this._anio = string.Empty;
                this.BADDBOLETOS.IsEnabled = true;
                NumTdc.Text = "";
                TFECHAVENC.Text = string.Empty;
                NomTdc.Text = "";

                  CmdLeer.IsEnabled = true;
                    CmdLeer.Visibility = Visibility.Visible;
                    CmdEnviar.IsEnabled = false;
                    CmdEnviar.Visibility = Visibility.Hidden;
                    CmdLeer.IsEnabled = true;
                    Importe.IsEnabled = true;
                    txtNoReservacion.IsEnabled = true;
                    StatusCmd = true;
                    Fecha.IsEnabled = true;
                    FechaR.IsEnabled = true;
                Globales.cpIntegraEMV.dbgCancelOperation();
                return;
            }

            CmdEnviar.IsEnabled = false;

            string money = lblMoneda.Content.ToString();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globales.cpIntegraEMV.dbgSetCurrencyType(lblMoneda.Content.ToString());
            Globales.csvAmexenBanda = (string.IsNullOrWhiteSpace(Globales.csvAmexenBanda)) ? "" : Globales.csvAmexenBanda;
            if (string.IsNullOrWhiteSpace(boletos) && !Globales.isAerolinea)
            {
                Globales.cpIntegraEMV.sndVtaDirectaEMV(TypeUsuario.usu,
              TypeUsuario.Pass,
              "",
              TypeUsuario.Id_Company,
              TypeUsuario.Id_Branch,
              TypeUsuario.country,
              strTypeC,
              Globales.merchantBanda,
               txtNoReservacion.Text,
              strTpOperacion,
              lblMoneda.Content.ToString(),
              Globales.csvAmexenBanda);
            }
            else
            {
                string FechaRetorno = string.Empty;

                if (Globales.isAerolinea)
                    if (string.IsNullOrWhiteSpace(FechaR.Text))
                    {
                        // FechaRetorno = FechaSalida;
                    }
                    else
                        FechaRetorno = FechaR.Text;

                else
                    FechaRetorno = "";

                Globales.cpIntegraEMV.sndVtaBoletosEMV(TypeUsuario.usu,
              TypeUsuario.Pass,
              "",
              TypeUsuario.Id_Company,
              TypeUsuario.Id_Branch,
              TypeUsuario.country,
              strTypeC,
              Globales.merchantBanda,
               txtNoReservacion.Text,
              strTpOperacion,
              lblMoneda.Content.ToString(),
              boletos,
              Fecha.Text,
              FechaRetorno,
              Globales.csvAmexenBanda);

            }

            Mouse.OverrideCursor = null;

            Globales.csvAmexenBanda = string.Empty;

            if ((Globales.isAgencias || Globales.isAerolinea) && !Globales.isVentasPropias)
            {
                ejecutaCobro();
            }
            if (Globales.GetDataXml("cd_response", Globales.cpIntegraEMV.getRspXML()) == Globales.COD_VF)
            { //'if ( para decidir si hace la venta forzada
                string noaut;
                noaut = Globales.cpIntegraEMV.getRspAuth();
                if (Globales.MessageConfirm(Globales.GetDataXml("msgautvf", TypeUsuario.CadenaXML)))
                {
                    string strNumAut = string.Empty;
                    string strNumTarjeta = string.Empty;
                    string strNumCvv = string.Empty;
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
                    if (!string.IsNullOrWhiteSpace(strNumAut) && !string.IsNullOrWhiteSpace(strNumTarjeta) && !string.IsNullOrWhiteSpace(strNumCvv))
                    {
                        string strFolioC;
                        strFolioC = Globales.GetDataXml("foliocpagos", Globales.cpIntegraEMV.getRspXML());

                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);

                        Globales.cpIntegraEMV.sndVtaFzadaMOTO(TypeUsuario.usu,
                      TypeUsuario.Pass,
                      "",
                      TypeUsuario.Id_Company,
                      TypeUsuario.Id_Branch,
                      TypeUsuario.country,
                      Globales.merchantBanda,
                      txtNoReservacion.Text,
                      strTpOperacion,
                      Importe.Text,
                      lblMoneda.Content.ToString(),
                      strNumAut,
                      strTypeC,
                      NomTdc.Text,
                      strNumTarjeta,
                      TFECHAVENC.Text.Substring(0, 2),
                            //        "  Utils.Mid(Anio.Text, 1, 2)",
                      TFECHAVENC.Text.Substring(3, 2),
                      strNumCvv);
                        Mouse.OverrideCursor = null;
                        switch (Globales.cpIntegraEMV.getRspDsResponse())
                        {
                            case "approved":
                                cmdCambiar.Visibility = Visibility.Hidden;
                                FormaPago.IsEnabled = false;
                                txtNoReservacion.IsEnabled = false;
                                Importe.IsEnabled = false;
                                mensaje = Globales.cpIntegraEMV.getRspAuth();
                                Globales.MessageBoxMitApproved(mensaje);
                                cmdVoucher.IsEnabled = true; ;
                                CmdNuevo.IsEnabled = true; ;
                                CmdEnviar.Visibility = Visibility.Hidden;
                                CmdNuevo.Visibility = Visibility.Visible;
                                CmdLeer.Visibility = Visibility.Hidden;
                                Fecha.IsEnabled = false;
                                FechaR.IsEnabled = false;

                                this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;
                                this.BENVIAMAIL.Tag = this.NomTdc.Text;

                                Globales.PrintOptions(Globales.cpIntegraEMV.getRspVoucher(), Globales.cpIntegraEMV.getRspOperationNumber());

                                if (this.lista_boletos.Count > 1)
                                {
                                    AsociaBoletos();
                                }
                                else
                                {
                                    this.LTOTALBOLETOS.Content = "0";
                                    this.LADDBOLETOS.Content = string.Format("Agregar boletos({0})", this.lista_boletos.Count);
                                }

                                if (Globales.FacturaE == "1")
                                {
                                    if (Globales.MessageConfirm("¿Desea factura electrónica?"))
                                    {
                                        frmPreguntaFactura pf = new frmPreguntaFactura();
                                        pf.abrirFrmAhora = abrir;
                                        pf.cerraPage = cerrar;
                                        abrir(pf);

                                        return;
                                    }
                                    else
                                    {
                                        Globales.XMLFacturaE = "";
                                    }
                                }
                                break;
                            case "denied":
                                cmdCambiar.Visibility = Visibility.Hidden;
                                //cboBanco.IsEnabled = false;
                                FormaPago.IsEnabled = false;
                                txtNoReservacion.IsEnabled = false;
                                Importe.IsEnabled = false;
                                mensaje = Globales.msjRech + " " + Globales.cpIntegraEMV.getRspCdResponse() + " " + Globales.cpIntegraEMV.getRspFriendlyResponse();
                                Globales.MessageBoxMitDenied(mensaje);
                                CmdNuevo.Visibility = Visibility.Visible;
                                CmdEnviar.Visibility = Visibility.Hidden;
                                cmdVoucher.IsEnabled = false;

                                this.BENVIAMAIL.Visibility = Visibility.Hidden;
                                this.BENVIAMAIL.Tag = string.Empty;

                                break;
                            case "error":
                                mensaje = Globales.GetDataXml("nb_error", Globales.cpIntegraEMV.getRspXML());
                                Globales.MessageBoxMitError(mensaje);
                                CmdNuevo.Visibility = Visibility.Visible;
                                CmdNuevo.IsEnabled = true;
                                CmdEnviar.Visibility = Visibility.Hidden;
                                cmdVoucher.IsEnabled = false;
                                LimpiarBPin();
                                CmdNuevo.IsEnabled = true;
                                BADDBOLETOS.IsEnabled = true;
                                Fecha.IsEnabled = true;
                                FechaR.IsEnabled = true;

                                this.BENVIAMAIL.Visibility = Visibility.Hidden;
                                this.BENVIAMAIL.Tag = string.Empty;

                                break;
                            default:
                                cmdCambiar.Visibility = Visibility.Hidden;
                                mensaje = "Verifique su conexión de Internet.";
                                Globales.MessageBoxMitError(mensaje);
                                CmdNuevo.Visibility = Visibility.Visible;
                                CmdEnviar.Visibility = Visibility.Hidden;
                                cmdVoucher.IsEnabled = false;
                                LimpiarBPin();
                                CmdNuevo.IsEnabled = true;
                                BADDBOLETOS.IsEnabled = true;
                                Fecha.IsEnabled = true;
                                FechaR.IsEnabled = true;

                                this.BENVIAMAIL.Visibility = Visibility.Hidden;
                                this.BENVIAMAIL.Tag = string.Empty;
                                break;
                        }
                    } //'fin de if ( de captura de numero de autorizacion
                } //'Fin de if ( de pregunta

            }
            else
            {


                if (!YaCobro)
                {
                    this.ejecutaCobro();
                }

                // 'Aqui entra cuando todo sale bien y no es necesario realizar una venta forzada.
            } //'Fin de if ( para decidir si hace la venta forzada
            //'FIN LA IMPLEMENTACION EMV FULL **********************************************
            //'Globales.cpIntegraEMV.dbgEndOperation 'fin de la operacion
            //'CmdBoletosA.IsEnabled = true;;
            //GloboPcPay  LblTError,  LblAprob,  LblDenied,  LblAuth




        }
        private void ejecutaCobro()
        {
            YaCobro = true;
            switch (Globales.cpIntegraEMV.getRspDsResponse())
            {
                case "approved":
                    string mensaje = Globales.cpIntegraEMV.getRspAuth();
                    Globales.MessageBoxMitApproved(mensaje);
                    cmdCambiar.Visibility = Visibility.Hidden;
                    FormaPago.IsEnabled = false;
                    txtNoReservacion.IsEnabled = false;
                    Importe.IsEnabled = false;
                    cmdVoucher.IsEnabled = true; ;
                    CmdNuevo.IsEnabled = true; ;
                    CmdEnviar.Visibility = Visibility.Hidden;
                    CmdNuevo.Visibility = Visibility.Visible;
                    CmdLeer.Visibility = Visibility.Hidden;

                    this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;
                    this.BENVIAMAIL.Tag = this.NomTdc.Text;

                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    Globales.PrintOptions(Globales.cpIntegraEMV.getRspVoucher(), Globales.cpIntegraEMV.getRspOperationNumber());
                    Mouse.OverrideCursor = null;


                    if (Globales.FacturaE == "1")
                    {
                        if (Globales.MessageConfirm("¿Desea factura electrónica?"))
                        {
                            frmPreguntaFactura pf = new frmPreguntaFactura();
                            pf.abrirFrmAhora = abrir;
                            pf.cerraPage = cerrar;
                            abrir(pf);
                            return;
                        }
                        else
                        {
                            Globales.XMLFacturaE = "";
                        }
                    }
                    break;
                case "denied":
                    string mensajeD = Globales.msjRech + Globales.cpIntegraEMV.getRspCdResponse() + " " + Globales.cpIntegraEMV.getRspFriendlyResponse();
                    Globales.MessageBoxMitDenied(mensajeD);
                    cmdCambiar.Visibility = Visibility.Hidden;
                    FormaPago.IsEnabled = false;
                    txtNoReservacion.IsEnabled = false;
                    Importe.IsEnabled = false;
                    CmdNuevo.Visibility = Visibility.Hidden;
                    CmdNuevo.Visibility = Visibility.Visible;
                    CmdEnviar.Visibility = Visibility.Hidden;

                    this.BENVIAMAIL.Visibility = Visibility.Hidden;
                    this.BENVIAMAIL.Tag = string.Empty;

                    break;
                case "error":
                    string mensajeE = Globales.cpIntegraEMV.getRspDsError();
                    Globales.MessageBoxMitError(mensajeE);
                    cmdCambiar.Visibility = Visibility.Hidden;
                    CmdEnviar.IsEnabled = false;
                    CmdEnviar.Visibility = Visibility.Hidden;
                    CmdLeer.Visibility = Visibility.Hidden;
                    CmdLeer.IsEnabled = false;
                    FormaPago.IsEnabled = false;
                    Importe.IsEnabled = false;
                    txtNoReservacion.IsEnabled = false;
                    CmdNuevo.Visibility = Visibility.Visible;
                    CmdNuevo.IsEnabled = true;
                    LimpiarBPin();

                    this.BENVIAMAIL.Visibility = Visibility.Hidden;
                    this.BENVIAMAIL.Tag = string.Empty;
                    break;
                default:
                    string mensajeF = "Error de conexión, verifique su reporte";
                    Globales.MessageBoxMitError(mensajeF);
                    CmdEnviar.IsEnabled = false;
                    CmdLeer.Visibility = Visibility.Visible;
                    CmdLeer.IsEnabled = true; ;
                    FormaPago.IsEnabled = true; ;
                    Importe.IsEnabled = true; ;
                    txtNoReservacion.IsEnabled = true; ;
                    cmdCambiar.Visibility = Visibility.Hidden;
                    CmdNuevo.Visibility = Visibility.Hidden;
                    LimpiarBPin();

                    this.BENVIAMAIL.Visibility = Visibility.Hidden;
                    this.BENVIAMAIL.Tag = string.Empty;
                    break;
            }
        }
        private void LimpiarBPin()
        {

            Globales.BandPIN = 0;
            Globales.BandPIN2 = 0;
        }
        private void AsociaBoletos()
        {
            string strCadEncriptar, strCadaux, strBoletos, strTipo = string.Empty;

            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdAceptar3_Click()";
            strBoletos = CadenaBoletos();



            if (string.IsNullOrWhiteSpace(strBoletos))
            {
                return;
            }
            strCadEncriptar = "&transaccion=" + Globales.cpIntegraEMV.getRspOperationNumber() +
                              "&boletos=" + strBoletos +
                              "&op=ins" +
                              "&version=" + TypeUsuario.strVersion;
            //DoEvents
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globales.cpHTTP_Clear();
            Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
            Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);

            if (Globales.cpHTTP_SendcpCUCT())
            {
                strCadaux = Globales.GetDataXml("response", Globales.cpHTTP_sResult).ToLower();

                Mouse.OverrideCursor = null;
                if (!string.IsNullOrWhiteSpace(strCadaux))
                {
                    Globales.MessageBoxMit(Globales.GetDataXml("desc", Globales.cpHTTP_sResult));
                }

                else
                {

                    Globales.MessageBoxMit(Globales.GetDataXml("desc", Globales.cpHTTP_sResult) );
                }
            }


        }
        //'***********************************************************************************
        //'***********************************************************************************
        //'**                           CmdNuevo_Click()                                    **
        //'**                                                                               **
        //'**  Descripción   : Limpia los controles y prepara al sistema con lo necesario   **
        //'**                  para realizar una nueva transacción.                         **
        //'**                                                                               **
        //'***********************************************************************************
        //'***********************************************************************************
        private void CmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            YaCobro = false;
            lblMoneda.Content = "";
            Globales.strNombreFP = NOMBRE_GENERAL + ".CmdNuevo_Click";

            this.lista_boletos.Clear();
            this.LBOLETOS.Items.Clear();
            this.LADDBOLETOS.Content = string.Format("Agregar boletos({0})", this.lista_boletos.Count);
            this.LTOTALBOLETOS.Content = this.lista_boletos.Count;
            this.BADDBOLETOS.IsEnabled = true;

            this._mes = string.Empty;
            this._anio = string.Empty;
            this.TFECHAVENC.Text = string.Empty;


            Globales.csvAmexenBanda = string.Empty;

            Fecha.Text = "";
            FechaR.IsEnabled = true;

            if (Globales.isAerolinea)
            {
                FechaR.Text = "";
            }

            StatusCmd = true;
            CmdLeer.IsEnabled = true;
            Globales.BandPIN = 0;
            Globales.BandPIN2 = 0;
            //cboBanco.Enabled = True
            // FormaPago.Enabled = True
            txtNoReservacion.IsEnabled = true;
            //txtNoBoleto.IsEnabled = true;
            Importe.IsEnabled = true;
            Fecha.IsEnabled = true;

            //Label6.Visibility = Visibility.Hidden;
            FormaPago.Visibility = Visibility.Hidden;
            limpia();
            TypeUsuario.strVoucherCoP = "";
            CmdLeer.Visibility = Visibility.Visible;
            CmdEnviar.Visibility = Visibility.Hidden;

            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;

            //CheckT.IsEnabled = true;
            //CheckT.IsChecked = false;
            //CmdBoletosA.IsEnabled = false;
            //CmdBoletosA.Content = "Boletos adicionales(0)>>";
            cadenadeboletos = string.Empty;
            banboletosrepetidos = false;

            //TxtNoBoleto1.Text = "";

            //EnabledTickets();
        }
        private void limpia()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".Limpia";
            strfolio = "";
            TypeUsuario.strVoucher = "";
            NumTdc.Text = "";
            NomTdc.Text = "";
            txtNoReservacion.Text = "";
            Importe.Text = "";
            cmdVoucher.IsEnabled = false;
            FormaPago.SelectedIndex = -1;
            CmdEnviar.Visibility = Visibility.Visible;
            CmdNuevo.Visibility = Visibility.Hidden;
            cmdCambiar.Visibility = Visibility.Hidden;
            boletos = "";
        }


        public void Cargar()
        {
            Fecha.DisplayDateStart = DateTime.Now;
            FechaR.DisplayDateStart = DateTime.Now;
            FechaR.IsEnabled = false;
            BADDBOLETOS.IsEnabled = true;
            BADDBOLETOS.Visibility = Visibility.Visible;
            lblMoneda.Content = "";
            cadenadeboletos = "";
            banboletosrepetidos = false;
            UserIata.Content ="IATA: "+ TypeUsuario.iata;
            subEtiqueta.Content = "";
            
            Globales.strNombreFP = NOMBRE_GENERAL + ".Form_Load";
            Fecha.Text = "";
            StatusCmd = true;
            LimpiarBPin();
            if (Globales.isAgencias)
            {
                if (!string.IsNullOrWhiteSpace(Globales.cpHTTP_sResult) && Globales.cpHTTP_sResult.Contains("nb_reservacion"))
                {
                    ObtieneEmpresasAgenciaPorNumero(Globales.GetDataXml("catempresas", TypeUsuario.CadenaXML), "B", Globales.GetDataXml("cd_empresa_originante", Globales.cpHTTP_sResult));
                }
                else
                {
                    ObtieneEmpresasAgencia(Globales.GetDataXml("catempresas", TypeUsuario.CadenaXML), "B");
                }
                cboEmpresa.SelectedIndex = 0;
                txtNoReservacion.MaxLength = 6;
                if (!TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
                {
                    BADDBOLETOS.IsEnabled = true;
                    BADDBOLETOS.Visibility = Visibility.Visible;
                }
                else
                {
                    txtNoReservacion.MaxLength = 40;
                }
            }
            if (!Globales.isAgencias || TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
            {
                if (Globales.isAerolinea)
                {
                    UserIata.Content ="IATA: "+ TypeUsuario.iata;
                    FECHARETORNO.Visibility = Visibility.Visible;
                    Label13.Visibility = Visibility.Visible;
                    FechaR.Visibility = Visibility.Visible;
                    FechaR.Text = "";
                    Label7.Content = "PNR";
                    txtNoReservacion.MaxLength = 6;
                    cargaDts();// cmdAcepEmp_Click();
                }
                else
                {
                    Fecha.Visibility = Visibility.Hidden;
                    caption = "Venta con presencia de tarjeta.";
                }
            }
            limpia();
            CmdLeer.Visibility = Visibility.Visible;
            CmdEnviar.Visibility = Visibility.Hidden;
            if (Globales.isAerolinea)
            {
                Label7.Content = "PNR";
            }
            else
            {
                Label7.Content = TypeUsuario.reference;
            }

            if (!string.IsNullOrWhiteSpace(Globales.cpHTTP_sResult) && Globales.cpHTTP_sResult.Contains("nb_reservacion"))
            {
                cargaDts();
            }
        }
        private void ObtieneEmpresasAgencia(string strBancos, string tipo)
        {
            string[] empresas = strBancos.Split('|');

            for (int i = 0; i < empresas.Length; i++)
            {
                if (empresas[i].Contains("," + tipo + ","))
                {
                    auxiliar += empresas[i] + "|";
                    string[] aux = empresas[i].Split(',');

                    cboEmpresa.Items.Add(aux[1] + " - " + aux[0]);
                }
            }


        }
        private void cmdVoucher_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            switch (TypeUsuario.TipoImpresora)
            {

                case "1":

                    Globales.PrintOptions(Globales.cpIntegraEMV.getRspVoucher(), Globales.cpIntegraEMV.getRspOperationNumber());
                    cmdVoucher.IsEnabled = true;
                    CmdNuevo.IsEnabled = true;

                    break;
                case "3":
                    Globales.imprimirEpson();
                    break;
                case "4":
                    Globales.PrintOptions(Globales.cpIntegraEMV.getRspVoucher(), Globales.cpIntegraEMV.getRspOperationNumber());
                    cmdVoucher.IsEnabled = true;
                    CmdNuevo.IsEnabled = true;

                    break;

                case "6":
                        CmdNuevo.IsEnabled = false;
                        cmdVoucher.IsEnabled = false;
                        Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                        cmdVoucher.IsEnabled = true;
                        CmdNuevo.IsEnabled = true;
                    break;

                default:
                        Globales.MessageBoxMit("No ha definido un tipo de impresora.");
                    break;
            }
            Mouse.OverrideCursor = null;
        }
        private void cmdAcepEmp_Click(object sender, RoutedEventArgs e)
        {
            if (Globales.isAerolinea)
            {
                cboEmpresa.Text = TypeUsuario.nb_company + "-" + TypeUsuario.Id_Company;
            }



            if (cboEmpresa.Text.Contains("-") || cboEmpresa.SelectedIndex != -1)
            {

                Globales.mynumero = Globales.Right(cboEmpresa.Text, 4);

                // strNombreFP = NOMBRE_GENERAL & ".cboBanco_Click()"
                if (!string.IsNullOrWhiteSpace(TypeUsuario.iata) && Globales.isAerolinea)
                {
                    //lbl3Digitos.Content = TypeUsuario.iata;
                    //lbl3Digitos.Left = txtNoBoleto.Left - 450
                }
                else
                {
                    TypeUsuario.iata = Globales.GetDataXml("iatarp3", Globales.GetDataXml("ventasrp3" + Globales.mynumero, TypeUsuario.CadenaXML));// 'Format(cboEmpresa.ItemData(cboEmpresa.ListIndex), "0000"), TypeUsuario.CadenaXML))
                    UserIata.Content = "IATA: " + TypeUsuario.iata;
                    //lbl3Digitos.Content = TypeUsuario.iata;
                    // lbl3Digitos.Left = txtNoBoleto.Left - 450
                }


                subEtiqueta.Content = cboEmpresa.Text;


                fraCubreTodo.Visibility = Visibility.Hidden;
                frmDatos.Visibility = Visibility.Visible;
                // frmBanda.Height = 5820 '5500 '4950
                if (!Globales.isAerolinea)
                {
                    caption = "Cargo con presencia de tarjeta. " + cboEmpresa.Text;
                }
                else
                {
                    caption = "VENTA DE BOLETOS: Cargo con presencia de tarjeta";
                }
                // Skin Me
                if (TypeUsuario.ShowMsg)
                {
                    frmAvisoBanda avisoBanda = new frmAvisoBanda();
                    avisoBanda.ShowDialog();

                }
                string[] arregloboletos;
                string boleto;
                if (!string.IsNullOrWhiteSpace(Globales.cpHTTP_sResult) && Globales.cpHTTP_sResult.Contains("nb_reservacion"))
                {
                    txtNoReservacion.Text = Globales.GetDataXml("nb_reservacion", Globales.cpHTTP_sResult);
                    //txtNoBoleto.Text = Utils.Left(Globales.GetDataXml("tx_boleto", Globales.cpHTTP_sResult), 11);
                    Importe.Text = Globales.GetDataXml("nu_importe", Globales.cpHTTP_sResult);

                    Fecha.Text = Globales.GetDataXml("fh_salida", Globales.cpHTTP_sResult);
                    txtNoReservacion.IsEnabled = false;
                    //txtNoBoleto.IsEnabled = false;
                    Importe.IsEnabled = false;
                    // FechaSalida.Enabled = False
                    Fecha.IsEnabled = false;
                    CmdLeer.IsEnabled = true;


                    if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("tx_boleto", Globales.cpHTTP_sResult)))
                    {
                        arregloboletos = Globales.GetDataXml("tx_boleto", Globales.cpHTTP_sResult).Split(',');
                        CargaBoletosReservados(Globales.GetDataXml("tx_boleto", Globales.cpHTTP_sResult));
                        //CheckT.IsEnabled = false;
                        //CheckT.IsChecked = true;
                        //CmdBoletosA.IsEnabled = true; ;

                        //CmdBoletosA.Content = "Boletos adicionales(" + (arregloboletos.Length - 1) + ")";
                        //DisableTickets();
                    }

                    if (Globales.isAerolinea)
                    {

                        FechaR.Text = Globales.GetDataXml("fh_retorno", Globales.cpHTTP_sResult);
                        // FechaRetorno.Enabled = False
                        // FechaR.Enabled = False
                    }

                }
            }
            else
            {

                Globales.MessageBoxMit("Seleccione una empresa valida" );
            }
        }
        private void Numeros(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }
        private void Importe_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
        }
        private void cargaDts()
        {
            if (TypeUsuario.ShowMsg)
            {
                frmAvisoBanda frmAvisoBanda = new frmAvisoBanda();
                frmAvisoBanda.Show();

            }


            if (Globales.isAerolinea)
            {
                cboEmpresa.Text = TypeUsuario.nb_company + "-" + TypeUsuario.Id_Company;
                Globales.mynumero = TypeUsuario.Id_Company;

            }

            

                Globales.mynumero = clave;
            






            if (!string.IsNullOrWhiteSpace(TypeUsuario.iata) && Globales.isAerolinea)
            {
                //lbl3Digitos.Content = TypeUsuario.iata;

            }
            else
            {
                TypeUsuario.iata = Globales.GetDataXml("iatarp3", Globales.GetDataXml("ventasrp3" + Globales.mynumero, TypeUsuario.CadenaXML)); //'Format(cboEmpresa.ItemData(cboEmpresa.ListIndex), "0000"), TypeUsuario.CadenaXML))
                //lbl3Digitos.Content = TypeUsuario.iata;

            }

            fraCubreTodo.Visibility = Visibility.Hidden;
            frmDatos.Visibility = Visibility.Visible;

            string[] arregloboletos;
            string boleto;
            if (!string.IsNullOrWhiteSpace(Globales.cpHTTP_sResult) && Globales.cpHTTP_sResult.Contains("nb_reservacion"))
            {
                txtNoReservacion.Text = Globales.GetDataXml("nb_reservacion", Globales.cpHTTP_sResult);
                //txtNoBoleto.Text = Utils.Left(Globales.GetDataXml("tx_boleto", Globales.cpHTTP_sResult), 11);
                Importe.Text = Globales.GetDataXml("nu_importe", Globales.cpHTTP_sResult);

                Fecha.Text = Globales.GetDataXml("fh_salida", Globales.cpHTTP_sResult);
                txtNoReservacion.IsEnabled = false;
                //txtNoBoleto.IsEnabled = false;
                Importe.IsEnabled = false;
                // FechaSalida.Enabled = False
                Fecha.IsEnabled = false;
                CmdLeer.IsEnabled = true;


                if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("tx_boleto", Globales.cpHTTP_sResult)))
                {
                    arregloboletos = Globales.GetDataXml("tx_boleto", Globales.cpHTTP_sResult).Split(',');
                    CargaBoletosReservados(Globales.GetDataXml("tx_boleto", Globales.cpHTTP_sResult));
                    if (this.lista_boletos == null)
                        this.lista_boletos = new List<string>();


                    this.lista_boletos.AddRange(arregloboletos.ToList());
                    this.lista_boletos.ForEach(delegate(string elm)
                    {
                        this.LBOLETOS.Items.Add(elm);
                    });
                    this.LTOTALBOLETOS.Content = this.lista_boletos.Count;
                    this.LADDBOLETOS.Content = string.Format("Agregar boletos({0})", this.lista_boletos.Count);

                    //CheckT.IsEnabled = false;
                    //CheckT.IsChecked = true;
                    //CmdBoletosA.IsEnabled = true; ;

                    //CmdBoletosA.Content = "Boletos adicionales(" + (arregloboletos.Length - 1) + ")";
                    //DisableTickets();
                }

                if (Globales.isAerolinea)
                {

                    FechaR.Text = Globales.GetDataXml("fh_retorno", Globales.cpHTTP_sResult);
                    // FechaRetorno.Enabled = False
                    // FechaR.Enabled = False
                }

            }


        }
        private void ObtieneEmpresasAgenciaPorNumero(string strBancos, string tipo, string emp)
        {
            //throw new NotImplementedException();
            string[] empresas = strBancos.Split('|');

            for (int i = 0; i < empresas.Length; i++)
            {
                if (empresas[i].Contains("," + tipo) && empresas[i].Contains(emp + ","))
                {
                    auxiliar += empresas[i] + "|";
                    string[] aux = empresas[i].Split(',');
                    clave = aux[0];

                    cboEmpresa.Items.Add(aux[1] + " - " + aux[0]);
                }
            }
        }
        private void Command2_Click(object sender, RoutedEventArgs e)
        {
            this.TNUMBOLETO.Text = string.Empty;

            BoletosExtra.Visibility = Visibility.Hidden;
            frmDatos.Visibility = Visibility.Visible;
            this.LTOTALBOLETOS.Content = this.lista_boletos.Count;
            this.LADDBOLETOS.Content = string.Format("Agregar boletos({0})", this.lista_boletos.Count);
        }



        private void CmdBoletosA_Click(object sender, RoutedEventArgs e)
        {
            BoletosExtra.Visibility = Visibility.Visible;
            frmDatos.Visibility = Visibility.Hidden;
        }
        private void Fecha_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Fecha.Text))
            {
                string[] fecha = Fecha.Text.Split('/');
                int anio = Convert.ToInt32(fecha[2]);
                int mes = Convert.ToInt32(fecha[1]); ;
                int dia = Convert.ToInt32(fecha[0]); ;
                FechaR.DisplayDateStart = new DateTime(anio, mes, dia);

            }
        }


        private void txtNoReservacion_GotFocus(object sender, RoutedEventArgs e)
        {
        }



        private void txtNoBoleto_GotFocus(object sender, RoutedEventArgs e)
        {
        }
        private void Importe_GotFocus(object sender, RoutedEventArgs e)
        {
//            control.Focus();
        }
        private void NumerosPunto(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }
        public bool NoBoletosExtra { get; set; }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }
        #region"Agregar boletos"
        private void BADDBOLETOS_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.BoletosExtra.Visibility = Visibility.Visible;
            frmDatos.Visibility = Visibility.Hidden;

            if (this.lista_boletos == null)
                this.lista_boletos = new List<string>();
        }
        private void BAGREGABOLETO_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TNUMBOLETO.Text))
            {
                Globales.MessageBoxMit("Ingrese un número de boleto.");
                TNUMBOLETO.Focus();
                return;
            }
            if (Globales.verificadorBoleto(TNUMBOLETO.Text))
            {
                TNUMBOLETO.Focus();
                return;
            }

            string _num_boleto = string.Format("{0}{1}", TypeUsuario.iata, this.TNUMBOLETO.Text);
            if (!Globales.isAerolinea)
            {
                int intDigVer, intDigVer2 = 0;
                if (Globales.IsNumeric(this.TNUMBOLETO.Text))
                {
                    intDigVer = Globales.DigitoVerificador(TypeUsuario.iata, this.TNUMBOLETO.Text);
                    intDigVer2 = Globales.DigitoVerificador("", this.TNUMBOLETO.Text);
                    if (intDigVer == -1 && intDigVer2 == -1)
                    {
                        Globales.MessageBoxMit("Verifique el No. de boleto.");
                        TNUMBOLETO.Focus();
                        return;
                    }
                }
            }

            if (this.lista_boletos.Count < 10)
            {
                if (!this.lista_boletos.Exists(o => o == _num_boleto))
                    this.lista_boletos.Add(_num_boleto);
                else
                {
                    Globales.MessageBoxMit("El boleto ya existe en la lista.");
                    TNUMBOLETO.Focus();
                    return;
                }
                this.LBOLETOS.Items.Clear();

                this.lista_boletos.ForEach(delegate(string elm)
                {
                    this.LBOLETOS.Items.Add(elm);
                });

                this.TNUMBOLETO.Text = string.Empty;
                this.LTOTALBOLETOS.Content = this.lista_boletos.Count;
            }
            else
            {
                Globales.MessageBoxMit("El límite de boletos permitidos son 10.");
                TNUMBOLETO.Focus();
                this.TNUMBOLETO.Text = string.Empty;
            }
        }
        private void BELIMINARBOLETO_Click(object sender, RoutedEventArgs e)
        {
            if (this.LBOLETOS.SelectedItem != null)
            {
                var valor = this.LBOLETOS.SelectedItem.ToString();
                this.lista_boletos.Remove(valor);
                this.LBOLETOS.Items.Clear();
                this.LBOLETOS.Items.Refresh();

                this.lista_boletos.ForEach(delegate(string elm)
                {
                    this.LBOLETOS.Items.Add(elm);
                });
                this.LTOTALBOLETOS.Content = this.lista_boletos.Count;

            }
        }
        private void TNUMBOLETO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                BAGREGABOLETO_Click(sender, e);
        }
        #endregion
        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir(true);
        }
        private void EVENTO(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }
        public string mensaje { get; set; }
        public TextBox control { get; set; }
        private void noReservacionLostFocus(object sender, RoutedEventArgs e)
        {
          if(!Globales.noMostrarMensajes){
              if (Globales.isAerolinea && txtNoReservacion.Text.Length != 6)
              {
                  if (!string.IsNullOrWhiteSpace(txtNoReservacion.Text))
                  {
                      Globales.MessageBoxMit("El PNR debe tener 6 dígitos");
                      TextBox aux = sender as TextBox;
                      control = aux;
                  }
              }
          }
        }
        private void Fecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FechaR.IsEnabled = true;
                FechaR.Text = "";
                string[] fechita = Fecha.Text.Split('/');
                DateTime fechaAux = new DateTime(Convert.ToInt32(fechita[2]), Convert.ToInt32(fechita[1]), Convert.ToInt32(fechita[0]));
                FechaR.DisplayDateStart = fechaAux;
            }
            catch { 
            
            }
        }

        private void cargando(object sender, RoutedEventArgs e)
        {
            FECHARETORNO.Visibility = Visibility.Hidden;
            Label13.Visibility = Visibility.Hidden;
            TimeSpan t = new TimeSpan(0,0,0,0,5);
            tiempo.Interval = t;
            tiempo.Tick += (a, b) =>
            {
                if(control != null){
                    control.Focus();
                    control = null;
                }
            };
            tiempo.Start();
            if (!Globales.isAgencias || TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
            {
                if (Globales.isAerolinea)
                {
                    FECHARETORNO.Visibility = Visibility.Visible;
                    Label13.Visibility = Visibility.Visible;

                }
            }
            UserIata.Content = "IATA: " + TypeUsuario.iata;
        }

        private void foco1(object sender, RoutedEventArgs e)
        {
            Globales.setFocusMit2(sender,e);
        }

        private void perferfoco(object sender, RoutedEventArgs e)
        {
            Globales.lostFocusMit2(sender,e);
        }
    }
}


