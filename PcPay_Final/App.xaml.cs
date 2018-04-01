using cpIntegracionEMV;
using PcPay.Code.Configuracion;
using PcPay.Code.Usuario;
using PcPay.Forms.Formularios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PcPay_Final
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string NOMBRE_GENERAL { get; set; }
        private void Main(object sender, StartupEventArgs e)
        {
            string  nombreProceso = Process.GetCurrentProcess().ProcessName;
            int cantidad = Process.GetProcesses().Count(p => p.ProcessName == nombreProceso);
            if (cantidad > 1)
            {
                System.Windows.Forms.MessageBox.Show("La aplicación ya se encuentra en ejecución","",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Information);
                Application.Current.Shutdown();
                return;
            }



            this.NOMBRE_GENERAL = "MAIN";
            Program.WidthScreen = System.Windows.SystemParameters.PrimaryScreenWidth;
            Program.HeightScreen = System.Windows.SystemParameters.PrimaryScreenHeight;

            //se escribe en el archivo .config, la resolución del dispositivo
            ConfigurationManager.AppSettings["DESIGN_TIME_SCREEN_WIDTH"] = Program.WidthScreen.ToString();
            ConfigurationManager.AppSettings["DESIGN_TIME_SCREEN_HEIGHT"] = Program.HeightScreen.ToString();

            //Creacion de objetos
            Program.cpIntegraEMV = new clsCpIntegracionEMV();



            //Cargando la libreria de CryptoCOMLib y su clase CryptoTripleDES
            //Código de visual basic seria  CryptTools as New CryptoCOMLib.CryptoTripleDES;
            //Llenado de variables
            Program.ip = ConfigurationManager.AppSettings["ip"];
            Program.ipPub = ConfigurationManager.AppSettings["ipPub"];
            Program.ipPrep = ConfigurationManager.AppSettings["ipPrep"];
            Program.ipvip = ConfigurationManager.AppSettings["ipvip"];
            Program.ipPoints2 = ConfigurationManager.AppSettings["ipPoints2"];

            Program.ipfe = ConfigurationManager.AppSettings["ipfe"];
            Program.ipLoginInstalador = ConfigurationManager.AppSettings["ipLoginInstalador"];
            Program.ipFirmaPanel = ConfigurationManager.AppSettings["ipFirmaPanel"];
            Program.ipKeyWeb = ConfigurationManager.AppSettings["ipKeyWeb"];
            Program.VersionApp = ConfigurationManager.AppSettings["versionApp"];
            Program.VersionPcPay = ConfigurationManager.AppSettings["versionPcPay"];

            Program.NombreWebForm = "App.xaml";
            TypeUsuario.isCDP = true;

            Program.cpIntegraEMV.dbgEnabledLog(false);
           // generateLogs.LogsApp.setDirectoryLogs("c:\\logsPcPAy");
            Globales.strNombreFP = NOMBRE_GENERAL + ".Main()";
            string valor = Globales.GetSettingString("IdUnico");
            var aux = Globales.encryptString("TEMPORAL2", Globales.KEYCARD, true);

            if (valor == "")
            {
                Globales.SaveSettingString("Licence", "");
                Globales.SaveSettingString("ShwMsg", "");
                Globales.SaveSettingString("COM", "");
            }
            frmUpdate frm = new frmUpdate();

            Globales.BeginAplicacion();
        }
    }




}

namespace PcPay
{
    public static class Extension
    {
        public static void PerformClick(this Button btn)
        {
            btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
    public enum MITWindowSize
    {
        Normal,
        Medium,
        Large
    }
}
