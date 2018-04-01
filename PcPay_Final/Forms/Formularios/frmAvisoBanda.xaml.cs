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
using PcPay.Forms.Formularios;
using PcPay.Code.Usuario;
namespace PcPay.Forms.Formularios
{
    /// <summary>
    /// Interaction logic for frmAvisoBanda.xaml
    /// </summary>
    public partial class frmAvisoBanda : Window
    {
        public frmAvisoBanda()
        {
            InitializeComponent();
            this.Owner = Globales.principal;
        }

        private void CerrarVentana(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(chk1.IsChecked))
            {

                Globales.SaveSettingString("ShwMsg", "0");
                TypeUsuario.ShowMsg = false;
            }
            else
            {
                Globales.SaveSettingString("ShwMsg", "1");
                TypeUsuario.ShowMsg = true;
            }

            this.Close();
        }

        private void CargandoVentana(object sender, RoutedEventArgs e)
        {
            lblTip.Text = "Antes de deslizar o insertar el chip de la tarjeta, ingrese " + TypeUsuario.reference + " y el importe a cobrar, posteriormente haga clic en el boton \"Activar Lector\"";
        }
    }
}
