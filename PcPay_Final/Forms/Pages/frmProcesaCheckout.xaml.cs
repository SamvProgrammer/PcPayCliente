using PcPay.Code.Configuracion;
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

namespace PcPay.Forms.Pages
{
    /// <summary>
    /// Lógica de interacción para frmProcesaCheckout.xaml
    /// </summary>
    public partial class frmProcesaCheckout : Page
    {
        public cerrarVentana cierra;
        public abrirFrm abrir;
        public string lblOperacion { get; set; }
        public string txtNumOper { get; set; }
        public string txtNumAut { get; set; }
        public string usuario { get; set; }
        public string sucursal { get; set; }
        //public string  noOperacion { get; set; }

        public frmProcesaCheckout()
        {
            InitializeComponent();
            Cargar();
            Importe.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;
            Importe.GotFocus += Globales.setFocusMit2;
            Importe.LostFocus += Globales.lostFocusMit2;
            Importe.MaxLength = 9;
        }



        private void CmdEnviar_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".CmdEnviar_Click()";

            string mensaje = string.Empty;
            if (string.IsNullOrWhiteSpace(Importe.Text))
            {

                Globales.MessageBoxMit("Introduzca el importe.");
                Importe.Focus();
            }
            else
            {

                lblimportecheckin.Content = lblimportecheckin.Content.ToString().Replace(",", "");
                string importe = lblimportecheckin.Content.ToString();
                importe = importe.Replace("$", "");
                importe = importe.Trim();

                if (Convert.ToDouble(Importe.Text) > Convert.ToDouble(importe))
                {
                    double ImporteRA;
                    ImporteRA = Convert.ToDouble(Importe.Text) - Convert.ToDouble(lblimportecheckin.Content.ToString().Replace("$", "").Trim());

                    Globales.cpIntegracion_Clear();
                    Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL_RA;
                    CmdEnviar.IsEnabled = false;

                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                    Globales.cpIntegraEMV.sndReautorizacionMOTO(TypeUsuario.usu,
                                                    TypeUsuario.Pass,
                                                    "",
                                                    TypeUsuario.Id_Company,
                                                    TypeUsuario.Id_Branch,
                                                    TypeUsuario.country,
                                                    ImporteRA.ToString(),
                                                   lblOperacion);
                    Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();
                    Mouse.OverrideCursor = null;
                    string caso = Globales.GetDataXml("response", Globales.cpIntegracion_sResult).ToLower();
                    switch (caso)
                    {
                        case "approved":
                            goto DoCheckout;
                            break;
                        default:

                            Globales.MessageBoxMit("No es posible realizar el checkout");
                            CmdEnviar.IsEnabled = true;
                            CmdEnviar.Visibility = Visibility.Visible;
                            return;
                            break;
                    }
                }
                else
                {
                    goto DoCheckout;
                }

            DoCheckout:
                CmdEnviar.IsEnabled = false;

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                string strTypeC = string.Empty;
                Globales.cpIntegracion_Clear();

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                Globales.cpIntegraEMV.sndCheckOutMOTO(TypeUsuario.usu,
                                                TypeUsuario.Pass,
                                                "",
                                                TypeUsuario.Id_Company,
                                                TypeUsuario.Id_Branch,
                                                TypeUsuario.country,
                                                Importe.Text,
                                               this.lblOperacion);
                Globales.cpIntegracion_sResult = Globales.cpIntegraEMV.getRspXML();

                Mouse.OverrideCursor = null;
                string strCadEncriptar = string.Empty;

                string case2 = Globales.GetDataXml("response", Globales.cpIntegracion_sResult).ToLower();

                switch (case2)
                {
                    case "approved":
                        Importe.IsEnabled = false;
                        mensaje =  Globales.GetDataXml("auth", Globales.cpIntegracion_sResult);
                        Globales.MessageBoxMitApproved(mensaje);
                        TypeUsuario.strVoucherCoP = (Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult)).Trim();
                        TypeUsuario.strVoucherCoP = (Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult)).Trim();


                        //'************************************************************************************************
                        //''********************************** FIRMA EN PANEL *********************************************
                        //'VALIDACIONES PARA LA FIRMA EN PANEL

                        //'Texto de marca de agua ceriroji
                        string textoAgua = string.Empty;
                        textoAgua = "Folio: " + Globales.cpIntegraEMV.getRspOperationNumber() + "\n";
                        textoAgua += "Auth: " + Globales.cpIntegraEMV.getRspAuth() + "\n";
                        textoAgua += "Importe: " + Globales.cpIntegraEMV.getTx_Amount() + "\n";
                        textoAgua += "Fecha: " + Globales.cpIntegraEMV.getRspDate() + "\n";
                        textoAgua += "Merchant: " + Globales.cpIntegraEMV.getRspDsMerchant() + "\n";
                        textoAgua += " " + "\n";
                        textoAgua += "___________________" + "\n";
                        textoAgua += "FIRMA DIGITALIZADA:" + "\n";


                        //'************************************************************************************************
                        //'valida si la tarjeta es Chip and Pin
                        bool IsChipAndPin, esQPS;
                        string cadenaHexFirma = string.Empty;
                        int tipoVta;
                        IsChipAndPin = false;
                        esQPS = false;
                        tipoVta = 1;

                        //'valida si es chip and pin
                        if (Globales.cpIntegraEMV.chkCc_IsPin() == "01")
                        {
                            IsChipAndPin = true;
                        }

                        //'************************************************************************************************
                        //'valida si la venta es QPS
                        if (Globales.cpIntegraEMV.getRspVoucher().Contains("@cnn Autorizado sin Firma ") && tipoVta == 1 && !IsChipAndPin)
                        {
                            esQPS = true;
                        }

                        //'************************************************************************************************
                        //'Si la PinPad no soporta Firma en Panel y no es touch, sigue el flujo normal de PcPay
                        if (!Globales.cpIntegraEMV.EsTouch() && !Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                        {
                            goto GoImpresion;
                        }

                        //'************************************************************************************************
                        //'Si la PinPad Soporta Firma en Panel y no es touch,
                        if (!Globales.cpIntegraEMV.EsTouch() && Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                        {

                            // 'Llama a la función de obtener la firma en Panel
                            cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(false, textoAgua, Globales.cpIntegraEMV.getRspVoucher(),
                                                                            IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), (short)tipoVta, esQPS);

                            if (!cadenaHexFirma.Contains("Error"))
                            {
                                //'Llama a la funcion de Guardar la firma
                                if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Program.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                {
                                    imgEmailFirmaPanel.Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                Globales.MessageBoxMit("No se pudo obtener la imagen del PinPad");
                            }
                            goto GoImpresion;
                        }

                        //'************************************************************************************************
                        //'SI EL DISPOSITIVO TIENE CAPACIDAD TOUCH
                        if (Globales.cpIntegraEMV.EsTouch())
                        {
                            if (TypeUsuario.UtilizarCapacidadTouch && TypeUsuario.EnviarVoucherMail)
                            {
                                if (!Globales.ObtieneFirmaPanel(Program.ipFirmaPanel, textoAgua, (short)tipoVta, IsChipAndPin, esQPS))
                                    goto GoImpresion;
                                else
                                {
                                    cmdVoucher.Visibility = Visibility.Visible;
                                    cmdVoucher.IsEnabled = false;
                                    CmdEnviar.Visibility = Visibility.Hidden;

                                    goto finaliza;
                                }
                            }
                        }


                        if (TypeUsuario.UtilizarCapacidadTouch && !TypeUsuario.EnviarVoucherMail)
                        {

                            //   'Llama a la función de obtener la firma en Panel
                            cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(true, textoAgua,
                                                                                Globales.cpIntegraEMV.getRspVoucher(), IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), (short)tipoVta, esQPS);

                            if (!cadenaHexFirma.Contains("Error"))
                            {
                                // 'Llama a la funcion de Guardar la firma
                                if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Program.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                {
                                    imgEmailFirmaPanel.Visibility = Visibility.Visible;
                                }
                                else
                                    Globales.MessageBoxMit("No se pudo obtener la imagen del PinPad" + "\n" + cadenaHexFirma);

                                goto GoImpresion;
                            }
                        }
                        if (!TypeUsuario.UtilizarCapacidadTouch && !TypeUsuario.EnviarVoucherMail)
                        {
                            if (Globales.cpIntegraEMV.getRspSoportaFirmaPanel())
                            {
                                //  'Llama a la función de obtener la firma en Panel
                                cadenaHexFirma = Globales.cpIntegraEMV.sndObtieneFirmaPanel(false, textoAgua,
                                                                                    Globales.cpIntegraEMV.getRspVoucher(), IsChipAndPin, Globales.cpIntegraEMV.chkPp_Trademark(), (short)tipoVta, esQPS);
                                if (!cadenaHexFirma.Contains("Error"))
                                {
                                    // 'Llama a la funcion de Guardar la firma
                                    if (Globales.cpIntegraEMV.sndGuardaFirmaPanel(cadenaHexFirma, Program.ipFirmaPanel, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.chkPp_Serial()))
                                    {
                                        imgEmailFirmaPanel.Visibility = Visibility.Visible;
                                    }
                                    else
                                    {
                                        Globales.MessageBoxMit("No se pudo obtener la imagen del PinPad" + "\n" + cadenaHexFirma);
                                    }
                                }
                                goto GoImpresion;
                            }
                        }

                     //    '************************************************************************************************************
                    GoImpresion:

                        //  Select Case Mid(TypeUsuario.TipoImpresora, 1, 1)
                        cmdVoucher.IsEnabled = true;
                        CmdEnviar.Visibility = Visibility.Hidden;
                        escogerImpresora();

                        if (TypeUsuario.IsAQ)
                        {
                            //VerificaVoucher
                        }

                        CmdEnviar.Visibility = Visibility.Hidden;
                        cmdVoucher.Visibility = Visibility.Visible;

                    finaliza:
                        //    'fin del case

                        //   '''*******************************FACTURA ELECTRONICA*********************************
                        Globales.cpIntegracion_sResult = Globales.cpIntegracion_sResult.Replace("<amount/>", "<amount>" + Importe.Text + "</amount>");
                        if (Globales.FacturaE == "1")
                        {
                            if (Globales.MessageConfirm("¿Desea factura electrónica?"))
                            {
                                frmPreguntaFactura fpf = new frmPreguntaFactura();
                                fpf.abrirFrmAhora = abrir;
                                fpf.cerraPage = cierra;
                                abrir(fpf);
                            }
                            else
                            {
                                Globales.XMLFacturaE = "";
                            }
                        }
                        break;
                    //    ''''***********************************************************************************



                    case "denied":
                        Importe.IsEnabled = false;
                        mensaje = " msjRech" + "\n" + Globales.GetDataXml("cd_response", Globales.cpIntegracion_sResult) + " " + Globales.GetDataXml("friendly_response", Globales.cpIntegracion_sResult);
                        Globales.MessageBoxMitDenied(mensaje);
                        CmdEnviar.Visibility = Visibility.Hidden;
                        break;
                    case "error":                        
                        mensaje = Globales.GetDataXml("nb_error", Globales.cpIntegracion_sResult);
                        Globales.MessageBoxMitError(mensaje);
                        CmdEnviar.Visibility = Visibility.Visible;
                        break;
                    default:
                        mensaje = "Verifique su conexión de Internet.";
                        Globales.MessageBoxMitError(mensaje);
                        CmdEnviar.Visibility = Visibility.Visible;
                        Mouse.OverrideCursor = null;
                        CmdEnviar.IsEnabled = true;
                        break;
                }
            }
        }



        private void cmdVoucher_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdVoucher_Click()";
            escogerImpresora();
        }
        private void escogerImpresora()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                TypeUsuario.strVoucherCoP = Globales.GetDataXml("voucher", Globales.cpIntegracion_sResult).Trim();
                switch (TypeUsuario.TipoImpresora)
                {
                    case "1":
                        if (Globales.VoucherHtml(Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult),
                            TypeUsuario.Id_Company, TypeUsuario.Id_Branch, "impvouch", "false"))
                        {
                            TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                            Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                        }
                        cmdVoucher.IsEnabled = true;
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
                        break;
                    case "5":
                        break;
                    case "6":
                        cmdVoucher.IsEnabled = false;
                        Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                        cmdVoucher.IsEnabled = true;
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

        private void imgFirmaPanel_Click(object sender, RoutedEventArgs e)
        {
            string strMail = string.Empty;
            string strError = string.Empty;
            int reintentos = 0;

            strMail = Globales.cpIntegraEMV.ObtieneFormMail("-1", "");


            if (!string.IsNullOrWhiteSpace(strMail))
            {

            ReintentaEnviarMail:
                //'************************************************************************************
                //'Se envía el Voucher por correo electrónico
                Globales.cpIntegraEMV.sndEnviaMailFirmaPanel(strMail, Globales.cpIntegraEMV.getRspOperationNumber(), Globales.cpIntegraEMV.dbgGetId_Company(),
                                                        Globales.cpIntegraEMV.dbgGetId_Branch(), Globales.cpIntegraEMV.dbgGetCountry(), Globales.cpIntegraEMV.dbgGetUser(), Globales.cpIntegraEMV.dbgGetPass(), Program.ipFirmaPanel);

                if ((Globales.GetDataXml("response", Globales.cpIntegraEMV.getRspXML())).ToLower() == "true")
                {
                    Globales.cpIntegraEMV.ObtieneFormMail("0", strMail);
                }
                else
                {
                    if (reintentos > 2)
                    {
                        strError = Globales.GetDataXml("nb_error", Globales.cpIntegraEMV.getRspXML());
                        Globales.MessageBoxMit("No se pudo enviar el Voucher." + "\n" + strError);
                    }
                    else
                    {
                        reintentos++;
                        goto ReintentaEnviarMail;
                    }

                }
            }
            else
            {
                Globales.MessageBoxMit("No se pudo enviar el Correo electrónico.." + "\n" + "Correo electrónico vacío");

            }


        }

        private void Importe_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
        }

        public string NOMBRE_GENERAL = "frmProcesaCheckout";

        public void limpia()
        {
            try
            {
                Globales.strNombreFP = NOMBRE_GENERAL + ".Limpia()";
                this.txtNumOper = string.Empty;
                this.txtNumAut = string.Empty;
                this.lblOperacion = string.Empty;

                Importe.Text = "";
                Importe.IsEnabled = true;

                this.sucursal = TypeUsuario.Id_Branch + "\n" + TypeUsuario.nb_branch;
                this.usuario = TypeUsuario.usu;

                CmdEnviar.Visibility = Visibility.Visible;


            }
            catch
            {

            }

        }


        public void Cargar()
        {

            CargaLabels(Globales.InfoCheckOut.Replace("$$", "$"));
        }

        private void CargaLabels(string InfoCheckOut)
        {
            if (!string.IsNullOrWhiteSpace(InfoCheckOut))
            {
                string[] dts = InfoCheckOut.Split('|');
                lblCuarto.Content = dts[0];
                lblimportecheckin.Content = dts[1];
                lblFechacheckin.Content = dts[2];
                this.lblOperacion = dts[3];
                nombre.Content = dts[4];
            }
            else
            {
                Globales.MessageBoxMit("Hubo un problema al cargar datos del Check out");
            }
        }





        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void ImporteFormato(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void cmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            abriendo();
        }




        public Forms.abrir abriendo { get; set; }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void imgEmailFirmaPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Globales.sendMailFirmaPanel_MouseDown(sender,e);    
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            imgEmailFirmaPanel.Visibility = Visibility.Hidden;
        }
    }
}
