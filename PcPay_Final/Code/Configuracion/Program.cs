using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcPay.Code.Configuracion
{
    public static class Program
    {

        public static cpIntegracionEMV.clsCpIntegracionEMV cpIntegraEMV = new cpIntegracionEMV.clsCpIntegracionEMV();
        public static double WidthScreen { get; set; }
        public static double HeightScreen { get; set; }
        public static Responsive ResponsiveObj;


        public static string ip { get; set; }
        public static string ipPub { get; set; }
        public static string ipPrep { get; set; }
        public static string ipvip { get; set; }
        public static string ipPoints2 { get; set; }
        public static string ipfe { get; set; }
        public static string ipLoginInstalador { get; set; }
        public static string ipFirmaPanel { get; set; }
        public static string ipKeyWeb { get; set; }

        public static string VersionApp { get; set; }
        public static string VersionPcPay { get; set; }
        public static string NombreWebForm { get; set; }
        public static string NOMBRE_APP { get; set; }
        

    }
}
