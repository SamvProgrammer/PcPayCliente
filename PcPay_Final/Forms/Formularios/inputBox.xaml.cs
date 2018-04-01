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
    /// Interaction logic for inputBox.xaml
    /// </summary>
    public partial class inputBox : Window
    {
        private string valorCadena = string.Empty;
        public bool cancelarActivo = false;
        public inputBox(string TituloVentana = "Información")
        {
            InitializeComponent();
            this.Owner = Globales.principal;
            cancelarActivo = false;
            Title = TituloVentana;

            this.txt.GotFocus += Globales.setFocusMit2;
            this.txt.LostFocus += Globales.lostFocusMit2;
        }

        public void setTitulo(string titulo)
        {
            lblMensaje.Text = titulo;
        }
        public void setMax(int tamaño) {
            txt.MaxLength = tamaño;
        }
        private void CancelarBoton(object sender, RoutedEventArgs e)
        {

            Close();
            cancelarActivo = true;
        }

        public string getValor()
        {
            return valorCadena;

        }

        private void aceptarBoton(object sender, RoutedEventArgs e)
        {
            valorCadena = txt.Text;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                Globales.MessageBoxMit(mensajePersonalizado);
                return;
            }
            this.Close();
            cancelarActivo = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cancelarActivo = true;

        }


        public string mensajePersonalizado { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt.Focus();
        }

        private void txt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //aceptarBoton(null,null);
            }
        }
    }
}
