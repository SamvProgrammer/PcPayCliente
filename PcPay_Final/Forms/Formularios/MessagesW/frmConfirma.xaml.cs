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
    /// Lógica de interacción para frmConfirma.xaml
    /// </summary>
    public partial class frmConfirma : Window
    {
        public bool answer { get; set; }
        public frmConfirma(string _message)
        {
            InitializeComponent();
            this.Owner = Globales.principal;
            this.answer = false;

            this.TBMESSAGE.Text = _message;

            this.BSI.Click += this.button_event;
            this.BNO.Click += this.button_event;

            this.Width = this.Owner.Width;
            this.Height = this.Owner.Height;
        }

        private void button_event(object sender, RoutedEventArgs e)
        {
            string _senderName = ((Button)sender).Name;

            switch (_senderName)
            {
                case "BSI":
                    answer = true;
                    break;
                case "BNO":
                    answer = false;
                    break;
            }
            this.Close();
        }
    }
}
