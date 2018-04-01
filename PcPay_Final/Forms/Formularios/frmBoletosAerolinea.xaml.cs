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
    /// Lógica de interacción para frmBoletosAerolinea.xaml
    /// </summary>
    public partial class frmBoletosAerolinea : Window
    {
        public string numOperPNR { get; set; }
        public string boletosPNR { get; set; }
        public bool isUpdatePNR { get; set; }
        public cerrarVentana cerrar;
        private string boletoOriginalPNR { get; set; }
        private List<string> lista_boletos;
        private int cantidad = 0;
        public frmBoletosAerolinea()
        {
            InitializeComponent();
            Owner = Globales.principal;
            TNUMBOLETO.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
        }

        private void cargar()
        {
            cmdVoucher.Visibility = Visibility.Hidden;
            cmdOtro.Visibility = Visibility.Hidden;
            UserIata.Content = "IATA: " + TypeUsuario.iata;
            lista_boletos = new List<string>();
            if (!isUpdatePNR)
            {
                numOperPNR = (string.IsNullOrWhiteSpace(numOperPNR)) ? Globales.cpIntegraEMV.getRspOperationNumber() : numOperPNR;
            }
            else {
                Fechas.Visibility = Visibility.Hidden;
                cmdOtro.Content = "Cancelar";
                cmdOtro.Visibility = Visibility.Visible;
                cmdEnviar.Content = "Asociar";
                boletoOriginalPNR = string.Empty;
                if (!string.IsNullOrWhiteSpace(boletosPNR) && boletosPNR != "null")
                {
                    string[] numBoletos = boletosPNR.Split(',');
                    foreach(string item in numBoletos){
                        LBOLETOS.Items.Add(item);
                        lista_boletos.Add(item);
                    }
                    cantidad = lista_boletos.Count();
                    LTOTALBOLETOS.Content = cantidad;
                    if (numBoletos.Length == 10) cmdEnviar.IsEnabled = false;
                }

            }
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender,e);
        }

        public string Importe { get; set; }

        private void Command2_Click(object sender, RoutedEventArgs e)
        {
            if (!isUpdatePNR)
            {

            }
            else {
                string updateBoletoPNR = string.Empty;
                foreach(string item in lista_boletos){
                    updateBoletoPNR += item + ",";
                }
                updateBoletoPNR = updateBoletoPNR.Substring(0,updateBoletoPNR.Length-1);
                if (cantidad != lista_boletos.Count())
                {
                    if(Globales.insertaBoleto(numOperPNR,updateBoletoPNR)){
                        Close();
                        cerrar();
                    }
                }
                else {
                    Globales.MessageBoxMit("Sin boletos que asociar.");
                    Close();
                }
            }
        }

        private void BAGREGABOLETO_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TNUMBOLETO.Text))
            {
                Globales.MessageBoxMit("Ingrese un número de boleto.");
                return;
            }
            if (Globales.verificadorBoleto(TNUMBOLETO.Text))
            {                
                return;
            }

            string _num_boleto = string.Format("{0}{1}", TypeUsuario.iata, this.TNUMBOLETO.Text);
            if (!Globales.isAerolinea)
            {
                int intDigVer, intDigVer2 = 0;
                if (Globales.IsNumeric(this.TNUMBOLETO.Text))
                {
                    intDigVer = Globales.DigitoVerificador(TypeUsuario.iata, this.TNUMBOLETO.Text);
                    intDigVer2 = Globales.DigitoVerificador("", this.TNUMBOLETO.Text);
                    if (intDigVer == -1 && intDigVer2 == -1)
                    {
                        Globales.MessageBoxMit("Verifique el No. de boleto.");
                        return;
                    }
                }
            }

            if (this.lista_boletos.Count < 10)
            {
                if (!this.lista_boletos.Exists(o => o == _num_boleto))
                    this.lista_boletos.Add(_num_boleto);
                else
                {
                    Globales.MessageBoxMit("El boleto ya existe en la lista.");
                    return;
                }
                this.LBOLETOS.Items.Clear();

                this.lista_boletos.ForEach(delegate(string elm)
                {
                    this.LBOLETOS.Items.Add(elm);
                });

                this.TNUMBOLETO.Text = string.Empty;
                this.LTOTALBOLETOS.Content = this.lista_boletos.Count;
            }
            else
            {
                Globales.MessageBoxMit("El límite de boletos permitidos son 10.");
                this.TNUMBOLETO.Text = string.Empty;
            }
        }

        public string auth { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cargar();
        }

        private void cmdOtro_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cmdVoucher_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TNUMBOLETO_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender,e);
        }
    }
}
