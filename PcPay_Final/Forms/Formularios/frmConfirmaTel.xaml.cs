using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for frmConfirmaTel.xaml
    /// </summary>
    public partial class frmConfirmaTel : Window
    {
        private string valor = string.Empty;
        public frmConfirmaTel()
        {
            InitializeComponent();
        }

        private void soloNumero(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^0-9]+");
            e.Handled = r.IsMatch(e.Text);
        }

        private void aceptar(object sender, RoutedEventArgs e)
        {
                if(string.IsNullOrWhiteSpace(txtTel.Text)){
                    Globales.MessageBoxMit("Introduzca número de celular.");
                    txtTel.Focus();
                    return;
                }

            if(txtTel.Text.Length < 10){
                Globales.MessageBoxMit("Número Celular incompleto");
                txtTel.Focus();
                return;
            }
            valor = txtTel.Text;
            Close();
        }

        public string GetConfirmacionTel() {
            return valor;
        }

        private void cancelar(object sender, RoutedEventArgs e)
        {
            valor = "";
            Close();
        }
    }
}
