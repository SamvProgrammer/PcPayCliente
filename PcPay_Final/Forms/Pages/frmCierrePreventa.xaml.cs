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
    /// Lógica de interacción para frmCierrePreventa.xaml
    /// </summary>
    public partial class frmCierrePreventa : Page
    {
        private const string NOMBRE_GENERAL = "frmCheckout";
        public abrirFrm abre;
        public cerrarVentana cierra;
        public frmCierrePreventa()
        {
            InitializeComponent();
            Cargar();
            txtReservacion.GotFocus += Globales.setFocusMit2;
            txtReservacion.LostFocus += Globales.lostFocusMit2;
        }

        private void Cargar()
        {

            fraP2.Visibility = Visibility.Hidden;
        }

        //'***********************************************************************************
        //'***********************************************************************************
        //'**                          cmdAceptar1_Click()                                  **
        //'**                                                                               **
        //'**  Descripción   : Hace una consulta con base a un numero de reservación        **
        //'**                  de las preventas realizadas por el usuario y asi mostrarlas  **
        //'**                  en options.                                                  **
        //'**                                                                               **
        //'***********************************************************************************
        //'***********************************************************************************
        private void cmdAceptar1_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdAceptar1_Click()";
            try
            {

                string strCadEncriptar, strCadaux = string.Empty;
                int i;


                if (string.IsNullOrEmpty(txtReservacion.Text))
                {
                    Globales.MessageBoxMit("Introduzca el No. de Reservación.");
                    return;
                }


                strCadEncriptar = "&usuario=" + TypeUsuario.usu +
                                       "&referencia=" + txtReservacion.Text.Trim() +
                                       "&op=consultapreventa";

                //DoEvents
                //MousePointer = vbHourglass
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                Globales.cpHTTP_Clear();
                Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
                Globales.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
                i = 0;

                if (Globales.cpHTTP_SendcpCUCT())
                {
                    Mouse.OverrideCursor = null;
                    strCadaux = Globales.cpHTTP_sResult;
                    if (Globales.GetDataXml("response", strCadaux) == "0")
                    {
                        fraP1.Visibility = Visibility.Visible;
                        fraP2.Visibility = Visibility.Hidden;
                        //MousePointer = vbNormal
                        Mouse.OverrideCursor = null;
                        txtReservacion.Focus();
                        Globales.MessageBoxMit("No existe referencia asociada.");
                    }
                    else
                    {

                        strCadaux = strCadaux.Replace("-", "");
                        do
                        {
                            RadioButton radios = new RadioButton();
                            radios.Visibility = Visibility.Visible;
                            //  string fecha = String.Format("DD-MMM-YY",Globales.GetDataXml("fecha" + i, strCadaux) );
                            string fecha = Globales.GetDataXml("fecha" + i, strCadaux);

                            DateTime fecha2 = Convert.ToDateTime(fecha);

                            fecha = fecha2.ToString("dd-MMM-yy");


                            radios.Content = Globales.GetDataXml("importe" + i, strCadaux) + " / " + fecha + " / " + Globales.GetDataXml("operacion" + i, strCadaux) + " / " + Globales.GetDataXml("mesero" + i, strCadaux) + " / " + Globales.GetDataXml("turno" + i, strCadaux) + " / " + Globales.GetDataXml("cc_name" + i, strCadaux);
                            // Meter aqui los radios
                            radios.Name = "a" + i;
                            radios.FontSize = 14;
                            AreaRadios.Children.Add(radios);

                            i++;
                            if (i >= 3)
                            {
                                //cmdAceptar2.Top = cmdAceptar2.Top + 640
                                //cmdCancelar2.Top = cmdCancelar2.Top + 640
                                //fraP2.Height = fraP2.Height + 640
                                //frmCierrePreventa.Height = frmCierrePreventa.Height + 640
                            }
                        } while (!string.IsNullOrEmpty(Globales.GetDataXml("mesero" + i, strCadaux)) && i < 11);






                        //   Loop
                        fraP1.Visibility = Visibility.Hidden;
                        fraP2.Visibility = Visibility.Visible;

                        //If Option1(0).Visible = True Then
                        //    Option1(0).value = True
                        //End If

                    }
                }

            }
            catch (Exception Err)
            {


                Globales.MessageBoxMit(Globales.strNombreFP + "\n Descripcion" + Err.Message + "\n");

            }
            Mouse.OverrideCursor = null;

        }

        private void cmdCancelar1_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Globales.strNombreFP = NOMBRE_GENERAL + ".cmdCancelar1_Click()";
                // this.
            }
            catch (Exception Err)
            {
                System.Windows.Forms.MessageBox.Show(Globales.strNombreFP + "\n Descripcion" + Err.Message + "\n", Globales.NOMBRE_APP, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

            }
        }

        private void cmdAceptar2_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdAceptar2_Click()";
            try
            {
                UIElementCollection col = AreaRadios.Children;
                foreach (RadioButton item in col)
                {
                    if (Convert.ToBoolean(item.IsChecked))
                    {
                        Globales.InfoCheckOut = item.Name.Substring(1, 1);
                    }
                }

                Globales.bolActivaMotoP = true;

                if (string.IsNullOrEmpty(Globales.InfoCheckOut))
                {
                    Globales.MessageBoxMit("Debe seleccionar una reservación.");
                    return;
                }

                frmProcesaCierrePreventa frmCp = new frmProcesaCierrePreventa();
                frmCp.cierra = cierra;
                frmCp.abrirFrmNuevo = abre;
                abre(frmCp);
            }
            catch (Exception Err)
            {
                Globales.MessageBoxMit(Globales.strNombreFP + "\n Descripcion" + Err.Message + "\n");
            }
        }

        private void cmdCancelar2_Click(object sender, RoutedEventArgs e)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".cmdCancelar2_Click()";
            try
            {
                // Cerrar
            }
            catch (Exception Err)
            {
                System.Windows.Forms.MessageBox.Show(Globales.strNombreFP + "\n Descripcion" + Err.Message + "\n", Globales.NOMBRE_APP, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

            }
        }

        private void Carga()
        {
            try
            {
                Globales.cpIntegraEMV.dbgEndOperation();
                Globales.strNombreFP = NOMBRE_GENERAL + ".Form_Load()";

                if (TypeUsuario.Id_Company == "0059")
                {
                    txtReservacion.MaxLength = 28;
                }
                else
                {

                    txtReservacion.MaxLength = Globales.MAXCAR;
                }

            }
            catch (Exception Err)
            {

            }


        }

        private void txtReservacion_KeyDown(object sender, KeyEventArgs e)
        {
            //char Tecla = (char)e.Key;

            //if(!Char.IsLetterOrDigit(Tecla)){
            //    e.Handled = true;
            //}
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void txtReservacion_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtReservacion.Focus();
        }






    }
}
