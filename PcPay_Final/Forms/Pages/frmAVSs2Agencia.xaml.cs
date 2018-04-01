using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
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
    /// Lógica de interacción para frmAVSs2Agencia.xaml
    /// </summary>
    public partial class frmAVSs2Agencia : Page
    {
        public abrirFrm abrir;
        private string boletos { get; set; }
        public DispatcherTimer tiempo;
        public frmAVSs2Agencia()
        {
            lista_boletos = new List<string>();
            InitializeComponent();
            Carga();
            tiempo = new DispatcherTimer();
            #region"**********Efectos**********"
            txtNombre.GotFocus += Globales.setFocusMit2;
            txtPaterno.GotFocus += Globales.setFocusMit2;
            txtMaterno.GotFocus += Globales.setFocusMit2;
            txtTelefonoPersonal.GotFocus += Globales.setFocusMit2;
            txtLadaPersonal.GotFocus += Globales.setFocusMit2;
            txtEmail.GotFocus += Globales.setFocusMit2;
            numCvv.GotFocus += Globales.setFocusMit2;
            numTdc.GotFocus += Globales.setFocusMit2;
            Mes.GotFocus += Globales.setFocusMit2;
            Anio.GotFocus += Globales.setFocusMit2;
            nomTdc.GotFocus += Globales.setFocusMit2;
            numOrden.GotFocus += Globales.setFocusMit2;
            importe.GotFocus += Globales.setFocusMit2;


            txtCalle.GotFocus += Globales.setFocusMit2;
            txtExt.GotFocus += Globales.setFocusMit2;
            txtInt.GotFocus += Globales.setFocusMit2;
            txtCP.GotFocus += Globales.setFocusMit2;
            cboColonia.GotFocus += Globales.setFocusMit2;
            txtEstado.GotFocus += Globales.setFocusMit2;
            txtPais.GotFocus += Globales.setFocusMit2;
            txtDelMun.GotFocus += Globales.setFocusMit2;
            TNUMBOLETO.GotFocus += Globales.setFocusMit2;
            txtCiudad.GotFocus += Globales.setFocusMit2;
            txtCiudad.LostFocus += Globales.lostFocusMit2;

            txtNombre.LostFocus += Globales.lostFocusMit2;
            txtPaterno.LostFocus += Globales.lostFocusMit2;
            txtMaterno.LostFocus += Globales.lostFocusMit2;
            txtTelefonoPersonal.LostFocus += Globales.lostFocusMit2;
            txtLadaPersonal.LostFocus += Globales.lostFocusMit2;
            txtEmail.LostFocus += Globales.lostFocusMit2;
            numCvv.LostFocus += Globales.lostFocusMit2;
            numTdc.LostFocus += Globales.lostFocusMit2;
            Mes.LostFocus += Globales.lostFocusMit2;
            Anio.LostFocus += Globales.lostFocusMit2;
            nomTdc.LostFocus += Globales.lostFocusMit2;
            numOrden.LostFocus += Globales.lostFocusMit2;
            importe.LostFocus += Globales.lostFocusMit2;

            txtCalle.LostFocus += Globales.lostFocusMit2;
            txtExt.LostFocus += Globales.lostFocusMit2;
            txtInt.LostFocus += Globales.lostFocusMit2;
            txtCP.LostFocus += Globales.lostFocusMit2;
            cboColonia.LostFocus += Globales.lostFocusMit2;
            txtEstado.LostFocus += Globales.lostFocusMit2;
            txtPais.LostFocus += Globales.lostFocusMit2;
            txtDelMun.LostFocus += Globales.lostFocusMit2;
            TNUMBOLETO.LostFocus += Globales.lostFocusMit2;
            TNUMBOLETO.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;

            #endregion
            this.numTdc.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;

            this.importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;
            numCvv.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            this.fraCliente.Visibility = Visibility.Visible;
            this.fraDireccion.Visibility = Visibility.Hidden;

            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;
            this.BENVIAMAIL.MouseDown += Globales.sendMail_GridMouseDown;
        }

        private const string NOMBRE_GENERAL = "frmAVSs2Agencia";
        private string cadenadeboletos;
        private bool banboletosrepetidos;
        private bool desactivar;
        private bool NoBoletosExtra;


        private void Carga()
        {
            //lblMoneda.Content = "";
            Globales.cpIntegraEMV.dbgEndOperation();
            Globales.strNombreFP = NOMBRE_GENERAL + ".Form_Load()";
            //  closeFrm 18

            //lbl3Digitos.Content = TypeUsuario.iata;

            cadenadeboletos = "";
            banboletosrepetidos = false;
            //UserIata.Caption = TypeUsuario.iata
            //If MDImit.PicHeader = MDImit.PicTravelClub Then
            //    Call cpIntegraEMV.dbgSetIsAgencia("1")
            //End If

            Fecha.DisplayDateStart = DateTime.Now;
            FechaR.DisplayDateStart = DateTime.Now;
            FechaR.IsEnabled = false;

            if (Globales.isAerolinea)
            { //isAerolinea Then
                lbl3Digitos.Content = TypeUsuario.iata;
                UserIata.Content = "IATA: " + TypeUsuario.iata;
                lbl3Digitos.Visibility = Visibility.Visible;
                //UserIata.Content = TypeUsuario.iata;
                //UserIata.Visibility = Visibility.Visible;
                //Label(20).Visibility = Visibility.Visible;

                //   FechaRetorno.Visibility = Visibility.Visible;
                //   Label13.Visibility = Visibility.Visible;

                FechaR.Visibility = Visibility.Visible;
                FechaR.Text = "";
                // Label8.Content = "PNR";
                numOrden.MaxLength = 6;
                //Me.Caption = "VENTA DE BOLETOS: Cargo a tarjeta bancaria vía AVS - s2"
            }
            limpia();
            fraDireccion.Visibility = Visibility.Hidden;
            if (Globales.isAerolinea)
            {
                //Fecha.Visibility = Visibility.Hidden;
                //FechaR.Visibility = Visibility.Hidden;
                //CheckT.Visibility = Visibility.Hidden;
                //CmdBoletosA.Visibility = Visibility.Hidden;
                //LabFe.Visibility = Visibility.Hidden;
                //LabFeR.Visibility = Visibility.Hidden;

            }

            fraCliente.Visibility = Visibility.Visible;

        }

        private void limpia()
        {
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

            txtNombre.Text = "";
            txtPaterno.Text = "";
            txtMaterno.Text = "";
            txtLadaPersonal.Text = "";
            txtTelefonoPersonal.Text = "";
            txtEmail.Text = "";

            

            numTdc.Text = "";
            numeroTarjeta = "";
            numeroTarjeta = "";
            numCvv.Password = "";
            nomTdc.Text = "";
            numOrden.Text = "";
            importe.Text = "";


            txtCalle.Text = "";
            txtExt.Text = "";
            txtInt.Text = "";
            txtCP.Text = "";
            cboColonia.Text = "";

            Fecha.Text = "";
            FechaR.Text = "";
            txtDelMun.Text = "";
            txtCiudad.Text = "";
            txtEstado.Text = "";
            txtPais.Text = "";
            cmdVoucher.IsEnabled = false;
        }
        private void mayuscula(object sender, RoutedEventArgs e)
        {
            TextBox texto = sender as TextBox;
            texto.Text = texto.Text.ToUpper().Trim();
        }

        private void soloNumeroDecimal(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }

        private void soloNumero1(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
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

        private void soloLetras(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void numTdcLostFocus(object sender, RoutedEventArgs e)
        {
            if (Globales.noMostrarMensajes) return;
            if (!string.IsNullOrWhiteSpace(numTdc.Text))
            {
                if (numTdc.Text.Length >= 14)
                {
                    numeroTarjeta = numTdc.Text;
                    numTdc.Text = numTdc.Text.Substring(0, 6) + "******" + numTdc.Text.Substring(numTdc.Text.Length - 4);
                }
                else
                {
                    Globales.MessageBoxMit("Favor de validar tarjeta.");
                    numTdc.Text = "";
                    numeroTarjeta = "";
                    control = sender as TextBox;
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(numTdc.Text))
            {
                Globales.cpIntegraEMV.dbgSetTipoPago(2);
                Globales.merchantMoto = Globales.cpIntegraEMV.dbgGetMerchantAvs(numeroTarjeta);
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

        private void imprimeVoucherClick(object sender, RoutedEventArgs e)
        {
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
                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        cmdVoucher.IsEnabled = true;
                        cmdNuevo.IsEnabled = true;

                        break;
                    case "3":
                        Globales.imprimirEpson();
                        break;
                    case "4":
                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        cmdVoucher.IsEnabled = true;
                        cmdNuevo.IsEnabled = true;

                        break;
                    case "6":
                        {
                            cmdNuevo.IsEnabled = false;
                            cmdVoucher.IsEnabled = false;


                            Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                            cmdVoucher.IsEnabled = true;
                            cmdNuevo.IsEnabled = true;
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
        }

        private void importeLostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
        }

        private void cmdnuevoClick(object sender, RoutedEventArgs e)
        {
            fraCliente.Visibility = Visibility.Visible;
            fraDireccion.Visibility = Visibility.Hidden;
            Fecha.Visibility = Visibility.Hidden;
            FechaR.Visibility = Visibility.Hidden;

            limpia();
            lblMoneda.Content = "";
            numTdc.Text = "";
            numeroTarjeta = "";
            numTdc.IsEnabled = true;
            numTdc.Visibility = Visibility.Visible;
            Mes.IsEnabled = true;
            Anio.IsEnabled = true;
            nomTdc.IsEnabled = true;
            // cboBanco.IsEnabled = true;
            numCvv.IsEnabled = true;

            label_6.IsEnabled = false;
            formaPago.IsEnabled = false;
            formaPago.Visibility = Visibility.Hidden;

            numOrden.IsEnabled = true;
            importe.IsEnabled = true;
            txtCalle.IsEnabled = true;
            txtInt.IsEnabled = true;
            txtCP.IsEnabled = true;
            cboColonia.IsEnabled = true;

            txtDelMun.IsEnabled = true;
            txtCiudad.IsEnabled = true;
            txtEmail.IsEnabled = true;
            txtPais.IsEnabled = true;

            limpia();

            TypeUsuario.strVoucherCoP = "";

            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;

            cmdRev.IsEnabled = true;
            cmdRevClick(null, null);

            //Código de aerolineas..

            Fecha.Text = "";
            Fecha.IsEnabled = true;
            FechaR.IsEnabled = true;
            cmdEnviar.IsEnabled = true;
            cmdEnviar.Visibility = Visibility.Visible;
            txtExt.IsEnabled = true;
            txtEstado.IsEnabled = true;
            lista_boletos = null;
            LADDBOLETOS.Content = "Agregar boletos (0)";
            BADDBOLETOS.IsEnabled = true;
            lista_boletos = null;
            LBOLETOS.Items.Clear();
            LTOTALBOLETOS.Content = "0";
        }

        private void cmdRevClick(object sender, RoutedEventArgs e)
        {
            fraCliente.Visibility = Visibility.Visible;
            fraDireccion.Visibility = Visibility.Hidden;
            if (Globales.isAerolinea)
            {
                Fecha.Visibility = Visibility.Hidden;
                FechaR.Visibility = Visibility.Hidden;
                LabFe.Visibility = Visibility.Hidden;
                LabFeR.Visibility = Visibility.Hidden;
                Fechas.Visibility = Visibility.Hidden;
            }
        }

        private void cmdFWDClick(object sender, RoutedEventArgs e)
        {
            desactivar = true;
            Globales.strNombreFP = NOMBRE_GENERAL + "CMDfwd()";
            if (string.IsNullOrWhiteSpace(numTdc.Text))
            {
                Globales.MessageBoxMit("Introduzca el número de la tarjeta");
                numTdc.Focus();
                return;
            }
            else if (Mes.SelectedIndex == -1)
            {
                Globales.MessageBoxMit("Seleccione el mes de vencimiento de la tarjeta.");
                Mes.Focus();
                return;
            }
            else if (Anio.SelectedIndex == -1)
            {
                Globales.MessageBoxMit("Seleccione el año de vencimiento de la tarjeta.");
                Anio.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(nomTdc.Text))
            {
                Globales.MessageBoxMit("Introduzca el nombre del titular.");
                nomTdc.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(numCvv.Password) && !(Globales.isAgencias || Globales.isAerolinea))
            {
                Globales.MessageBoxMit("Introduzca el Code de la tarjeta");
                numTdc.Focus();
                return;
            }
            else if ((numCvv.Password == "0000" || numCvv.Password == "000") && numCvv.Visibility == Visibility.Visible)
            {
                Globales.MessageBoxMit("Código de seguridad inválido.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(Globales.merchantMoto))
            {
                Globales.MessageBoxMit("No hay planes de pago para esta tarjeta, por favor cambie la tarjeta.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(numOrden.Text))
            {
                Globales.MessageBoxMit("Introduzca " + TypeUsuario.reference + ".");
                numOrden.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(importe.Text))
            {
                Globales.MessageBoxMit("Introduzca el importe.");
                importe.Focus();
                return;
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

                if (Convert.ToInt16(Anio.Text) < Convert.ToInt16(DateTime.Now.Year.ToString()))
                {
                    Globales.MessageBoxMit("Tarjeta vencida.");
                    return;
                }
                else if (Convert.ToInt16(Anio.Text) == Convert.ToInt16(DateTime.Now.Year.ToString()) && Convert.ToInt16(Mes.Text) < Convert.ToInt16(DateTime.Now.Month))
                {
                    Globales.MessageBoxMit("Tarjeta vencida.");
                    return;
                }
                if (numTdc.Text.Length < 15)
                {
                    Globales.MessageBoxMit("Número de tarjeta invalida, favor de reingresar.");
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
                // BADDBOLETOS.IsEnabled = true;

                Fechas.Visibility = Visibility.Visible;
                Fecha.Visibility = Visibility.Visible;
                FechaR.Visibility = Visibility.Visible;
                LabFe.Visibility = Visibility.Visible;
                LabFeR.Visibility = Visibility.Visible;
                cmdNuevo.IsEnabled = false;
                txtCalle.Focus();
            }
        }

        private void cmdEnviarClick(object sender, RoutedEventArgs e)
        {
            bool bolOperacion = false;
            string isCheckin = "";
            if (string.IsNullOrWhiteSpace(txtCalle.Text))
            {
                Globales.MessageBoxMit("Introduzca la calle del domicilio");
                //txtCalle.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtExt.Text))
            {
                Globales.MessageBoxMit("Introduzca el número exterior del domicilio.");
                //txtExt.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtCP.Text))
            {
                Globales.MessageBoxMit("Introduzca el código postal del domicilio");
                //txtCP.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(cboColonia.Text))
            {
                Globales.MessageBoxMit("Introduzca la colonia del domicilio.");
                //cboColonia.Focus();
                return;

            }
            else if (string.IsNullOrWhiteSpace(txtDelMun.Text))
            {
                Globales.MessageBoxMit("Introduzca la delegación o municipio del domicilio.");
                //txtDelMun.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtCiudad.Text))
            {
                Globales.MessageBoxMit("Introduzca la ciudad del domicilio.");
                // txtCiudad.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtEstado.Text))
            {
                Globales.MessageBoxMit("Introduzca el estado del domicilio.");
                //  txtEstado.Focus();
                return;
            }
            else
            {
                
                if(Globales.isAerolinea){
                   if(string.IsNullOrWhiteSpace(Fecha.Text)){
                       Globales.MessageBoxMit("Favor de seleccionar la fecha de salida");
                       cmdEnviar.IsEnabled = true;
                       return;
                   }
                    if(numOrden.Text.Length != 6){
                        Globales.MessageBoxMit("El PNR debe de ser de 6 dígitos.");
                        cmdEnviar.IsEnabled = true;
                        return;
                    }

                }
                    
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                boletos = string.Empty;
                foreach(var item in lista_boletos){
                    boletos += item + ",";
                }
                if(!string.IsNullOrEmpty(boletos))
                            boletos = boletos.Substring(0,boletos.Length-1);
                

                if (Globales.isAerolinea)
                {
                    if (!string.IsNullOrEmpty(boletos))
                    {
                        if (!Globales.insertaBoletoAgencia(TypeUsuario.Id_Company, "", boletos, Fecha.Text, FechaR.Text))
                        {
                            return;
                        }
                    }
                }
                else {
                    if (!string.IsNullOrEmpty(boletos))
                    {
                        if (Globales.insertaBoletoAgencia(TypeUsuario.Id_Company, "", boletos, Fecha.Text, ""))
                        {
                            return;
                        }
                    }
                }



                    cmdEnviar.IsEnabled = false;
                    //  MousePointer = vbHourglass
                    string strTypeC = string.Empty;
                    Globales.cpIntegracion_Clear();
                    if (Globales.isAmex)
                    {
                        strTypeC = "AMEX";
                    }
                    else
                    {
                        strTypeC = "V/MC";
                    }


                    Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL;
                    string strTpOperacion = string.Empty;
                    strTpOperacion = "17";



                    string FRetorno = string.Empty;
                    string FechaRetorno = string.Empty;

                    if (Globales.isAerolinea)
                    {
                        if (string.IsNullOrWhiteSpace(FechaR.Text))
                        {
                            // FechaRetorno.Text = FechaSalida.Text;
                        }
                        else
                        {
                            FechaRetorno = FechaR.Text;

                        }
                    }
                    else
                    {
                        FechaRetorno = "";
                    }

                    if ((Globales.isAgencias || Globales.isAerolinea) && string.IsNullOrWhiteSpace(numCvv.Password) && !TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
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

                    bolOperacion = Globales.cpIntegracion_cpAVSs2(TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country,
                                                        TypeUsuario.usu,
                                                        TypeUsuario.Pass,
                                                        Globales.merchantMoto,
                                                        numOrden.Text,
                                                        strTpOperacion,
                                                        strTypeC,
                                                        nomTdc.Text,
                                                        numeroTarjeta,
                                                        Mes.Text,
                                                        Anio.Text.Substring(2, 2),
                                                        numCvv.Password,
                                                        importe.Text,
                                                        lblMoneda.Content.ToString(),
                                                        txtCalle.Text,
                                                        txtInt.Text,
                                                        txtExt.Text,
                                                        txtDelMun.Text,
                                                        txtCiudad.Text,
                                                        txtEstado.Text,
                                                        txtCP.Text,
                                                        cboColonia.Text,
                                                        txtNombre.Text + " " + txtPaterno.Text + " " + txtMaterno.Text,
                                                        txtPais.Text,
                                                        txtLadaPersonal.Text + txtTelefonoPersonal.Text, txtEmail.Text, "", boletos, Fecha.Text, FechaR.Text);


                    Mouse.OverrideCursor = null;
                    string strCadEncriptar = string.Empty;

                    cmdRev.IsEnabled = false;
                    string eva = Globales.GetDataXml("response", Globales.cpIntegracion_sResult).ToLower();

                    switch (eva)
                    {
                        case "approved":
                            numTdc.IsEnabled = false;
                            Mes.IsEnabled = false;
                            Anio.IsEnabled = false;
                            nomTdc.IsEnabled = false;
                            numCvv.IsEnabled = false;
                            // cboBanco.IsEnabled = false;
                            //FormaPago.IsEnabled = false;
                            numOrden.IsEnabled = false;
                            importe.IsEnabled = false;
                            txtCalle.IsEnabled = false;
                            txtExt.IsEnabled = false;
                            txtInt.IsEnabled = false;
                            txtCP.IsEnabled = false;
                            cboColonia.IsEnabled = false;
                            BADDBOLETOS.IsEnabled = false;

                            txtDelMun.IsEnabled = false;
                            txtCiudad.IsEnabled = false;
                            txtEstado.IsEnabled = false;
                            txtPais.IsEnabled = false;

                            string mensaje =  Globales.GetDataXml("auth", Globales.cpIntegracion_sResult);
                            Globales.MessageBoxMitApproved(mensaje);
                            Fecha.IsEnabled = false;
                            FechaR.IsEnabled = false;
                            TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult).Trim();
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            switch (TypeUsuario.TipoImpresora)
                            {
                                case "6":

                                    Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                                    cmdVoucher.IsEnabled = true;
                                    cmdNuevo.IsEnabled = true;
                                    break;
                                case "1":
                                    if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult), TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                                    {
                                        TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                                    }


                                    break;
                                case "4":
                                    if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult), TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                                    {
                                        TypeUsuario.strVoucher = Globales.VoucherHtml1(Globales.cpHTTP_sResult);
                                        Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                                    }
                                    break;
                                case "3":
                                    Globales.imprimirEpson();
                                    break;
                                default:
                                    Globales.MessageBoxMit("No existe impresora, favor de escoger una en administración->Escoger_impresora");
                                    break;

                            }

                            Mouse.OverrideCursor = null;
                            if (TypeUsuario.IsAQ)
                            {
                                Globales.VerificaVoucher();

                            }

                            cmdVoucher.IsEnabled = true;
                            cmdNuevo.Visibility = Visibility.Visible;
                            cmdNuevo.IsEnabled = true;
                            cmdEnviar.Visibility = Visibility.Hidden;

                            this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;
                            this.BENVIAMAIL.Tag = this.nomTdc.Text;

                            if (Globales.FacturaE == "1")
                            {
                                if (Globales.MessageConfirm("¿Desea factura electrónica?"))
                                {
                                    frmPreguntaFactura pregunta = new frmPreguntaFactura();
                                    pregunta.abrirFrmAhora = abrir;
                                    pregunta.cerraPage = cerrar;
                                    abrir(pregunta);
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
                            //  cboBanco.IsEnabled = false;
                            // FormaPago.IsEnabled = false;
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
                            Globales.MessageBoxMitDenied("Cobro rechazado" + Globales.msjRech + "\n" + Globales.GetDataXml("cd_response", Globales.cpIntegracion_sResult) + " " + Globales.GetDataXml("friendly_response", Globales.cpIntegracion_sResult));
                            cmdNuevo.Visibility = Visibility.Visible;
                            cmdEnviar.Visibility = Visibility.Hidden;
                            cmdNuevo.IsEnabled = true;
                            BADDBOLETOS.IsEnabled = false;
                            
                            this.BENVIAMAIL.Visibility = Visibility.Hidden;
                            this.BENVIAMAIL.Tag = string.Empty;


                            // FechaSalida.IsEnabled = false;
                            Fecha.IsEnabled = false;
                            //FechaRetorno.IsEnabled = false;
                            FechaR.IsEnabled = false;
                            break;
                        case "error":
                            Globales.MessageBoxMitError(Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult));
                            cmdNuevo.Visibility = Visibility.Hidden;
                            cmdEnviar.Visibility = Visibility.Visible;
                            cmdRev.IsEnabled = true;
                             this.BENVIAMAIL.Visibility = Visibility.Hidden;
                            this.BENVIAMAIL.Tag = string.Empty;
                            break;
                        default:
                            Globales.MessageBoxMitError("Verifique su conexión de Internet.");
                            cmdNuevo.Visibility = Visibility.Hidden;
                            cmdEnviar.Visibility = Visibility.Visible;
                            cmdRev.IsEnabled = true;
                             this.BENVIAMAIL.Visibility = Visibility.Hidden;
                            this.BENVIAMAIL.Tag = string.Empty;
                            break;
                    }



                
            }
            Mouse.OverrideCursor = null;
            cmdEnviar.IsEnabled = true;

        }

        public cerrarVentana cerrar;











    
        



        private bool ChecaBoletoRepetido(TextBox t)
        {

            UIElementCollection cajas = BoletosExtra.Children;
            try
            {
                foreach (Object item in cajas)
                {
                    if (item.GetType().ToString().Equals("System.Windows.Controls.Border"))
                    {
                        Border borde = (Border)item;
                        Grid ob = (Grid)borde.Child;
                        UIElementCollection hijos = ob.Children;
                        foreach (Object items in hijos)
                        {
                            if (items.GetType().ToString().Equals("System.Windows.Controls.TextBox"))
                            {
                                TextBox texto = (TextBox)items;
                                if (!string.IsNullOrWhiteSpace(t.Text))
                                {
                                    if (t.Text == texto.Text && texto.Name != t.Name)
                                    {

                                        Globales.MessageBoxMit("El No. de boleto " + (t.Text) + " esta repetido en el campo " + texto.Name.Substring(11));
                                        //t.Focus();
                                        banboletosrepetidos = true;
                                        return true;
                                    }
                                }
                                else
                                {
                                    banboletosrepetidos = false;
                                }
                            }
                        }
                    }
                }
            }

            catch { }
            return banboletosrepetidos;
        }


        private bool TicketCheckPNR(string cadena)
        {
            bool TicketCheckPNR = false;
            if (!string.IsNullOrWhiteSpace(cadena) && (Globales.isAgencias || Globales.isAerolinea))
            {
                if (cadena.Length < 10)
                {
                    Globales.MessageBoxMit("¡La longitud del boleto es incorrecta!");
                    TicketCheckPNR = true;
                }
            }
            return TicketCheckPNR;
        }
        private void importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void CheckT_Click(object sender, RoutedEventArgs e)
        {
        }

        private void VerificaBoleto(object sender, RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (Globales.isAerolinea)
            {
                if (TicketCheckPNR(txt.Text))
                {
                    return;
                }
                if (!banboletosrepetidos)
                {
                    ChecaBoletoRepetido(txt);
                }
                else
                {
                    banboletosrepetidos = false;
                }
            }
            else
            {
                TicketCheckPNR(txt.Text);
                if (!banboletosrepetidos)
                {
                    ChecaBoletoRepetido(txt);
                }
                else
                {
                    banboletosrepetidos = false;
                }
            }
        }

        private void Fecha_CalendarClosed(object sender, RoutedEventArgs e)
        {

        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }


        public string numeroTarjeta { get; set; }

        private void numTdc_GotFocus(object sender, RoutedEventArgs e)
        {
            numeroTarjeta = (string.IsNullOrWhiteSpace(numeroTarjeta)) ? "" : numeroTarjeta;
            numTdc.Text = numeroTarjeta;
        }

        private void nomTdc_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTexto(sender, e);
        }
        private bool isCvv = false;
        private Control control = null;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            fraDireccion.Visibility = Visibility.Hidden;
            fraCliente.Visibility = Visibility.Visible;
            Fechas.Visibility = Visibility.Hidden;
            BoletosExtra.Visibility = Visibility.Hidden;
            label_8.Content = TypeUsuario.reference;
            if(Globales.isAerolinea){
                label_8.Content = "PNR";
            }


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
            fraDireccion.Visibility = Visibility.Hidden;
            Fechas.Visibility = Visibility.Hidden;
            if (this.lista_boletos == null)
            {
                this.lista_boletos = new List<string>();

            }
        }

        public List<string> lista_boletos { get; set; }

        private void TNUMBOLETO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                BAGREGABOLETO_Click(sender, e);
        }


        private void Command2_Click(object sender, RoutedEventArgs e)
        {
            this.TNUMBOLETO.Text = string.Empty;


            if (this.lista_boletos == null)
                this.lista_boletos = new List<string>();

            if (!banboletosrepetidos)
            {
                BoletosExtra.Visibility = Visibility.Hidden;
                fraDireccion.Visibility = Visibility.Visible;
                Fechas.Visibility = Visibility.Visible;
                this.LTOTALBOLETOS.Content = this.lista_boletos.Count;
                this.LADDBOLETOS.Content = string.Format("Agregar boletos({0})", this.lista_boletos.Count);
            }
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

        private void numeroytexto(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

        private void letraytexto(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

        private void numeros(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }

        private void antes(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.numeroTarjeta))
            {
                Globales.MessageBoxMit("Ingrese número de tarjeta.");
                e.Handled = true;
            }
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

        private void evitar(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void cambiaFecha(object sender, SelectionChangedEventArgs e)
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

        private void codigoPostal(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender,e);
        }

        private void lostfocus_cvv(object sender, RoutedEventArgs e)
        {
            if (Globales.noMostrarMensajes) return;
            if (Globales.isAerolinea && string.IsNullOrWhiteSpace(numCvv.Password))
            {
                return;
            }
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
            else
            {
                Globales.MessageBoxMit("El cvv es incorrecto, favor de validar");
                isCvv = true;
                control = sender as PasswordBox;

            }            
        }

        private void losfocus_contrato(object sender, RoutedEventArgs e)
        {
            if (Globales.noMostrarMensajes) return;

            if(Globales.isAerolinea && !string.IsNullOrWhiteSpace(numOrden.Text) && numOrden.Text.Length != 6){
                Globales.MessageBoxMit("El PNR debe tener 6 dígitos");
                control = sender as TextBox;
            }
        }
    }
}
