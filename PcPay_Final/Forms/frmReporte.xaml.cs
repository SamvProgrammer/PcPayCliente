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

namespace PcPay.Forms
{
    /// <summary>
    /// Interaction logic for frmReporte.xaml
    /// </summary>
    public partial class frmReporte : Window
    {
        public frmReporte()
        {
            InitializeComponent();
            this.Owner = Globales.principal;
            if (Globales.isReporte)
            {
                this.Width = this.Owner.Width - 20;
                this.Height = this.Owner.Height - 10;
            }
            else
            {

                this.Height = 900;
                
            }
            Globales.isReporte = false;
        }


        public void imprimirHtml(string html, bool isReporte = false)
        {
            navegando.NavigateToString(html);
        }
        private void cargando(object sender, RoutedEventArgs e)
        {

        }

        private void BCERRAR_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
