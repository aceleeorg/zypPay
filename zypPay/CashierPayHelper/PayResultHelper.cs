using System;
using System.Collections.Generic;
using System.Text;

namespace CashierPayHelper
{
  
    /// <summary>
    /// 支付结果
    /// </summary>
    public class WeiXinPayResult:TradeInfo
    {
        
    }
    /// <summary>
    /// 定单查询
    /// </summary>
    public class QueryOrder : TradeInfo
    {
        /// <summary>
        /// 交易类型  微信支付（调用接口提交的交易类型，取值如下：JSAPI，NATIVE，APP，MICROPAY），支付宝（），百度钱包
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        ///交易状态   微信支付[ SUCCESS—支付成功   REFUND—转入退款,  NOTPAY—未支付  CLOSED—已关闭 REVOKED—已撤销  USERPAYING--用户支付中 PAYERROR--支付失败(其他原因，如银行返回失败)]
        /// </summary>
        public string trade_state { get; set; }
        /// <summary>
        /// 交易状态描述
        /// </summary>
        public string trade_state_desc { get; set; }

        /// <summary>
        /// 代金券或立减优惠使用数量
        /// </summary>
        public string coupon_count { get; set; }

        /// <summary>
        /// 代金券或立减优惠使用数量
       /// 代金券或立减优惠批次ID  coupon_batch_id_$n
        /// </summary>
        public string coupon_batch_id_ { get; set; }

        /// <summary>
        /// ///代金券或立减优惠批次ID ,$n为下标，从1开始编号
       /// 代金券或立减优惠ID  coupon_id_$n
        /// </summary>
        public string coupon_id_ { get; set; }
        /// <summary>
        /// 代金券或立减优惠ID, $n为下标，从1开始编号
        ///单个代金券或立减优惠支付金额  coupon_fee_$n
        /// </summary>
        public string coupon_fee_ { get; set; }
    }
    /// <summary>
    /// 取消订单
    /// </summary>
    public class CacelResult : TradeInfo
    {
        /// <summary>
        /// 是否重调  是否需要继续调用撤销，Y-需要，N-不需要
        /// </summary>
        public string recall { get; set; }

    }
    /// <summary>
    /// 申请退款
    /// </summary>
    public class RefundResult:TradeInfo
    {
        /// <summary>
        /// 商户退款单号
        /// </summary>
         public string out_refund_no{ get; set; }
         /// <summary>
         /// 支付系统退款单号
         /// </summary>
        public string refund_id{ get; set; }
         /// <summary>
         /// 退款渠道 微信支付（ORIGINAL ORIGINAL—原路退款 BALANCE—退回到余额）
         /// </summary>
         public string refund_channel{ get; set; }
        /// <summary>
        /// 退款金额  退款总金额,单位为分,可以做部分退款
        /// </summary>
         public string refund_fee{ get; set; }
       
      /// <summary>
      /// 现金退款金额
      /// </summary>
       public string cash_refund_fee{get;set;}

       public string coupon_refund_fee { get; set; }
       public string coupon_refund_count { get; set; }

    }
    /// <summary>
    /// 查询退款
    /// </summary>
    public class QueryRefund : TradeInfo
    {
        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string out_refund_no { get; set; }
        /// <summary>
        /// 支付系统退款单号
        /// </summary>
        public string refund_id { get; set; }

        public string refund_count { get; set; }
        /// <summary>
        /// 退款状态  退款状态：微信[ SUCCESS—退款成功 FAIL—退款失败 PROCESSING—退款处理中 NOTSURE—未确定，需要商户原退款单号重新发起 CHANGE—转入代发，退款到银行发现用户的卡作废或者冻结了，导致原路退款银行卡失败，资金回流到商户的现金帐号，需要商户人工干预，通过线下或者财付通转账的方式进行退款 ]
        /// </summary>
        public string refund_status { get; set; }
    }

    public class QueryAndDownLoadBill : TradeInfo
    {
        /// <summary>
        /// 对账单日期  微信格式(20140603)
        /// </summary>
        public string bill_date { get; set; }
        /// <summary>
        /// 账单类型 微信支付(ALL，返回当日所有订单信息，默认值 SUCCESS，返回当日成功支付的订单 REFUND，返回当日退款订单 REVOKED，已撤销的订单)
        /// </summary>
        public string bill_type { get; set; }
    }
    public class BaseErrorInfo
    {
        /// <summary>
        /// 返回状态码  SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// 自定义代码 
        /// "8001";-"支付类型错误或暂不支持指定的支付类型";
        /// "8002";-"商户信息不匹配，请校验clientID和clientKey";
        /// "8003";-"金额不正确";
        /// "8004";-"商户编号未填写";
        /// "8005";-"商户key未填写";
        /// "8006";-"用户渠道编号未填写";
        /// "8007";-"渠道key未填写";
        /// "8008";-"终端编号未填写";
        /// "8009";-"支付金额未填写";
        /// "8010";-"支付条码号未填写";
        /// "8011";-"卖家账号未填写";
        /// "8012";-"商品名称未填写";
        /// "8013";-"支付类型未填写"; 
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息  返回信息，如非空，为错误原因 签名失败 参数格式校验错误
        /// </summary>
        public string return_msg { get; set; }
    }
    public class TradeInfo : BaseErrorInfo
    {
        /// <summary>
        /// 公众账号ID  调用接口提交的公众账号ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号 调用接口提交的商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 商户的密钥
        /// </summary>
        public string API_KEY { get; set; }
        /// <summary>
        /// 用户标识 用户在商户appid 下的唯一标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 由账设备编号
        /// </summary>
        public string device_Info { get; set; }
        /// <summary>
        /// 售货员工号
        /// </summary>
        public string seller_id { get; set; }
        /// <summary>
        /// 售货员姓名
        /// </summary>
        public string seller_name{get;set;}
        /// <summary>
        /// 客户编号
        /// </summary>
        public string customer_id { get; set; }
        /// <summary>
        /// 客户姓名，名称
        /// </summary>
        public string customer_name{get;set;}
        /// <summary>
        /// 系统内部交易号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 微信 、支付宝、百度钱包，内部生成的流水号或交易号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 支付类型为MICROPAY(即扫码支付或是其它)
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见  http://pay.weixin.qq.com/wiki/doc/api/micropay.php?chapter=4_2
        /// </summary>
        public string fee_type { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public string total_fee { get; set; }
        /// <summary>
        /// 商家数据包，原样返回
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 交易完成时间
        /// </summary>
        public string time_end { get; set; }
        /// <summary>
        /// 交易描述信息
        /// </summary>
        public string desc { get; set; }
      
        /// <summary>
        /// 随机字符串  微信返回的随机字符串
        /// </summary>
        public string nonce_str { get; set; }//	是	String(32)	5K8264ILTKCH16CQ2502SI8ZNMTM67VS	微信返回的随机字符串
        ///
        public string sign { get; set; }	//	是	String(32)	C380BEC2BFD727A4B6845133519F3AD6	微信返回的签名，详见签名生成算法
        /// <summary>
        /// 业务结果
        /// </summary>
        public string result_code { get; set; }	//	是	String(16)	SUCCESS	SUCCESS/FAIL
        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code { get; set; }	//	否	String(32)	SYSTEMERROR	详细参见错误列表
        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string err_code_des { get; set; }	///	否	String(128)	系统错误	错误返回的信息描述
        /// <summary>
        /// 交易类型
        /// </summary>
        public string is_subscribe { get; set; }
        /// <summary>
        /// 付款银行
        /// </summary>
        public string bank_type { get; set; }
        /// <summary>
        /// 现金支付货币类型
        /// </summary>
        public string cash_fee_type { get; set; }
        /// <summary>
        /// 现金支付金额
        /// </summary>
        public string cash_fee { get; set; }
        /// <summary>
        /// 代金券或立减优惠金额
        /// </summary>
        public string coupon_fee { get; set; }

    }
 
}
