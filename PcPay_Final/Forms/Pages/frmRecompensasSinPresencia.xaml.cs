using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using PcPay.Forms.Formularios;
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
    /// Lógica de interacción para frmRecompensasSinPresencia.xaml
    /// </summary>
    public partial class frmRecompensasSinPresencia : Page
    {
        private const string NOMBRE_GENERAL = "frmRecompensas";
        private bool StatusCmd;
        public string saldoRecom, diferenciaRecom, montoCobrado, NumOper, NumAuth;
        public abrirFrm abrir;
        public frmRecompensasSinPresencia()
        {
            InitializeComponent();
            #region
            numTdc.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;
            numCvv.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            #endregion
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void numTdcGotFocus(object sender, RoutedEventArgs e)
        {
            numTdc.Text = num_tarjeta;
        }

        private void numTdcLostFocus(object sender, RoutedEventArgs e)
        {
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
                    Globales.MessageBoxMit("Favor de validar tarjeta");
                    this.num_tarjeta = string.Empty;
                    this.numTdc.Text = string.Empty;
                    this.numCvv.Password = string.Empty;
                    return;
                }
            }
            if(!string.IsNullOrWhiteSpace(num_tarjeta)){
                if (num_tarjeta.Length == 15)
                {
                    Globales.isAmex = true;
                }
                else {
                    Globales.isAmex = false;
                }

                formaPagoClick();
                numCvv.Password = "";
                if (Globales.isAmex)
                {
                    numCvv.MaxLength = 4;
                }
                else {
                    numCvv.MaxLength = 3;
                }
            }
        }

        private void formaPagoClick()
        {
            if (Globales.GetDataXml("MXN", TypeUsuario.CadenaXML).Contains(Globales.merchantBanda))
            {
                lblMoneda.Content = "MXN";
            }
            else {
                lblMoneda.Content = "USD";
            }
        }

        private void nomTdc_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTexto(sender,e);
        }

        private void cvvLostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void antesEscribirLetra(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }

        private void importeLostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender,e);
        }

        private void cmdVoucher_click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
             switch(TypeUsuario.TipoImpresora){
                 case "1":
                     Globales.Imprimir_HTML(TypeUsuario.strVoucherCoP);
                     break;
                 case "4":
                     Globales.Imprimir_HTML(TypeUsuario.strVoucherCoP);
                     break;
                 case "6":
                     Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                     cmdVoucher.IsEnabled = true;
                     cmdNuevo.IsEnabled = true;
                     break;
                 default:
                     break;
             }
            }
            catch { 
            
            }
            Mouse.OverrideCursor = null;
        }

        private void cmdEnviarClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TypeUsuario.Id_Company == Globales.EMPREF && TypeUsuario.Id_Branch == Globales.EMPREF2)
                {
                    numOrden.Text = "REFTEMP"+importe.Text;
                }
                if(string.IsNullOrWhiteSpace(numTdc.Text)){
                    Globales.MessageBoxMit("Introduzca el número de tarjeta.");
                    numTdc.Focus();
                    return;
                }else if(Mes.SelectedIndex == -1){
                    Globales.MessageBoxMit("Seleccione el mes de vencimiento de la tarjeta");
                    Mes.Focus();
                    return;
                }else if(Anio.SelectedIndex == -1){
                    Globales.MessageBoxMit("Seleccione el año de vencimiento de la tarjeta");
                    Anio.Focus();
                    return;
                }else if(string.IsNullOrWhiteSpace(nomTdc.Text)){
                    Globales.MessageBoxMit("introduzca el nombre del titular");
                    nomTdc.Focus();
                    return;
                }else if(string.IsNullOrWhiteSpace(numCvv.Password)){
                    Globales.MessageBoxMit("Introduzca el Cod Seg de la tarjeta");
                    numCvv.Focus();
                    return;
                }else if(numCvv.Password == "000" || numCvv.Password == "0000"){
                    Globales.MessageBoxMit("Códig de seguridad invalido");
                    numCvv.Focus();
                    return;
                }else if(string.IsNullOrWhiteSpace(numOrden.Text)){
                    Globales.MessageBoxMit("Introduzca"+TypeUsuario.reference);
                    numOrden.Focus();
                    return;
                }
                else if (Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML).Substring(0, 1) == "1" && Globales.GetDataXml("tarjeta", Globales.cpHTTP_sResult) != num_tarjeta)
                {
                    Globales.MessageBoxMit("La tarjeta introducida no corresponde con el servicio a cobrar");
                    return;
                }
                else { 
                   if(TypeUsuario.Id_Company == "0059"){
                      if(numOrden.Text.Length != 28){
                          Globales.MessageBoxMit("El no. de REFERENCIA debe ser de 28 posiciones");
                          numOrden.Focus();
                          return;
                      }
                   }
                    if(Convert.ToInt32(Anio.Text) == Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2,2))){
                       if(Convert.ToInt32(Mes.Text) < Convert.ToInt32(DateTime.Now.Month)){
                           Globales.MessageBoxMit("Tarjeta vencida ");
                           return;
                       }
                    }
                    Globales.CheckOm("3",numCvv.Password);
                    if(numCvv.Password.Length != 3 && !(Globales.isAgencias || Globales.isAerolinea)){
                        Globales.MessageBoxMit("Introduzca el Cod Seg de la tarjeta");
                        numCvv.Focus();
                        return;
                    }
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    cobroRecompensas();
                    Mouse.OverrideCursor = null;
                    cmdEnviar.IsEnabled = false;
                }
                
            }
            catch { 
            
            }
        }

        private void cobroRecompensas()
        {
            Mouse.OverrideCursor = null;
            string Voucher, strCadEncriptar;
          //  Globales.cpIntegraEMV.sndvtaRe
            string opcion = Globales.cpIntegraEMV.getRspCd_StatusRecom();
            switch(opcion){
                case "2":
                    formaPago.IsEnabled = false;
                    numOrden.IsEnabled = false;
                    importe.IsEnabled = false;
                    Globales.MessageBoxMitApproved("Cobro aprovado");
                    TypeUsuario.strVoucherCoP = Globales.cpIntegraEMV.getRspVoucher().Trim();
                    cmdNuevo.Visibility = Visibility.Visible;
                    cmdNuevo.IsEnabled = true;
                    cmdEnviar.Visibility = Visibility.Hidden;
                    imgMail.Visibility = Visibility.Hidden;
                    if(TypeUsuario.enviaCorreo){
                        imgMail.Visibility = Visibility.Visible;
                    }
                    Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();
                    Voucher = Globales.cpIntegraEMV.getRspVoucher();
                    if(Voucher.Contains("@cnb -C-O-P-I-A- ")){
                        Voucher = Voucher.Replace("@cnb -C-O-P-I-A- ","");
                    }
                    printVoucherRecompensas(Voucher);
                    cmdVoucher.IsEnabled = true;
                    break;
                case "1":
                    saldoRecom = Globales.cpIntegraEMV.getRspSaldoRecom();
                    diferenciaRecom = Convert.ToString(Convert.ToDouble(importe.Text)-Convert.ToDouble(saldoRecom));
                    Globales.MessageBoxMit("Saldo disponible: "+saldoRecom);
                    cmdNuevo.Visibility = Visibility.Visible;
                    cmdNuevo.IsEnabled = true;
                    cmdEnviar.Visibility = Visibility.Hidden;
                    cmdVoucher.IsEnabled = false;
                    cmdVoucher.Visibility = Visibility.Hidden;
                    break;
                case "0":
                    Globales.MessageBoxMitError(Globales.cpIntegraEMV.dbgGetRspError());
                    cmdEnviar.IsEnabled = false;
                    cmdEnviar.Visibility = Visibility.Hidden;
                    formaPago.IsEnabled = true;
                    importe.IsEnabled = true;
                    numOrden.IsEnabled = true;
                    Globales.MessageBoxMitError(Globales.cpIntegraEMV.getRspDsError());
                    cmdNuevo.Visibility = Visibility.Visible;
                    cmdNuevo.IsEnabled = true;
                    break;
                default:
                    Globales.MessageBoxMit("La transacción ha finalizado, clic en nuevo para hacer otra");
                    cmdEnviar.Visibility = Visibility.Hidden;
                    cmdEnviar.IsEnabled = false;
                    cmdNuevo.Visibility = Visibility.Visible;
                    cmdNuevo.IsEnabled = true;
                    formaPago.IsEnabled = true;
                    numOrden.IsEnabled = true;
                    if (!string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.getRspDsError()))
                    {
                        Globales.MessageBoxMitError(Globales.cpIntegraEMV.getRspDsError());
                    }
                    else {
                        Globales.MessageBoxMitError("Error de conexión, verifique su reporte..");
                    }
                    StatusCmd = true;
                    break;
            }
        }

        private void printVoucherRecompensas(string Voucher)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            string strCadEncriptar = string.Empty;
            try
            {
                Globales.cpHTTP_Clear();
                Globales.cpHTTP_sURL_cpCUCT = Globales.URL_3GATE+"/recomp/pcpay";
                strCadEncriptar = "&folio="+Globales.cpIntegraEMV.getRspOperationNumber()+
                                  "&usuario="+TypeUsuario.usu+
                                  "&op=impticket&co=false";
                Globales.cpHTTP_cadena1 = "strcadEncriptar=<pgs><data0>" + TypeUsuario.Id_Company + "</data0><data>" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4_CP_COM(TypeUsuario.Id_Company), true) + "</data></pgs>";
               switch(TypeUsuario.TipoImpresora){
                   case "1":
                       if(Globales.cpHTTP_SendcpCUCT()){
                           TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                           Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                       }
                       break;
                   case "4":
                       if (Globales.cpHTTP_SendcpCUCT())
                       {
                           TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                           Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                       }
                       break;
                   case "6":
                       Globales.PrintOptions(Voucher);
                       break;
                   default:
                       Globales.MessageBoxMit("Seleccione una impresora");
                       break;
               }
            }
            catch { 
            
            }
            Mouse.OverrideCursor = null;
        }

        private void cmdNuevoClick(object sender, RoutedEventArgs e)
        {

            Globales.csvAmexenBanda = "";
            StatusCmd = true;
            numTdc.IsEnabled = true;
            numTdc.Visibility = Visibility.Visible;
            num_tarjeta = "";
            formaPago.IsEnabled = true;
            numOrden.IsEnabled = true;
            importe.IsEnabled = true;
            limpia();
            TypeUsuario.strVoucherCoP = "";
            cmdEnviar.IsEnabled = true;
            imgMail.Visibility = Visibility.Hidden;
            label_6.Visibility = Visibility.Hidden;
            formaPago.Visibility = Visibility.Hidden;
            lblMoneda.Content = "";
            saldoRecom = "";
            diferenciaRecom = "";
            cmdVoucher.Visibility = Visibility.Visible;
            cmdVoucher.IsEnabled = false;
            if(Globales.GetDataXml("facileasing",TypeUsuario.CadenaXML).Substring(0,1) == "1"){
            
            }
            

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StatusCmd = true;
                if(TypeUsuario.Id_Company == Globales.EMPREF && TypeUsuario.Id_Branch == Globales.EMPREF2){
                    numOrden2.Visibility = Visibility.Hidden;
                    label_7.Visibility = Visibility.Hidden;
                }
                if(Globales.bolActivaMotoP){
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    limpia();
                    Mouse.OverrideCursor = null;
                    cmdEnviar.Visibility = Visibility.Visible;
                    cmdNuevo.Visibility = Visibility.Hidden;
                    label_7.Content = "Referencia";
                    if(TypeUsuario.ShowMsg){
                        new frmAvisoBanda().ShowDialog();
                    }
                    
                    
                }
            }
            catch { 
            
            }
        }

        private void limpia()
        {
            try
            {
                numTdc.Text = "";
                numCvv.Password = "";
                nomTdc.Text = "";

                numOrden.Text = "";
                importe.Text = "";
                Anio.Items.Clear();
                Mes.Items.Clear();
                for (int x = 0; x <= 10; x++)
                {
                    Anio.Items.Add((DateTime.Now.Year + x).ToString().Substring(2, 2));
                }
                for (int x = 1; x <= 12; x++)
                {
                    string aux = string.Empty;
                    if (x < 10) aux = "0";
                    aux += Convert.ToString(x);
                    Mes.Items.Add(aux);
                }
                numOrden.MaxLength = Globales.MAXCAR;
                if(TypeUsuario.Id_Company == "0059"){
                    numOrden.MaxLength = 28;
                }
            }
            catch { 
            
            }
        }

        public string num_tarjeta { get; set; }

        private void nomTdc_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTexto(sender,e);
        }

        private void numOrden_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }
    }
}
