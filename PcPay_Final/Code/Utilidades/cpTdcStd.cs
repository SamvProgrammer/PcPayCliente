using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcPay.Code.Utilidades
{
    class cpTdcStd
    {
        //Declaracion de las variables
        public static string STD_sResult { get; set; }
        private static string sEncKey { get; set; }
        public static string STD_getLicence()
        {
            return Globales.GetSettingString("License");
        }
        public static void STD_GeneratePrint()
        {
            sEncKey = "KEY CREDIT CARD KEY";
            STD_sResult = "";
            STD_ClearVariables();
            sSystemID = "";
            getSystemID();
            //Faltan mas cosas por agregar!!.. ver el apartado de visual basic...
            Globales.SaveSettingString("HSist", "-1");
        }

        private static void getSystemID()
        {

            //Codigo que todavía no se va a implementar nada!!.. se obtiene el ID del sistema... OJO

        }

        private static void STD_ClearVariables()
        {
            STD_usuario = "";
            STD_pwdUsuario = "";
            STD_usuarioAdministrador = "";
            STD_pwdAdministrador = "";

            STD_Importe = 0;
            STD_op = "";
            STD_tracks = "";
            STD_cc_num = "";
            STD_cc_mes = 0;
            STD_cc_anio = 0;
            STD_cc_cvv = "";
            STD_cc_name = "";
            STD_tipo_pago = "";

            STD_Comodin = "";

            authOri = "";
            justDev = "";

            avsNom1 = "";
            avsNom2 = "";
            avsPat = "";
            avsMat = "";
            avsApAdic = "";
            avsFecNac = "";
            avsRfc = "";


            avsCalle = "";
            avsNumExt = "";
            avsNumInt = "";
            avsCp = "";
            avsCol = "";
            avsDel = "";
            avsCd = "";
            avsEdo = "";
            avsLada = "";
            avsTel = "";
            avsCredHip = "";
            avsCredAut = "";
            avsNumRef = "";
            avsEmail = "";
            avsCel = "";
            avsNumCta = "";
            STD_sSerial = "";

            STD_merchant = "";
        }


        public static string STD_pwdUsuario { get; set; }

        public static string STD_usuario { get; set; }

        public static string STD_usuarioAdministrador { get; set; }

        public static string STD_pwdAdministrador { get; set; }

        public static int STD_Importe { get; set; }

        public static string STD_op { get; set; }

        public static string STD_tracks { get; set; }

        public static string STD_cc_num { get; set; }

        public static int STD_cc_mes { get; set; }

        public static int STD_cc_anio { get; set; }

        public static string STD_cc_cvv { get; set; }

        public static string STD_cc_name { get; set; }

        public static string STD_tipo_pago { get; set; }

        public static string STD_merchant { get; set; }

        public static string STD_Comodin { get; set; }

        public static string authOri { get; set; }

        public static string justDev { get; set; }

        public static string avsNom1 { get; set; }

        public static string avsNom2 { get; set; }

        public static string avsPat { get; set; }

        public static string avsMat { get; set; }

        public static string avsApAdic { get; set; }

        public static string avsFecNac { get; set; }

        public static string avsRfc { get; set; }

        public static string avsCalle { get; set; }

        public static string avsNumExt { get; set; }

        public static string avsNumInt { get; set; }

        public static string avsCp { get; set; }

        public static string avsCol { get; set; }

        public static string avsDel { get; set; }

        public static string avsCd { get; set; }

        public static string avsEdo { get; set; }

        public static string avsLada { get; set; }

        public static string avsTel { get; set; }

        public static string avsCredHip { get; set; }

        public static string avsCredAut { get; set; }

        public static string avsNumRef { get; set; }

        public static string avsEmail { get; set; }

        public static string avsCel { get; set; }

        public static string avsNumCta { get; set; }

        public static string STD_sSerial { get; set; }

        public static string sSystemID { get; set; }
    }

}
