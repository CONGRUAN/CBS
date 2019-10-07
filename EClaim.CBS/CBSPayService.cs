using EClaim.CBS.Common;
using EClaim.CBS.Model.Req;
using EClaim.CBS.Model.Res;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EClaim.CBS
{
    public sealed class CBSPayService
    {
        private readonly string CRC32_PASSWORD = ConfigurationManager.AppSettings["CRC32_PASSWORD"];
        private readonly string CRC32_PREFIX = ConfigurationManager.AppSettings["CRC32_PREFIX"];
        private readonly string Key = ConfigurationManager.AppSettings["Key"];
        private readonly string Url = ConfigurationManager.AppSettings["Url"];

        /// <summary>
        /// 支付经办
        /// </summary>
        /// <param name="payInfo">支付信息</param>
        /// <param name="startIndex">支付失败开启轮询的起始数字</param>
        /// <param name="endIndex">支付失败开启轮询的终止数字</param>
        /// <param name="cbsRespAction">cbs返回参数回调函数，主要用于记录日志</param>
        /// <param name="payParamAction">支付参数回调函数，主要用于记录日志，返回失败才会执行，第一个参数是请求参数，第二个参数是错误信息</param>
        /// <returns>item1：客户业务参考号  item2：业务流水号</returns>
        public Tuple<string, string> PaySAV(PaySAVInfo payInfo, int startIndex, int endIndex, Action<string> cbsRespAction = null, Action<string, string> payParamAction = null)
        {
            string error = "";
            //生成支付请求信息参数
            string param = this.PaySAVInfoXMLStr(payInfo);
            string respStr = HttpHelper.Post(Url, param);
            cbsRespAction?.Invoke(respStr);

            respStr = this.GetXMLInnerData(respStr);
            CBSERPPGK respObj = XMLHelper.DeserializeToObject<CBSERPPGK>(respStr);
            if (!respObj.IsSucess)
            {
                bool flag1 = false;
                bool flag2 = false;
                bool flag3 = false;

                if (respObj.INFO != null)
                {
                    respObj.INFO.ERRMSG = respObj.INFO.ERRMSG.Replace("\n", "");
                    if (!string.IsNullOrEmpty(respObj.INFO.ERRMSG))
                        error += "\n" + respObj.INFO.ERRMSG;
                    else
                        flag1 = !flag1;
                }
                if (respObj.APPAYSAVZ != null)
                {
                    respObj.APPAYSAVZ.ERRMSG = respObj.APPAYSAVZ.ERRMSG.Replace("\n", "");
                    if (!string.IsNullOrEmpty(respObj.APPAYSAVZ.ERRMSG))
                        error += "\n" + respObj.APPAYSAVZ.ERRMSG;
                    else
                        flag2 = !flag2;
                }
                if (respObj.SYCOMRETZ != null)
                {
                    respObj.SYCOMRETZ.ERRMSG = respObj.SYCOMRETZ.ERRMSG.Replace("\n", "");
                    if (!string.IsNullOrEmpty(respObj.SYCOMRETZ.ERRMSG))
                        error += "\n" + respObj.SYCOMRETZ.ERRMSG;
                    else
                        flag3 = !flag3;
                }

                payParamAction?.Invoke(param, error);

                //非支付信息错误问题导致的支付失败，轮询请求
                if (flag1 && flag2 && flag3 && startIndex <= endIndex)
                {
                    startIndex += 1;
                    this.PaySAV(payInfo, startIndex, endIndex, cbsRespAction, payParamAction);
                }
                return new Tuple<string, string>(null, null);
            }
            else
            {
                Tuple<string, string> tuple = new Tuple<string, string>(respObj.APPAYSAVZ.REFNBR, respObj.APPAYSAVZ.BUSNBR);
                return tuple;
            }
        }

        /// <summary>
        /// 查询支付业务是否存在
        /// </summary>
        /// <param name="payBUS"></param>
        /// <param name="cbsRespAction">cbs返回参数回调函数，主要用于记录日志</param>
        /// <param name="payParamAction">查询参数回调函数，主要用于记录日志，返回失败才会执行，第一个参数是请求参数，第二个参数是错误信息</param>
        /// <returns></returns>
        public string SearchPayBUS(PayBUSInfo payBUS, Action<string> cbsRespAction = null, Action<string, string> payParamAction = null)
        {
            string error = "";
            //生成支付请求信息参数
            string param = this.PayBUSInfoXMLStr(payBUS);
            string respStr = HttpHelper.Post(Url, param);
            cbsRespAction?.Invoke(respStr);

            respStr = this.GetXMLInnerData(respStr);
            CBSERPPGK respObj = XMLHelper.DeserializeToObject<CBSERPPGK>(respStr);
            if (!respObj.IsSucess)
            {
                bool flag1 = false;
                bool flag3 = false;

                if (respObj.INFO != null)
                {
                    respObj.INFO.ERRMSG = respObj.INFO.ERRMSG.Replace("\n", "");
                    if (!string.IsNullOrEmpty(respObj.INFO.ERRMSG))
                        error += "\n" + respObj.INFO.ERRMSG;
                    else
                        flag1 = !flag1;
                }
                if (respObj.SYCOMRETZ != null)
                {
                    respObj.SYCOMRETZ.ERRMSG = respObj.SYCOMRETZ.ERRMSG.Replace("\n", "");
                    if (!string.IsNullOrEmpty(respObj.SYCOMRETZ.ERRMSG))
                        error += "\n" + respObj.SYCOMRETZ.ERRMSG;
                    else
                        flag3 = !flag3;
                }

                payParamAction?.Invoke(param, error);
                return "";
            }
            else
            {
                return respObj.ERPAYBUSZ.BUSNBR;
            }
        }

        /// <summary>
        /// 查询支付状态
        /// </summary>
        /// <param name="paySTAList"></param>
        /// <param name="cbsRespAction">cbs返回参数回调函数，主要用于记录日志</param>
        /// <param name="payParamAction">查询参数回调函数，主要用于记录日志，返回失败才会执行，第一个参数是请求参数，第二个参数是错误信息</param>
        /// <returns></returns>
        public List<ERPAYSTAZ> SearchPayStatus(List<PaySTAInfo> paySTAList, Action<string> cbsRespAction = null, Action<string, string> payParamAction = null)
        {
            string error = "";
            //生成支付请求信息参数
            string param = this.PaySTAInfoXMLStr(paySTAList);
            string respStr = HttpHelper.Post(Url, param);
            cbsRespAction?.Invoke(respStr);

            List<ERPAYSTAZ> list = this.GetPayStatusData(respStr, out error);
            if (error != "")
                payParamAction?.Invoke(param, error);
            return list;
        }

        private List<ERPAYSTAZ> GetPayStatusData(string text, out string error)
        {
            error = "";
            List<ERPAYSTAZ> list = new List<ERPAYSTAZ>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(text);
            XmlNodeList data = xml.SelectNodes("/PGK/DATA");
            if (data.Count == 1)
            {
                XmlElement ele = (XmlElement)data[0];
                var str = ele.InnerText;
                XmlDocument childXml = new XmlDocument();
                childXml.LoadXml(str);
                XmlNodeList coreData = childXml.SelectNodes("/CBSERPPGK");

                foreach (XmlNode item in coreData)
                {
                    foreach (XmlNode node in item.ChildNodes)
                    {
                        if (node.Name == "ERPAYSTAZ")
                        {
                            ERPAYSTAZ eRPAYSTAZ = new ERPAYSTAZ
                            {
                                BUSNBR = node.SelectSingleNode("BUSNBR").InnerText,
                                CLTACC = node.SelectSingleNode("CLTACC").InnerText,
                                ERRCOD = node.SelectSingleNode("ERRCOD").InnerText,
                                OPRTYP = node.SelectSingleNode("OPRTYP").InnerText,
                                OPTSTU = node.SelectSingleNode("OPTSTU").InnerText,
                                REFNBR = node.SelectSingleNode("REFNBR").InnerText,
                                STATUS = node.SelectSingleNode("STATUS").InnerText
                            };
                            if (node.SelectSingleNode("REMARK") != null)
                                eRPAYSTAZ.REMARK = node.SelectSingleNode("REMARK").InnerText;
                            list.Add(eRPAYSTAZ);
                        }
                        else if(node.Name == "INFO")
                        {
                            if(node.SelectSingleNode("RETCOD").InnerText != "0000000")
                            {
                                error = node.SelectSingleNode("ERRMSG").InnerText;
                            }
                        }
                    }
                    
                }
            }
            
            return list;
        }

        /// <summary>
        /// 生成支付经办请求信息xml字符串
        /// </summary>
        /// <param name="payInfo"></param>
        /// <returns></returns>
        private string PaySAVInfoXMLStr(PaySAVInfo payInfo)
        {
            string head = "<?xml version='1.0' encoding='GBK'?><PGK><DATA><![CDATA[";
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version='1.0' encoding='GBK'?>");
            sb.Append("<CBSERPPGK>");
            sb.Append("<INFO><FUNNAM>ERPAYSAV</FUNNAM></INFO>");
            sb.Append("<APPAYSAVX>");
            sb.Append($"<BNKTYP>{payInfo.BNKTYP}</BNKTYP>");
            sb.Append($"<BUSTYP>{payInfo.BUSTYP}</BUSTYP>");
            sb.Append($"<CCYNBR>{payInfo.CCYNBR}</CCYNBR>");
            sb.Append($"<PAYTYP>{payInfo.PAYTYP}</PAYTYP>");
            sb.Append($"<CLTACC>{payInfo.CLTACC}</CLTACC>");
            sb.Append($"<CLTNBR>{payInfo.CLTNBR}</CLTNBR>");
            sb.Append($"<EPTDAT>{payInfo.EPTDAT}</EPTDAT>");
            sb.Append($"<EPTTIM>{payInfo.EPTTIM}</EPTTIM>");
            sb.Append($"<EXTTX1>{payInfo.EXTTX1}</EXTTX1>");
            sb.Append($"<OPRMOD>{payInfo.OPRMOD}</OPRMOD>");
            sb.Append($"<OPRTYP>{payInfo.OPRTYP}</OPRTYP>");
            sb.Append($"<REFNBR>{payInfo.REFNBR}</REFNBR>");
            sb.Append($"<REVACC>{payInfo.REVACC}</REVACC>");
            sb.Append($"<REVBNK>{payInfo.REVBNK}</REVBNK>");
            sb.Append($"<REVCIT>{payInfo.REVCIT}</REVCIT>");
            sb.Append($"<REVEML/><REVMOB/>");
            sb.Append($"<REVNAM>{payInfo.REVNAM}</REVNAM>");
            sb.Append($"<REVPRV>{payInfo.REVPRV}</REVPRV>");
            sb.Append($"<TRSAMT>{payInfo.TRSAMT}</TRSAMT>");
            sb.Append($"<TRSUSE>{payInfo.TRSUSE}</TRSUSE>");
            sb.Append("</APPAYSAVX>");
            sb.Append("</CBSERPPGK>");

            string body = sb.ToString();
            string mbody = body.Replace("\n", "").Replace("\r", "");
            string checkCode = this.CheckCode(CRC32_PASSWORD + Key + mbody);
            string end = $"]]></DATA><CHECKCODE>{checkCode}</CHECKCODE></PGK>";
            return head + body + end;
        }

        /// <summary>
        /// 生成查询支付业务是否存在请求信息xml字符串
        /// </summary>
        /// <param name="payInfo"></param>
        /// <returns></returns>
        private string PayBUSInfoXMLStr(PayBUSInfo payInfo)
        {
            string head = "<?xml version='1.0' encoding='GBK'?><PGK><DATA><![CDATA[";
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version='1.0' encoding='GBK'?>");
            sb.Append("<CBSERPPGK>");
            sb.Append("<INFO><FUNNAM>ERPAYBUS</FUNNAM></INFO>");
            sb.Append("<ERPAYBUSX>");
            sb.Append($"<CLTACC>{payInfo.CLTACC}</CLTACC>");
            sb.Append($"<REVACC>{payInfo.REVACC}</REVACC>");
            sb.Append($"<TRSAMT>{payInfo.TRSAMT}</TRSAMT>");
            sb.Append($"<REFNBR>{payInfo.REFNBR}</REFNBR>");
            sb.Append("</ERPAYBUSX>");
            sb.Append("</CBSERPPGK>");
            string body = sb.ToString();
            string mbody = body.Replace("\n", "").Replace("\r", "");
            string checkCode = this.CheckCode(CRC32_PASSWORD + Key + mbody);
            string end = $"]]></DATA><CHECKCODE>{checkCode}</CHECKCODE></PGK>";
            return head + body + end;
        }

        /// <summary>
        /// 生成查询支付状态请求信息xml字符串
        /// </summary>
        /// <param name="payInfoList"></param>
        /// <returns></returns>
        private string PaySTAInfoXMLStr(List<PaySTAInfo> payInfoList)
        {
            string head = "<?xml version='1.0' encoding='GBK'?><PGK><DATA><![CDATA[";
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version='1.0' encoding='GBK'?>");
            sb.Append("<CBSERPPGK>");
            sb.Append("<INFO><FUNNAM>ERPAYSTA</FUNNAM></INFO>");
            if(payInfoList != null)
            {
                foreach(var payInfo in payInfoList)
                {
                    sb.Append("<ERPAYSTAX>");
                    sb.Append($"<REFNBR>{payInfo.REFNBR}</REFNBR>");
                    sb.Append($"<BUSNBR>{payInfo.BUSNBR}</BUSNBR>");
                    sb.Append("</ERPAYSTAX>");
                }
            }
            sb.Append("</CBSERPPGK>");
            string body = sb.ToString();
            string mbody = body.Replace("\n", "").Replace("\r", "");
            string checkCode = this.CheckCode(CRC32_PASSWORD + Key + mbody);
            string end = $"]]></DATA><CHECKCODE>{checkCode}</CHECKCODE></PGK>";
            return head + body + end;
        }

        private string CheckCode(string str)
        {
            long result = (long)CRC32Helper.GetCRC32Str(str);
            string code = CRC32_PREFIX + Convert.ToString(result, 16).ToUpper().PadLeft(8, '0');
            return code;
        }

        private string GetXMLInnerData(string text)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(text);
            XmlNodeList data = xml.SelectNodes("/PGK/DATA");
            if (data.Count == 1)
            {
                XmlElement ele = (XmlElement)data[0];
                var str = ele.InnerText;
                return str;
            }
            return "";
        }
        
    }
}
