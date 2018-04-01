using PcPay.Code;
using PcPay.Code.Configuracion;
using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using PcPay.Forms.formularios;
using PcPay.Forms.Formularios;
using PcPay.Forms.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
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

namespace PcPay.Forms
{
    #region Métodos Delegados
    public delegate void cerrarVentana();
    public delegate void abrirFrm(Page pagina, string titulo = "");
    public delegate void menu(string valor);
    public delegate void menu2();
    public delegate void abrir();
    public delegate void logearse(string usuario, string contraseña);
    #endregion
    /// <summary>
    /// Lógica de interacción para frmMenuPrincipal.xaml
    /// </summary>
    public partial class frmMenuPrincipal : Window
    {
        string[] iconosMenuOK = new string[] {"ventaspropias","transacciones","administracion","cupones",
            "ventaservicios","e-factura","tokenizacion","ayuda","salir","ventadeboletos","ventasmiscelaneos"};


        private const string NOMBRE_GENERAL = "frmMenuPrincipal";
        private string sPathUserPicture;
        private string pathFile;
        private string xmlPicture;
        private List<MenuItem> listaMenu = new List<MenuItem>();
        public List<Grid> menuList { get; set; }
        public int indexInicialMenu { get; set; }
        public bool lectorConectado { get; set; }
        public logearse login { get; set; }
        #region"Variables y formularios"
        private frmMotoAgencia frmMotoAgencia { get; set; }
        private frmBandaEMV frmBandaEMV { get; set; }
        private frmImpresion frmImpresion { get; set; }
        private frmCancelacion frmCancelacion { get; set; }
        private frmAVSs2 frmAVSs2 { get; set; }
        #endregion
        public frmMenuPrincipal()
        {
            InitializeComponent();
            Program.ResponsiveObj = new Responsive(Program.WidthScreen, Program.HeightScreen);
            Program.ResponsiveObj.SetMultiplicationFactor();
            Globales.principal = this;

            this.SPMENUOPCIONES.Children.Clear();
            this.SPSUBMENUS.Width = 0;
            this.GMODAL.Visibility = Visibility.Hidden;


            


            this.ventanaDisplay.Navigated += navegacion;
            this.indexInicialMenu = 0;           
            this.cargandoMenuPrincipal();
            if (this.menuList.Count > 7)
            {
                this.BADELANTE.MouseDown += adelantarAtrazar;
                this.BATRAS.MouseDown += adelantarAtrazar;
            }
            else
            {
                this.BADELANTE.Visibility = Visibility.Hidden;
                this.BATRAS.Visibility = Visibility.Hidden;
            }
        }
        internal void cargandoMenuPrincipal()
        {
            try
            {
                

                //this.CreaCarpeta();
                this.creaCarpetVersion10();


                string actualizarImages = Globales.GetSettingString("IMAGENES");

                Program.NombreWebForm = NOMBRE_GENERAL + " - frmMenuPrincipal1_Loaded";
                Utils.RedimensionaForm(this);

                this.Title = Program.NOMBRE_APP;
                if (!Program.ip.Equals("https://ssl.e-pago.com.mx"))
                    this.Title = this.Title + " " + modMIT.URL_3GATE;

                if (Program.ip.Equals("https://170.20.25.26:8080"))
                    this.Title = Program.ip + " Chinese Testing...";

                this.Title += (TypeUsuario.IsVIP == 1) ? " - VIP" : "";
                string versionAplicacion = Globales.GetVersionApp();
                this.imageSelectV2();
                Dictionary<string, List<MenuOpciones>> obtenerMenu = Globales.getMenu();
                //this.setMenu(obtenerMenu);
                ////this.setMenuV2(obtenerMenu);
                if (Globales.desvinculaInstaladorLector)
                {
                    goto noValidaLector;
                }


                    //      goto saltar;
                    string MarcaEMV = Globales.cpIntegraEMV.chkPp_Trademark();
                    if (string.IsNullOrWhiteSpace(MarcaEMV))
                    {
                        Mouse.OverrideCursor = null;
                        lblLector.Visibility = Visibility.Hidden;
                        this.lectorConectado = false;
                    }
                    else
                    {
                        this.lectorConectado = true;

                        lblLector.Visibility = Visibility.Hidden;
                        TypeUsuario.SerialReader = Globales.cpIntegraEMV.chkPp_Serial();
                        Globales.SaveSettingString("SERIE", TypeUsuario.SerialReader);
                        //Menu corte por usuario
                        foreach (var item in obtenerMenu)
                        {
                            foreach (var item2 in item.Value)
                            {
                                if (item2.id == "menu2CorteporUsuario")
                                {
                                    if(Globales.cpIntegraEMV.chkPp_Printer() != "1")
                                        item2.activo = false;
                                    goto salir;
                                }
                            }
                        }

                    salir:
                        string COM, i;
                        COM = Globales.GetSettingString("COM");
                        TypeUsuario.IsAQ = false;
                        //SE VALIDA EL TIP DE LECTOR DE ACUERDO A LA IMPRESORA
                        TypeUsuario.strTipoLector = "2";
                        if (Globales.cpIntegraEMV.chkPp_Printer() == "1")
                            TypeUsuario.strTipoLector = "3";

                        //SE AGREGA PARA TRAER LA IMPRESORA QUE TIENE CONFIGURADA POR DEFAUL
                        TypeUsuario.TipoImpresora = Globales.GetSettingString("PRINTERNUMBER");
                        Globales.EMVLector = "1";
                        Globales.SaveSettingString("COM", COM);
                        Globales.SaveSettingString("EMV", Globales.EMVLector);
                        Globales.SaveSettingString("TYPE", TypeUsuario.strTipoLector);

                    }
                
                //saltar:
                this.setMenuV3(obtenerMenu);
                Globales.setConfiguracion(TypeUsuario.CadenaXML);

                //*********************************************************************************************
                //SE ACTUALIZA EL LOGO INFERIOR
                BitmapImage imgAdquieriente = null;
                string sPathUserPicture = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MIT\\Data\\Images10\\adquiriente");
                string adquirientePath = string.Format("{0}\\{1}.png", sPathUserPicture, modMIT.IdAdquiriente);
                string adquirientePathDefault = string.Format("{0}\\default.png", sPathUserPicture);
                if(!Globales.sinImagen){
                   
                if (File.Exists(adquirientePath) )
                    imgAdquieriente = new BitmapImage(new Uri(adquirientePath));
                else
                    imgAdquieriente = new BitmapImage(new Uri(adquirientePathDefault));
                }

                this.IADQUIRIENTE.Source = imgAdquieriente;


                //*********************************************************************************************
                var a = TypeUsuario.Id_Company;
                Globales.setConfiguracion(TypeUsuario.CadenaXML);
                a = TypeUsuario.Id_Company;
            //FrmBanner.show
            // if (Globales.desvincularInstaladorLector) goto noValidaLector;

               noValidaLector:
                if (!(Globales.isCDP))
                {
                    //Desvincular.visible = true;
                    //Desvincular.Caption = "desligar"
                }
                var x = TypeUsuario.Id_Company;
                string xml = TypeUsuario.CadenaXML;

                this.LUSER.Content = TypeUsuario.usu;
                this.LNAME.Content = Globales.GetDataXml("nb_user", xml);


                this.LEMPRESA.Content = "EMP: " + Globales.GetDataXml("nb_company", xml);
                this.LSUCURSAL.Content = "SUC: " + TypeUsuario.Id_Branch + " / " + TypeUsuario.nb_branch;
                if (Globales.GetSettingString("stlogin") == "1")
                {
                    //Menu4_4 = visible            
                }

                string version = string.Empty;
                version ="PcPay "+ Globales.GetVersionApp();
                Globales.SaveSettingString("VERSION", version);
                //Se solicita cambio de contraseña


                string cd_adquiriente = Globales.GetDataXml("cd_adquiriente", TypeUsuario.CadenaXML);
                //this.IADQUIRIENTE.Visibility=(cd_adquiriente.Equals("003")||cd_adquiriente.Equals(""))?Visibility.Visible:Visibility.Hidden;
                if (modMIT.esPagoFacil)
                {
                    string folderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MIT\\Data\\Images10\\giro");
                    string archivo = string.Format("{0}\\pago_facil.png", folderPath);
                    BitmapImage image = new BitmapImage(new Uri(archivo));

                    this.ILOGO.Source = image;
                }
            }
            catch (InvalidCastException ex)
            {

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                Utils.GuardaParametrosMIT("COM", "0");
                Utils.GuardaParametrosMIT("TYPE", "0");
                TypeUsuario.strTipoLector = "0";
                System.Windows.Forms.MessageBox.Show(Program.NombreWebForm + "\r\nError: " + ex.Message, Program.NOMBRE_APP);
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                MessageBox.Show(Program.NombreWebForm + "\r\nError: " + ex.Message, Program.NOMBRE_APP);
            }

            TypeUsuario.TipoImpresora = Globales.GetSettingString("PRINTERNUMBER");
            if (string.IsNullOrWhiteSpace(TypeUsuario.TipoImpresora))
            {
                Globales.SaveSettingString("PRINTERNUMBER", "4");
                TypeUsuario.TipoImpresora = "4";
            }

        }
        #region "Métodos"
        private void creaCarpetVersion10()
        {
            try
            {
                string folderPath = string.Format("{0}\\MIT\\Data\\Images10", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

                if (!System.IO.Directory.Exists(folderPath))
                    System.IO.Directory.CreateDirectory(folderPath);
                //********************************************************************************************************
                //********************************************************************************************************
                //*********************Cambiar Properties.Resources.DevTempURL a ======> modMIT.URL_3GATE***************** 
                //********************************************************************************************************
                //********************************************************************************************************
                string urlTemp = string.Format("{0}/{1}/{2}", Globales.ip, Properties.Resources.remoteImg, "version.txt");
                string urlZipGiro = string.Format("{0}/{1}/{2}", Globales.ip, Properties.Resources.remoteImg, "giro.zip");
                string urlZipAdquiriente = string.Format("{0}/{1}/{2}", Globales.ip, Properties.Resources.remoteImg, "adquiriente.zip");

                int versionImgLocal = int.Parse(string.IsNullOrWhiteSpace(Globales.GetSettingString("IMAGESv10")) ? "0" : Globales.GetSettingString("IMAGESv10"));
                int remoteImgVersion = int.Parse(Utils.readVersionImages(urlTemp));

                if (remoteImgVersion > versionImgLocal)
                {
                    Globales.SaveSettingString("IMAGESv10", remoteImgVersion.ToString());
                    string zipGiros = Utils.downloadImgV10(urlZipGiro, "giros");
                    string zipAdquirientes = Utils.downloadImgV10(urlZipAdquiriente, "adquiriente");
                    DirectoryInfo[] carpetas = new DirectoryInfo(folderPath).GetDirectories();
                    foreach (DirectoryInfo carpeta in carpetas)
                    {
                        FileInfo[] archivos = carpeta.GetFiles();
                        foreach (FileInfo archivo in archivos)
                        {
                            archivo.Delete();
                        }
                        carpeta.Delete();
                    }



                    Utils.unzipFile(zipGiros, folderPath);
                    Utils.unzipFile(zipAdquirientes, string.Format("{0}\\adquiriente", folderPath));
                }
                else
                {
                    if (!System.IO.Directory.Exists(System.IO.Path.Combine(folderPath, "giro")))
                    {
                        string zipGiros = Utils.downloadImgV10(urlZipGiro, "giros");
                        Utils.unzipFile(zipGiros, folderPath);
                    }
                    if (!System.IO.Directory.Exists(System.IO.Path.Combine(folderPath, "adquiriente")))
                    {
                        string zipAdquirientes = Utils.downloadImgV10(urlZipAdquiriente, "adquiriente");
                        Utils.unzipFile(zipAdquirientes, System.IO.Path.Combine(folderPath, "adquiriente"));
                    }
                }
            }
            catch {
                Globales.sinImagen = true;
            }
        }
        private void CreaCarpeta()
        {
            try
            {
                sPathUserPicture = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\MIT\\Data\\Picture";
                if (!System.IO.Directory.Exists(sPathUserPicture))
                    System.IO.Directory.CreateDirectory(sPathUserPicture);

                xmlPicture = "";
                xmlPicture = this.ConsultaXML(modMIT.URL_3GATE, "xmlPicture.jsp");

                if (Utils.CompararVersions(Program.VersionApp, Utils.GetDataXML(xmlPicture, "versionPcPay")))
                {

                    if (Utils.ObtieneParametrosMIT("VerPicture").Equals(""))
                        Utils.GuardaParametrosMIT("VerPicture", "0");
                    //Descarga el archivo zip en caso de que la versión sea mayor
                    if (Utils.ConvierteStringToInt(Utils.ObtieneParametrosMIT("VerPicture")) < Utils.ConvierteStringToInt(Utils.GetDataXML(xmlPicture, "version")))
                    {
                        this.Download(Utils.GetDataXML(xmlPicture, "path"), Utils.GetDataXML(xmlPicture, "name"), "Picture");
                        this.Unzip(sPathUserPicture + "\\" + Utils.GetDataXML(xmlPicture, "name"), sPathUserPicture);
                        Utils.GuardaParametrosMIT("VerPicture", Utils.GetDataXML(xmlPicture, "version"));
                    }
                }

                //********************************************************************************************
                //DESCARGA DE LA CARPETA CON LAS NUEVAS IMAGENES DE PAGO FACIL
                sPathUserPicture = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\MIT\\Data\\images";

                if (!System.IO.Directory.Exists(sPathUserPicture))
                    System.IO.Directory.CreateDirectory(sPathUserPicture);

                string xmlPagoFacil;
                xmlPagoFacil = Utils.GetDataXML(xmlPicture, "pagofacil");

                if (Utils.CompararVersions(Program.VersionApp, Utils.GetDataXML(xmlPagoFacil, "versionPcPay")))
                {
                    if (Utils.ObtieneParametrosMIT("SkinPagoFacil").Equals(""))
                        Utils.GuardaParametrosMIT("SkinPagoFacil", "0");

                    if (Utils.ConvierteStringToInt(Utils.ObtieneParametrosMIT("SkinPagoFacil")) < Utils.ConvierteStringToInt(Utils.GetDataXML(xmlPagoFacil, "version")))
                    {
                        this.Download(Utils.GetDataXML(xmlPagoFacil, "path"), Utils.GetDataXML(xmlPagoFacil, "name"), "images");
                        this.Unzip(sPathUserPicture + "\\" + Utils.GetDataXML(xmlPagoFacil, "name"), sPathUserPicture);
                        Utils.GuardaParametrosMIT("SkinPagoFacil", Utils.GetDataXML(xmlPagoFacil, "version"));
                    }
                }


            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                MessageBox.Show(NOMBRE_GENERAL + " CreaCarpeta" + "\r\nError: " + ex.Message, Program.NOMBRE_APP);
            }
        }
        private string ConsultaXML(string url, string jspName, int op = 0)
        {
            string resultado = "";
            string strDataToSend;
            string urlWS;

            strDataToSend = "op=" + op;

            if (!jspName.Equals(""))
                urlWS = url + "/pgs/jsp/cpagos/" + jspName;
            else
                urlWS = url + "/pgs/EMVKeysService";

            resultado = Utils.SendWS(urlWS, strDataToSend);
            resultado = resultado.Replace("\r\n", "");

            return resultado;
        }
        private void Download(string pathXML, string nameFile, string nombreCarpeta)
        {
            string url = modMIT.URL_3GATE + pathXML;

            if (nameFile.Equals(""))
                nameFile = "name";

            sPathUserPicture = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\MIT\\Data\\" + nombreCarpeta;

            try
            {
                WebClient webClient = new WebClient();

                if (!Directory.Exists(sPathUserPicture))
                    Directory.CreateDirectory(sPathUserPicture);
                webClient.DownloadFile(url, sPathUserPicture + "\\" + nameFile);

            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                MessageBox.Show(NOMBRE_GENERAL + " Download" + "\r\nError: " + ex.Message, Program.NOMBRE_APP);
            }
        }
        private void imageSelect()
        {
            //Seleccionar imagenes
            int imagen = Globales.BanImg;
            string rutaUsuario = Globales.getRutaUsuario();
            string rutaImagen = rutaUsuario + @"\MIT\Data\Picture";
            var archivos = Directory.GetFiles(rutaImagen);
            foreach (var item in archivos)
            {
                if (item == (rutaImagen + "\\" + imagen + ".jpg"))
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri(item);
                    bi3.EndInit();
                    //this.IMGGIRO.Stretch = Stretch.None;
                    //this.IMGGIRO.Source = bi3;
                    break;
                }
            }
        }
        private void imageSelectV2()
        {


            xmlPicture = this.ConsultaXML(modMIT.URL_3GATE, "xmlPicture.jsp");

            if (Globales.sinImagen) return;
            BitmapImage image = null;
            BitmapImage image2 = null;
            string folderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MIT\\Data\\Images10\\giro");
            var archivos = Directory.GetFiles(folderPath).ToList();
            string archivo = string.Format("{0}\\{1}", folderPath, Globales.BanImg == 16 ? 18 : Globales.BanImg);

            if(Globales.BanImg == 17){
                if (string.IsNullOrWhiteSpace(Globales.GetDataXml("case" + TypeUsuario.giro, xmlPicture)))
                {
                    return;
                }
            }

            if(Globales.esSantander){
                try
                {
                    string archivoSantnader = string.Format("{0}\\{1}", folderPath, "000.png");
                    image2 = new BitmapImage(new Uri(archivoSantnader));
                    santander.Source = image2;
                }
                catch { 
                
                }
            }

            try
            {
                if (archivos.Exists(o => o == archivo + ".PNG"))
                {
                    image = new BitmapImage(new Uri(archivo + ".PNG"));
                }
                else if (archivos.Exists(o => o == archivo + ".png"))
                {
                    image = new BitmapImage(new Uri(archivo + ".png"));
                }
                else
                    image = new BitmapImage(new Uri(string.Format("{0}\\{1}.png", folderPath, 10)));

                this.ILOGO.Source = image;
            }
            catch { 
            
            }
        }
        private void setMenu(Dictionary<string, List<MenuOpciones>> obtenerMenu)
        {
            foreach (var item in obtenerMenu)
            {
                MenuItem menu = new MenuItem();
                MenuItem submenu = new MenuItem();
                menu.Header = item.Key;
                foreach (var item2 in item.Value)
                {
                    if (item2.activo)
                    {
                        submenu = new MenuItem();
                        submenu.Header = item2.nombre;
                        submenu.Name = item2.id;
                        //submenu.Click += new System.Windows.RoutedEventHandler(this.eventoMenu);
                        menu.Items.Add(submenu);
                    }
                }
                //menuPrincipal.Items.Add(menu);
            }
        }
        private void setMenuV2(Dictionary<string, List<MenuOpciones>> menuOptions)
        {
            foreach (var item in menuOptions)
            {
                Button boton = new Button();
                boton.Content = item.Key;
                string nombre = item.Key.Replace("-", "_");
                boton.Name = nombre.Replace(" ", "_");

                boton.Tag = item.Value;
                boton.Background = new SolidColorBrush(Color.FromRgb(12, 67, 117));
                boton.Foreground = Brushes.White;
                boton.Width = 60;
                boton.Height = 60;
                boton.Margin = new Thickness(0, 0, 5, 0);
                boton.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                boton.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                boton.Cursor = Cursors.Hand;
                boton.ToolTip = item.Key;
                boton.Focusable = false;
                boton.Click += setSubmenus;
                if (item.Key.ToUpper() != "SALIR")
                    this.SPMENUOPCIONES.Children.Add(boton);
            }
        }
        private void setMenuV3(Dictionary<string, List<MenuOpciones>> menuOptions)
        {
            List<string> auxlist = this.iconosMenuOK.ToList();

            this.menuList = new List<Grid>();
            Style gridStyle = this.FindResource("gridMenuMit") as Style;
            Style imgStyle = this.FindResource("imgMenuMit") as Style;
            Style textStyle = this.FindResource("textMenuMit") as Style;

            System.Windows.Media.Effects.DropShadowEffect dropShadowEffect = new System.Windows.Media.Effects.DropShadowEffect();
            dropShadowEffect.Opacity = 1;
            dropShadowEffect.ShadowDepth = 5;
            dropShadowEffect.BlurRadius = 5;
            dropShadowEffect.Color = Colors.Black;
            dropShadowEffect.Direction = 315;

            foreach (var item in menuOptions)
            {
               
                Grid grid = new Grid();
                Image image = new Image();
                TextBlock text = new TextBlock();
                grid.Style = gridStyle;
                image.Style = imgStyle;
                text.Style = textStyle;

                text.Text = item.Key;
                string nombre = item.Key.Replace("-", "_");
                grid.Name = nombre.Replace(" ", "_");
                grid.Tag = item.Value;

                string name = item.Key.Replace("ó", "o");
                name = name.Replace("á", "a");
                name = name.Replace(" ", "").ToLower();

                if (auxlist.Exists(o => o == name) && !Globales.sinImagen)
                {
                    BitmapImage bitmap = Utils.CargaImagen(string.Format("/Resources/menuIcons/{0}.png", name), true);
                    image.Source = bitmap;
                }

                grid.Children.Add(image);
                grid.Children.Add(text);
               
                //grid.Effect = dropShadowEffect;
                if (item.Key == "Token")
                {
                    grid.MouseDown += new MouseButtonEventHandler(funciona);

                }
                else {
                    grid.MouseDown += Grid_MouseDown;
                }

                if (item.Key.ToUpper() != "SALIR")
                    this.menuList.Add(grid);
                //this.SPMENUOPCIONES.Children.Add(grid);
            }

            for (int i = 0; i < this.menuList.Count; i++)
            {
                if (i <= 6)
                    this.SPMENUOPCIONES.Children.Add(this.menuList[i]);
            }
        }

        private void funciona(object sender, MouseButtonEventArgs e)
        {
            MenuOpciones menu = new MenuOpciones();
            menu.id = "tokenToken";
            eventoMenu(menu);
        }


        private void Unzip(string pathFile, string pathExtract)
        {

            try
            {
                if (!Directory.Exists(pathExtract))
                    Directory.CreateDirectory(pathExtract);

                ZipFile.ExtractToDirectory(pathFile, pathExtract);

            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                MessageBox.Show(NOMBRE_GENERAL + " Unzip" + "\r\nError: " + ex.Message, Program.NOMBRE_APP);
            }
        }
        private void cambiaContraseña(bool opcion)
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "-mnu4_1_Click()";
            if (Globales.isQualitas)
            {
                if (!Globales.isQualitasCierraForm)
                {
                    System.Windows.Forms.MessageBox.Show("Debes Actualizar el pago", "Actualizar pago");
                    return;
                }
            }
            frmPassword contra = new frmPassword("");
            contra.desdePrincipal = opcion;
            contra.ShowDialog();
        }
        private void cierraFormas()
        {
            //tituloVentana.Content = "";
            if (Globales.isVentanaQualitas)
            {
                Globales.isVentanaQualitas = false;
                typeUsuarioQualitas.TipoCobro = "";
            }
            ventanaDisplay.Content = null;
            //botonCerrar.Visibility = Visibility.Hidden;
        }
        #endregion

        #region "Eventos"

        private void navegacion(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.ventanaDisplay.NavigationService.RemoveBackEntry();
        }
        private void setSubmenus(object sender, RoutedEventArgs e)
        {
            this.SPSUBMENUS.Children.Clear();
            //this.ventanaDisplay.Content = null;
            Style lblStyle = this.FindResource("subMenuLblMit") as Style;
            Style btnStyle = this.FindResource("subMenuBtnMit") as Style;

            Button button = (Button)sender;
            var subMenus = (List<MenuOpciones>)button.Tag;

            Label titulo = new Label
            {
                Content = button.Name.Replace("_", " "),
                Style = lblStyle
            };
            this.SPSUBMENUS.Children.Add(titulo);
            this.SPSUBMENUS.Width = 250;

            if (subMenus != null)
            {
                foreach (var item in subMenus)
                {
                    if (item.activo)
                    {
                        Button boton = new Button
                        {
                            Content = item.nombre,
                            ToolTip = item.nombre,
                            Style = btnStyle,
                            Tag = item
                        };

                        if (Globales.cpIntegraEMV.EsTouch())
                            boton.Height = 50;

                        boton.Click += selectOptionMenu;
                        this.SPSUBMENUS.Children.Add(boton);
                    }
                }
            }
        }
        private void selectOptionMenu(object sender, RoutedEventArgs e)
        {

            MenuOpciones opcion = (MenuOpciones)((Button)sender).Tag;
            this.ventanaDisplay.Content = null;
            this.SPSUBMENUS.Width = 0;
            this.GMODAL.Visibility = Visibility.Hidden;
            this.SPSUBMENUS.Children.Clear();

            this.eventoMenu(opcion);
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        public void abrirFrmNuevo(Page formulario, string titulo = "")
        {
            cierraFormas();
            //botonCerrar.Visibility = Visibility.Visible;
            //tituloVentana.Content = titulo;

            ventanaDisplay.Navigate(formulario);
        }
        private void adelantarAtrazar(object sender, MouseButtonEventArgs e)
        {
            string nombre = ((Grid)sender).Name;

            if (e.ChangedButton != MouseButton.Left)
                return;

            this.SPSUBMENUS.Width = 0;
            this.SPSUBMENUS.Children.Clear();

            int finalIndex = 0;
            bool cargaMenu = false;
            switch (nombre)
            {
                case "BATRAS":
                    if (this.indexInicialMenu > 0)
                    {
                        this.indexInicialMenu--;
                        cargaMenu = true;
                    }
                    break;
                case "BADELANTE":
                    if (this.menuList.Count > 7)
                    {
                        this.indexInicialMenu++;
                        cargaMenu = true;
                    }
                    break;
            }

            finalIndex = 7 + this.indexInicialMenu;

            if (finalIndex > this.menuList.Count)
            {
                finalIndex = this.menuList.Count;
                this.indexInicialMenu--;
            }
            if (cargaMenu)
            {
                this.SPMENUOPCIONES.Children.Clear();
                for (int i = this.indexInicialMenu; i < finalIndex; i++)
                {
                    this.SPMENUOPCIONES.Children.Add(this.menuList[i]);
                }
            }
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Style lblStyle = this.FindResource("subMenuLblMit") as Style;
                Style btnStyle = this.FindResource("subMenuBtnMit") as Style;

                this.SPSUBMENUS.Children.Clear();
                //this.ventanaDisplay.Content = null;

                Grid button = (Grid)sender;
                var subMenus = (List<MenuOpciones>)button.Tag;

                Label titulo = new Label
                {
                    Content = button.Name.Replace("_", " "),
                    Style = lblStyle
                };
                this.SPSUBMENUS.Children.Add(titulo);
                this.SPSUBMENUS.Width = 250;
                this.GMODAL.Visibility = Visibility.Visible;
                if (subMenus != null)
                {
                    foreach (var item in subMenus)
                    {
                        if (item.activo)
                        {
                            Button boton = new Button
                            {
                                Content = item.nombre,
                                ToolTip = item.nombre,
                                Style = btnStyle,
                                Tag = item
                            };
                            boton.Click += selectOptionMenu;
                            this.SPSUBMENUS.Children.Add(boton);
                        }
                    }
                }
            }
        }
        #endregion

        #region"Metodos de acceso a opciones de menú."
        private void eventoMenu(MenuOpciones opcion)
        {
            Globales.principal = this;
            switch (opcion.id)
            {
                case "menu2Ventasinpresenciadetarjeta":
                    ventaSinPresenciaDeTarjetaMenu2(opcion);
                    break;
                case "menu2Ventaconpresenciadetarjeta":
                    menu2Ventaconpresenciadetarjeta();
                    break;
                case "menu2VentaconAutenticación":
                    menu2VentaconAutenticacion();
                    break;
                case "menu2VentaForzada":
                    menu2VentaForzada();
                    break;
                case "menu2Reporteporusuario":
                    this.menu2Reporteporusuario('U');
                    break;
                case "menu2Reporteporsucursal":
                    this.menu2Reporteporusuario('S');
                    break;
                case "menu2CorteporUsuario":
                    menu2CorteporUsuario();
                    break;
                case "menu3Reimpresióndevoucherbancario":
                    menu3Reimpresióndevoucherbancario();
                    break;
                case "menu3ReenvíodeVoucher":
                    menu3ReenvíodeVoucher();
                    break;
                case "menu3Cancelación":
                    menu3Cancelación();
                    break;
                case "menu2OtrosCargosVta_telefónica"://Inicia funcionalidad de HOTEL.
                    ventaSinPresenciaDeTarjetaMenu2(opcion.nombre);
                    break;
                case "menu2CheckInVta_telefónica":
                    menu2CheckInVta_telefónica();
                    break;
                case "menu2Re_Autorización":
                    menu2Re_Autorización();
                    break;
                case "menu2CheckInconpresenciadetarjeta":
                    CheckInConPresenciadeTarjeta();
                    break;
                case "menu2CheckOut":
                    menu2CheckOut();
                    break;
                case "menu2Pre_Venta"://Inicia funcionalidad de Restaurante preventa
                    mnu2_7_Click();
                    break;
                case "menu2CierrePre_Venta":
                    mnu2_8_Click();
                    break;
                case "menu2Consumo"://Inicia funcionalidad de Restaurante CONSUMO
                    //menu2Ventaconpresenciadetarjeta();//MASS06062017
                    this.menu2VentaConPresenciaConsumo();
                    break;
                case "menu4Cambiodecontraseña":
                    menu4Cambiodecontraseña(true);
                    break;
                case "menu4Tipodeimpresoras":
                    menu4Tipodeimpresoras();
                    break;
                case "menu9VentaenEfectivo":
                    mnu10_1_Click();
                    break;
                case "menu1Reporteporusuario":
                    menu2Reporteporusuario('U',true);
                    break;
                case "menu1Reporteporsucursal":
                    menu2Reporteporusuario('S',true);
                    break;
                case "menu1PreventadeBoletos":
                    menu1PreventadeBoletos();
                    break;
                case "menu1Ventaoperativamanual":
                    menu1Ventaoperativamanual();
                    break;
                case "menu1Ventaconpresenciadetarjeta":
                    menu1Ventaconpresenciadetarjeta();
                    break;
                case "menu1CierredePreventa":
                    menu1CierredePreventa();
                    break;
                case "tokenToken":
                    Tokenizacion();
                    break;
                case "menu1VentaconAutenticación":
                    menu1VentaconAutenticacion();
                    break;

                case "menu1Ventasinpresenciadetarjeta":
                    menu1Ventasinpresenciadetarjeta();
                    break;
                case "menu1VentaForzada":
                    menu1VentaForzada();
                    break;
                case "menu4Lectordetarjetas":
                    menu4Lectordetarjetas();
                    break;
                case "menu8Productos":
                    menu8Productos();
                    break;
                case "recompensasReportes":
                    recompensasReportes();
                    break;
                case "recompensasReimpresión":
                    recompensasReimpresión();
                    break;
                case "pagueDirectoVentas":
                    pagueDirectoVentas();
                    break;
                case "pagueDirectoReimpresión":
                    pagueDirectoReimpresión();
                    break;
                case "pagueDirectoReporte":
                    pagueDirectoReporte();
                    break;
                case "menu5AcercadePCPAY":
                    menu5AcercadePCPAY();
                    break;
                case "menu5AcercadeAeroPayPC":
                    menu5AcercadePCPAY();
                    break;
                case "menuprepagoActivación":
                    menuprepagoActivación();
                    break;
                case "menuprepagoCobro":
                    menuprepagoCobro();
                    break;
                case "menuprepagoCobroManual":
                    menuprepagoCobroManual();
                    break;
                case "menuprepagoRecarga":
                    menuprepagoRecarga();
                    break;
                case "menuprepagoRecargaconId":
                    menuprepagoRecargaconId();
                    break;
                case "menuprepagoConsultadeSaldo":
                    menuprepagoConsultadeSaldo();
                    break;
                case "menuprepagoCancelación":
                    menuprepagoCancelación();
                    break;
                case "menuprepagoCancelaciónManual":
                    menuprepagoCancelaciónManual();
                    break;
                case "menu3Asociacióndeboletos":
                    menu3Asociacióndeboletos();
                    break;
                case "menu7Referencia":
                    menu7Referencia();
                    break;
                case "WalletsImpresiónvoucher":
                    WalletsImpresiónvoucher();
                    break;
                case "cuponesAltacliente":
                    cuponesAltacliente();
                    break;
                case "cuponesCanjear":
                    cuponesCanjear();
                    break;
                case "cuponesNúmerocelular":
                    cuponesNúmerocelular();
                    break;
                case "menu5Conectividad":
                    ConectividadMenu();
                    break;
                case "menu5ProbarLector":
                    mnu5_4_Click();
                    break;
                case "menu5ConversorDCC":
                    mnu5_5_Click();
                    break;
                case "menu4Capacidadtouch":
                    this.capacidadTouch();
                    break;
                case "menu5AcercadeAGENCIAS":
                    menu5AcercadePCPAY();
                    break;
                case "reportesReportes":
                    reportesReportes();
                    break;
                case "recompensasVentasinpresencia":
                    recompensasVentasinpresencia();
                    break;
                case "menu2Ventaoperativamanual":
                    menu2Ventaoperativamanual();
                    break;
                case "menu8ReporteVentadeServicios":
                    menu8ReporteVentadeServicios();
                    break;
                case "menu4Cambiardeusuario":
                    menu4Cambiardeusuario();
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Opción del menu no existe, contacte con el departamento de sistemas", "Error menú", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    break;
            }
            Globales.XMLFacturaE = "";
            Globales.cpIntegracion_sResult = "";

        }

        private void menu1Ventaoperativamanual()
        {
            frmMoto moto;
            Globales.isVentaForzada = false;
            if(Globales.isQualitas){
              if(!Globales.isQualitasCierraForm){
                  Globales.MessageBoxMit("Debes actualizar pagos.");
                  return;
              }
            }

            cierraFormas();
            Globales.IsOM = true;
            if (Globales.isAerolinea)
            {
                moto = new frmMoto();
                Globales.isVentasPropias = false;
                Globales.isVentaForzada = false;
                moto.LTITULO.Content = "VENTAS DE BOLETOS: Venta operativa manual";
                moto.reabrir = menu1Ventaoperativamanual;
                ventanaDisplay.Navigate(moto);
            }
        }

        private void menu4Cambiardeusuario()
        {
        volver:
            string input = "";
            inputBox box = new inputBox("Contraseña Cambiar de Usuario");
            box.mensajePersonalizado = "Debe introducir una contraseña.";
            box.setTitulo("Introduzca password de configuración:");
            box.ShowDialog();
            if (box.cancelarActivo) return;
            input = box.getValor();
            if (input == "MIT" + Math.Pow((DateTime.Now.Day * DateTime.Now.Month), 3))
            {
            otraVez:
                cierraFormas();
                frmLoginMDI logear = new frmLoginMDI();
                logear.login = login;
                logear.ShowDialog();
                if(logear.cerrarVentana){
                    return;
                }
                if (Globales.logeo)
                {
                    Hide();
                    Globales.saltar = false;
                    Globales.logeo = false;
                }
                else {
                    Mouse.OverrideCursor = null;
                    Globales.mostrar = true;

                    Globales.logeo = false;

                    if (!string.IsNullOrWhiteSpace(Globales.answerLoginUser))
                        Globales.MessageBoxMit(Globales.answerLoginUser);
                    else
                    {
                        if (string.IsNullOrWhiteSpace(TypeUsuario.CadenaXML))
                            Globales.MessageBoxMit("No hay conexión a internet, verifique permisos a:" + "\r\n" + Program.ip);
                        else if (TypeUsuario.CadenaXML.IndexOf("Administrador") > 0 && !Utils.GetDataXML(TypeUsuario.CadenaXML, "url_auto_desbloq").Equals(""))
                        {
                            string contentenido = Globales.GetDataXml("url_auto_desbloq", TypeUsuario.CadenaXML);
                            //pendiente
                            Mouse.OverrideCursor = null;
                            new frmBloqueo().ShowDialog();
                        }
                        else if (!Utils.GetDataXML(TypeUsuario.CadenaXML, "error").Equals(""))
                        {
                            Globales.MessageBoxMit(Utils.GetDataXML(TypeUsuario.CadenaXML, "error"));
                        }
                        else
                            Globales.MessageBoxMit("Temporalmente fuera de servicio. \r\nInténtelo de nuevo más tarde");
                    }


                    goto otraVez;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    Globales.MessageBoxMit("!Contraseña incorrecta¡");
                    goto volver;
                }
                else
                {
                    Globales.MessageBoxMit("Debe introducir una contraseña.");
                }
            }
        }

        private void menu8ReporteVentadeServicios()
        {
            string str = string.Empty;
            Globales.VServicios.dbgSetVsUrl(Globales.URL_VTASERV);
            str = Globales.VServicios.dbgVSHTMLReport(TypeUsuario.usu,TypeUsuario.Pass,TypeUsuario.Id_Branch,TypeUsuario.Id_Company);
            Globales.Imprimir_HTML(str,true);
        }

        private void menu2Ventaoperativamanual()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "menu2Ventaoperativamanual()";
            try
            {
                Globales.cleanValoresQualitas();
                if (Globales.isQualitas)
                {
                    if (Globales.isQualitasCierraForm)
                    {
                        goto Continua;
                    }
                    else
                    {
                        Globales.MessageBoxMit("Debe Actualizar el pago");
                        return;
                    }
                }
            Continua:
                cierraFormas();
                Globales.IsOM = true;
                frmMotoAgencia moto = new Pages.frmMotoAgencia();
                moto.caption = "VENTAS PROPIAS: Cargo a tarjeta bancaria vía operativa manual.";
                moto.LTITULO.Content = "VENTAS PROPIAS: Cargo a tarjeta bancaria vía operativa manual.";
                Globales.isVentasPropias = true;
                moto.isVentaSinPresencia = false;
                moto.numcvv2.Visibility = Visibility.Hidden;
                moto.lblcvv.Visibility = Visibility.Hidden;
                ventanaDisplay.Navigate(moto);
                if (Globales.isQualitas)
                {
                    moto.txtEmail.Visibility = Visibility.Hidden;
                    moto.lblEmail.Visibility = Visibility.Hidden;
                    moto.label_8.Content = "Referencia";
                    moto.numOrden.Text = "";
                    moto.numOrden.IsEnabled = true;
                    moto.importe.Text = "";
                    moto.importe.IsEnabled = true;
                    //Globales.cpIntegraEMV.set_IsQualitasVtaForzada(true);
                }
            }
            catch
            {

            }
        }

        private void recompensasVentasinpresencia()
        {
            if (Globales.isQualitas)
            {
                if (!Globales.isQualitasCierraForm)
                {
                    Globales.MessageBoxMit("Debes actualizar el pago.");
                    return;
                }
            }
            cierraFormas();
            Globales.bolActivaMotoP = true;
            Globales.isVentasPropias = true;
            frmRecompensasSinPresencia recompensas = new frmRecompensasSinPresencia();
            ventanaDisplay.Navigate(recompensas);
        }

        private void reportesReportes()
        {
            frmReporteCyC reporte = new frmReporteCyC();
            reporte.ShowDialog();
        }
        private void menu1VentaForzada()
        {
            Globales.IsOM = false;
            Globales.isVentaForzada = true;
            frmMoto frmMoto = new frmMoto();
            frmMoto.LTITULO.Content = "VENTA DE BOLETOS: Venta forzada";
            frmMoto.abrir = abrirFrmNuevo;
            ventanaDisplay.Navigate(frmMoto);
        }
        private void mnu5_5_Click()
        {
            try
            {
                modMIT.strNombreFP = NOMBRE_GENERAL + ".mnu5_5_Click()";

                //'**************************************************************************
                //'SOLO APLICA PARA QUALITAS
                //    If isQualitas Then
                //        If isQualitasCierraForm Then
                //            GoTo Continua
                //        Else
                //            MsgBoxEx "Debes Actualizar el pago", , , , vbCritical, NOMBRE_APP
                //            Exit Sub
                //        End If
                //    End If

                //Continua:
                //    CloseSkin
                if (TypeUsuario.bolCambiaPwd)
                {
                    string msj = Globales.GetDataXml("cambiaPwdDesc", TypeUsuario.CadenaXML);
                    Globales.MessageBoxMit(msj);


                }
                else
                {
                    Globales.cpIntegraEMV.ConversorDCC();

                }

            }
            catch
            {
                // MsgBoxEx strNombreFP & vbCrLf & "Error: " & Err.Number & vbCrLf & "Descripcion: " & Err.Description, , , , vbCritical, NOMBRE_APP
            }
        }
        private void mnu5_4_Click()
        {
            // Lector
            if (Globales.cpIntegraEMV.dbgSetReader())
            {
                frmProbarLector PruebaLector = new frmProbarLector();
                if (PruebaLector.existError)
                    Globales.MessageBoxMit(PruebaLector.errorDescription);
                else
                    PruebaLector.Show();
            }
            else
                Globales.MessageBoxMit("No hay lector conectado");


        }
        private void ConectividadMenu()
        {
            frmConectividad frmC = new frmConectividad();
            frmC.ShowDialog();
        }
        private void cuponesNúmerocelular()
        {
            cierraFormas();
            Globales.isCuponTelefonico = true;

            frmCuponBusqueda frmCuponTel = new frmCuponBusqueda();
            frmCuponTel.LTITULO.Content = "CUPONES:Búsqueda de cupón por teléfono móvil";
            frmCuponTel.cerrar = cierraFormas;
            ventanaDisplay.Navigate(frmCuponTel);
        }
        private void cuponesCanjear()
        {
            cierraFormas();
            Globales.isCuponTelefonico = false;
            frmCuponBusqueda formularioCupon = new frmCuponBusqueda();
            formularioCupon.LTITULO.Content = "CUPONES: Búsqueda de cupón";
            formularioCupon.cerrar = cierraFormas;
            ventanaDisplay.Navigate(formularioCupon);
        }
        public void cuponesAltacliente()
        {
            cierraFormas();
            frmCuponAlta alta = new frmCuponAlta();
            alta.LTITULO.Content = "CUPONES: Alta de cliente";
            alta.cerrar = cierraFormas;
            ventanaDisplay.Navigate(alta);
        }
        private void WalletsImpresiónvoucher()
        {
            cierraFormas();
            // botonCerrar.Visibility = Visibility.Visible;
            // tituloVentana.Content = "Wallets";
            frmImpresionWallets frmImpresionWallets = new frmImpresionWallets();

            frmImpresionWallets.cerrar = cierraFormas;
            ventanaDisplay.Navigate(frmImpresionWallets);
        }
        private void menu7Referencia()
        {
            cierraFormas();
            // botonCerrar.Visibility = Visibility.Visible;
            frm3GateReferencia frm3GateReferencia = new frm3GateReferencia();
            frm3GateReferencia.abrir = abrirFrmNuevo;
            ventanaDisplay.Navigate(frm3GateReferencia);
        }
        private void menu3Asociacióndeboletos()
        {
            //  tituloVentana.Content = "Asociacion de Boletos ";
            // botonCerrar.Visibility = Visibility.Visible;
            cierraFormas();
            frmConsultaPNR frmConsultaPNR = new frmConsultaPNR();
            // frmPagoReservacion.abrir = abrirFrmNuevo;
            frmConsultaPNR.LTITULO.Content = "ASOCIACIÓN DE BOLETOS";
            frmConsultaPNR.cerrar = cierraFormas;
            ventanaDisplay.Navigate(frmConsultaPNR);
        }
        private void menuprepagoCancelaciónManual()
        {
            cierraFormas();
            //  botonCerrar.Visibility = Visibility.Visible;

            frmGiftCancelacionM cancelacionManual = new frmGiftCancelacionM();
            cancelacionManual.LTITULO.Content = "CANCELACIÓN MANUAL";
            cancelacionManual.cerrar = cierraFormas;
            ventanaDisplay.Navigate(cancelacionManual);
        }
        private void menuprepagoCancelación()
        {
            cierraFormas();
            //  botonCerrar.Visibility = Visibility.Visible;
            frmGiftCancelacion cancelacion = new frmGiftCancelacion();
            cancelacion.LTITULO.Content = "CANCELACIÓN";
            cancelacion.cerrar = cierraFormas;
            ventanaDisplay.Navigate(cancelacion);
        }
        private void menuprepagoConsultadeSaldo()
        {
            cierraFormas();
            //  botonCerrar.Visibility = Visibility.Visible;
            // tituloVentana.Content = "Consulta de saldo";
            frmGiftEdoCuenta estadoCuenta = new frmGiftEdoCuenta();
            estadoCuenta.LTITULO.Content = "CONSULTA DE SALDO";
            estadoCuenta.cerrar = cierraFormas;
            ventanaDisplay.Navigate(estadoCuenta);
        }
        private void menuprepagoRecargaconId()
        {
            cierraFormas();
            //  botonCerrar.Visibility = Visibility.Visible;
            //  tituloVentana.Content = "Recarga con id";
            frmGiftRECIdPersonal recargaId = new frmGiftRECIdPersonal();
            recargaId.LTITULO.Content = "RECARGA CON ID";
            //recargaId.cerr = cierraFormas;
            ventanaDisplay.Navigate(recargaId);
        }
        private void menuprepagoRecarga()
        {
            cierraFormas();
            // botonCerrar.Visibility = Visibility.Visible;
            //  tituloVentana.Content = "Recarga";
            frmGiftREC recarga = new frmGiftREC();
            recarga.LTITULO.Content = "RECARGA";
            recarga.cerrar = cierraFormas;
            ventanaDisplay.Navigate(recarga);
        }
        private void menuprepagoCobroManual()
        {
            cierraFormas();
            //    botonCerrar.Visibility = Visibility.Visible;
            //   tituloVentana.Content = "Cobro Manual";
            frmGiftCobroM cobroManual = new frmGiftCobroM();
            cobroManual.LTITULO.Content = "COBRO MANUAL";
            //cobroManual.cerrar = cierraFormas;
            ventanaDisplay.Navigate(cobroManual);
        }
        private void menuprepagoCobro()
        {
            cierraFormas();
            // botonCerrar.Visibility = Visibility.Visible;
            // tituloVentana.Content = "Cobro";
            frmGiftVenta venta = new frmGiftVenta();
            venta.LTITULO.Content = "COBRO";
            venta.cerrar = cierraFormas;
            ventanaDisplay.Navigate(venta);

        }
        private void menuprepagoActivación()
        {
            cierraFormas();
            //  botonCerrar.Visibility = Visibility.Visible;
            // tituloVentana.Content = "Activación";
            frmGiftACT frm = new frmGiftACT();
            frm.LTITULO.Content = "ACTIVACIÓN";
            frm.cerrando = cierraFormas;
            ventanaDisplay.Navigate(frm);
        }
        private void menu5AcercadePCPAY()
        {

            cierraFormas();
            // botonCerrar.Visibility = Visibility.Visible;
            //tituloVentana.Content = "Acerca de ...";
            fmrAcerca fmrAcerca = new fmrAcerca();
            fmrAcerca.cerrar = cierraFormas;
            ventanaDisplay.Navigate(fmrAcerca);
        }
        private void pagueDirectoReporte()
        {
            cierraFormas();

            // tituloVentana.Content = "Pague directo - Pague directo";
            //frmPagueDirectoReporte impre = new frmPagueDirectoReporte();
            //impre.cerrar = cierraFormas;
            ////botonCerrar.Visibility = Visibility.Visible;
            //ventanaDisplay.Navigate(impre);
        }
        private void pagueDirectoReimpresión()
        {
            cierraFormas();

            // tituloVentana.Content = "Pague directo- Reimpresión";
            //frmPagueDirectoReimpresion impre = new frmPagueDirectoReimpresion();
            //impre.cerrar = cierraFormas;
            ////botonCerrar.Visibility = Visibility.Visible;
            //ventanaDisplay.Navigate(impre);
        }
        private void pagueDirectoVentas()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "pagueDirectoVentas;";
            try
            {
                cierraFormas();
                //// botonCerrar.Visibility = Visibility.Visible;
                //// tituloVentana.Content = "Pago directo";
                //frmPagueDirectoVenta directo = new frmPagueDirectoVenta();
                //ventanaDisplay.Navigate(directo);
            }
            catch
            {
                Globales.MessageBoxMit(Globales.strNombreFP);
            }
        }
        private void recompensasReimpresión()
        {
            cierraFormas();
            //tituloVentana.Content = "Reimpresion recompensas";
            //botonCerrar.Visibility = Visibility.Visible;
            frmImpresionRecompensas frmImpresionRecompensas = new frmImpresionRecompensas();
            frmImpresionRecompensas.LTITTULO.Content = "Reimpresión recompensas";
            frmImpresionRecompensas.cerrar = cierraFormas;
            ventanaDisplay.Navigate(frmImpresionRecompensas);
        }
        private void recompensasReportes()
        {
            cierraFormas();
            //tituloVentana.Content = "Reportes Recompensas";
            //botonCerrar.Visibility = Visibility.Visible;
            frmCorteRecompensas frmCorteRecompensas = new frmCorteRecompensas();
            frmCorteRecompensas.LTITTULO.Content = "Reportes Recompensas";
            frmCorteRecompensas.cerrar = cierraFormas;
            ventanaDisplay.Navigate(frmCorteRecompensas);
        }
        private void menu8Productos()
        {
            cierraFormas();

            //botonCerrar.Visibility = Visibility.Visible;
            frmVtaSrvSeleccionar frmVtaSrvSeleccionar = new frmVtaSrvSeleccionar();
            frmVtaSrvSeleccionar.LTITTULO.Content = "PRODUCTOS";
            frmVtaSrvSeleccionar.abrirFrmNuevo = abrirFrmNuevo;
            ventanaDisplay.Navigate(frmVtaSrvSeleccionar);

        }
        private void menu4Lectordetarjetas()
        {
        volver:
            string input = "";
            inputBox box = new inputBox("Contraseña Impresora");
            box.mensajePersonalizado = "Debe introducir una contraseña.";
            box.setTitulo("Introduzca password de configuración:");
            box.ShowDialog();
            if (box.cancelarActivo) return;
            input = box.getValor();
            if (input == "MIT" + Math.Pow((DateTime.Now.Day * DateTime.Now.Month), 3))
            {
                cierraFormas();
                frmLector frmLector = new frmLector();
                frmLector.LTITULO.Content = "Lector tarjetas";
                frmLector.cerrar = cierraFormas;
                ventanaDisplay.Navigate(frmLector);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    Globales.MessageBoxMit("!Contraseña incorrecta¡");
                    goto volver;
                }
                else
                {
                    Globales.MessageBoxMit("Debe introducir una contraseña.");
                }
            }
        }
        private void menu1VentaconAutenticacion()
        {
            //tituloVentana.Content = "Venta de Boleto con cargo a Tarjeta ";
            //botonCerrar.Visibility = Visibility.Visible;
            frmAVSs2Agencia frmAVSs2Agencia = new frmAVSs2Agencia();
            frmAVSs2Agencia.abrir = abrirFrmNuevo;
            frmAVSs2Agencia.cerrar = cierraFormas;
            ventanaDisplay.Navigate(frmAVSs2Agencia);
        }
        private void menu1CierredePreventa()
        {
            //tituloVentana.Content = "VENTA DE BOLETOS: busqueda de reservación ";
            //botonCerrar.Visibility = Visibility.Visible;
            frmPagoReservacion frmPagoReservacion = new frmPagoReservacion();
            frmPagoReservacion.LTITULO.Content = "VENTA DE BOLETOS: Búsqueda de la reservación";
            frmPagoReservacion.abrir = abrirFrmNuevo;
            ventanaDisplay.Navigate(frmPagoReservacion);
        }
        private void Tokenizacion()
        {
            cierraFormas();
            //botonCerrar.Visibility = Visibility.Visible;
            //tituloVentana.Content = "Tokenizacion usuario";

            if (!Globales.cpIntegraEMV.dbgSetReader())
            {
                frmLectorConectado lector = new frmLectorConectado();
                lector.ShowDialog();
                if (!Globales.lectorConectado)
                {
                    //botonCerrar.Visibility = Visibility.Hidden;
                    return;
                }

            }

            frmToken frmToken = new frmToken();
            ventanaDisplay.Navigate(frmToken);
        }
        private void menu1Ventaconpresenciadetarjeta()
        {
            Globales.cpHTTP_sResult = "";
            int iCOM = 0;
            Globales.strNombreFP = NOMBRE_GENERAL + ".mnu1_2_Click()";

            if (TypeUsuario.bolCambiaPwd)
            {
                Globales.MessageBoxMit(Globales.GetDataXml("cambiaPwdDesc", TypeUsuario.CadenaXML));
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_Trademark()))
                {
                    frmLectorConectado frmLector = new frmLectorConectado();
                    frmLector.Show();
                    return;
                }

                if (TypeUsuario.strTipoLector != "0")
                {
                    if (Globales.isAerolinea)
                    {
                        Globales.bolActivaMotoP = true;
                        Globales.isVentasPropias = false;

                        //tituloVentana.Content = "VENTA DE BOLETO: Venta con Presencia de tarjeta ";
                        //botonCerrar.Visibility = Visibility.Visible;
                        frmBanda frmBanda = new frmBanda();
                        frmBanda.LTITULO.Content = "VENTA DE BOLETO: Cargo con presencia de tarjeta ";
                        frmBanda.abrir = abrirFrmNuevo;
                        frmBanda.cerrar = cierraFormas;
                        ventanaDisplay.Navigate(frmBanda);
                    }
                    else
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        Mouse.OverrideCursor = null;
                        Globales.bolActivaMotoP = true;

                        if (Globales.isAgencias && TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
                        {

                            //tituloVentana.Content = "VENTAS RP3: Cargo con presencia de tarjeta. ";
                            //botonCerrar.Visibility = Visibility.Visible;
                            frmBanda frmBanda = new frmBanda();
                            frmBanda.LTITULO.Content = "VENTAS RP3: Cargo con presencia de tarjeta. ";
                            frmBanda.abrir = abrirFrmNuevo;
                            ventanaDisplay.Navigate(frmBanda);
                        }
                        else if (Globales.isAgencias)
                        {
                            //tituloVentana.Content = "VENTA DE BOLETO: Venta con Presencia de tarjeta ";
                            //botonCerrar.Visibility = Visibility.Visible;
                            frmBanda frmBanda = new frmBanda();
                            frmBanda.LTITULO.Content = "VENTA DE BOLETO: Venta con Presencia de tarjeta ";
                            frmBanda.abrir = abrirFrmNuevo;
                            ventanaDisplay.Navigate(frmBanda);

                        }
                        else
                        {
                            //tituloVentana.Content = "VENTA DE BOLETO: Venta con Presencia de tarjeta ";
                            //botonCerrar.Visibility = Visibility.Visible;
                            frmBanda frmBanda = new frmBanda();
                            frmBanda.LTITULO.Content = "VENTA DE BOLETO: Venta con Presencia de tarjeta ";
                            frmBanda.abrir = abrirFrmNuevo;
                            ventanaDisplay.Navigate(frmBanda);
                        }

                    }

                }
                else
                {
                    //tituloVentana.Content = "VENTA DE BOLETO: Venta con Presencia de tarjeta ";
                    //botonCerrar.Visibility = Visibility.Visible;
                    frmBanda frmBanda = new frmBanda();
                    frmBanda.abrir = abrirFrmNuevo;
                    ventanaDisplay.Navigate(frmBanda);
                    Globales.bolActivaMoto = true;
                   

                }

            }

        }
        private void menu1PreventadeBoletos()
        {
            //tituloVentana.Content = "PREVENTA DE BOLETO";
            //botonCerrar.Visibility = Visibility.Visible;
            frmReservacion frmReservacion = new frmReservacion();
            frmReservacion.LTITULO.Content = "Preventa de boleto";
            ventanaDisplay.Navigate(frmReservacion);
        }
        private void menu1Ventasinpresenciadetarjeta()
        {
            if (TypeUsuario.bolCambiaPwd)
            {
                Globales.MessageBoxMit(Globales.GetDataXml("cambiaPwdDesc", TypeUsuario.CadenaXML));
            }

            else
            {
                if (Globales.isAerolinea)
                {
                    Globales.isVentasPropias = false;
                    Globales.isVentaForzada = false;
                    Globales.IsOM = false;

                    //tituloVentana.Content = "VENTAS DE BOLETOS: Cargo a tarjeta bancaria sin presencia de tarjeta";
                    //botonCerrar.Visibility = Visibility.Visible;
                    frmMoto frmMoto = new frmMoto();
                    frmMoto.propietario = this;
                    frmMoto.LTITULO.Content = "VENTAS DE BOLETOS: Cargo a tarjeta bancaria sin presencia de tarjeta";
                    frmMoto.abrir = abrirFrmNuevo;
                    frmMoto.cerrar = cierraFormas;
                    ventanaDisplay.Navigate(frmMoto);
                }
                else
                {
                    //tituloVentana.Content = "VENTAS DE BOLETOS: Venta sin presencia de tarjeta";
                    //botonCerrar.Visibility = Visibility.Visible;
                    Globales.isVentaForzada = false;
                    frmMoto frmMoto = new frmMoto();
                    frmMoto.propietario = this;
                    frmMoto.LTITULO.Content = "VENTA DE BOLETOS: Venta sin presencia de tarjeta";
                    frmMoto.abrir = abrirFrmNuevo;
                    ventanaDisplay.Navigate(frmMoto);
                    Globales.IsOM = false;
                    frmMoto.limpia();

                    if (Globales.isAgencias && TypeUsuario.CadenaXML.Contains("<PCPAYRP3>"))
                    {

                        frmMoto.caption = "VENTAS RP3: " + " Venta sin Presencia de Tarjeta";
                        frmMoto.LTITULO.Content = "VENTAS RP3: " + " Venta sin Presencia de Tarjeta";
                        //tituloVentana.Content = "VENTAS RP3: " + " Venta sin Presencia de Tarjeta";
                        //.txtNoBoleto.Visible = False
                        //   .Label(11).Visible = False
                        //   .lbl3Digitos(12).Visible = False
                        //   .lblTitulo = "Empresas"
                    }
                    else if (Globales.isAgencias)
                    {
                        Globales.isVentasPropias = false;
                        //.Image1.Visible = True
                        //'Me.PicHeader = Me.PicAgencias'checa aqui
                        //.fraCubreTodo.BackColor = 16777215   '16761024
                        //.fraDatos.BackColor = 16777215   '16761024
                        //.BackColor = 16777215   '16761024
                        //.lblTitulo = "Aerolínea"

                    }
                    else
                    {

                        //.Image2.Visible = True 'Express
                        //.fraCubreTodo.BackColor = 16777215
                        //.fraDatos.BackColor = 16777215
                        //.BackColor = 16777215
                        //.fraCubreTodo.Visible = False
                        //.fraDatos.Visible = True
                        //.Height = 5400 '3975 '5385
                    }



                }
            }
        }
        private void mnu10_1_Click()
        {

            cierraFormas();
            //tituloVentana.Content = "Factura electrónica";
            //botonCerrar.Visibility = Visibility.Visible;
            frmPreguntaFactura frmPF = new frmPreguntaFactura();
            frmPF.abrirFrmAhora = abrirFrmNuevo;
            frmPF.cerraPage = cierraFormas;
            ventanaDisplay.Navigate(frmPF);

        }
        private void menu4Tipodeimpresoras()
        {
        volver:
            string input = "";
            PcPay.Forms.Formularios.inputBox box = new inputBox("Contraseña Impresora");
            box.mensajePersonalizado = "Debe introducir una contraseña.";
            box.setTitulo("Introduzca password de configuración:");
            box.ShowDialog();
            if (box.cancelarActivo) return;
            input = box.getValor();
            if (input == "MIT" + Math.Pow((DateTime.Now.Day * DateTime.Now.Month), 3))
            {
                cierraFormas();
                //tituloVentana.Content = "Tipo de impresoras";
                //botonCerrar.Visibility = Visibility.Visible;
                frmImpresora impresora = new frmImpresora();
                impresora.cerrar = cierraFormas;
                ventanaDisplay.Navigate(impresora);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    Globales.MessageBoxMit("!Contraseña incorrecta¡");
                    goto volver;
                }
                else
                {
                    Globales.MessageBoxMit("Debe introducir una contraseña.");
                }
            }
        }
        private void menu4Cambiodecontraseña(bool p)
        {
            cambiaContraseña(true);
        }
        public void abrirFrmNuevo(Page formulario)
        {
            cierraFormas();
            //botonCerrar.Visibility = Visibility.Visible;
            ventanaDisplay.Navigate(formulario);
        }
        private void ventaSinPresenciaDeTarjetaMenu2(MenuOpciones opcion)
        {
            cierraFormas();
            if (Globales.isQualitas)
            {
                if ((!string.IsNullOrWhiteSpace(typeUsuarioQualitas.TipoCobro) ? typeUsuarioQualitas.TipoCobro.ToUpper() : "") == "OTROS")
                {
                    frmQualitasTipocobro frmQualitasTipocobr = new frmQualitasTipocobro();
                    frmQualitasTipocobr.tipoVtaQualitas = 1;
                    frmQualitasTipocobr.cerrar = cierraFormas;
                    cierraFormas();
                    // typeUsuarioQualitas.TipoCobro = "";
                    goto cargaVtaDirecta;
                }

                if (Globales.isQualitasCierraForm)
                {
                    goto continua;
                }
                else
                {
                    Globales.MessageBoxMit("Debes actualizzar el pago.");
                    return;
                }

            continua:
                if (!Globales.isQualitasTrxValida)
                {
                    cierraFormas();
                    //botonCerrar.Visibility = Visibility.Visible;
                    frmQualitasTipocobro qualitas = new frmQualitasTipocobro();
                    qualitas.cerrar = cierraFormas;
                    qualitas.tipoVtaQualitas = 2;
                    ventanaDisplay.Navigate(qualitas);
                    //tituloVentana.Content = qualitas.caption;
                    //tituloVentana.Content = "QUÁLITAS: Tipo de Cobro";
                    qualitas.menu1 = ventaSinPresenciaDeTarjetaMenu2;
                    qualitas.menu2 = menu2Ventaconpresenciadetarjeta;
                    return;
                }
                else
                {
                    goto cargaVtaDirecta;
                }

            }
        cargaVtaDirecta:

            cierraFormas();
            //botonCerrar.Visibility = Visibility.Visible;
            frmMotoAgencia = new frmMotoAgencia();
            //tituloVentana.Content = "VENTAS PROPIAS " + Convert.ToString(p);
            Globales.IsOM = false;
            Globales.isVentasPropias = true;
            frmMotoAgencia.isVentaSinPresencia = true;
            frmMotoAgencia.limpia();
            frmMotoAgencia.caption = "VENTAS PROPIAS: " + Convert.ToString(opcion.nombre);
            frmMotoAgencia.LTITULO.Content = "VENTAS PROPIAS: " + Convert.ToString(opcion.nombre);

            if (!Globales.isVentasPropias && !Globales.IsOM && Globales.isAerolinea)
            {
                frmMotoAgencia.caption = "VENTA DE BOLETOS:Cargo a tarjeta bancaria sin presencia de tarjeta.";
                frmMotoAgencia.LTITULO.Content = "VENTA DE BOLETOS:Cargo a tarjeta bancaria sin presencia de tarjeta.";
                //tituloVentana.Content = "VENTA DE BOLETOS:Cargo a tarjeta bancaria sin presencia de tarjeta";
            }
            else if (Globales.IsOM && Globales.isAerolinea && !Globales.isVentasPropias)
            {
                frmMotoAgencia.caption = "VENTA DE BOLETOS: Cargo a tarjeta bancaria vía operativa manual.";
                //tituloVentana.Content = "VENTA DE BOLETOS: Cargo a tarjeta bancaria vía operativa manual";
            }
            frmMotoAgencia.cerrando = cierraFormas;
            frmMotoAgencia.cierra = cierraFormas;
            frmMotoAgencia.abrirFrmNuevo = abrirFrmNuevo;
            ventanaDisplay.Navigate(frmMotoAgencia);
            Globales.isQualitasTrxValida = false;
        }
        private void menu2Ventaconpresenciadetarjeta()
        {
            cierraFormas();
            if (Globales.isQualitas)
            {
                if ((!string.IsNullOrWhiteSpace(typeUsuarioQualitas.TipoCobro) ? typeUsuarioQualitas.TipoCobro.ToUpper() : "") == "OTROS")
                {
                    frmQualitasTipocobro frmQualitasTipocobr = new frmQualitasTipocobro();
                    frmQualitasTipocobr.tipoVtaQualitas = 1;
                    frmQualitasTipocobr.cerrar = cierraFormas;
                    cierraFormas();
                    // typeUsuarioQualitas.TipoCobro = "";
                    goto cargaVtaDirecta;
                }

                if (Globales.isQualitasCierraForm)
                {
                    goto continua;
                }
                else
                {
                    Globales.MessageBoxMit("Debes actualizar el pago.");
                    return;
                }

            continua:
                if (!Globales.isQualitasTrxValida)
                {
                    cierraFormas();
                    //botonCerrar.Visibility = Visibility.Visible;
                    frmQualitasTipocobro qualitas = new frmQualitasTipocobro();
                    qualitas.cerrar = cierraFormas;
                    qualitas.tipoVtaQualitas = 1;
                    ventanaDisplay.Navigate(qualitas);
                    //tituloVentana.Content = qualitas.caption;
                    //tituloVentana.Content = "QUÁLITAS: Tipo de Cobro";
                    qualitas.menu1 = ventaSinPresenciaDeTarjetaMenu2;
                    qualitas.menu2 = menu2Ventaconpresenciadetarjeta;
                    return;
                }
                else
                {
                    goto cargaVtaDirecta;
                }

            }
        cargaVtaDirecta:

            cierraFormas();
            //tituloVentana.Content = "Venta con presencia de tarjeta.";
            //botonCerrar.Visibility = Visibility.Visible;
            //
            if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_Trademark()))
            {
                frmLectorConectado lector = new frmLectorConectado();
                lector.ShowDialog();
                if (!Globales.lectorConectado)
                {
                    //botonCerrar.Visibility = Visibility.Hidden;
                    return;
                }

            }

            else if (Globales.cpIntegraEMV.chkPp_soportaDUKPT())
            {
                if (!(Globales.cpIntegraEMV.chkPp_llaveDUKPT()))
                {
                    frmLectorConectado lector = new frmLectorConectado();
                    lector.Owner = this;
                    lector.ShowDialog();

                    if (!Globales.lectorConectado)
                    {
                        return;
                    }
                }
            }
            Globales.bolActivaMotoP = true;
            if (Globales.GetDataXml("facileasing", TypeUsuario.CadenaXML).Substring(0, 1) == "1")
            {
                frmFacileasing formulario = new frmFacileasing();
                formulario.abrir = abrirFrmNuevo;
                ventanaDisplay.Navigate(formulario);
            }
            else
            {
                if (Globales.isQualitas)
                {
                    if (!Globales.isQualitasCierraForm)
                    {
                        Globales.MessageBoxMit("Debes actualizar el pago");
                        return;
                    }
                }
            }
            Globales.isVentasPropias = true;
            frmBandaEMV = new frmBandaEMV();
            frmBandaEMV.propietario = this;
            frmBandaEMV.abrirFrmNuevo = abrirFrmNuevo;
            frmBandaEMV.cerrar = cierraFormas;
            this.frmBandaEMV.LTITULO.Content = "VENTAS PROPIAS: Cargo con presencia de tarjeta.";
            ventanaDisplay.Navigate(frmBandaEMV);
        }
        private void menu2VentaconAutenticacion()
        {
            cierraFormas();
            //tituloVentana.Content = "Venta con autentificación";
            //botonCerrar.Visibility = Visibility.Visible;
            Globales.isVentasPropias = true;
            frmAVSs2 = new frmAVSs2();
            frmAVSs2.abrirFrmNuevo = abrirFrmNuevo;
            frmAVSs2.cerrar = cierraFormas;
            frmAVSs2.LTITTULO.Content = frmAVSs2.Title;
            ventanaDisplay.Navigate(frmAVSs2);
        }
        private void menu3Reimpresióndevoucherbancario()
        {

            cierraFormas();
            //botonCerrar.Visibility = Visibility.Visible;
            frmImpresion = new frmImpresion();
            frmImpresion.caption = "Consulta de vouchers";
            //tituloVentana.Content = "Consulta de vouchers";
            frmImpresion.LTITULO.Content = "Consulta de vouchers";
            frmImpresion.cerrando = cierraFormas;
            ventanaDisplay.Navigate(frmImpresion);
        }
        private void menu3ReenvíodeVoucher()
        {
            cierraFormas();
            //botonCerrar.Visibility = Visibility.Visible;
            frmImpresion = new frmImpresion();
            frmImpresion.caption = "Reenvío de Vouchers";
            //tituloVentana.Content = "Reenvío de Vouchers";
            frmImpresion.LTITULO.Content = "Reenvío de Vouchers";
            frmImpresion.cerrando = cierraFormas;
            ventanaDisplay.Navigate(frmImpresion);
        }
        private void menu3Cancelación()
        {
            cierraFormas();
            //tituloVentana.Content = "Cancelación";
            //botonCerrar.Visibility = Visibility.Visible;
            frmCancelacion = new frmCancelacion();
            frmCancelacion.propietario = this;
            ventanaDisplay.Navigate(frmCancelacion);
        }
        private void menu2Reporteporusuario(char reportType,bool isMenu1 = false)
        {
            string NOMBRE_APP = Globales.NOMBRE_APP;
            string cadenaToEncrypt = string.Empty;

            string strNombreFP = "MDIForm.mnu2_9_Click()";
            string auxName = string.Empty;
            string strTipo = "EXPRESS";

            if(Globales.isAgencias){
                strTipo = (isMenu1) ? "BOL" : "PROP";
            }

            auxName = "TPV Santander Retail";
            if (isMenu1)
            {
                if(NOMBRE_APP == "AeroPay PC"){
                    auxName = "AeroPay Express";
                }
            }
            else {
                if (NOMBRE_APP == "TPV Santander Retail")
                {
                    auxName = "PcPay";
                    NOMBRE_APP = auxName;
                }
            }

            if (NOMBRE_APP == "AeroPay PC") NOMBRE_APP = "AeroPay Express";

            cadenaToEncrypt =
                "&usuario="+TypeUsuario.usu+
                "&sucursal="+TypeUsuario.Id_Branch+
                "&cdEmpresa=" + TypeUsuario.Id_Company +
                "&tipoVenta="+strTipo+
                "&tipoCorte=" + reportType +
                "&op="+"corteDia"+
                "&app="+NOMBRE_APP+
                "&etiqueta="+TypeUsuario.reference+
                "&version="+TypeUsuario.strVersion;

            Globales.cpHTTP_Clear();
            Globales.cpHTTP_sURL_cpCUCT = TypeUsuario.Url;
            Globales.cpHTTP_cadena1 = string.Format("enc={0}",
                Globales.encryptString(cadenaToEncrypt, Globales.KEY_RC4, true));
            if (Globales.cpHTTP_SendcpCUCT())
            {
                Globales.Imprimir_HTML(Globales.cpHTTP_sResult, true);
            }
        }
        private void menu2CorteporUsuario()
        {
            try
            {
                Globales.bolActivaMotoP = true;
                cierraFormas();
                Globales.frmImprimeReporteDiario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ventaSinPresenciaDeTarjetaMenu2(string p)
        {

            if (Globales.isQualitas)
            {
                if ((!string.IsNullOrWhiteSpace(typeUsuarioQualitas.TipoCobro) ? typeUsuarioQualitas.TipoCobro.ToUpper() : "") == "OTROS")
                {
                    frmQualitasTipocobro frmQualitasTipocobr = new frmQualitasTipocobro();
                    frmQualitasTipocobr.tipoVtaQualitas = 1;
                    frmQualitasTipocobr.cerrar = cierraFormas;
                    cierraFormas();
                    // typeUsuarioQualitas.TipoCobro = "";
                    goto cargaVtaDirecta;
                }

                if (Globales.isQualitasCierraForm)
                {
                    goto continua;
                }
                else
                {
                    Globales.MessageBoxMit("Debes actualizar el pago.");
                    return;
                }

            continua:
                if (!Globales.isQualitasTrxValida)
                {
                    cierraFormas();
                    //botonCerrar.Visibility = Visibility.Visible;
                    frmQualitasTipocobro qualitas = new frmQualitasTipocobro();
                    qualitas.cerrar = cierraFormas;
                    qualitas.tipoVtaQualitas = 2;
                    ventanaDisplay.Navigate(qualitas);
                    //tituloVentana.Content = qualitas.caption;
                    //tituloVentana.Content = "QUÁLITAS: Tipo de Cobro";
                    qualitas.menu1 = ventaSinPresenciaDeTarjetaMenu2;
                    qualitas.menu2 = menu2Ventaconpresenciadetarjeta;
                    return;
                }
                else
                {
                    goto cargaVtaDirecta;
                }

            }
        cargaVtaDirecta:

            cierraFormas();
            //botonCerrar.Visibility = Visibility.Visible;
            this.frmMotoAgencia = new frmMotoAgencia();
            //tituloVentana.Content = "VENTAS PROPIAS " + Convert.ToString(p);
            Globales.IsOM = false;
            Globales.isVentasPropias = true;
            this.frmMotoAgencia.isVentaSinPresencia = true;
            this.frmMotoAgencia.limpia();
            this.frmMotoAgencia.caption = string.Format("VENTAS PROPIAS: {0}", p);
            this.frmMotoAgencia.LTITULO.Content = string.Format("VENTAS PROPIAS: {0}", p);

            if (!Globales.isVentasPropias && !Globales.IsOM && Globales.isAerolinea)
            {
                this.frmMotoAgencia.caption = "VENTA DE BOLETOS:Cargo a tarjeta bancaria sin presencia de tarjeta";
                //tituloVentana.Content = "VENTA DE BOLETOS:Cargo a tarjeta bancaria sin presencia de tarjeta";
            }
            else if (Globales.IsOM && Globales.isAerolinea && !Globales.isVentasPropias)
            {
                this.frmMotoAgencia.caption = "VENTA DE BOLETOS: Cargo a tarjeta bancaria vía operativa manual";
                //tituloVentana.Content = "VENTA DE BOLETOS: Cargo a tarjeta bancaria vía operativa manual";
            }
            this.frmMotoAgencia.cerrando = cierraFormas;
            this.frmMotoAgencia.abrirFrmNuevo = abrirFrmNuevo;
            this.ventanaDisplay.Navigate(this.frmMotoAgencia);
            Globales.isQualitasTrxValida = false;
        }
        private void menu2CheckInVta_telefónica()
        {
            cierraFormas();
            //tituloVentana.Content = "CHECK IN: Cargo a tarjeta bancaria vía manual.";
            //botonCerrar.Visibility = Visibility.Visible;
            frmMotoChekin frmMotoChekin = new frmMotoChekin();
            //frmMotoChekin.caption = Convert.ToString(tituloVentana.Content);
            frmMotoChekin.LTITULO.Content = "Moto Checkin";
            ventanaDisplay.Navigate(frmMotoChekin);
        }
        private void menu2Re_Autorización()
        {
            Globales.bolActivaMotoP = true;
            cierraFormas();
            //tituloVentana.Content = "ReAutorizacion.";
            //botonCerrar.Visibility = Visibility.Visible;
            frmReAutorizacion frmReAutorizacion = new frmReAutorizacion();
            frmReAutorizacion.LTITULO.Content = "Re-Autorización";
            frmReAutorizacion.cerrar = cierraFormas;
            ventanaDisplay.Navigate(frmReAutorizacion);
        }
        private void CheckInConPresenciadeTarjeta()
        {

            Globales.strNombreFP = NOMBRE_GENERAL + ".mnu2_5_Click()";

            if (TypeUsuario.bolCambiaPwd)
            {
                // MsgBoxEx getDataXML("cambiaPwdDesc", TypeUsuario.CadenaXML), , , , vbInformation, NOMBRE_APP
                Globales.MessageBoxMit(Globales.GetDataXml("cambiaPwdDesc", TypeUsuario.CadenaXML));
            }
            else
            {

                if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_Trademark()))
                {
                    //frmBanner.LbProc.Visible = False
                    //frmBanner.LbProc.Caption = ""
                    frmLectorConectado lector = new frmLectorConectado();
                    lector.Show();
                    return;
                }

                Globales.bolActivaMotoP = true;
                cierraFormas();
                //tituloVentana.Content = "Check In Con Presencia de Tarjeta";
                //botonCerrar.Visibility = Visibility.Visible;



                frmCheckIn frmToken = new frmCheckIn();
                frmToken.abrir = abrirFrmNuevo;
                frmToken.cerrar = cierraFormas;
                frmToken.propietario = this;
                frmToken.LTITULO.Content = "Check In Con Presencia de Tarjeta";
                ventanaDisplay.Navigate(frmToken);

            }



        }

        private void menu2CheckOut()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".mnu2_6_Click()";
            if (TypeUsuario.bolCambiaPwd)
            {
                Globales.MessageBoxMit(Globales.GetDataXml("cambiaPwdDesc", TypeUsuario.CadenaXML));
            }
            else
            {
                //tituloVentana.Content = "Check Out";
                //botonCerrar.Visibility = Visibility.Visible;
                frmCheckout frmCheckout = new frmCheckout();
                frmCheckout.LTITULO.Content = "CHECK OUT";
                frmCheckout.abrir = abrirFrmNuevo;
                frmCheckout.abriendo = menu2CheckOut;
                frmCheckout.cierra = cierraFormas;
                ventanaDisplay.Navigate(frmCheckout);
            }

        }
        private void mnu2_7_Click()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + ".mnu2_7_Click()";
            frmLectorConectado frmLec = new frmLectorConectado();
            //'Carga nueva forma de preventa 30-09-2009 MRR
            if (TypeUsuario.AddTableNum)
            {
                if (string.IsNullOrEmpty(Globales.cpIntegraEMV.chkPp_Trademark()))
                {
                    //frmBanner.LbProc.Visible = False
                    //frmBanner.LbProc.Caption = ""
                    frmLec.Show();
                    return;
                }
                frmPreventaPropina frmPreventaPropina = new frmPreventaPropina();
                frmPreventaPropina.abrir = abrirFrmNuevo;
                frmPreventaPropina.cierra = cierraFormas;
                frmPreventaPropina.menuPrincipal = this;
                //tituloVentana.Content = "Preventa menu";
                ventanaDisplay.Navigate(frmPreventaPropina);
            }
            else
            {
                if (TypeUsuario.bolCambiaPwd)
                {
                    string msj = Globales.GetDataXml("cambiaPwdDesc", TypeUsuario.CadenaXML);
                    System.Windows.Forms.MessageBox.Show(msj, Globales.NOMBRE_APP);
                }
                else
                {
                    cierraFormas();
                    if (string.IsNullOrEmpty(Globales.cpIntegraEMV.chkPp_Trademark()))
                    {
                        //frmBanner.LbProc.Visible = False
                        //frmBanner.LbProc.Caption = ""
                        frmLec.Show();
                        return;
                    }
                    Globales.bolActivaMotoP = true;
                    //botonCerrar.Visibility = Visibility.Visible;
                    frmPreventa frmPreventa = new frmPreventa();
                    
                    frmPreventa.LTITULO.Content = "Preventa";
                    //tituloVentana.Content = "Preventa";
                    ventanaDisplay.Navigate(frmPreventa);
                }
            }
        }
        public void menu2VentaConPresenciaConsumo()
        {
            this.cierraFormas();
            //botonCerrar.Visibility = Visibility.Visible;
            //tituloVentana.Content = "Venta consumo";

            if (TypeUsuario.AddTableNum)
            {
                if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_Trademark()))
                {
                    frmLectorConectado lector = new frmLectorConectado();
                    lector.ShowDialog();
                    if (!Globales.lectorConectado)
                    {
                        //botonCerrar.Visibility = Visibility.Hidden;
                        return;
                    }
                }
                Globales.msjRech = "Consumo rechazado";


                frmPreventaPropina frmPreventaPropina = new frmPreventaPropina();
                frmPreventaPropina.LTITULO.Content = "CONSUMO";
                frmPreventaPropina.abrir = abrirFrmNuevo;
                frmPreventaPropina.menuPrincipal = this;
                ventanaDisplay.Navigate(frmPreventaPropina);

            }
            else
            {
                if (TypeUsuario.bolCambiaPwd)
                    MessageBox.Show(string.Format("{0}", Globales.GetDataXml("cambiaPwdDesc", TypeUsuario.CadenaXML)));
                else
                {
                    this.cierraFormas();
                    if (string.IsNullOrWhiteSpace(Globales.cpIntegraEMV.chkPp_Trademark()))
                    {
                        frmLectorConectado lector = new frmLectorConectado();
                        lector.ShowDialog();
                        if (!Globales.lectorConectado)
                        {
                            //botonCerrar.Visibility = Visibility.Hidden;
                            return;
                        }
                    }
                    Globales.bolActivaMotoP = true;

                    /// Aquí cargar la funcionalidad para preventa... MASS06062017
                    frmPreventa preventa = new frmPreventa();
                    preventa.LTITULO.Content = "PREVENTA";
                    /// ******
                }
            }

        }
        private void mnu2_8_Click()
        {

            Globales.strNombreFP = NOMBRE_GENERAL + ".mnu2_8_Click()";
            if (TypeUsuario.bolCambiaPwd)
            {
                string msj = Globales.GetDataXml("cambiaPwdDesc", TypeUsuario.CadenaXML);
                System.Windows.Forms.MessageBox.Show(msj, Globales.NOMBRE_APP, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            }
            else
            {
                cierraFormas();
                //botonCerrar.Visibility = Visibility.Visible;
                frmCierrePreventa frmCierrePreventa = new frmCierrePreventa();
                frmCierrePreventa.LTITULO.Content = "CIERRE PREVENTA";
                frmCierrePreventa.abre = abrirFrmNuevo;
                frmCierrePreventa.cierra = cierraFormas;
                ventanaDisplay.Navigate(frmCierrePreventa);
            }


        }
        private void menu2VentaForzada()
        {
            Globales.strNombreFP = NOMBRE_GENERAL + "menu2VentaForzada();";
            try
            {
                cierraFormas();
                // botonCerrar.Visibility = Visibility.Visible;
                Globales.bolActivaMotoP = true;
                Globales.IsOM = true;
                // tituloVentana.Content = "Venta forzada";
                frmVentaForzada frmVentaForzada = new frmVentaForzada();
                frmVentaForzada.caption = "VENTA FORZADA";
                ventanaDisplay.Navigate(frmVentaForzada);
            }

            catch
            {

            }
        }
        private void capacidadTouch()
        {
            //fin de la verificación para qualitas.
            if (TypeUsuario.bolCambiaPwd)
                Globales.MessageBoxMit(string.Format("{0}", Globales.GetDataXml("cambiaPwdDesc", TypeUsuario.CadenaXML)));
            else
            {
                inputBox inputbox = new inputBox("Capaciadad Touch.");
                inputbox.setTitulo("Introduzca password de configuración.");
                inputbox.mensajePersonalizado = "Debe introducir una contraseña.";

                inputbox.ShowDialog();

                string xmlvalor = TypeUsuario.CadenaXML;

                string valor = inputbox.getValor().ToUpper();
                if (valor == string.Format("MIT{0}", (long)Math.Pow(DateTime.Now.Day * DateTime.Now.Month, 3)))
                {
                    Globales.cpIntegraEMV.CapacidadTouch();


                    string capTouch = Globales.GetSettingString("CAPACIDAD_TOUCH");
                    string enviaVoucherMail = Globales.GetSettingString("CAPACIDAD_TOUCH_MAIL");
                    TypeUsuario.UtilizarCapacidadTouch = (capTouch == "1" ? true : false);
                    TypeUsuario.EnviarVoucherMail = (enviaVoucherMail == "1" ? true : false);
                }
                else
                {
                    Globales.MessageBoxMit("¡Contraseña incorrecta!");
                }
            }
        }

        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Globales.principal = this;
            bool mensaje = Globales.MessageConfirm("¿Desea cerrar el programa?");
            if (mensaje)
            {
                e.Cancel = false;
                Application.Current.Shutdown();
            }
            else
                e.Cancel = true;
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.SPSUBMENUS.Width = 0;
                this.SPSUBMENUS.Children.Clear();
                this.GMODAL.Visibility = Visibility.Hidden;
            }
        }
        public void modalIsVisible(bool isVisible)
        {
            if (isVisible)
                this.GMODAL.Visibility = Visibility.Visible;
            else
                this.GMODAL.Visibility = Visibility.Hidden;
        }
        private void GMODAL_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.SPSUBMENUS.Width = 0;
                this.SPSUBMENUS.Children.Clear();
                this.GMODAL.Visibility = Visibility.Hidden;
            }

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.lectorConectado)
                new frmLectorConectado().ShowDialog();
        }

        private void mouseEnter_Principal(object sender, MouseEventArgs e)
        {
            Globales.noMostrarMensajes = true;
        }

        private void mouseLeave_Principal(object sender, MouseEventArgs e)
        {
            Globales.noMostrarMensajes = false;
        }

        private void SPSUBMENUS_MouseDown(object sender, MouseButtonEventArgs e)
        {
          
        }

        
    }
}
