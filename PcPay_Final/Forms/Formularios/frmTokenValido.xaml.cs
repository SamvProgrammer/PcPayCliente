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

namespace PcPay.Forms.formularios
{
    /// <summary>
    /// Lógica de interacción para frmTokenValido.xaml
    /// </summary>
    public partial class frmTokenValido : Window
    {
        public string strToken { set; get; }
        public frmTokenValido(string token)
        {
            InitializeComponent();
            this.Owner = Globales.principal;

            this.Height = this.Owner.Height;
            this.Width = this.Owner.Width;
            this.lblToken.Content = token;
        }


        private void cmdCopiar_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Clipboard.Clear();            
            System.Windows.Forms.Clipboard.SetText(lblToken.Content.ToString()); 
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
