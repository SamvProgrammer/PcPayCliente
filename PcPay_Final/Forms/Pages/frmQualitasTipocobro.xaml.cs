using PcPay.Code.Usuario;
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
    /// Interaction logic for frmQualitasTipocobro.xaml
    /// </summary>
    /// 
    public delegate void metodo();
    public partial class frmQualitasTipocobro : Page
    {

        public menu menu1;
        public menu2 menu2;
        public int tipoVtaQualitas = 1;
        public cerrarVentana cerrar;
        public string caption = "";
        private const string NOMBRE_GENERAL = "frmQualitas";
        public int tipoVista = 0;
        string strXML, resultado;
        string fecha;
        string strAux = "";
        public frmQualitasTipocobro()
        {
            InitializeComponent();
            this.frameDeducible.Visibility = Visibility.Hidden;
            this.framePoliza.Visibility = Visibility.Hidden;
            txtDeducible.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            txtNumPoliza.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
            txtDeducible.GotFocus += Globales.setFocusMit2;
            txtNumPoliza.GotFocus += Globales.setFocusMit2;
            txtDeducible.LostFocus += Globales.lostFocusMit2;
            txtNumPoliza.LostFocus += Globales.lostFocusMit2;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Globales.isQualitas)
            {
                if (Globales.isQualitasCierraForm)
                {
                    //  cerrar();
                }
                else
                {
                    Globales.MessageBoxMit("Debes Actualizar el pago");
                    return;
                }
            }
            framePoliza.Visibility = Visibility.Hidden;
            frameDeducible.Visibility = Visibility.Hidden;

            caption = "QUÁLITAS: Tipo de cobro.";
            Mouse.OverrideCursor = null;
            return;
        }


        private void cmdCancelar_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
        }

        private void cmdAceptarDeducible_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdAceptarDedicibleClick();";
            try
            {

                if (string.IsNullOrWhiteSpace(txtDeducible.Text))
                {
                    Globales.MessageBoxMit("Introduzca Número de Siniestro.");
                    return;
                }
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                typeUsuarioQualitas.NumSiniestro = txtDeducible.Text;
                strXML = "<?xml version=" + Convert.ToChar(34) + "1.0" + Convert.ToChar(34) + " encoding=" + Convert.ToChar(34) + "UTF-8" + Convert.ToChar(34) + "?>";
                strXML += " <SOAP-ENV:Envelope xmlns:SOAP-ENV=" + Convert.ToChar(34) + "http://schemas.xmlsoap.org/soap/envelope/" + Convert.ToChar(34) + " xmlns:ns1=" + Convert.ToChar(34) + "http://service.pcpay.qualitas.smartkode.mx/" + Convert.ToChar(34) + ">";
                strXML += " <SOAP-ENV:Body>";
                strXML += " <ns1:consultaDeducible>";
                strXML += "<siniestro>" + txtDeducible.Text + "</siniestro>";
                strXML += " </ns1:consultaDeducible>";
                strXML += " </SOAP-ENV:Body>";
                strXML += " </SOAP-ENV:Envelope>";

                resultado = Globales.WSSoap(strXML, Globales.ipQualitas);

                //'Se llenan los campos de acuerdo a la respuesta del WS
                if (!string.IsNullOrWhiteSpace(resultado) && !resultado.Contains("Error"))
                {

                    typeUsuarioQualitas.RespuestaCodigo = Globales.GetDataXml("codigo", resultado);
                    typeUsuarioQualitas.RespuestaMensaje = Globales.GetDataXml("mensaje", resultado);

                    if (typeUsuarioQualitas.RespuestaCodigo != "0")
                    {
                        //frameError.Visibility = Visibility.Hidden;
                        //frameVehiculo.Visibility = Visibility.Visible;
                        //lblModelo.Content = "Modelo:" + Globales.GetDataXml("modelo", resultado);
                        //strAux = Globales.GetDataXml("vehiculo", resultado);
                        //strAux = Globales.GetDataXml("descripcion", strAux);
                        //lblDescripcion.Content = "Descripción:" + strAux;
                        //lblNombre.Content = "Nombre:" + Globales.GetDataXml("nombre", resultado);
                        Mouse.OverrideCursor = null;
                        ventanaQualitasTipoCobro cobro = new ventanaQualitasTipoCobro(true, resultado);
                        cobro.enviar = enviarSi;
                        cobro.ShowDialog();
                    }
                    else
                    {
                        Mouse.OverrideCursor = null;
                        //frameError.Visibility = Visibility.Visible;
                        //txtError.Text = typeUsuarioQualitas.RespuestaMensaje;

                        Globales.MessageBoxMitError(typeUsuarioQualitas.RespuestaMensaje);
                        Globales.isQualitasTrxValida = false;
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    //frameError.Visibility = Visibility.Visible;
                    //txtError.Text = "Error al contactar el servicio de Consulta, por favor inténtelo más tarde";
                    Globales.MessageBoxMitDenied("Error al contactar el servicio de Consulta, por favor inténtelo más tarde");
                    Globales.isQualitasTrxValida = false;
                }



            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
            }
            Mouse.OverrideCursor = null;

        }

        private void cmdCancelarPoliza_Click(object sender, RoutedEventArgs e)
        {
            framePoliza.Visibility = Visibility.Hidden;
            txtNumPoliza.Text = "";
        }

        private void cmdCancelarDeducible_Click(object sender, RoutedEventArgs e)
        {
            frameDeducible.Visibility = Visibility.Hidden;
        }

        private void cmdAceptarPoliza_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "cmdAceptarPoliza_Click";
            try
            {

                if (string.IsNullOrWhiteSpace(txtNumPoliza.Text))
                {
                    Globales.MessageBoxMit("Introduzca Número de Póliza");
                    return;
                }
                typeUsuarioQualitas.NumPoliza = txtNumPoliza.Text;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                strXML = "<?xml version=" + Convert.ToChar(34) + "1.0" + Convert.ToChar(34) + " encoding=" + Convert.ToChar(34) + "UTF-8" + Convert.ToChar(34) + "?>";
                strXML += " <SOAP-ENV:Envelope xmlns:SOAP-ENV=" + Convert.ToChar(34) + "http://schemas.xmlsoap.org/soap/envelope/" + Convert.ToChar(34) + " xmlns:ns1=" + Convert.ToChar(34) + "http://service.pcpay.qualitas.smartkode.mx/" + Convert.ToChar(34) + ">";
                strXML += " <SOAP-ENV:Body>";
                strXML += " <ns1:consultaPoliza>";
                strXML += "<poliza>" + txtNumPoliza.Text + "</poliza>";
                strXML += " </ns1:consultaPoliza>";
                strXML += " </SOAP-ENV:Body>";
                strXML += " </SOAP-ENV:Envelope>";

                resultado = Globales.WSSoap(strXML, Globales.ipQualitas);
                //Se llena los campos de acuerdo a la respuesta de ES
                if (!string.IsNullOrWhiteSpace(resultado) && !resultado.Contains("Error"))
                {
                    typeUsuarioQualitas.RespuestaCodigo = Globales.GetDataXml("codigo", resultado);
                    typeUsuarioQualitas.RespuestaMensaje = Globales.GetDataXml("mensaje", resultado);
                    if (typeUsuarioQualitas.RespuestaCodigo == "1")
                    {
                        //frameError.Visibility = Visibility.Hidden;
                        //frameVehiculo.Visibility = Visibility.Visible;
                        //lblModelo.Content = "Modelo: " + Globales.GetDataXml("modelo", resultado);
                        //lblDescripcion.Content = "Descripción: " + Globales.GetDataXml("descripcion", resultado);
                        //lblNombre.Content = "Nombre: " + Globales.GetDataXml("asegurado", resultado);
                        Mouse.OverrideCursor = null;
                        ventanaQualitasTipoCobro cobro = new ventanaQualitasTipoCobro(false, resultado);
                        cobro.enviar = enviarSi;
                        cobro.ShowDialog();

                    }
                    else
                    {
                        Mouse.OverrideCursor = null;
                        //frameError.Visibility = Visibility.Visible;
                        //txtError.Text = typeUsuarioQualitas.RespuestaMensaje;
                        Globales.MessageBoxMitError(typeUsuarioQualitas.RespuestaMensaje);
                        Globales.isQualitasTrxValida = false;
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    //frameError.Visibility = Visibility.Visible;
                    //txtError.Text = "Error al contactar el Servicio de Consulta, Por favor inténtelo más tarde.";
                    Globales.MessageBoxMitError("Error al contactar el Servicio de Consulta, Por favor inténtelo más tarde.");
                    Globales.isQualitasTrxValida = false;
                }



            }
            catch
            {

            }
            Mouse.OverrideCursor = null;
        }

        private void cmdNo_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "cmdNo_Click";
            try
            {
                if (tipoVista == 1)
                {
                    txtNumPoliza.Focus();
                }
                else if (tipoVista == 2)
                {
                    txtDeducible.Focus();
                }
            }
            catch
            {

            }
        }


        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {

        }

        private void enviarSi()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "cmdOk_Click";
            try
            {

                if (tipoVista == 1)
                {
                    typeUsuarioQualitas.PolizaAsegurado = Globales.GetDataXml("asegurado", resultado);
                    typeUsuarioQualitas.PolizaMoneda = Globales.GetDataXml("moneda", resultado);
                    typeUsuarioQualitas.PolizaNumero = Globales.GetDataXml("numero", resultado);

                    typeUsuarioQualitas.PolizaReciboEndoso = Globales.GetDataXml("endoso", resultado);
                    typeUsuarioQualitas.PolizaReciboEstatus = Globales.GetDataXml("estatus", resultado);
                    typeUsuarioQualitas.PolizaReciboImporte = Globales.GetDataXml("importe", resultado);
                    strAux = Globales.GetDataXml("recibo", resultado);
                    strAux = Globales.GetDataXml("numero", strAux);
                    typeUsuarioQualitas.PolizaReciboNumero = strAux;
                    typeUsuarioQualitas.PolizaReciboVencimiento = Globales.GetDataXml("vecimiento", resultado);

                    typeUsuarioQualitas.PolizaVehiculoDescripcion = Globales.GetDataXml("descripcion", resultado);
                    typeUsuarioQualitas.PolizaVehiculoModelo = Globales.GetDataXml("modelo", resultado);
                    typeUsuarioQualitas.PolizaVehiculoSerie = Globales.GetDataXml("serie", resultado);

                    strAux = Globales.GetDataXml("tipoPagos", resultado);
                    if (strAux.Contains("CONTADO"))
                    {
                        typeUsuarioQualitas.TipoPagosContado = "CONTADO";
                    }
                    if (strAux.Contains("MSI"))
                    {
                        typeUsuarioQualitas.TipoPagosMSI = "MSI";
                        typeUsuarioQualitas.TipoPagosMSIPlan = Globales.GetDataXml("mesesSinIntereses", resultado);
                    }
                    Globales.isQualitasTrxValida = true;
                    Globales.cpIntegraEMV.dbgSetQualitasTipoPagosContado(typeUsuarioQualitas.TipoPagosContado);
                    Globales.cpIntegraEMV.dbgSetQualitasTipoPagosMSI(typeUsuarioQualitas.TipoPagosMSI);
                    Globales.cpIntegraEMV.dbgSetQualitasMoneda(typeUsuarioQualitas.PolizaMoneda);
                    Globales.cpIntegraEMV.dbgSetQualitasPlanPagosMSI(typeUsuarioQualitas.TipoPagosMSIPlan);
                    Globales.cpIntegraEMV.dbgSetQualitasTipocobro(typeUsuarioQualitas.TipoCobro);
                    if (tipoVtaQualitas == 1)
                    { //Venta con presencia de tarjeta
                        menu2();
                    }
                    else
                    {
                        menu1("Venta sin presencia de tarjeta");
                    }

                }

                if (tipoVista == 2)
                {
                    typeUsuarioQualitas.DeducibleIdAsegurado = Globales.GetDataXml("id", resultado);
                    typeUsuarioQualitas.DeducibleNombreAsegurado = Globales.GetDataXml("nombre", resultado);

                    typeUsuarioQualitas.DeducibleEndoso = Globales.GetDataXml("endoso", resultado);
                    typeUsuarioQualitas.DeducibleInciso = Globales.GetDataXml("inciso", resultado);

                    typeUsuarioQualitas.DeducibleMoneda = Globales.GetDataXml("moneda", resultado);
                    typeUsuarioQualitas.PolizaMoneda = typeUsuarioQualitas.DeducibleMoneda;
                    typeUsuarioQualitas.DeduciblePoliza = Globales.GetDataXml("poliza", resultado);
                    typeUsuarioQualitas.DeducibleReporte = Globales.GetDataXml("reporte", resultado);
                    typeUsuarioQualitas.DeducibleSiniestro = Globales.GetDataXml("siniestro", resultado);


                    int countCoberturas = 0;
                    Globales.docXML = new System.Xml.XmlDocument();
                    resultado = "<RespuestaConsultaDeducible>" + Globales.GetDataXml("RespuestaConsultaDeducible", resultado) + "</RespuestaConsultaDeducible>";
                    Globales.docXML.LoadXml(resultado);
                    Globales.nodeListXML = Globales.docXML.GetElementsByTagName("cobertura");
                    countCoberturas = Globales.nodeListXML.Count;
                    if (countCoberturas > 1)
                    {
                        Globales.isCoberturaMultiple = true;
                        Globales.numTotalCoberturas = countCoberturas;
                        countCoberturas = 0;
                    }
                    else
                    {
                        typeUsuarioQualitas.DeducibleCoberturaAplicaDeducible = Globales.GetDataXml("aplicaDeducible", resultado);
                        strAux = Globales.GetDataXml("cobertura", resultado);
                        strAux = Globales.GetDataXml("codigo", strAux);
                        typeUsuarioQualitas.DeducibleCoberturaCodigo = strAux;

                        typeUsuarioQualitas.DeducibleCoberturaDescripcion = Globales.GetDataXml("descripcion", resultado);
                        typeUsuarioQualitas.DeducibleCoberturaMonto = Globales.GetDataXml("monto", resultado);
                    }

                    strAux = Globales.GetDataXml("vehiculo", resultado);
                    strAux = Globales.GetDataXml("descripcion", strAux);
                    typeUsuarioQualitas.DeducibleVehiculoDescripcion = strAux;
                    typeUsuarioQualitas.DeducibleVehiculoModelo = Globales.GetDataXml("modelo", resultado);
                    typeUsuarioQualitas.DeducibleVehiculoSerie = Globales.GetDataXml("serie", resultado);
                    strAux = Globales.GetDataXml("tipoPagos", resultado);
                    if (strAux.Contains("CONTADO"))
                    {
                        typeUsuarioQualitas.TipoPagosContado = "CONTADO";
                    }
                    if (strAux.Contains("MSI"))
                    {
                        typeUsuarioQualitas.TipoPagosMSI = "MSI";
                        typeUsuarioQualitas.TipoPagosMSIPlan = Globales.GetDataXml("mesesSinIntereses", resultado);
                        if (typeUsuarioQualitas.DeducibleCoberturaAplicaDeducible.ToUpper() == "S" || Globales.isCoberturaMultiple)
                        {
                            Globales.isQualitasTrxValida = true;

                            Globales.cpIntegraEMV.dbgSetQualitasTipoPagosContado(typeUsuarioQualitas.TipoPagosContado);
                            Globales.cpIntegraEMV.dbgSetQualitasTipoPagosMSI(typeUsuarioQualitas.TipoPagosMSI);
                            Globales.cpIntegraEMV.dbgSetQualitasPlanPagosMSI(typeUsuarioQualitas.TipoPagosMSIPlan);
                            Globales.cpIntegraEMV.dbgSetQualitasMoneda(typeUsuarioQualitas.DeducibleMoneda);
                            Globales.cpIntegraEMV.dbgSetQualitasTipocobro(typeUsuarioQualitas.TipoCobro);

                            if (tipoVtaQualitas == 1)
                            {//Venta con presencia de tarjeta
                                menu2();
                            }
                            else
                            {
                                menu1("Venta sin presencia de tarjeta.");
                            }
                        }

                        Globales.MessageBoxMit(typeUsuarioQualitas.RespuestaMensaje);
                        Globales.isQualitasTrxValida = false;
                    }
                }
            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
            }
        }

        private void txtDeducible_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtDeducible.Text))
                if (this.txtDeducible.Text.Length < 11)
                    this.txtDeducible.Text = this.txtDeducible.Text.PadLeft(11, '0');

            typeUsuarioQualitas.NumSiniestro = txtDeducible.Text;
        }
        private void txtDeducible_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrWhiteSpace(this.txtDeducible.Text))
                    if (this.txtDeducible.Text.Length < 11)
                        this.txtDeducible.Text = this.txtDeducible.Text.PadLeft(11, '0');

                typeUsuarioQualitas.NumSiniestro = txtDeducible.Text;
                cmdAceptarDeducible_Click(sender, e);
            }

        }
        private void txtNumPoliza_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtNumPoliza.Text))
                if (this.txtNumPoliza.Text.Length < 11)
                    this.txtNumPoliza.Text = this.txtNumPoliza.Text.PadLeft(10, '0');

            typeUsuarioQualitas.NumPoliza = txtNumPoliza.Text;
        }

        private void txtNumPoliza_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrWhiteSpace(this.txtNumPoliza.Text))
                    if (this.txtNumPoliza.Text.Length < 11)
                        this.txtNumPoliza.Text = this.txtNumPoliza.Text.PadLeft(10, '0');

                typeUsuarioQualitas.NumPoliza = txtNumPoliza.Text;
                cmdAceptarPoliza_Click(sender, e);
            }
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void cbTipoCobro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                framePoliza.Visibility = Visibility.Hidden;
                frameDeducible.Visibility = Visibility.Hidden;
                strXML = "";
                resultado = "";
                fecha = "";
                if (cbTipoCobro.SelectedIndex == -1)
                {
                    Globales.MessageBoxMit("Seleccionar un tipo de cobro");
                }

                Globales.cleanValoresQualitas();

                //=======================================polizas======================================
                if (cbTipoCobro.SelectedIndex == 0)
                {
                    framePoliza.Visibility = Visibility.Visible;
                    txtNumPoliza.Focus();
                    txtNumPoliza.Text = "";
                    typeUsuarioQualitas.TipoCobro = "Poliza";
                    tipoVista = 1;
                }
                //=======================================Reedicible======================================

                if (cbTipoCobro.SelectedIndex == 1)
                {
                    frameDeducible.Visibility = Visibility.Visible;
                    txtDeducible.Text = "";
                    txtDeducible.Focus();
                    typeUsuarioQualitas.TipoCobro = "Deducible";
                    tipoVista = 2;
                }

                //====================================================================================
                if (cbTipoCobro.SelectedIndex == 2)
                {
                    Globales.isQualitasTrxValida = true;
                    typeUsuarioQualitas.TipoCobro = "Otros";
                    tipoVista = 3;
                    Globales.cpIntegraEMV.dbgSetQualitasTipocobro(typeUsuarioQualitas.TipoCobro);
                    if (tipoVtaQualitas == 1)
                    {
                        menu2();
                    }
                    else
                    {
                        menu1("Venta sin presencia de tarjeta");
                    }
                }
            }
            catch
            {

            }
        }


    }
}
