using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using PcPay.Code.Configuracion;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace PcPay.Code.Utilidades
{
    public static class Utils
    {

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hwnd, bool revert);

        [DllImport("user32.dll")]
        private static extern bool DeleteMenu(IntPtr hMenu, uint position, uint flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        private const uint SC_CLOSE = 0xF060;
        private const uint CloseByPosition = 1024;
        /// <summary>
        /// LEE EL ARCHIVO PARAMETERS.MIT Y OBTIENE EL VALOR DEL PARAMETRO ENVIADO
        /// </summary>
        /// <param name="parametro">Parametro a buscar</param>
        /// <returns></returns>
        public static string ObtieneParametrosMIT(string parametro)
        {
            string parameter = "";
            string text;
            string file;
            string ruta;

            ruta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            file = System.IO.Path.Combine(ruta + "\\MIT\\Data", "parameters.mit");

            if (System.IO.File.Exists(file))
            {
                text = System.IO.File.ReadAllText(file);

                //DESCIFRA EL CONTENIDO DEL ARCHIVO PARAMETERS.MIT
                text = EncryptC.DecryptRC4(text, modMIT.KEY_RC4_CP);

                //SE OBTIENE EL VALOR DEL XML
                parameter = GetDataXML(text, parametro);
            }
            else
            {
                parameter = "";
            }

            return parameter;

        }
        /// <summary>
        /// GUARDA EN EL ARCHIVO PARAMETERS.MIT, EL PARAMETRO INDICADO
        /// </summary>
        /// <param name="parametro">parametro a guardar</param>
        /// <param name="valor">valor del parametro</param>
        /// <returns></returns>
        public static bool GuardaParametrosMIT(string parametro, string valor)
        {
            bool guardar = false;
            string parameter = "";
            string text;
            string file;
            string ruta;

            ruta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            existenDirectorios();
            file = System.IO.Path.Combine(ruta + "\\MIT\\Data", "parameters.mit");

            //se verica que exista el archivo
            if (!System.IO.File.Exists(file))
            {
                if (CreaCarpetaMIT())
                    System.IO.File.Create(file);
            }


            text = System.IO.File.ReadAllText(file);

            //DESCIFRA EL CONTENIDO DEL ARCHIVO PARAMETERS.MIT
            text = EncryptC.DecryptRC4(text, modMIT.KEY_RC4_CP);

            //SE OBTIENE EL VALOR DEL XML
            parameter = GetDataXML(text, parametro);

            //se sustituye el valor anterior
            string valorOriginal = "";
            string valorNuevo = "";

            if (!parameter.Equals(""))
            {
                valorOriginal = "<" + parametro + ">" + parameter + "</" + parametro + ">";
                valorNuevo = "<" + parametro + ">" + valor + "</" + parametro + ">";
                text = text.Replace(valorOriginal, valorNuevo);
            }
            else
            {
                valorOriginal = "<" + parametro + ">" + parameter + "</" + parametro + ">";
                valorNuevo = "<" + parametro + ">" + valor + "</" + parametro + ">";
                text = text.Replace(valorOriginal, "");
                text = text + valorNuevo;
            }


            //se guarda el archivo nuevamente
            System.IO.File.WriteAllText(file, EncryptC.EncryptRC4(text, modMIT.KEY_RC4_CP));

            return guardar;


        }
        /// <summary>
        /// Crea la carpeta MIT
        /// </summary>
        /// <returns></returns>
        private static bool CreaCarpetaMIT()
        {
            bool carpetaCreada = false;
            string directorio;
            string ruta;


            try
            {
                ruta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                directorio = ruta + "\\MIT";
                if (!System.IO.Directory.Exists(directorio))
                    System.IO.Directory.CreateDirectory(directorio);

                directorio = ruta + "\\MIT\\Data";
                if (!System.IO.Directory.Exists(directorio))
                    System.IO.Directory.CreateDirectory(directorio);

                directorio = ruta + "\\MIT\\Load";
                if (!System.IO.Directory.Exists(directorio))
                    System.IO.Directory.CreateDirectory(directorio);

                directorio = ruta + "\\MIT\\Log";
                if (!System.IO.Directory.Exists(directorio))
                    System.IO.Directory.CreateDirectory(directorio);


                carpetaCreada = true;
            }
            catch (Exception)
            {

                carpetaCreada = false;
            }


            return carpetaCreada;
        }
        /// <summary>
        /// Obtiene el valor del tag indicado, dentro del XML que se envía
        /// </summary>
        /// <param name="xml">XML donde se busca</param>
        /// <param name="tag">Valor a buscar</param>
        /// <returns></returns>
        /// 
        public static void existenDirectorios()
        {
            string ruta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);//Se saca la ruta de la carpeta de usuario.
            string rutaMIT = ruta + @"\MIT";
            string rutaData = ruta + @"\MIT\Data";
            string rutaArchivo = rutaData + @"\parameters.mit";
            crearDirectorio(rutaMIT);//Si no existe la carpeta raiz la crea.
            crearDirectorio(rutaData); //Si existe no crea nada, pero si no crea una carpeta
            if (!ExisteArchivo(rutaArchivo))
            {
                crearArchivo(rutaArchivo, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><configuracion></configuracion>", true);
            }
        }
        public static void crearDirectorio(string ruta)
        {
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
        }
        public static void crearArchivo(string ruta, string informacion, bool encriptar = false)
        {
            TextWriter archivo = new StreamWriter(ruta);

            archivo.Close();
        }
        public static bool ExisteArchivo(string rutaNumerosTxt)
        {
            return File.Exists(rutaNumerosTxt);
        }
        public static string GetDataXML(string xml, string tag)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return "";
            }
            string tagIni = "<" + tag + ">";
            string tagFin = "</" + tag + ">";
            int iIni = xml.IndexOf(tagIni);
            int iFin = xml.IndexOf(tagFin);
            if ((iIni >= 0) && (iFin > iIni))
            {
                return xml.Substring(iIni + tagIni.Length, iFin - iIni - tagIni.Length);
            }

            return "";
        }
        /// <summary>
        /// Deshabilita el boton Cerrar
        /// </summary>
        /// <param name="window">Form a deshabilitar el botón</param>
        public static void DeshabilitaBotonCerrar(Window window, bool habilita)
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            IntPtr hwnd = GetSystemMenu(helper.Handle, habilita);
            DeleteMenu(hwnd, SC_CLOSE, 0);
        }
        /// <summary>
        /// deshabilita el menu derecho (a lado del icono de la barra de titulo)
        /// </summary>
        /// <param name="window"></param>
        public static void DeshabilitaMenuSistema(Window window)
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            IntPtr hwnd = GetSystemMenu(helper.Handle, false);
            RemoveMenu(hwnd, 0, CloseByPosition);
            RemoveMenu(hwnd, 1, CloseByPosition);
            RemoveMenu(hwnd, 1, CloseByPosition);
            RemoveMenu(hwnd, 1, CloseByPosition);
        }
        public static void RedimensionaForm(Window window)
        {
            window.Width = Program.ResponsiveObj.GetMetrics(window.Width, "Width");
            window.Height = Program.ResponsiveObj.GetMetrics(window.Height, "Height");
            window.Left = Program.WidthScreen / 2 - window.Width / 2;
            window.Top = Program.HeightScreen / 2 - window.Height / 2 - 30;

            Panel mainContainer = (Panel)window.Content;
            UIElementCollection element = mainContainer.Children;
            List<FrameworkElement> lstElement = element.Cast<FrameworkElement>().ToList();
            var lstControl = lstElement.OfType<Control>();

            foreach (Control control in lstControl)
            {
                control.FontFamily = new FontFamily("Arial");
                control.FontSize = Program.ResponsiveObj.GetMetrics(control.FontSize);
                control.FontStyle = FontStyles.Normal;

                control.Width = Program.ResponsiveObj.GetMetrics(control.Width, "Width");
                control.Height = Program.ResponsiveObj.GetMetrics(control.Height, "Height");
                Thickness t = new Thickness(Program.ResponsiveObj.GetMetrics(control.Margin.Left, "Top"), Program.ResponsiveObj.GetMetrics(control.Margin.Top, "Top"), control.Margin.Right, control.Margin.Bottom);
                control.Margin = t;
            }

        }
        /// <summary>
        /// Retorna la version de la aplicacion
        /// </summary>
        /// <returns></returns>
        public static string GetVersionApp()
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            AssemblyName assemName = assem.GetName();
            Version ver = assemName.Version;
            return ver.Major + "." + ver.Minor + "." + ver.Revision;
        }
        /// <summary>
        /// Convierte una cadena en bool
        /// </summary>
        /// <param name="value">valor a convetir</param>
        /// <returns></returns>
        public static bool ConvierteStringToBoolean(string value)
        {
            switch (value.ToLower())
            {
                case "true":
                    return true;
                case "t":
                    return true;
                case "1":
                    return true;
                case "0":
                    return false;
                case "false":
                    return false;
                case "f":
                    return false;
                default:
                    return false;
            }
        }
        /// <summary>
        /// Convierte un string a un integer
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static int ConvierteStringToInt(string valor)
        {
            int valorInt;
            int.TryParse(valor, out valorInt);
            return valorInt;
        }
        /// <summary>
        /// Compare versions of form "1,2,3,4" or "1.2.3.4". Throws FormatException
        /// in case of invalid version.
        /// </summary>
        /// <param name="versionA">the first version</param>
        /// <param name="versionB">the second version</param>
        /// <returns>less than zero if strA is less than strB, equal to zero if
        /// strA equals strB, and greater than zero if strA is greater than strB</returns>
        public static bool CompararVersions(string versionA, string versionB)
        {
            bool esVersionMayor = false;
            Version vA = new Version(versionA.Replace(",", "."));
            Version vB = new Version(versionB.Replace(",", "."));

            if (vA.CompareTo(vB) > 0)
                esVersionMayor = true;

            return esVersionMayor;
        }
        public static BitmapImage CargaImagen(string pathFile, bool isRelative = false)
        {
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            if (isRelative)
                b.UriSource = new Uri(pathFile, UriKind.Relative);
            else
                b.UriSource = new Uri(pathFile);
            b.EndInit();
            return b;
        }
        /// <summary>
        /// Ejecuta un webservice
        /// </summary>
        /// <param name="url">Url del servicio</param>
        /// <param name="data">parametros</param>
        /// <returns></returns>
        public static string SendWS(string url, string data)
        {
            try
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(url);

                //User Agent
                ((HttpWebRequest)request).UserAgent = "pcpay";
                // Set the Method property of the request to POST.
                request.Method = "POST";
                // Create POST data and convert it to a byte array.
                String postData = data;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                String responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (WebException ex)
            {
                return "";
            }
        }
        public static string acentoOnString(string value, bool mayusculas = false)
        {
            if (!mayusculas)
            {
                value = value.Replace("aaa", "á");
                value = value.Replace("eee", "é");
                value = value.Replace("iii", "í");
                value = value.Replace("ooo", "ó");
                value = value.Replace("uuu", "ú");
                value = value.Replace("nnn", "ñ");
            }
            else
            {
                value = value.Replace("AAA", "Á");
                value = value.Replace("EEE", "É");
                value = value.Replace("III", "Í");
                value = value.Replace("OOO", "Ó");
                value = value.Replace("UUU", "Ú");
                value = value.Replace("NNN", "Ñ");

            }

            return value;
        }
        public static string Mid(string cadena, int inicio = 1, int longitud = 0)
        {
            if (longitud == 0)
            {

                longitud = cadena.Length - inicio;
            }

            if (inicio > cadena.Length)
            {
                return "";
            }
            else
            {
                return cadena.Substring(inicio - 1, longitud);
            }

        }
        internal static string Left(string param, int length)
        {
            string result = param.Substring(0, length);

            return result;
        }


        public static string readVersionImages(string url)
        {
            string line;
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, string.Format("{0}//imgVersion.tmp", System.IO.Path.GetTempPath()));

            System.IO.StreamReader file = new System.IO.StreamReader(string.Format("{0}//imgVersion.tmp", System.IO.Path.GetTempPath()));
            line = file.ReadLine();
            file.Close();
            return line;
        }
        public static string downloadImgV10(string url, string fileName)
        {
            string pathFile = string.Format("{0}{1}.zip", System.IO.Path.GetTempPath(), fileName);
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, pathFile);
            return pathFile;
        }
        public static void unzipFile(string pathFile, string pathExtract)
        {
            if (!Directory.Exists(pathExtract))
                Directory.CreateDirectory(pathExtract);
            ZipFile.ExtractToDirectory(pathFile, pathExtract);
        }

    }
}
