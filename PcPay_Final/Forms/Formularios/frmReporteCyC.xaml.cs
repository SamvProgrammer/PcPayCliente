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

namespace PcPay.Forms.Formularios
{
    /// <summary>
    /// Lógica de interacción para frmReporteCyC.xaml
    /// </summary>
    public partial class frmReporteCyC : Window
    {
        public frmReporteCyC()
        {
            InitializeComponent();
            this.Owner = Globales.principal;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Fecha_CalendarClosed(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DTInicio.Text = Convert.ToString(DateTime.Now);
                DTFin.Text = Convert.ToString(DateTime.Now);

                string strCadEncriptar = "&fecha1="+DTInicio.Text+
                                         "&fecha2="+DTInicio.Text+
                                         "&empresa="+TypeUsuario.Id_Company+
                                         "&sucursal="+TypeUsuario.Id_Branch+
                                         "&usuario="+TypeUsuario.usu+
                                         "&comando=lista_usuarios";
                if (Globales.cpintegracion_cpReporte(Globales.encryptString(strCadEncriptar, "KEY RePoRtEs PcPaY KEYñ", true),Globales.URL_REPORTECYC))
                {
                    string strCadena = Globales.cpIntegracion_sResult;
                    strCadena = (string.IsNullOrWhiteSpace(strCadena)) ? "" : strCadena;
                    if (strCadena.Contains("COBRANZA"))
                    {
                        cboUsuarios.Items.Add(TypeUsuario.usu);
                        cboUsuarios.IsEnabled = false;
                        OptHSuc.IsEnabled = false;
                        OptIRPag.IsEnabled = false;
                        OptTVServ.IsEnabled = false;
                        OptTrPag.IsEnabled = false;
                        OptHUsr.IsEnabled = true;
                    }
                    else {
                        if (!string.IsNullOrWhiteSpace(strCadena) && strCadena != "null")
                        {
                            strAux = strCadena;
                            string[] aux = strAux.Split('|');
                            foreach(string item in aux){
                                cboUsuarios.Items.Add(item);
                            }
                       
                       }
                    }
                    cboUsuarios.SelectedIndex = 0;
                }
            }
            catch { 
            
            }
        }

        public string strAux { get; set; }

        private void cmdEnviar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                enviar();
            }
            catch {

                Mouse.OverrideCursor = null;
            }
        }

        private void enviar()
        {
            string tipoReporte = string.Empty;
            string strCadEncriptar = string.Empty;
            //AQUÍ SE DEFINE EL TIPO DE REPORTE...
            if (Convert.ToBoolean(OptHSuc.IsChecked))
            {

            }
            else if (Convert.ToBoolean(OptHUsr.IsChecked))
            {

            }
            else if (Convert.ToBoolean(OptIRPag.IsChecked))
            {

            }
            else if (Convert.ToBoolean(OptIVServ.IsChecked))
            {

            }
            else if (Convert.ToBoolean(OptTrPag.IsChecked))
            {

            }
            else if (Convert.ToBoolean(OptTVServ.IsChecked))
            {

            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
              strCadEncriptar = "&fecha1="+DTInicio.Text+
                                         "&fecha2="+DTInicio.Text+
                                         "&empresa="+TypeUsuario.Id_Company+
                                         "&sucursal="+TypeUsuario.Id_Branch+
                                         "&usuario="+cboUsuarios.Text+
                                         "&comando="+tipoReporte;
             if (Globales.cpintegracion_cpReporte(Globales.encryptString(strCadEncriptar, "KEY RePoRtEs PcPaY KEYñ", true), Globales.URL_REPORTECYC))
             {
                 Globales.Imprimir_HTML(Globales.cpIntegracion_sResult);
                 Mouse.OverrideCursor = null;
             }
        }

        public string strCadEncriptar { get; set; }
    }
}
