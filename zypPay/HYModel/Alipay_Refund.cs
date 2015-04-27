using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace HYModel
{
     [XmlRoot(ElementName = "alipay")]
   public class Alipay_Refund
    {
        //is_success
         [XmlElement(ElementName = "is_success")]
         public string is_success { get; set; }

         [XmlElement(ElementName = "sign")]
         public string sign { get; set; }


         [XmlElement(ElementName = "sign_type")]
         public string sign_type { get; set; }

         //[XmlElement(ElementName = "request")]
         //public Alipay_RefundRequest[] Requsets
         //{
         //    get;
         //    set;
         //}


         [XmlElement(ElementName = "response")]
         public Alipay_Response[] Response
         {
             get;
             set;
         }



         [XmlElement(ElementName = "error")]
         public string error { get; set; }

      
    }

     
     //public class Alipay_RefundRequest
     //{
     //    [XmlElement(ElementName = "loginId")]
     //    public string loginId { get; set; }
     //}



     public class Alipay_Response
     {
         [XmlElement(ElementName = "alipay")]
         public Alipay_ResponseAlipay[] ReponseAlipays { get; set; }
     }

     public class Alipay_QueryResponse
     {
         [XmlElement(ElementName = "alipay")]
         public Alipay_QueryResponseAlipay[] ReponseAlipays { get; set; }
     }

     public class Alipay_ResponseAlipay
     {
         [XmlElement(ElementName = "result_code")]
         public string result_code { get; set; }

         [XmlElement(ElementName = "buyer_logon_id")]
         public string buyer_logon_id { get; set; }


         [XmlElement(ElementName = "buyer_user_id")]
         public string buyer_user_id { get; set; }

         [XmlElement(ElementName = "partner")]
         public string partner { get; set; }

         [XmlElement(ElementName = "fund_change")]
         public string fund_change { get; set; }

         [XmlElement(ElementName = "out_trade_no")]
         public string out_trade_no { get; set; }

         [XmlElement(ElementName = "trade_no")]
         public string trade_no { get; set; }

         [XmlElement(ElementName = "detail_error_code")]
         public string detail_error_code { get; set; }


         [XmlElement(ElementName = "detail_error_des")]
         public string detail_error_des { get; set; }


         [XmlElement(ElementName = "trade_status")]
         public string trade_status { get; set; }

         //retry_flag
         [XmlElement(ElementName = "retry_flag")]
         public string retry_flag { get; set; }


        
     }

     public class Alipay_QueryResponseAlipay
     {
         [XmlElement(ElementName = "result_code")]
         public string result_code { get; set; }

         [XmlElement(ElementName = "buyer_logon_id")]
         public string buyer_logon_id { get; set; }


         [XmlElement(ElementName = "buyer_user_id")]
         public string buyer_user_id { get; set; }

         [XmlElement(ElementName = "partner")]
         public string partner { get; set; }

         [XmlElement(ElementName = "fund_change")]
         public string fund_change { get; set; }

         [XmlElement(ElementName = "out_trade_no")]
         public string out_trade_no { get; set; }

         [XmlElement(ElementName = "trade_no")]
         public string trade_no { get; set; }

         [XmlElement(ElementName = "detail_error_code")]
         public string detail_error_code { get; set; }


         [XmlElement(ElementName = "detail_error_des")]
         public string detail_error_des { get; set; }


         [XmlElement(ElementName = "trade_status")]
         public string trade_status { get; set; }

         //retry_flag
         [XmlElement(ElementName = "retry_flag")]
         public string retry_flag { get; set; }

          

         //[XmlElement(ElementName = "fund_bill_list")]
         //public string fund_bill_list { get; set; }

         [XmlElement(ElementName = "total_fee")]
         public string total_fee { get; set; }

         [XmlElement(ElementName = "send_pay_date")]
         public string send_pay_date { get; set; }
        
     }


     
}
