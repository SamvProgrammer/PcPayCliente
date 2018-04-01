using PcPay.Code.Usuario;
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
    /// Interaction logic for frmReaderDigest.xaml
    /// </summary>
    public partial class frmReaderDigest : Window
    {
        public frmReaderDigest()
        {
            InitializeComponent();
            this.Owner = Globales.principal;
            txtRDM0.GotFocus += Globales.setFocusMit2;
            txtRDM1.GotFocus += Globales.setFocusMit2;
            txtRDM2.GotFocus += Globales.setFocusMit2;
            txtRDM3.GotFocus += Globales.setFocusMit2;
            txtRDM4.GotFocus += Globales.setFocusMit2;
            txtRDM0.LostFocus += Globales.lostFocusMit2;
            txtRDM1.LostFocus += Globales.lostFocusMit2;
            txtRDM2.LostFocus += Globales.lostFocusMit2;
            txtRDM3.LostFocus += Globales.lostFocusMit2;
            txtRDM4.LostFocus += Globales.lostFocusMit2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string strReferencia = string.Empty;
            strReferencia = txtRDM0.Text + lblDv.Text +
                txtRDM1.Text + txtRDM2.Text + txtRDM3.Text+txtRDM4.Text;
            if (strReferencia.Length != 28)
            {
                Globales.MessageBoxMit("El no. de REFERENCIA debe ser de 28 posiciones");
                txtRDM0.Focus();
            }
            else {
                Globales.referenciaAux = strReferencia;
                Close();
            }
        }

        private void numero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender,e);
        }

        private void txtRDM0_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox caja = sender as TextBox;
            if(caja.Name.Contains("0")){
                if (txtRDM0.Text.Length == 10)
                {
                    lblDv.Text = Globales.digitoVF(txtRDM0.Text);
                    txtRDM1.Focus();
                }
                else {
                    lblDv.Text = "";
                }
            }
            if (caja.Name.Contains("1"))
            {
                if (txtRDM1.Text.Length == 3) txtRDM2.Focus();
            }
            if (caja.Name.Contains("2"))
            {
                if (txtRDM2.Text.Length == 2) txtRDM3.Focus();
            }
            if (caja.Name.Contains("3"))
            {
                if (txtRDM3.Text.Length == 2) txtRDM4.Focus();
            }
            if (caja.Name.Contains("4"))
            {
                if (txtRDM4.Text.Length == 10) cmdAceptar.Focus();
            }
        }
    }
}
