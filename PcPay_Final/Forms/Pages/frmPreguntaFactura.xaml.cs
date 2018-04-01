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
    /// Interaction logic for frmPreguntaFactura.xaml
    /// </summary>
    public partial class frmPreguntaFactura : Page
    {
        public abrirFrm abrirFrmAhora;
        public cerrarVentana cerraPage;
        public frmPreguntaFactura()
        {
            InitializeComponent();
        }

        private void cmdAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(OptAhora.IsChecked))
            {
                string fecha = Globales.GetDataXml("date", Globales.cpIntegracion_sResult);
                
                if (!string.IsNullOrWhiteSpace(fecha))
                {
                    fecha = String.Format("yyyy-mm-dd", fecha);
                }
                Globales.GeneraFacturaElectronica(Globales.GetDataXml("amount", Globales.cpIntegracion_sResult), Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult), fecha + " " + Globales.GetDataXml("time", Globales.cpIntegracion_sResult), Globales.GetDataXml("propina", Globales.cpIntegracion_sResult));
                abrirFrmAhora(new frmFacturaDatos() { 
                  cerrar = cerraPage
               
                });
            }
            else {
                Globales.FacturaElectronica(Globales.GetDataXml("amount", Globales.cpIntegracion_sResult), Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult), Globales.GetDataXml("date", Globales.cpIntegracion_sResult), Globales.GetDataXml("propina", Globales.cpIntegracion_sResult));
                abrirFrmAhora(new frmFacturaE() {
                    cerrar = cerraPage
                });
            }
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }
    }
}
