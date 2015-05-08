using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Configuration;
using WeixinPay;
using CashierPayHelper.WeiXin;

namespace CashierPayService
{
    /// <summary>
    /// Service1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CashierPayService : System.Web.Services.WebService
    {
        //private string appid = "wx78cb8b26be235e33";
        //private string mch_id = "1238213702";
        //private string partnerKey = "zypwxpayshhy2015zypwxpayshhy2015";
        /// <summary>
        /// 信分配的公众账号ID
        /// </summary>
        private string appid = "";
        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        private string mch_id = "";
        /// <summary>
        /// API密钥
        /// </summary>
        private string partnerKey = "";
        /// <summary>
        /// 安全认证的存放路径
        /// </summary>
        private string PATH_TO_CERTIFICATE = "";
        /// <summary>
        /// 交易过期的时间
        /// </summary>
        private int expireTime = 0;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CashierPayService()
        {
            appid = ConfigurationManager.AppSettings["weixin_appid"];
            mch_id = ConfigurationManager.AppSettings["weixin_mch_id"];
            partnerKey = ConfigurationManager.AppSettings["weixin_partnerKey"];
            PATH_TO_CERTIFICATE = ConfigurationManager.AppSettings["PATH_TO_CERTIFICATE"];
            expireTime = Int32.Parse(ConfigurationManager.AppSettings["expireTime"]);
        }
        [WebMethod]
        public string hello()
        {
            return appid + " " + mch_id + " " + partnerKey;
        }
        #region 微信支付操作区域
        [WebMethod]
        /// <summary>
        /// 提交请求支付
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="mch_id"></param>
        /// <param name="device_info"></param>
        /// <param name="nonce_str"></param>
        /// <param name="body"></param>
        /// <param name="detail"></param>
        /// <param name="attach"></param>
        /// <param name="out_trade_no"></param>
        /// <param name="fee_type"></param>
        /// <param name="total_fee"></param>
        /// <param name="spbill_create_ip"></param>
        /// <param name="time_start"></param>
        /// <param name="time_expire"></param>
        /// <param name="goods_tag"></param>
        /// <param name="auth_code"></param>
        /// <param name="partnerKey"></param>
        /// <returns></returns>
        public string Submit(string appid, string mch_id, string device_info, string body, string detail, string attach, string out_trade_no, string fee_type, int total_fee, string spbill_create_ip, string time_start, string time_expire, string goods_tag, string auth_code, string partnerKey, string paytype, string sellerID, string subject, string shopID, string oprID)
        {
            string result = "";

            result = WeiXinPayAction.Submit(this.appid, this.mch_id, device_info, Util.CreateNonce_str(), body, detail, attach, out_trade_no, fee_type, total_fee, spbill_create_ip, Util.CreateTradeStartOrExpireTime(DateTime.Now), Util.CreateTradeStartOrExpireTime(DateTime.Now.AddMinutes(expireTime)), goods_tag, auth_code, this.partnerKey, paytype, sellerID, subject, shopID, oprID);
           
            return result;
        }

        [WebMethod]
        public  string Refund(string appid, string mch_id, string device_info, 
                                 string transaction_id, string out_trade_no, string out_refund_no,
                                 int total_fee, int refund_fee, string refund_fee_type, string op_user_id,
                                 string partnerKey, string PATH_TO_CERTIFICATE, string spbill_create_ip)
        {
            string result = WeiXinPayAction.Refund(this.appid, this.mch_id, device_info, Util.CreateNonce_str(),
                                  transaction_id,  out_trade_no,  out_refund_no,
                                  total_fee,  refund_fee,  refund_fee_type,  
                                  this.partnerKey, op_user_id, this.PATH_TO_CERTIFICATE,  spbill_create_ip);

            return result;
        }
        [WebMethod]
        public string OrderQuery(string appid, string mch_id, string transaction_id, string out_trade_no, string partnerKey, string spbill_create_ip, string terminalID, string op_user_id)
        {
            string result = WeiXinPayAction.OrderQuery(this.appid, this.mch_id, transaction_id, out_trade_no, Util.CreateNonce_str(), this.partnerKey, spbill_create_ip, terminalID, op_user_id);
            return result;
        }

        [WebMethod]
        public string ReverseSubmit(string appid, string mch_id, string transaction_id, string out_trade_no, string partnerKey, string PATH_TO_CERTIFICATE, string spbill_create_ip, string terminalID, string op_user_id)
        {
            string result = WeiXinPayAction.ReverseSubmit(this.appid, this.mch_id, transaction_id, out_trade_no, Util.CreateNonce_str(), this.partnerKey, this.PATH_TO_CERTIFICATE,spbill_create_ip, terminalID, op_user_id);
            return result;
        }
        [WebMethod]
        public  string RefundQuery(string appid, string mch_id, string device_info, 
                             string transaction_id, string out_trade_no, string out_refund_no, string refund_id,
                             string partnerKey, string op_user_id, string spbill_create_ip)
        {
            string result = WeiXinPayAction.RefundQuery(this.appid, this.mch_id, device_info, Util.CreateNonce_str(), transaction_id, out_trade_no, out_refund_no, refund_id, this.partnerKey, op_user_id, spbill_create_ip);
            return result;
        }
        [WebMethod]
        public  string Downloadbill(string appid, string mch_id, string device_info, string nonce_str,
                           string bill_date, string bill_type,
                           string partnerKey, string op_user_id, string spbill_create_ip)
        {
            string result = WeiXinPayAction.Downloadbill(this.appid, this.mch_id, device_info, Util.CreateNonce_str(), bill_date, bill_type, this.partnerKey, op_user_id, spbill_create_ip);
            return result;
        }
        #endregion


    }
}