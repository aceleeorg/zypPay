using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;

namespace Alipay
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
    /// </summary>
    public class Config
    {
        #region 字段
        private static string partner = "";
        private static string key = "";
        private static string input_charset = "";
        private static string sign_type = "";
        #endregion

        static Config()
        {
            //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

            //合作身份者ID，以2088开头由16位纯数字组成的字符串
            partner = "2088201565141845";
            if (System.Configuration.ConfigurationManager.AppSettings["PID"] != null)
            {
                partner = System.Configuration.ConfigurationManager.AppSettings["PID"];
            }

            //交易安全检验码，由数字和字母组成的32位字符串
            key = "sstyxmh3erotgsjpbgukdz2y5dde46o0";
            if (System.Configuration.ConfigurationManager.AppSettings["PKey"] != null)
            {
                key = System.Configuration.ConfigurationManager.AppSettings["PKey"];
            }
            //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑



            //字符编码格式 目前支持 gbk 或 utf-8
            input_charset = "utf-8";

            //签名方式，选择项：RSA、DSA、MD5
            sign_type = "MD5";
        }

      

        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public static string Partner
        {
            get { return partner; }
            set { partner = value; }
        }

        /// <summary>
        /// 获取或设交易安全校验码
        /// </summary>
        public static string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public static string Input_charset
        {
            get { return input_charset; }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public static string Sign_type
        {
            get { return sign_type; }
        }
        #endregion
    }

    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
    /// </summary>
    public class AlipayConfig
    {
        #region 字段
        private  string partner = "";
        private  string key = "";
        private  string input_charset = "";
        private  string sign_type = "";
        #endregion

        public AlipayConfig(string pid,string inputkey,string charset,string signtype)
        {
            //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

            //合作身份者ID，以2088开头由16位纯数字组成的字符串
            //partner = "2088201565141845";
            partner = pid;
          

            //交易安全检验码，由数字和字母组成的32位字符串
            //key = "sstyxmh3erotgsjpbgukdz2y5dde46o0";
            key = inputkey;
          
            //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑



            //字符编码格式 目前支持 gbk 或 utf-8
            input_charset = "utf-8";
            if (!string.IsNullOrEmpty(charset))
            {
                input_charset = charset;
            }

            //签名方式，选择项：RSA、DSA、MD5
            sign_type = "MD5";
            if (!string.IsNullOrEmpty(signtype))
            {
                sign_type = signtype;
            }
        }



        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public  string Partner
        {
            get { return partner; }
            set { partner = value; }
        }

        /// <summary>
        /// 获取或设交易安全校验码
        /// </summary>
        public  string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public  string Input_charset
        {
            get { return input_charset; }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public  string Sign_type
        {
            get { return sign_type; }
        }
        #endregion
    }
}