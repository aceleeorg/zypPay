using System;
using System.Collections.Generic;
using System.Text;
using System.Dynamic;
using Newtonsoft.Json;
namespace CashierPayHelper
{
    /// <summary>
    /// 
    /// </summary>
    public  class PayUtil
    {
        public static string GetDynamicValue(dynamic curDy,string name)
        {
            string result = "";
            try
            {
                result = curDy.name.Value;
            }
            catch
            {

            }
            return result;

        }
        /// <summary>
        /// 对于填写的支付字段进行判断,是否是必填写项，以及填写的字段是否过长。
        /// </summary>
        /// <param name="payResult"></param>
        /// <param name="curField">当前字段中文意思 </param>
        /// <param name="curFieldValue">当前字段如果为 “-9999”，则表示非必填写项</param>       
        /// <param name="return_code">输入的错误提示代码</param>
        /// <param name="isCheckNullOrEmpty">如果为真，则去检验是否为空，该项为必填</param>
        /// <param name="maxLength">允许的最大字符数</param>
        /// <returns></returns>
        public static string checkField(CashierPayHelper.BaseErrorInfo payResult, string curField, string curFieldValue, string return_code, bool isCheckNullOrEmpty, int maxLength)
        {
            string strRes = "";
            ///如果为空，且是必填写项
            if (string.IsNullOrEmpty(curFieldValue) && isCheckNullOrEmpty)
            {
                payResult.return_code = return_code;// "FAIL";
                payResult.return_msg = getReturnCode_msg(curField,return_code); // curField + "未填写，该项是必填写项";
                strRes = JsonConvert.SerializeObject(payResult);
                return strRes;
            }
            ///如果有最大长度限制，则返回判断下填写的字符是否大于最大限制
            if (maxLength > 0 && !string.IsNullOrEmpty(curFieldValue))
            {

                int len = System.Text.Encoding.Default.GetBytes(curFieldValue).Length;
                if (len > maxLength)
                {
                    payResult.return_code = "FAIL";
                    payResult.return_msg = curField + "超过了最大字符长度:" + maxLength+" 位";
                    strRes = JsonConvert.SerializeObject(payResult);
                    return strRes;
                }
            }
            return strRes;

        }
        /// <summary>
        /// 根据代码返回相关提示消息
        /// </summary>
        /// <param name="return_code"></param>
        /// <returns></returns>
        public static string getReturnCode_msg(string fieldName,string return_code) {
            string _msg = "";
            
            switch(return_code){
                case "8001":
                    _msg = "支付类型错误或暂不支持指定的支付类型";
                    break;
                case "8002":
                    _msg ="商户信息不匹配，请校验clientID和clientKey";
                    break;
                case "8003":
                 _msg =   "金额不正确";
                    break;
                case "8004":
                _msg = "商户编号未填写";
                    break;
                case "8005":
                    _msg = "商户key未填写";
                    break;
                case "8006":
                    _msg = "用户渠道编号未填写";
                    break;
                case "8007":
                    _msg = "渠道key未填写";
                    break;
                case "8008":
                    _msg = "终端编号未填写";
                    break;
                case "8009":
                    _msg = "支付金额未填写";
                    break;
                case "8010":
                    _msg = "支付条码号未填写";
                    break;
                case "8011":
                    _msg = "卖家账号未填写";
                    break;
                case "8012":
                    _msg = "商品名称未填写";
                    break;
                case "8013":
                    _msg = "支付类型未填写";
                    break;
                case "-8000":
                    _msg = fieldName + "未填写";
                    break;
                default:
                    _msg = "";
                    break;
            }

            return _msg;
        }

    }
}
