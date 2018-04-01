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
using System.Windows.Shapes;

namespace PcPay.Forms.Formularios
{
    /// <summary>
    /// Lógica de interacción para frmBlosbloquear.xaml
    /// </summary>
    public partial class frmBlosbloquear : Window
    {
        public frmBlosbloquear(string url)
        {
            InitializeComponent();
            (navegador.Child as System.Windows.Forms.WebBrowser).ScriptErrorsSuppressed = true;
            (navegador.Child as System.Windows.Forms.WebBrowser).Navigate(url);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void wbWinForms_DocumentTitleChanged(object sender, EventArgs e)
        {

        }
    }
}
