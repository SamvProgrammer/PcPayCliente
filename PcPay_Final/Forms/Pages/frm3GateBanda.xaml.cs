using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Lógica de interacción para frm3GateBanda.xaml
    /// </summary>
    public partial class frm3GateBanda : Page
    {

        //'Manejo de Mensajes.
        private string NOpe { get; set; }
        public int IndexO { get; set; }
        private string numEmpr { get; set; }
        private string VoucherTrx { get; set; }
        private string CdO1 { get; set; }
        private string RO { get; set; }


        public frm3GateBanda()
        {
            InitializeComponent();
        }


        private void CmdLeer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (vacio(TxtReferencia.Text))
                {
                    System.Windows.Forms.MessageBox.Show("Introduzca una referencia para la activación MITarjeta.", Globales.NOMBRE_APP, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    TxtReferencia.Focus();
                    //Exit Sub
                    return;
                }
                if (vacio(Importe.Text))
                {

                    System.Windows.Forms.MessageBox.Show("Introduzca el importe MITarjeta.", Globales.NOMBRE_APP, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    Importe.Focus();
                    //Exit Sub  
                    return;
                }

                //FormaPago.Enabled = False
                //cboBanco.Enabled = False   
                Importe.IsEnabled = false;
                TxtReferencia.IsEnabled = false;

                CmdLeer.IsEnabled = false;
                //CmdEnviar.Default = True

                NumTdc.Text = "";
                NomTdc.Text = "";
                Mes.Text = "";
                Anio.Text = "";

                //SetMensaje "Inserta el chip o desliza tarjeta y Espera un momento..." & vbNewLine & "Sigue las instrucciones del lector", &HD28106, LblTInfo
                //    Globales.SetMensaje();
                LblTInfo.Text = "Inserta el chip o desliza tarjeta y Espera un momento..." + Environment.NewLine + "Sigue las instrucciones del lector";
                LblTInfo.Foreground = Brushes.Blue;


                if (Globales.CpCobro3G.dbgActivaLector())
                {
                    if (vacio(Globales.CpCobro3G.chkCc_Number()))
                    {
                        NumTdc.Text = Globales.CpCobro3G.chkCc_Number();
                        NomTdc.Text = Globales.CpCobro3G.chkCc_Name();
                        Mes.Text = Globales.CpCobro3G.chkCc_ExpMonth();
                        Anio.Text = Globales.CpCobro3G.chkCc_ExpYear();

                        if (Globales.GetDataXml("csvamexenbanda", TypeUsuario.CadenaXML) == "1" /*&& Globales.CpCobro.chkCc_Number.Length == 15*/)
                        {
                            //frmCsvAMEX.Show vbModal
                        }
                        //MousePointer = vbNormal
                        cmdActual(CmdEnviar);
                        CmdEnviar.Focus();
                    }
                }
                else
                {
                    //if (!vacio(Globales.CpCobro3G.chkPp_DsError()))
                    //{

                    //    System.Windows.Forms.MessageBox.Show(Globales.CpCobro3G.chkPp_DsError, Globales.NOMBRE_APP, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    //    cmdActual(CmdLeer);
                    //    // MousePointer = vbNormal
                    //}
                }
                Globales.cpIntegraEMV.dbgSetTrxData(TypeUsuario.usu, TypeUsuario.Pass, "", TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country);
            }
            catch
            {
            }
        }

        private void cmdActual(Button cmd)
        {
            CmdLeer.IsEnabled = true;
            CmdEnviar.IsEnabled = true;
            //    cmdNuevo.IsEnabled = true;

            CmdLeer.Visibility = Visibility.Hidden;
            CmdEnviar.Visibility = Visibility.Hidden;
            // cmdNuevo.Visibility = Visibility.Hidden;

            cmd.Visibility = Visibility.Visible;
            cmd.IsEnabled = true;
            ///cmd.Default = True
        }


        private bool vacio(string cadena)
        {
            return string.IsNullOrEmpty(cadena);
        }

        private void CmdNuevo_Click(object sender, RoutedEventArgs e)
        {
            lblMoneda.Content = "MXN";
            VoucherTrx = "";
            Globales.isSmart = false;
            //Unload Me
            //frm3GateReferencia.Show
        }

  

        private void CmdEnviar_Click(object sender, RoutedEventArgs e)
        {
            CmdEnviar.IsEnabled = false;
            Enviar();

        }

        private void Enviar()
        {
            Globales.strNombreFP = "frmGiftVenta.cmdEnviar_Click()";
            LblTInfo.Text = "";
            CmdEnviar.IsEnabled = false;

            Importe.IsEnabled = false;
            TxtReferencia.IsEnabled = false;

            // MousePointer = vbHourglass
            string strTypeC = string.Empty;
            string auxRef = string.Empty;
            string auxRef2 = string.Empty;

            //  'Se agregan para SmartReference
            string SEmpr = string.Empty;
            string SBranch = string.Empty;

            Globales.cpIntegracion_Clear();



            if (Globales.isSmart)
            {
               // Globales.CpCobro3G.snd3GateSmartReference(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TxtReferencia.Text, numEmpr, Importe.Text);
            }
            else
            {
             //   Globales.CpCobro3G.sndGetEmpresasTGate(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TxtReferencia.Text);// ', numEmpr, Importe.Text)
            }

           // Globales.CpCobro3G.sndVentaTGate(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TypeUsuario.country, "MXN", modMIT.TGate.getRspTGCompany(), strTypeC);

            if (string.IsNullOrWhiteSpace(Globales.cpIntegracion_sResult))
            {
                Globales.cpIntegracion_sResult = Globales.CpCobro3G.getRspXML();
            }

            string selector = Globales.CpCobro3G.getRspDsResponse();

            switch (selector)
            {

                case "approved": //'Transacción Aprobada
                    {
                        cmdVoucher.IsEnabled = true;
                        cmdVoucher.Visibility = Visibility.Visible;
                        //imgMail1.Visible = TypeUsuario.enviaCorreo;
                        //LblTInfo.Visible = false;
                        //LblDenied.Visible = false;
                        //LblAprob.Visible = true;
                        //LblAuth.Caption = Globales.CpCobro3G.getRspAuth();
                        //     'falta incluir numero de activación y numero de autorizacion.
                        //   cmdActual (cmdNuevo);
                        //    PicResult.Visible = True

                        NOpe = Globales.CpCobro3G.getRspOperationNumber();
                        //     'Modificar para que solo se imprima una vez el recibo
                        VoucherTrx = Globales.CpCobro3G.getRspVoucher();
                        //     'Se agrega para la impresión de SmartReference
                        if (Globales.isSmart)
                        {
                            SEmpr = TypeUsuario.Id_Company;
                            SBranch = TypeUsuario.Id_Branch;
                            TypeUsuario.Id_Company = modMIT.TGate.getRspTGCompany();
                            TypeUsuario.Id_Branch = modMIT.TGate.getRspTGBranch();
                            Globales.isSmart = false;
                            Globales.PrintOptions(VoucherTrx, Globales.CpCobro3G.getRspOperationNumber());//, Impresora);
                            Thread.Sleep(2000);
                            Globales.isSmart = true;
                            TypeUsuario.Id_Company = SEmpr;
                            TypeUsuario.Id_Branch = SBranch;
                            modMIT.TGate.sndTGGetRecibo(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, modMIT.TGate.getRspTGCompany(), Globales.CpCobro3G.getTx_Reference(), Globales.CpCobro3G.getRspOperationNumber());
                            Globales.PrintOptions(modMIT.TGate.getRspTGRecibo(), Globales.CpCobro3G.getRspOperationNumber());//, Impresora, True
                        }
                        else
                        {
                            Globales.PrintOptions(VoucherTrx, Globales.CpCobro3G.getRspOperationNumber());//, Impresora
                            Thread.Sleep(2000);
                            Globales.PrintOptions2(Globales.CpCobro3G.getRspTicket(), Globales.CpCobro3G.getRspOperationNumber()); //Impresora, True
                        }

                    }
                    break;

                case "denied":
                    {
                        //LblAprob.Visible = False
                        //LblAuth.Visible = False
                        //LblTInfo.Visible = False

                        //LblDenied.AutoSize = True
                        //LblDenied.Caption = CpCobro3G.getRspCdResponse & " " & msjRech

                        //   LblDenied.Visible = True
                        //     cmdActual(CmdLeer);
                    }
                    break;

                case "error":
                    {
                        //LblAprob.Visible = False
                        //LblAuth.Visible = False
                        //  LblTInfo.Visible = True
                        LblTInfo.Text = Globales.CpCobro3G.getRspDsError();
                        //LblDenied.Visible = False
                        //PicResult.Visible = True
                        //cboBanco.Enabled = True
                        //FormaPago.Enabled = True



                        ///cmdActual (CmdLeer);
                    }
                    break;

                default:
                    {
                        //LblAprob.Visible = False
                        //LblAuth.Visible = False
                        //LblTInfo.Visible = True
                        LblTInfo.Text = "Verifique su conexión de Internet.";

                        cmdActual(CmdLeer);
                    }
                    break;

            }



            Globales.csvAmexenBanda = "";
            Globales.CpCobro3G.getRspXML();


            //   PicResult.Visible = True

        }

        private void cmdVoucher_Click(object sender, RoutedEventArgs e)
        {
            string SEmpr = string.Empty;
            string SBranch = string.Empty;

            if (Globales.isSmart)
            {

                SEmpr = TypeUsuario.Id_Company;
                SBranch = TypeUsuario.Id_Branch;
                TypeUsuario.Id_Company = modMIT.TGate.getRspTGCompany();
                TypeUsuario.Id_Branch = modMIT.TGate.getRspTGBranch();
                Globales.isSmart = false;

                Globales.PrintOptions(VoucherTrx, Globales.CpCobro3G.getRspOperationNumber()); // Impresora
                Thread.Sleep(2000);


                Globales.isSmart = true;
                TypeUsuario.Id_Company = SEmpr;
                TypeUsuario.Id_Branch = SBranch;
                modMIT.TGate.sndTGGetRecibo(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, modMIT.TGate.getRspTGCompany(), Globales.CpCobro3G.getTx_Reference(), Globales.CpCobro3G.getRspOperationNumber());
                //  PrintOptions TGate.getRspTGRecibo, CpCobro3G.getRspOperationNumber, Impresora, True
                Globales.PrintOptions(modMIT.TGate.getRspTGRecibo(), Globales.CpCobro3G.getRspOperationNumber());


            }
            else
            {

                Globales.PrintOptions(VoucherTrx, Globales.CpCobro3G.getRspOperationNumber()); // Impresora
                Thread.Sleep(2000);
                Globales.PrintOptions2(Globales.CpCobro3G.getRspTicket(), Globales.CpCobro3G.getRspOperationNumber());  //, Impresora, True


            }

        }


        private void Cargar()
        {
            string cport = string.Empty;
            Globales.cpIntegraEMV.dbgEndOperation();
            //Globales.msjRech = LblDenied.Content();
            if (!Globales.CpCobro3G.dbgSetReader())
            {
                
                System.Windows.Forms.MessageBox.Show("Imposible establecer comunicación con el lector.", Globales.NOMBRE_APP, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            }

            cport = Utils.ObtieneParametrosMIT("COM");

            //  Globales.ObtieneBancos( cboBanco, modMIT.TGate.getRspTGCatBancos);
            if (TypeUsuario.ShowMsg)
            {
                // frmAvisoBanda.Show vbModal
            }
            //'Mostrar mas información en pantalla
            lbInfo.Content = "Empresa Originadora: " + modMIT.TGate.getRspTGNbOriginadora();
            TxtReferencia.Text = modMIT.TGate.getRspTGReferencia();
            Importe.Text = Globales.FormatMoneda(modMIT.TGate.getRspTGImporte());


            //cboBanco.SetFocus

            Globales.CpCobro3G.dbgSetUrl(Globales.URL_3GATE);
            Globales.CpCobro3G.setTrxAmount(Importe.Text);

            if (Globales.isSmart)
            {

               // Globales.CpCobro3G.snd3GateSmartReference(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company,
               // TypeUsuario.Id_Branch, TxtReferencia.Text, numEmpr, Importe.Text);
            }
            else
            {
              //  Globales.CpCobro3G.sndGetEmpresasTGate(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, TxtReferencia.Text);// ', numEmpr, Importe.Text)
            }
            string usr = string.Empty;
            string comp = string.Empty;
            //Globales.CpCobro3G.dbgSetTrxData(Globales.CpCobro3G.getRspTGUser(), "", "", Globales.CpCobro3G.getRspTGCompany(), "", "");



        }


        private void Importe_KeyDown(object sender, KeyEventArgs e)
        {
            if (Importe.Text.Contains("."))
            {
                if (!char.IsDigit((char)e.Key))
                {
                    e.Handled = true;
                }

                if (e.Key == Key.Back)
                {
                    e.Handled = false;
                }
            }
            else
            {
                if (!char.IsDigit((char)e.Key))
                {
                    e.Handled = true;
                }

                if (e.Key == Key.OemPeriod || e.Key == Key.Back)
                {
                    e.Handled = false;
                }
            }

        }




        public void setIndexO(int Index)
        {
            IndexO = Index;
        }

        private void TxtReferencia_KeyDown(object sender, KeyEventArgs e)
        {
            char LetraPulsada = (char)e.Key;
            if (!Char.IsLetterOrDigit(LetraPulsada))
            {

                e.Handled = true;
            }
        }

        private void Importe_LostFocus(object sender, RoutedEventArgs e)
        {
            double valor = 0;
            if (double.TryParse(Importe.Text, out valor))
            {
                int aux = 0;
                if (int.TryParse(Importe.Text, out aux))
                {
                    if (aux > 0)
                    {
                        Importe.Text = aux + ".00";
                    }
                    else
                    {
                        Importe.Text = "0.00";
                    }
                }
                else
                {
                    if (valor <= 0)
                    {
                        Importe.Text = "0.00";
                    }
                    else
                    {
                        double valor1 = Math.Round(valor, 2);
                        Importe.Text = valor1.ToString();
                        if (Importe.Text.Contains(".00"))
                        {
                            Importe.Text = Importe.Text + ".00";
                        }
                    }

                }
            }
            else
            {
                Importe.Text = "0.00";
            }
        }



        public string numEmp { get; set; }
    }
}
