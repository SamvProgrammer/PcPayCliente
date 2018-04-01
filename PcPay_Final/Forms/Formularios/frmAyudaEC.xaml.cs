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
    /// Lógica de interacción para frmAyudaEC.xaml
    /// </summary>
    public partial class frmAyudaEC : Window
    {
        public frmAyudaEC()
        {
            InitializeComponent();
        }

        private void cerrar(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
