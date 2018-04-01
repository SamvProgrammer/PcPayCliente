using PcPay.Code.Modelos;
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
    /// Lógica de interacción para frmCheckout.xaml
    /// </summary>
    public partial class frmCheckout : Page
    {
        public abrir abriendo;
        public cerrarVentana cierra;
        public frmCheckout()
        {
            InitializeComponent();
            fraP2.Visibility = System.Windows.Visibility.Hidden;
            fraP1.Visibility = Visibility.Visible;

            #region
            txtCuarto.GotFocus += Globales.setFocusMit2;
            txtCuarto.LostFocus += Globales.lostFocusMit2;
            #endregion
        }


        public abrirFrm abrir;

        private void cmdAceptar1_Click(object sender, RoutedEventArgs e)
        {
            string strCadEncriptar = string.Empty;
            string strCadaux = string.Empty;
            int i;
            string sReferencia = string.Empty;
            string sImporte = string.Empty;
            string sFecha = string.Empty;
            string sOperacion = string.Empty;
            string sCcname = string.Empty;

            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdAceptar1_Click()";

            if (string.IsNullOrWhiteSpace(txtCuarto.Text))
            {
                var mensaje = Convert.ToBoolean(Referencia.IsChecked) ? "Introduzca el No. de Referencia." : "Introduzca el Dato Adicional.";
                Globales.MessageBoxMit(mensaje);
                return;
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            string strParam = Convert.ToBoolean(Referencia.IsChecked) == true ? "referencia" : "cuarto";
            strCadEncriptar = "&idcompany=" + TypeUsuario.Id_Company +
                              "&idbranch=" + TypeUsuario.Id_Branch +
                              "&usuario=" + TypeUsuario.usu +
                              "&buscar=" + txtCuarto.Text.ToUpper().Trim() +
                              "&por=" + strParam +
                              "&op=consultacheckin";
            // DoEvents

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globales.cpHTTP_Clear();
            Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
            Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
            i = 0;
            if (Globales.cpHTTP_SendcpCUCT())
            {
                Mouse.OverrideCursor = null;
                strCadaux = Globales.cpHTTP_sResult;
                if (string.IsNullOrWhiteSpace(Globales.GetDataXml("referencia" + i, strCadaux)))
                {
                    fraP1.Visibility = Visibility.Visible;
                    fraP2.Visibility = Visibility.Hidden;
                    // MousePointer = vbNormal
                    Mouse.OverrideCursor = null;
                    txtCuarto.Focus();

                    if (Convert.ToBoolean(Referencia.IsChecked))
                    {
                        Globales.MessageBoxMit("No existe referencia asociada.");
                    }
                    else {
                        Globales.MessageBoxMit("No existe Dato Adicional asociado.");
                    }
                }
                else
                {

                    strCadaux = strCadaux.Replace("-", "");
                    while (!string.IsNullOrWhiteSpace(Globales.GetDataXml("referencia" + i, strCadaux)))
                    {
                        MSFlexGrid1.Items.Add(new HotelCheck()
                        {
                            sReferencia = Globales.GetDataXml("referencia" + i, strCadaux),
                            sImporte = "$ " + Globales.GetDataXml("importe" + i, strCadaux),
                            sFecha = Globales.GetDataXml("fecha" + i, strCadaux),
                            sOperacion = Globales.GetDataXml("operacion" + i, strCadaux),
                            sCcname = Globales.GetDataXml("cc_name" + i, strCadaux),
                            vacio = "   "
                        });
                        i++;

                    }



                    fraP1.Visibility = Visibility.Hidden;
                    fraP2.Visibility = Visibility.Visible;

                }
            }

            Mouse.OverrideCursor = null;


        }



        public string NOMBRE_GENERAL = "frmCheckout";

        private void cmdCancelar1_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdCancelar1_Click()";
            Globales.botonSalir();
        }

        private void cmdAceptar2_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdAceptar2_Click()";
            Globales.bolActivaMotoP = true;
            if (string.IsNullOrWhiteSpace(Globales.InfoCheckOut))
            {
                Globales.MessageBoxMit("Debe seleccionar una operación.");
                cmdAceptar2.IsEnabled = false;
                return;
            }
            HotelCheck info = (HotelCheck)MSFlexGrid1.SelectedItem;
            if (info == null)
            {
                Globales.MessageBoxMit("Debe seleccionar una operación.");
                cmdAceptar2.IsEnabled = false;
                return;
            }

            // Guarda Info de CheckIn
            Globales.InfoCheckOut = info.sReferencia + "|" + info.sImporte + "|" + info.sFecha + "|" + info.sOperacion + "|" + info.sCcname;
            frmProcesaCheckout frmout = new frmProcesaCheckout();
            frmout.abrir = abrir;
            frmout.abriendo = abriendo;
            frmout.cierra = cierra;
            abrir(frmout);
            Globales.InfoCheckOut = "";

        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void txtCuarto_GotFocus(object sender, RoutedEventArgs e)
        {
        }

   
        private void Referencia_Checked(object sender, RoutedEventArgs e)
        {
            
           
       
                
        }

        private void Referencia2_Checked(object sender, RoutedEventArgs e)
        {
                        
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void MSFlexGrid1_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void escogerTransaccion(object sender, SelectionChangedEventArgs e)
        {
            DataGrid tablita = sender as DataGrid;
            HotelCheck datos = tablita.SelectedItem as HotelCheck;
            Globales.InfoCheckOut = datos.sOperacion;
            cmdAceptar2.IsEnabled = true;
             
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtCuarto.Focus();
        }

        private void txtCuarto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }

        private void Referencia_Click(object sender, RoutedEventArgs e)
        {
            txtCuarto.Focus();
            txtCuarto.Text = "";
        }

    }
}
