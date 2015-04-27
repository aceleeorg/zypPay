using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data ;
using Newtonsoft.Json;
using HYModel;
using Alipay;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
namespace CashierMgr
{
    /// <summary>
    /// CashierService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class CashierService : System.Web.Services.WebService
    {

       

        [WebMethod]
        public string Active(string activeCode,string machineCode)
        {
            string res = "";

            HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
            ActiveRes resEntity = dal.Active(activeCode, machineCode);
            res = JsonConvert.SerializeObject(resEntity);
            return res;
        }

        [WebMethod]
        public bool UpdateOpr(OprInfoEntiy oprInfo)
        {
            bool flag = false;

            HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
            flag= dal.UpdateOpr(oprInfo);
             
            return flag;
        }

        [WebMethod]
        public bool UpdateOprPwd(string oprID,string newPwd)
        {
            bool flag = false;

            HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
            flag = dal.UpdateOprPwd(oprID ,newPwd );

            return flag;
        }
        [WebMethod ]
        public string GetClientOpr(string clientID)
        {
            string res = "";
            HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
            DataTable dt = dal.GetAllOpr(clientID);
            if (dt != null && dt.Rows.Count > 0)
            {
                res = JsonConvert.SerializeObject(dt);
            }
            return res;
        }


        [WebMethod]
        public string GetClient()
        {
            string res = "";
            HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
            DataTable dt = dal.GetAllOpr("");
            if (dt != null && dt.Rows.Count > 0)
            {
                IList<ClientInfoEntiy> clients = DataTableUtility.ToList<ClientInfoEntiy>(dt);
                res = JsonConvert.SerializeObject(clients);
            }
            return res;
        }
        ///// <summary>
        ///// 支付
        ///// </summary>
        ///// <param name="paytype">支付类型，支付宝001，必填</param>
        ///// <param name="clientID">商户编号 必填</param>
        ///// <param name="clientKey">商户key 必填</param>
        ///// <param name="channelID">用户渠道编号，如支付宝PID 必填</param>
        ///// <param name="channelKey">渠道key，如支付宝KEY 必填</param>
        ///// <param name="terminalID">终端编号 必填</param>
        ///// <param name="strAmmount">支付金额，小数点后两位，必须大于0.01 必填</param>
        ///// <param name="payCode">支付条码号：如支付宝当面付条码号 必填</param>
        ///// <param name="sellerID">卖家账号 必填</param>
        ///// <param name="subject">商品名称 必填</param>
        ///// <param name="shopID">门店编号</param>
        ///// <param name="oprID"></param>
        ///// <returns></returns>
        //  [WebMethod] 
        //public string Pay(string paytype, string clientID,string clientKey,string channelID, string channelKey, string terminalID, string  strAmmount,string payCode,string sellerID,string subject,string shopID,string oprID)
        //{
        //    DateTime requestTime=DateTime .Now ;
        //    string strRes="";
        //    string clientIP = HttpContext.Current.Request.UserHostAddress;
        //    string strFlowNo = "";
        //    string strRequestInfo = "paytype:"+paytype+" clientID:"+clientID ;
        //    strRequestInfo += " clientKey:" + clientKey + " channelID:" + channelID;
        //    strRequestInfo += " channelKey:" + channelKey + " terminalID:" + terminalID;
        //    strRequestInfo += " ammount" + strAmmount + " payCode:" + payCode;
        //    strRequestInfo += " sellerID:" + sellerID + " subject:" + subject;
        //    strRequestInfo += " shopID:" + shopID;
        //    strRequestInfo += " oprID:" + oprID ;
            
        //    //var stackTrace = new StackTrace();
        //    //var stackFrame = stackTrace.GetFrame(0);
        //    //// 如果要获取上层函数信息调用 GetFrame(1)， 这样就可以写成通用函数了

        //    //var methodBase = stackFrame.GetMethod();
          
        //    //var parameterInfos = methodBase.GetParameters();

         
        //    ////strRequestInfo = methodBase.ToString();
        //    //foreach (var parameterInfo in parameterInfos)
        //    //{
        //    //    strRequestInfo += parameterInfo.Name + ":" + parameterInfo.RawDefaultValue;
        //    //}
        //    strFlowNo=clientID +terminalID +DateTime .Now .ToString ("yyMMddHHmmss");
        //    PayResult payResult = new PayResult();
        //    payResult.Code = "9999";
        //    DateTime channelRequestTime = DateTime.Now;
        //    DateTime channelResponseTime = channelRequestTime;
        //    DateTime responseTime = channelRequestTime;
        //    string strChannelResponse = "";
        //    HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();

        //    if (!dal.AddServiceLog(strFlowNo, strRequestInfo, clientID, clientIP, requestTime,oprID ))
        //    {
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(paytype))
        //    {
        //        payResult.Code = "8013";
        //        payResult.Desc = "支付类型未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(clientID))
        //    {
        //        payResult.Code = "8004";
        //        payResult.Desc = "商户编号未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }
        //    if (string.IsNullOrEmpty(clientKey))
        //    {
        //        payResult.Code = "8005";
        //        payResult.Desc = "商户key未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(channelID))
        //    {
        //        payResult.Code = "8006";
        //        payResult.Desc = "用户渠道编号未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(channelKey))
        //    {
        //        payResult.Code = "8007";
        //        payResult.Desc = "渠道key未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(terminalID))
        //    {
        //        payResult.Code = "8008";
        //        payResult.Desc = "终端编号未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }
        //    if (string.IsNullOrEmpty(strAmmount))
        //    {
        //        payResult.Code = "8009";
        //        payResult.Desc = "支付金额未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(payCode))
        //    {
        //        payResult.Code = "8010";
        //        payResult.Desc = "支付条码号未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(sellerID))
        //    {
        //        payResult.Code = "8011";
        //        payResult.Desc = "卖家账号未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(subject))
        //    {
        //        payResult.Code = "8012";
        //        payResult.Desc = "商品名称未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }


        //    if (strAmmount.Contains("-"))
        //    {
        //        payResult.Code = "8003";
        //        payResult.Desc = "金额不正确";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (strAmmount.Contains(".") && strAmmount.Split('.')[1].Length > 2)
        //    {
        //        payResult.Code = "8003";
        //        payResult.Desc = "金额不正确";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    decimal ammount = 0;

        //    try
        //    {
        //        ammount = decimal.Parse(strAmmount);
        //    }
        //    catch
        //    {
        //        payResult.Code = "8003";
        //        payResult.Desc = "金额不正确";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }
           
        //    if (ammount < 0.01m)
        //    {
        //        payResult.Code = "8003";
        //        payResult.Desc = "金额不正确";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }
        //    if (paytype == "001")
        //    {

               
        //        try
        //        {

                   
        //            if (dal.IsClientOK(clientID, clientKey))
        //            {
        //               // string localFlowNo = clientID+terminalID + DateTime.Now.ToString("yyMMddHHmmss");
        //                string localFlowNo = strFlowNo;
        //                Alipay.AlipayBiz alipay = new AlipayBiz(channelID, channelKey, "", "");
        //                channelRequestTime = DateTime.Now;
        //                strChannelResponse = alipay.BARCODE_PAY_OFFLINE_Ext(sellerID, localFlowNo, subject, ammount.ToString("f2"), payCode, clientID, "1", terminalID, shopID,oprID );
        //                channelResponseTime = DateTime.Now;
        //                if (!string.IsNullOrEmpty(strChannelResponse))
        //                {
        //                    HYModel.Alipay_Pay payRes = XmlHelper.Deserialize(typeof(Alipay_Pay), strChannelResponse) as Alipay_Pay;
        //                    if (payRes != null)
        //                    {
        //                        if (payRes.is_success.ToLower() == "t")
        //                        {
        //                            string serverNo = payRes.Response[0].ReponseAlipays[0].trade_no;
        //                            string resCode = payRes.Response[0].ReponseAlipays[0].result_code.ToUpper();

        //                            string buyID = payRes.Response[0].ReponseAlipays[0].buyer_user_id;
        //                            string buyName = payRes.Response[0].ReponseAlipays[0].buyer_logon_id;
        //                            ChannelInfo channelPayInfo = new ChannelInfo();
        //                            channelPayInfo.Ammount = ammount;
        //                            channelPayInfo.BuyerID = buyID;
        //                            channelPayInfo.BuyerName = buyName;
        //                            channelPayInfo.ChannelFlowNo = serverNo;
        //                            channelPayInfo.ChannelResultCode = resCode;
        //                            channelPayInfo.SellerID = sellerID;
        //                            payResult.ChannelPayDetail = channelPayInfo;
        //                            payResult.Code = "0000";
        //                            payResult.Desc = "交易成功";
        //                            payResult.FlowNo = localFlowNo;
        //                            payResult.Note = strChannelResponse;
        //                        }
        //                        else
        //                        {
        //                            payResult.Code = "1002";
        //                            payResult.Desc = payRes .error ;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        payResult.Code = "1003";
        //                        payResult.Desc = payRes.error;
        //                    }
        //                }
        //                else
        //                {
        //                    payResult.Code = "1001";
        //                    payResult.Desc = "渠道无返回信息";
        //                }
                    
        //            }
        //            else
        //            {
        //                payResult.Code = "8002";
        //                payResult.Desc = "商户信息不匹配，请校验clientID和clientKey";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            payResult.Code = "9999";
        //            payResult.Desc = ex.ToString();
        //        }
        //    }
        //    else
        //    {
        //        payResult.Code = "8001";
        //        payResult.Desc = "支付类型错误或暂不支持指定的支付类型";
        //    }

           
        //    strRes=JsonConvert.SerializeObject (payResult );
        //    responseTime = DateTime.Now;
        //    if (!dal.UpdateServiceLog (strFlowNo,channelRequestTime ,channelResponseTime ,responseTime ,strChannelResponse ,strRes ))
        //    {
        //        //strRes = JsonConvert.SerializeObject(payResult);
        //        //return strRes;
        //    }
            
        //    return strRes;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="paytype"> 必填</param>
        ///// <param name="clientID"> 必填</param>
        ///// <param name="clientKey"> 必填</param>
        ///// <param name="channelID"> 必填</param>
        ///// <param name="channelKey"> 必填</param>
        ///// <param name="channelOrderNo">  </param>
        ///// <param name="orderNo"> 必填</param>
        ///// <param name="strAmmount">必填</param>
        ///// <param name="terminalID">必填</param>
        ///// <param name="oprID"></param>
        ///// <param name="refundReason"></param>
        ///// <returns></returns>
        //[WebMethod]
        //  public string Refund(string paytype, string clientID, string clientKey, string channelID, string channelKey, string channelOrderNo, string orderNo, string strAmmount, string terminalID,string oprID,string refundReason)
        //  {
        //      DateTime requestTime = DateTime.Now;
        //      string strRes = "";
        //      string clientIP = HttpContext.Current.Request.UserHostAddress;
        //      string strFlowNo = "";
        //      string strRequestInfo = "paytype:" + paytype + " clientID:" + clientID;
        //      strRequestInfo += " clientKey:" + clientKey + " channelID:" + channelID;
        //      strRequestInfo += " channelKey:" + channelKey + " terminalID:" + terminalID;
        //      strRequestInfo += " ammount:" + strAmmount ;
        //      strRequestInfo += " orderNo:" + orderNo;
        //      strRequestInfo += " channelOrderNo:" + channelOrderNo;
        //      strRequestInfo += " refundReason:" + refundReason;
          
        //      strFlowNo = clientID + terminalID + DateTime.Now.ToString("yyMMddHHmmss");
        //      RefundResult payResult = new RefundResult();
        //      payResult.Code = "9999";
        //      DateTime channelRequestTime = DateTime.Now;
        //      DateTime channelResponseTime = channelRequestTime;
        //      DateTime responseTime = channelRequestTime;
        //      string strChannelResponse = "";
        //      HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();

        //      if (!dal.AddServiceLog(strFlowNo, strRequestInfo, clientID, clientIP, requestTime,oprID ))
        //      {
        //          strRes = JsonConvert.SerializeObject(payResult);
        //          responseTime = DateTime.Now;
        //          if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //          {
        //              //strRes = JsonConvert.SerializeObject(payResult);
        //              //return strRes;
        //          }
        //          return strRes;
        //      }

        //      if (string.IsNullOrEmpty(paytype))
        //      {
        //          payResult.Code = "8013";
        //          payResult.Desc = "支付类型未填写";
        //          strRes = JsonConvert.SerializeObject(payResult);
        //          responseTime = DateTime.Now;
        //          if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //          {
        //              //strRes = JsonConvert.SerializeObject(payResult);
        //              //return strRes;
        //          }
        //          return strRes;
        //      }

        //      if (string.IsNullOrEmpty(clientID))
        //      {
        //          payResult.Code = "8004";
        //          payResult.Desc = "商户编号未填写";
        //          strRes = JsonConvert.SerializeObject(payResult);
        //          responseTime = DateTime.Now;
        //          if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //          {
        //              //strRes = JsonConvert.SerializeObject(payResult);
        //              //return strRes;
        //          }
        //          return strRes;
        //      }
        //      if (string.IsNullOrEmpty(clientKey))
        //      {
        //          payResult.Code = "8005";
        //          payResult.Desc = "商户key未填写";
        //          strRes = JsonConvert.SerializeObject(payResult);
        //          responseTime = DateTime.Now;
        //          if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //          {
        //              //strRes = JsonConvert.SerializeObject(payResult);
        //              //return strRes;
        //          }
        //          return strRes;
        //      }

        //      if (string.IsNullOrEmpty(channelID))
        //      {
        //          payResult.Code = "8006";
        //          payResult.Desc = "用户渠道编号未填写";
        //          strRes = JsonConvert.SerializeObject(payResult);
        //          responseTime = DateTime.Now;
        //          if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //          {
        //              //strRes = JsonConvert.SerializeObject(payResult);
        //              //return strRes;
        //          }
        //          return strRes;
        //      }

        //      if (string.IsNullOrEmpty(channelKey))
        //      {
        //          payResult.Code = "8007";
        //          payResult.Desc = "渠道key未填写";
        //          strRes = JsonConvert.SerializeObject(payResult);
        //          responseTime = DateTime.Now;
        //          if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //          {
        //              //strRes = JsonConvert.SerializeObject(payResult);
        //              //return strRes;
        //          }
        //          return strRes;
        //      }
        //      if (string.IsNullOrEmpty(terminalID))
        //      {
        //          payResult.Code = "8008";
        //          payResult.Desc = "终端编号未填写";
        //          strRes = JsonConvert.SerializeObject(payResult);
        //          responseTime = DateTime.Now;
        //          if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //          {
        //              //strRes = JsonConvert.SerializeObject(payResult);
        //              //return strRes;
        //          }
        //          return strRes;
        //      }
            
        //      if (string.IsNullOrEmpty(strAmmount))
        //      {
        //          payResult.Code = "8009";
        //          payResult.Desc = "支付金额未填写";
        //          strRes = JsonConvert.SerializeObject(payResult);
        //          responseTime = DateTime.Now;
        //          if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //          {
        //              //strRes = JsonConvert.SerializeObject(payResult);
        //              //return strRes;
        //          }
        //          return strRes;
        //      }

        //       decimal ammount = 0;

        //    try
        //    {
        //        ammount = decimal.Parse(strAmmount);
        //    }
        //    catch
        //    {
        //        payResult.Code = "8003";
        //        payResult.Desc = "金额不正确";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }
           
        //    if (ammount < 0.01m)
        //    {
        //        payResult.Code = "8003";
        //        payResult.Desc = "金额不正确";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }
        //    if (paytype == "001")
        //    {
        //        try
        //        {
        //            if (dal.IsClientOK(clientID, clientKey))
        //            {
        //                // string localFlowNo = clientID+terminalID + DateTime.Now.ToString("yyMMddHHmmss");
        //                string localFlowNo = strFlowNo;
        //                Alipay.AlipayBiz alipay = new AlipayBiz(channelID, channelKey, "", "");
        //                channelRequestTime = DateTime.Now;
        //                strChannelResponse = alipay.Refund(orderNo, channelOrderNo, strAmmount, "1", oprID, refundReason);
        //                channelResponseTime = DateTime.Now;
        //                if (!string.IsNullOrEmpty(strChannelResponse))
        //                {
        //                    HYModel.Alipay_Refund payRes = XmlHelper.Deserialize(typeof(Alipay_Refund), strChannelResponse) as Alipay_Refund;
        //                    if (payRes != null)
        //                    {
        //                        if (payRes.is_success.ToLower() == "t")
        //                        {
        //                            string serverNo = payRes.Response[0].ReponseAlipays[0].trade_no;
        //                            string resCode = payRes.Response[0].ReponseAlipays[0].result_code.ToUpper();

        //                            string buyID = payRes.Response[0].ReponseAlipays[0].buyer_user_id;
        //                            string buyName = payRes.Response[0].ReponseAlipays[0].buyer_logon_id;
        //                            ChannelInfo channelPayInfo = new ChannelInfo();
        //                            channelPayInfo.Ammount = ammount;
        //                            channelPayInfo.BuyerID = buyID;
        //                            channelPayInfo.BuyerName = buyName;
        //                            channelPayInfo.ChannelFlowNo = serverNo;
        //                           // channelPayInfo.ChannelStatus = payRes.Response[0].ReponseAlipays[0].trade_status;
        //                           // channelPayInfo.SellerID = sellerID;
        //                            channelPayInfo.ChannelResultCode = payRes.Response[0].ReponseAlipays[0].result_code;
        //                            payResult.ChannelRefundDetail  = channelPayInfo;
        //                            payResult.Code = "0000";
        //                            payResult.Desc = "交易成功";
        //                            payResult.FlowNo = localFlowNo;
        //                            payResult.Note = strChannelResponse;
        //                        }
        //                        else
        //                        {
        //                            payResult.Code = "1002";
        //                            payResult.Desc = payRes.error;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        payResult.Code = "1003";
        //                        payResult.Desc = payRes.error;
        //                    }
        //                }
        //                else
        //                {
        //                    payResult.Code = "1001";
        //                    payResult.Desc = "渠道无返回信息";
        //                }

        //            }
        //            else
        //            {
        //                payResult.Code = "8002";
        //                payResult.Desc = "商户信息不匹配，请校验clientID和clientKey";
        //            }
                
        //        }
        //        catch (Exception ex)
        //        {
        //            payResult.Code = "9999";
        //            payResult.Desc = ex.ToString();
        //        }
        //    }
        //    else
        //    {
        //        payResult.Code = "8001";
        //        payResult.Desc = "支付类型错误或暂不支持指定的支付类型";
        //    }

        //    return strRes;
        //  }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="paytype"></param>
        ///// <param name="clientID"></param>
        ///// <param name="clientKey"></param>
        ///// <param name="channelID"></param>
        ///// <param name="channelKey"></param>
        ///// <param name="channelOrderNo"></param>
        ///// <param name="orderNo"></param>
        ///// <param name="terminalID"></param>
        ///// <param name="oprID"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public string QueryOrder(string paytype, string clientID, string clientKey, string channelID, string channelKey, string channelOrderNo, string orderNo,  string terminalID, string oprID)
        //{
        //    DateTime requestTime = DateTime.Now;
        //    string strRes = "";
        //    string clientIP = HttpContext.Current.Request.UserHostAddress;
        //    string strFlowNo = "";
        //    string strRequestInfo = "paytype:" + paytype + " clientID:" + clientID;
        //    strRequestInfo += " clientKey:" + clientKey + " channelID:" + channelID;
        //    strRequestInfo += " channelKey:" + channelKey + " terminalID:" + terminalID;
        //    strRequestInfo += " channelOrderNo:" + channelOrderNo;
        //    strRequestInfo += " orderNo:" + orderNo;


        //    strFlowNo = clientID + terminalID + DateTime.Now.ToString("yyMMddHHmmss");
        //    QueryResult payResult = new QueryResult();
        //    payResult.Code = "9999";
        //    DateTime channelRequestTime = DateTime.Now;
        //    DateTime channelResponseTime = channelRequestTime;
        //    DateTime responseTime = channelRequestTime;
        //    string strChannelResponse = "";
        //    HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();

        //    if (!dal.AddServiceLog(strFlowNo, strRequestInfo, clientID, clientIP, requestTime, oprID))
        //    {
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(paytype))
        //    {
        //        payResult.Code = "8013";
        //        payResult.Desc = "支付类型未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(clientID))
        //    {
        //        payResult.Code = "8004";
        //        payResult.Desc = "商户编号未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }
        //    if (string.IsNullOrEmpty(clientKey))
        //    {
        //        payResult.Code = "8005";
        //        payResult.Desc = "商户key未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(channelID))
        //    {
        //        payResult.Code = "8006";
        //        payResult.Desc = "用户渠道编号未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(channelKey))
        //    {
        //        payResult.Code = "8007";
        //        payResult.Desc = "渠道key未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }
        //    if (string.IsNullOrEmpty(terminalID))
        //    {
        //        payResult.Code = "8008";
        //        payResult.Desc = "终端编号未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

            
        //    if (paytype == "001")
        //    {
        //        try
        //        {
        //            if (dal.IsClientOK(clientID, clientKey))
        //            {
        //                // string localFlowNo = clientID+terminalID + DateTime.Now.ToString("yyMMddHHmmss");
        //                string localFlowNo = strFlowNo;
        //                Alipay.AlipayBiz alipay = new AlipayBiz(channelID, channelKey, "", "");
        //                channelRequestTime = DateTime.Now;
        //                strChannelResponse = alipay.Query (orderNo, channelOrderNo);
        //                channelResponseTime = DateTime.Now;
        //                if (!string.IsNullOrEmpty(strChannelResponse))
        //                {
        //                    HYModel.Alipay_Query payRes = XmlHelper.Deserialize(typeof(Alipay_Query), strChannelResponse) as Alipay_Query;
        //                    if (payRes != null)
        //                    {
        //                        if (payRes.is_success.ToLower() == "t")
        //                        {
        //                            string serverNo = payRes.Response[0].ReponseAlipays[0].trade_no;
        //                            string resCode = payRes.Response[0].ReponseAlipays[0].result_code.ToUpper();

        //                            string buyID = payRes.Response[0].ReponseAlipays[0].buyer_user_id;
        //                            string buyName = payRes.Response[0].ReponseAlipays[0].buyer_logon_id;
        //                            ChannelQueryInfo channelPayInfo = new ChannelQueryInfo();
        //                            channelPayInfo.Ammount =decimal .Parse ( payRes.total_fee);
        //                            channelPayInfo.BuyerID = buyID;
        //                            channelPayInfo.BuyerName = buyName;
        //                            channelPayInfo.ChannelFlowNo = serverNo;
        //                            channelPayInfo.ChannelResultCode = resCode;
        //                            channelPayInfo.ChannelStatus = payRes.Response[0].ReponseAlipays[0].trade_status;
        //                            // channelPayInfo.SellerID = sellerID;
        //                            payResult.ChannelRefundDetail = channelPayInfo;
        //                            payResult.Code = "0000";
        //                            payResult.Desc = "交易成功";
        //                            payResult.FlowNo = localFlowNo;
        //                            payResult.Note = strChannelResponse;
        //                        }
        //                        else
        //                        {
        //                            payResult.Code = "1002";
        //                            payResult.Desc = payRes.error;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        payResult.Code = "1003";
        //                        payResult.Desc = payRes.error;
        //                    }
        //                }
        //                else
        //                {
        //                    payResult.Code = "1001";
        //                    payResult.Desc = "渠道无返回信息";
        //                }

        //            }
        //            else
        //            {
        //                payResult.Code = "8002";
        //                payResult.Desc = "商户信息不匹配，请校验clientID和clientKey";
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            payResult.Code = "9999";
        //            payResult.Desc = ex.ToString();
        //        }
        //    }
        //    else
        //    {
        //        payResult.Code = "8001";
        //        payResult.Desc = "支付类型错误或暂不支持指定的支付类型";
        //    }

        //    return strRes;
        //}

        //[WebMethod]
        //public string CancelOrder(string paytype, string clientID, string clientKey, string channelID, string channelKey, string channelOrderNo, string orderNo,  string terminalID, string oprID)
        //{
        //    DateTime requestTime = DateTime.Now;
        //    string strRes = "";
        //    string clientIP = HttpContext.Current.Request.UserHostAddress;
        //    string strFlowNo = "";
        //    string strRequestInfo = "paytype:" + paytype + " clientID:" + clientID;
        //    strRequestInfo += " clientKey:" + clientKey + " channelID:" + channelID;
        //    strRequestInfo += " channelKey:" + channelKey + " terminalID:" + terminalID;
         
        //    strRequestInfo += " orderNo:" + orderNo;
        //    strRequestInfo += " channelOrderNo:" + channelOrderNo;
        //   // strRequestInfo +=" cancelReason:"+cancelReason;


        //    strFlowNo = clientID + terminalID + DateTime.Now.ToString("yyMMddHHmmss");
        //    CancelResult payResult = new CancelResult();
        //    payResult.Code = "9999";
        //    DateTime channelRequestTime = DateTime.Now;
        //    DateTime channelResponseTime = channelRequestTime;
        //    DateTime responseTime = channelRequestTime;
        //    string strChannelResponse = "";
        //    HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();

        //    if (!dal.AddServiceLog(strFlowNo, strRequestInfo, clientID, clientIP, requestTime, oprID))
        //    {
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(paytype))
        //    {
        //        payResult.Code = "8013";
        //        payResult.Desc = "支付类型未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(clientID))
        //    {
        //        payResult.Code = "8004";
        //        payResult.Desc = "商户编号未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }
        //    if (string.IsNullOrEmpty(clientKey))
        //    {
        //        payResult.Code = "8005";
        //        payResult.Desc = "商户key未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(channelID))
        //    {
        //        payResult.Code = "8006";
        //        payResult.Desc = "用户渠道编号未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

        //    if (string.IsNullOrEmpty(channelKey))
        //    {
        //        payResult.Code = "8007";
        //        payResult.Desc = "渠道key未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }
        //    if (string.IsNullOrEmpty(terminalID))
        //    {
        //        payResult.Code = "8008";
        //        payResult.Desc = "终端编号未填写";
        //        strRes = JsonConvert.SerializeObject(payResult);
        //        responseTime = DateTime.Now;
        //        if (!dal.UpdateServiceLog(strFlowNo, channelRequestTime, channelResponseTime, responseTime, strChannelResponse, strRes))
        //        {
        //            //strRes = JsonConvert.SerializeObject(payResult);
        //            //return strRes;
        //        }
        //        return strRes;
        //    }

         
        //    if (paytype == "001")
        //    {
        //        try
        //        {
        //            if (dal.IsClientOK(clientID, clientKey))
        //            {
        //                // string localFlowNo = clientID+terminalID + DateTime.Now.ToString("yyMMddHHmmss");
        //                string localFlowNo = strFlowNo;
        //                Alipay.AlipayBiz alipay = new AlipayBiz(channelID, channelKey, "", "");
        //                channelRequestTime = DateTime.Now;
        //                strChannelResponse = alipay.Cancel(orderNo, channelOrderNo, "1", oprID);
        //                channelResponseTime = DateTime.Now;
        //                if (!string.IsNullOrEmpty(strChannelResponse))
        //                {
        //                    HYModel.Alipay_Cancel payRes = XmlHelper.Deserialize(typeof(Alipay_Cancel), strChannelResponse) as Alipay_Cancel;
        //                    if (payRes != null)
        //                    {
        //                        if (payRes.is_success.ToLower() == "t")
        //                        {
        //                            string serverNo = payRes.Response[0].ReponseAlipays[0].trade_no;
        //                            string resCode = payRes.Response[0].ReponseAlipays[0].result_code.ToUpper();

        //                            string buyID = payRes.Response[0].ReponseAlipays[0].buyer_user_id;
        //                            string buyName = payRes.Response[0].ReponseAlipays[0].buyer_logon_id;
        //                            ChannelInfo channelPayInfo = new ChannelInfo();
        //                            //channelPayInfo.Ammount = ammount;
        //                            channelPayInfo.BuyerID = buyID;
        //                            channelPayInfo.BuyerName = buyName;
        //                            channelPayInfo.ChannelFlowNo = serverNo;
        //                            channelPayInfo.ChannelResultCode = payRes.Response[0].ReponseAlipays[0].result_code;
        //                           // channelPayInfo.ChannelStatus = payRes.Response[0].ReponseAlipays[0].trade_status;
        //                            // channelPayInfo.SellerID = sellerID;
        //                            payResult.ChannelRefundDetail = channelPayInfo;
        //                            payResult.Code = "0000";
        //                            payResult.Desc = "交易成功";
        //                            payResult.FlowNo = localFlowNo;
        //                            payResult.Note = strChannelResponse;
        //                        }
        //                        else
        //                        {
        //                            payResult.Code = "1002";
        //                            payResult.Desc = payRes.error;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        payResult.Code = "1003";
        //                        payResult.Desc = payRes.error;
        //                    }
        //                }
        //                else
        //                {
        //                    payResult.Code = "1001";
        //                    payResult.Desc = "渠道无返回信息";
        //                }

        //            }
        //            else
        //            {
        //                payResult.Code = "8002";
        //                payResult.Desc = "商户信息不匹配，请校验clientID和clientKey";
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            payResult.Code = "9999";
        //            payResult.Desc = ex.ToString();
        //        }
        //    }
        //    else
        //    {
        //        payResult.Code = "8001";
        //        payResult.Desc = "支付类型错误或暂不支持指定的支付类型";
        //    }

        //    return strRes;
        //}


      


    }

    public static class DataTableUtility
    {
        /// <summary>
        /// DataTable To IList<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> ToList<T>(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return null;
            IList<T> list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T obj = row.ToEntity<T>();
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// DataRow To T
        /// </summary>
        public static T ToEntity<T>(this DataRow row)
        {
            Type objType = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in row.Table.Columns)
            {
                PropertyInfo property =
                objType.GetProperty(column.ColumnName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null || !property.CanWrite)
                {
                    continue;
                }
                object value = row[column.ColumnName];

                if (value == DBNull.Value) value = null;

                property.SetValue(obj, value, null);

            }
            return obj;
        }


        /// <summary>
        /// List To DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            try
            {
                Type objType = typeof(T);
                DataTable dataTable = new DataTable(objType.Name);
                if (list != null ? list.Count > 0 : false)
                {
                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(objType);
                    foreach (PropertyDescriptor property in properties)
                    {
                        Type propertyType = property.PropertyType;

                        //nullables must use underlying types
                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            propertyType = Nullable.GetUnderlyingType(propertyType);
                        //enums also need special treatment
                        if (propertyType.IsEnum)
                            propertyType = Enum.GetUnderlyingType(propertyType); //probably Int32

                        dataTable.Columns.Add(property.Name, propertyType);
                    }

                    foreach (T li in list)
                    {
                        DataRow row = dataTable.NewRow();
                        foreach (PropertyDescriptor property1 in properties)
                        {
                            row[property1.Name] = property1.GetValue(li) ?? DBNull.Value; //can't use null
                        }
                        dataTable.Rows.Add(row);

                    }
                }
                return dataTable;
            }
            catch
            {
                return null;
            }
        }
    }
}
