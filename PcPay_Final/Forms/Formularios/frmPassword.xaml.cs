using PcPay.Code.Usuario;
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

namespace PcPay.Forms.formularios
{
    /// <summary>
    /// Interaction logic for frmPassword.xaml
    /// </summary>
    public partial class frmPassword : Window
    {
        string afuera = "";
        public frmPassword()
        {
            InitializeComponent();
            afuera = string.Empty;
            txtnewpass1.GotFocus += Globales.setFocusMit2;
            txtnewpass2.GotFocus += Globales.setFocusMit2;
            txtPass.GotFocus += Globales.setFocusMit2;
            txtUser.GotFocus += Globales.setFocusMit2;

            txtnewpass1.LostFocus += Globales.lostFocusMit2;
            txtnewpass2.LostFocus += Globales.lostFocusMit2;
            txtPass.LostFocus += Globales.lostFocusMit2;
            txtUser.LostFocus += Globales.lostFocusMit2;
            this.Owner = Globales.principal;

            this.txtUser.Text = TypeUsuario.usu;
            this.txtPass.Password = TypeUsuario.Pass;
            this.txtnewpass1.Focus();
        }
        public frmPassword(string str = "afuera")
        {
            InitializeComponent();
            afuera = str;
            txtnewpass1.GotFocus += Globales.setFocusMit2;
            txtnewpass2.GotFocus += Globales.setFocusMit2;
            txtPass.GotFocus += Globales.setFocusMit2;
            txtUser.GotFocus += Globales.setFocusMit2;

            txtnewpass1.LostFocus += Globales.lostFocusMit2;
            txtnewpass2.LostFocus += Globales.lostFocusMit2;
            txtPass.LostFocus += Globales.lostFocusMit2;
            txtUser.LostFocus += Globales.lostFocusMit2;
            //this.Owner = Globales.principal;

            this.txtUser.Text = TypeUsuario.usu;
            this.txtPass.Password = TypeUsuario.Pass;
            this.txtnewpass1.Focus();
        }
        

        private void cmdCancelClick(object sender, RoutedEventArgs e)
        {
            if (desdePrincipal)
            {
                this.Close();
                return;
            }
            Globales.strNombreFP = NOMBRE_GENERAL + "cmd_cancel_click()";
            if (!TypeUsuario.bolCambiaPwd)
            {
                this.Close();
            }
            else
            {
                //System.Windows.Forms.MessageBox.Show("Debe cambiar su contraseña.\nSi no cambia la contraseña no accedera a la ventana principal del programa.", "Contraseña", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                if (afuera == "afuera")
                {
                    System.Windows.Forms.MessageBox.Show("Debe cambiar su contraseña.\nSi no cambia la contraseña no accedera a la ventana principal del programa.", "Cambio contraseña", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    goto salir;
                }
                Globales.MessageBoxMit("Debe cambiar su contraseña.\nSi no cambia la contraseña no accedera a la ventana principal del programa.");
            salir:
                System.Windows.Forms.DialogResult p = System.Windows.Forms.MessageBox.Show("¿Desea salir del programa", "Salir", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                if (p == System.Windows.Forms.DialogResult.Yes) Application.Current.Shutdown();
            }
        }

        public bool desdePrincipal { get; set; }

        public string NOMBRE_GENERAL { get; set; }

        private void cmdOkClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) && string.IsNullOrWhiteSpace(txtPass.Password) && string.IsNullOrWhiteSpace(txtnewpass1.Password) && string.IsNullOrWhiteSpace(txtnewpass2.Password))
            {
                if (string.IsNullOrWhiteSpace(txtUser.Text))
                {
                    //System.Windows.Forms.MessageBox.Show("Introduzca el usuario.", "Usuario", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (afuera == "afuera")
                    {
                        System.Windows.Forms.MessageBox.Show("Introduzca la nueva contraseña (Mínimo 8 caracteres).", "Cambio contraseña", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        goto salir;
                    }
                    Globales.MessageBoxMit("Introduzca el usuario.");
                salir:
                    txtUser.Focus();
                    return;
                }
                //if (string.IsNullOrWhiteSpace(txtPass.Text))
                //{
                //    //System.Windows.Forms.MessageBox.Show("Instroduzca la contraseña.", "Contraseña", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                //    Globales.MessageBoxMit("Instroduzca la contraseña.");
                //    txtPass.Focus();
                //    return;
                //}
                if (string.IsNullOrWhiteSpace(txtnewpass1.Password))
                {
                    //System.Windows.Forms.MessageBox.Show("Introduzca la nueva contraseña (Mínimo 8 caracteres).", "Contraseña nueva", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if(afuera == "afuera"){
                        System.Windows.Forms.MessageBox.Show("Introduzca la nueva contraseña (Mínimo 8 caracteres).","Cambio contraseña",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
                        goto salir;
                    }
                    Globales.MessageBoxMit("Introduzca la nueva contraseña (Mínimo 8 caracteres).");
                salir:
                    txtnewpass1.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtnewpass2.Password))
                {
                    //System.Windows.Forms.MessageBox.Show("Introduzca la confimación de la contraseña.", "Contraseña nueva", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (afuera == "afuera")
                    {
                        System.Windows.Forms.MessageBox.Show("Introduzca la confimación de la contraseña.", "Cambio contraseña", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        goto salir;
                    }
                    Globales.MessageBoxMit("Introduzca la confimación de la contraseña.");
                salir:
                    txtnewpass2.Focus();
                    return;
                }
            }
            else
            {

                if (txtnewpass1.Password.Length < 8)
                {
                    //System.Windows.Forms.MessageBox.Show("La longitud de la contraseña debe ser al menos 8 caracteres.", "Contraseña nueva", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (afuera == "afuera")
                    {
                        System.Windows.Forms.MessageBox.Show("La longitud de la contraseña debe ser al menos 8 caracteres.", "Cambio contraseña", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        goto salir;
                    }
                    Globales.MessageBoxMit("La longitud de la contraseña debe ser al menos 8 caracteres.");
                salir:
                    txtnewpass1.Focus();
                    txtnewpass1.Password = "";
                    txtnewpass2.Password = "";
                    return;
                }
                if (txtnewpass2.Password.ToUpper() != txtnewpass1.Password.ToUpper())
                {
                    //System.Windows.Forms.MessageBox.Show("La nueva contraseña no concide con la de confirmación.", "Contraseña nueva", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (afuera == "afuera")
                    {
                        System.Windows.Forms.MessageBox.Show("La nueva contraseña no concide con la de confirmación.", "Cambio contraseña", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        goto salir;
                    }
                    Globales.MessageBoxMit("La nueva contraseña no concide con la de confirmación.");
                salir:
                    txtnewpass1.Focus();
                    txtnewpass1.Password = "";
                    txtnewpass2.Password = "";
                    return;
                }
                string strCadEncriptar = string.Empty;
                string strCadAux = string.Empty;
                strCadEncriptar = "&user=" + txtUser.Text.ToUpper() +
                                  "&pw=" + txtPass.Password +
                                  "&newpw=" + txtnewpass1.Password +
                                  "&op=cpw" +
                                  "&version=" + TypeUsuario.strVersion;
                string peticion = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
                Globales.cpHTTP_Clear();
                Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
                Globales.cpHTTP_cadena1 = string.Format("enc={0}", Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true));
                if (Globales.cpHTTP_SendcpCUCT())
                {
                    strCadAux = Globales.GetDataXml("response", Globales.cpHTTP_sResult).ToLower();
                    if (strCadAux == "true")
                    {
                        TypeUsuario.Pass = txtnewpass1.Password;
                        if (afuera == "afuera")
                        {
                            System.Windows.Forms.MessageBox.Show(Globales.GetDataXml("desc", Globales.cpHTTP_sResult), "Cambio contraseña", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                            goto salir;
                        }
                        Globales.MessageBoxMit(Globales.GetDataXml("desc", Globales.cpHTTP_sResult));
                    salir:
                        TypeUsuario.bolCambiaPwd = false;
                        if (TypeUsuario.IsADO == "1")
                        {
                            TypeUsuario.UserApp = Globales.encryptString(TypeUsuario.usu, "KEY CREDIT CARD KEY", true);
                            TypeUsuario.PwdApp = Globales.encryptString(TypeUsuario.Pass, "KEY CREDIT CARD KEY", true);
                            Globales.SaveSettingString("AUTHOR", TypeUsuario.UserApp);
                            Globales.SaveSettingString("AUTHORID", TypeUsuario.PwdApp);
                            Globales.SaveSettingString("ISADO", "1");
                        }
                        this.Close();
                    }
                    else
                    {
                        txtUser.IsEnabled = true;
                        txtnewpass1.IsEnabled = true;
                        txtnewpass2.IsEnabled = true;
                        txtnewpass1.Password = "";
                        txtnewpass2.Password = "";
                        //System.Windows.Forms.MessageBox.Show(Globales.GetDataXml("desc", Globales.cpHTTP_sResult));
                        if (afuera == "afuera")
                        {
                            System.Windows.Forms.MessageBox.Show(Globales.GetDataXml("desc", Globales.cpHTTP_sResult), "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                            return;
                        }
                        Globales.MessageBoxMit(Globales.GetDataXml("desc", Globales.cpHTTP_sResult));
                        return;
                    }

                }


            }
        }

        private void convertirMayusculas(object sender, RoutedEventArgs e)
        {
            PasswordBox contra = sender as PasswordBox;
            contra.Password = contra.Password.ToUpper();
        }

        private void txtnewpass1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void validar(object sender, TextCompositionEventArgs e)
        {
            Regex exprecion = new Regex("^[?|&|ñ]+");
            e.Handled = exprecion.IsMatch(e.Text);
        }
    }
}
