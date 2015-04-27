using System;
using System.Collections.Generic;
using System.Text;

namespace HYModel
{

  
    public class PayResult
    {
        /// <summary>
        /// 返回编码
        /// 成功：0000；系统异常：9999;
        /// 参数异常8开头,
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 返回描述信息
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 本地流水号
        /// </summary>
        public string FlowNo { get; set; }
        /// <summary>
        /// 备注信息
        /// 目前使用渠道返回信息
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 支付结果明细
        /// </summary>
        public ChannelInfo ChannelPayDetail { get; set; }
    }

    //public class ChannelPayInfo
    //{
    //    /// <summary>
    //    /// 卖家账号
    //    /// </summary>
    //    public string SellerID { get; set; }

    //    /// <summary>
    //    /// 买家账号
    //    /// </summary>
    //    public string BuyerID { get; set; }
    //    /// <summary>
    //    /// 买家名称
    //    /// </summary>
    //    public string BuyerName { get; set; }
    //    /// <summary>
    //    /// 金额
    //    /// </summary>
    //    public decimal Ammount { get; set; }
    //    /// <summary>
    //    /// 渠道流水号
    //    /// </summary>
    //    public string ChannelFlowNo { get; set; }
    //    /// <summary>
    //    /// 渠道支付结果
    //    /// </summary>
    //    public string ChannelStatus { get; set; }
      

    //}

    public class RefundResult
    {
        /// <summary>
        /// 返回编码
        /// 成功：0000；系统异常：9999;
        /// 参数异常8开头,
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 返回描述信息
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 本地流水号
        /// </summary>
        public string FlowNo { get; set; }
        /// <summary>
        /// 备注信息
        /// 目前使用渠道返回信息
        /// </summary>
        public string Note { get; set; }

        public ChannelInfo ChannelRefundDetail { get; set; }
    }

    public class QueryResult
    {
        /// <summary>
        /// 返回编码
        /// 成功：0000；系统异常：9999;
        /// 参数异常8开头,
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 返回描述信息
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 本地流水号
        /// </summary>
        public string FlowNo { get; set; }
        /// <summary>
        /// 备注信息
        /// 目前使用渠道返回信息
        /// </summary>
        public string Note { get; set; }

        public ChannelQueryInfo ChannelQueryDetail { get; set; }
    }


    public class CancelResult
    {
        /// <summary>
        /// 返回编码
        /// 成功：0000；系统异常：9999;
        /// 参数异常8开头,
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 返回描述信息
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 本地流水号
        /// </summary>
        public string FlowNo { get; set; }
        /// <summary>
        /// 备注信息
        /// 目前使用渠道返回信息
        /// </summary>
        public string Note { get; set; }

        public ChannelCancelInfo ChannelCancelDetail { get; set; }
    }
    public class ChannelInfo
    {
        /// <summary>
        /// 卖家账号
        /// </summary>
        public string SellerID { get; set; }

        /// <summary>
        /// 买家账号
        /// </summary>
        public string BuyerID { get; set; }
        /// <summary>
        /// 买家名称
        /// </summary>
        public string BuyerName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Ammount { get; set; }
        /// <summary>
        /// 渠道流水号
        /// </summary>
        public string ChannelFlowNo { get; set; }
      
        /// <summary>
        /// 渠道结果代码
        /// </summary>
        public string ChannelResultCode { get; set; }

        /// <summary>
        /// 详细错误代码
        /// </summary>
        public string DetailErrorCode { get; set; }
        /// <summary>
        /// 详细错误描述
        /// </summary>
        public string DetailErrorDesc { get; set; }


    }

    public class ChannelCancelInfo
    {
        /// <summary>
        /// 卖家账号
        /// </summary>
        public string SellerID { get; set; }

        /// <summary>
        /// 买家账号
        /// </summary>
        public string BuyerID { get; set; }
        /// <summary>
        /// 买家名称
        /// </summary>
        public string BuyerName { get; set; }
       
        /// <summary>
        /// 渠道流水号
        /// </summary>
        public string ChannelFlowNo { get; set; }

        /// <summary>
        /// 渠道结果代码
        /// </summary>
        public string ChannelResultCode { get; set; }

        /// <summary>
        /// 详细错误代码
        /// </summary>
        public string DetailErrorCode { get; set; }
        /// <summary>
        /// 详细错误描述
        /// </summary>
        public string DetailErrorDesc { get; set; }


    }

    public class ChannelQueryInfo
    {
        /// <summary>
        /// 卖家账号
        /// </summary>
        public string SellerID { get; set; }

        /// <summary>
        /// 买家账号
        /// </summary>
        public string BuyerID { get; set; }
        /// <summary>
        /// 买家名称
        /// </summary>
        public string BuyerName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Ammount { get; set; }
        /// <summary>
        /// 渠道流水号
        /// </summary>
        public string ChannelFlowNo { get; set; }
        /// <summary>
        /// 渠道返回状态
        /// </summary>
        public string ChannelStatus { get; set; }
        /// <summary>
        /// 渠道结果代码
        /// </summary>
        public string ChannelResultCode { get; set; }


    }
}
