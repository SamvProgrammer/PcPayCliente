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
    /// Lógica de interacción para frmVtaAprobada.xaml
    /// </summary>
    public partial class frmVtaAprobada : Window
    {

        public frmVtaAprobada(string numAutorizacion, bool isCancelacion = false)
        {
            InitializeComponent();
            this.Owner = Globales.principal;
            this.BACEPTAR.Focus();
            this.TBMESSAGE.Text = string.Format("Aut. {0}", numAutorizacion);

            this.Width = this.Owner.Width;
            this.Height = this.Owner.Height;

            if (isCancelacion)
            {
                this.LTITLE.Content = "Cancelación Aprobada.";
                this.LTITLE.FontSize = 20;
            }
        }


        public frmVtaAprobada(string message, string title)
        {
            InitializeComponent();
            this.Owner = Globales.principal;
            this.BACEPTAR.Focus();
            this.TBMESSAGE.Text = message;

            this.Width = this.Owner.Width;
            this.Height = this.Owner.Height;

            this.LTITLE.Content = title;
        }
        public frmVtaAprobada(string message, string title, MITWindowSize size = MITWindowSize.Normal)
        {
            InitializeComponent();
            this.Owner = Globales.principal;
            this.BACEPTAR.Focus();
            this.TBMESSAGE.Text = message;

            this.Width = this.Owner.Width;
            this.Height = this.Owner.Height;

            switch (size)
            {
                case MITWindowSize.Medium:
                    this.BBODY.Width = 550;
                    break;
                case MITWindowSize.Large:
                    this.BBODY.Width = 650;
                    break;
            }


            this.LTITLE.Content = title;
        }
        public void setNumAutorizacion(string numAutorizacion)
        {
            this.TBMESSAGE.Text = string.Format("Aut. {0}", numAutorizacion);
        }
        private void BACEPTAR_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
