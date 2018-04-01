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

namespace PcPay.Forms.Formularios.MessagesW
{
    /// <summary>
    /// Lógica de interacción para frmVtaDenegada.xaml
    /// </summary>
    public partial class frmVtaDenegada : Window
    {
        public frmVtaDenegada(string description)
        {
            InitializeComponent();
            this.Owner = Globales.principal;
            this.BACEPTAR.Focus();
            this.TBDESCRIPCION.Text = description;
        }
        public void setDescription(string description)
        {
            this.BACEPTAR.Focus();
            this.TBDESCRIPCION.Text = description;
        }
        private void BACEPTAR_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
