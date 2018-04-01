using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using PcPay.Forms.Formularios;
using RawDataPrinter;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace PcPay.Forms.Pages
{
    /// <summary>
    /// Interaction logic for frmImpresora.xaml
    /// </summary>
    public partial class frmImpresora : Page
    {
        public cerrarVentana cerrar;
        public frmImpresora()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LTITTULO.Content = "Configuración de terminal";
            Globales.cpIntegraEMV.dbgEndOperation();
            combo1 = Globales.llenaImp(combo1);
           // chkUnaHoja.Visibility = Visibility.Hidden;
           // imgImp.Visibility = Visibility.Hidden;
            chkImp.Visibility = Visibility.Hidden;
            //image6.Visibility = Visibility.Hidden;
            switch(TypeUsuario.TipoImpresora){
                case "1":
                    optHTML.IsChecked = true;
                    break;
                case "2":
                    optTermino.IsChecked = true;
                    break;
                case "3":
                    optEpson.IsChecked = true;
                    break;
                case "4":
                    optHTML1.IsChecked = true;
                    break;
                case "5":
                    optEpson2.IsChecked = true;
                    break;
                default:
                    optNinguno.IsChecked = true;
                    break;
            }
            radio(null, null);
        }

        private void radio(object sender, RoutedEventArgs e)
        {
            frmescape.Visibility = Visibility.Hidden;
            frmImpresoraPlug.Visibility = Visibility.Hidden;
            chkImp.Visibility = Visibility.Hidden;
       //     image6.Visibility = Visibility.Hidden;
            if (Convert.ToBoolean(optHTML.IsChecked) || Convert.ToBoolean(optHTML1.IsChecked))
            {
                chkImp.Visibility = Visibility.Visible;
         //       image6.Visibility = Visibility.Visible;
            }
            else if (Convert.ToBoolean(optEpson.IsChecked))
            {
                frmImpresoraPlug.Visibility = Visibility.Visible;
            }
            else if (Convert.ToBoolean(optEpson2.IsChecked))
            {
                frmescape.Visibility = Visibility.Visible;
           }    
            
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int reVal = 0;
            if(Convert.ToBoolean(optNinguno.IsChecked)){
                TypeUsuario.TipoImpresora = "6";
            }else if(Convert.ToBoolean(optHTML.IsChecked)){

                TypeUsuario.TipoImpresora = "1";
                if (Convert.ToBoolean(chkImp.IsChecked))
                {
                    Globales.banImp = 1;
                    Globales.SaveSettingString("AUTOMATICO", "1");
                }
                else {
                    Globales.banImp = 1;
                    Globales.SaveSettingString("AUTOMATICO", "0");
                }

                Globales.printerName = combo1.SelectedItem.ToString();
            }
            else if (Convert.ToBoolean(optHTML1.IsChecked))
            {
                TypeUsuario.TipoImpresora = "4";
                if (Convert.ToBoolean(chkImp.IsChecked))
                {
                    Globales.banImp = 1;
                    Globales.SaveSettingString("AUTOMATICO", "1");
                }
                else
                {
                    Globales.banImp = 1;
                    Globales.SaveSettingString("AUTOMATICO", "0");
                }
                Globales.printerName = combo1.SelectedItem.ToString();
            }
            else if (Convert.ToBoolean(optTermino.IsChecked))
            {

            }
            else if (Convert.ToBoolean(optEpson.IsChecked))
            {
                TypeUsuario.TipoImpresora = "3";
                Globales.printDialog1.Document = Globales.printDocument1;
                int left;
                string strLeft;

                string strTIpoFont = string.Empty;
                TypeUsuario.strFont = txtFont.Text.Trim();
                Globales.SaveSettingString("leftPrinter", "0");
                testEpson(0);
                if (Convert.ToBoolean(chkUnaHoja.IsChecked))
                {
                    Globales.SaveSettingString("HOJA", "1");
                    TypeUsuario.strSoloUnaHoja = "1";
                }
                else {
                    Globales.SaveSettingString("HOJA", "0");
                    TypeUsuario.strSoloUnaHoja = "0";
                }

                if (Globales.MessageConfirm("¿Se imprimió correctamente la página de prueba?"))
                {
                    if(Globales.MessageConfirm("¿Desea ajustar el margen izquierdo de la hoja?")){
                       bool cancelado;
                    string valor = string.Empty;
                    do
                    {
                        DialogResult answer = InputBox.InputBoxQ("Introduzca el valor del nuevo margen [0,80]:", "Configuración", ref valor);
                        if (answer == DialogResult.OK || answer == DialogResult.Yes)
                        {
                            int aux = 0;
                            if(int.TryParse(valor,out aux)){
                               if(!(aux < 0 || aux > 80)){
                                   Globales.SaveSettingString("leftPrinter", aux.ToString());
                                   testEpson(aux);
                                   if(Globales.MessageConfirm("¿Se configuró correctamente la impresora?")){
                                       break;
                                   }
                               }
                            }
                        }
                        else {
                            break;
                        }
                    } while (true);

                    Globales.MessageBoxMit("Si desea configurar nuevamente el margen, vuelva a configurar la impresora.");
                    }
                }
                else {
                    Globales.MessageBoxMit("Verifique si está instalada correctamente la impresora e inténtelo nuevamente.");
                    TypeUsuario.TipoImpresora = "0";
                }
                
            }
            else if (Convert.ToBoolean(optEpson2.IsChecked))
            {

            }
            else {
                Globales.MessageBoxMit("No se encontró impresora.");
                TypeUsuario.TipoImpresora = "0";
            }
            Globales.SaveSettingString("PRINTERNUMBER",TypeUsuario.TipoImpresora);
            cerrar();
        }

        private void testEpson(float marginLeft)
        {
            Globales.MessageBoxMit("Verifique que la impresora esté lista y preparada para imprimir");
            Globales.epsonImprimir("Font: "+txtFont.Text+"\n\n\nABCDEFGHIJKLMNÑOPQRSTWYZ\n\n\n0123456789", marginLeft,true);            
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void combo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            frmAyudaEC ayuda = new frmAyudaEC();
            ayuda.ShowDialog();
        }
    }
}
