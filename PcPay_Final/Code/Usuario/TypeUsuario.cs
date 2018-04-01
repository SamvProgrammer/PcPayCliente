using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcPay.Code.Usuario
{
    public static class TypeUsuario
    {

        public static string MerchantBanda { get; set; }
        public static bool IsAmex { get; set; }
        public static bool isCDP { get; set; }

        public static string Id_Company { get; set; }
        public static string nb_company { get; set; }
        public static string nb_companystreet { get; set; }
        public static string Id_Branch { get; set; }
        public static string nb_branch { get; set; }
        public static string reference { get; set; }
        public static string country { get; set; }
        public static string Url { get; set; }
        public static string iata { get; set; }
        public static string usu { get; set; }
        public static int giro { get; set; }
        public static string Pass { get; set; }
        public static string strVersion { get; set; }            //Version de la aplicación
        public static string CadenaXML { get; set; }             //Cadena donde se almacena el perfil del usuario
        public static string strVoucher { get; set; }            //voucher de la transaccion HTML/Termico
        public static string strVoucherCoP { get; set; }         //voucher de centro de CENTEROFPAYMENTS
        public static string strSoloUnaHoja { get; set; }        //Imprimira solo una copia para impresoras Plug & Play
        public static bool bolCambiaPwd { get; set; }
        public static string strTipoLector { get; set; }
        public static string TipoImpresora { get; set; }         //1 = HTML   2 = Térmica     3 =Epson     0 / -1 = Ninguna
        public static string NumLicencia { get; set; }
        public static string SerieLector { get; set; }
        public static bool enviaCorreo { get; set; }

        public static string strRefReaDig { get; set; }          //Solo para la 0059
        public static string strFont { get; set; }

        public static bool IsTip { get; set; }
        public static string TipMsg { get; set; }
        public static string DrpUrl { get; set; }
        public static bool SaveLogTransaction { get; set; }                   //Verifica si se guarda el log
        public static int IsVIP { get; set; }                                    //DEFINE SI EL USUARIO ES VIP O NO
        public static bool ShowMsg { get; set; }

        public static string SerialReader { get; set; }
        public static string ProdsVtaServ { get; set; }


        public static string UserApp { get; set; }
        public static string PwdApp { get; set; }
        public static string IsADO { get; set; }

        public static string VersionPcPay { get; set; }
        public static string VersionPcPayActualizador { get; set; }
        public static bool IsAQ { get; set; }

        public static bool AddTableNum { get; set; }
        public static string IsEmvFull { get; set; }
        public static string PromoPay { get; set; }
        public static string Publicidad { get; set; }
        public static string Estado { get; set; }
        public static string Mcc { get; set; }
        public static string MenuMITCard { get; set; }
        public static string Points2 { get; set; }
        public static string DCC_afiliaciones { get; set; }

        public static bool UtilizarCapacidadTouch { get; set; }
        public static bool EnviarVoucherMail { get; set; }


        //public static string Update { get; set; }




        public static string strTipo { get; set; }
    }
}
