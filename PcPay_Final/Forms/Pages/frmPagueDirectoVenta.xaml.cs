using PcPay.Code.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for frmPagueDirectoVenta.xaml
    /// </summary>
    public partial class frmPagueDirectoVenta : Page
    {
        string tarjetaOriginal;
        string tarjetaComision;
        string merchanFactura;
        string merchanCOmision;
        string Voucher;
        string VoucherComision;
        string numOperacionComision;
        string numAuthComision;
        bool trxComisionCobrada;
        public frmPagueDirectoVenta()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "Page_Loaded";
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                limpia();
                Mouse.OverrideCursor = null;

            }
            catch { 
            
            }
        }

        private void limpia()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Thread.Sleep(2000);
            frameResultado.Visibility = Visibility.Hidden;
            frameEmpresa.Visibility = Visibility.Hidden;
            frameError.Visibility = Visibility.Hidden;

            cmdFacturaSi.IsEnabled = true;
            cmdFacturaNo.IsEnabled = true;
            if (Convert.ToBoolean(optFactura.IsChecked))
            {
                optFactura.IsChecked = true;
                frameFactura.Visibility = Visibility.Visible;


                txtFactura.Text = "";
                txtFacturaConfirmar.Text = "";
                txtFactura.IsEnabled = true;
                txtFacturaConfirmar.IsEnabled = true;
                txtFactura.Focus();

                cmdFactura.IsEnabled = true;
                cmdLimpiar.IsEnabled = true;
                cmdFacturaSi.IsEnabled = true;
                cmdFacturaNo.IsEnabled = true;
                Globales.cpIntegraEMV.dbgEndOperation();
                frameComision.Visibility = Visibility.Hidden;

            }
            else {
                cmdLimpiarPA_Click(null,null);
                cmdAceptarOpcion.Visibility = Visibility.Visible;
                framePagoAbierto.Visibility = Visibility.Hidden;

                txtReferenciaConfir.IsEnabled = true;
                txtReferenciaConfir.IsEnabled = true;
                txtMontoConfir.IsEnabled = true;
                txtMontoConfir.IsEnabled = true;
                cmdAceptarPA.IsEnabled = true;
                cmdLimpiarPA.IsEnabled = true;
                cmdNuevoPA.IsEnabled = true;

                cmdAceptarOpcion.IsEnabled = true;
                frameError.Visibility = Visibility.Hidden;
            }

            Mouse.OverrideCursor = null;
        }

       

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        public string NOMBRE_GENERAL = "Prepago venta directa";

        private void cmdLimpiarPA_Click(object sender, RoutedEventArgs e)
        {
            txtReferencia.Text = "";
            txtReferenciaConfir.Text = "";
            txtMonto.Text = "";
            txtMontoConfir.Text = "";
        }

        private void cmdAceptarOpcion_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "cmdAceptarOpcion_Click";
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                cmdAceptarOpcion.IsEnabled = false;

                bool Resp = false;
                Resp = Globales.cpIntegraEMV.GetReferenciaPagueDirecto(false);
                if (Resp)
                {
                    framePagoAbierto.Visibility = Visibility.Visible;
                    txtReferencia.Focus();
                }
                else {
                    cmdAceptarOpcion.IsEnabled = true;
                }
            }
            catch {
                Globales.mensajeAlerta(Globales.strNombreFP);
            }
            Mouse.OverrideCursor = null;
        }

        private void optFactura_Click(object sender, RoutedEventArgs e)
        {
            frameFactura.Visibility = Visibility.Visible;
            frameEmpresa.Visibility = Visibility.Hidden;
            cmdAceptarOpcion.Visibility = Visibility.Hidden;

            txtFactura.Text = "";
            txtFactura.IsEnabled = true;
            txtFacturaConfirmar.Text = "";
            txtFacturaConfirmar.IsEnabled = true;

            cmdFactura.IsEnabled = true;
            cmdLimpiar.IsEnabled = true;
            cmdNuevo.IsEnabled = false;

            framePagoAbierto.Visibility = Visibility.Hidden;
        }

        private void optPago_Click(object sender, RoutedEventArgs e)
        {
            frameFactura.Visibility = Visibility.Hidden;
            frameEmpresa.Visibility = Visibility.Hidden;
            cmdNuevo.IsEnabled = false;
            cmdAceptarOpcion.Visibility = Visibility.Visible;
            cmdAceptarOpcion.IsEnabled = true;
        }

        private void cmdLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtFactura.Text = "";
            txtFacturaConfirmar.Text = "";
        }

        private void cmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            limpia();
        }

        private void cmdFactura_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFactura.Text) && string.IsNullOrWhiteSpace(txtFacturaConfirmar.Text))
            {
                Globales.mensajeAlerta("Favor de verificar la factura.");
                txtFactura.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtFactura.Text) && !string.IsNullOrWhiteSpace(txtFacturaConfirmar.Text))
            {
                Globales.mensajeAlerta("Favor de verificar la factura.");
                txtFactura.Focus();
                return;
            }

            if(!string.IsNullOrWhiteSpace(txtFactura.Text) && string.IsNullOrWhiteSpace(txtFactura.Text)){
             Globales.mensajeAlerta("Favor de verificar la factura.");
                txtFactura.Focus();
                return;
            }
            if(txtFactura.Text.ToUpper() != txtFacturaConfirmar.Text.ToUpper()){
                Globales.mensajeAlerta("Las facturas no conciden");
                txtFactura.Focus();
                return;
            }

            //***********************Valida la factura*************************+
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
            Globales.cpIntegraEMV.GetValidaReferenciaPagueDirecto(TypeUsuario.usu,txtFactura.Text);
            if(Globales.cpIntegraEMV.getRspResponsePagueDirecto() != "1"){
                Globales.mensajeAlerta(Globales.cpIntegraEMV.getRspErrorPagueDirecto());
                txtFactura.Focus();
                Mouse.OverrideCursor = null;
                return;
            }
            Globales.cpIntegraEMV.GetReferenciaPagueDirecto(true);

            cmdFactura.IsEnabled = false;
            txtFactura.IsEnabled = false;
            txtFacturaConfirmar.IsEnabled = false;
            cmdLimpiar.IsEnabled = false;
            //Se visualiza el frame de datos de la empresa.
            frameEmpresa.Visibility = Visibility.Visible;
            lblTienda.Content = Globales.cpIntegraEMV.getDescTiendaPagueDirecto();
            lblEmpresa.Content = Globales.cpIntegraEMV.getProveedorPagueDirecto();
            lblFactura.Content = txtFactura.Text;
            lblMonto.Content = "$" + Globales.cpIntegraEMV.getImportePagueDirecto();
            Mouse.OverrideCursor = null;
        }

        private void cmdNuevoPA_Click(object sender, RoutedEventArgs e)
        {
            limpia();
        }

        private void cmdAceptarPA_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtReferencia.Text)){
                Globales.mensajeAlerta("Falta la referencia");
                txtReferencia.Focus();
                return;
            }

            if(string.IsNullOrWhiteSpace(txtReferenciaConfir.Text)){
                Globales.mensajeAlerta("Falta la confirmación de la referencia");
                txtReferenciaConfir.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtMonto.Text))
            {
                Globales.mensajeAlerta("Falta el monto.");
                txtMonto.Focus();
                return;
            }
            if(string.IsNullOrWhiteSpace(txtMontoConfir.Text)){
                Globales.mensajeAlerta("Falta la confirmación del monto");
                txtMontoConfir.Focus();
                return;
            }
            if(txtReferencia.Text != txtReferenciaConfir.Text){
                Globales.mensajeAlerta("Las referencias son diferentes");
                txtReferenciaConfir.Focus();
                return;
            }
            if(txtMontoConfir.Text != txtMonto.Text){
                Globales.mensajeAlerta("Los montos son diferentes");
                txtMonto.Focus();
                return;
            }

            frameError.Visibility = Visibility.Hidden;
            //Se visualiza el frame de datos de la empresa

            framePagoAbierto.Visibility = Visibility.Hidden;
            frameEmpresa.Visibility = Visibility.Visible;

            lblTienda.Content = "";
            lblFactura.Content = txtReferencia.Text;
            lblEmpresa.Content = Globales.cpIntegraEMV.getProveedorPagueDirecto();
            lblMonto.Content = "$"+txtMonto.Text;
            Globales.cpIntegraEMV.setImportePagueDirecto(txtMonto.Text);
        }

        private void cmdFacturaNo_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(optFactura.IsChecked))
            {
                txtFactura.IsEnabled = true;
                txtFactura.Focus();
                txtFacturaConfirmar.IsEnabled = true;
                cmdFactura.IsEnabled = true;
                cmdLimpiar.IsEnabled = true;
            }
            else {
                framePagoAbierto.Visibility = Visibility.Visible;
                txtReferencia.Focus();
            }
            frameEmpresa.Visibility = Visibility.Hidden;
        }

        private void cmdFacturaSi_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "cmdFacturaSi_Click";
            try
            {
                if (Convert.ToBoolean(optFactura.IsChecked))
                {
                    Grid.SetRow(frameError,2);
                    Grid.SetRow(frameResultado,2);
                }
                else {
                    Grid.SetRow(frameError,3);
                    Grid.SetRow(frameResultado,3);
                }

                cmdFacturaSi.IsEnabled = false;
                cmdFacturaNo.IsEnabled = false;

                Globales.mensajeAlerta("Inserte Chip o Deslice Tarjeta");
                Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                Globales.cpIntegraEMV.dbgStartTxEMV(Globales.cpIntegraEMV.getImportePagueDirecto());
                Globales.cpIntegraEMV.dbgSetCurrencyType("MXN");
                if (Globales.cpIntegraEMV.chkPp_CdError() == "" && Globales.cpIntegraEMV.chkCc_Number() != "")
                {
                    strTypeC = "";
                    tarjetaOriginal = Globales.cpIntegraEMV.chkCc_Number();
                    //Se valida el bin de la tarjeta para saber la comision a cobrar.
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    Globales.cpIntegraEMV.GetValidaBinTarjetaPagueDirecto(TypeUsuario.usu, tarjetaOriginal.Substring(0, 6), Globales.cpIntegraEMV.getImportePagueDirecto());
                    //Valida si se presenta un error;

                    if (Globales.cpIntegraEMV.getRspResponsePagueDirecto() != "1")
                    {
                        Globales.mensajeAlerta(Globales.cpIntegraEMV.getRspErrorPagueDirecto());
                        cmdFacturaNo.IsEnabled = true;
                        cmdFacturaSi.IsEnabled = true;
                        return;
                    }
                    //SE PREGUINTA SI SE DESEA PAGAR MANDADO EL MENSAJE DEL COBRO DE LA COMISION
                    string mensajeCobro;
                    string opcionCobro;
                    string referenciaCobro;

                    if (Convert.ToBoolean(optFactura.IsChecked))
                    {
                        referenciaCobro = txtFactura.Text;
                    }
                    else
                    {
                        referenciaCobro = txtReferencia.Text;
                    }
                    if (Globales.cpIntegraEMV.getImporteComisionPagueDirecto() == "0.00")
                    {
                        mensajeCobro = Globales.cpIntegraEMV.getMensajeComisionPagueDirecto();
                        opcionCobro = "1";
                    }
                    else
                    {
                        mensajeCobro = Globales.cpIntegraEMV.getMensajeComisionPagueDirecto() + " $ " + Globales.cpIntegraEMV.getImporteComisionPagueDirecto();
                        opcionCobro = "2";
                    }
                    Mouse.OverrideCursor = null;
                    if (System.Windows.Forms.DialogResult.Yes == Globales.mensajeQuestion(mensajeCobro, ""))
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        merchanFactura = Globales.cpIntegraEMV.getAfiliacionPagueDirecto();
                        merchanCOmision = Globales.cpIntegraEMV.getMerchantComisionPagueDirecto();
                        if (opcionCobro == "1")
                        {
                            //Se cobra la factura
                            Globales.cpIntegraEMV.sndVtaDirectaEMV(TypeUsuario.usu, TypeUsuario.Pass, "", TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
                                TypeUsuario.country, strTypeC, merchanFactura, referenciaCobro, "8", "MXN", Globales.csvAmexenBanda);
                            //Valida la respuesta del centro de pagos
                            validaOperacion();
                        }
                        else
                        {
                            //SE TERMINA LA LECTURA DE LA TARJETA ANTERIOR
                            Globales.cpIntegraEMV.dbgCancelOperation();
                            Globales.mensajeAlerta("Se cobrará la comisión del servicion\nPara continuar con el proceso, favor de retirar la tarjeta");

                            //Se establece ñps datps para cifrar la información de la pinpad

                            Globales.cpIntegraEMV.dbgSetTrxDataPagueDirecto(Globales.cpIntegraEMV.getUserComisionPagueDirecto(),
                                Globales.cpIntegraEMV.getIdCompanyComisionPagueDirecto(),
                                true);
                            //-----------------------------------------------------------------

                            //SE COBRARA LA COMISIÓN
                            Globales.cpIntegraEMV.dbgStartTxEMV(Globales.cpIntegraEMV.getImporteComisionPagueDirecto());
                            tarjetaComision = Globales.cpIntegraEMV.chkCc_Number();

                            //SE VALIDAN LAS TARJETAS   
                            if (tarjetaOriginal != tarjetaComision)
                            {
                                Globales.mensajeAlerta("Error, no es la misma tarjeta de cobro");
                                CancelaCobroComision(false);
                                Globales.mensajeAlerta("Error al pagar la factora, comisión cancelada.");
                                Globales.cpIntegraEMV.dbgSetTrxDataPagueDirecto("", "", false);
                                return;
                            }

                            Globales.cpIntegraEMV.sndVtaDirectaEMV(Globales.cpIntegraEMV.getUserComisionPagueDirecto(), Globales.cpIntegraEMV.getPwdComisionPagueDirecto(), "",
                                Globales.cpIntegraEMV.getIdCompanyComisionPagueDirecto(), Globales.cpIntegraEMV.getIdBranchComisionPagueDirecto(),
                                TypeUsuario.country, strTypeC, merchanCOmision, referenciaCobro, "9", "MXN", Globales.csvAmexenBanda);

                            //ESTBLACE LOS DATOS PARA CIGFRAR LA INFORMACION DE LA PINPAD
                            Globales.cpIntegraEMV.dbgSetTrxDataPagueDirecto("", "", false);

                            //VALIDA LA RESPUESTA DE LA COMISION
                            if (Globales.cpIntegraEMV.getRspDsResponse() != "approved")
                            {
                                trxComisionCobrada = false;
                                validaOperacionComision();
                                Mouse.OverrideCursor = null;
                                return;
                            }
                            else
                            {
                                trxComisionCobrada = true;
                                numOperacionComision = Globales.cpIntegraEMV.getRspOperationNumber();
                                numAuthComision = Globales.cpIntegraEMV.getRspAuth();
                                Globales.ultimaTrxPD = numOperacionComision;
                                string facElect = "";
                                string idFormaPago = "";
                                string descFormaPago = "";
                                if (Globales.EsTarjetaCredito())
                                {
                                    idFormaPago = Globales.cpIntegraEMV.ObtieneIDFacturaElectronica("TARJETA DE CRÉDITO");
                                    descFormaPago = "TARJETA DE CRÉDITO";
                                }
                                else
                                {
                                    idFormaPago = Globales.cpIntegraEMV.ObtieneIDFacturaElectronica("TARJETA DE DÉBITO");
                                    descFormaPago = "TARJETA DE DÉBITO";
                                }
                                Globales.cpIntegraEMV.dbgSetUrl(Globales.ipfe);
                                facElect = Globales.cpIntegraEMV.sndFacturaElectronica(Globales.cpIntegraEMV.getUserComisionPagueDirecto(), Globales.cpIntegraEMV.getIdCompanyComisionPagueDirecto(), Globales.cpIntegraEMV.getIdBranchComisionPagueDirecto(),
                                    Globales.cpIntegraEMV.getImporteComisionPagueDirecto(), "", numAuthComision, numOperacionComision, Globales.cpIntegraEMV.getRspDate(), idFormaPago, descFormaPago, "COBRO COMISION", tarjetaOriginal.Substring(tarjetaOriginal.Length - 4));
                                if (string.IsNullOrWhiteSpace(facElect) || Globales.GetDataXml("cd_response", facElect) != "100")
                                {
                                    Globales.mensajeAlerta("Error el obtener la factura electrónico");
                                    cancelCobroComision(true);
                                    Mouse.OverrideCursor = null;
                                    return;
                                }

                                Globales.cpIntegraEMV.GetFacturaEletronicaPagueDirecto(numOperacionComision);
                                VoucherComision = Globales.cpIntegraEMV.getVoucherReimpresionPagueDirecto();

                                if (VoucherComision.Contains("@cbb"))
                                {
                                    VoucherComision = VoucherComision.Replace("@cbb", "@cnb");
                                }

                                if (VoucherComision.Contains("@cnb -C-O-P-I-A- "))
                                {
                                    VoucherComision = VoucherComision.Replace("@cnb -C-O-P-I-A- ", "");
                                }
                                Globales.mensajeAlerta("Se cobrará la factura\nInserte chip o deslice tarjeta");
                                Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                                Globales.cpIntegraEMV.dbgStartTxEMV(Globales.cpIntegraEMV.getImportePagueDirecto());

                                if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()) && !string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkCc_Number()))
                                {
                                    strTypeC = "";
                                    tarjetaComision = Globales.cpIntegraEMV.chkCc_Number();

                                    //===============================================================
                                    //SE VALIDAN LAS TARJETAS PARA QUE NO HAYA CAMBIO DE ELLAS
                                    if (tarjetaOriginal != tarjetaComision)
                                    {
                                        Globales.mensajeAlerta("Error, no es la misma tarjeta de cobro");
                                        //Si ya se cobró una comisión, esta se cancela.

                                        if (trxComisionCobrada)
                                        {
                                            cancelCobroComision(true);
                                        }
                                        Globales.mensajeAlerta("Error al pagar la factura, comisión cancelada.");
                                        Mouse.OverrideCursor = null;
                                        return;
                                    }
                                    //Se cobra la factura
                                    Globales.cpIntegraEMV.sndVtaDirectaEMV(TypeUsuario.usu, TypeUsuario.Pass, "", TypeUsuario.Id_Company, TypeUsuario.Id_Branch,
                                        TypeUsuario.country, strTypeC, merchanFactura, referenciaCobro, "8", "MXN", Globales.csvAmexenBanda);
                                    validaOperacion();
                                }
                                else
                                {

                                }
                            }
                        }
                        Mouse.OverrideCursor = null;
                    }
                    else
                    {
                        Globales.cpIntegraEMV.dbgCancelOperation();

                        cmdFacturaSi.IsEnabled = true;
                        cmdFacturaNo.IsEnabled = true;
                    }
                }
                else {
                    if (!string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_CdError()))
                    {
                       if(trxComisionCobrada){
                           CancelaCobroComision(true);
                       }

                       Globales.mensajeAlerta(Globales.cpIntegraEMV.chkPp_DsError());
                       cmdFacturaNo.IsEnabled = true;
                       cmdFacturaSi.IsEnabled = true;
                    }
                }
            }
            catch {
                Globales.mensajeAlerta(Globales.strNombreFP);
            }
        }

      

        private void validaOperacionComision()
        {
            frameEmpresa.Visibility = Visibility.Hidden;
            framePagoAbierto.Visibility = Visibility.Visible;

            switch(Globales.cpIntegraEMV.getRspDsResponse()){
                case "denied":
                    frameResultado.Visibility = Visibility.Hidden;
                    frameResultado.Visibility = Visibility.Visible;
                    lblTError.Visibility = Visibility.Hidden;

                    lblDenied.Visibility = Visibility.Visible;
                    lblDenied.Content = Globales.cpIntegraEMV.getRspCdResponse();

                    cmdNuevo.IsEnabled = true;
                    break;
                case "approved":
                    break;
                case "error":
                    cmdNuevo.IsEnabled = true;

                    frameResultado.Visibility = Visibility.Hidden;
                    frameError.Visibility = Visibility.Visible;
                    lblDenied.Visibility = Visibility.Hidden;

                    lblTError.Visibility = Visibility.Visible;
                    lblTError.Text = Globales.cpIntegraEMV.getRspDsError();
                    break;
                default:
                    cmdNuevo.IsEnabled = true;
                    frameResultado.Visibility = Visibility.Hidden;
                    frameError.Visibility = Visibility.Visible;
                    lblDenied.Visibility = Visibility.Hidden;

                    lblTError.Visibility = Visibility.Visible;
                    lblTError.Text = "Error de conexión, verifique su reporte.";
                    break;
            }
        }

        private void CancelaCobroComision(bool llamaCancelacion)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globales.cpIntegracion_Clear();
            Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL_CANC;
            Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);

            if(llamaCancelacion){
                Globales.cpIntegraEMV.sndCancelacion(Globales.cpIntegraEMV.getUserComisionPagueDirecto(),
                    Globales.cpIntegraEMV.getPwdComisionPagueDirecto(),"",Globales.cpIntegraEMV.getIdCompanyComisionPagueDirecto(),
                    Globales.cpIntegraEMV.getIdBranchComisionPagueDirecto(),TypeUsuario.country,Globales.cpIntegraEMV.getImporteComisionPagueDirecto(),
                    numOperacionComision,numAuthComision);

                Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();
                Globales.numOperaCancel = Globales.cpIntegraEMV.getRspOperationNumber();
            }
            frameEmpresa.Visibility = Visibility.Hidden;
            if (frameResultado.Visibility == Visibility.Visible || frameError.Visibility == Visibility.Visible)
            {
                Grid.SetRow(frameComision,4);
                frameComision.Visibility = Visibility.Visible;
            }
            else {
                Grid.SetRow(frameComision, 2);
                frameComision.Visibility = Visibility.Visible;
            }

            if(!llamaCancelacion){
                noCancelacionja = true;
            }

            if(noCancelacionja){
                lblCancelDenied.Content = "Cancelación rechazada";
                lblCancelDenied.Visibility = Visibility.Visible;
                txtCancelDenied.Visibility = Visibility.Visible;

                txtCancelDenied.Text = "Cancelación de comisión rechazada\n" + Globales.cpIntegraEMV.getRspCdResponse()+" "+Globales.cpIntegraEMV.getRspFriendlyResponse();
                lblComisionOk.Visibility = Visibility.Hidden;
                
                if(!llamaCancelacion){
                    lblCancelDenied.Content = "Error....";
                        txtCancelDenied.Text = "No es la misma tarjeta de cobro.";
                }
            }else{
                switch(Globales.cpIntegraEMV.getRspDsResponse()){
                    case "approved":
                        lblCancelDenied.Visibility = Visibility.Hidden;
                        txtCancelDenied.Visibility = Visibility.Hidden;
                        txtCancelDenied.Visibility = Visibility.Hidden;
                        lblComisionOk.Visibility = Visibility.Visible;
                        //Falta lblAuthComision
                        lblComisionOk.Content = "Cancelación de comisión aprobada";

                        //Bloque de impresión
                        Voucher = Globales.cpIntegraEMV.getRspVoucher();
                        if(Voucher.Contains("@cnb -C-O-P-I-A- ")){
                          Voucher = Voucher.Replace("@cnb -C-O-P-I-A- ","");
                        }
                        if((TypeUsuario.strTipoLector == "3" || TypeUsuario.strTipoLector == "4") && TypeUsuario.TipoImpresora == "6" ){
                           Globales.cpIntegraEMV.dbgImprimeVoucher(VoucherComision);
                        }else{
                          Thread.Sleep(10000);
                            Globales.printOptionPagueDirecto();
                        }
                        break;
                  case  "denied":
                 lblCancelDenied.Content = "Cancelación rechazada";
                lblCancelDenied.Visibility = Visibility.Visible;
                txtCancelDenied.Visibility = Visibility.Visible;

                txtCancelDenied.Text = "Cancelación de comisión rechazada\n" + Globales.cpIntegraEMV.getRspCdResponse()+" "+Globales.cpIntegraEMV.getRspFriendlyResponse();
                lblComisionOk.Visibility = Visibility.Hidden;
                
                if(!llamaCancelacion){
                    lblCancelDenied.Content = "Error....";
                        txtCancelDenied.Text = "No es la misma tarjeta de cobro.";
                }
                break;
                    case "error":
                        lblCancelDenied.Visibility = Visibility.Visible;
                        txtCancelDenied.Visibility = Visibility.Visible;
                        lblCancelDenied.Content = "Cancelación Rechazada";
                        txtCancelDenied.Text = "Cancelación de comisión rechazada\n"+Globales.cpIntegraEMV.getRspDsError();
                        lblComisionOk.Visibility = Visibility.Hidden;
                        break;
                    default:

                        lblCancelDenied.Visibility = Visibility.Visible;
                        txtCancelDenied.Visibility = Visibility.Visible;

                        lblCancelDenied.Content = "Cancelación rechazada";
                        txtCancelDenied.Text = "Cancelación de comisión Rechazada\nError de conexión, verifique su reporte";
                        lblComisionOk.Visibility = Visibility.Hidden;
                        break;
                }

                if(llamaCancelacion){
                  //Código que no sirve mucho
                }

                if(frameResultado.Visibility == Visibility.Visible || frameError.Visibility == Visibility.Visible){
                    cmdNuevo.IsEnabled = true;
                }else{
                   cmdNuevo.IsEnabled = true;
                }
                trxComisionCobrada = false;
            }
            Mouse.OverrideCursor = null;
        }

        private void validaOperacion()
        {
            frameEmpresa.Visibility = Visibility.Hidden;
            if(!Convert.ToBoolean(optFactura.IsChecked)){
                framePagoAbierto.Visibility = Visibility.Visible;
                txtReferencia.IsEnabled = false;
                txtReferenciaConfir.IsEnabled = false;
                txtMonto.IsEnabled = false;
                txtMontoConfir.IsEnabled = false;
                cmdAceptarPA.IsEnabled = false;
                cmdLimpiarPA.IsEnabled = false;
                cmdNuevoPA.IsEnabled = false;


                switch(Globales.cpIntegraEMV.getRspDsResponse()){
                    case "approved":
                        frameResultado.Visibility = Visibility.Visible;
                        frameError.Visibility = Visibility.Hidden;
                        lblAuth.Text = "Auth.\n" + Globales.cpIntegraEMV.getRspAuth();
                        TypeUsuario.strVoucherCoP = Globales.cpIntegraEMV.getRspVoucher().Trim();

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

                        Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();

                    GoImpresion:
                        Voucher = Globales.cpIntegraEMV.getRspVoucher();
                        if(Voucher.Contains("@cnb -C-O-P-I-A- ")){
                            Voucher = Voucher.Replace("@cnb -C-O-P-I-A- ","");
                        }
                        Globales.PrintOptions(Voucher, Globales.cpIntegraEMV.getRspOperationNumber());
                        //Imprimir voucher de comision
                        if (trxComisionCobrada)
                        {
                            if ((TypeUsuario.strTipoLector == "3" || TypeUsuario.strTipoLector == "4") && TypeUsuario.TipoImpresora == "6")
                            {
                                Globales.cpIntegraEMV.dbgImprimeVoucher(VoucherComision);
                            }
                            else {
                                Thread.Sleep(1000);
                                Globales.printOptionPagueDirecto();
                            }
                        }
                finaliza:
                        if(Globales.FacturaE == "1"){
                            if (System.Windows.Forms.DialogResult.Yes == Globales.mensajeQuestion("Desea factura electrónica", ""))
                            {
                                abrir(new frmPreguntaFactura());
                                return;
                            }
                            else {
                                Globales.XMLFacturaE = "";
                            }
                        }

                        cmdNuevo.IsEnabled = true;

                        break;
                    case "denied":
                        frameResultado.Visibility = Visibility.Hidden;
                        frameError.Visibility = Visibility.Visible;

                        lblTError.Visibility = Visibility.Visible;
                        lblDenied.Visibility = Visibility.Visible;
                        lblDenied.Content = Globales.msjRech+" "+Globales.cpIntegraEMV.getRspCdResponse();
                        cmdNuevo.IsEnabled = true;

                        //Si ya se cobró una comisión, esta de cancela
                        if(trxComisionCobrada){
                            cancelCobroComision(true);
                        }
                        break;
                    case "error":
                        cmdNuevo.IsEnabled = true;
                        frameResultado.Visibility = Visibility.Hidden;
                        frameError.Visibility = Visibility.Visible;
                        lblDenied.Visibility = Visibility.Hidden;

                        lblTError.Visibility = Visibility.Visible;
                        lblTError.Text = Globales.cpIntegraEMV.getRspDsError();

                        if(trxComisionCobrada){
                            cancelCobroComision(true);
                        }
                        break;
                    default:
                        cmdNuevo.IsEnabled = true;
                        frameResultado.Visibility = Visibility.Hidden;
                        frameError.Visibility = Visibility.Visible;
                        lblDenied.Visibility = Visibility.Hidden;

                        lblTError.Visibility = Visibility.Visible;
                        lblTError.Text = "Error de conexión, verifique su reporte";
                        if (trxComisionCobrada)
                        {
                            cancelCobroComision(true);
                        }
                        break;
                }
            }
            Mouse.OverrideCursor = null;
            trxComisionCobrada = true;
        }

        private void cancelCobroComision(bool p)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                Globales.cpIntegracion_Clear();
                Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL_CANC;
                Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                if(p){
                    Globales.cpIntegraEMV.sndCancelacion(Globales.cpIntegraEMV.getUserComisionPagueDirecto(),
                        Globales.cpIntegraEMV.getPwdComisionPagueDirecto(),
                        "",
                        Globales.cpIntegraEMV.getIdCompanyComisionPagueDirecto(),
                        Globales.cpIntegraEMV.getIdBranchComisionPagueDirecto(),
                        TypeUsuario.country,
                        Globales.cpIntegraEMV.getImporteComisionPagueDirecto(),
                        numOperacionComision,
                        numAuthComision);
                    Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();

                    Globales.numOperaCancel = Globales.cpIntegraEMV.getRspOperationNumber();

                }
                frameEmpresa.Visibility = Visibility.Hidden;
                if (frameResultado.Visibility == Visibility.Visible || frameError.Visibility == Visibility.Visible)
                {
                    frameComision.Visibility = Visibility.Visible;
                }
                else {
                    frameComision.Visibility = Visibility.Visible;
                }
                if(!p){
                    //goto NoCancelacion;
                    noCancelacion = true;
                }
                if (noCancelacion)
                {
                    lblCancelDenied.Content = "Cancelación Rechazada";
                    lblCancelDenied.Visibility = Visibility.Visible;
                    txtCancelDenied.Visibility = Visibility.Visible;
                    txtCancelDenied.Text = "Cancelación de Comisión Rechazada\n"+Globales.cpIntegraEMV.getRspCdResponse();
                    lblComisionOk.Visibility = Visibility.Hidden;
                    if(!p){
                        lblCancelDenied.Content = "Error...";
                        txtCancelDenied.Text = "No es la misma tarjeta de cobro";
                    }
                }
                else { 
                   switch(Globales.cpIntegraEMV.getRspDsResponse()){
                       case "approved":
                           lblCancelDenied.Visibility = Visibility.Hidden;
                           txtCancelDenied.Visibility = Visibility.Hidden;
                           txtCancelDenied.Visibility = Visibility.Hidden;
                           lblComisionOk.Visibility = Visibility.Hidden;
                           lblComisionOk.Content = "Cancelación de comisión Aprobada.";

                           //BLOQUE DE IMPRESIÓN
                           Voucher = Globales.cpIntegraEMV.getRspVoucher();
                           if(Voucher.Contains("@cnb -C-O-P-I-A- ")){
                               Voucher.Replace("@cnb -C-O-P-I-A- ","");
                           }
                           if (TypeUsuario.strTipoLector == "3" || TypeUsuario.strTipoLector == "4" || TypeUsuario.strTipoLector == "6")
                           {
                               Globales.cpIntegraEMV.dbgImprimeVoucher(VoucherComision);
                           }
                           else {
                               Thread.Sleep(1000);
                               Globales.printOptionPagueDirecto();
                           }
                           break;
                       case "denied":
                             lblCancelDenied.Content = "Cancelación Rechazada";
                    lblCancelDenied.Visibility = Visibility.Visible;
                    txtCancelDenied.Visibility = Visibility.Visible;
                    txtCancelDenied.Text = "Cancelación de Comisión Rechazada\n"+Globales.cpIntegraEMV.getRspCdResponse();
                    lblComisionOk.Visibility = Visibility.Hidden;
                    if(!p){
                        lblCancelDenied.Content = "Error...";
                        txtCancelDenied.Text = "No es la misma tarjeta de cobro";
                    }
                           break;
                       case "error":
                           lblCancelDenied.Visibility = Visibility.Visible;
                           txtCancelDenied.Visibility = Visibility.Visible;
                           lblCancelDenied.Content = "Cancelación Rechazada";
                           txtCancelDenied.Text = "Cancelación de Comisión Rechazada."+Globales.cpIntegraEMV.getRspDsError();
                           lblComisionOk.Visibility = Visibility.Hidden;
                           //Falta algo aqui
                           break;
                       default:
                           lblCancelDenied.Visibility = Visibility.Visible;
                           txtCancelDenied.Visibility = Visibility.Visible;

                           lblCancelDenied.Content = "Cancelación Rechazada";
                           txtCancelDenied.Text = "Cancelacion de comisión Rechazada Error de conexión, verifique su reporte";
                           lblComisionOk.Visibility = Visibility.Hidden;
                           
                           break;
                   }
                }
                if(p){
                    //GLobo pcpay
                }
                if (frameResultado.Visibility == Visibility.Visible || frameError.Visibility == Visibility.Visible)
                {
                    cmdNuevo.IsEnabled = true;
                }
                else {
                    cmdNuevo.IsEnabled = true;
                }
                trxComisionCobrada = true;
                Mouse.OverrideCursor = null;
            }
            catch { 
            
            }
        }



        public string strTypeC { get; set; }
        public abrirFrm abrir { get; set; }

        public bool noCancelacion { get; set; }

        public bool finalizaLectura { get; set; }

        public bool noCancelacionja { get; set; }
    }
}
