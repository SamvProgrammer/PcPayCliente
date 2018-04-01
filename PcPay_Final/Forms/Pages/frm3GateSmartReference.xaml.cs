using PcPay.Code.Usuario;
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
    /// Lógica de interacción para frmGateSmartReference.xaml
    /// </summary>
    public partial class frmGateSmartReference : Page
    {
        public abrirFrm abrir;
        public frmGateSmartReference()
        {
            InitializeComponent();
        }

        public void carga(string p)
        {
            LbReferencia.Content = p;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CboEmpresas = Globales.obtieneBancos(CboEmpresas, Globales.GetDataXml("smart", modMIT.TGate.getRspTGXML()));
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                if(CboEmpresas.SelectedIndex == -1){
                    Globales.MessageBoxMit("Favor de seleccionar una referencia.");
                    return;
                }
                string msg, isMod, import;
                import = modMIT.TGate.getRspTGImporte();
                string empresa = CboEmpresas.Text;
                string valorEmpresa = empresa.Substring(0,empresa.IndexOf(","));
                int valor = Convert.ToInt32(Convert.ToChar(valorEmpresa.Substring(0, 1)));
                string numEmp = Convert.ToString(valor) + valorEmpresa.Substring(1);
                isMod = numEmp.Substring(numEmp.Length-1,1);
                import = (string.IsNullOrWhiteSpace(import)) ? "0.00" : import;
                double import2 = Convert.ToDouble(import);
                if(isMod == "1"){
                    do{
                        Formularios.inputBox box = new Formularios.inputBox();
                        box.setTitulo("Introduzca el monto");
                        string aux = box.getValor();
                        double aux2 = 0;
                        try
                        {
                            aux2 = Convert.ToDouble(aux);
                            import2 = aux2;
                        }
                        catch
                        {
                            import2 = 0;
                        }
                    }while(import2 > 0);

                }

                modMIT.TGate.snd3GateSmartReference(TypeUsuario.usu,TypeUsuario.Pass,TypeUsuario.Id_Company,TypeUsuario.Id_Branch,Convert.ToString(lblReferencia.Content),Convert.ToString(numEmp),Convert.ToString(import2));
                modMIT.TGate.dbgSetTGXML(Globales.GetDataXml("originadora1", modMIT.TGate.getRspTGXML()));
                if (modMIT.TGate.getRspTGTarjeta() == "1" && modMIT.TGate.getRspTGEfectivo() == "1")
                {
                    frm3GateFormaPago formaPago = new frm3GateFormaPago();
                    formaPago.abrir = abrir;
                    abrir(formaPago);                    
                    return;
                }
                else {
                    if (modMIT.TGate.getRspTGTarjeta() == "1")
                    {
                        //Abrir formulario cobro con tarjeta.
                        //verifica si se paga comisión..
                        if(Globales.MessageConfirm("El cliente debe pagar una comisión de: $"+modMIT.TGate.getRspTGNuComision())){
                            Globales.botonSalir();
                            return;
                        }
                        Globales.isSmart = true;
                        frm3GateBanda banda = new frm3GateBanda();
                        banda.numEmp = numEmp;
                        abrir(banda);
                    }
                    else if (modMIT.TGate.getRspTGEfectivo() == "1")
                    {

                    }
                    else { 
                      //NO HAY INFORMACIÓN...
                        if (!string.IsNullOrWhiteSpace(Globales.GetDataXml("dserror", modMIT.TGate.getRspTGXML())))
                        {
                            Globales.MessageBoxMitError(Globales.GetDataXml("dserror", modMIT.TGate.getRspTGXML()));
                        }
                        else {
                            Globales.MessageBoxMitError("No hay información relacionada, vuelva ingresar la referencia");
                        }
                        frm3GateReferencia referencia = new frm3GateReferencia();
                        referencia.abrir = abrir;
                        abrir(referencia);
                    }
                }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
