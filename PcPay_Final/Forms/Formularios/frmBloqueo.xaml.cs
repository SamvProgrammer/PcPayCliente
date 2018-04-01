using PcPay.Code.Usuario;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace PcPay.Forms.Formularios
{
    /// <summary>
    /// Lógica de interacción para frmBloqueo.xaml
    /// </summary>
    public partial class frmBloqueo : Window
    {
        public frmBloqueo()
        {
            InitializeComponent();
            this.Owner = Globales.principal;
        }

        private const string DisableScriptError =@"function noError() {return true;}window.onerror = noError;";
        private void wbWinForms_DocumentTitleChanged(object sender, EventArgs e)
        {
            this.Title = (sender as System.Windows.Forms.WebBrowser).DocumentTitle;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebClient web = new WebClient();
            visualizador.Visibility = Visibility.Visible;
            string url = Globales.GetDataXml("url_auto_desbloq", TypeUsuario.CadenaXML);
            frmBlosbloquear n = new frmBlosbloquear(url);
            n.Show();
            Close();
            
        }

        private void navegador_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var browser = sender as WebBrowser;

            if (browser == null || browser.Document == null)
                return;

            dynamic document = browser.Document;

            if (document.readyState != "complete")
                return;

            dynamic script = document.createElement("script");
            script.type = @"text/javascript";
            script.text = @"window.onerror = function(msg,url,line){return true;}";
            document.head.appendChild(script);
        }



    }
}
