using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for frmReferencia.xaml
    /// </summary>
    public partial class frmReferencia : Window
    {
        public bool cancelar { get; set; }
        public frmReferencia()
        {
            InitializeComponent();
            this.Owner = Globales.principal;
            txtReferencia.GotFocus += Globales.setFocusMit2;
            txtReferencia.LostFocus += Globales.lostFocusMit2;
        }

        private void perderFoco(object sender, RoutedEventArgs e)
        {
            txtReferencia.Text = txtReferencia.Text.ToUpper();
        }

        private void letrasNumeros(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9A-Za-z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cerrar(object sender, RoutedEventArgs e)
        {
            this.Close();
            cancelar = true;
        }

        private void aceptarClick(object sender, RoutedEventArgs e)
        {
            string ope, aut;
            if(string.IsNullOrWhiteSpace(txtReferencia.Text)){
                Globales.MessageBoxMit("Introduzca una referencia");
                txtReferencia.Focus();
                return;
            }

            if (Globales.cpIntegraEMV.getRspOperationNumber() == "")
            {
                ope = Globales.GetDataXml("foliocpagos", Globales.cpIntegracion_sResult);
                aut = Globales.GetDataXml("auth", Globales.cpIntegracion_sResult);
            }
            else {
                ope = Globales.cpIntegraEMV.getRspOperationNumber();
                aut = Globales.cpIntegraEMV.getRspAuth();
            }
            if (ChangeRef(ope, aut, txtReferencia.Text) == "1")
            {
                cmdEnviar.IsEnabled = false;
                Globales.MessageBoxMit("El proceso ha concluido correctamente");
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                Close();
            }
            else {
                Globales.MessageBoxMitError("El proceso NO se ha completado exitosamente, Verifique sus datos.");
            }

        }

        private string ChangeRef(string numOpe, string numAut, string referencia)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            string valor = "";
            try {
                Globales.WS.WS_Url = Globales.URL_REF;
                Globales.WS.WS_Action = "http://" + "cambio.referencia.cpagos";
                Globales.WS.ClearWS();
                Globales.WS.WS_Params = numOpe+","+numAut+","+referencia;
                Globales.WS.WS_nbParams = "in0,in1,in2";
                Globales.WS.WS_Method = "actualizaReferencia";
                Globales.WS.MITWebServices(Globales.WS.WS_Params, Globales.WS.WS_nbParams, Globales.WS.WS_Method);
                valor = Globales.WS.WS_Response;
                valor = Globales.GetDataXml("ns1:out", valor);
            }
            catch
            {


            }
            Mouse.OverrideCursor = null;
            return valor;
        }

        private void txtReferencia_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
