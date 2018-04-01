using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using PcPay.Forms.Formularios;

namespace PcPay.Code.Utilidades
{

    #region clase que llama a las clases que actualiza el pcpay
    public class desintalador
    {
        public static  void actualizando()
        {
            try
            {                
                general actualizando = new desinstalando();
                actualizando.actualizar();
                actualizando = new instalando();
                actualizando.actualizar();
                Thread.Sleep(3000);
            }
            catch { 
            
            }
        }
    }
    #endregion


    #region clase abstracta que llama dependiendo a que desinstalar o instalar la aplicacion de pcpay....
    public abstract class general {

        public abstract void actualizar();
        protected bool handler { get; set; }
        public  List<string> getKeyPrograma(string productName)
        {
            string query = string.Format("select * from Win32_Product where Name='{0}'", productName);
            List<string> lista = new List<string>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                var temporal = searcher.Get();
                foreach (ManagementObject product in searcher.Get())
                {
                    lista.Add(product["IdentifyingNumber"].ToString());
                }
                return lista;
            }
        }
        protected  void ejecuta(string argumento)
        {
            handler = true;
            string carpetaSys = "";
            string fileName;
            string ruta;
            string exe;
            ruta = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            fileName = "msiexec.exe";

            if (Environment.Is64BitOperatingSystem)
                carpetaSys = "SysWOW64";
            else
                carpetaSys = "System32";

            exe = System.IO.Path.Combine(ruta + "\\" + carpetaSys, fileName);


            if (System.IO.File.Exists(exe))
            {
                try
                {
                    Process myProcess = new Process();
                    myProcess.StartInfo.FileName = exe;
                    myProcess.StartInfo.Arguments = argumento;
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.EnableRaisingEvents = true;
                    myProcess.Exited += new EventHandler(myProcess_Exited);
                    myProcess.Start();
                    while(handler){
                            
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("ocurrio un error al desinstalar versión anterior\nCódigo de error:" + ex.GetBaseException());
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No existe el programa Msiexec.exe, favor de descargarlo");
            }
        }

        private  void myProcess_Exited(object sender, EventArgs e)
        {
            handler = false;
        }
    }
    #endregion

    #region instalando el pcpay
    public class instalando:general {
        public override void actualizar() {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\PcPay.msi";
            string desinstalar = "/i " + path + " /qn";
            ejecuta(desinstalar);
        }
    }
    #endregion

    #region desinstalando el pcpay
    public class desinstalando:general 
    {   
        public override void actualizar()
        {
            List<string> lista = getKeyPrograma("PcPay");
            foreach(string item in lista){
                ejecuta("/x "+item+" /qn");
            }
        }
    }
#endregion
}