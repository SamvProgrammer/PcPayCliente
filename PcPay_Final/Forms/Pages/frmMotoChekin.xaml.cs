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

namespace PcPay.Forms.Pages
{
    /// <summary>
    /// Interaction logic for frmMotoChekin.xaml
    /// </summary>
    public partial class frmMotoChekin : Page
    {
        public string caption = "";
        public string num_tarjeta { get; set; }
        public frmMotoChekin()
        {
            InitializeComponent();
            this.numTdc.GotFocus += Globales.setFocusMit2;
            this.mes.GotFocus += Globales.setFocusMit2;
            this.anio.GotFocus += Globales.setFocusMit2;
            this.nomTdc.GotFocus += Globales.setFocusMit2;
            this.numCvv.GotFocus += Globales.setFocusMit2;
            this.numOrden.GotFocus += Globales.setFocusMit2;
            this.txtRoom.GotFocus += Globales.setFocusMit2;
            this.importe.GotFocus += Globales.setFocusMit2;

            this.numTdc.LostFocus += Globales.lostFocusMit2;
            this.mes.LostFocus += Globales.lostFocusMit2;
            this.anio.LostFocus += Globales.lostFocusMit2;
            this.nomTdc.LostFocus += Globales.lostFocusMit2;
            this.numCvv.LostFocus += Globales.lostFocusMit2;
            this.numOrden.LostFocus += Globales.lostFocusMit2;
            this.txtRoom.LostFocus += Globales.lostFocusMit2;
            this.importe.LostFocus += Globales.lostFocusMit2;


            numCvv.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            numTdc.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;


            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;
            this.BENVIAMAIL.MouseDown += Globales.sendMail_GridMouseDown;


        }

        private void cargandoVentana(object sender, RoutedEventArgs e)
        {
            lblMoneda.Content = "";
            Globales.cpIntegraEMV.dbgEndOperation();
            label2.Content = "Dato Adicional";

            this.numTdc.ToolTip = Globales.msjTarjeta;
            this.nomTdc.ToolTip = Globales.msjNombre;
            this.numOrden.ToolTip = Globales.MsjReferencia;

            limpia();
            label_8.Content = TypeUsuario.reference;
        }

        private void limpia()
        {
            numTdc.Text = "";
            this.num_tarjeta = string.Empty;
            numCvv.Password = "";
            nomTdc.Text = "";
            numOrden.Text = "";
            importe.Text = "";

            txtRoom.Text = "";
            cmdVoucher.IsEnabled = false;

            //typyeusuario
            formaPago.Items.Clear();
            Globales.obtieneBancos(cboBanco, Globales.GetDataXml("catbancos", TypeUsuario.CadenaXML));
            //end typeusuario
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
            cmdEnviar.Visibility = Visibility.Visible;
            cmdNuevo.Visibility = Visibility.Hidden;
            if (TypeUsuario.Id_Company == "0059")
            {
                numOrden.MaxLength = 28;
            }
            else
            {
                numOrden.MaxLength = Globales.MAXCAR;
            }
        }

        private void gotFocusnumtdc(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(this.num_tarjeta))
                this.numTdc.Text = this.num_tarjeta;
            else
                this.numTdc.Text = string.Empty;
        }

        private void lostfocusnumtdc(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(numTdc.Text.Trim()))
            {
                if (numTdc.Text.Length >= 14)
                {
                    this.num_tarjeta = this.numTdc.Text;
                    this.numTdc.Text = string.Format("{0}******{1}", numTdc.Text.Substring(0, 6), numTdc.Text.Substring(numTdc.Text.Length - 4));
                }
                else
                {
                    Globales.MessageBoxMit("Favor de validar el número de tarjeta");
                    numTdc.Text = "";
                    this.num_tarjeta = string.Empty;
                    return;
                }
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
            if (Globales.GetDataXml("MXN", TypeUsuario.CadenaXML).Contains(Globales.merchantMoto))
            {
                lblMoneda.Content = "MXN";
            }
            else
            {
                lblMoneda.Content = "USD";
            }
        }

        private void solonumero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }


        private void gotFocustxtMascara(object sender, RoutedEventArgs e)
        {

            numTdc.Visibility = Visibility.Visible;
        }

        private void gotFocusNomtdc(object sender, RoutedEventArgs e)
        {
            //lblTError.Text = Globales.msjNombre;
        }

        private void soloLetra(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTexto(sender, e);
        }

        private void gotfocusReferencia(object sender, RoutedEventArgs e)
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

        private void gotFocusCuarto(object sender, RoutedEventArgs e)
        {
            //lblTError.Text = "Ingrese el dato adicional";
        }

        private void numeroLetra(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

        private void gotfocusImporte(object sender, RoutedEventArgs e)
        {
            //lblTError.Text = Globales.MsjImporte;
        }

        private void cmdEnviarClick(object sender, RoutedEventArgs e)
        {
            bool bolOperacion;
            string mensaje = string.Empty;
            //lblTError.Text = "";
            //lblError.Visibility = Visibility.Hidden;
            cmdEnviar.IsEnabled = false;
            if (string.IsNullOrWhiteSpace(this.num_tarjeta))
            {
                Globales.MessageBoxMit("Introduzca el número de tarjeta");
                cmdEnviar.IsEnabled = true;
                return;
            }
            else if (mes.SelectedIndex == -1)
            {
                Globales.MessageBoxMit("Seleccione el mes de vencimiento de la tarjeta");
                cmdEnviar.IsEnabled = true;
                return;
            }
            else if (anio.SelectedIndex == -1)
            {
                Globales.MessageBoxMit("Seleccione el año de vencimiento de la tarjeta");
                cmdEnviar.IsEnabled = true;
                return;
            }
            else if (string.IsNullOrWhiteSpace(nomTdc.Text))
            {
                Globales.MessageBoxMit("Introduzca el nombre del titular");
                cmdEnviar.IsEnabled = true;
                return;
            }else if(string.IsNullOrWhiteSpace(numCvv.Password)){
                Globales.MessageBoxMit("Introduzca el code de la tarjeta");
                cmdEnviar.IsEnabled = true;
                return;
            }
            else if (string.IsNullOrWhiteSpace(numOrden.Text))
            {
                Globales.MessageBoxMit("Introduzca "+TypeUsuario.reference);
                cmdEnviar.IsEnabled = true;
                return;
            }
            else if ((numCvv.Password == "000" || numCvv.Password == "0000") && numCvv.Visibility == Visibility.Visible)
            {
                Globales.MessageBoxMit("Código de seguridad invalido");
                cmdEnviar.IsEnabled = true;
                return;
            }
            else if (string.IsNullOrWhiteSpace(Globales.merchantMoto))
            {
                Globales.MessageBoxMit("NO hay planes de pago para esta tarjeta, porfavor cambie la tarjeta");
                cmdEnviar.IsEnabled = true;
                return;
            }
            else if (string.IsNullOrWhiteSpace(importe.Text))
            {
                Globales.MessageBoxMit("Introduzca el importe");
                cmdEnviar.IsEnabled = true;
                return;
            }
            else
            {
                if (TypeUsuario.Id_Company == "0059")
                {
                    if (numOrden.Text.Length != 28)
                    {
                        Globales.MessageBoxMit("El no. de REFERENCIA debe ser de 28 posiciones.");
                        cmdEnviar.IsEnabled = true;
                        return;
                    }
                }

                int aux = 0;
                int aux2 = 0;
                if (anio.Text.Substring(2, 2) == DateTime.Now.Year.ToString().Substring(2) && Convert.ToInt32(mes.Text) < DateTime.Now.Month)
                {
                    Globales.MessageBoxMit("La tarjeta ha vencido");
                    cmdEnviar.IsEnabled = true;
                    return;
                }
                if (Globales.isAmex)
                {
                    if (numCvv.Password.Length != 4)
                    {
                        Globales.MessageBoxMit("Introduzca el code de la tarjeta");
                        cmdEnviar.IsEnabled = true;
                        return;
                    }
                }
                else
                {
                    if (numCvv.Password.Length != 3)
                    {
                        Globales.MessageBoxMit("Introduzca el code de la tarjeta");
                        cmdEnviar.IsEnabled = true;
                        return;
                    }
                }
                cmdEnviar.IsEnabled = false;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                string strTypeC = "";
                Globales.cpIntegracion_Clear();
                if (Globales.isAmex)
                {
                    strTypeC = "AMEX";
                }
                else
                {
                    strTypeC = "V/MC";
                }
                Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL_CHECKIN;
                string strTpOperacion = "";
                if (!caption.Contains("operativa"))
                {
                    strTpOperacion = "10";
                }
                else
                {
                    strTpOperacion = "14";
                }
                Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
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
                     lblMoneda.Content.ToString(),
                     txtRoom.Text,
                     strTypeC,
                     nomTdc.Text,
                     this.num_tarjeta,
                     mes.Text,
                     anio.Text.Substring(2, 2),
                     numCvv.Password
                    );

                Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();
                bolOperacion = true;
                if (bolOperacion)
                {
                    //=====================================Venta Forzada============================

                    string strCadEncriptar = "";
                    if (Globales.GetDataXml("cd_response", Globales.cpIntegracion_sResult) == Globales.COD_VF)
                    {
                        if (Globales.MessageConfirm(Globales.GetDataXml("msgautvf", TypeUsuario.CadenaXML)))
                        {
                            string strNumAut = "";
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
                                string strFolioC = "";


                                strFolioC = Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult);
                                Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL_VF;
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
                                     mes.Text,
                                     anio.Text.Substring(2, 2),
                                     numCvv.Password,
                                     importe.Text,
                                     lblMoneda.Content.ToString(),
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
                                            txtRoom.IsEnabled = false;
                                            //lblAprob.Visibility = Visibility.Visible;
                                            //lblAuth.Visibility = Visibility.Visible;
                                            //lblTError.Visibility = Visibility.Hidden;
                                            //lblError.Visibility = Visibility.Hidden;
                                            //lblDenied.Visibility = Visibility.Hidden;

                                            mensaje = string.Format("¡COBRO APROBADO! AUTORIZACIÓN: {0}", Globales.GetDataXml("auth", Globales.cpIntegracion_sResult));
                                            Globales.MessageBoxMitApproved(mensaje);

                                            TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult).Trim();
                                            escogerImpresora();
                                            if (TypeUsuario.IsAQ)
                                            {
                                                Globales.VerificaVoucher();
                                            }
                                            cmdVoucher.IsEnabled = true;
                                            cmdNuevo.Visibility = Visibility.Visible;
                                            cmdEnviar.Visibility = Visibility.Hidden;
                                            //imgmail.visinble = tyoeusuar.enviacorreo
                                            this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;
                                            this.BENVIAMAIL.Tag = this.nomTdc.Text;

                                            break;
                                        case "denied":
                                            numTdc.IsEnabled = false;
                                            mes.IsEnabled = false;
                                            anio.IsEnabled = false;
                                            nomTdc.IsEnabled = false;
                                            numCvv.IsEnabled = false;
                                            cboBanco.IsEnabled = false;
                                            formaPago.IsEnabled = false;
                                            numOrden.IsEnabled = false;
                                            importe.IsEnabled = false;
                                            txtRoom.IsEnabled = false;
                                            mensaje = string.Format("CHECKIN RECHAZADO: {0}", Globales.GetDataXml("cd_response", Globales.cpIntegracion_sResult));
                                            Globales.MessageBoxMitDenied(mensaje);
                                            cmdNuevo.Visibility = Visibility.Visible;
                                            cmdEnviar.Visibility = Visibility.Hidden;
                                            break;
                                        case "error":
                                            mensaje = Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult);
                                            Globales.MessageBoxMitError(mensaje);
                                            cmdNuevo.Visibility = Visibility.Visible;
                                            cmdEnviar.Visibility = Visibility.Hidden;
                                            cmdVoucher.IsEnabled = false;
                                            break;
                                        default:
                                            //lblAprob.Visibility = Visibility.Hidden;
                                            //lblAuth.Visibility = Visibility.Hidden;
                                            //lblError.Visibility = Visibility.Visible;
                                            //lblTError.Visibility = Visibility.Hidden;
                                            mensaje = "Verifique su conexión a internet";
                                            Globales.MessageBoxMitError(mensaje);
                                            //lblDenied.Visibility = Visibility.Hidden;
                                            cmdNuevo.Visibility = Visibility.Visible;
                                            cmdEnviar.Visibility = Visibility.Hidden;
                                            cmdVoucher.IsEnabled = false;
                                            break;
                                    }
                                    cmdEnviar.IsEnabled = true;
                                }
                                else
                                {
                                    //lblAprob.Visibility = Visibility.Hidden;
                                    //lblAuth.Visibility = Visibility.Hidden;
                                    //lblTError.Visibility = Visibility.Visible;
                                    mensaje = Globales.cpIntegracion_sError;
                                    Globales.MessageBoxMitError(mensaje);
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

                    //FIN DE VENTA FORZADA
                    Mouse.OverrideCursor = null;
                    switch (Globales.GetDataXml("response", Globales.cpIntegracion_sResult).ToLower())
                    {
                        case "approved":
                            numTdc.IsEnabled = false;
                            txtRoom.IsEnabled = false;
                            mes.IsEnabled = false;
                            anio.IsEnabled = false;
                            nomTdc.IsEnabled = false;
                            numCvv.IsEnabled = false;
                            cboBanco.IsEnabled = false;
                            formaPago.IsEnabled = false;
                            numOrden.IsEnabled = false;
                            importe.IsEnabled = false;


                            mensaje = string.Format(" {0}", Globales.GetDataXml("auth", Globales.cpIntegracion_sResult));
                            Globales.MessageBoxMitApproved(mensaje);

                            TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult).Trim();
                            escogerImpresora();
                            if (TypeUsuario.IsAQ)
                            {
                                Globales.VerificaVoucher();
                                Mouse.OverrideCursor = null;
                                return;
                            }
                            cmdVoucher.IsEnabled = true;
                            cmdNuevo.Visibility = Visibility.Visible;
                            cmdEnviar.Visibility = Visibility.Hidden;
                            //Falta lo de imagenmail.visible = typeusuario.enviarCorreo
                            this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;
                            this.BENVIAMAIL.Tag = this.nomTdc.Text;
                            break;
                        case "denied":
                            numTdc.IsEnabled = false;
                            numCvv.IsEnabled = false;
                            mes.IsEnabled = false;
                            anio.IsEnabled = false;
                            nomTdc.IsEnabled = false;
                            numTdc.IsEnabled = false;
                            cboBanco.IsEnabled = false;
                            formaPago.IsEnabled = false;
                            numOrden.IsEnabled = false;
                            importe.IsEnabled = false;
                            txtRoom.IsEnabled = false;

                            mensaje = string.Format("{0}", Globales.GetDataXml("cd_response", Globales.cpIntegracion_sResult));
                            Globales.MessageBoxMitDenied(mensaje);

                            cmdNuevo.Visibility = Visibility.Visible;
                            cmdEnviar.Visibility = Visibility.Hidden;
                            break;
                        case "error":
                            mensaje = Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult);
                            Globales.MessageBoxMitError(mensaje);
                            if (mensaje.Contains("La transaccion ya fue aprobada"))
                            {
                                numTdc.IsEnabled = false;
                                numCvv.IsEnabled = false;
                                mes.IsEnabled = false;
                                anio.IsEnabled = false;
                                nomTdc.IsEnabled = false;
                                numTdc.IsEnabled = false;
                                cboBanco.IsEnabled = false;
                                formaPago.IsEnabled = false;
                                numOrden.IsEnabled = false;
                                importe.IsEnabled = false;
                                txtRoom.IsEnabled = false;
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

                            cmdNuevo.Visibility = Visibility.Hidden;
                            cmdEnviar.Visibility = Visibility.Visible;

                            mensaje = Globales.GetDataXml("Verifique su conexión de internet", Globales.cpIntegracion_sResult);
                            Globales.MessageBoxMitError(mensaje);
                            break;
                    }
                }
                else
                {

                }

            }
            Mouse.OverrideCursor = null;
            cmdEnviar.IsEnabled = true;
        }

        private void escogerImpresora()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
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
                            TypeUsuario.strVoucher = Globales.VoucherHtml1(Globales.cpHTTP_sResult);
                            Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        }
                        cmdVoucher.IsEnabled = true;
                        cmdNuevo.IsEnabled = true;
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
                        Globales.MessageBoxMit("No ha definido un tipo de impresora.");
                        break;

                }
            }
            catch
            {

            }
            Mouse.OverrideCursor = null;
        }

        private void cmdNuevoClick(object sender, RoutedEventArgs e)
        {
            lblMoneda.Content = "";
            numTdc.IsEnabled = true;
            numTdc.Visibility = Visibility;
            this.num_tarjeta = string.Empty;

            mes.IsEnabled = true;
            anio.IsEnabled = true;
            nomTdc.IsEnabled = true;
            cboBanco.IsEnabled = true;
            cboBanco.IsEnabled = true;
            numCvv.IsEnabled = true;
            label_6.Visibility = Visibility.Hidden;
            formaPago.Visibility = Visibility.Hidden;
            formaPago.IsEnabled = true;

            numOrden.IsEnabled = true;
            importe.IsEnabled = true;
            txtRoom.IsEnabled = true;
            limpia();
            TypeUsuario.strVoucherCoP = "";
            //imgmail.visible = false;
            this.BENVIAMAIL.Tag = string.Empty;
            this.BENVIAMAIL.Visibility = Visibility.Hidden;
        }

        private void cmdvoucherclick(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (TypeUsuario.IsAQ)
            {
                Globales.VerificaVoucher();
                Mouse.OverrideCursor = null;
                return;
            }
            escogerImpresora();
            Mouse.OverrideCursor = null;
        }

        private void txtRoom_LostFocus(object sender, RoutedEventArgs e)
        {
            txtRoom.Text = txtRoom.Text.ToUpper();
        }

        private void numOrden_LostFocus(object sender, RoutedEventArgs e)
        {
            numOrden.Text = numOrden.Text.ToUpper();
        }

        private void numCvv_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Windows.Controls.PasswordBox texto = (System.Windows.Controls.PasswordBox)sender;
            Regex es;
            if (texto.Password.Contains(".") && e.Text.Contains("."))
            {
                es = new Regex("[^¡]+");
                e.Handled = es.IsMatch(e.Text);
                return;
            }
            es = new Regex("[^0-9.]+");
            e.Handled = es.IsMatch(e.Text);
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void numOrden_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

        private void contra(object sender, TextCompositionEventArgs e)
        {
            Regex es = new Regex("[^0-9]+");
            e.Handled = es.IsMatch(e.Text);
        }

        private void importe_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void antes(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(num_tarjeta))
            {
                Globales.MessageBoxMit("Ingrese número de tarjeta.");
                e.Handled = true;
            }
        }
    }
}
