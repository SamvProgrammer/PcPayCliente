using PcPay.Code.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PcPay.Code;
using PcPay.Code.Utilidades;
using PcPay.Code.Usuario;
using System.Windows.Controls;
using PcPay.Forms.Formularios;
using PcPay.Forms.formularios;

namespace PcPay.Forms
{
    /// <summary>
    /// Interaction logic for frmLogin.xaml
    /// </summary>
    public partial class frmLogin : Window
    {
        private const string NOMBRE_GENERAL = "FrmLogin";
        public frmLogin()
        {
            InitializeComponent();

            

            Program.ResponsiveObj = new Responsive(Program.WidthScreen, Program.HeightScreen);
            Program.ResponsiveObj.SetMultiplicationFactor();

            //this.TMENSAJE.Visibility = Visibility.Hidden;

            this.txtUser.PreviewKeyDown += this.keyPreviewKeyDown;
            this.txtPwd.PreviewKeyDown += this.keyPreviewKeyDown;
            this.GFONDO.MouseDown += this.Arrastre;
#if DEBUG
            this.CBOGIRO.SelectionChanged += this.ComboBox_SelectionChanged;
          //  this.CBOGIRO.Visibility = Visibility.Hidden;
#endif
            if (Globales.cpIntegraEMV.EsTouch())
                this.GBACEPTAR.Visibility = Visibility.Visible;
            else
                this.GBACEPTAR.Visibility = Visibility.Visible;

        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string valor = ((ComboBoxItem)this.CBOGIRO.SelectedItem).Content.ToString();
            switch (valor)
            {
                case "Retail":
                    this.txtUser.Text = "0002AMJE0";
                    this.txtPwd.Password = "TEMPORAL15";
                    break;
                case "Hotel":
                    this.txtUser.Text = "A003GOMI0";
                    this.txtPwd.Password = "TEMPORAL15";
                    break;
                case "Aeropay":
                    this.txtUser.Text = "A001GOMI0";
                    this.txtPwd.Password = "TEMPORAL30";
                    break;
                case "Travel":
                    this.txtUser.Text = "A000GOMI0";
                    this.txtPwd.Password = "TEMPORAL15";
                    break;
                case "Restaurant Prev":
                    this.txtUser.Text = "A004GOMI0";
                    this.txtPwd.Password = "TEMPORAL15";
                    break;
                case "Restaurant Cons":
                    this.txtUser.Text = "0082GOMI0";
                    this.txtPwd.Password = "TEMPORAL15";
                    break;
                case "Pago facil":
                    this.txtUser.Text = "Z745SMRU1";
                    this.txtPwd.Password = "TEMPORAL01";
                    break;
            }

            this.txtPwd.Focus();
        }
        public void frmLogin1_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                Program.ip = Globales.DecryptString(Program.ip, Globales.KEY_RC4, true);
                Program.ipPub = Globales.DecryptString(Program.ipPub, Globales.KEY_RC4, true);
                Program.ipPrep = Globales.DecryptString(Program.ipPrep, Globales.KEY_RC4, true);
                Program.ipvip = Globales.DecryptString(Program.ipvip, Globales.KEY_RC4, true);
                Program.ipPoints2 = Globales.DecryptString(Program.ipPoints2, Globales.KEY_RC4, true);
                Program.ipfe = Globales.DecryptString(Program.ipfe, Globales.KEY_RC4, true);
                Program.ipLoginInstalador = Globales.DecryptString(Program.ipLoginInstalador, Globales.KEY_RC4, true);
                Program.ipFirmaPanel = Globales.DecryptString(Program.ipFirmaPanel, Globales.KEY_RC4, true);
                Program.ipKeyWeb = Globales.DecryptString(Program.ipKeyWeb, Globales.KEY_RC4, true);


                modMIT.URLLOGIN = Globales.ip + "/pgs/pcpayLogin8";


                Program.NombreWebForm = NOMBRE_GENERAL + " - frmLogin1_Loaded";
                Utils.DeshabilitaBotonCerrar(this, false);
                Utils.DeshabilitaMenuSistema(this);
                Utils.RedimensionaForm(this);

                TypeUsuario.strVersion = Utils.GetVersionApp();
                lblVersion.Content = "Versión " + TypeUsuario.strVersion;

                txtUser.Focus();

                if (!Program.ip.Equals("https://ssl.e-pago.com.mx"))
                    this.Title = this.Title + " " + Program.ip;

                if (Program.ip.Equals("https://170.20.25.26:8080"))
                    this.Title = Program.ip + " Chinese Testing...";

                if (Utils.ObtieneParametrosMIT("Instalador").Equals("1"))
                {
                    TypeUsuario.IsADO = "1";
                    Program.cpIntegraEMV.dbgSetUrlInstalador(Program.ipLoginInstalador);
                    lblUser.Content = "Instalador:";
                }
                else
                {
                    TypeUsuario.IsADO = "-1";
                    TypeUsuario.isCDP = true;
                    lblUser.Content = "Usuario:";
                }

            }
            catch (Exception ex)
            {
               
            }

        }
        private void cmdSalir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Program.NombreWebForm = NOMBRE_GENERAL + " - cmdSalir_Click";
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                this.setErrorMessage(Program.NombreWebForm + "\r\nError: " + ex.Message);
            }


        }
        public void cmdAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Globales.saltar)
                {
                    goto salta;
                }
                Program.NombreWebForm = NOMBRE_GENERAL + " - cmdAceptar_Click";
             

                if (string.IsNullOrEmpty(txtUser.Text))
                {
                    this.setErrorMessage("Introduzca el Usuario"); 
                    return;
                }

                if (string.IsNullOrEmpty(txtPwd.Password))
                {
                    this.setErrorMessage("Introduzca la contraseña");  
                    return;
                }

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                txtUser.IsEnabled = false;
                txtPwd.IsEnabled = false;
                //cmdAceptar.IsEnabled = false;
                cmdSalir.IsEnabled = false;

                if (TypeUsuario.isCDP)
                {
                    txtUser.Text = txtUser.Text.ToUpper();
                    txtPwd.Password = txtPwd.Password.ToUpper();
                }
                 usuario = txtUser.Text.ToUpper();
                 contraseña = txtPwd.Password.ToUpper();
             salta:
                  string answerLoginUser = string.Empty;
                  setErrorMessage("");
                #region Login start
                if (modMIT.LoginUser(usuario, contraseña, out answerLoginUser))
                {
                    if (!Program.cpIntegraEMV.dbgIsUpdate())
                    {
                        if (Program.ip.Contains("dev3"))
                            modMIT.MatarProceso("Dev_PcPay.exe");
                        else if (Program.ip.Contains("qa3"))
                            modMIT.MatarProceso("QA_PcPay.exe");
                        else if (Program.ip.Contains("ssl0"))
                            modMIT.MatarProceso("QA_PcPay.exe");
                        else if (Program.ip.Contains("ssl2"))
                            modMIT.MatarProceso("QA_PcPay.exe");
                        else if (Program.ip.Contains("ssl"))
                            modMIT.MatarProceso("PcPay.exe");
                    }
                    Globales.mostrar = false;
                    TypeUsuario.TipoImpresora = "6";
                    TypeUsuario.strTipoLector = "3";
                    modMIT.Update = Utils.GetDataXML(TypeUsuario.CadenaXML, "update");
                    TypeUsuario.PromoPay = Utils.GetDataXML(TypeUsuario.CadenaXML, "promopay");
                    TypeUsuario.DCC_afiliaciones = Utils.GetDataXML(TypeUsuario.CadenaXML, "DCC_afiliaciones");

                    TypeUsuario.Points2 = Utils.GetDataXML(TypeUsuario.CadenaXML, "points2");
                    TypeUsuario.Publicidad = Utils.GetDataXML(TypeUsuario.PromoPay, "publicidad");
                    TypeUsuario.Estado = Utils.GetDataXML(TypeUsuario.PromoPay, "estado");
                    TypeUsuario.Mcc = Utils.GetDataXML(TypeUsuario.PromoPay, "mcc");

                    TypeUsuario.usu = txtUser.Text.ToUpper();
                    TypeUsuario.Pass = txtPwd.Password.ToUpper();
                    TypeUsuario.VersionPcPayActualizador = Utils.GetDataXML(TypeUsuario.CadenaXML, "versionPcPayActualizador");
                    TypeUsuario.giro = Utils.ConvierteStringToInt(Utils.GetDataXML(TypeUsuario.CadenaXML, "cd_giro"));
                    TypeUsuario.IsEmvFull = Utils.GetDataXML(TypeUsuario.CadenaXML, "emvfull");

                    modMIT.FacturaE = Utils.GetDataXML(TypeUsuario.CadenaXML, "facturaElectronica");

                    if (Utils.ConvierteStringToBoolean(Utils.GetDataXML(TypeUsuario.CadenaXML, "emvReverso")))
                    {
                        //Si se hace reverso
                        Program.cpIntegraEMV.dbgSetActivateReverse("1");
                        modMIT.NoReverso = false;
                    }
                    else
                    {
                        //En esta condición no se hace reverso
                        Program.cpIntegraEMV.dbgSetActivateReverse("0");
                        Program.cpIntegraEMV.dbgSetTimeOut("60");
                        modMIT.NoReverso = true;
                        //modMIT.CpCobro.x0309D("STRUCTUREMANAGERUPDATE_M");
                    }

                    if (Utils.ConvierteStringToBoolean(Utils.GetDataXML(TypeUsuario.CadenaXML, "st_mesa")))
                        TypeUsuario.AddTableNum = true;
                    else
                        TypeUsuario.AddTableNum = false;


                    modMIT.KEY_CP_DYNAMIC = EncryptC.EncryptRC4(Utils.GetDataXML(TypeUsuario.CadenaXML, "id_company"), Utils.GetDataXML(TypeUsuario.CadenaXML, "id_company"));

                    //******************************************************************************
                    //VALIDACION DE LOS GIROS
                    if (TypeUsuario.giro > 30)
                    {
                        Program.NOMBRE_APP = Utils.GetDataXML(TypeUsuario.CadenaXML, "nombre_app");
                        modMIT.BanImg = 17;
                    }
                    else if (Utils.GetDataXML(TypeUsuario.CadenaXML, "superpago").Substring(0, 1) == "1")
                    {
                        Program.NOMBRE_APP = "Súper Cobros";
                        modMIT.BanImg = 13;
                    }
                    else if (TypeUsuario.CadenaXML.IndexOf("<AGENCIAS>") > 0)
                    {
                        Program.NOMBRE_APP = "Travel Club";
                        modMIT.isAgencias = true;
                        TypeUsuario.iata = Utils.GetDataXML(TypeUsuario.CadenaXML, "iatarp3");
                        modMIT.intKindofImage = 2;
                        modMIT.BanImg = 1;
                    }
                    else if (TypeUsuario.CadenaXML.Contains("<EXPRESS>"))
                    {
                        Program.NOMBRE_APP = "AeroPay PC";
                        modMIT.isAgencias = false;
                        TypeUsuario.iata = Utils.GetDataXML(TypeUsuario.CadenaXML, "iata");
                        modMIT.intKindofImage = 2;
                        modMIT.BanImg = 2;
                        modMIT.isAerolinea = true;
                    }
                    else if (TypeUsuario.CadenaXML.IndexOf("<PCPAYRP3>") > 0)
                    {
                        Program.NOMBRE_APP = "PcPay";
                        modMIT.isAgencias = true;
                        TypeUsuario.iata = Utils.GetDataXML(TypeUsuario.CadenaXML, "iatarp3");
                        modMIT.intKindofImage = 1;
                        modMIT.BanImg = 3;
                    }
                    else if (Utils.GetDataXML(TypeUsuario.CadenaXML, "facileasing").Substring(0, 1) == "1")
                    {
                        Program.NOMBRE_APP = "Facileasing";
                        modMIT.intKindofImage = 3;
                        TypeUsuario.iata = Utils.GetDataXML(TypeUsuario.CadenaXML, "iata");
                        modMIT.BanImg = 4;
                    }
                    else if (Utils.GetDataXML(TypeUsuario.CadenaXML, "hotel").Substring(0, 1) == "1")
                    {
                        Program.NOMBRE_APP = "Hotel";
                        modMIT.intKindofImage = 4;
                        modMIT.BanImg = 5;

                        //Se agrega para que incluir TPV Hotel. PcPay 7.2.1 */ AGG \*
                        if (TypeUsuario.giro == 29)
                        {
                            Program.NOMBRE_APP = "TPV Santander Hotel";
                            Globales.esSantander = true;
                            modMIT.BanImg = 14;
                        }
                    }
                    else if (Utils.GetDataXML(TypeUsuario.CadenaXML, "restaurant").Substring(0, 1) == "1")
                    {
                        Program.NOMBRE_APP = "PcPay";
                        modMIT.intKindofImage = 5;
                        modMIT.BanImg = 6;

                        if (Utils.GetDataXML(TypeUsuario.CadenaXML, "facileasing").Substring(0, 1) == "1")
                        {
                            Program.NOMBRE_APP = "Facileasing";
                            modMIT.intKindofImage = 3;
                            modMIT.BanImg = 4;
                        }

                        //Se agrega para que incluir TPV Restaurante. PcPay 7.2.1 */ AGG \*
                        if (TypeUsuario.giro == 24)
                        {
                            Program.NOMBRE_APP = "TPV Santander Restaurante";
                            Globales.esSantander = true;
                            modMIT.BanImg = 16;
                        }

                    }
                    else if (Utils.GetDataXML(TypeUsuario.CadenaXML, "conectaycobra").Substring(0, 1) == "1")
                    {
                        Program.NOMBRE_APP = "TPV Santander Retail";
                        Globales.esSantander = true;
                        modMIT.BanImg = 9;
                    }
                    else if (Utils.GetDataXML(TypeUsuario.CadenaXML, "supernegocio").Substring(0, 1) == "1")
                    {
                        Program.NOMBRE_APP = "Súper Negocio";
                        modMIT.BanImg = 12;
                    }
                    else
                    {

                        if (Utils.GetDataXML(TypeUsuario.CadenaXML, "pagalaescuela").Equals("1"))
                        {
                            Program.NOMBRE_APP = "TPV Santander Escuelas";
                            Globales.esSantander = true;
                            modMIT.intKindofImage = 1;
                            modMIT.BanImg = 7;
                        }
                        else
                        {
                            Program.NOMBRE_APP = "PcPay";
                            modMIT.intKindofImage = 1;
                            modMIT.BanImg = 3;
                        }

                    }

                    //******************************************************************************

                    //******************************************************************************
                    //ACTUALIZACION DE PCPAY
                    if (modMIT.Update.ToLower().Equals("true") && (Utils.CompararVersions(TypeUsuario.VersionPcPayActualizador, Program.VersionApp)))
                    {
                       
                        if (System.Windows.Forms.DialogResult.Yes == System.Windows.Forms.MessageBox.Show("Hay una nueva actualización del programa, ¿Desea descargarlo?","Actualización",System.Windows.Forms.MessageBoxButtons.YesNo,System.Windows.Forms.MessageBoxIcon.Information))
                        {
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                            frmUpdate formActualiza = new frmUpdate();
                            formActualiza.Show(); //PENDIENTE
                            this.Close();
                            return;
                        }
                    }

                    //******************************************************************************

                    if (Utils.GetDataXML(TypeUsuario.CadenaXML, "vip").Equals("1"))
                    {
                        modMIT.URL_DLL = Program.ipvip + "/pgs/cobroXml";
                        modMIT.URL_DLL_VF = Program.ipvip + "/pgs/VentaForzadaXml";
                        modMIT.URL_DLL_RA = Program.ipvip + "/pgs/ReAutorizacionXml";
                        modMIT.URL_DLL_CANC = Program.ipvip + "/pgs/CancelacionXml";
                        modMIT.URL_DLL_CHECKIN = Program.ipvip + "/pgs/CheckInXml";
                        modMIT.URL_DLL_CHECKOUT = Program.ipvip + "/pgs/CheckOutXml";
                        modMIT.URL_DLL_PREVENTA = Program.ipvip + "/pgs/PreventaXml";
                        modMIT.URL_DLL_PROPINA = Program.ipvip + "/pgs/Propina";
                        modMIT.URL_DLL_CIERREPREVENTA = Program.ipvip + "/pgs/CierrePreventaXml";
                        modMIT.URL_REF = Program.ipvip + "/pgs/services/ActualizaRef";
                        modMIT.URL_REVERSO = Program.ipvip + "/pgs/services/Reverso";
                        modMIT.URL_REPORTECYC = Program.ipvip + "/pgs/ReportesConectayCobra";
                        modMIT.URL_GIFT = Program.ipPrep;
                        modMIT.URL_POINTS2 = Program.ipPoints2;
                        modMIT.URL_3GATE = Program.ipvip;
                        modMIT.URL_VTASERV = Program.ipvip;
                        modMIT.URL_LOGININSTALADOR = Program.ipLoginInstalador;

                        TypeUsuario.IsVIP = 1;
                        TypeUsuario.Url = Program.ipvip + "/pgs/pcpayAgencia";
                        modMIT.BanImg = 8;
                    }
                    else
                    {
                        modMIT.URL_DLL = Program.ip + "/pgs/cobroXml";
                        modMIT.URL_DLL_VF = Program.ip + "/pgs/VentaForzadaXml";
                        modMIT.URL_DLL_RA = Program.ip + "/pgs/ReAutorizacionXml";
                        modMIT.URL_DLL_CANC = Program.ip + "/pgs/CancelacionXml";
                        modMIT.URL_DLL_CHECKIN = Program.ip + "/pgs/CheckInXml";
                        modMIT.URL_DLL_CHECKOUT = Program.ip + "/pgs/CheckOutXml";
                        modMIT.URL_DLL_PREVENTA = Program.ip + "/pgs/PreventaXml";
                        modMIT.URL_DLL_PROPINA = Program.ip + "/pgs/Propina";
                        modMIT.URL_DLL_CIERREPREVENTA = Program.ip + "/pgs/CierrePreventaXml";
                        modMIT.URL_REF = Program.ip + "/pgs/services/ActualizaRef";
                        modMIT.URL_REVERSO = Program.ip + "/pgs/services/Reverso";
                        modMIT.URL_REPORTECYC = Program.ip + "/pgs/ReportesConectayCobra";
                        modMIT.URL_GIFT = Program.ipPrep;
                        modMIT.URL_POINTS2 = Program.ipPoints2;
                        modMIT.URL_3GATE = Program.ip;
                        modMIT.URL_VTASERV = Program.ip;

                        TypeUsuario.Url = Program.ip + "/pgs/pcpayAgencia";
                        TypeUsuario.IsVIP = 0;
                    }


                    //******************************************************************************
                    //PILOTO
                    string piloto = Utils.GetDataXML(TypeUsuario.CadenaXML, "piloto");

                    if (Utils.GetDataXML(piloto, "ispiloto").Equals("1"))
                    {
                        string Url = Utils.GetDataXML(piloto, "url");
                        modMIT.URL_DLL = Url + "/pgs/cobroXml";
                        modMIT.URL_DLL_VF = Url + "/pgs/VentaForzadaXml";
                        modMIT.URL_DLL_RA = Url + "/pgs/ReAutorizacionXml";
                        modMIT.URL_DLL_CANC = Url + "/pgs/CancelacionXml";
                        modMIT.URL_DLL_CHECKIN = Url + "/pgs/CheckInXml";
                        modMIT.URL_DLL_CHECKOUT = Url + "/pgs/CheckOutXml";
                        modMIT.URL_DLL_PREVENTA = Url + "/pgs/PreventaXml";
                        modMIT.URL_DLL_PROPINA = Url + "/pgs/Propina";
                        modMIT.URL_DLL_CIERREPREVENTA = Url + "/pgs/CierrePreventaXml";
                        modMIT.URL_REF = Url + "/pgs/services/ActualizaRef";
                        modMIT.URL_REVERSO = Url + "/pgs/services/Reverso";
                        modMIT.URL_REPORTECYC = Url + "/pgs/ReportesConectayCobra";
                        modMIT.URL_3GATE = Url;
                        modMIT.URL_VTASERV = Url;
                        TypeUsuario.Url = Url + "/pgs/pcpayAgencia";
                    }

                    //******************************************************************************

                    modMIT.URL_PUBLICIDAD = Program.ipPub;
                    TypeUsuario.ProdsVtaServ = Utils.GetDataXML(TypeUsuario.CadenaXML, "RESPRODUCTOS");

                    if (Utils.GetDataXML(TypeUsuario.CadenaXML, "log").Equals("1"))
                    {
                        // CrearDirectorio "Log"
                        TypeUsuario.SaveLogTransaction = true;
                        //Program.cpIntegraEMV.dbgEnabledLog(true);
                    }
                    else
                        TypeUsuario.SaveLogTransaction = false;

                    TypeUsuario.DrpUrl = Utils.GetDataXML(TypeUsuario.CadenaXML, "ipsconectividad");

                    string AuxGCMerchant;
                    AuxGCMerchant = Utils.GetDataXML(TypeUsuario.CadenaXML, "tipopagobSIP");
                    modMIT.MerchantGC = Utils.GetDataXML(AuxGCMerchant, "merchant");


                    if (!string.IsNullOrEmpty(Utils.GetDataXML(TypeUsuario.CadenaXML, "rentaautos")) &&
                        Utils.GetDataXML(TypeUsuario.CadenaXML, "rentaautos").Substring(0, 1).Equals("1"))
                    {
                        Program.NOMBRE_APP = "Renta Autos";
                        modMIT.BanImg = 11;

                        if (TypeUsuario.giro == 30)
                        {
                            Program.NOMBRE_APP = "TPV Santander Car Rental";
                            Globales.esSantander = true;
                            modMIT.BanImg = 15;
                        }
                    }

                    if (!string.IsNullOrEmpty(Utils.GetDataXML(Utils.GetDataXML(TypeUsuario.CadenaXML, "menu6"), "option")) &&
                        Utils.GetDataXML(Utils.GetDataXML(TypeUsuario.CadenaXML, "menu6"), "option").Substring(0, 1).Equals("1"))
                    {
                        Program.NOMBRE_APP = "PcPay";
                        modMIT.BanImg = 10;
                    }

                    modMIT.MsjReferencia = "Ingresa el dato " + TypeUsuario.reference + ", este dato es para llevar un mejor control interno de las transacciones realizadas, puede ser: matricula, contacto, número de cliente, número de folio, etc.";
                    modMIT.MsjImporte = "Indica el Importe a cobrar";
                    modMIT.Msjtarjeta = "Ingresa el número de la tarjeta bancaria.";
                    modMIT.MsjMes = "Selecciona el mes de expiración de la tarjeta.";
                    modMIT.MsjAnyo = "Selecciona el año de expiración de la tarjeta.";
                    modMIT.MsjNombre = "Ingresa el nombre del Tarjetahabiente.";
                    modMIT.MsjCvv = "Ingresa el código de seguridad, para V/MC son los últimos tres dígitos que se encuentran en el adverso de la tarjeta y para AMEX son los cuatro dígitos que se encuentran en el frente de la tarjeta.";

                    

                    if (Utils.GetDataXML(TypeUsuario.CadenaXML, "stlogin").Equals("1") || Utils.ObtieneParametrosMIT("Instalador").Equals("1"))
                    {
                        TypeUsuario.UserApp = EncryptC.EncryptRC4(TypeUsuario.usu, modMIT.KEY_RC4_CP);
                        TypeUsuario.PwdApp = EncryptC.EncryptRC4(TypeUsuario.Pass, modMIT.KEY_RC4_CP);
                        Utils.GuardaParametrosMIT("AUTHOR", TypeUsuario.UserApp);
                        Utils.GuardaParametrosMIT("AUTHORID", TypeUsuario.PwdApp);
                        Utils.GuardaParametrosMIT("ISADO", "1");
                        TypeUsuario.IsADO = "1";
                    }
                    else
                    {
                        
                        TypeUsuario.UserApp = "";
                        TypeUsuario.PwdApp = "";
                        Utils.GuardaParametrosMIT("AUTHOR", "");
                        Utils.GuardaParametrosMIT("AUTHORID", "");
                        Utils.GuardaParametrosMIT("ISADO", "");
                        TypeUsuario.IsADO = "";
                    }

                    //**********************************************************************************
                    //Valida usuario qualitas
                    Program.cpIntegraEMV.dbgGetIsUserQualitas(false);
                    Program.cpIntegraEMV.dbgSetQualitasActivaMSI(false);
                    if (Utils.GetDataXML(TypeUsuario.CadenaXML, "id_company").ToUpper().Equals(typeUsuarioQualitas.userQualitas))
                    {
                        Globales.isQualitas = true;
                        Globales.isQualitasCierraForm = true;
                        Program.cpIntegraEMV.dbgGetIsUserQualitas(true);

                        if (typeUsuarioQualitas.sucursalQualitas.Contains("-" + Utils.GetDataXML(TypeUsuario.CadenaXML, "id_branch") + "-"))
                            Program.cpIntegraEMV.dbgSetQualitasActivaMSI(true);
                    }

                    //**********************************************************************************
                    //USUARIO CON OPCION DE TOKENIZACION
                    modMIT.userTokenizacion = false;
                    if (Utils.GetDataXML(TypeUsuario.CadenaXML, "st_tokenizacion").Equals("1"))
                        modMIT.userTokenizacion = true;

                    //**********************************************************************************
                    //Usuario de Recompensans Santander
                    if (Utils.GetDataXML(TypeUsuario.CadenaXML, "id_company").ToUpper().Equals(modMIT.userSantanderVta))
                        TypeUsuario.TipoImpresora = "1";

                    //**********************************************************************************
                    //PAGO FACIL
                    modMIT.esPagoFacil = Utils.ConvierteStringToBoolean(Utils.GetDataXML(TypeUsuario.CadenaXML, "pagofacil"));
                    modMIT.IdAdquiriente = Utils.GetDataXML(TypeUsuario.CadenaXML, "cd_adquiriente");
                    TypeUsuario.VersionPcPay = Utils.GetDataXML(TypeUsuario.CadenaXML, "versionPcPayActualizador");
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    TypeUsuario.bolCambiaPwd = (Globales.GetDataXml("cambiaPwd", TypeUsuario.CadenaXML).ToLower() == "false") ? false : true;
                    if (TypeUsuario.bolCambiaPwd)
                    {
                        System.Windows.Forms.MessageBox.Show(Globales.GetDataXml("cambiaPwdDesc", TypeUsuario.CadenaXML), "Cambio contraseña", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                        Close();
                        Mouse.OverrideCursor = null;
                        frmPassword contra = new frmPassword("afuera");
                        contra.desdePrincipal = false;
                        contra.ShowDialog();
                    }
                    menuPrincipal= new frmMenuPrincipal();
                    menuPrincipal.login = login;
                    menuPrincipal.Show();

                    if (Utils.GetDataXML(TypeUsuario.CadenaXML, "tipmsg") != "")
                    {
                        TypeUsuario.IsTip = true;
                        TypeUsuario.TipMsg = Utils.GetDataXML(TypeUsuario.CadenaXML, "tipmsg");

                        //Aqui validar si es http, es una imagen externa y mostrarla sino continuar con la misma operativa
                        if (TypeUsuario.TipMsg.Substring(0, 4).Equals("http"))
                        {
                            //Aqui pintar la imagen en HTA
                            //Listo para mandar funcion que ejecute la imagen
                            //Call ImprimirImgCyc //pendiente
                            //MDImit.ImgTip.Visible = true;  //pendiente
                        }
                        else
                        {
                            //MDImit.ImgTip.Visible = true; //pendiente
                            //frmTip.Show vbModal  //pendiente
                            frmTip tip = new frmTip();
                            tip.lblTip.Text = TypeUsuario.TipMsg;
                            tip.ShowDialog();
                        }
                    }
                    else
                    {
                        TypeUsuario.IsTip = false;
                        TypeUsuario.TipMsg = "";
                    }

                 //  new frmUpdate().Show();
                    this.Close();
                    Globales.logeo = true;
                    //this.setErrorMessage("Iniciar el menu principal...");
                }
                else
                {
                    Globales.answerLoginUser = answerLoginUser;

                    Mouse.OverrideCursor = null;
                    txtUser.IsEnabled = true;
                    txtPwd.IsEnabled = true;

                    cmdSalir.IsEnabled = true;
                    txtUser.Text = "";
                    txtUser.Focus();
                    txtPwd.Password = "";
                    Globales.mostrar = true;

                    Globales.logeo = false;

                    if (!TypeUsuario.IsADO.Equals("1"))
                        txtUser.Focus();

                    if (!string.IsNullOrWhiteSpace(answerLoginUser))
                        this.setErrorMessage(answerLoginUser);
                    else
                    {
                        if (string.IsNullOrWhiteSpace(TypeUsuario.CadenaXML))
                            this.setErrorMessage("No hay conexión a internet, verifique permisos a:" + "\r\n" + Program.ip);
                        else if (TypeUsuario.CadenaXML.IndexOf("Administrador") > 0 && !Utils.GetDataXML(TypeUsuario.CadenaXML, "url_auto_desbloq").Equals(""))
                        {
                            string contentenido = Globales.GetDataXml("url_auto_desbloq", TypeUsuario.CadenaXML);
                            //pendiente
                            Mouse.OverrideCursor = null;
                            new frmBloqueo().ShowDialog();
                        }
                        else if (!Utils.GetDataXML(TypeUsuario.CadenaXML, "error").Equals(""))
                        {
                            this.setErrorMessage(Utils.GetDataXML(TypeUsuario.CadenaXML, "error"));
                        }
                        else
                            this.setErrorMessage("Temporalmente fuera de servicio. \r\nInténtelo de nuevo más tarde");
                    }


                    if (TypeUsuario.IsADO.Equals("1"))
                        if (Utils.GetDataXML(TypeUsuario.CadenaXML, "cd_error").Equals("1") || Utils.GetDataXML(TypeUsuario.CadenaXML, "cd_error").Equals("2"))
                            this.Show();
                        else
                        {
                            if (!TypeUsuario.UserApp.Equals(""))
                                this.Close();
                            else
                                return;
                        }
                }

#endregion

            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                this.setErrorMessage(Program.NombreWebForm + "\r\nError: " + ex.Message);

                txtUser.IsEnabled = true;
                txtPwd.IsEnabled = true;

                cmdSalir.IsEnabled = true;
                txtUser.Text = "";
                txtPwd.Password = "";

                // txtUser.Focus();

            }
            Mouse.OverrideCursor = null;
        }
        private void txtUser_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Program.NombreWebForm = NOMBRE_GENERAL + " - txtUser_LostFocus";

                if (TypeUsuario.isCDP)
                    txtUser.Text = txtUser.Text.ToUpper();

            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                this.setErrorMessage(Program.NombreWebForm + "\r\nError: " + ex.Message);
            }
        }
        private void txtPwd_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Program.NombreWebForm = NOMBRE_GENERAL + " - txtPwd_LostFocus";

                if (TypeUsuario.isCDP)
                    txtPwd.Password = txtPwd.Password;

            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                this.setErrorMessage(Program.NombreWebForm + "\r\nError: " + ex.Message);
            }
        }
        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
           

        }
        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cmdAceptar_Click(null, null);
        }
        private void frmLogin1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.cmdSalir.PerformClick();
        }
        private void keyPreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.hideErrorMessage();
        }
        private void setErrorMessage(string message)
        {
            this.TMENSAJE.Text = message;
        }
        private void hideErrorMessage()
        {
          //  this.TMENSAJE.Text = string.Empty;
            //this.TMENSAJE.Visibility = Visibility.Hidden;
        }
        private void Arrastre(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                Application.Current.Shutdown();
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.cmdAceptar_Click(null, null);
        }


        public void login(string usuario,string contraseña) {
            Globales.saltar = true;
            this.usuario = usuario;
            this.contraseña = contraseña;
            this.cmdAceptar_Click(null,null);
        }

        private void txtUser_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Globales.soloTNumeroTexto(sender,e);
        }

        public frmMenuPrincipal menuPrincipal { get; set; }

        

        public string usuario { get; set; }

        public string contraseña { get; set; }
    }
}
