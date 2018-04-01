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
    /// Lógica de interacción para frmVtaError.xaml
    /// </summary>
    public partial class frmVtaError : Window
    {
        public frmVtaError(string description, string title = "")
        {
            InitializeComponent();
            this.Owner = Globales.principal;
            this.BACEPTAR.Focus();
            this.TBDESCRIPCION.Text = description;

            this.Width = this.Owner.Width;
            this.Height = this.Owner.Height;

            if (!string.IsNullOrWhiteSpace(title))
                this.LTITLE.Content = title;
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

        private void TBDESCRIPCION_Loaded(object sender, RoutedEventArgs e)
        {
            System.Media.SystemSounds.Exclamation.Play();
        }


    }
}
