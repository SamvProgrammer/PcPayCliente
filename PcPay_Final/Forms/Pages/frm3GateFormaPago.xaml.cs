using PcPay.Code.Utilidades;
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
    /// Lógica de interacción para frm3GateFormaPago.xaml
    /// </summary>
    public partial class frm3GateFormaPago : Page
    {
        public abrirFrm abrir;
        public frm3GateFormaPago()
        {
            InitializeComponent();
        }

        private void CmdAceptar_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
           string elegido= rb.Name;
            if (elegido.Equals("OptTarjeta") ) //OptTarjeta.Checked
            {
                frm3GateBanda banda = new frm3GateBanda();
                banda.Importe.Text = modMIT.TGate.getRspTGImporte();
                abrir(banda);
            }
            else if (elegido.Equals("OptEfectivo")) //OptEfectivo.Checked ==true
            {
                //'PENDIENTE
                //'frm3GateEfectivo.Show
                //Unload Me
            }


        }

   
    }
}
