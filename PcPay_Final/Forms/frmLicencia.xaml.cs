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
using PcPay.Code.Usuario;
using System.IO;
using PcPay.Forms.Formularios;

namespace PcPay.Forms
{
    /// <summary>
    /// Interaction logic for frmLicencia.xaml
    /// </summary>
    public partial class frmLicencia : Window
    {
        private int indice = 0;
        private bool atras = false;
        private bool siguiente = false;
        private AxiohmPrinterMit.sPort impresora;//=  AxiohmPrinterMit.sPortClass;
        public string NOMBRE_APP = "FrmLicencia";
        public bool chkImpresora { get; set; }
        public bool finalizarConfiguracion { get; set; }


        public frmLicencia()
        {
            InitializeComponent();
            primero.Focus();
            if(Globales.cpIntegraEMV.EsTouch()){
                image6.MouseDown += new MouseButtonEventHandler(mensaje1);
                image7.MouseDown += new MouseButtonEventHandler(mensaje1);
                image8.MouseDown += new MouseButtonEventHandler(mensaje1);
            }
            version.Content = Globales.GetVersionApp();
        }

        private void mensaje1(object sender, MouseButtonEventArgs e)
        {
            Image imagen = sender as Image;
            var aux = image6.Name;
            if(aux == "image6"){
                System.Windows.Forms.MessageBox.Show("Marque la casilla si quiere imprimir automáticamente el voucher");
            }
            else{
                System.Windows.Forms.MessageBox.Show("Marque la casilla si quiere imprimir dos voucher por hoja");
            }
        }

        private bool SetImpresora()
        {
            Globales.strNombreFP = NOMBRE_APP + ".cmdOk_Click()";
            int RetVal = 0;
            int i = 0;
            bool setImpresoraVariable = false;
            if (Convert.ToBoolean(rd1.IsChecked))
            {
                TypeUsuario.TipoImpresora = "6";

            }
            if (Convert.ToBoolean(rd3.IsChecked))
            {
                TypeUsuario.TipoImpresora = "1";
                if (chkImpresora)
                {
                    Globales.banImp = 1;
                    Globales.SaveSettingString("AUTOMATICO", "1");
                }
                else
                {
                    Globales.banImp = 0;
                    Globales.SaveSettingString("AUTOMATICO", "0");
                }
                //modMIt.printerName = comboBox1.GetItemText(comboBox1.SelectedItem);
            }
            else if (Convert.ToBoolean(rd2.IsChecked))
            {
                TypeUsuario.TipoImpresora = "4";
                if (chkImpresora)
                {
                    Globales.banImp = 1;
                    Globales.SaveSettingString("Automatico", "1");
                }
                else
                {
                    Globales.banImp = 0;
                    Globales.SaveSettingString("Automatico", "0");
                }
                // modMIt.printerName = comboBox1.GetItemText(comboBox1.SelectedItem);
            }
            else if (Convert.ToBoolean(rd4.IsChecked))
            {
                InitTerminalPrinter();
                if (iniciaImpresora())
                {
                    TypeUsuario.TipoImpresora = "2";
                    CierraImpresora();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("No se pudo iniciar la impresora térmica.", "Aviso", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    TypeUsuario.TipoImpresora = "0";
                }
            }
            else if (Convert.ToBoolean(rd5.IsChecked))
            {
                int intLeft;
                string strLeft;
                TypeUsuario.TipoImpresora = "3"; //Impresora tipo: Epson LX-300+
                string strTipoFont = "";//txtFont.Text.Trim();
                //i = WritePrivateProfileString("MIT_APP","TIPOFONT",strTipoFont,"MIT.ini")
                TypeUsuario.strFont = strTipoFont;
                TestEpson(0);
            }
            else if (Convert.ToBoolean(rd6.IsChecked))
            {
                /*
                     Descripción: Modo de impresión manual con la opción de seleccionar la impresora de forma manual,
                 * controla el numero de vouchers a imprimir por hoja.
                 */
               
             
                int indice = 0;
                RawDataPrinter.Printer RDP = new RawDataPrinter.Printer();
                if (/*RDP.SetPrinter(comboBox1.GetItemText(comboBox1.SelectedItem))*/ false)
                {
                    TypeUsuario.TipoImpresora = "5";
                    System.Windows.Forms.MessageBox.Show("Impresora a usar: " + "Algo aqui va escrito" /*comboBox1.GetItemText(comboBox1.SelectedItem)*/, "Nombre de impresora", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    System.Windows.Forms.DialogResult i1 = System.Windows.Forms.MessageBox.Show("Porfavor asegurase de que la impresora esté encendida", "Impresora", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (i1 == System.Windows.Forms.DialogResult.Cancel)
                    {
                        System.Windows.Forms.MessageBox.Show("No se pudo definir un tipo de impresora", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        TypeUsuario.TipoImpresora = "0";
                    }
                    else
                    {
                        //indice = txtECC.Text.IndexOf("7)") + 4;
                        //if (txtEC.Text != "")
                        //{
                        //    modMIt.config = replaceChr(txtEC.Text);
                        //}
                        //else
                        //{
                        //    modMIt.config = "";
                        //}
                        //if (txtECC.Text != "")
                        //{
                        //    modMIt.configCentrado = txtECC.Text.Substring(10, txtECC.Text.Length - 10);
                        //}
                        //else
                        //{
                        //    modMIt.configCentrado = "";
                        //}
                        //if (txtECL.Text != "")
                        //{
                        //    modMIt.configLeyenda = replaceChr(txtECL.Text);
                        //}
                        //else
                        //{
                        //    modMIt.configLeyenda = "";
                        //}
                        //if (txtECCte.Text != "")
                        //{
                        //    modMIt.configCorte = replaceChr(txtECCte.Text);
                        //}
                        //else
                        //{
                        //    modMIt.configCorte = "";
                        //}
                        //if (txtECMI.Text != "")
                        //{
                        //    modMIt.margenIzq = txtECMI.Text.Substring(10, txtECMI.Text.Length - 10);
                        //}
                        //else
                        //{
                        //    modMIt.margenIzq = "";
                        //}
                        //if (txtECA.Text != "")
                        //{
                        //    Globales.centrado =""// replaceChr(txtECA.Text);
                        //}
                        //else
                        //{
                        //    Globales.centrado = "";
                        //}

                        // Globales.config += replaceChr(txtECMI.Text) + replaceChr(txtECC.Text);
                        Globales.SaveSettingString("PRINTER", /*comboBox1.GetItemText(comboBox1.SelectedItem)*/"");
                        Globales.SaveSettingString("PRINTERCONFIG", Globales.config);
                        Globales.SaveSettingString("MARGENDER", Globales.configCentrado);
                        Globales.SaveSettingString("LEYENDA", Globales.configLeyenda);
                        Globales.SaveSettingString("CORTE", Globales.configCorte);
                        Globales.SaveSettingString("MARGENIZQ", Globales.margenIzq);
                        Globales.SaveSettingString("CENTRADO", Globales.centrado);

                        //if (txtEC.Text == "")
                        //{
                        //    MessageBox.Show("Tus documentos se imprimiran en el formato predifinido.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //}
                        // Globales.printerName = comboBox1.GetItemText(comboBox1.SelectedItem);
                        Globales.clsImpr.RDP.PrintRawData(Globales.config + "Pagina de prueba\n0123456789\nABCDEFGHIJKLMNÑOPQRSTUWXYZ\n\n\n\n\n" + Globales.configCorte);
                        System.Windows.Forms.MessageBox.Show("Tu pagina de prueba se a impreso correctamente.", "Exito", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }
                else
                {
                    TypeUsuario.TipoImpresora = "0";
                    System.Windows.Forms.MessageBox.Show("No se encontró impresora.", "Impresora", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                TypeUsuario.TipoImpresora = "0"; //Impresora tipo: N/A
            }


            //i = writePriuvateProfileString --> kernel32 --> parametros --> "MIT_APP","PRINTER",Usuario.TipoImpresora,"MIT.ini" 
            setImpresoraVariable = true;
            Globales.SaveSettingString("PRINTERNUMBER", TypeUsuario.TipoImpresora);
            return setImpresoraVariable;
        }

        private void CierraImpresora()
        {
            impresora.StartPort(99);
        }

        private void TestEpson(int p)
        {
            Globales.strNombreFP = Globales.NOMBRE_APP + ".TestEpston()";
            System.Windows.Forms.MessageBox.Show("Verifique que la impresorra este lista y preparada para imprimir", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

        }

        private bool iniciaImpresora()
        {
            bool bprinter = false;
            bool iniciaImpresoraVariable = false;
            short i = 0; //Esta parte del código esta en duda porque no se si es 1 o 0.
            Globales.strNombreFP = Globales.strNombreFP + ".IniciaImpresora()";
            while (!bprinter && i < 4)
            {
                bprinter = impresora.StartPort(i);
                if (bprinter)
                {
                    impresora.PrintLine("Impresora inicializada correctamente.\n");
                    for (int x = 1; x <= 7; x++)
                    {
                        impresora.PrintLine("");
                    }
                    System.Windows.Forms.DialogResult resultado = System.Windows.Forms.MessageBox.Show("¿La impresora se ha inicializado correctamente?", "Impresora", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                    if (resultado == System.Windows.Forms.DialogResult.Yes)
                    {
                        impresora.cutPaper();
                        bprinter = true;
                        iniciaImpresoraVariable = true;
                    }
                    else if (resultado == System.Windows.Forms.DialogResult.No)
                    {
                        bprinter = false;
                    }
                }
                i++;
            }

            return iniciaImpresoraVariable;
        }

        private void InitTerminalPrinter()
        {
            string est = string.Empty;
            // string directorioPadre = Path.GetPathRoot(Environment.SystemDirectory);
            System.Windows.Forms.DialogResult resultado = System.Windows.Forms.MessageBox.Show("El modelo de la impresora térmica es\n\"APOS PREMIUM PRINTER?\"", "Tipo de impresora", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
            if (resultado == System.Windows.Forms.DialogResult.Yes)
            {
                est = "Axiohm Premium";
            }
            else
            {
                est = "Axiohm";
            }
            /*
             i = WriteProfileString("PRINTER","TYPE",est,"AxiohmPrinter.ini")
             * set fs = createObject("scriptong.FileSystemobject")
             * fs.CopyFile strWindow&"\AxiohmPrinter.ini",strSystem&"\axiuhmPriner.ini"
             */
        }

        private bool registrar()
        {
            Globales.strNombreFP = Globales.NOMBRE_APP + ".form_Unload()";
            int iCOM = 0;
            bool registrar = false;
            Globales.SaveSettingString("IdUnico", "-1");
            Globales.SaveSettingString("License", "-1");

            registrar = true;
            return true;
            //Aqui empieza el otro codigo
            //if (string.IsNullOrWhiteSpace(txtUser.Text))
            //{
            //    MessageBox.Show("Introduzca el usuario.", "Usuario requerido", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}
            //if (string.IsNullOrWhiteSpace(txtPass.Text))
            //{
            //    MessageBox.Show("Introdusca la contraseña", "Contraseña requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}

            //txtUser.Enabled = false;
            //txtPass.Enabled = false;

            ////Funcion para registrar el sistema--> STD_Registrar_Sistema(usuario,contra, numeroDeLicencia(usuario),AccionOntieneLicencia. )
            //if (!registrarSistema(txtUser.Text, txtPass.Text))
            //{
            //    MessageBox.Show("Su licencia no ha sido registrada", "Error licencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    modRegistry.SaveSettingsString("IdUnico", "");
            //    modRegistry.SaveSettingsString("Licence", "");
            //    txtUser.Text = "";
            //    txtPass.Text = "";
            //    txtUser.Enabled = true;
            //    txtPass.Enabled = true;

            //    return false;
            //}
            //else
            //{
            //    modRegistry.SaveSettingsString("IdUnico", "-1");
            //    modRegistry.SaveSettingsString("Licence", "-1");
            //    MessageBox.Show("Su licencia ha sido registrada.", "Licencia registrada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //    registrado = true;
            //    return true;
            //}

            //return registrar;

            //Aqui va más operaciones ver código visual basic.
        }

        private void probarClick(object sender, RoutedEventArgs e)
        {
            if (Globales.cpIntegraEMV.chkPp_Printer() == "1")
            {
                Globales.cpIntegraEMV.dbgPrint("@lnn El lector funciona correctamente! @br @br @br @br @br @br @br @br @br");
            }
            else
            {
                Globales.cpIntegraEMV.dbgSendMessage("PcPay LISTO!!");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!finalizarConfiguracion)
            {
                System.Windows.Application.Current.Shutdown();
                string ruta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\MIT\data\parameters.mit";
                if (File.Exists(ruta))
                {
                    File.Delete(ruta);
                }
            }
        }

        private void CmdNav(object sender, RoutedEventArgs e)
        {


            Button boton = sender as Button;
            string nombre = boton.Name.Replace("btn", "").ToUpper();
            switch (nombre)
            {
                case "AYUDA":
                    if (primero.Visibility == Visibility.Visible)
                    {
                        System.Windows.Forms.MessageBox.Show("Para instalar y configurar Pagos MIT en su equipo, haga clic en el botón Siguiente");
                    }
                    else if (segundo.Visibility == Visibility.Visible)
                    {
                        System.Windows.Forms.MessageBox.Show("Su Usuario y Contraseña los recibió vía correo electrónico ó a través del Administrador de su Empresa");
                    }
                    else if (tercero.Visibility == Visibility.Visible)
                    {
                        System.Windows.Forms.MessageBox.Show("En esta pantalla puede observar la configuración de su lector conectado");
                    }
                    else if (cuarto.Visibility == Visibility.Visible)
                    {
                        System.Windows.Forms.MessageBox.Show("De no ser exitosa la prueba, verifique que su lector está conectado correctamente");
                    }
                    else if (quinto.Visibility == Visibility.Visible)
                    {
                        System.Windows.Forms.MessageBox.Show("Haga clic en la opción de impresora que requiera");
                    }
                    else if (sexto.Visibility == Visibility.Visible)
                    {
                        System.Windows.Forms.MessageBox.Show("Pagos MIT se ha configurado correctamente. Haga clic en Finalizar");
                    }
                    break;
                case "CANCELAR":
                    Cancelar();
                    break;
                case "ANTERIOR":

                    if (segundo.Visibility == Visibility.Visible)
                    {
                        btnAnterior.IsEnabled = false;
                        segundo.Visibility = Visibility.Hidden;
                        primero.Visibility = Visibility.Visible;

                    }
                    else if (tercero.Visibility == Visibility.Visible)
                    {
                        btnAnterior.IsEnabled = false;
                        tercero.Visibility = Visibility.Hidden;
                        primero.Visibility = Visibility.Visible;
                    }
                    else if (cuarto.Visibility == Visibility.Visible)
                    {
                        tercero.Visibility = Visibility.Visible;
                        cuarto.Visibility = Visibility.Hidden;
                    }
                    else if (quinto.Visibility == Visibility.Visible)
                    {
                        if (Globales.cpIntegraEMV.chkPp_Trademark() != "Magtek")
                        {
                            quinto.Visibility = Visibility.Hidden;
                            cuarto.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            quinto.Visibility = Visibility.Hidden;
                            tercero.Visibility = Visibility.Visible;
                        }
                    }
                    else if (sexto.Visibility == Visibility.Visible)
                    {
                        if (Globales.cpIntegraEMV.chkPp_Trademark() != "Magtek")
                        {
                            sexto.Visibility = Visibility.Hidden;
                            cuarto.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            sexto.Visibility = Visibility.Hidden;
                            quinto.Visibility = Visibility.Visible;
                        }
                        btnFinalizar.IsEnabled = false;
                        btnSiguiente.IsEnabled = true;
                    }
                    break;
                case "SIGUIENTE":
                    if (primero.Visibility == Visibility.Visible)
                    {
                        registrar();
                        btnAnterior.IsEnabled = true;
                        btnSiguiente.IsEnabled = false;
                        txtLector.Text = Globales.EstableceLector();
                        btnSiguiente.IsEnabled = true;
                        primero.Visibility = Visibility.Hidden;
                        tercero.Visibility = Visibility.Visible;
                    }
                    else if (segundo.Visibility == Visibility.Visible)
                    {
                        //Falta código de licencia...
                    }
                    else if (tercero.Visibility == Visibility.Visible)
                    {
                        if (Globales.cpIntegraEMV.chkPp_Printer() == "0")
                        {
                            lblTestIgenico.Visibility = Visibility.Visible;
                            lbTestVX.Visibility = Visibility.Hidden;
                            cmdTest.IsEnabled = true;
                        }
                        else if (Globales.cpIntegraEMV.chkPp_Printer() == "1")
                        {
                            lblTestIgenico.Visibility = Visibility.Hidden;
                            lbTestVX.Visibility = Visibility.Visible;
                            cmdTest.IsEnabled = true;
                            Globales.SaveSettingString("PRINTERNUMBER", "6");
                        }
                        else
                        {
                            string mensaje = "El tipo de lector configurado no permite esta opción";
                            lblLabels2.Content = mensaje;
                            System.Windows.Forms.MessageBox.Show(mensaje);
                            lblTestIgenico.Visibility = Visibility.Hidden;
                            lbTestVX.Visibility = Visibility.Hidden;
                            cmdTest.IsEnabled = false;
                        }
                        tercero.Visibility = Visibility.Hidden;
                        cuarto.Visibility = Visibility.Visible;
                    }
                    else if (cuarto.Visibility == Visibility.Visible)
                    {
                        cuarto.Visibility = Visibility.Hidden;
                        if (Globales.cpIntegraEMV.chkPp_Trademark() == "Magtek")
                        {
                            sexto.Visibility = Visibility.Visible;
                            btnFinalizar.IsEnabled = true;
                            btnSiguiente.IsEnabled = false;
                            btnCancelar.IsEnabled = true;
                        }
                        else
                        {
                            string opcion = Globales.cpIntegraEMV.chkPp_Printer();
                            if (Globales.cpIntegraEMV.chkPp_Printer() == "0" || string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_Printer()))
                            {
                                quinto.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                sexto.Visibility = Visibility.Visible;
                                btnSiguiente.IsEnabled = false;
                                btnFinalizar.IsEnabled = true;
                                btnCancelar.IsEnabled = true;
                            }
                        }
                    }
                    else if (quinto.Visibility == Visibility.Visible)
                    {
                        if (SetImpresora())
                        {
                            quinto.Visibility = Visibility.Hidden;
                            sexto.Visibility = Visibility.Visible;
                            btnSiguiente.IsEnabled = false;
                            btnFinalizar.IsEnabled = true;
                            btnCancelar.IsEnabled = true;
                        }
                    }
                    break;
                case "FINALIZAR":
                    finalizarConfiguracion = true;
                    Close();
                    this.VerificarImagenes();
                    new frmLogin().Show();
                    break;
            }

        }

        private void Cancelar()
        {
            System.Windows.Forms.DialogResult dialo = Globales.mensajeQuestion("¿Desea salir de la configuración?", "");
            if (System.Windows.Forms.DialogResult.Yes == dialo)
            {
                string ruta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\MIT\data\parameters.mit";
                string temporal = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                if (File.Exists(ruta))
                {
                    File.Delete(ruta);
                }
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            primero.Visibility = Visibility.Visible;
            segundo.Visibility = Visibility.Hidden;
            tercero.Visibility = Visibility.Hidden;
            cuarto.Visibility = Visibility.Hidden;
            quinto.Visibility = Visibility.Hidden;
            sexto.Visibility = Visibility.Hidden;
            gridManual.Visibility = Visibility.Hidden;
            combo1 = Globales.llenaImp(combo1);
        }

        private void VerificarImagenes()
        {
            Globales.SaveSettingString("IMAGESv10", string.Empty);
        }

        private void rd5_Click(object sender, RoutedEventArgs e)
        {
            frmImpresoraPlug.Visibility = Visibility.Hidden;
            gridManual.Visibility = Visibility.Hidden;
            chkimp.Visibility = Visibility.Hidden;
            chkNoVchr.Visibility = Visibility.Hidden;
            image6.Visibility = Visibility.Hidden;
            image7.Visibility = Visibility.Hidden;
            image8.Visibility = Visibility.Hidden;
            if(Convert.ToBoolean(rd5.IsChecked)){
                frmImpresoraPlug.Visibility = Visibility.Visible;
            }else if(Convert.ToBoolean(rd6.IsChecked)){
                gridManual.Visibility = Visibility.Visible;
                image8.Visibility = Visibility.Visible;
                image7.Visibility = Visibility.Visible;
                chkNoVchr.Visibility = Visibility.Visible;
            }
            else if (Convert.ToBoolean(rd2.IsChecked) || Convert.ToBoolean(rd3.IsChecked))
            {
                image6.Visibility = Visibility.Visible;
                chkimp.Visibility = Visibility.Visible;
            }            
        }

        private void image7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new frmAyudaEC().ShowDialog();
        }

    }
}
