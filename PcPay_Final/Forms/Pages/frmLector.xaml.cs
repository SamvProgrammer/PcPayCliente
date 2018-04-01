using PcPay.Code.Usuario;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
    /// Interaction logic for frmLector.xaml
    /// </summary>
    public partial class frmLector : Page
    {
        public cerrarVentana cerrar;
        public frmLector()
        {
            InitializeComponent();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            string cport = "";
            if(Convert.ToBoolean(optMSR.IsChecked) || Convert.ToBoolean(optMagtek.IsChecked)){
                cmdOk.IsEnabled = false;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                TypeUsuario.IsAQ = false;
                Globales.SaveSettingString("IsAQ",(TypeUsuario.IsAQ)?"1":"0");
                if(Convert.ToBoolean(optMSR.IsChecked)){
                   
                }else if(Convert.ToBoolean(optMagtek.IsChecked)){
                   
                }
            }

            cerrar();
            Mouse.OverrideCursor = null;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblDatos.Text = Globales.EstableceLector(true);
            if (Globales.cpIntegraEMV.dbgIsUpgradeable())
            {
                cmdActualizar.Visibility = Visibility.Visible;
                pctClic2.Visibility = Visibility.Hidden;
                lblMasInfo.Visibility = Visibility.Visible;
                clic.Visibility = Visibility.Visible;
            }
            else {
                pctClic2.Visibility = Visibility.Hidden;
                lblMasInfo.Visibility = Visibility.Hidden;
                clic.Visibility = Visibility.Hidden;
            }       
        }

        private void cmdActualizar_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            cmdActualizar.IsEnabled = false;
            Globales.cpIntegraEMV.dbgUpdatePinPad();
            Globales.cpIntegraEMV.dbgEjecutaInfoEMV(false);
            lblDatos.Text = Globales.EstableceLector();
            if(Globales.cpIntegraEMV.dbgIsUpgradeable()){
                cmdActualizar.IsEnabled = true;
            }
            Mouse.OverrideCursor = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string pdf = Globales.ip + "/pgs/jsp/cpagos/Info.pdf";
            WebClient cliente = new WebClient();
            cliente.DownloadFileAsync(new Uri(pdf),Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+@"/Info.pdf");
            cliente.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(completado);
        }

        private void completado(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"/Info.pdf");
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }
    }
}
