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
    /// Interaction logic for frmImpresionWallets.xaml
    /// </summary>
    public partial class frmImpresionWallets : Page
    {
        public frmImpresionWallets()
        {
            InitializeComponent();
            txtOperacion.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void txtOperacion_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender,e);
        }

        private void cmdCancelar1_Click(object sender, RoutedEventArgs e)
        {
            cerrar();
        }
        public cerrarVentana cerrar;

        private void cmdAceptar1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOperacion.Text) || txtOperacion.Text.Length != 9)
            {
                Globales.MessageBoxMit("Introduzca el número de operación");
            }
            else {
                strFolioReImp = txtOperacion.Text;
                imprimeVoucher();
            }
        }

        private void imprimeVoucher()
        {

            string op = "6";
            string strCadEncriptar = "";
            if (!string.IsNullOrWhiteSpace(strFolioReImp))
            {
                if (TypeUsuario.TipoImpresora == "1" || TypeUsuario.TipoImpresora == "4")
                {
                    op = "reimpvouch";
                }
                else
                {
                    op = "reimpvouchtermica";
                }

                Globales.cpHTTP_Clear();
                Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
                string nvoucher = "";
                if (TypeUsuario.TipoImpresora == "6")
                {
                    nvoucher = "true";
                    string auxop = "";
                    auxop = strFolioReImp.Substring(0, 9);
                    Globales.cpIntegraEMV.dbgSetUrl(Globales.URL_3GATE);
                   // TypeUsuario.strVoucherCoP = Globales.cpIntegraEMV.sndReimpresionWallets(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country,
                    //    auxop);
                }
                else
                {
                    nvoucher = "";
                }

                strCadEncriptar = "&folio=" + strFolioReImp +
                                   "&empresa=" + TypeUsuario.Id_Company +
                                   "&sucursal=" + TypeUsuario.Id_Branch +
                                   "&op=" + op +
                                   "&co=true&nvoucher" + nvoucher +
                                   "&tp_voucher=original";

                Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
                string rsp = "";
                rsp = Globales.cpIntegraEMV.getRspDsResponse();
                 
                switch (rsp)
                {
                    case "error":
                        Globales.MessageBoxMit(Globales.cpIntegraEMV.getRspDsError());
                        break;
                    case "denied":
                        Globales.MessageBoxMit("No es posible realizar la impresión");
                        break;
                    case "approved":
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        Globales.PrintOptions(TypeUsuario.strVoucherCoP);
                        Mouse.OverrideCursor = null;
                        break;
                    default:
                        Globales.MessageBoxMit(rsp);
                        break;

                }
            }
            else {
                Globales.MessageBoxMit("Debe indicar el número de operación.");
            }
        }

        public string strFolioReImp { get; set; }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }
    }
}
