using PcPay.Code.Configuracion;
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
    /// Lógica de interacción para frmProbarLector.xaml
    /// </summary>
    public partial class frmProbarLector : Window
    {
        public bool existError { get; set; }
        public string errorDescription { get; set; }
        public frmProbarLector()
        {
            InitializeComponent();
            this.existError = false;
            Carga();
        }

        private void CmdTest_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            CmdTest.IsEnabled = false;
            if (Program.cpIntegraEMV.chkPp_Printer() == "1")
            {
                Program.cpIntegraEMV.dbgPrint("@lnn El lector funciona correctamente! @br @br @br @br @br @br @br @br @br");
            }
            else
            {
                Program.cpIntegraEMV.dbgSendMessage("PcPay LISTO!!");
            }
            Mouse.OverrideCursor = null;
            this.Close();

        }

        public void Carga()
        {
            if (Program.cpIntegraEMV.chkPp_Printer() == "1")
            {
                LbTestVX.Visibility = Visibility.Visible;
                LbTestIngenico.Visibility = Visibility.Hidden;
            }
            else if (Program.cpIntegraEMV.chkPp_Printer() == "0")
            {
                LbTestIngenico.Visibility = Visibility.Visible;
                LbTestVX.Visibility = Visibility.Hidden;
            }
            else
            {
                //System.Windows.Forms.MessageBox.Show("El tipo de lector configurado no permite esta opción", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                //MsgBoxEx "El tipo de lector configurado no permite esta opción", , , , vbOKOnly + vbInformation, NOMBRE_APP
                this.existError = true;
                this.errorDescription = "El tipo de lector configurado no permite esta opción.";
                this.Close();
            }



        }
    }
}
