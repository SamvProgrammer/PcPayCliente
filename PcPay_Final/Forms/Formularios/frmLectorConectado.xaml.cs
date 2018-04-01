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

namespace PcPay.Forms.formularios
{
    /// <summary>
    /// Interaction logic for frmLectorConectado.xaml
    /// </summary>
    public partial class frmLectorConectado : Window
    {
        public frmLectorConectado()
        {
            InitializeComponent();
            if (Globales.principal != null)
                this.Owner = Globales.principal;
        }

        private void cancelar(object sender, RoutedEventArgs e)
        {
            Globales.lectorConectado = false;
            this.Close();
        }

        private void aceptar(object sender, RoutedEventArgs e)
        {
            lbl1.Text = "Espere---Esta operación puede tardar varios segundos";
            cmdBoton.IsEnabled = false;
            if (!Globales.cpIntegraEMV.dbgSetReader())
            {
                lbl1.Text = "No se pudo encontrar el lector, por favor, verifique que se encuentra conectado correctamente y vuelva a intentalo, de lo contrario haga clic en cancelar";
                lbl1.Foreground = new SolidColorBrush(Colors.Red);
                Globales.lectorConectado = false;
            }
            else
            {
                TypeUsuario.SerialReader = Globales.cpIntegraEMV.chkPp_Serial();
                Globales.SaveSettingString("SERIE", TypeUsuario.SerialReader);
                //frmBanner 
                //Se quita para dejar la validación con la bandera de la terminal
                Globales.lectorConectado = true;
                this.Close();
            }
            cmdBoton.IsEnabled = true;
        }
    }
}
