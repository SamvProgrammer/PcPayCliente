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
    /// Lógica de interacción para frmTip.xaml
    /// </summary>
    public partial class frmTip : Window
    {
        public frmTip()
        {
            InitializeComponent();
            Owner = Globales.principal;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
