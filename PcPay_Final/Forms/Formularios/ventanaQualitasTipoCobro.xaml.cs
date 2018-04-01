using PcPay.Forms.Pages;
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
    /// Lógica de interacción para ventanaQualitasTipoCobro.xaml
    /// </summary>
    public partial class ventanaQualitasTipoCobro : Window
    {
        private bool poliza;
        private string resultado = string.Empty;
        public metodo enviar;
        public ventanaQualitasTipoCobro(bool poliza ,string resultado)
        {
            InitializeComponent();
            Owner = Globales.principal;
            this.Width = this.Owner.Width;
            this.Height = this.Owner.Height;
            this.poliza = poliza;
            this.resultado = resultado;
        }

        private void event_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(poliza){
                txtModelo.Text = Globales.GetDataXml("modelo", resultado);
                string strAux = Globales.GetDataXml("vehiculo", resultado);
                strAux = Globales.GetDataXml("descripcion", strAux);
                txtNombre.Text =  Globales.GetDataXml("nombre", resultado);
                return;
            }
            txtModelo.Text =  Globales.GetDataXml("modelo", resultado);
            txtNombre.Text =  Globales.GetDataXml("asegurado", resultado);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            enviar();
            Close();
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
