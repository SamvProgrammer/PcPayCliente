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
using System.Xml;

namespace PcPay.Forms.Pages
{
    /// <summary>
    /// Interaction logic for frmImpresion.xaml
    /// </summary>
    public partial class frmImpresion : Page
    {
        public cerrarVentana cerrando { get; set; }
        private Dictionary<string, string> diccionario = new Dictionary<string, string>();
        private string strFolioReImp { get; set; }
        public string caption = "frmReimpresion";
        public frmImpresion()
        {
            InitializeComponent();
            this.txtReservacion.GotFocus += Globales.setFocusMit2;
            this.txtReservacion.LostFocus += Globales.lostFocusMit2;

            this.fraP2.Visibility = Visibility.Hidden;
        }

        private void txtReservacionFocus(object sender, RoutedEventArgs e)
        {

            if (Convert.ToBoolean(optFolio.IsChecked))
            {
              //  lblMensaje.Text = "Indica el número de operación del vucher a consultar. Haz clic en el boton aceptar";
            }
            else
            {
               // lblMensaje.Text = "Indica la referencia del vucher a consultar. Haz clic en el boton aceptar";
            }
        }

        private void cargarVentana(object sender, RoutedEventArgs e)
        {
            Globales.cpIntegraEMV.dbgEndOperation();
            Globales.strNombreFP = NOMBRE_GENERAL + "Form_load()";
            txtReservacion.Focus();


        }

        public string NOMBRE_GENERAL = "Formulario impresion";

        private void radio1Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(optReservacion.IsChecked))
            {
                txtReservacion.MaxLength = 30;
                txtReservacion.Text = "";
                txtReservacion.Focus();
                lblReferencia.Content = "Referencia";
            }
        }

        private void radio2Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(optFolio.IsChecked))
            {
                lblReferencia.Content = "Número de folio";
                txtReservacion.MaxLength = 9;
                txtReservacion.Text = "";
                txtReservacion.Focus();
            }
        }

        private void aceptarClick(object sender, RoutedEventArgs e)
        {
            byte bytVoucher, i;
            string sFolio, sReservacion, sMonto, sFecha, sCcname, sOperacion;
            string strCadEncriptar, strCadaux;
            if (string.IsNullOrWhiteSpace(txtReservacion.Text))
            {
                if (Convert.ToBoolean(optReservacion.IsChecked))
                {
                    Globales.MessageBoxMit("Introduzca el No. de " + Convert.ToString(optReservacion.Content) + ".");
                }
                else
                {
                    Globales.MessageBoxMit("Introduzca el " + Convert.ToString(optFolio.Content) + ".");
                }
                return;
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (Convert.ToBoolean(optReservacion.IsChecked))
            {
                strCadEncriptar = "&idcompany=" + TypeUsuario.Id_Company +
                    "&idbranch=" + TypeUsuario.Id_Branch +
                    "&reservacion=" + txtReservacion.Text +
                    "&op=consvouch";
            }
            else
            {
                strCadEncriptar = "&idcompany=" + TypeUsuario.Id_Company +
                    "&idbranch=" + TypeUsuario.Id_Branch +
                    "&folio=" + txtReservacion.Text +
                    "&op=consvouch";
            }

            Globales.cpHTTP_Clear();
            Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
            Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
            if (Globales.cpHTTP_SendcpCUCT())
            {
                try
                {
                    string valor = Globales.cpHTTP_sResult;
                    valor = "<padre><desc>" + Globales.GetDataXml("desc", valor) + "</desc></padre>";
                    XmlDocument documento = new XmlDocument();
                    documento.LoadXml(valor);
                    XmlNode aux = documento.GetElementsByTagName("desc")[0];
                    XmlNodeList lista = aux.ChildNodes;
                    //string[] lista1 = { "FOLIO", "FACTURA", "MONTO", "FECHA", "CLIENTE", "OPERACIÓN", "FIRMA" };
                    //string[] lista2 = { "folio", "reservacion", "monto", "fecha", "cc_name", "tp_operacion", "st_firma" };
                    string[] lista1 = { "FOLIO", "FACTURA", "MONTO", "FECHA", "CLIENTE", "OPERACIÓN" };
                    string[] lista2 = { "folio", "reservacion", "monto", "fecha", "cc_name", "tp_operacion" };

                    //for (int x = 0; x < lista1.Length; x++)
                    //{
                    //    DataGridTextColumn textoCabecera = new DataGridTextColumn();
                    //    textoCabecera.Header = lista1[x];
                    //    textoCabecera.Binding = new Binding((lista2[x]));
                    //    tabla.Columns.Add(textoCabecera);
                    //}

                    List<reimpresionVoucher> listReimpresion = new List<reimpresionVoucher>();
                    foreach (XmlNode item in lista)
                    {
                        XmlNodeList valores = item.ChildNodes;
                        listReimpresion.Add(new reimpresionVoucher()
                        {
                            folio = valores[0].InnerText,
                            reservacion = valores[1].InnerText,
                            fecha = valores[2].InnerText,
                            _fecha_date = Convert.ToDateTime(valores[2].InnerText),
                            monto = String.Format("{0:C}", Convert.ToDouble(valores[3].InnerText)),
                            cc_name = valores[4].InnerText,
                            tp_operacion = valores[5].InnerText
                        });
                        diccionario.Add(valores[0].InnerText, valores[6].InnerText);
                    }

                    listReimpresion = listReimpresion.OrderByDescending(o => o._fecha_date).ToList();
                    listReimpresion.ForEach(delegate(reimpresionVoucher o)
                    {
                        tabla.Items.Add(o);
                    });



                    fraP1.Visibility = Visibility.Hidden;
                    fraP2.Visibility = Visibility.Visible;
                  //  lblMensaje.Text = "Selecciona la transacción a reimprimir y haz clic en el botón\n\"Aceptar\"";
                }
                catch
                {
                    fraP1.Visibility = Visibility.Visible;
                    fraP2.Visibility = Visibility.Hidden;
                    Mouse.OverrideCursor = null;
                    Globales.MessageBoxMit(Globales.GetDataXml("desc", Globales.cpHTTP_sResult));
                }
            }
            else
            {
                Globales.MessageBoxMit("Error en el mensaje");
            }

            Mouse.OverrideCursor = null;
        }

        private void cmdAceptar2Click(object sender, RoutedEventArgs e)
        {

            string strCadEncriptar = string.Empty;
            Globales.strNombreFP = NOMBRE_GENERAL + " cmdAceptar2();";
            string op = "";
            bool isFirmaPanel = false;
            cmdAceptar2.IsEnabled = false;
            if (!string.IsNullOrWhiteSpace(strFolioReImp))
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                string[] arreglo = strFolioReImp.Split(',');

                isFirmaPanel = (diccionario[arreglo[0]] == "1")?true:false;
                if (isFirmaPanel && caption.Contains("Reenvío"))
                {
                    string folio = arreglo[0];
                    Globales.cpIntegraEMV.sndReenviaVoucherFirmaEnPanel(Globales.ipFirmaPanel, Globales.cpIntegraEMV.dbgGetId_Company(), Globales.cpIntegraEMV.dbgGetId_Branch(),
                        Globales.cpIntegraEMV.dbgGetCountry(), Globales.cpIntegraEMV.dbgGetUser(), Globales.cpIntegraEMV.dbgGetPass(), folio);
                }
                else
                {
                    if (TypeUsuario.TipoImpresora == "1" || TypeUsuario.TipoImpresora == "4")
                    {
                        op = "reimpvouch";
                    }
                    else
                    {
                        op = "reimpvouchtermica";
                    }
                    Globales.cpHTTP_Clear();
                    Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
                    string nvoucher = string.Empty;
                    if (TypeUsuario.TipoImpresora == "6")
                    {
                        nvoucher = "true";
                        string auxop = "";
                        auxop = arreglo[0];
                        Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                        TypeUsuario.strVoucherCoP = Globales.cpIntegraEMV.sndReimpresion(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country, auxop);
                    }
                    else
                    {
                        nvoucher = "";
                    }
                    strCadEncriptar = "&folio=" + arreglo[0] +
                        "&empresa=" + TypeUsuario.Id_Company +
                        "&sucursal=" + TypeUsuario.Id_Branch +
                        "&op=" + op +
                        "&co=true&nvoucher=" + nvoucher;
                    Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
                    switch (TypeUsuario.TipoImpresora)
                    {
                        case "1":

                            if (Globales.cpHTTP_SendcpCUCT())
                            {
                                TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                                Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                            }
                            break;
                        case "2":
                            if (Globales.cpHTTP_SendcpCUCT())
                            {

                            }
                            break;
                        case "3":
                         nvoucher = "true";
                        string auxop = "";
                        auxop = arreglo[0];
                        Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                        TypeUsuario.strVoucherCoP = Globales.cpIntegraEMV.sndReimpresion(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country, auxop);
                        Globales.imprimirEpson();
                            break;
                        case "4":
                            if (Globales.cpHTTP_SendcpCUCT())
                            {
                                TypeUsuario.strVoucher = Globales.cpHTTP_sResult;
                                Globales.Imprimir_HTML(TypeUsuario.strVoucher);
                            }
                            break;
                        case "5":
                            if (Globales.cpHTTP_SendcpCUCT())
                            {

                            }
                            break;
                        case "6":
                            string Rsp = Globales.cpIntegraEMV.getRspDsResponse();
                            switch (Rsp)
                            {
                                case "error":
                                    Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspDsError());
                                    break;
                                case "denied":
                                    Globales.MessageBoxMit("No fue posible realizar la reimpresión");
                                    break;
                                case "approved":
                                    Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                                    break;
                            }
                            break;
                        default:
                            Globales.MessageBoxMit("Debe seleccionar impresora... ir al menu correspondiente");
                            break;

                    }
                }
                Mouse.OverrideCursor = null;
                cerrando();
            }
            else
            {
                Globales.MessageBoxMit("Debe seleccionar una transacción.");
                cmdAceptar2.IsEnabled = false;
            }
        }

        private void escogerTransaccion(object sender, SelectionChangedEventArgs e)
        {
            DataGrid tablita = sender as DataGrid;
            reimpresionVoucher voucher = tablita.SelectedItem as reimpresionVoucher;
            strFolioReImp = voucher.folio + "," + voucher.reservacion + "," + voucher.fecha + "," + voucher.monto
                + "," + voucher.cc_name + "," + voucher.tp_operacion + "," + voucher.st_firma;
            cmdAceptar2.IsEnabled = true;
            
        }

        private void cmdCancelarClick(object sender, RoutedEventArgs e)
        {
            cerrando();
        }

        private void txtReservacion_LostFocus(object sender, RoutedEventArgs e)
        {
            txtReservacion.Text = txtReservacion.Text.ToUpper();
        }

        private void txtReservacion_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);

        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void optReservacion_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void optFolio_Checked(object sender, RoutedEventArgs e)
        {

        }


        private void sendEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.cmdAceptar1.PerformClick();
        }
    }
    public partial class reimpresionVoucher
    {
        public string folio { get; set; }
        public string reservacion { get; set; }
        public string fecha { get; set; }
        public DateTime _fecha_date { get; set; }
        public string monto { get; set; }
        public string cc_name { get; set; }
        public string tp_operacion { get; set; }
        public string st_firma { get; set; }

    }
}
