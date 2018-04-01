using PcPay.Code.Configuracion;
using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using PcPay.Forms.formularios;
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

namespace PcPay.Forms.Formularios
{
    /// <summary>
    /// Lógica de interacción para frmLoginMDI.xaml
    /// </summary>
    public partial class frmLoginMDI : Window
    {
        public logearse login { get; set; }
        public bool cerrarVentana { get; set; }
        public frmLoginMDI()
        {
            InitializeComponent();
            Owner = Globales.principal;

            txtUser.GotFocus += Globales.setFocusMit2;
            txtPwd.GotFocus += Globales.setFocusMit2;

            txtUser.LostFocus += Globales.lostFocusMit2;
            txtPwd.LostFocus += Globales.lostFocusMit2;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }

        private void PasswordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            PasswordBox pass = sender as PasswordBox;
            Regex expresion =new  Regex("[^a-zA-Z0-9]+");
            e.Handled = expresion.IsMatch(pass.Password);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void aceptar(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtUser.Text)){
                Globales.MessageBoxMit("Introduzca un usuario");
                txtUser.Focus();
                return;
            }
            if(string.IsNullOrWhiteSpace(txtPwd.Password)){
                Globales.MessageBoxMit("Introduzca la contraseña");
                txtPwd.Focus();
                return;
            }

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            txtPwd.IsEnabled = false;
            txtUser.IsEnabled = false;
            TypeUsuario.strVersion = Globales.GetVersionApp();
            login(txtUser.Text,txtPwd.Password);
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Globales.ip.Contains("s://ss1.e-pago.com.mx"))
            {
                this.Title += " " + Globales.ip;
            }
        }

        private void txtPwd_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPwd.Password = txtPwd.Password.ToUpper();
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Close();
            cerrarVentana = true;
        }
    }
}
