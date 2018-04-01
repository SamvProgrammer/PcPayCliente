using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PcPay.Code.Clases
{
    class ClsImp
    {
        public ASPPrinterCOM.ASPPrinter ASPPrinter;
        public RawDataPrinter.Printer RDP;
        public void ASPPrinter_PrintComplete()
        {
            System.Windows.Forms.MessageBox.Show("Su documento se ha impreso correctamente", Globales.NOMBRE_APP, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void ASPPrinter_PrintError()
        {
            System.Windows.Forms.MessageBox.Show("ASP_Error", Globales.NOMBRE_APP, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void RDP_Error()
        {
            System.Windows.Forms.MessageBox.Show("RDP error", Globales.NOMBRE_APP, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void RDP_PrintComplete()
        {
            System.Windows.Forms.MessageBox.Show("Su documento se ha impreso correctamente", Globales.NOMBRE_APP, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }
    }
}
