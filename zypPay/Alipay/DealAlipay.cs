using System;
using System.Collections.Generic;
using System.Text;

namespace Alipay
{
   public  class DealAlipay
    {
       /// <summary>
       /// 条码支付
       /// </summary>
       /// <param name="seller_email">卖家账号</param>
       /// <param name="out_trade_no">系统唯一流水号</param>
       /// <param name="subject">交易名称</param>
       /// <param name="total_fee">金额</param>
        ///<param name="barCode">支付宝当面付条码</param>
       /// <returns></returns>
       public static string BARCODE_PAY_OFFLINE(string seller_email, string out_trade_no, string subject, string total_fee,string barCode)
       {
           ////////////////////////////////////////////请求参数////////////////////////////////////////////

           ////卖家支付宝帐户
           //string seller_email;
           ////必填

           ////商户订单号
           //string out_trade_no ;
           ////商户网站订单系统中唯一订单号，必填

           ////订单名称
           //string subject ;
           ////必填

           ////付款金额
           //string total_fee ;
           //必填
          
           //订单业务类型
           string product_code = "BARCODE_PAY_OFFLINE";
           //SOUNDWAVE_PAY_OFFLINE：声波支付，FINGERPRINT_FAST_PAY：指纹支付，BARCODE_PAY_OFFLINE：条码支付

           //动态ID类型
           string dynamic_id_type = "barcode";
           //soundwave：声波，qrcode：二维码，barcode：条码

           //动态ID
           //string dynamic_id = Guid .NewGuid ().ToString ().Replace ("-","");
           //例如3856957008a73b7d
           
           ////////////////////////////////////////////////////////////////////////////////////////////////

           //把请求参数打包成数组
           SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
           sParaTemp.Add("partner", Config.Partner);
           sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
           sParaTemp.Add("service", "alipay.acquire.createandpay");
           sParaTemp.Add("seller_email", seller_email);
           sParaTemp.Add("out_trade_no", out_trade_no);
           sParaTemp.Add("subject", subject);
           sParaTemp.Add("total_fee", total_fee);
           sParaTemp.Add("product_code", product_code);
           sParaTemp.Add("dynamic_id_type", dynamic_id_type);
           sParaTemp.Add("dynamic_id", barCode );
           //extend_params
          // sParaTemp.Add("extend_params", barCode);
           //建立请求
           string sHtmlText = Submit.BuildRequest(sParaTemp);

           return sHtmlText;
       }


       /// <summary>
       /// 条码支付
       /// </summary>
       /// <param name="seller_email">卖家账号</param>
       /// <param name="out_trade_no">系统唯一流水号</param>
       /// <param name="subject">交易名称</param>
       /// <param name="total_fee">金额</param>
       /// <param name="SHOP_ID"></param>
       /// <param name="STORE_TYPE"></param>
       /// <param name="STORE_ID"></param>
       /// <param name="TERMINAL_ID"></param>
       ///<param name="barCode">支付宝当面付条码</param>
       /// <returns></returns>
       public static string BARCODE_PAY_OFFLINE_Ext(string seller_email, string out_trade_no, string subject, string total_fee, string barCode, string STORE_ID, string STORE_TYPE, string TERMINAL_ID, string SHOP_ID)
       {
           ////////////////////////////////////////////请求参数////////////////////////////////////////////

           ////卖家支付宝帐户
           //string seller_email;
           ////必填

           ////商户订单号
           //string out_trade_no ;
           ////商户网站订单系统中唯一订单号，必填

           ////订单名称
           //string subject ;
           ////必填

           ////付款金额
           //string total_fee ;
           //必填

           //订单业务类型
           string product_code = "BARCODE_PAY_OFFLINE";
           //SOUNDWAVE_PAY_OFFLINE：声波支付，FINGERPRINT_FAST_PAY：指纹支付，BARCODE_PAY_OFFLINE：条码支付

           //动态ID类型
           string dynamic_id_type = "barcode";
           //soundwave：声波，qrcode：二维码，barcode：条码

           //动态ID
           //string dynamic_id = Guid .NewGuid ().ToString ().Replace ("-","");
           //例如3856957008a73b7d

           ////////////////////////////////////////////////////////////////////////////////////////////////

           //把请求参数打包成数组
           SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
           sParaTemp.Add("partner", Config.Partner);
           sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
           sParaTemp.Add("service", "alipay.acquire.createandpay");
           sParaTemp.Add("seller_email", seller_email);
           sParaTemp.Add("out_trade_no", out_trade_no);
           sParaTemp.Add("subject", subject);
           sParaTemp.Add("total_fee", total_fee);
           sParaTemp.Add("product_code", product_code);
           sParaTemp.Add("dynamic_id_type", dynamic_id_type);
           sParaTemp.Add("dynamic_id", barCode);
           //extend_params
           string extInfo = "{\"AGENT_ID\":\"11864042a1\"";
           if (!string.IsNullOrEmpty(STORE_TYPE))
           {
               extInfo += ",\"STORE_TYPE\":\""+STORE_TYPE+"\"";
           }

           if (!string.IsNullOrEmpty(SHOP_ID))
           {
               extInfo += ",\"SHOP_ID\":\"" + SHOP_ID + "\"";
           }


           if (!string.IsNullOrEmpty(TERMINAL_ID))
           {
               extInfo += ",\"TERMINAL_ID\":\"" + TERMINAL_ID + "\"";
           }

           if (!string.IsNullOrEmpty(STORE_ID))
           {
               extInfo += ",\"STORE_ID\":\"" + STORE_ID + "\"";
           }

           extInfo += "}";
           sParaTemp.Add("extend_params", extInfo);
           //建立请求
           string sHtmlText = Submit.BuildRequest(sParaTemp);

           return sHtmlText;
       }

       /// <summary>
       /// 退款
       /// </summary>
       /// <param name="out_trade_no">本地唯一订单号【支付成功的】</param>
       /// <param name="alipayOrderNo">支付宝返回的订单号</param>
       /// <param name="refund_amount">退款金额</param>
       /// <param name="operator_type">操作员类型</param>
       /// <param name="operator_id">操作员编号</param>
       /// <param name="refund_reason">退款理由</param>
       /// <returns>支付宝返回xml格式文本</returns>
       public static string Refund(string out_trade_no,string alipayOrderNo, string refund_amount, string operator_type, string operator_id, string refund_reason)
       {
           ////////////////////////////////////////////请求参数////////////////////////////////////////////

           ////卖家支付宝帐户
           //string seller_email;
           ////必填

           ////商户订单号
           //string out_trade_no ;
           ////商户网站订单系统中唯一订单号，必填

           ////订单名称
           //string subject ;
           ////必填

           ////付款金额
           //string total_fee ;
           //必填

           //订单业务类型
          // string product_code = "BARCODE_PAY_OFFLINE";
           //SOUNDWAVE_PAY_OFFLINE：声波支付，FINGERPRINT_FAST_PAY：指纹支付，BARCODE_PAY_OFFLINE：条码支付

           //动态ID类型
          // string dynamic_id_type = "bar_code";
           //soundwave：声波，qrcode：二维码，barcode：条码  bar_code

           //动态ID
           //string dynamic_id = Guid.NewGuid().ToString().Replace("-", "");
           //例如3856957008a73b7d

           ////////////////////////////////////////////////////////////////////////////////////////////////

           //把请求参数打包成数组
           SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
           sParaTemp.Add("partner", Config.Partner);
           sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
           sParaTemp.Add("service", "alipay.acquire.refund");
           sParaTemp.Add("out_trade_no", out_trade_no);
           sParaTemp.Add("refund_amount", refund_amount);
           sParaTemp.Add("trade_no", alipayOrderNo);
           sParaTemp.Add("operator_type", operator_type);
           sParaTemp.Add("operator_id", operator_id);
           sParaTemp.Add("refund_reason", refund_reason);
         //  sParaTemp.Add("dynamic_id", dynamic_id);

           //建立请求
           string sHtmlText = Submit.BuildRequest(sParaTemp);

           return sHtmlText;
       }

       /// <summary>
       /// 取消订单
       /// </summary>
       /// <param name="out_trade_no">本地唯一订单号【支付成功的】</param>
       /// <param name="alipayOrderNo">支付宝返回的订单号</param>
       /// <param name="operator_type">操作员类型：支付宝操作员：0 本地操作员1 ，一般使用1</param>
       /// <param name="operator_id">操作员编号</param>
       /// <param name="refund_reason"></param>
       /// <returns></returns>
       public static string Cancel(string out_trade_no, string alipayOrderNo, string operator_type, string operator_id, string refund_reason)
       {
           ////////////////////////////////////////////请求参数////////////////////////////////////////////

           ////卖家支付宝帐户
           //string seller_email;
           ////必填

           ////商户订单号
           //string out_trade_no ;
           ////商户网站订单系统中唯一订单号，必填

           ////订单名称
           //string subject ;
           ////必填

           ////付款金额
           //string total_fee ;
           //必填

           //订单业务类型
           // string product_code = "BARCODE_PAY_OFFLINE";
           //SOUNDWAVE_PAY_OFFLINE：声波支付，FINGERPRINT_FAST_PAY：指纹支付，BARCODE_PAY_OFFLINE：条码支付

           //动态ID类型
           // string dynamic_id_type = "bar_code";
           //soundwave：声波，qrcode：二维码，barcode：条码  bar_code

           //动态ID
           //string dynamic_id = Guid.NewGuid().ToString().Replace("-", "");
           //例如3856957008a73b7d

           ////////////////////////////////////////////////////////////////////////////////////////////////

           //把请求参数打包成数组
           SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
           sParaTemp.Add("partner", Config.Partner);
           sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
           sParaTemp.Add("service", "alipay.acquire.cancel");

           sParaTemp.Add("out_trade_no", out_trade_no);
           sParaTemp.Add("trade_no", alipayOrderNo);
           sParaTemp.Add("operator_type", operator_type);
           sParaTemp.Add("operator_id", operator_id);
           sParaTemp.Add("refund_reason", refund_reason);
           //  sParaTemp.Add("dynamic_id", dynamic_id);

           //建立请求
           string sHtmlText = Submit.BuildRequest(sParaTemp);

           return sHtmlText;
       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="out_trade_no"></param>
       /// <param name="alipayOrderNo"></param>
       /// <returns></returns>
       public static string Query(string out_trade_no, string alipayOrderNo)
       {
           ////////////////////////////////////////////请求参数////////////////////////////////////////////

           ////卖家支付宝帐户
           //string seller_email;
           ////必填

           ////商户订单号
           //string out_trade_no ;
           ////商户网站订单系统中唯一订单号，必填

           ////订单名称
           //string subject ;
           ////必填

           ////付款金额
           //string total_fee ;
           //必填

           //订单业务类型
           // string product_code = "BARCODE_PAY_OFFLINE";
           //SOUNDWAVE_PAY_OFFLINE：声波支付，FINGERPRINT_FAST_PAY：指纹支付，BARCODE_PAY_OFFLINE：条码支付

           //动态ID类型
           // string dynamic_id_type = "bar_code";
           //soundwave：声波，qrcode：二维码，barcode：条码  bar_code

           //动态ID
           //string dynamic_id = Guid.NewGuid().ToString().Replace("-", "");
           //例如3856957008a73b7d

           ////////////////////////////////////////////////////////////////////////////////////////////////

           //把请求参数打包成数组
           SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
           sParaTemp.Add("partner", Config.Partner);
           sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
           sParaTemp.Add("service", "alipay.acquire.query");

           sParaTemp.Add("out_trade_no", out_trade_no);
           sParaTemp.Add("trade_no", alipayOrderNo);
        
           //  sParaTemp.Add("dynamic_id", dynamic_id);

           //建立请求
           string sHtmlText = Submit.BuildRequest(sParaTemp);

           return sHtmlText;
       }
    }


   public class AlipayBiz
   {
       string m_pid;
       string m_key;
       string m_charset = "utf-8";
       string m_signtype = "MD5";
       public AlipayBiz()
       { 
       
       }

       public AlipayBiz(string pid, string key, string charset, string signtype)
       {
           m_pid = pid;
           m_key = key;
           if (!string.IsNullOrEmpty(charset)) m_charset = charset.ToLower () ;
           if (!string.IsNullOrEmpty(signtype)) m_signtype = signtype.ToUpper();
       }
       /// <summary>
       /// 条码支付
       /// </summary>
       /// <param name="seller_email">卖家账号</param>
       /// <param name="out_trade_no">系统唯一流水号</param>
       /// <param name="subject">交易名称</param>
       /// <param name="total_fee">金额</param>
       ///<param name="barCode">支付宝当面付条码</param>
       /// <returns></returns>
       public  string BARCODE_PAY_OFFLINE(string seller_email, string out_trade_no, string subject, string total_fee, string barCode,string oprID)
       {
           ////////////////////////////////////////////请求参数////////////////////////////////////////////

           ////卖家支付宝帐户
           //string seller_email;
           ////必填

           ////商户订单号
           //string out_trade_no ;
           ////商户网站订单系统中唯一订单号，必填

           ////订单名称
           //string subject ;
           ////必填

           ////付款金额
           //string total_fee ;
           //必填

           //订单业务类型
           string product_code = "BARCODE_PAY_OFFLINE";
           //SOUNDWAVE_PAY_OFFLINE：声波支付，FINGERPRINT_FAST_PAY：指纹支付，BARCODE_PAY_OFFLINE：条码支付

           //动态ID类型
           string dynamic_id_type = "barcode";
           //soundwave：声波，qrcode：二维码，barcode：条码

           //动态ID
           //string dynamic_id = Guid .NewGuid ().ToString ().Replace ("-","");
           //例如3856957008a73b7d

           ////////////////////////////////////////////////////////////////////////////////////////////////

           //把请求参数打包成数组
           SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
           sParaTemp.Add("partner", m_pid );
           sParaTemp.Add("_input_charset", m_charset);
           sParaTemp.Add("service", "alipay.acquire.createandpay");
           sParaTemp.Add("seller_email", seller_email);
           sParaTemp.Add("out_trade_no", out_trade_no);
           sParaTemp.Add("subject", subject);
           sParaTemp.Add("total_fee", total_fee);
           sParaTemp.Add("product_code", product_code);
           sParaTemp.Add("dynamic_id_type", dynamic_id_type);
           sParaTemp.Add("dynamic_id", barCode);
           //operator_id
           sParaTemp.Add("operator_id", oprID);
           //extend_params
           // sParaTemp.Add("extend_params", barCode);
           //建立请求
           AlipaySubmit submit = new AlipaySubmit(m_key, m_charset, m_signtype);
           string sHtmlText = submit.BuildRequest(sParaTemp);
           
           return sHtmlText;
       }


       /// <summary>
       /// 条码支付
       /// </summary>
       /// <param name="seller_email">卖家账号</param>
       /// <param name="out_trade_no">系统唯一流水号</param>
       /// <param name="subject">交易名称</param>
       /// <param name="total_fee">金额</param>
       /// <param name="SHOP_ID"></param>
       /// <param name="STORE_TYPE"></param>
       /// <param name="STORE_ID"></param>
       /// <param name="TERMINAL_ID"></param>
       ///<param name="barCode">支付宝当面付条码</param>
       /// <returns></returns>
       public  string BARCODE_PAY_OFFLINE_Ext(string seller_email, string out_trade_no, string subject, string total_fee, string barCode, string STORE_ID, string STORE_TYPE, string TERMINAL_ID, string SHOP_ID,string oprID)
       {
           ////////////////////////////////////////////请求参数////////////////////////////////////////////

           ////卖家支付宝帐户
           //string seller_email;
           ////必填

           ////商户订单号
           //string out_trade_no ;
           ////商户网站订单系统中唯一订单号，必填

           ////订单名称
           //string subject ;
           ////必填

           ////付款金额
           //string total_fee ;
           //必填

           //订单业务类型
           string product_code = "BARCODE_PAY_OFFLINE";
           //SOUNDWAVE_PAY_OFFLINE：声波支付，FINGERPRINT_FAST_PAY：指纹支付，BARCODE_PAY_OFFLINE：条码支付

           //动态ID类型
           string dynamic_id_type = "barcode";
           //soundwave：声波，qrcode：二维码，barcode：条码

           //动态ID
           //string dynamic_id = Guid .NewGuid ().ToString ().Replace ("-","");
           //例如3856957008a73b7d

           ////////////////////////////////////////////////////////////////////////////////////////////////

           //把请求参数打包成数组
           SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
           sParaTemp.Add("partner", m_pid);
           sParaTemp.Add("_input_charset", m_charset);
           sParaTemp.Add("service", "alipay.acquire.createandpay");
           sParaTemp.Add("seller_email", seller_email);
           sParaTemp.Add("out_trade_no", out_trade_no);
           sParaTemp.Add("subject", subject);
           sParaTemp.Add("total_fee", total_fee);
           sParaTemp.Add("product_code", product_code);
           sParaTemp.Add("dynamic_id_type", dynamic_id_type);
           sParaTemp.Add("dynamic_id", barCode);
           if(!string.IsNullOrEmpty (oprID ))   sParaTemp.Add("operator_id", oprID );
           //extend_params
           string extInfo = "{\"AGENT_ID\":\"11864042a1\"";
           if (!string.IsNullOrEmpty(STORE_TYPE))
           {
               extInfo += ",\"STORE_TYPE\":\"" + STORE_TYPE + "\"";
           }

           if (!string.IsNullOrEmpty(SHOP_ID))
           {
               extInfo += ",\"SHOP_ID\":\"" + SHOP_ID + "\"";
           }


           if (!string.IsNullOrEmpty(TERMINAL_ID))
           {
               extInfo += ",\"TERMINAL_ID\":\"" + TERMINAL_ID + "\"";
           }

           if (!string.IsNullOrEmpty(STORE_ID))
           {
               extInfo += ",\"STORE_ID\":\"" + STORE_ID + "\"";
           }

           extInfo += "}";
           sParaTemp.Add("extend_params", extInfo);
           //建立请求
           AlipaySubmit submit = new AlipaySubmit(m_key, m_charset, m_signtype);
           string sHtmlText = submit.BuildRequest(sParaTemp);

           return sHtmlText;
       }

       /// <summary>
       /// 退款
       /// </summary>
       /// <param name="out_trade_no">本地唯一订单号【支付成功的】</param>
       /// <param name="alipayOrderNo">支付宝返回的订单号</param>
       /// <param name="refund_amount">退款金额</param>
       /// <param name="operator_type">操作员类型</param>
       /// <param name="operator_id">操作员编号</param>
       /// <param name="refund_reason">退款理由</param>
       /// <returns>支付宝返回xml格式文本</returns>
       public  string Refund(string out_trade_no, string alipayOrderNo, string refund_amount, string operator_type, string operator_id, string refund_reason)
       {
           ////////////////////////////////////////////请求参数////////////////////////////////////////////

           ////卖家支付宝帐户
           //string seller_email;
           ////必填

           ////商户订单号
           //string out_trade_no ;
           ////商户网站订单系统中唯一订单号，必填

           ////订单名称
           //string subject ;
           ////必填

           ////付款金额
           //string total_fee ;
           //必填

           //订单业务类型
           // string product_code = "BARCODE_PAY_OFFLINE";
           //SOUNDWAVE_PAY_OFFLINE：声波支付，FINGERPRINT_FAST_PAY：指纹支付，BARCODE_PAY_OFFLINE：条码支付

           //动态ID类型
           // string dynamic_id_type = "bar_code";
           //soundwave：声波，qrcode：二维码，barcode：条码  bar_code

           //动态ID
           //string dynamic_id = Guid.NewGuid().ToString().Replace("-", "");
           //例如3856957008a73b7d

           ////////////////////////////////////////////////////////////////////////////////////////////////

           //把请求参数打包成数组
           SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
           sParaTemp.Add("partner", m_pid);
           sParaTemp.Add("_input_charset",m_charset);
           sParaTemp.Add("service", "alipay.acquire.refund");
           sParaTemp.Add("out_trade_no", out_trade_no);
           sParaTemp.Add("refund_amount", refund_amount);
           if(!string .IsNullOrEmpty (alipayOrderNo ))  sParaTemp.Add("trade_no", alipayOrderNo);
           sParaTemp.Add("operator_type", operator_type);
           if(!string.IsNullOrEmpty (operator_id))  sParaTemp.Add("operator_id", operator_id);
           sParaTemp.Add("refund_reason", refund_reason);
           //  sParaTemp.Add("dynamic_id", dynamic_id);

           //建立请求
           AlipaySubmit submit = new AlipaySubmit(m_key, m_charset, m_signtype);
           string sHtmlText = submit.BuildRequest(sParaTemp);

           return sHtmlText;
       }

       /// <summary>
       /// 取消订单
       /// </summary>
       /// <param name="out_trade_no">本地唯一订单号【支付成功的】</param>
       /// <param name="alipayOrderNo">支付宝返回的订单号</param>
       /// <param name="operator_type">操作员类型：支付宝操作员：0 本地操作员1 ，一般使用1</param>
       /// <param name="operator_id">操作员编号</param>
       /// <returns></returns>
       public  string Cancel(string out_trade_no, string alipayOrderNo, string operator_type, string operator_id)
       {
           ////////////////////////////////////////////请求参数////////////////////////////////////////////

           ////卖家支付宝帐户
           //string seller_email;
           ////必填

           ////商户订单号
           //string out_trade_no ;
           ////商户网站订单系统中唯一订单号，必填

           ////订单名称
           //string subject ;
           ////必填

           ////付款金额
           //string total_fee ;
           //必填

           //订单业务类型
           // string product_code = "BARCODE_PAY_OFFLINE";
           //SOUNDWAVE_PAY_OFFLINE：声波支付，FINGERPRINT_FAST_PAY：指纹支付，BARCODE_PAY_OFFLINE：条码支付

           //动态ID类型
           // string dynamic_id_type = "bar_code";
           //soundwave：声波，qrcode：二维码，barcode：条码  bar_code

           //动态ID
           //string dynamic_id = Guid.NewGuid().ToString().Replace("-", "");
           //例如3856957008a73b7d

           ////////////////////////////////////////////////////////////////////////////////////////////////

           //把请求参数打包成数组
           SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
           sParaTemp.Add("partner", m_pid );
           sParaTemp.Add("_input_charset", m_charset);
           sParaTemp.Add("service", "alipay.acquire.cancel");

           sParaTemp.Add("out_trade_no", out_trade_no);
           if(!string.IsNullOrEmpty (alipayOrderNo )) sParaTemp.Add("trade_no", alipayOrderNo);
           sParaTemp.Add("operator_type", operator_type);
           if(!string.IsNullOrEmpty (operator_id )) sParaTemp.Add("operator_id", operator_id);
          
           //  sParaTemp.Add("dynamic_id", dynamic_id);

           //建立请求
           AlipaySubmit submit = new AlipaySubmit(m_key, m_charset, m_signtype);
           string sHtmlText = submit.BuildRequest(sParaTemp);

           return sHtmlText;
       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="out_trade_no"></param>
       /// <param name="alipayOrderNo"></param>
       /// <returns></returns>
       public  string Query(string out_trade_no, string alipayOrderNo)
       {
           ////////////////////////////////////////////请求参数////////////////////////////////////////////

           ////卖家支付宝帐户
           //string seller_email;
           ////必填

           ////商户订单号
           //string out_trade_no ;
           ////商户网站订单系统中唯一订单号，必填

           ////订单名称
           //string subject ;
           ////必填

           ////付款金额
           //string total_fee ;
           //必填

           //订单业务类型
           // string product_code = "BARCODE_PAY_OFFLINE";
           //SOUNDWAVE_PAY_OFFLINE：声波支付，FINGERPRINT_FAST_PAY：指纹支付，BARCODE_PAY_OFFLINE：条码支付

           //动态ID类型
           // string dynamic_id_type = "bar_code";
           //soundwave：声波，qrcode：二维码，barcode：条码  bar_code

           //动态ID
           //string dynamic_id = Guid.NewGuid().ToString().Replace("-", "");
           //例如3856957008a73b7d

           ////////////////////////////////////////////////////////////////////////////////////////////////

           //把请求参数打包成数组
           SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
           sParaTemp.Add("partner", m_pid);
           sParaTemp.Add("_input_charset", m_charset);
           sParaTemp.Add("service", "alipay.acquire.query");

           sParaTemp.Add("out_trade_no", out_trade_no);
           if(!string.IsNullOrEmpty (alipayOrderNo))  sParaTemp.Add("trade_no", alipayOrderNo);

           //  sParaTemp.Add("dynamic_id", dynamic_id);

           //建立请求
           AlipaySubmit submit = new AlipaySubmit(m_key, m_charset, m_signtype);
           string sHtmlText = submit.BuildRequest(sParaTemp);

           return sHtmlText;
       }
   }
}
