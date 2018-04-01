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
    /// Lógica de interacción para frmMessageBox.xaml
    /// </summary>
    public partial class frmMessageBox : Window
    {
        public frmMessageBox(string message)
        {
            InitializeComponent();
            if(Globales.principal == null){
                goto saltar;
            }
            this.Owner = Globales.principal;

            saltar:
            this.TBMESSAGE.Text = message;
            this.BACEPTAR.Click += BACEPTAR_Click;
            this.BACEPTAR.Focus();
            if(Globales.principal == null){
                goto saltar2;
            }
            
            this.Width = this.Owner.Width;
            this.Height = this.Owner.Height;
            saltar2:
            var string2 = "";
        }

        private void BACEPTAR_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void event_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Media.SystemSounds.Beep.Play();
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.Close();
        }
    }
}
