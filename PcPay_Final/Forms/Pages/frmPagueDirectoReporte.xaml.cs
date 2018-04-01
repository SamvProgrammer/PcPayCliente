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
    /// Interaction logic for frmPagueDirectoReporte.xaml
    /// </summary>
    public partial class frmPagueDirectoReporte : Page
    {
        public frmPagueDirectoReporte()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           if(TypeUsuario.ShowMsg){
               frmAvisoBanda banda = new frmAvisoBanda();
               banda.ShowDialog();
           }
        }

        private void opActual_Click(object sender, RoutedEventArgs e)
        {
        }

        private void opAnterior_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cmdCancelar_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
        }
        public cerrarVentana cerrar;

        private void cmdAceptar_Click(object sender, RoutedEventArgs e)
        {
            //Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            //string tipoReporte = "";
            //Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
            //if (Convert.ToBoolean(opActual.IsChecked))
            //{
            //    tipoReporte = "08";
            //}
            //else {
            //    tipoReporte = "09";
            //}

            //Globales.cpIntegraEMV.GetReportePagueDirecto(TypeUsuario.usu,TypeUsuario.Pass,tipoReporte);

            ////Valida si se presenta un error.
            //if(Globales.cpIntegraEMV.getRspResponsePagueDirecto() != "1"){
            //    Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspErrorPagueDirecto());
            //    Mouse.OverrideCursor = null;
            //    return;
            //}

            //if ((TypeUsuario.strTipoLector == "3" || TypeUsuario.strTipoLector == "4") && TypeUsuario.TipoImpresora == "6")
            //{
            //    Globales.cpIntegraEMV.dbgPrintVoucher(Globales.cpIntegraEMV.getVoucherReimpresionPagueDirecto());
            //}
            //else {
            //    Globales.PrintOptions(Globales.cpIntegraEMV.getVoucherReimpresionPagueDirecto());
            //}
            //Mouse.OverrideCursor = null;
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }
    }
}
