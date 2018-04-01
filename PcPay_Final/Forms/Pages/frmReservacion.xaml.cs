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
    /// Lógica de interacción para frmReservacion.xaml
    /// </summary>
    public partial class frmReservacion : Page
    {
        private string auxiliar;
        private bool NoExtra;
        private string NOMBRE_GENERAL;
        private bool banboletosrepetidos;
        public string digito_travel { get; set; }
        public List<string> lista_boletos { get; set; }
        public frmReservacion()
        {
            InitializeComponent();

            this.frmDatos.Visibility = Visibility.Hidden;
            this.BoletosExtra.Visibility = Visibility.Hidden;

            this.BRETORNO.Visibility = Visibility.Hidden;
            this.LRETORNO.Visibility = Visibility.Hidden;

            Cargar();
            #region"**********Efectos**********"

            this.txtReservacion.GotFocus += Globales.setFocusMit2;
            this.txtReservacion.LostFocus += Globales.lostFocusMit2;


            this.txtImporte.GotFocus += Globales.setFocusMit2;
            this.txtImporte.LostFocus += Globales.lostFocusMit2;

            TNUMBOLETO.GotFocus += Globales.setFocusMit2;
            TNUMBOLETO.LostFocus += Globales.lostFocusMit2;
            #endregion

            txtImporte.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDownImporte;
            TNUMBOLETO.PreviewKeyDown += Globales.onlyNumbers_PreviewKeyDown;
        }

        private void Cargar()
        {
            Fecha.DisplayDateStart = DateTime.Now;

            if (Globales.isAgencias && !string.IsNullOrWhiteSpace(Globales.GetDataXml("catempresas", TypeUsuario.CadenaXML)))
            {

                ObtieneEmpresasAgencia(Globales.GetDataXml("catempresas", TypeUsuario.CadenaXML), "B");
                cboEmpresa.SelectedIndex = 0;
                //lblTitulo.Caption = "Aerolínea:";
            }
            else if (Globales.isAerolinea)
            {
                cboEmpresa.SelectedIndex = 0;
                cboEmpresa.Text = TypeUsuario.nb_company + "-" + TypeUsuario.Id_Company;
                // cmdAcepEmp_Click
                // FechaRetorno.Visible = True
                // Label(13).Visible = True

                //  FechaR.Visible = True
                FechaR.Text = "";

                txtReservacion.MaxLength = 40;
                FechaR.TabIndex = 4;
            }


            label7.Content = TypeUsuario.reference;

        }
        private void ObtieneEmpresasAgencia(string strBancos, string tipo)
        {
            string[] empresas = strBancos.Split('|');

            for (int i = 0; i < empresas.Length; i++)
            {
                if (empresas[i].Contains("," + tipo + ","))
                {
                    auxiliar += empresas[i] + "|";
                    string[] aux = empresas[i].Split(',');

                    cboEmpresa.Items.Add(aux[1] + " - " + aux[0]);
                }
            }


        }

        private void cmdAlmacena_Click(object sender, RoutedEventArgs e)
        {
            string reg_agencia = string.Empty;
            string BanStrBoletos = string.Empty;

            string boletos = string.Empty;
            boletos = this.CadenaBoletos();




            if (string.IsNullOrWhiteSpace(txtReservacion.Text))
            {
                Globales.MessageBoxMit("Favor de introducir la reservación.");
                return;
            }
            else if (txtReservacion.Text.Length != 6)
            {
                if (!Globales.isAerolinea)
                {
                    Globales.MessageBoxMit("La reservación debe tener 6 caracteres.");
                    return;
                }
            }
            else if (txtReservacion.Text.Length > 7)
            {
                Globales.MessageBoxMit("La reservacón debe tener máximo 7 caracteres.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtImporte.Text.Trim()))
            {

                Globales.MessageBoxMit("Favor de introducir el importe.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Fecha.Text.Trim()))
            {
                Globales.MessageBoxMit("Favor de seleccionar la fecha de salida.");
                return;
            }
            if (string.IsNullOrWhiteSpace(boletos))
            {
                Globales.MessageBoxMit("Favor de introducir el boleto.");
                return;
            }
            if (Globales.isAerolinea)
            {


            }
            if (Globales.isAerolinea)
            {
                if (!string.IsNullOrWhiteSpace(boletos))
                {
                    if (!Globales.insertaBoletoAgencia(TypeUsuario.Id_Company, "", boletos, Fecha.Text, FechaR.Text))
                    {
                        cmdAlmacena.IsEnabled = true;
                        return;
                    }
                }
            }
            else
            {

                if (!string.IsNullOrWhiteSpace(boletos))
                {
                    if (!Globales.insertaBoletoAgencia(TypeUsuario.Id_Company, "", boletos, Fecha.Text, ""))
                    {
                        cmdAlmacena.IsEnabled = true;
                        return;
                    }
                }
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            this.BADDBOLETOS.IsEnabled = false;
            Globales.cpIntegracion_sURL_cpINTEGRA = Globales.URL_DLL;

            string fechaS = string.Empty;
            //'apago todo
            txtReservacion.IsEnabled = false;
            txtImporte.IsEnabled = false;

            bool respuestaResevacion = false;
            if (Globales.isAerolinea)
            {
                respuestaResevacion = Globales.insertaReservacion(txtReservacion.Text.Trim(),
                                    Fecha.Text.Trim(),
                                    FechaR.Text.Trim(),
                                    txtImporte.Text.Trim(),
                                    Globales.mynumero, boletos);
            }
            else
            {
                respuestaResevacion = Globales.insertaReservacion(txtReservacion.Text.Trim(),
                                    Fecha.Text.Trim(), "",
                                    txtImporte.Text.Trim(),
                                    Globales.mynumero, boletos);
            }

            Mouse.OverrideCursor = null;
            if (respuestaResevacion)
            {
                cmdNuevo.IsEnabled = true;
                cmdAlmacena.IsEnabled = false;
                txtReservacion.IsEnabled = false;
                txtImporte.IsEnabled = false;
                Fecha.IsEnabled = false;
            }
            else
            {
                Fecha.IsEnabled = true;
                txtReservacion.IsEnabled = true;
                txtImporte.IsEnabled = true;
                cmdAlmacena.IsEnabled = true;
                cmdNuevo.IsEnabled = false;
                this.BADDBOLETOS.IsEnabled = true;
            }
            Globales.cpHTTP_sResult = "";
        }

          private string boletos()
        {


            return Globales.CadenaBoletos(BoletosExtra, string.Empty, NoExtra);
        }
        private void cmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            this.lista_boletos.Clear();
            this.LBOLETOS.Items.Clear();

            txtReservacion.Text = "";
            txtImporte.Text = "";
            Fecha.Text = "";
            FechaR.Text = "";
            txtReservacion.IsEnabled = true;

            txtImporte.IsEnabled = true;
            cmdNuevo.IsEnabled = false;
            cmdAlmacena.IsEnabled = true;
            Fecha.IsEnabled = true;

            this.LTOTALBOLETOS.Content = "0";
            this.LADDBOLETOS.Content = string.Format("Agregar boletos({0})", this.lista_boletos.Count);
            this.BADDBOLETOS.IsEnabled = true;
        }

        private void cmdAcepEmp_Click(object sender, RoutedEventArgs e)
        {

            if (cboEmpresa.Text.Contains("-") || cboEmpresa.SelectedIndex != -1)
            {
                fraCubreTodo.Visibility = Visibility.Hidden;
                frmDatos.Visibility = Visibility.Visible;
                this.LTITULO.Content = "Preventa de boleto: " + cboEmpresa.Text;

                Globales.mynumero = Globales.Right(cboEmpresa.Text, 4);

                string lacad;
                lacad = cboEmpresa.Text.Trim();


                if ((cboEmpresa.SelectedIndex != -1) || Globales.isAerolinea)
                {
                    if (Globales.isAerolinea)
                    {
                        if (TypeUsuario.iata.Length < 3)
                        {
                            //lbl3Digitos.Content = "0" + TypeUsuario.iata.Trim();

                        }
                        else
                        {
                            //lbl3Digitos.Content = TypeUsuario.iata.Trim();
                        }
                    }
                    else
                    {
                        string busca = cboEmpresa.Text;
                        busca = busca.Split('-')[1];
                        string digito = string.Empty;
                        string[] p = auxiliar.Split('|');
                        for (int b = 0; b < p.Length; b++)
                        {

                            string auxi = p[b];
                            auxi = auxi.Split(',')[0];
                            if (auxi == busca.Trim())
                            {
                                digito = p[b].Split(',')[3];
                            }
                        }

                        this.digito_travel = (digito.Length < 3) ? string.Format("0{0}", digito) : digito;
                        UserIata.Content ="IATA: "+ digito_travel;
                        TypeUsuario.iata = digito_travel;
                        
                        //if( Len(Trim(Str(cboEmpresa.ItemData(cboEmpresa.ListIndex)))) < 3 Then){
                        //    dig.Caption = "0" & Trim(Str(cboEmpresa.ItemData(cboEmpresa.ListIndex)))// 'TypeUsuario.iata
                        //}
                        //else{
                        //    dig.Content = Trim(Str(cboEmpresa.ItemData(cboEmpresa.SelectedIndex))); //'TypeUsuario.iata
                        //}
                    }
                }
                else
                {
                    if (Globales.IsNumeric(Utils.Left(lacad, 3)))
                    {
                        //lbl3Digitos.Content = Utils.Left(cboEmpresa.Text, 3).Trim();
                    }
                    else
                    {

                        for (int i = 0; i < cboEmpresa.Items.Count; i++)
                        {
                            //if((cboEmpresa.SelectedIndex=i) == lacad){
                            //    dig.Content = Trim(Str(cboEmpresa.ItemData(i)))
                            //}



                        }
                    }
                }
            }
            else
            {
                Globales.MessageBoxMit("Seleccione una empresa valida");
            }
            Mouse.OverrideCursor = null;
        }

        private void txtImporte_LostFocus(object sender, RoutedEventArgs e)
        {
            Globales.formatoMoneda(sender, e);
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Globales.noCopy(sender, e);
        }

        private void NumeroPunto(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumeroConPunto(sender, e);
        }

        private void Command2_Click(object sender, RoutedEventArgs e)
        {
            this.TNUMBOLETO.Text = string.Empty;


            if (this.lista_boletos == null)
                this.lista_boletos = new List<string>();

            if (!banboletosrepetidos)
            {
                BoletosExtra.Visibility = Visibility.Hidden;
                this.frmDatos.Visibility = Visibility.Visible;
                this.LTOTALBOLETOS.Content = this.lista_boletos.Count;
                this.LADDBOLETOS.Content = string.Format("Agregar boletos({0})", this.lista_boletos.Count);
            }

        }


        private void Fecha_CalendarClosed(object sender, RoutedEventArgs e)
        {

        }

        private void Numeros(object sender, TextCompositionEventArgs e)
        {
            Globales.soloNumero(sender, e);
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir(false);
        }






        #region "Formulario de boletos"
        private void BAGREGABOLETO_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TNUMBOLETO.Text))
            {
                Globales.MessageBoxMit("Ingrese un número de boleto.");
                TNUMBOLETO.Focus();
                return;
            }
            if (Globales.verificadorBoleto(TNUMBOLETO.Text))
            {
                TNUMBOLETO.Focus();
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
                        TNUMBOLETO.Focus();
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
                    TNUMBOLETO.Focus();
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
                TNUMBOLETO.Focus();
                this.TNUMBOLETO.Text = string.Empty;
            }
        }
        private void ELIMINARBOLETO_Click(object sender, RoutedEventArgs e)
        {
            if (this.LBOLETOS.SelectedItem != null)
            {
                var valor = this.LBOLETOS.SelectedItem.ToString();
                this.lista_boletos.Remove(valor);
                this.LBOLETOS.Items.Clear();
                this.LBOLETOS.Items.Refresh();

                this.lista_boletos.ForEach(delegate(string elm)
                {
                    this.LBOLETOS.Items.Add(elm);
                });
                this.LTOTALBOLETOS.Content = this.lista_boletos.Count;

            }
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BoletosExtra.Visibility = Visibility.Visible;
            this.frmDatos.Visibility = Visibility.Hidden;

            if (this.lista_boletos == null)
                this.lista_boletos = new List<string>();
        }



        private string CadenaBoletos()
        {
            //return Globales.CadenaBoletos(BoletosExtra, lbl3Digitos.Content.ToString(), NoBoletosExtra);
            string returnValue = string.Empty;

            if (this.lista_boletos == null)
                returnValue = string.Empty;
            else
            {
                if (this.lista_boletos.Count == 0)
                    returnValue = string.Empty;
                else
                    returnValue = string.Join(",", this.lista_boletos.ToArray());
            }
            return returnValue;
        }
        #endregion

        private void letraynumero(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }
    }
}
