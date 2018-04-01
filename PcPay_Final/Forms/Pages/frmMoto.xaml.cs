using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using PcPay.Forms.Formularios;
using PcPay.Forms.Formularios.MessagesW;
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

    public partial class frmMoto : Page
    {
        public cerrarVentana cerrar;
        
        private const string NOMBRE_GENERAL = "frmMoto";
        private bool banboletosrepetidos;

        private string numeroTarjeta;

        public abrirFrm abrir;

        public string caption;
        private bool entroCobro;
        private bool NoBoletosExtra;
        public Window propietario { get; set; }
        public List<string> lista_boletos { get; set; }
        DispatcherTimer tiempo;
        public menu2 reabrir;

        public frmMoto()
        {
            InitializeComponent();
            lista_boletos = new List<string>();
            this.fraCubreTodo.Visibility = Visibility.Visible;
            this.fraDatos.Visibility = Visibility.Hidden;
            this.BoletosExtra.Visibility = Visibility.Hidden;

            this.carga();
            tiempo = new DispatcherTimer();
            #region"**********Efectos**********"
            NumTdc.GotFocus += Globales.setFocusMit2;
            Mes.GotFocus += Globales.setFocusMit2;
            Anio.GotFocus += Globales.setFocusMit2;
            NomTdc.GotFocus += Globales.setFocusMit2;
            NumCvv.GotFocus += Globales.setFocusMit2;
            Importe.GotFocus += Globales.setFocusMit2;
            txtNoReservacion.GotFocus += Globales.setFocusMit2;
            TNUMBOLETO.GotFocus += Globales.setFocusMit2;
            txtAut.GotFocus += Globales.setFocusMit2;

            NumTdc.LostFocus += Globales.lostFocusMit2;
            Mes.LostFocus += Globales.lostFocusMit2;
            Anio.LostFocus += Globales.lostFocusMit2;
            NomTdc.LostFocus += Globales.lostFocusMit2;
            NumCvv.LostFocus += Globales.lostFocusMit2;
            Importe.LostFocus += Globales.lostFocusMit2;
            txtNoReservacion.LostFocus += Globales.lostFocusMit2;
            TNUMBOLETO.LostFocus += Globales.lostFocusMit2;
            txtAut.LostFocus += Globales.lostFocusMit2;

            #endregion
            NumTdc.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            NumCvv.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            Importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;
            TNUMBOLETO.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;

            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;
            this.BENVIAMAIL.MouseDown += Globales.sendMail_GridMouseDown;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CmdEnviar_Click(object sender, RoutedEventArgs e)
        {

            bool bolOperacion = false;
            string Resu, strCadEncriptar = string.Empty;
            int intDigVer = 0;
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdEnviar_Click()";
            CmdEnviar.IsEnabled = false;
            LblTError.Text = string.Empty;

            string boletos = string.Empty;
            boletos = this.CadenaBoletos();

            if((Globales.isAgencias || !string.IsNullOrWhiteSpace(boletos)) && TypeUsuario.CadenaXML.Contains("<PCPAYRP3")){
               if(string.IsNullOrWhiteSpace(boletos)){
                   Globales.MessageBoxMit("Favor de introducir el número de boleto.");
                   CmdEnviar.IsEnabled = true;
                   return;
               }
            }

            if (string.IsNullOrWhiteSpace(NumTdc.Text))
            {
                Globales.MessageBoxMit("Introduzca el número de tarjeta.");
                NumTdc.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            else if (Mes.SelectedIndex < 0)
            {
                Globales.MessageBoxMit("Seleccione el mes de vencimiento de la tarjeta.");
                Mes.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            else if (Anio.SelectedIndex < 0)
            {
                Globales.MessageBoxMit("Seleccione el año de vencimiento de la tarjeta.");
                Anio.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            else if (NomTdc.Text == "")
            {
                Globales.MessageBoxMit("Introduzca el nombre del titular.");
                NomTdc.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            else if (string.IsNullOrWhiteSpace(NumCvv.Password) && !(Globales.isAgencias || Globales.isAerolinea))
            {
                Globales.MessageBoxMit("Introduzca el Código de seguridad de la tarjeta.");
                NumCvv.Focus();
                CmdEnviar.IsEnabled = true;
                return;
                //   'Se agrega la validación del 000 para trx moto agg
            }
            else if ((NumCvv.Password == "000" || (NumCvv.Password == "0000")) && numCvv2.Visibility == Visibility.Visible)
            {
                //MsgBoxEx "Código de seguridad inválido", , , , vbExclamation, NOMBRE_APP
                Globales.MessageBoxMit("Código de seguridad inválido");
                NumCvv.Focus();
                CmdEnviar.IsEnabled = true;
                return;

                // 'Se agrega validación para no permitir enviar la transacción sin contar con merchant
            }
            else if (string.IsNullOrWhiteSpace(Globales.merchantMoto))
            {
                Globales.MessageBoxMit("No hay planes de pago para esta tarjeta, por Favor cambie la tarjeta.");
                CmdEnviar.IsEnabled = true;
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtNoReservacion.Text))
            {
                if (Globales.isAgencias && TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
                {
                    Globales.MessageBoxMit("Introduzca un dato en el campo " + TypeUsuario.reference);
                }
                else
                {
                    Globales.MessageBoxMit("Introduzca el No. de reservación");
                }
                this.txtNoReservacion.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            else if (string.IsNullOrWhiteSpace(Importe.Text))
            {

                Globales.MessageBoxMit("Introduzca el Importe.");
                Importe.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
             
            if(Convert.ToInt32(string.Format("{0}", Anio.SelectedItem)) < DateTime.Now.Year)
                {

                    Globales.MessageBoxMit("Tarjeta vencida.");
                    CmdEnviar.IsEnabled = true;
                    return;
                }
                else if (Convert.ToInt32(Anio.SelectedItem.ToString()) == DateTime.Now.Year && Convert.ToInt32(Mes.SelectedItem.ToString()) < DateTime.Now.Month)
                {
                    Globales.MessageBoxMit("Tarjeta vencida.");
                    CmdEnviar.IsEnabled = true;
                    return;
                }
            if (Globales.isAmex)
            {
                Globales.CheckOm("4", NumCvv.Password);
                    if (Globales.IsOM)
                    {
                            NumCvv.Password = "0000";
                    }
                if (NumCvv.Password.Length != 4 && !(Globales.isAgencias || Globales.isAerolinea))
                {
                    Globales.MessageBoxMit("Introduzca el Código de seguridad de la tarjeta.");
                    NumCvv.Focus();
                    CmdEnviar.IsEnabled = true;
                    return;
                }
                if (NumCvv.Password.Length != 4 && Globales.isAgencias && !string.IsNullOrWhiteSpace(NumCvv.Password.Trim()))
                {
                    Globales.MessageBoxMit("Introduzca el Código de seguridad de la tarjeta a 4 dígitos.");
                    NumCvv.Focus();
                    CmdEnviar.IsEnabled = true;
                    return;
                }
            }
            else
            {
                Globales.CheckOm("3", NumCvv.Password);
                if(Globales.IsOM){
                        NumCvv.Password = "000";
                }
                if (NumCvv.Password.Length != 3 && !(Globales.isAgencias || Globales.isAerolinea))
                {
                    Globales.MessageBoxMit("Introduzca el código de seguridad a 3 dígitos.");
                    NumCvv.Focus();
                    CmdEnviar.IsEnabled = true;
                    return;
                }
                if (NumCvv.Password.Length != 3 && Globales.isAgencias && !string.IsNullOrWhiteSpace(NumCvv.Password.Trim()))
                {
                    Globales.MessageBoxMit("Introduzca el Cod Seg de la tarjeta a tres dígitos.");
                    NumCvv.Focus();
                    CmdEnviar.IsEnabled = true;
                    return;
                }
            }
            if (!Globales.IsNumeric(Importe.Text))
            {
                Globales.MessageBoxMit("El Importe debe ser numérico.");
                Importe.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            if (txtNoReservacion.Text.Length != 6 && Globales.isAgencias && !TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
            {
                Globales.MessageBoxMit("La reservación debe tener 6 caracteres.");
                txtNoReservacion.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(Fecha.Text.Trim()) && (Globales.isAgencias || Globales.isAerolinea) && !TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
            {
                Globales.MessageBoxMit("Favor de seleccionar la fecha de salida.");
                Fecha.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }
            if (txtNoReservacion.Text.Length < 6 && Globales.isAgencias && !TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
            {
                Globales.MessageBoxMit("La reservación debe contener al menos 6 caracteres..");
                txtNoReservacion.Focus();
                CmdEnviar.IsEnabled = true;
                return;
            }

            if (Globales.isAerolinea)
            {
                if (txtNoReservacion.Text.Length != 6)
                {
                    Globales.MessageBoxMit("El PNR debe tener 6 dígitos");
                    CmdEnviar.IsEnabled = true;
                    txtNoReservacion.Focus();
                    return;
                }
                if (txtAut2.Visibility == Visibility.Visible)
                {
                    if (txtAut.Text.Length != 6)
                    {
                        Globales.MessageBoxMit("El num. de autorización debe tener 6 dígitos");
                        CmdEnviar.IsEnabled = true;
                        txtAut.Focus();
                        return;
                    }
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
            if (FechaVencida())
            {
                // Globales.MessageBoxMit("Tarjeta vencida");
                this.CmdEnviar.IsEnabled = true;
                return;
            }

            if (Globales.isAerolinea)
            {
                if (!string.IsNullOrWhiteSpace(boletos))
                    if (!Globales.insertaBoletoAgencia(TypeUsuario.Id_Company, "", boletos, Fecha.Text, FechaR.Text))
                    {
                        CmdEnviar.IsEnabled = true;
                        return;
                    }
            }
            else
                if (!string.IsNullOrWhiteSpace(boletos))
                    if (!Globales.insertaBoletoAgencia(TypeUsuario.Id_Company, "", boletos, Fecha.Text, ""))
                    {
                        CmdEnviar.IsEnabled = true;
                        return;
                    }

            CmdEnviar.IsEnabled = false;
            bool bolBoletoLibre = false;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;


            string strTypeC = string.Empty;
            Globales.cpIntegracion_Clear();

            if (Globales.isAmex)
                strTypeC = "AMEX";
            else
                strTypeC = "V/MC";

            Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL;

            string strTpOperacion = string.Empty;
            if (Globales.isAgencias)
                strTpOperacion = "15";
            else if (Globales.isAerolinea)
            {
                if (!caption.Contains("operativa"))
                    if (Globales.isVentaForzada)
                        strTpOperacion = "18";
                    else
                        strTpOperacion = "10";
                else
                    strTpOperacion = "14";
            }
            else
                strTpOperacion = "10";

            Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
            if ((Globales.isAgencias || Globales.isAerolinea) && NumCvv.Password == "" && !TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
            {
                if (strTypeC == "V/MC")
                    NumCvv.Password = "000";
                else
                    NumCvv.Password = "0000";
            }
            if (boletos == "" && !Globales.isAerolinea)
            {
                Globales.cpIntegraEMV.sndVtaMOTO(TypeUsuario.usu,
                   TypeUsuario.Pass,
                   "",
                   TypeUsuario.Id_Company,
                   TypeUsuario.Id_Branch,
                   TypeUsuario.country,
                   Globales.merchantMoto,
                   txtNoReservacion.Text,
                   strTpOperacion,
                   Importe.Text,
                   lblMoneda.Content.ToString(),
                   strTypeC,
                   NomTdc.Text,
                   numeroTarjeta,
                   Mes.Text,
                   Anio.Text.Substring(2, 2),
                   NumCvv.Password.Trim());
                Mouse.OverrideCursor = null;
            }
            else
            {
                string FRetorno = string.Empty;
                if (Globales.isAerolinea)
                {
                    if (string.IsNullOrWhiteSpace(FechaR.Text))
                    {
                    }
                    else
                    {
                    }
                }
                else
                {
                    FRetorno = "";
                }
                if (Globales.isVentaForzada)
                {
                    Globales.cpIntegraEMV.sndVtaFzadaMOTO(TypeUsuario.usu,
                       TypeUsuario.Pass,
                       "",
                       TypeUsuario.Id_Company,
                       TypeUsuario.Id_Branch,
                       TypeUsuario.country,
                       Globales.merchantMoto,
                       txtNoReservacion.Text,
                       strTpOperacion,
                       Importe.Text,
                       lblMoneda.Content.ToString(),
                       txtAut.Text.Trim(),
                       strTypeC,
                       NomTdc.Text,
                       numeroTarjeta,
                       Mes.Text,
                       Anio.Text.Substring(2, 2),
                       NumCvv.Password,
                       boletos,
                     Fecha.Text,
                       FRetorno);
                    Mouse.OverrideCursor = null;

                }
                else
                {
                    Globales.cpIntegraEMV.sndVtaBoletosMOTO(TypeUsuario.usu,
                       TypeUsuario.Pass,
                       "",
                       TypeUsuario.Id_Company,
                       TypeUsuario.Id_Branch,
                       TypeUsuario.country,
                       Globales.merchantMoto,
                       txtNoReservacion.Text,
                       strTpOperacion,
                       Importe.Text,
                       lblMoneda.Content.ToString(),
                       strTypeC,
                       NomTdc.Text,
                       numeroTarjeta,
                       Mes.Text,
                       Anio.Text.Substring(2, 2),
                       NumCvv.Password,
                       boletos,
                      Fecha.Text,
                       FechaR.Text);
                    Mouse.OverrideCursor = null;

                }
            }
            Globales.csvAmexenBanda = "";

            //   'Se mete valicación para ya no realizar venta de boletos cu&&o sea llame para aut||izar. 07032013
            if ((Globales.isAgencias || Globales.isAerolinea) && !Globales.isVentasPropias)
            {
                ejecutaCobro();
            }
            if (Globales.GetDataXml("cd_response", Globales.cpIntegraEMV.getRspXML()) == Globales.COD_VF)
            {//if( para decidir si hace la venta f||zada
                string noaut = string.Empty;
                noaut = Globales.cpIntegraEMV.getRspAuth();
                if (Globales.MessageConfirm(Globales.GetDataXml("msgautvf", TypeUsuario.CadenaXML)))
                { //'if( de pregunta
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
                        strFolioC = Globales.GetDataXml("foliocpagos", Globales.cpIntegraEMV.getRspXML());

                        Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        Globales.cpIntegraEMV.sndVtaFzadaMOTO(TypeUsuario.usu,
                        TypeUsuario.Pass,
                        "",
                        TypeUsuario.Id_Company,
                        TypeUsuario.Id_Branch,
                        TypeUsuario.country,
                        Globales.merchantMoto,
                        txtNoReservacion.Text,
                        strTpOperacion,
                        Importe.Text,
                        lblMoneda.Content.ToString(),
                        strNumAut,
                        strTypeC,
                        NomTdc.Text,
                        numeroTarjeta,
                        Mes.Text,
                        Anio.Text.Substring(2, 2),
                        NumCvv.Password);
                        Mouse.OverrideCursor = null;
                        switch (Globales.cpIntegraEMV.getRspDsResponse())
                        {
                            case "approved":

                                FormaPago.IsEnabled = false;
                                txtNoReservacion.IsEnabled = false;
                                Importe.IsEnabled = false;
                                string mensaje = Globales.cpIntegraEMV.getRspAuth();
                                LblAprob.Visibility = Visibility.Visible;
                                LblAuth.Visibility = Visibility.Visible;
                                LblAuth.Content = Globales.cpIntegraEMV.getRspAuth();
                                Globales.MessageBoxMitApproved(mensaje);

                                LblTError.Visibility = Visibility.Hidden;
                                LblDenied.Visibility = Visibility.Hidden;
                                cmdVoucher.IsEnabled = true;
                                CmdNuevo.IsEnabled = true;
                                this.BADDBOLETOS.IsEnabled = false;
                                this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;
                                this.BENVIAMAIL.Tag = this.NomTdc.Text;


                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                Globales.PrintOptions(Globales.cpIntegraEMV.getRspVoucher(), Globales.cpIntegraEMV.getRspOperationNumber());
                                Mouse.OverrideCursor = null;

                                if (this.lista_boletos.Count > 1)
                                    AsociaBoletos();
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
                                        Globales.XMLFacturaE = "";
                                }
                                break;

                            //***********************************************************************************
                            case "denied":

                                // cboBanco.IsEnabled = false;
                                FormaPago.IsEnabled = false;
                                txtNoReservacion.IsEnabled = false;
                                Importe.IsEnabled = false;
                                LblAprob.Visibility = Visibility.Hidden;
                                LblAuth.Visibility = Visibility.Hidden;
                                LblTError.Visibility = Visibility.Hidden;

                                LblDenied.Visibility = Visibility.Visible;
                                //LblDenied.AutoSize = True
                                LblDenied.FontSize = 12;
                                LblDenied.Content = Globales.msjRech + "\n" + Globales.cpIntegraEMV.getRspCdResponse() + " " + Globales.cpIntegraEMV.getRspFriendlyResponse();
                                mensaje =  Globales.cpIntegraEMV.getRspCdResponse() + " " + Globales.cpIntegraEMV.getRspFriendlyResponse();
                                Globales.MessageBoxMitDenied(mensaje);
                                LblDenied.Visibility = Visibility.Visible;
                                CmdNuevo.Visibility = Visibility.Visible;
                                CmdEnviar.Visibility = Visibility.Hidden;
                                cmdVoucher.IsEnabled = false;
                                break;
                            case "error":
                                LblAprob.Visibility = Visibility.Hidden;
                                LblAuth.Visibility = Visibility.Hidden;
                                LblTError.Visibility = Visibility.Visible;
                                mensaje = Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult);
                                LblTError.Text = Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult);
                                Globales.MessageBoxMitError(mensaje);
                                LblDenied.Visibility = Visibility.Hidden;
                                CmdNuevo.Visibility = Visibility.Visible;
                                CmdEnviar.Visibility = Visibility.Hidden;
                                cmdVoucher.IsEnabled = false;
                                break;
                            default:
                                LblAprob.Visibility = Visibility.Hidden;
                                LblAuth.Visibility = Visibility.Hidden;
                                LblTError.Visibility = Visibility.Visible;
                                LblTError.Text = "Verifique su conexión de Internet.";
                                Globales.MessageBoxMitError("Verifique su conexión a internet");
                                LblDenied.Visibility = Visibility.Hidden;
                                CmdNuevo.Visibility = Visibility.Visible;
                                CmdEnviar.Visibility = Visibility.Hidden;
                                cmdVoucher.IsEnabled = false;
                                //'LimpiarBPin
                                break;
                        }
                    } //'fin de if( de captura de numero de aut||izacion
                } //'Fin de if( de pregunta
                // }

                if (!entroCobro)
                {
                    ejecutaCobro();

                }
            }
            Mouse.OverrideCursor = null;
        }

        private string CadenaBoletos()
        {
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

        private void AsociaBoletos()
        {
            string strCadEncriptar, strCadaux, strBoletos, strTipo = string.Empty;
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
                    Globales.MessageBoxMit(Globales.GetDataXml("desc", Globales.cpHTTP_sResult));
                }
            }



        }

        private void ejecutaCobro()
        {
            entroCobro = true;
            switch (Globales.cpIntegraEMV.getRspDsResponse())
            {// 'Evaluacion de la primera llamada a la venta directa
                case "approved":
                    string mensaje = Globales.cpIntegraEMV.getRspAuth();
                    Globales.MessageBoxMitApproved(mensaje);
                    FormaPago.IsEnabled = false;
                    txtNoReservacion.IsEnabled = false;
                    this.BADDBOLETOS.IsEnabled = false;
                    Importe.IsEnabled = false;
                    LblAprob.Visibility = Visibility.Visible;
                    LblAuth.Visibility = Visibility.Visible;
                    LblAuth.Content = "AUT." + Globales.cpIntegraEMV.getRspAuth();

                    LblTError.Visibility = Visibility.Hidden;
                    LblDenied.Visibility = Visibility.Hidden;
                    cmdVoucher.IsEnabled = true;
                    CmdNuevo.IsEnabled = true;
                    CmdEnviar.Visibility = Visibility.Hidden;
                    CmdNuevo.Visibility = Visibility.Visible;

                    this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;
                    this.BENVIAMAIL.Tag = this.NomTdc.Text;

                    NumTdc.IsEnabled = false;
                    Anio.IsEnabled = false;
                    Mes.IsEnabled = false;
                    NomTdc.IsEnabled = false;
                    NumCvv.IsEnabled = false;
                    Fecha.IsEnabled = false;
                    FechaR.IsEnabled = false;
                    BADDBOLETOS.IsEnabled = false;
                    FECHARETORNO.IsEnabled = false;

                    if (Globales.isVentaForzada)
                    {
                        txtAut.IsEnabled = false;
                    }
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    Globales.PrintOptions(Globales.cpIntegraEMV.getRspVoucher(), Globales.cpIntegraEMV.getRspOperationNumber());
                    Mouse.OverrideCursor = null;


                    if (Globales.FacturaE == "1")
                    {
                        if (Globales.MessageConfirm("¿Deseas factura electrónica?"))
                        {
                            frmPreguntaFactura frmPreguntaFactura = new frmPreguntaFactura();
                            frmPreguntaFactura.abrirFrmAhora = abrir;
                            frmPreguntaFactura.cerraPage = cerrar;
                            abrir(frmPreguntaFactura);
                            return;
                        }
                        else
                        {
                            Globales.XMLFacturaE = "";
                        }
                    }
                    break;

                //   ''''***********************************************************************************
                case "denied":
                    string mensajeD = Globales.msjRech + " " + Globales.cpIntegraEMV.getRspCdResponse() + " " + Globales.cpIntegraEMV.getRspFriendlyResponse();
                    Globales.MessageBoxMitDenied(mensajeD);
                    FormaPago.IsEnabled = false;
                    txtNoReservacion.IsEnabled = false;
                    Importe.IsEnabled = false;
                    CmdNuevo.Visibility = Visibility.Hidden;
                    LblAprob.Visibility = Visibility.Hidden;
                    LblAuth.Visibility = Visibility.Hidden;
                    LblTError.Visibility = Visibility.Hidden;

                    LblDenied.Visibility = Visibility.Visible;
                    LblDenied.Content = Globales.msjRech + "\n" + Globales.cpIntegraEMV.getRspCdResponse() + " " + Globales.cpIntegraEMV.getRspFriendlyResponse();

                    LblDenied.Visibility = Visibility.Visible;
                    CmdNuevo.Visibility = Visibility.Visible;
                    CmdEnviar.Visibility = Visibility.Hidden;


                    NumTdc.IsEnabled = false;

                    Anio.IsEnabled = false;
                    Mes.IsEnabled = false;
                    NomTdc.IsEnabled = false;
                    NumCvv.IsEnabled = false;
                    Fecha.IsEnabled = false;
                    BADDBOLETOS.IsEnabled = false;
                    FECHARETORNO.IsEnabled = false;
                    if (Globales.isVentaForzada)
                    {
                        txtAut.IsEnabled = false;
                    }
                    break;
                case "error":
                    string mensajeE = Globales.cpIntegraEMV.getRspDsError();
                    Globales.MessageBoxMitError(mensajeE);
                    if (mensajeE.Contains("La transaccion ya fue aprobada"))
                    {
                        FormaPago.IsEnabled = false;
                        txtNoReservacion.IsEnabled = false;
                        Importe.IsEnabled = false;
                        CmdNuevo.Visibility = Visibility.Hidden;
                        CmdNuevo.Visibility = Visibility.Visible;
                        CmdEnviar.Visibility = Visibility.Hidden;


                        NumTdc.IsEnabled = false;

                        Anio.IsEnabled = false;
                        Mes.IsEnabled = false;
                        NomTdc.IsEnabled = false;
                        NumCvv.IsEnabled = false;
                        Fecha.IsEnabled = false;
                        BADDBOLETOS.IsEnabled = false;
                        FECHARETORNO.IsEnabled = false;
                        if (Globales.isVentaForzada)
                        {
                            txtAut.IsEnabled = false;
                        }
                    }
                    else {
                        CmdEnviar.IsEnabled = true;
                        CmdEnviar.Visibility = Visibility.Visible;
                        FormaPago.IsEnabled = true;
                        Importe.IsEnabled = true;
                        txtNoReservacion.IsEnabled = true;

                        LblAprob.Visibility = Visibility.Hidden;
                        LblAuth.Visibility = Visibility.Hidden;

                        LblTError.Visibility = Visibility.Visible;

                        LblTError.Text = Globales.cpIntegraEMV.getRspDsError();
                    }
                    break;
                default:
                    string mensajeF = "Error de conexión, verifique su reporte";
                    Globales.MessageBoxMitError(mensajeF);
                    CmdEnviar.IsEnabled = true;
                    CmdEnviar.Visibility = Visibility.Visible;
                    FormaPago.IsEnabled = true;
                    Importe.IsEnabled = true;
                    txtNoReservacion.IsEnabled = true;

                    LblAprob.Visibility = Visibility.Hidden;
                    LblAuth.Visibility = Visibility.Hidden;

                    LblTError.Visibility = Visibility.Visible;
                    LblTError.Text = "Verif(ique su conexión de Internet.";
                    CmdNuevo.Visibility = Visibility.Hidden;
                    break;
            }

        }

        private void CmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            this.lista_boletos.Clear();
            this.LBOLETOS.Items.Clear();

            entroCobro = false;
            numeroTarjeta = "";
            lblMoneda.Content = "";
            NumTdc.Text = "";
            numeroTarjeta = "";
            NumCvv.Clear();

            if (Globales.IsOM)
            {
                reabrir();
            }
            else
            {
                NumTdc.IsEnabled = true;
                NumTdc.Visibility = Visibility.Visible;
                Mes.IsEnabled = true;
                Anio.IsEnabled = true;
                NomTdc.IsEnabled = true;
                NumCvv.IsEnabled = true;
                Fecha.Text = "";

                if (Globales.isAerolinea)
                {

                    FechaR.Text = "";
                    FechaRetorno.IsEnabled = true;
                    FechaR.IsEnabled = true;
                }
                Fecha.IsEnabled = true;
                FechaRetorno.IsEnabled = true;
                Label6.Visibility = Visibility.Hidden;
                FormaPago.IsEnabled = false;
                FormaPago.Visibility = Visibility.Hidden;
                txtNoReservacion.IsEnabled = true;
                Importe.IsEnabled = true;
                limpia();
                TypeUsuario.strVoucherCoP = "";
                this.BENVIAMAIL.Visibility = Visibility.Hidden;
                this.BENVIAMAIL.Tag = string.Empty;


                CmdEnviar.IsEnabled = true;
                FECHARETORNO.IsEnabled = true;
            }

            LblTError.Visibility = Visibility.Visible;
            LblTError.Text = "";
            Fecha.Text = "";
            this.LTOTALBOLETOS.Content = "0";
            this.LADDBOLETOS.Content = string.Format("Agregar boletos({0})", this.lista_boletos.Count);
            this.BADDBOLETOS.IsEnabled = false;


            if (Globales.isVentaForzada)
            {
                txtAut.IsEnabled = true;
                txtAut.Text = "";
            }




        }

        private void cmdVoucher_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                Globales.strNombreFP = NOMBRE_GENERAL + ".cmdVoucher_Click()";
                if (TypeUsuario.IsAQ)
                {
                    Globales.VerificaVoucher();

                    return;
                }
                switch (TypeUsuario.TipoImpresora)
                {
                    case "1":

                        TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                        Globales.PrintOptions(Globales.cpIntegraEMV.getRspVoucher(), Globales.cpIntegraEMV.getRspOperationNumber());
                        cmdVoucher.IsEnabled = true;
                        CmdNuevo.IsEnabled = true;

                        break;
                    case "3":
                        Globales.imprimirEpson();
                        break;
                    case "4":

                        TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                        Globales.PrintOptions(Globales.cpIntegraEMV.getRspVoucher(), Globales.cpIntegraEMV.getRspOperationNumber());
                        cmdVoucher.IsEnabled = true;
                        CmdNuevo.IsEnabled = true;

                        break;
                    case "6":
                        {
                            CmdNuevo.IsEnabled = false;
                            cmdVoucher.IsEnabled = false;
                            Globales.PrintOptions(Globales.cpIntegraEMV.getRspVoucher(), Globales.cpIntegraEMV.getRspOperationNumber());
                            cmdVoucher.IsEnabled = true;
                            CmdNuevo.IsEnabled = true;
                        }
                        break;
                    default:
                        {
                            Globales.MessageBoxMit("No ha definido un tipo de impresora.");
                        }
                        break;
                }
            }
            catch
            {
            }
            Mouse.OverrideCursor = null;
        }
        private bool TicketCheckPNR(TextBox t)
        {

            string intDigVer, intDigVer2 = string.Empty;
            bool TicketCheckPNR = false;
            if (!string.IsNullOrWhiteSpace(t.Text) && (Globales.isAgencias || Globales.isAerolinea))
            {
                Globales.strNombreFP = NOMBRE_GENERAL + ".txtNoBoleto_LostFocus";
                if (t.Text.Length < 10)
                {
                    Globales.MessageBoxMit("¡La longitud del boleto es incorrecta!");
                    t.Focus();
                    TicketCheckPNR = true;
                }
            }
            return TicketCheckPNR;
        }

        public void limpia()
        {

            Globales.strNombreFP = NOMBRE_GENERAL + ".Limpia()";
            strfolio = "";
            TypeUsuario.strVoucher = "";
            NumTdc.Text = "";
            numeroTarjeta = "";
            NumCvv.Password = "";
            NomTdc.Text = "";
            txtNoReservacion.Text = "";
            Importe.Text = "";
            LblTError.Text = "";
            LblAuth.Content = "";
            cmdVoucher.IsEnabled = false;
            LblAprob.Visibility = Visibility.Hidden;
            LblAuth.Visibility = Visibility.Hidden;

            LblDenied.Visibility = Visibility.Hidden;


            Mes.Text = "";
            Anio.Text = "";
            int anio = DateTime.Now.Year;

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

            FormaPago.SelectedIndex = -1;
            CmdEnviar.Visibility = Visibility.Visible;
            CmdNuevo.Visibility = Visibility.Hidden;

        }

        public string strfolio { get; set; }

        private void cmdAcepEmp_Click(object sender, RoutedEventArgs e)
        {
            if (Globales.isAerolinea)
            {
                cboEmpresa.Text = TypeUsuario.nb_company + "-" + TypeUsuario.Id_Company;
            }

            if (cboEmpresa.Text.Contains("-") || cboEmpresa.SelectedIndex != -1)
            {

                Globales.mynumero = Globales.Right(cboEmpresa.Text.Trim(), 4);

                Globales.strNombreFP = NOMBRE_GENERAL + ".cmdAcepEmp_Click()";
                if (TypeUsuario.iata != "" && Globales.isAerolinea)
                {
                    
                }
                else
                {

                    TypeUsuario.iata = Globales.GetDataXml("iatarp3", Globales.GetDataXml("ventasrp3" + Globales.mynumero, TypeUsuario.CadenaXML));
                    UserIata.Content = "IATA: "+TypeUsuario.iata;
                }
                fraCubreTodo.Visibility = Visibility.Hidden;
                fraDatos.Visibility = Visibility.Visible;
                subEtiqueta.Content = cboEmpresa.Text;
                if (Globales.isAgencias)
                {
                    caption = "Cargo sin presencia de tarjeta. " + cboEmpresa.Text;
                }
                else
                {
                    if (!caption.Contains("operativa"))
                    {
                        if (Globales.isVentaForzada)
                        {
                            caption = "VENTA DE BOLETOS: Venta Forzada";
                        }
                        else
                        {
                            caption = "VENTA DE BOLETOS: Cargo a tarjeta bancaria sin presencia de tarjeta";
                        }
                    }
                    else
                    {
                        caption = "VENTA DE BOLETOS: Cargo a tarjeta bancaria vía operativa manual";
                    }
                }
            }
        }

        private void carga()
        {
            subEtiqueta.Content = "";
            Fecha.DisplayDateStart = DateTime.Now;
            FechaR.DisplayDateStart = DateTime.Now;
            FechaR.IsEnabled = false;
            Fecha.DisplayDate = DateTime.Now;
            lblMoneda.Content = string.Empty;
            Globales.cpIntegraEMV.dbgEndOperation();
            banboletosrepetidos = false;
            UserIata.Content ="IATA: "+ TypeUsuario.iata;
            if (Globales.isAgencias)
            {
                Globales.cpIntegraEMV.dbgSetIsAgencia("1");
                ObtieneEmpresasAgencia(Globales.GetDataXml("catempresas", TypeUsuario.CadenaXML), "M");
                cboEmpresa.SelectedIndex = 0;
                txtNoReservacion.MaxLength = 6;

                if (!TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
                {
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

                    UserIata.Content = "IATA: "+ TypeUsuario.iata;
                    caption = "Venta sin presencia de tarjeta.";
                    FechaRetorno.Visibility = Visibility.Visible;
                    FECHARETORNO.Visibility = Visibility.Visible;
                    FechaR.Visibility = Visibility.Visible;
                    FechaR.Text = "";

                    Label8.Content = "PNR";
                    txtNoReservacion.MaxLength = 6;
                    if (Globales.isVentaForzada)
                    {
                        Label5.Content = "Número de Autorización";
                        Label5.Visibility = Visibility.Visible;
                        numCvv2.Visibility = Visibility.Hidden;
                        txtAut2.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        Label5.Content = "Cod Seg";
                        Label5.Visibility = Visibility.Visible;
                        numCvv2.Visibility = Visibility.Visible;
                        txtAut2.Visibility = Visibility.Hidden;
                    }
                    cmdAcepEmp_Click();
                }
                else
                {
                    Fecha.Visibility = Visibility.Hidden;
                    Label12.Visibility = Visibility.Hidden;
                    txtNoReservacion.MaxLength = 40;
                }
            }
            limpia();
            if (Globales.isAerolinea)
            {
                Label8.Content = "PNR";
            }
            else
            {
                Label8.Content = TypeUsuario.reference;
            }
        }

        public void cmdAcepEmp_Click()
        {
            if (Globales.isAerolinea)
            {
                cboEmpresa.Text = TypeUsuario.nb_company + "-" + TypeUsuario.Id_Company;
                Globales.mynumero = TypeUsuario.Id_Company;
            }
            if (!string.IsNullOrWhiteSpace(Globales.mynumero))
            {

                Globales.strNombreFP = NOMBRE_GENERAL + ".cmdAcepEmp_Click()";
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
                fraDatos.Visibility = Visibility.Visible;
                if (Globales.isAgencias)
                {
                    caption = "Cargo sin presencia de tarjeta. " + cboEmpresa.Text;
                }
                else
                {
                    if (!caption.Contains("operativa"))
                    {
                        if (Globales.isVentaForzada)
                        {
                            caption = "VENTA DE BOLETOS: Venta Forzada";
                        }
                        else
                        {
                            caption = "VENTA DE BOLETOS: Cargo a tarjeta bancaria sin presencia de tarjeta";
                        }
                    }
                    else
                    {
                        caption = "VENTA DE BOLETOS: Cargo a tarjeta bancaria vía operativa manual";
                    }
                    //lbl3Digitos.Visibility = Visibility.Visible;
                    //lbl3Digitos.Content = TypeUsuario.iata;

                }


            }
            else
            {
                Globales.MessageBoxMit("Seleccione Empresa Valida");
            }




        }

        private void ObtieneEmpresasAgencia(string strBancos, string tipo)
        {
            string[] empresas = strBancos.Split('|');

            for (int i = 0; i < empresas.Length; i++)
            {
                if (empresas[i].Contains("," + tipo + ","))
                {

                    string[] aux = empresas[i].Split(',');

                    cboEmpresa.Items.Add(aux[1] + " - " + aux[0]);
                }
            }


        }
        private Control control = null;
        private void NumTdc_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Globales.noMostrarMensajes) return;
            numeroTarjeta = "";

            if (!string.IsNullOrWhiteSpace(NumTdc.Text))
            {

                if (NumTdc.Text.Length >= 14)
                {
                    numeroTarjeta = NumTdc.Text;
                    NumTdc.Text = NumTdc.Text.Substring(0, 6) + "******" + NumTdc.Text.Substring(NumTdc.Text.Length - 4);
                }
                else
                {
                    Globales.MessageBoxMit("Favor de validar el número de tarjeta");
                    NumTdc.Text = "";
                    numeroTarjeta = "";
                    control = sender as TextBox;
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(NumTdc.Text))
            {

                validaBotonBoletos();
                string numEmpresa = string.Empty;

                numEmpresa = Globales.Right(cboEmpresa.Text, 4);
                if (Globales.isAgencias)
                {
                    Globales.cpIntegraEMV.dbgSetTipoRP3(true, numEmpresa);
                }
                if (!caption.Contains("operativa"))
                {// InStr(1, frmMoto.Caption, "operativa") = 0 Then 'MOTO{
                    if (Globales.isVentaForzada)
                    {
                        Globales.cpIntegraEMV.dbgSetTipoPago(3);
                        Globales.merchantMoto = Globales.cpIntegraEMV.dbgGetMerchantVtaForzada(numeroTarjeta);
                        if (Globales.cpIntegraEMV.dbgGetisAmex())
                        {
                            Globales.MessageBoxMit("No aplica Venta Forzada para American Express");
                            //txtMascara.Text = ""
                            //txtMascara.Visible = False
                            //NumTdc.Visible = Truec
                            NumTdc.Text = "";
                            numeroTarjeta = "";
                            // NumTdc.Focus();
                            return;
                        }
                    }
                    else
                        try
                        {

                            if (Globales.isAgencias)
                            {
                                Globales.merchantMoto = Globales.cpIntegraEMV.dbgGetMerchantMOTORP3(numeroTarjeta, numEmpresa); //dbgGetMerchantMotoRP3
                            }
                            else
                            {
                                Globales.merchantMoto = Globales.cpIntegraEMV.dbgGetMerchantMoto(numeroTarjeta, "10");
                            }
                        }
                        catch { 
                        
                        }
                }
                else
                {
                    Globales.cpIntegraEMV.dbgSetTipoPago(1);
                    Globales.merchantMoto = Globales.cpIntegraEMV.dbgGetMerchantOpManual(numeroTarjeta);
                }
                Globales.isAmex = Globales.cpIntegraEMV.dbgGetisAmex();
                FormaPago_Click();
                NumCvv.Password = "";
                if (Globales.isAmex)
                {
                    NumCvv.MaxLength = 4;
                }
                else
                {
                    NumCvv.MaxLength = 3;
                }
            }



        }

        private void validaBotonBoletos()
        {
            if (!string.IsNullOrWhiteSpace(NumTdc.Text) &&
            !string.IsNullOrWhiteSpace(Mes.Text) &&
                !string.IsNullOrWhiteSpace(Anio.Text) &&
                !string.IsNullOrWhiteSpace(NomTdc.Text) &&
                !string.IsNullOrWhiteSpace(txtNoReservacion.Text) &&
                 !string.IsNullOrWhiteSpace(Importe.Text) &&
                 !string.IsNullOrWhiteSpace(Fecha.Text))
            {
                BADDBOLETOS.IsEnabled = true;
            }
            else {
                BADDBOLETOS.IsEnabled = false;
            }
            
        }

        private void FormaPago_Click()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".FormaPago_Click";

            if (Globales.GetDataXml("MXN", TypeUsuario.CadenaXML).Contains(Globales.merchantMoto))
            {
                lblMoneda.Content = "MXN";
            }
            else
            {
                lblMoneda.Content = "USD";
            }
        }

        private void Numeros(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }
        private void Importe_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
            validaBotonBoletos();
        }

        private void NumCvv_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Globales.noMostrarMensajes) return;
            if (Globales.isAgencias && string.IsNullOrWhiteSpace(NumCvv.Password))
            {
                return;
            }
            if (!Globales.isAgencias)
            {// Not isAgencias Then
                if (NumCvv.Password != "000" || NumCvv.Password != "0000")
                {
                    if (Globales.isAmex && NumCvv.Password.Length != 4 && NumCvv.Password.Length != 0)
                    {
                        Globales.MessageBoxMit("El código de seguridad debe ser de 4 dígitos para AMEX");
                        control = sender as PasswordBox;
                        isCvv = true;
                        return;
                    }
                    else if (!Globales.isAmex && NumCvv.Password.Length != 3 && NumCvv.Password.Length != 0)
                    {
                        Globales.MessageBoxMit("El código de seguridad debe ser de 3 dígitos para V/MC");
                        control = sender as PasswordBox;
                        isCvv = true;
                        return;
                    }
                }
                else
                {

                    Globales.MessageBoxMit("El CVV es incorrecto, favor de validar");
                    control = sender as PasswordBox;
                    isCvv = true;
                    return;
                }
            }
        }

       

        private void Command2_Click(object sender, RoutedEventArgs e)
        {
            this.TNUMBOLETO.Text = string.Empty;


            if (this.lista_boletos == null)
                this.lista_boletos = new List<string>();

            if (!banboletosrepetidos)
            {
                BoletosExtra.Visibility = Visibility.Hidden;
                fraDatos.Visibility = Visibility.Visible;
                this.LTOTALBOLETOS.Content = this.lista_boletos.Count;
                this.LADDBOLETOS.Content = string.Format("Agregar boletos({0})", this.lista_boletos.Count);
            }
        }

        private void CmdBoletosA_Click(object sender, RoutedEventArgs e)
        {
            BoletosExtra.Visibility = Visibility.Visible;
            fraDatos.Visibility = Visibility.Hidden;

            if (this.lista_boletos == null)
                this.lista_boletos = new List<string>();
        }


        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
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


        private void validaCVV(object sender, TextCompositionEventArgs e)
        {
            Globales.validaCVV(sender, e);
        }

        private void NumerosPunto(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private bool FechaVencida()
        {
            if (Mes.SelectedItem != null && Anio.SelectedItem != null)
            {
                DateTime hoy = DateTime.Now;
                int mes_c = Convert.ToInt32(Mes.Text);
                int anio_c = Convert.ToInt32( Anio.Text);
                DateTime fecha_elegida = new DateTime(anio_c, mes_c, DateTime.DaysInMonth(anio_c, mes_c));
                int resultado = DateTime.Compare(fecha_elegida, hoy);
                if (resultado < 0)
                {
                    Globales.MessageBoxMit("Tarjeta Vencida");
                    Anio.Focus();
                    return true;
                }

            }
            return false;
        }

        private void ValidaFecha(object sender, RoutedEventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           
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
                    else {
                        TextBox text = control as TextBox;
                        text.Focus();
                    }
                   control = null;
                }
            };
            tiempo.Start();  
          
            if(Globales.IsOM){
                numCvv2.Visibility = Visibility.Hidden;
                Label5.Visibility = Visibility.Hidden;
            }
            if(!Globales.isAgencias || TypeUsuario.CadenaXML.Contains("PCPAYRP3")){
                if (Globales.isAerolinea)
                {
                    LTITULO.Content = "VENTA DE BOLETOS: Venta sin presencia de tarjeta.";
                 
                }
            }
        }





        #region "Formulario de boletos"
        private void BAGREGABOLETO_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TNUMBOLETO.Text))
            {
                Globales.MessageBoxMit("Ingrese un número de boleto.");
                TNUMBOLETO.Focus();
                return;
            }
            if(Globales.verificadorBoleto(TNUMBOLETO.Text)){
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
        private void ELIMINARBOLETO_Click(object sender, RoutedEventArgs e)
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
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BoletosExtra.Visibility = Visibility.Visible;
            fraDatos.Visibility = Visibility.Hidden;

            if (this.lista_boletos == null)
                this.lista_boletos = new List<string>();
        }
        #endregion

        private void TNUMBOLETO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                BAGREGABOLETO_Click(sender, e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void eventoTeclado(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

        private void gotfocusnumtdc(object sender, RoutedEventArgs e)
        {
            NumTdc.Text = numeroTarjeta;
        }

        private void letra(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTexto(sender, e);
        }

        private void NomTdc_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.numeroTarjeta))
            {
                Globales.MessageBoxMit("Ingrese número de tarjeta.");
                e.Handled = true;
            }
        }

        private void evitar(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
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

        private void Mes_LostFocus(object sender, RoutedEventArgs e)
        {

            validaBotonBoletos();
        }
        bool isCvv = false;
        private void lostfocusnumtdc(object sender, RoutedEventArgs e)
        {
            if (Globales.noMostrarMensajes) return;
            if(Globales.isAerolinea && txtNoReservacion.Text.Length != 6 && !string.IsNullOrWhiteSpace(txtNoReservacion.Text)){
                Globales.MessageBoxMit("El PNR debe de ser de 6 dígitos");
                control = sender as TextBox;
            }
        }

        private void solonumeroyletras(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }
    }
}












