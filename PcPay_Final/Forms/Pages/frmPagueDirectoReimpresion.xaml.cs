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
    /// Interaction logic for frmPagueDirectoReimpresion.xaml
    /// </summary>
    public partial class frmPagueDirectoReimpresion : Page
    {
        public cerrarVentana cerrar;
        public frmPagueDirectoReimpresion()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void txtNumero_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender,e);
        }

        private void optUltima_Click(object sender, RoutedEventArgs e)
        {
            
            lblNumero.Visibility = Visibility.Hidden;
            txtNumero.Visibility = Visibility.Hidden;
        }

        private void optComprobante_Click(object sender, RoutedEventArgs e)
        {
            
            lblNumero.Visibility = Visibility.Visible;
            txtNumero.Visibility = Visibility.Visible;
            txtNumero.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = null;

            if(TypeUsuario.ShowMsg){
                frmAvisoBanda aviso = new frmAvisoBanda();
                aviso.ShowDialog();
            }
        }

        private void cmdAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //string numFacE = string.Empty;

                //if(Convert.ToBoolean(optComprobante.IsChecked)){
                //    if (string.IsNullOrEmpty(txtNumero.Text))
                //    {
                //        Globales.MessageBoxMit("Falta número de comprobante");
                //        txtNumero.Focus();
                //        return;
                //    }
                //}

                //Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                //Globales.cpIntegraEMV.dbgSetUrl(Globales.ipfe);
                //if (Convert.ToBoolean(optUltima.IsChecked))
                //{
                //    numFacE = Globales.ultimaTrxPD;
                //}
                //else {
                //    numFacE = txtNumero.Text;
                //}

                //Globales.cpIntegraEMV.GetFacturaEletronicaPagueDirecto(numFacE);

                //if (Globales.cpIntegraEMV.getRspResponsePagueDirecto() != "1" && Globales.cpIntegraEMV.getRspResponsePagueDirecto() != "100")
                //{
                //    if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.getVoucherReimpresionPagueDirecto()))
                //    {
                //        Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspErrorPagueDirecto());
                //    }
                //    else
                //    {
                //        Globales.MessageBoxMit(Globales.cpIntegraEMV.getVoucherReimpresionPagueDirecto());
                //    }
                //}
                //else {
                //    if ((TypeUsuario.strTipoLector == "3" || TypeUsuario.strTipoLector == "4") && TypeUsuario.TipoImpresora == "6")
                //    {
                //        string voucherPD = "";
                //        voucherPD = Globales.cpIntegraEMV.getVoucherReimpresionPagueDirecto();
                //        if (voucherPD.Contains("@cbb"))
                //        {
                //            voucherPD.Replace("@cbb", "@cnb");
                //        }

                //        Globales.cpIntegraEMV.dbgImprimeVoucher(voucherPD);
                //    }
                //    else {
                //        Globales.printOptionPagueDirecto();
                //    }
                //}
            }
            catch {
                Globales.MessageBoxMit("Error en frmPagueDirecoReimresion cmdaceptar_click");
            }
            Mouse.OverrideCursor = null;
            
            
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }
    }
}
