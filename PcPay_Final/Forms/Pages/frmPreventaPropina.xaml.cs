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

using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using PcPay.Forms.Formularios;
using System.Text.RegularExpressions;
namespace PcPay.Forms.Pages
{
    /// <summary>
    /// Lógica de interacción para frmPreventaPropina.xaml
    /// </summary>
    public partial class frmPreventaPropina : Page
    {
        public abrirFrm abrir;
        public cerrarVentana cierra;
        public frmMenuPrincipal menuPrincipal { get; set; }
        public string VoucherTrx { get; set; }
        public double propina_total { get; set; }
        public string strNumOpe { get; set; }
        public double importe { get; set; }

        public frmPreventaPropina()
        {
            InitializeComponent();
            this.OPT5.Click += new RoutedEventHandler(this.OPT0_Click);
            this.OPT10.Click += new RoutedEventHandler(this.OPT0_Click);
            this.OPT15.Click += new RoutedEventHandler(this.OPT0_Click);
            this.OPT20.Click += new RoutedEventHandler(this.OPT0_Click);
            this.OPTOTRO.Click += new RoutedEventHandler(this.OPT0_Click);

            this.TITEMCOBRO.Visibility = Visibility.Hidden;
            this.TITEMPROPINA.Visibility = Visibility.Hidden;
            this.TFECHAVENC.Text = string.Empty;

            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;
            this.BENVIAMAIL.MouseDown += Globales.sendMail_GridMouseDown;

            if (!Globales.cpIntegraEMV.dbgSetReader())
                Globales.MessageBoxMit("Ocurrión un error al iniciar el lector");
            else
            {
                if (TypeUsuario.ShowMsg)
                    new frmAvisoBanda().ShowDialog();
                Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_VTASERV);
                Globales.obtieneBancos(this.CBANCO, Globales.GetDataXml("catbancos", TypeUsuario.CadenaXML));
            }

            #region Estilos
            TIMPORTE.GotFocus += Globales.setFocusMit2;
            TIMPORTE.LostFocus += Globales.lostFocusMit2;

            TREFERENCIA.GotFocus += Globales.setFocusMit2;
            TMESERO.GotFocus += Globales.setFocusMit2;
            TMESA.GotFocus += Globales.setFocusMit2;
            TTURNO.GotFocus += Globales.setFocusMit2;

            TREFERENCIA.LostFocus += Globales.lostFocusMit2;
            TMESERO.LostFocus += Globales.lostFocusMit2;
            TMESA.LostFocus += Globales.lostFocusMit2;
            TTURNO.LostFocus += Globales.lostFocusMit2;

            #endregion
        }

        #region"Formulario de propina"

        private void TIMPORTE_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter || e.Key == Key.Tab)
            //{
            //    if (string.IsNullOrWhiteSpace(this.TIMPORTE.Text))
            //        Globales.MessageBoxMit("Ingrese el importe");
            //    else
            //    {
            //        this.TIMPORTE.Text = this.TIMPORTE.Text.Replace(" ", "");
            //        if (this.TIMPORTE.Text.Length == 1 && this.TIMPORTE.Text.Contains("."))
            //            this.TIMPORTE.Text = "00.00";
            //        double importe = double.Parse(this.TIMPORTE.Text);
            //        this.TIMPORTE.Text = Globales.importe(Convert.ToString(importe));

            //        if (importe > 0)
            //        {
            //            this.GPROPINA.IsEnabled = true;
            //            this.TTOTAL.Text = this.TIMPORTE.Text;
            //            this.BSIGUIENTE.Focus();
            //        }
            //        else
            //        {
            //            Globales.MessageBoxMit("El importe no es válido.");
            //            this.TIMPORTE.Text = string.Empty;
            //        }
            //    }
            //}
            //else if (e.Key == Key.OemPeriod || e.Key == Key.Decimal)
            //{
            //    if (this.TIMPORTE.Text.Contains("."))
            //        e.Handled = true;
            //}
            //else if (!Globales.NUMBERKEYS.Exists(o => o == e.Key))
            //    e.Handled = true;
        }
        private void TIMPORTE_TextChanged(object sender, TextChangedEventArgs e)
        {
            //this.TOTRO.Text = string.Empty;
            //this.TTOTAL.Text = string.Empty;
            //this.OPT0.IsChecked = true;
            //this.GPROPINA.IsEnabled = false;
        }
        private void TIMPORTE_GotFocus(object sender, RoutedEventArgs e)
        {
            this.TIMPORTE.SelectAll();
        }
        private void OPT0_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TIMPORTE.Text))
            {
                OPT0.IsChecked = true;
                Globales.MessageBoxMit("Ingrese un importe");
                TOTRO.IsEnabled = false;
                TIMPORTE.Focus();
                return;
            }
            string name = ((RadioButton)sender).Name;
            double total = double.Parse(this.TIMPORTE.Text);
            this.TOTRO.Text = string.Empty;
            this.TOTRO.IsEnabled = false;
            switch (name)
            {
                case "OPT0":
                    if ((bool)this.OPT0.IsChecked)
                        this.TTOTAL.Text = this.TIMPORTE.Text;
                    break;
                case "OPT5":
                    if ((bool)this.OPT5.IsChecked)
                        this.TTOTAL.Text = Globales.importe(Convert.ToString(Math.Round(total + (total * .05), 2)));

                    break;
                case "OPT10":
                    if ((bool)this.OPT10.IsChecked)
                        this.TTOTAL.Text = Globales.importe(Convert.ToString(Math.Round(total + (total * .10), 2)));
                    break;
                case "OPT15":
                    if ((bool)this.OPT15.IsChecked)
                        this.TTOTAL.Text = Globales.importe(Convert.ToString(Math.Round(total + (total * .15), 2)));
                    break;
                case "OPT20":
                    if ((bool)this.OPT20.IsChecked)
                        this.TTOTAL.Text = Globales.importe(Convert.ToString(Math.Round(total + (total * .20), 2)));
                    break;
                case "OPTOTRO":
                    if ((bool)this.OPTOTRO.IsChecked)
                    {
                        this.TTOTAL.Text = string.Empty;
                        this.TOTRO.IsEnabled = true;
                        this.TOTRO.Focus();
                    }
                    break;
            }
            if (name != "OPTOTRO")
                this.BSIGUIENTE.Focus();

        }
        private void validaPropina()
        {
            if (string.IsNullOrWhiteSpace(this.TOTRO.Text))
                this.TOTRO.Text = "00.00";

            this.TOTRO.Text = this.TOTRO.Text.Replace(" ", "");
            if (this.TOTRO.Text.Length == 1 && this.TOTRO.Text.Contains("."))
                this.TOTRO.Text = "00.00";

            double importe = double.Parse(this.TIMPORTE.Text);
            double propina = double.Parse(this.TOTRO.Text);

            this.TOTRO.Text = Globales.importe(Convert.ToString(propina));
            this.TTOTAL.Text = Globales.importe(Convert.ToString(Math.Round((importe + propina), 2)));
        }
        private void BSIGUIENTE_Click(object sender, RoutedEventArgs e)
        {
            BCOBRAR.Visibility = Visibility.Visible;
            BVOUCHER.Visibility = Visibility.Hidden;

            if (string.IsNullOrWhiteSpace(TIMPORTE.Text))
            {
                Globales.MessageBoxMit("Ingrese un importe");
                TIMPORTE.Focus();
                return;
            }
            TREFERENCIA.Focus();
            double max = 0.0;
            string propMax = Globales.GetDataXml("pcPropinaMax", TypeUsuario.CadenaXML);
            this.importe = double.Parse(this.TIMPORTE.Text);
            double importe_mas_propina = double.Parse(this.TTOTAL.Text);

            if (propMax != "LIBRE" && propMax != "")
            {
                double dPropmax = double.Parse(propMax);
                max = Math.Round(double.Parse(this.TIMPORTE.Text) * (dPropmax / 100));
                if (importe_mas_propina > max)
                {
                    Globales.MessageBoxMit(string.Format("El máximo de propina permitido es del {0} por favor verifique y vuelva a intentar", propMax));
                }
            }
            if (string.IsNullOrWhiteSpace(this.TTOTAL.Text))
                Globales.MessageBoxMit("El total de importe no es válido.");
            else
            {
                this.TCBODY.SelectedItem = this.TITEMCOBRO;
                this.TTOTALPAGO.Text = this.TTOTAL.Text;
                this.TTOTALPAGO.Focus();
                this.propina_total = Math.Round(importe_mas_propina - this.importe, 2);
                //BSIGUIENTE.Visibility = Visibility.Hidden;
                TREFERENCIA.Focus();
                //BVOUCHER.Visibility = Visibility.Visible;
            }

        }
        private void TOTRO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                this.validaPropina();
                this.BSIGUIENTE.Focus();
            }
            else if (e.Key == Key.OemPeriod)
            {
                if (this.TOTRO.Text.Contains("."))
                    e.Handled = true;
            }
            else if (!Globales.NUMBERKEYS.Exists(o => o == e.Key))
                e.Handled = true;
        }
        private void TOTRO_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((bool)this.OPTOTRO.IsChecked)
            {
                this.validaPropina();
                this.BSIGUIENTE.Focus();
            }
        }


        #endregion

        #region"Formulario de tarjeta"
        private void BACTIVARLECTOR_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TREFERENCIA.Text))
            {
                Globales.MessageBoxMit("Introduza la referencia");
                this.TREFERENCIA.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(this.TMESERO.Text))
            {
                Globales.MessageBoxMit("Introduzca el mesero.");
                this.TMESERO.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(this.TTURNO.Text))
            {
                Globales.MessageBoxMit("Introduza el turno.");
                this.TTURNO.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(this.TMESA.Text))
            {
                Globales.MessageBoxMit("Introduzca la mesa.");
                this.TMESA.Focus();
                return;
            }

            this.TREFERENCIA.IsEnabled = false;
            this.TMESERO.IsEnabled = false;
            this.TTURNO.IsEnabled = false;
            this.TMESA.IsEnabled = false;

            this.BACTIVARLECTOR.IsEnabled = false;
            this.cmdAtras.IsEnabled = false;

            this.TNOTARJETA.Text = string.Empty;
            this.TMES.Text = string.Empty;
            this.TANIO.Text = string.Empty;
            this.TNOMTARJETAHAB.Text = string.Empty;

            double pagoTotal = double.Parse(this.TTOTALPAGO.Text);
            double importePreventa = 0.0;
            string incremento = string.Empty;
            incremento = Globales.GetDataXml("incremento_propina", TypeUsuario.CadenaXML);
            if (string.IsNullOrWhiteSpace(incremento))
                importePreventa = pagoTotal;
            else
                importePreventa = pagoTotal + (pagoTotal * (double.Parse(incremento) / 100));
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globales.cpIntegraEMV.dbgStartTxEMV(this.importe.ToString());
            BACTIVARLECTOR.IsEnabled = false;
            if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()) && !string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkCc_Number()))
            {
                Mouse.OverrideCursor = null;
                this.TNOTARJETA.Text = Globales.cpIntegraEMV.chkCc_Number();
                this.TNOMTARJETAHAB.Text = Globales.cpIntegraEMV.chkCc_Name();
                this.TMES.Text = Globales.cpIntegraEMV.chkCc_ExpMonth();
                this.TANIO.Text = Globales.cpIntegraEMV.chkCc_ExpYear();
                this.TFECHAVENC.Text = TMES.Text + "/" + TANIO.Text;

                Globales.merchantBanda = Globales.cpIntegraEMV.dbgGetMerchantBanda("9");

                if (Globales.merchantBanda != "00000")
                {
                    Globales.isAmex = Globales.cpIntegraEMV.dbgGetisAmex();
                    if (Globales.GetDataXml("MXN", TypeUsuario.CadenaXML).Contains(Globales.merchantBanda))
                        this.LMONEDA.Content = "MXN";
                    else
                        this.LMONEDA.Content = "USD";

                    this.BACTIVARLECTOR.IsEnabled = false;
                    this.BCOBRAR.IsEnabled = true;

                    //this.TBINFO.Text = "Inserta el chip o desliza tarjeta y Espera un momento...\nSigue las instrucciones del lector.";
                    //this.TBINFO.Foreground = Brushes.Blue;

                    if (!Globales.cpIntegraEMV.dbgTxMonitor())
                    {
                        Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspDsError());
                        this.BACTIVARLECTOR.IsEnabled = true;

                        this.BCOBRAR.IsEnabled = false;
                        this.TNOTARJETA.Text = string.Empty;
                        this.TNOMTARJETAHAB.Text = string.Empty;
                        this.TMES.Text = string.Empty;
                        this.TANIO.Text = string.Empty;
                        this.TFECHAVENC.Text = string.Empty;


                        Globales.MessageBoxMitError(Globales.cpIntegraEMV.chkPp_DsError());
                        //this.TBINFO.Foreground = Brushes.Red;
                    }
                }
                else
                {
                    Globales.MessageBoxMit(Globales.cpIntegraEMV.dbgGetRspError());
                    this.LMONEDA.Content = string.Empty;
                    this.VoucherTrx = string.Empty;
                    ///Verificar formulario de plan de pagos
                }
            }
            else
            {
                BNUEVO.IsEnabled = true;
                if (!string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()))
                {
                    Globales.MessageBoxMitError(Globales.cpIntegraEMV.chkPp_DsError());

                }
            }
            Mouse.OverrideCursor = null;
        }
        private void TREFERENCIA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrWhiteSpace(this.TREFERENCIA.Text))
                {
                    Globales.MessageBoxMit("Introduza la referencia");
                    this.TREFERENCIA.Text = string.Empty;
                }
                else
                {
                    this.TMESERO.Focus();
                    this.TREFERENCIA.Text = this.TREFERENCIA.Text.ToUpper();
                }
            }
        }
        private void EventoTexto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string nombre = ((TextBox)sender).Name;
                switch (nombre)
                {
                    case "TMESERO":
                        if (string.IsNullOrWhiteSpace(this.TMESERO.Text))
                            this.TMESERO.Text = "0";

                        this.TTURNO.Focus();

                        break;
                    case "TTURNO":
                        if (string.IsNullOrWhiteSpace(this.TTURNO.Text))
                            this.TTURNO.Text = "0";
                        this.TMESA.Focus();

                        break;
                    case "TMESA":
                        if (string.IsNullOrWhiteSpace(this.TMESA.Text))
                            this.TMESA.Text = "0";
                        this.BACTIVARLECTOR.Focus();

                        break;
                }
            }
            else if (!Globales.NUMBERKEYS.Exists(o => o == e.Key))
                e.Handled = true;
        }
        private void BCOBRAR_Click(object sender, RoutedEventArgs e)
        {
            double max = 0.0;
            string propMax = Globales.GetDataXml("pcPropinaMax", TypeUsuario.CadenaXML);

            if (string.IsNullOrWhiteSpace(Globales.merchantBanda))
            {
                Globales.MessageBoxMit("No hay planes de pago para esta tarjeta, por favor cambie la tarjeta.");
                this.BCOBRAR.IsEnabled = true;
                return;
            }
            this.BCOBRAR.IsEnabled = false;
            if (propMax != "LIBRE" && propMax != "")
            {
                double dPropmax = double.Parse(propMax);
                max = Math.Round(double.Parse(this.TIMPORTE.Text) * (dPropmax / 100));
                if (double.Parse(this.TTOTAL.Text) > max)
                {
                    Globales.MessageBoxMit(string.Format("El máximo de propina permitido es del {0} por favor verifique y vuelva a intentar", propMax));
                    this.BCOBRAR.IsEnabled = true;
                }
                else
                {
                    this.enviar();
                    this.propina_total = 0;
                    this.importe = 0.0;
                    this.strNumOpe = string.Empty;
                }
            }
            else
            {
                this.enviar();
                this.propina_total = 0;
                this.importe = 0.0;
                this.strNumOpe = string.Empty;
            }
        }
        private void BACTIVARLECTOR_MouseEnter(object sender, MouseEventArgs e)
        {
            Button isEnabledButton = ((Button)sender);
            if (isEnabledButton.IsEnabled)
                isEnabledButton.Cursor = Cursors.Hand;
            else
                isEnabledButton.Cursor = Cursors.No;
        }
        private void BVOUCHER_Click(object sender, RoutedEventArgs e)
        {
            Globales.PrintOptions(this.VoucherTrx, this.strNumOpe);
        }

        private void BNUEVO_Click(object sender, RoutedEventArgs e)
        {
            this.TCBODY.SelectedItem = this.TITEMPROPINA;
            this.defaultValues();
            this.menuPrincipal.menu2VentaConPresenciaConsumo();
            this.BVOUCHER.Visibility = Visibility.Hidden;
        }
        public void enviar()
        {

            string strTypeC = string.Empty;
            string mensaje = string.Empty;

            Globales.cpIntegracion_Clear();

            if (Globales.isAmex)
                strTypeC = "AMEX";
            else
                strTypeC = "V/MC";


            string _csvAmexenBanda = Globales.csvAmexenBanda;
            string _mesa = this.TMESA.Text;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            _csvAmexenBanda = (string.IsNullOrWhiteSpace(_csvAmexenBanda)) ? "" : _csvAmexenBanda;
            Globales.cpIntegraEMV.dbgSetCurrencyType(this.LMONEDA.Content.ToString());
            Globales.cpIntegraEMV.sndPreventaPropinaEMV(TypeUsuario.usu, TypeUsuario.Pass, string.Empty, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country
                , strTypeC, Globales.merchantBanda, this.TREFERENCIA.Text, "9", this.LMONEDA.Content.ToString(), this.TMESERO.Text, this.TTURNO.Text, this.propina_total.ToString(), _csvAmexenBanda
                , _mesa);
            string temporal = Globales.cpIntegraEMV.getRspXML();
            Mouse.OverrideCursor = null;
            Globales.csvAmexenBanda = string.Empty;
            string respuesta = Globales.cpIntegraEMV.getRspDsResponse();

            switch (respuesta)
            {

                case "approved":
                    
                    mensaje = string.Format(Globales.cpIntegraEMV.getRspAuth());
                    Globales.MessageBoxMitApproved(mensaje);
                    this.BNUEVO.IsEnabled = true;
                    this.strNumOpe = Globales.cpIntegraEMV.getRspOperationNumber();
                    this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;
                    this.BENVIAMAIL.Tag = this.TNOMTARJETAHAB.Text;
                    if (Globales.cpIntegraEMV.dbgGetEsImprimibleVoucher() == "0")
                    {
                        this.BVOUCHER.IsEnabled = false;
                        this.finaliza();
                        return;
                    }
                    else
                        this.BVOUCHER.IsEnabled = true;

                    string textoAgua = "";
                    textoAgua = "Folio: " + Globales.cpIntegraEMV.getRspOperationNumber() + "\n";
                    textoAgua += "Auth: " + Globales.cpIntegraEMV.getRspAuth() + "\n";
                    textoAgua += "Importe: " + Globales.cpIntegraEMV.getTx_Amount() + "\n";
                    textoAgua += "Fecha: " + Globales.cpIntegraEMV.getRspDate() + "\n";
                    textoAgua += "Merchant: " + Globales.cpIntegraEMV.getRspDsMerchant() + "\n";
                    textoAgua += " " + "\n";
                    textoAgua += "_____________" + "\n";
                    textoAgua += "FIRMA DIGITALIZADA" + "\n";


                    //Valida si la tarjeta es Chip y Pin
                    bool IsChipAndPin = false, esQPS = false;
                    string cadenaHexFirma = string.Empty;
                    short tipoVta = 1;

                    //Valida si es chipp and pin
                    if (Globales.cpIntegraEMV.chkCc_IsPin() == "01")
                        IsChipAndPin = true;

                    //Valida si la venta es QPS

                    if (Globales.cpIntegraEMV.getRspVoucher().Contains("@cnn Autorizado sin Firma ") && !IsChipAndPin && tipoVta == 1)
                        esQPS = true;

                    if (!Globales.cpIntegraEMV.EsTouch() && !Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                    { this.GoImpresion(); return; }

                    if (!Globales.cpIntegraEMV.EsTouch() && Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                    {
                        cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(false, textoAgua, Globales.cpIntegraEMV.getRspVoucher()
                            , IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), tipoVta, esQPS);
                        if (!cadenaHexFirma.Contains("error"))
                        {
                            if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Globales.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                            {
                                imgEmailFirmaPanel.Visibility = Visibility.Visible;
                            }
                        }
                        else
                            Globales.MessageBoxMit(string.Format("No se pudo obtener la imagen del PinPad. '\n{0}", cadenaHexFirma));
                        this.GoImpresion();
                        return;
                    }

                    if (Globales.cpIntegraEMV.EsTouch())
                    {
                        if (TypeUsuario.UtilizarCapacidadTouch == true && TypeUsuario.EnviarVoucherMail == true)
                            if (!Globales.ObtieneFirmaPanel(Globales.ipFirmaPanel, textoAgua, tipoVta, IsChipAndPin, esQPS))//MASS07062017 CORREGIR
                            {
                                this.GoImpresion();
                                return;
                            }
                            else
                            {
                                this.finaliza();
                                return;
                            }
                        else if (TypeUsuario.UtilizarCapacidadTouch == true && TypeUsuario.EnviarVoucherMail == false)
                        {
                            cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(true, textoAgua, Globales.cpIntegraEMV.getRspVoucher()
                                , IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), tipoVta, esQPS);
                            if (!cadenaHexFirma.Contains("error"))
                            {
                                if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Globales.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                {
                                    imgEmailFirmaPanel.Visibility = Visibility.Visible;
                                }
                            }
                            else
                                Globales.MessageBoxMit(string.Format("No se pudo obtener la imagen del PinPad. '\n{0}", cadenaHexFirma));
                            this.GoImpresion();
                            return;
                        }
                        else if (TypeUsuario.UtilizarCapacidadTouch == false && TypeUsuario.EnviarVoucherMail == false)
                        {
                            if (Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                            {
                                cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(false, textoAgua, Globales.cpIntegraEMV.getRspVoucher()
                                , IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), tipoVta, esQPS);
                                if (!cadenaHexFirma.Contains("error"))
                                {
                                    if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Globales.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                    {
                                        imgEmailFirmaPanel.Visibility = Visibility.Visible;
                                    }
                                }
                                else
                                    Globales.MessageBoxMit(string.Format("No se pudo obtener la imagen del PinPad. '\n{0}", cadenaHexFirma));
                            }
                            this.GoImpresion();
                        }
                    }
                    this.BENVIAMAIL.Visibility = TypeUsuario.enviaCorreo ? Visibility.Visible : Visibility.Hidden;
                    this.BENVIAMAIL.Tag = this.TNOMTARJETAHAB.Text;

                    break;
                case "denied":
                    mensaje = string.Format("{0}\n{1} {2}", Globales.msjRech, Globales.cpIntegraEMV.getRspCdResponse(), Globales.cpIntegraEMV.getRspFriendlyResponse());
                    Globales.MessageBoxMitDenied(mensaje);
                    this.HabilitaBotonNuevo();
                    break;
                case "error":
                    mensaje = Globales.cpIntegraEMV.getRspDsError();
                    Globales.MessageBoxMitError(mensaje);
                    this.HabilitaBotonNuevo();
                    break;
                default:
                    mensaje = "Verifique su conexión a internet.";
                    Globales.MessageBoxMitError(mensaje);
                    this.HabilitaBotonNuevo();
                    break;
            }
        }
        public void GoImpresion()
        {

            this.VoucherTrx = Globales.cpIntegraEMV.getRspVoucher();
            TypeUsuario.strVoucherCoP = VoucherTrx;
            Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();
            escogerImpresora();
            BCOBRAR.Visibility = Visibility.Hidden;
            this.BVOUCHER.IsEnabled = true;
            this.BVOUCHER.Visibility = Visibility.Visible;
            this.finaliza();
        }
        private void escogerImpresora()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                TypeUsuario.strVoucherCoP = Globales.cpIntegraEMV.getRspVoucher(); 
                switch (TypeUsuario.TipoImpresora)
                {
                    case "1":
                        if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult),
                                                      TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                        {
                            TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                            Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        }
                        //  cmdVoucher.IsEnabled = true;
                        //  cmdNuevo.IsEnabled = true;
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
                        break;
                    case "5":
                        break;
                    case "6":
                        BVOUCHER.IsEnabled = false;
                        BNUEVO.IsEnabled = false;
                        Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                        BVOUCHER.IsEnabled = true;
                        BNUEVO.IsEnabled = true;
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
        public void finaliza()
        {
            Globales.cpIntegracion_sResult = string.Format("{0}<propina>{1}</propina>", Globales.cpIntegraEMV.getRspXML(), this.propina_total);
            string monto = string.Format("<amount>{0}</amount>", this.importe);
            string tempBusqueda = string.Format("<amount>{0}</amount>", Globales.GetDataXml("amount", Globales.cpIntegracion_sResult));

            Globales.cpIntegracion_sResult = Globales.cpIntegracion_sResult.Replace(tempBusqueda, monto);

            if (Globales.FacturaE == "1")
            {
                if (Globales.MessageConfirm("¿Desea factura electronica?"))
                {
                    frmPreguntaFactura fFactura = new frmPreguntaFactura();
                    fFactura.abrirFrmAhora = abrir;
                    fFactura.cerraPage = cierra;
                    abrir(fFactura);
                }
                else
                    Globales.XMLFacturaE = string.Empty;
            }
        }
        public void HabilitaBotonNuevo()
        {
            this.BCOBRAR.IsEnabled = false;
            this.BACTIVARLECTOR.IsEnabled = false;
            this.BNUEVO.IsEnabled = true;
            this.BVOUCHER.IsEnabled = false;
            this.BENVIAMAIL.Visibility = Visibility.Hidden;
            this.BENVIAMAIL.Tag = string.Empty;
        }
        public void defaultValues()
        {
            this.propina_total = 0;
            this.importe = 0;

            this.LMONEDA.Content = string.Empty;
            this.VoucherTrx = string.Empty;

            this.TREFERENCIA.Text = string.Empty;
            this.TREFERENCIA.IsEnabled = true;

            this.TMES.Text = string.Empty;
            this.TMES.IsEnabled = true;

            this.TANIO.Text = string.Empty;
            this.TANIO.IsEnabled = true;

            this.TFECHAVENC.Text = string.Empty;

            this.TMESA.Text = string.Empty;
            this.TMESA.IsEnabled = true;

            this.TMESERO.Text = string.Empty;
            this.TMESERO.IsEnabled = true;

            this.TTURNO.Text = string.Empty;
            this.TTURNO.IsEnabled = true;

            this.TTOTALPAGO.Text = string.Empty;
            this.TNOMTARJETAHAB.Text = string.Empty;
            this.TNOTARJETA.Text = string.Empty;

            this.BACTIVARLECTOR.IsEnabled = true;
            this.BCOBRAR.IsEnabled = false;
            this.BVOUCHER.IsEnabled = false;
            this.BNUEVO.IsEnabled = false;
        }


        #endregion

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void Importe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir(true);
        }

        private void TOTRO_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TIMPORTE.Focus();
            BVOUCHER.Visibility = Visibility.Hidden;
            imgEmailFirmaPanel.Visibility = Visibility.Hidden;
            LREFERENCIA.Content = TypeUsuario.reference;
        }

        private void cmdAtras_Click(object sender, RoutedEventArgs e)
        {
            this.TCBODY.SelectedIndex = 0;

        }

        private void TMESERO_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void letraNumero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

        private void TIMPORTE_PreviewKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void imprimir(object sender, RoutedEventArgs e)
        {
            escogerImpresora();
        }

        private void OPTOTRO_Click(object sender, RoutedEventArgs e)
        {
            TOTRO.IsEnabled = true;
        }

        private void OPT20_Click(object sender, RoutedEventArgs e)
        {
            TOTRO.IsEnabled = false;
        }

        private void TREFERENCIA_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TypeUsuario.Id_Company == "0059" && TREFERENCIA.IsEnabled)
            {
                TypeUsuario.strRefReaDig = "MA";
                frmReaderDigest reader = new frmReaderDigest();
                reader.ShowDialog();
                TREFERENCIA.Text = Globales.referenciaAux;
            }
        }

        private void imgEmailFirmaPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Globales.sendMailFirmaPanel_MouseDown(sender, e);
        }

        private void lostFocusImporte(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender,e);
            if(string.IsNullOrWhiteSpace(TIMPORTE.Text)){
                TTOTAL.Text = "";
                TOTRO.Text = "";
                OPT0.IsChecked = true;
                return;
            }
            TTOTAL.Text = TIMPORTE.Text;
            if (!Convert.ToBoolean(OPT0.IsChecked))
            {
               if(Convert.ToBoolean(OPT5.IsChecked)){
                   TTOTAL.Text = Globales.importe(Convert.ToString(Convert.ToDouble(TIMPORTE.Text)+(Convert.ToDouble(TIMPORTE.Text) * .05)));
               }
               else if (Convert.ToBoolean(OPT10.IsChecked))
               {

                   TTOTAL.Text =  Globales.importe(Convert.ToString(Convert.ToDouble(TIMPORTE.Text) + (Convert.ToDouble(TIMPORTE.Text) * .10)));
               }
               else if (Convert.ToBoolean(OPT15.IsChecked))
               {
                   TTOTAL.Text =  Globales.importe(Convert.ToString(Convert.ToDouble(TIMPORTE.Text) + (Convert.ToDouble(TIMPORTE.Text) * .15)));

               }
               else if (Convert.ToBoolean(OPT20.IsChecked))
               {
                   TTOTAL.Text =  Globales.importe(Convert.ToString(Convert.ToDouble(TIMPORTE.Text) + (Convert.ToDouble(TIMPORTE.Text) * .20)));

               } if (Convert.ToBoolean(OPTOTRO.IsChecked))
               {
                   if (!string.IsNullOrWhiteSpace(TOTRO.Text))
                   {

                       TTOTAL.Text = Globales.importe(Convert.ToString(Convert.ToDouble(TIMPORTE.Text) + Convert.ToDouble(TOTRO.Text)));
                   }
               }
            }
        }

        private void TTOTALPAGO_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
