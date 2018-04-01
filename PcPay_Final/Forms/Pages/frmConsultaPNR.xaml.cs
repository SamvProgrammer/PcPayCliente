using PcPay.Code.Modelos;
using PcPay.Code.Usuario;
using PcPay.Forms.formularios;
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
    /// Lógica de interacción para frmConsultaPNR.xaml
    /// </summary>
    public partial class frmConsultaPNR : Page
    {
        public frmConsultaPNR()
        {
            InitializeComponent();
            Cargar();
        }
        public cerrarVentana cerrar;


        private void cmdAceptar1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string strCadEncriptar = string.Empty;
                int bytVoucher, i;
                string sReferencia, sImporte, sFecha, sOperacion, sBoletos = string.Empty;
                //alto = 0
                //alto2 = 0
                //strNombreFP = NOMBRE_GENERAL + ".cmdAceptar1_Click()"

         

                if (string.IsNullOrWhiteSpace(txtPNR.Text))
                {
                    Globales.MessageBoxMit("Introduzca el PNR.");
                    return;
                }
                if(string.IsNullOrWhiteSpace(txtFechaVenta.Text)){
                    Globales.MessageBoxMit("Favor de ingresar la fecha de venta.");
                    return;
                }

                strCadEncriptar = "&idcompany=" + TypeUsuario.Id_Company +
                                  "&idbranch=" + TypeUsuario.Id_Branch +
                                  "&usuario=" + TypeUsuario.usu +
                                  "&pnr=" + txtPNR.Text.Trim() +
                                  "&fecha=" + txtFechaVenta.Text.Replace("/", "") +
                                  "&op=consultaPNR";


                Globales.cpHTTP_Clear();

                Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
                Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);

                i = 0;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                if (Globales.cpHTTP_SendcpCUCT())
                {
                    Mouse.OverrideCursor = null;
                    string strCadaux = Globales.cpHTTP_sResult;
                    if (string.IsNullOrWhiteSpace(Globales.GetDataXml("referencia" + i, strCadaux)))
                    {
                        Globales.MessageBoxMit(Globales.GetDataXml("nb_response", strCadaux));
                        return;
                    }
                    else
                    {
                        strCadaux = strCadaux.Replace("-", "");

                        string[] lista = { "PNR", "FECHA", "IMPORTE", "OPERACIÓN",""};
                        string[] lista2 = { "sReferencia", "sFecha", "sImporte", "sOperacion", "sBoletos" };
                        for (int x = 0; x < lista.Length; x++)
                        {
                            DataGridTextColumn textoCabecera = new DataGridTextColumn();
                            textoCabecera.Header = lista[x];
                            textoCabecera.Binding = new Binding((lista2[x]));
                            MSFlexGrid1.Columns.Add(textoCabecera);
                        }

                                      

                        while (!string.IsNullOrWhiteSpace(Globales.GetDataXml("referencia" + i, strCadaux)))
                        {
                            MSFlexGrid1.Items.Add(new EmpresaBoletos()
                           {
                               sReferencia = Globales.GetDataXml("referencia" + i, strCadaux),
                               sFecha = Globales.GetDataXml("fecha" + i, strCadaux),
                               sImporte = "$ " + Globales.GetDataXml("importe" + i, strCadaux),
                               sOperacion = Globales.GetDataXml("operacion" + i, strCadaux),
                               sBoletos = (Globales.GetDataXml("boletos" + i, strCadaux) == "null") ? "" : Globales.GetDataXml("boletos" + i, strCadaux)
                           });
                            i++;
                        }


                        fraP1.Visibility = Visibility.Hidden;
                        fraP2.Visibility = Visibility.Visible;

                    }

                }

 

            }
            catch
            {
            }
        }

        private void cmdAceptar2_Click(object sender, RoutedEventArgs e)
        {
            
            EmpresaBoletos selecionada = (EmpresaBoletos)MSFlexGrid1.SelectedItem;
            if(selecionada == null){
                Globales.MessageBoxMit("Se debe seleccionar alguna operación");
                return;
            }

            Globales.InfoPNR = selecionada.sBoletos+"|"+selecionada.sFecha+"|"+selecionada.sImporte+"|"+selecionada.sOperacion+"|"+selecionada.sReferencia;


            if (Globales.InfoPNR.Length < 5)
            {
                Globales.MessageBoxMit("Debe de seleccionar una operación");
            }
            else
            {



                frmBoletosAerolinea frmBoletosAerolinea = new frmBoletosAerolinea();     
                frmBoletosAerolinea.numOperPNR = selecionada.sOperacion;
                frmBoletosAerolinea.boletosPNR = selecionada.sBoletos;
                frmBoletosAerolinea.isUpdatePNR = true;
                frmBoletosAerolinea.cerrar = cerrar;
                frmBoletosAerolinea.Show();
             
            }
        }


        private void Cargar()
        {

            txtFechaVenta.DisplayDateEnd = DateTime.Now;
            fraP1.Visibility = Visibility.Visible;



        }

        private void Alfa(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender, e);
        }

    

        private void cmdCancelar1_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender,e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            fraP1.Visibility = Visibility.Visible;
            fraP2.Visibility = Visibility.Hidden;
        }

        private void txtPNR_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }

        private void cmdCancelar2_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void MSFlexGrid1_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid tablita = sender as DataGrid;
            EmpresaBoletos voucher = tablita.SelectedItem as EmpresaBoletos;
        }
    }
}
