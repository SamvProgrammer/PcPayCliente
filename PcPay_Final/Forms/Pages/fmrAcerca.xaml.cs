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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PcPay.Forms.Pages
{
    /// <summary>
    /// Interaction logic for fmrAcerca.xaml
    /// </summary>
    public partial class fmrAcerca : Page
    {
        public cerrarVentana cerrar;
        public fmrAcerca()
        {
            InitializeComponent();
            this.showInfo();
        }


        private void showInfo()
        {
            string message = string.Empty;

            this.lblVersion.Content = "Versión " + TypeUsuario.strVersion;
            switch (Globales.BanImg)
            {
                case 1:
                    message = PcPay.Properties.Resources.MSSGTRAVEL;
                    break;
                case 2:
                    message = PcPay.Properties.Resources.MSSGMIT;
                    break;
                case 3:
                    message = PcPay.Properties.Resources.MSSGMIT;
                    break;
                case 4:
                    message = PcPay.Properties.Resources.MSSGMIT;
                    break;
                case 5:
                    message = PcPay.Properties.Resources.MSSGMIT;
                    break;
                case 6:
                    message = PcPay.Properties.Resources.MSSGMIT;
                    break;
                case 7:
                    message = PcPay.Properties.Resources.MSSGMASIVOS;
                    break;
                case 8:
                    message = PcPay.Properties.Resources.MSSGMIT;
                    break;
                case 9:
                    message = PcPay.Properties.Resources.MSSGMASIVOS;
                    break;
                case 10:
                    message = PcPay.Properties.Resources.MSSGMIT;
                    break;
                case 11:
                    message = PcPay.Properties.Resources.MSSGMIT;
                    break;
                case 12:
                    message = PcPay.Properties.Resources.MSSGMASIVOS;
                    break;
                case 13:
                    message = PcPay.Properties.Resources.MSSGMASIVOS;
                    break;
                case 14:
                    message = PcPay.Properties.Resources.MSSGMASIVOS;
                    break;
                case 15:
                    message = PcPay.Properties.Resources.MSSGMASIVOS;
                    break;
                case 16:
                    message = PcPay.Properties.Resources.MSSGMASIVOS;
                    break;
                case 17:
                    switch (Globales.GetDataXml("tel_contacto", TypeUsuario.CadenaXML))
                    {
                        case "1":
                            message = PcPay.Properties.Resources.MSSGTRAVEL;
                            break;
                        case "2":
                            message = PcPay.Properties.Resources.MSSGMIT;
                            break;
                        case "3":
                            message = PcPay.Properties.Resources.MSSGMASIVOS;
                            break;
                    }
                    break;
            }
            message = message.Replace("|", "\n");
            this.LDESCRIPCION.Text = message;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblDisclamer.Text = TypeUsuario.nb_company;
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }
    }
}
