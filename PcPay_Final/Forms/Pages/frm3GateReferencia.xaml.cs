using PcPay.Code.Modelos;
using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// Lógica de interacción para frm3GateReferencia.xaml
    /// </summary>
    public partial class frm3GateReferencia : Page
    {
        public abrirFrm abrir;
        public frm3GateReferencia()
        {
            InitializeComponent();
            Carga();
        }

        private void Carga()
        {
            modMIT.TGate.dbgSetTGURL(modMIT.URL_3GATE);
            fraEmpresa.Visibility = Visibility.Hidden;
            TxtReferencia.GotFocus += Globales.setFocusMit2;
            TxtReferencia.LostFocus += Globales.lostFocusMit2;
        }

        private void BtnConsulta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtReferencia.Text))
                {
                    Globales.MessageBoxMit("Favor de poner una referencia");
                }
                int i = 0;
                string nbOriginadora = string.Empty;
                string cdOriginadora = string.Empty;
                //'Se agrega paravalidar la longitud de la referencia.
                if (TxtReferencia.Text.Length > 3 && TxtReferencia.Text.Length < 23)
                {

                    TxtReferencia.Text = TxtReferencia.Text.ToUpper();
                    Limpiar();
                    CmdConsultar.IsEnabled = true;
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    modMIT.TGate.dbgSetTGURL(modMIT.URL_3GATE);
                    modMIT.TGate.snd3GateReference(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company,
                    TypeUsuario.Id_Branch, TxtReferencia.Text);
                    Mouse.OverrideCursor = null;

                    if (!string.IsNullOrEmpty(modMIT.TGate.getRspTGXML()))
                    {
                        i = 1;
                        string aux = Utils.GetDataXML(Utils.GetDataXML(modMIT.TGate.getRspTGXML(), "originadora" + i), "cd_originadora");

                        while (!vacio(aux))
                        {
                            nbOriginadora = Utils.GetDataXML("nb_originadora", Utils.GetDataXML("originadora" + i, modMIT.TGate.getRspTGXML()));
                            cdOriginadora = Utils.GetDataXML("cd_originadora", Utils.GetDataXML("originadora" + i, modMIT.TGate.getRspTGXML()));

                            //frmGateSmartReference.CboEmpresas.Items.Add(nbOriginadora);
                            //frmGateSmartReference.CboEmpresas.Items[i - 1];
                            i++;

                        }
                        // frmGateSmartReference.CboEmpresas.Items.Add("OTRA");
                        //frmGateSmartReference.CboEmpresas.ItemData(i - 1) = i - 1;
                        LlenaEmpresas();
                        fraEmpresa.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        

                        Globales.MessageBoxMit("No hay información en la consulta");
                    }
                    CmdConsultar.IsEnabled = true;

                }
                else
                {
                    
                    Globales.MessageBoxMit("Referencia con longitud incorrecta");
                }





            }
            catch (Exception Err)
            {
               
                Globales.MessageBoxMit("Surgio un error: " + Err.Message);
                CmdConsultar.IsEnabled = true;
            }
        }

        private void LlenaEmpresas()
        {
            int i = 1;


            string[] lista = { "Empresa", "Importe" };
            for (int x = 0; x < lista.Length; x++)
            {
                DataGridTextColumn textoCabecera = new DataGridTextColumn();
                textoCabecera.Header = lista[x];
                textoCabecera.Binding = new Binding((lista[x]));
                MSFlexGrid1.Columns.Add(textoCabecera);
            }


            if (!vacio(Utils.GetDataXML(Utils.GetDataXML(modMIT.TGate.getRspTGXML(), "originadora" + i), "cd_originadora")))
            {
                while (!vacio(Utils.GetDataXML(Utils.GetDataXML(modMIT.TGate.getRspTGXML(), "originadora" + i), "cd_originadora")))
                {
                    MSFlexGrid1.Items.Add(new EmpresaReferencia()
                    {
                        Empresa = Globales.GetDataXml("nb_originadora", Globales.GetDataXml("originadora" + i, modMIT.TGate.getRspTGXML())),
                        Importe = Globales.GetDataXml("importe", Globales.GetDataXml("originadora" + i, modMIT.TGate.getRspTGXML()))
                    });
                    i++;
                }

            }
            else
            {

                MSFlexGrid1.Items.Add(new EmpresaReferencia() { Empresa = "OTRA", Importe = " " });
            }

        }

        private void Limpiar()
        {
            // frmGateSmartReference.CboEmpresas.Items.Clear();
        }




        private bool vacio(string cad)
        {
            return string.IsNullOrEmpty(cad);
        }

        private void TxtReferencia_TextChanged(object sender, TextChangedEventArgs e)
        {
            TxtReferencia.Text = TxtReferencia.Text.ToUpper();
            fraEmpresa.Visibility = System.Windows.Visibility.Hidden;
        }

        private void CmdAceptar_Click(object sender, RoutedEventArgs e)
        {
            int i = 6;
            
            if (this.empresa.ToUpper() == "OTRA")
            {
                frmGateSmartReference frm3GateSmartReference = new frmGateSmartReference();
                frm3GateSmartReference.lblReferencia.Content = TxtReferencia.Text;
                frm3GateSmartReference.abrir = abrir;
                abrir(frm3GateSmartReference);
            }
            else
            {


                i = 1;
                //'i = CboEmpresas.ItemData(CboEmpresas.ListIndex + 1)
                //   i = CboEmpresas.ItemData(MSFlexGrid1.RowSel)
                //   'Se explota el XML de la empresa seleccionada en el combo.
                i = MSFlexGrid1.SelectedIndex;
                modMIT.TGate.dbgSetTGXML(Globales.GetDataXml("originadora" + i, modMIT.TGate.getRspTGXML()));
                if (modMIT.TGate.getRspTGTarjeta() == "1" && modMIT.TGate.getRspTGEfectivo() == "1")
                {
                    // 'Dar a escoger la forma de pago.
                    //frm3GateFormaPago.Show
                    //CloseSkin
                    //Unload Me
                    //Exit Sub
                }
                else
                    if (modMIT.TGate.getRspTGTarjeta() == "1")
                    {
                        //'Abrir formulario de cobro con tarjeta.
                        //'verificar si se paga comisión
                        if (modMIT.TGate.getRspTGThPagaComision() == "1")
                        {
                            //  'Alerta de que paga una comisión
                            System.Windows.Forms.DialogResult msg;

                                   //MsgBox("Informar al cliente que el cargo por servicio para esta operación será de $" & TGate.getRspTGNuComision & " ¿Deseas continuar?", vbYesNo + vbQuestion, NOMBRE_APP)
                            if (!Globales.MessageConfirm("Informar al cliente que el cargo por servicio para esta operación será de $" + modMIT.TGate.getRspTGNuComision() + " ¿Deseas continuar?"))
                            {
                                //    CloseSkin
                                //    Unload Me
                                //    Exit Sub
                            }
                            else
                            {
                                
                                //    frm3GateBanda.Show
                                //    frm3GateBanda.setIndexO i
                                //    frm3GateBanda.lbInfo.Caption = frm3GateBanda.lbInfo.Caption & vbNewLine & "El cliente debe pagar una comisión de: $ " & TGate.getRspTGNuComision
                                
                            }
                        }
                        else
                        {
                            //frm3GateBanda.Show
                            //frm3GateBanda.lbInfo.Caption = frm3GateBanda.lbInfo.Caption '& vbNewLine & "El cliente debe pagar una comisión de: " & TGate.getRspTGNuComision
                        }
                    }
                    //  'Exit Sub
                    else if (modMIT.TGate.getRspTGEfectivo() == "1")
                    {
                        //'Abrir formulario de cobro en efectivo.
                        //frm3GateEfectivo.Show
                    }
                    else
                    {
                        //'No hay información.
                        Globales.MessageBoxMit("No es posible continuar, vuelva a ingresar la referencia");
                        //fraEmpresa.Visible = False
                        //'fraReferencia.Enabled = True
                        //txtReferencia.SetFocus
                        //Exit Sub
                    }
            }




        }

        private void TxtReferencia_KeyDown(object sender, KeyEventArgs e)
        {
            char LetraPulsada = (char)e.Key;
            if (!Char.IsLetterOrDigit(LetraPulsada))
            {

                e.Handled = true;
            }
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }

        private void TxtReferencia_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }

        private void escogerTransaccion(object sender, SelectionChangedEventArgs e)
        {
            DataGrid tablita = sender as DataGrid;
            EmpresaReferencia voucher = tablita.SelectedItem as EmpresaReferencia;
            empresa = voucher.Empresa;
            CmdAceptar.IsEnabled = true;
        }

        public string empresa { get; set; }
    }
}

