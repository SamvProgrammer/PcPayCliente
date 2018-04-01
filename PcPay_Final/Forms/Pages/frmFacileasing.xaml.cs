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
    /// Lógica de interacción para frmFacileasing.xaml
    /// </summary>
    public partial class frmFacileasing : Page
    {
        string numOrdenText, importeText, strProveedor, strNoServicio, strNoEconomico;
        public cerrarVentana cerrar;
        public abrirFrm abrir;
        public frmFacileasing()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cmdEnviar_Click(object sender, RoutedEventArgs e)
        {
            //****************************************************************************
            //*Descripcion: Se hace una consulta del servicio que introduce el usuario   *
            //*             a la B D de facileasing                                      *
            //****************************************************************************
            string strCadEncriptar = string.Empty;
            numOrdenText = string.Empty;
            importeText = string.Empty;
            strProveedor = string.Empty;
            strNoServicio = string.Empty;
            strNoEconomico = string.Empty;

            if(string.IsNullOrWhiteSpace(txtnoservicio.Text)){
               Globales.MessageBoxMit("Escriba No. Servicio");
                return;
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globales.cpHTTP_Clear();
            Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
            strCadEncriptar = "&op=facileasing&cdempresa="+TypeUsuario.Id_Company+"&nuservicio="+txtnoservicio.Text.Trim();
            Globales.cpHTTP_cadena1 = "enc="+Globales.encryptString(strCadEncriptar,Globales.KEY_RC4,true);
            string resultado = Globales.cpHTTP_sResult;
            try{
            if(Globales.cpHTTP_SendcpCUCT()){
                Mouse.OverrideCursor = null;
                if(Globales.GetDataXml("response",Globales.cpHTTP_sResult).Substring(0,1)== "0"){
                    string mensaje = Globales.GetDataXml("nb_response",resultado);
                    Globales.MessageBoxMitError(mensaje);
                    cmdCancelar.IsEnabled = false;
                    cmdSiguiente.IsEnabled = false;
                }else{
                    string mensaje = Globales.GetDataXml("nb_response",resultado);
                    Globales.MessageBoxMitApproved(mensaje);
                    numOrdenText = Globales.GetDataXml("nu_servicio",resultado);
                    importeText = Globales.GetDataXml("importe",resultado);
                    lblimporte.Text = Globales.GetDataXml("importe",resultado);
                    cmdSiguiente.IsEnabled = true;
                    cmdCancelar.IsEnabled = true;
                    strProveedor =  Globales.GetDataXml("nu_proveedor",resultado);
                    strNoServicio =  Globales.GetDataXml("nu_servicio",resultado);
                    strNoEconomico =  Globales.GetDataXml("nu_economico",resultado);
                }
            }
            }catch{
               Globales.MessageBoxMit("Error en consultar facileasing.");
            }

            Mouse.OverrideCursor = null;

        }

        private void cmdCancelar_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void cmdSiguiente_Click(object sender, RoutedEventArgs e)
        {
            frmBandaEMV banda = new frmBandaEMV();
            banda.NumOrden.IsEnabled = false;
            banda.NumOrden.Text = numOrdenText;
            banda.Importe.IsEnabled = false;
            banda.Importe.Text = importeText;
            banda.strProveedor = strProveedor;
            banda.strNoServicio = strNoServicio;
            banda.strNoEconomico = strNoEconomico;
            abrir(banda);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }
    }
}
