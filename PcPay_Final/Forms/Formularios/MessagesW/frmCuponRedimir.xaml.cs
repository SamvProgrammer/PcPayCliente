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
    /// Lógica de interacción para frmCuponRedimir.xaml
    /// </summary>
    public partial class frmCuponRedimir : Window
    {
        public bool redimirCupon { get; set; }
        public frmCuponRedimir(string message, string title = "Descripción")
        {
            InitializeComponent();
            this.Owner = Globales.principal;

            this.Width = this.Owner.Width;
            this.Height = this.Owner.Height;

            this.LTITLE.Content = title;
            this.TBMESSAGE.Text = message;

        }

        private void BACEPTAR_Click(object sender, RoutedEventArgs e)
        {
            string nombre = ((Button)sender).Name;
            switch (nombre)
            {
                case "BREDIMIR":
                    this.redimirCupon = true;
                    break;
                case "BREGRESAR":
                    this.redimirCupon = false;
                    break;
            }
            this.Close();
        }
    }
}
