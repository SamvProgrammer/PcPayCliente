using PcPay.Code.Configuracion;
using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace PcPay.Forms.formularios
{
    /// <summary>
    /// Lógica de interacción para frmConectividad.xaml
    /// </summary>
    public partial class frmConectividad : Window
    {
        private string rutaImagen = string.Empty;
        private string drpPROD { set; get; }
        private string drpVIP { set; get; }
        private string SFTP { set; get; }
        private string NBdrpPROD { set; get; }
        private string NBdrpVIP { set; get; }
        private string NBSFTP { set; get; }
        private string strCadEncriptar { set; get; }


        private FtpWebRequest Ftp1 = null;
        private FtpWebResponse Ftpr = null;
        public frmConectividad()
        {
            InitializeComponent();
            //Carga();
            Owner = Globales.principal;
        }

        private void Carga()
        {
            try
            {
                string strAux = string.Empty;
                string validaNull = string.Empty;
                string strCadena = string.Empty;
                int i;
                string[] ArrNbCon = new string[3];
                string[] ArrDsCon = new string[3];

                Limpiar();

                strAux = TypeUsuario.DrpUrl;
                strCadena = strAux;
                string[] aux = strCadena.Split('|');
                string[] temp = new string[6];
                List<string> listaAux = new List<string>();
                foreach(string item in aux){
                    string[] aux2 = item.Split(',');
                    listaAux.Add(aux2[0]);
                    listaAux.Add(aux2[1]);
                }

                int contador = 0;
                foreach(string item in listaAux){
                    temp[contador] = item;
                    contador++;
                }


                ArrNbCon[0] = temp[0];
                ArrNbCon[1] = temp[2];
                ArrNbCon[2] = temp[4];

                ArrDsCon[0] = temp[1];
                ArrDsCon[1] = temp[3];
                ArrDsCon[2] = temp[5];
        

                drpPROD = ArrDsCon[0];
                drpVIP = ArrDsCon[1];
                SFTP = ArrDsCon[2];

        
                if (drpPROD == "https://200.57.86.60")
                {
                    drpPROD = "https://ssl2.e-pago.com.mx";
                }

                if (drpVIP == "https://200.57.86.61")
                {
                    drpVIP = "https://vip2.e-pago.com.mx";
                }

                NBdrpPROD = ArrNbCon[0];
                NBdrpVIP = ArrNbCon[1];
                NBSFTP = ArrNbCon[2];

                lbDescDRPProd.Content = NBdrpPROD;
                lbDescDRPVIP.Content = NBdrpVIP;
                lbDescSFTP.Content = NBSFTP;

                fraProd.Content = Program.ip;
                fraProdDRP.Content = drpPROD;
                fraVIP.Content = SFTP;
      
            }
            catch
            {

            }

            Style img1;
            img1 = FindResource("maso") as Style;
            imagen1.Style = img1;
            imagen2.Style = img1;
            imagen3.Style = img1;
        }

        private void CmdCompProd_Click(object sender, RoutedEventArgs e)
        {
            // MousePointer = vbHourglass
            Style imagenConectividad = null;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            CmdCompProd.IsEnabled = false;
            strCadEncriptar = "op=disponibilidad";
            cpHTTP.cpHTTP_Clear();
            cpHTTP.cpHTTP_sURL_cpCUCT = Program.ip + "/pgs/pcpayAgencia";
            //No seguro
            cpHTTP.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);
            
            if (cpHTTP.cpHTTP_SendcpCUCT())
            {

                if (!string.IsNullOrEmpty(Utils.GetDataXML(cpHTTP.cpHTTP_sResult, "response")))
                {
                    //ImgProdOK.Visibility= System.Windows.Visibility.Visible;
                    //ImgProdWarning.Visibility = System.Windows.Visibility.Hidden;
                    //ImgProd.Visibility = System.Windows.Visibility.Hidden;
                    //ImgProdNO.Visibility = System.Windows.Visibility.Hidden;
                    CmdCompProd.Content = "Actualizar";
                    imagenConectividad = FindResource("bien") as Style;
                    imagen1.Style = imagenConectividad;
                }
                else
                {
                    //ImgProdOK.Visibility = System.Windows.Visibility.Hidden;
                    //ImgProdWarning.Visibility = System.Windows.Visibility.Hidden;
                    //ImgProd.Visibility = System.Windows.Visibility.Hidden;
                    //ImgProdNO.Visibility = System.Windows.Visibility.Visible;
                    imagenConectividad = FindResource("mal") as Style;
                    imagen1.Style = imagenConectividad;
                }
            }
            else
            {
                //ImgProdOK.Visibility = System.Windows.Visibility.Hidden;
                //ImgProd.Visibility = System.Windows.Visibility.Hidden;

                //ImgProdWarning.Visibility = System.Windows.Visibility.Hidden;
                //ImgProdNO.Visibility = System.Windows.Visibility.Visible;
                imagenConectividad = FindResource("maso") as Style;
                imagen1.Style = imagenConectividad;
            }

            //  MousePointer = vbNormal;
            Mouse.OverrideCursor = null;
            CmdCompProd.IsEnabled = true;


        }

        private void CmdCompDRP_Click(object sender, RoutedEventArgs e)
        {
            //MousePointer = vbHourglass
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Style img;

            CmdCompDRP.IsEnabled = true;
            strCadEncriptar = "op=disponibilidad";
            cpHTTP.cpHTTP_Clear();
            cpHTTP.cpHTTP_sURL_cpCUCT = drpPROD + "/pgs/pcpayAgencia";

            //No seguro
            cpHTTP.cpHTTP_cadena1 = "enc=" + Globales.encryptString(strCadEncriptar, Globales.KEY_RC4, true);

            if (cpHTTP.cpHTTP_SendcpCUCT())
            {

                if (!string.IsNullOrEmpty(Utils.GetDataXML(cpHTTP.cpHTTP_sResult, "response")))
                {
                    //ImgProdDRPOK.Visibility = System.Windows.Visibility.Visible;
                    //ImgProdDRPWarning.Visibility = System.Windows.Visibility.Hidden;
                    //ImgProdDRP.Visibility = System.Windows.Visibility.Hidden;
                    //ImgProdDRPNO.Visibility = System.Windows.Visibility.Hidden;
                    img = FindResource("bien") as Style;
                    imagen2.Style = img;
                    CmdCompDRP.Content = "Actualizar";
                }
                else
                {
                    //ImgProdDRPOK.Visibility = System.Windows.Visibility.Hidden;
                    //ImgProdDRPWarning.Visibility = System.Windows.Visibility.Hidden;
                    //ImgProdDRP.Visibility = System.Windows.Visibility.Hidden;
                    //ImgProdDRPNO.Visibility = System.Windows.Visibility.Visible;
                    img = FindResource("mal") as Style;
                    imagen2.Style = img;
                }
            }
            else
            {
                //ImgProdDRPOK.Visibility = System.Windows.Visibility.Hidden;
                //ImgProdDRP.Visibility = System.Windows.Visibility.Hidden;

                //ImgProdDRPWarning.Visibility = System.Windows.Visibility.Hidden;
                //ImgProdDRPNO.Visibility = System.Windows.Visibility.Visible;
                img = FindResource("maso") as Style;
                imagen2.Style = img;
            }

            //  MousePointer = vbNormal;
            CmdCompProd.IsEnabled = true;



            Mouse.OverrideCursor = null;

        }

        private void CmdSFTP_Click(object sender, RoutedEventArgs e)
        {
            CheckSFTP();
        }

        private void CheckSFTP()
        {
           try{

               Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            //Dim Ftp1 As FTP
            string strIP = string.Empty;
               string strCarpeta = string.Empty;
               string strUsers = string.Empty;
               string strPass = string.Empty;
               string strFile = string.Empty;

              cpHTTP.cpHTTP_Clear();
                cpHTTP.cpHTTP_cadena1 = "Mike";
               cpHTTP.cpHTTP_sURL_cpCUCT = "https://ssl.e-pago.com.mx/pgs/jsp/cpagos/actualizador.jsp";
    
                if(cpHTTP.cpHTTP_SendcpCUCT() ){
                    string strAux = string.Empty;
                    strAux = cpHTTP.cpHTTP_sResult;
                    strIP = Utils.Mid(strAux, 1,strAux.IndexOf( "|") );
                    strAux = Utils.Mid(strAux, strIP.Length + 2);
                    strCarpeta = Utils.Mid(strAux, 1, strAux.IndexOf("|") );
                    strAux =Utils.Mid(strAux, strCarpeta.Length + 2);
                    strUsers = Utils.Mid(strAux, 1, strAux.IndexOf("|") );
                    strAux = Utils.Mid(strAux, strUsers.Length + 2);
                    strPass = strAux.Trim();
                }

                try
                {
                    string host = "ftp://" + strIP;
                    Ftp1 = (FtpWebRequest)FtpWebRequest.Create(host);// + "/" + strCarpeta);
                    /* Log in to the FTP Server with the User Name and Password Provided */
                    Ftp1.Credentials = new NetworkCredential(strUsers, strPass);
                    /* When in doubt, use these options */

                    Ftp1.Method = WebRequestMethods.Ftp.DownloadFile;
                    /* Establish Return Communication with the FTP Server */
                    Ftpr = (FtpWebResponse)Ftp1.GetResponse();
                    /* Get the FTP Server's Response Stream */
                    Stream ftpStream = Ftpr.GetResponseStream();

                }
                catch { }
                Style img;
                if (Ftpr!=null)
                { 
                        //ImgSftpOK.Visibility = System.Windows.Visibility.Hidden;
                        //ImgSftpWarning.Visibility = System.Windows.Visibility.Hidden;
                        //ImgSftp.Visibility = System.Windows.Visibility.Hidden;
                        //ImgSftpNO.Visibility = System.Windows.Visibility.Visible;
                    CmdSFTP.Content = "Actualizar";
                    img = FindResource("bien") as Style;
                    imagen3.Style = img;
                }
                else{
                        //ImgSftpOK.Visibility = System.Windows.Visibility.Visible;
                        //ImgSftpWarning.Visibility = System.Windows.Visibility.Hidden;
                        //ImgSftp.Visibility = System.Windows.Visibility.Hidden;
                        //ImgSftpNO.Visibility = System.Windows.Visibility.Hidden;
                        CmdSFTP.Content = "Comprobar";
                        img = FindResource("mal") as Style;
                        imagen3.Style = img;
                
                }



               lbDescSFTP.Content = NBSFTP;

            //    Set Ftp1 = New FTP
            //    With Ftp1
            //        .Inicializar Me
            //        .PassWord = strPass
            //        .Usuario = strUsers
            //        .Servidor = strIP
            //        .CambiarDirectorio " & strCarpeta & "
            //        If .ConectarFtp(lbDescSFTP) = False Then
            //            ImgSftpOK.Visible = False
            //            ImgSftpWarning.Visible = False
            //            ImgSftp.Visible = False
            //            ImgSftpNO.Visible = True
            //        Else
            //            ImgSftpOK.Visible = True
            //            ImgSftpWarning.Visible = False
            //            ImgSftp.Visible = False
            //            ImgSftpNO.Visible = False
            //            CmdSFTP.Caption = "Actualizar"
            //        End If
            //    End With
            //    lbDescSFTP = NBSFTP
            //Exit Sub
           }
            catch(Exception Err){
             System.Windows.MessageBox.Show("Descripcion: " + Err.Message,"Error");
   // MsgBox "Error: " & Err.Number & vbCrLf & "Descripcion: " & Err.Description, vbCritical, "MIT Update Para PcPay"

            }
           Mouse.OverrideCursor = null;
        }

        private void CmdCompVIP_Click(object sender, RoutedEventArgs e)
        {



            CmdCompVIP.IsEnabled = false;
            strCadEncriptar = "op=disponibilidad";
            cpHTTP.cpHTTP_Clear();
            cpHTTP.cpHTTP_sURL_cpCUCT = "http" + Program.ipvip + "/pgs/pcpayAgencia";

            //No seguro
            cpHTTP.cpHTTP_cadena1 = "enc=" + EncryptC.EncryptRC4(strCadEncriptar, modMIT.KEY_RC4, true);

            if (cpHTTP.cpHTTP_SendcpCUCT())
            {

                if (!string.IsNullOrEmpty(Utils.GetDataXML(cpHTTP.cpHTTP_sResult, "response")))
                {
                    //ImgVipOK.Visibility = System.Windows.Visibility.Visible;
                    //ImgVIPWarning.Visibility = System.Windows.Visibility.Hidden;
                    //ImgVip.Visibility = System.Windows.Visibility.Hidden;
                    //ImgVipNO.Visibility = System.Windows.Visibility.Hidden;
                    Globales.MessageBoxMit("Verde");
                    CmdCompVIP.Content = "Actualizar";
                }
                else
                {
                    //ImgVipOK.Visibility = System.Windows.Visibility.Hidden;
                    //ImgVIPWarning.Visibility = System.Windows.Visibility.Hidden;
                    //ImgVip.Visibility = System.Windows.Visibility.Hidden;
                    //ImgVipNO.Visibility = System.Windows.Visibility.Visible;
                    Globales.MessageBoxMit("Verde");
                }
            }
            else
            {
                //ImgVipOK.Visibility = System.Windows.Visibility.Hidden;
                //ImgVip.Visibility = System.Windows.Visibility.Hidden;

                //ImgVIPWarning.Visibility = System.Windows.Visibility.Hidden;
                //ImgVipNO.Visibility = System.Windows.Visibility.Visible;
            }

            //  MousePointer = vbNormal;
            CmdCompVIP.IsEnabled = true;




        }

        private void CmdCompVIPDRP_Click(object sender, RoutedEventArgs e)
        {

            CmdCompVIPDRP.IsEnabled = false; ;
            strCadEncriptar = "op=disponibilidad";
            cpHTTP.cpHTTP_Clear();
            cpHTTP.cpHTTP_sURL_cpCUCT = "http" + Globales.drpVIP + "/pgs/pcpayAgencia";

            //No seguro
            cpHTTP.cpHTTP_cadena1 = "enc=" + EncryptC.EncryptRC4(strCadEncriptar, modMIT.KEY_RC4, true);

            if (cpHTTP.cpHTTP_SendcpCUCT())
            {

                if (!string.IsNullOrEmpty(Utils.GetDataXML(cpHTTP.cpHTTP_sResult, "response")))
                {
                    //ImgVipDRPOK.Visibility = System.Windows.Visibility.Visible;
                    //ImgVIPDRPWarning.Visibility = System.Windows.Visibility.Hidden;
                    //ImgVipDRP.Visibility = System.Windows.Visibility.Hidden;
                    //ImgVipDRPNO.Visibility = System.Windows.Visibility.Hidden;

                    CmdCompVIPDRP.Content = "Actualizar";
                }
                else
                {
                    //ImgVipDRPOK.Visibility = System.Windows.Visibility.Hidden;
                    //ImgVIPDRPWarning.Visibility = System.Windows.Visibility.Hidden;
                    //ImgVipDRP.Visibility = System.Windows.Visibility.Hidden;
                    //ImgVipDRPNO.Visibility = System.Windows.Visibility.Visible;
                }
            }
            else
            {
                //ImgVipDRPOK.Visibility = System.Windows.Visibility.Hidden;
                //ImgVipDRP.Visibility = System.Windows.Visibility.Hidden;

                //ImgVIPDRPWarning.Visibility = System.Windows.Visibility.Hidden;
                //ImgVipDRPNO.Visibility = System.Windows.Visibility.Visible;
            }

            //  MousePointer = vbNormal;
            CmdCompVIPDRP.IsEnabled = true;


        }

        private void Command1_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Limpiar()
        {
            double ALTO = this.Height;
            if (Utils.GetDataXML(TypeUsuario.CadenaXML, "vip") == "1")
            {
                //Me.Height = 5115
                //VIP.Visibility = Visibility.Visible;
               // this.Height = ALTO;
            }
            else
            {

               // this.Height = ALTO / 2;
            }

            //ImgProdOK.Visibility = System.Windows.Visibility.Hidden;
            //ImgProd.Visibility = System.Windows.Visibility.Visible;
            //ImgProdNO.Visibility = System.Windows.Visibility.Hidden;
            //ImgProdWarning.Visibility = System.Windows.Visibility.Hidden;
            

            //ImgProdDRPOK.Visibility = System.Windows.Visibility.Hidden;
            //ImgProdDRP.Visibility = System.Windows.Visibility.Visible;
            //ImgProdDRPNO.Visibility = System.Windows.Visibility.Hidden;
            //ImgProdDRPWarning.Visibility = System.Windows.Visibility.Hidden;
            

            //ImgVipOK.Visibility = System.Windows.Visibility.Hidden;
            //ImgVip.Visibility = System.Windows.Visibility.Visible;
            //ImgVipNO.Visibility = System.Windows.Visibility.Hidden;
            //ImgVIPWarning.Visibility = System.Windows.Visibility.Hidden;
            

            //ImgVipDRPOK.Visibility = System.Windows.Visibility.Hidden;
            //ImgVipDRP.Visibility = System.Windows.Visibility.Visible;
            //ImgVipDRPNO.Visibility = System.Windows.Visibility.Hidden;
            //ImgVIPDRPWarning.Visibility = System.Windows.Visibility.Hidden;
            lbVIPDRP.Content = "";

            //ImgSftpOK.Visibility = System.Windows.Visibility.Hidden;
            //ImgSftp.Visibility = System.Windows.Visibility.Visible;
            //ImgSftpNO.Visibility = System.Windows.Visibility.Hidden;
            //ImgSftpWarning.Visibility = System.Windows.Visibility.Hidden;
            

            CmdCompProd.Content = "Comprobar";
            CmdCompDRP.Content = "Comprobar";
            CmdCompVIP.Content = "Comprobar";
            CmdCompVIPDRP.Content = "Comprobar";
            CmdSFTP.Content = "Comprobar";

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Carga();
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



    }
}
