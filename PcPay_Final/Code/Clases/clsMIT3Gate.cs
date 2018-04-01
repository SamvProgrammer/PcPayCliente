using PcPay.Code.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PcPay.Code.Clases
{
    public class clsMIT3Gate
    {
        private string TGURL { set; get; }
        private string TGXML { set; get; }

        private string TGCdOriginadora { set; get; }
        private string TGNbOriginadora { set; get; }
        private string TGReferencia { set; get; }
        private string TGImporte { set; get; }
        private string TGTarjeta { set; get; }
        private string TGEfectivo { set; get; }

        private string TGUser { set; get; }
        private string TGPwd { set; get; }
        private string TGCompany { set; get; }
        private string TGBranch { set; get; }


        private string TGThPagaComision { set; get; }
        private string TGRefComision { set; get; }
        private string TGNuComision { set; get; }
        private string TGMerchantComision { set; get; }
        private string TGUsrComision { set; get; }
        private string TGPwdComision { set; get; }
        private string TGCompanyComision { set; get; }
        private string TGBranchComision { set; get; }

        private string TGCatBancos { set; get; }
        private string TGXmlBancos { set; get; }

        private string TGRecibo { set; get; }
        private string TGDsError { set; get; }



        private string Cd_empresa { set; get; }
        private string Nb_empresa { set; get; }
        private string CapturaImporte { set; get; }
        private string TipoCadena { set; get; }
        private string TamanoCadena { set; get; }
        private string Cd_convenio { set; get; }



        private const string KTGAPPWEB = "#<?V>E=ZqRmW*:9%rH7";
        MITWebService WS = new MITWebService();



        public void snd3GateReference(string Bs_User,
                                        string Bs_Pwd,
                                        string Bs_Company,
                                        string Bs_Branch,
                                        string Tx_Reference)
        {

            try
            {
                string strXml = string.Empty;
                dbgClearDLL();

                strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                strXml += "<REQ3GATE>";
                strXml += "<opcion>consultareferencia</opcion>";
                strXml += "<id_company>" + Bs_Company + "</id_company>";
                strXml += "<id_branch>" + Bs_Branch + "</id_branch>";
                strXml += "<user>" + Bs_User + "</user>";
                strXml += "<pwd>" + Bs_Pwd + "</pwd>";
                strXml += "<referencia>" + Tx_Reference + "</referencia>";


                strXml += "</REQ3GATE>";

                strXml = EncryptC.EncryptTripleDES(strXml, KTGAPPWEB);

                WS.WS_Url = TGURL + "/pgs/services/getReferencias";

                WS.WS_Action = "http://server.referencias.webservice.cpagos"; //'http://server.referencias.webservice.cpagos
                WS.ClearWS();
                WS.WS_Params = strXml;
                WS.WS_nbParams = "in0";
                WS.WS_Method = "getReferencias";

                WS.MITWebServices(WS.WS_Params, WS.WS_nbParams, WS.WS_Method);

                WS.WS_Response = Utils.GetDataXML(WS.WS_Response, "ns1:out");
                WS.WS_Response = EncryptC.DecryptTripleDES(WS.WS_Response, KTGAPPWEB);

                setRspTGXML(WS.WS_Response);

            }
            catch { }



        }



        public void snd3GateSmartReference(string Bs_User,
                                        string Bs_Pwd,
                                        string Bs_Company,
                                        string Bs_Branch,
                                        string Tx_Reference,
                                        string Tx_Originadora,
                                        string Tx_Amount = "")
        {
            try
            {
                string strXml = string.Empty;
                dbgClearDLL();
                strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                strXml += "<REQ3GATE>";
                strXml += "<opcion>consultareferenciasmart</opcion>";
                strXml += "<id_company>" + Bs_Company + "</id_company>";
                strXml += "<id_branch>" + Bs_Branch + "</id_branch>";
                strXml += "<user>" + Bs_User + "</user>";
                strXml += "<pwd>" + Bs_Pwd + "</pwd>";
                strXml += "<referencia>" + Tx_Reference + "</referencia>";
                strXml += "<originadora>" + Tx_Originadora + "</originadora>";

                //'Se agrega para enviar el monto
                if (!vacio(Tx_Amount))
                {


                    //strXml = strXml + "<importe>" + Format(Tx_Amount, "##########.00") + "</importe>";
                }

                strXml += "</REQ3GATE>";

                strXml = EncryptC.EncryptTripleDES(strXml, KTGAPPWEB);


                WS.WS_Url = TGURL + "/pgs/services/getReferencias";

                WS.WS_Action = "http://server.referencias.webservice.cpagos";// 'http://server.referencias.webservice.cpagos
                WS.ClearWS();
                WS.WS_Params = strXml;
                WS.WS_nbParams = "in0";
                WS.WS_Method = "getReferencias";

                WS.MITWebServices(WS.WS_Params, WS.WS_nbParams, WS.WS_Method);
                WS.WS_Response = EncryptC.DecryptTripleDES(WS.WS_Response, KTGAPPWEB);

                setRspTGXML(WS.WS_Response);
                //'Aqui parcear XML de Respuesta
            }
            catch { }
        }




        public void dbgSetTGXML(string XML)
        {
            try
            {
                string AuxXML = string.Empty;

                AuxXML = Utils.GetDataXML(XML, "comisionth");

                setRspTGCdOriginadora(Utils.GetDataXML( XML,"cd_originadora"));
                setRspTGNbOriginadora(Utils.GetDataXML( XML,"nb_originadora"));
                setRspTGReferencia(Utils.GetDataXML(XML, "referencia"));
                setRspTGImporte(Utils.GetDataXML("importe", XML));
                setRspTGTarjeta(Utils.GetDataXML("tarjeta", XML));
                setRspTGEfectivo(Utils.GetDataXML("efectivo", XML));

                setRspTGThPagaComision(Utils.GetDataXML( XML,"thpagacomision"));
                setRspTGRefComision(Utils.GetDataXML(AuxXML, "referencia"));
                setRspTGNuComision(Utils.GetDataXML( AuxXML,"nu_comision"));
                setRspTGMerchantComision(Utils.GetDataXML( AuxXML,"merchant"));

                setRspTGUser(Utils.GetDataXML(XML,"usr"));
                setRspTGPwd(Utils.GetDataXML(XML,"psw"));
                setRspTGCompany(Utils.GetDataXML(XML, "company"));
                setRspTGBranch(Utils.GetDataXML(XML, "branch"));




                setRspTGUsrComision(Utils.GetDataXML(AuxXML, "usr"));
                setRspTGPwdComision(Utils.GetDataXML( AuxXML,"psw"));
                setRspTGCompanyComision(Utils.GetDataXML(AuxXML, "company"));
                setRspTGBranchComision(Utils.GetDataXML(AuxXML, "branch"));

                setRspTGCatBancos(Utils.GetDataXML(XML, "catbancos"));
                setRspTGxmlBancos(Utils.GetDataXML(XML, "ventas3gate"));
                setRspTGDsError(Utils.GetDataXML(XML, "dserror"));




            }
            catch { }


        }


        public void dbgGetBancos(ComboBox cboContenedor, string catBancos)
        {
            try
            {
                string strAux = string.Empty;
                int I;
                strAux = catBancos;
                //With cboContenedor
                //    I = 0
                //    .Clear
                //    Do While Len(catBancos) > 0
                //        strAux = Mid(catBancos, 1, InStr(1, catBancos, "|") - 1)
                //        .AddItem Mid(strAux, InStr(1, strAux, ",") + 1)
                //        .ItemData(I) = CInt(Mid(strAux, 1, InStr(1, strAux, ",") - 1))
                //        catBancos = Mid(catBancos, InStr(1, catBancos, "|") + 1)
                //        I = I + 1
                //    Loop
                //End With

            }
            catch { }
        }


        public void sndTGGetRecibo(string Bs_User,
                                string Bs_Pwd,
                                string Bs_Company,
                                string Bs_Branch,
                                string TG_CdOriginante,
                                string TG_Referencia,
                                string TG_NuOperacion)
        {
            try
            {
                string strXml = string.Empty;
                dbgClearDLL();

                strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                strXml += "<REQGETRECIBO>";
                strXml += "<id_company>" + Bs_Company + "</id_company>";
                strXml += "<id_branch>" + Bs_Branch + "</id_branch>";
                strXml += "<user>" + Bs_User + "</user>";
                strXml += "<pwd>" + Bs_Pwd + "</pwd>";
                strXml += "<cd_originante>" + TG_CdOriginante + "</cd_originante>";
                strXml += "<referencia>" + TG_Referencia + "</referencia>";
                strXml += "<no_operacion>" + TG_NuOperacion + "</no_operacion>";
                strXml += "</REQGETRECIBO>";

                strXml = EncryptC.EncryptTripleDES(strXml, KTGAPPWEB);


                WS.WS_Url = TGURL + "/pgs/services/getReferencias";

                WS.WS_Action = "http://server.referencias.webservice.cpagos"; // 'http://server.referencias.webservice.cpagos
                WS.ClearWS();
                WS.WS_Params = strXml;
                WS.WS_nbParams = "in0";
                WS.WS_Method = "getRecibo";

                WS.MITWebServices(WS.WS_Params, WS.WS_nbParams, WS.WS_Method);
                WS.WS_Response = EncryptC.DecryptTripleDES(WS.WS_Response, KTGAPPWEB);
                setRspTGXML(WS.WS_Response);
                setRspTGRecibo(Utils.GetDataXML(TGXML, "recibo"));

            }
            catch
            {
                dbgClearDLL();
            }
        }



        //'Funciones implementadas para el nuevo flujo 3Gate (21-12-2009)
        public void sndTGGetEmpresas(string Bs_Company, string Bs_Branch, string Bs_User, string Bs_Pwd)
        {
            try
            {
                string strXml = string.Empty;
                dbgClearDLL();
                strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                strXml += "<REQ3GATE>";
                strXml += "<opcion>empconv</opcion>";
                strXml += "<id_company>" + Bs_Company + "</id_company>";
                strXml += "<id_branch>" + Bs_Branch + "</id_branch>";
                strXml += "<user>" + Bs_User + "</user>";
                strXml += "<pwd>" + Bs_Pwd + "</pwd>";
                strXml += "</REQ3GATE>";

                strXml = EncryptC.EncryptTripleDES(strXml, KTGAPPWEB);

                WS.WS_Url = TGURL + "/pgs/services/getReferencias";
                WS.WS_Action = "http://server.referencias.webservice.cpagos";
                WS.ClearWS();
                WS.WS_Params = strXml;
                WS.WS_nbParams = "in0";
                WS.WS_Method = "getInfoEmpresas";

                WS.MITWebServices(WS.WS_Params, WS.WS_nbParams, WS.WS_Method);
                WS.WS_Response = EncryptC.DecryptTripleDES(WS.WS_Response, KTGAPPWEB);

                setRspTGXML(WS.WS_Response);

            }
            catch { }
        }

        public void snd3GateRefAmount(string Bs_User,
                                string Bs_Pwd,
                                string Bs_Company,
                                string Bs_Branch,
                                string Tx_Reference,
                                string Tx_Amount,
                                string Tg_CdOriginadora,
                                string Tg_CdConvenio)
        {
            try
            {
                string strXml = string.Empty;
                dbgClearDLL();
                strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                strXml += "<REQ3GATE>";
                strXml += "<opcion>consultareferencia</opcion>";
                strXml += "<id_company>" + Bs_Company + "</id_company>";
                strXml += "<id_branch>" + Bs_Branch + "</id_branch>";
                strXml += "<user>" + Bs_User + "</user>";
                strXml += "<pwd>" + Bs_Pwd + "</pwd>";
                strXml += "<referencia>" + Tx_Reference + "</referencia>";
                strXml += "<importe>" + Tx_Amount + "</importe>";
                strXml += "<cd_originadora>" + Tg_CdOriginadora + "</cd_originadora>";
                strXml += "<cd_convenio>" + Tg_CdConvenio + "</cd_convenio>";
                strXml += "<tipocadena>" + getTipoCadena() + "</tipocadena>";
                strXml += "</REQ3GATE>";

                strXml = EncryptC.EncryptTripleDES(strXml, KTGAPPWEB);

                WS.WS_Url = TGURL + "/pgs/services/getReferencias";
                //'WS.WS_Url = "http://172.20.25.28:8080/obtenReferencias/services/getReferencias";
                WS.WS_Action = "http://server.referencias.webservice.cpagos"; //'http://server.referencias.webservice.cpagos
                WS.ClearWS();
                WS.WS_Params = strXml;
                WS.WS_nbParams = "in0";
                WS.WS_Method = "getReferencias";

                WS.MITWebServices(WS.WS_Params, WS.WS_nbParams, WS.WS_Method);
                WS.WS_Response = EncryptC.DecryptTripleDES(WS.WS_Response, KTGAPPWEB);

                setRspTGXML(WS.WS_Response);

            }
            catch { }
        }


        public void setXmlEmpresa(string XML)
        {
            setCd_empresa(Utils.GetDataXML(XML, "cd_empresa"));
            setNb_empresa(Utils.GetDataXML(XML, "nb_empresa"));
            setCd_convenio(Utils.GetDataXML(XML, "cd_convenio"));
            setCapturaImporte(Utils.GetDataXML(XML, "capturaimporte"));
            setTipoCadena(Utils.GetDataXML(XML, "tipocadena"));
            setTamanoCadena(Utils.GetDataXML(XML, "tamanocadena"));
        }

        private void setTamanoCadena(string p)
        {
            TamanoCadena = p;
        }
        public string getTamanoCadena()
        {
            return TamanoCadena.Replace("null", "");
        }
        private void setTipoCadena(string p)
        {
            TipoCadena = p;
        }


        private void setCapturaImporte(string p)
        {
            CapturaImporte = p;
        }

        public string getCapturaImporte()
        {
            return CapturaImporte;
        }
        private void setCd_convenio(string p)
        {
            Cd_convenio = p;
        }

        public string getCd_convenio()
        {
            return Cd_convenio;
        }

        private void setNb_empresa(string p)
        {
            Nb_empresa = p;
        }

        public string getNb_empresa()
        {
            return Nb_empresa;
        }

        private void setCd_empresa(string p)
        {
            Cd_empresa = p;
        }

        public string getCd_empresa()
        {
            return Cd_empresa;
        }




        private string getTipoCadena()
        {
            return TipoCadena.Replace("null", "");
        }



        private void setRspTGRecibo(string p)
        {
            TGRecibo = p;
        }

        public string getRspTGRecibo()
        {
            return TGRecibo;
        }


        private void setRspTGDsError(string p)
        {
            TGDsError = p;
        }

        public string getRspTGDsError()
        {
            return TGDsError.Replace("null", "");
        }

        private void setRspTGxmlBancos(string p)
        {
            TGXmlBancos = p;
        }

        public string getRspTGxmlBancos()
        {
            return TGXmlBancos;
        }

        private void setRspTGCatBancos(string p)
        {
            TGCatBancos = p;
        }

        public string getRspTGCatBancos()
        {
            return TGCatBancos;
        }

        private void setRspTGBranchComision(string p)
        {
            TGBranchComision = p;
        }

        private void setRspTGCompanyComision(string p)
        {
            TGCompanyComision = p;
        }

        public string getRspTGBranchComision()
        {
            return TGBranchComision;
        }

        public string getRspTGCompanyComision()
        {
            return TGCompanyComision;
        }

        public string getRspTGCompany()
        {
            return TGCompany.Replace("null", "");
        }

        private void setRspTGPwdComision(string p)
        {
            TGPwdComision = p;
        }

        public string getRspTGPwdComision()
        {
            return TGPwdComision;
        }

        private void setRspTGUsrComision(string p)
        {
            TGUsrComision = p;
        }


        public string getRspTGUsrComision()
        {
            return TGUsrComision;
        }

        private void setRspTGBranch(string p)
        {
            TGBranch = p;
        }

        public string getRspTGBranch()
        {
            return TGBranch.Replace("null", "");
        }

        private void setRspTGCompany(string p)
        {
            TGCompany = p;
        }

        private void setRspTGPwd(string p)
        {
            TGPwd = p;
        }
        public string getRspTGPwd()
        {
            return TGPwd.Replace("null", "");
        }

        private void setRspTGUser(string p)
        {
            TGUser = p;
        }

        public string getRspTGUser()
        {
            return TGUser.Replace("null", "");
        }


        private void setRspTGMerchantComision(string p)
        {
            TGMerchantComision = p;
        }
        public string getRspTGMerchantComision()
        {
            return TGMerchantComision;
        }

        private void setRspTGNuComision(string p)
        {
            TGNuComision = p;
        }

        public string getRspTGNuComision()
        {
            return TGNuComision;
        }
        private void setRspTGRefComision(string p)
        {
            TGRefComision = p;
        }

        private void setRspTGThPagaComision(string p)
        {
            TGThPagaComision = p;
        }

        private void setRspTGEfectivo(string p)
        {
            TGEfectivo = p;
        }

        public string getRspTGRefComision()
        {
            return TGRefComision;
        }

        public string getRspTGThPagaComision()
        {
            return TGThPagaComision;
        }

        private void setRspTGTarjeta(string p)
        {
            TGTarjeta = p;
        }

        public string getRspTGTarjeta()
        {
            return TGTarjeta;
        }

        private void setRspTGImporte(string p)
        {
            TGImporte = p;
        }
        public string getRspTGImporte()
        {
            return TGImporte;
        }

        private void setRspTGReferencia(string p)
        {
            TGReferencia = p;
        }

        private void setRspTGNbOriginadora(string p)
        {
            TGNbOriginadora = p;
        }
        public string getRspTGCdOriginadora()
        {
            return TGCdOriginadora.Replace("null", "");
        }

        private void setRspTGCdOriginadora(string p)
        {
            TGCdOriginadora = p;
        }

        public string getRspTGReferencia()
        {
            return TGReferencia.Replace("null", "");
        }





        //'Funciones de Respuesta 3Gate
        public string getRspTGNbOriginadora()
        {
            return TGNbOriginadora.Replace("null", "");
        }

        public string dbgTGHTMLReport(string User,
                            string Pwd,
                            string Id_Branch,
                            string Id_Company)
        {
            string strParametros = string.Empty;

            strParametros = "&usuario=" + User +
                            "&password=" + Pwd +
                            "&sucursal=" + Id_Branch +
                            "&cdEmpresa=" + Id_Company;

            strParametros = EncryptC.EncryptTripleDES(strParametros, KTGAPPWEB);
            WS.WS_Url = TGURL + "/pgs/services/getReferencias";
            Console.WriteLine("URL: "+WS.WS_Url);

            WS.WS_Action = "http://server.referencias.webservice.cpagos";
            WS.ClearWS();
            WS.WS_Params = strParametros;
            Console.WriteLine("Parametros Encryptados: "+WS.WS_Params);
            WS.WS_nbParams = "in0";
            WS.WS_Method = "getReporte";
            WS.MITWebServices(WS.WS_Params, WS.WS_nbParams, WS.WS_Method);
            Console.WriteLine("Respuesta "+WS.WS_Response);
            WS.WS_Response = Utils.GetDataXML(WS.WS_Response, "ns1:out");
            Console.WriteLine("Respuesta sin basura" + WS.WS_Response);
            WS.WS_Response = EncryptC.DecryptTripleDES(WS.WS_Response, KTGAPPWEB);
            return WS.WS_Response;



        }


        private bool vacio(string cadena)
        {
            return String.IsNullOrEmpty(cadena);
        }

        private void setRspTGXML(string p)
        {
            TGXML = p;
            //dbgSetTGXML(p);
        }

        private void dbgClearDLL()
        {
            TGXML = "";
            TGCdOriginadora = "";
            TGNbOriginadora = "";
            TGReferencia = "";
            TGImporte = "";
            TGTarjeta = "";
            TGEfectivo = "";

            TGThPagaComision = "";
            TGRefComision = "";
            TGNuComision = "";
            TGMerchantComision = "";
            TGCatBancos = "";
            TGXmlBancos = "";

            TGRecibo = "";


        }


        public string dbgGetTGURL()
        {
            return TGURL;
        }

        public void dbgSetTGURL(string value)
        {
            TGURL = value;
        }




        public string getRspTGXML()
        {
            return TGXML;
        }


        public string getRspTGEfectivo()
        {
            return TGEfectivo;
        }

        public void snd3GateInsertReference(string Bs_User, string Bs_Pwd, string Bs_Company, string Tx_Reference, string Tx_OptionalReference, string Tx_Amount, string Tx_Date)
        {
            dbgClearDLL();
             WS.WS_Url = TGURL + "/pgs/services/insertreferencias";
                WS.WS_Action = "http://server.insertreferencias.webservice.tgate.cpagos";
                WS.ClearWS();
                WS.WS_Params = EncryptC.EncryptTripleDES(Bs_User, KTGAPPWEB) + "," + EncryptC.EncryptTripleDES(Bs_Pwd, KTGAPPWEB) + "," + Bs_Company + "," + EncryptC.EncryptTripleDES(Tx_Reference, KTGAPPWEB) + "," + Tx_OptionalReference + "," + Tx_Amount + "," + Tx_Date;
                WS.WS_nbParams = "in0,in1,in2,in3,in4,in5,in6";
                WS.WS_Method = "insert";
    
                WS.MITWebServices(WS.WS_Params, WS.WS_nbParams, WS.WS_Method);
                setRspTGXML(WS.WS_Response);


        }
    }
}
