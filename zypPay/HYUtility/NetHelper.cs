using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;
using System.Configuration;

namespace HYUtility
{
    public class NetHelper
    {
        private const int INTERNET_CONNECTION_MODEM = 1;
        private const int INTERNET_CONNECTION_LAN = 2;
        [DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(
        ref   int dwFlag,
        int dwReserved
        );

        ///// <summary>
        ///// 判断网络的连接状态
        ///// </summary>
        ///// <returns>
        ///// 网络状态(1-->未联网;2-->采用调治解调器上网;3-->采用网卡上网)
        /////</returns>
        //public static int GetNetConStatus(string dbConnectName)
        //{
        //    int iNetStatus = 0;
        //    System.Int32 dwFlag = new int();
        //    if (!InternetGetConnectedState(ref dwFlag, 0))
        //    {  
        //        //没有能连上互联网
        //        iNetStatus = 1;
        //    }
        //    else if ((dwFlag & INTERNET_CONNECTION_MODEM) != 0)
        //    {

        //        DbHelper dbHelper = new DbHelper(dbConnectName);
        //        if (string.IsNullOrEmpty(dbHelper.LastError))
        //        {
        //            iNetStatus = 0;
        //        }
        //        else
        //        {
        //            iNetStatus = 1;
        //        }
             
        //    }
            
        //    else if ((dwFlag & INTERNET_CONNECTION_LAN) != 0)
        //    {
        //        DbHelper dbHelper = new DbHelper(dbConnectName);
        //        if (string.IsNullOrEmpty(dbHelper.LastError))
        //        {
        //            iNetStatus = 0;
        //        }
        //        else
        //        {
        //            iNetStatus = 1;
        //        }
        //    }

        //    return iNetStatus;
        //}

        /// <summary>
        /// 判断网络的连接状态
        /// </summary>
        /// <returns>
        /// 网络状态(1-->未联网;0 正常)
        ///</returns>
        public static int GetNetConStatus(string strNetAddress)
        {
            int iNetStatus = 0;
            System.Int32 dwFlag = new int();
            if (!InternetGetConnectedState(ref dwFlag, 0))
            {
                //没有能连上互联网
                iNetStatus = 1;
            }
            else if ((dwFlag & INTERNET_CONNECTION_MODEM) != 0)
            {
                //采用调治解调器上网,需要进一步判断能否登录具体网站
                if (PingNetAddress(strNetAddress))
                {
                    //可以ping通给定的网址,网络OK  2
                    iNetStatus = 0;
                }
                else
                {
                    //不可以ping通给定的网址,网络不OK  4
                    iNetStatus = 1;
                }
            }

            else if ((dwFlag & INTERNET_CONNECTION_LAN) != 0)
            {
                //采用网卡上网,需要进一步判断能否登录具体网站
                if (PingNetAddress(strNetAddress))
                {
                    //可以ping通给定的网址,网络OK 3
                    iNetStatus = 0;
                }
                else
                {
                    //不可以ping通给定的网址,网络不OK 5
                    iNetStatus = 1;
                }
            }

            return iNetStatus;
        }

        /// <summary>
        /// ping 具体的网址看能否ping通
        /// </summary>
        /// <param name="strNetAdd"></param>
        /// <returns></returns>
        private static bool PingNetAddress(string strNetAdd)
        {
            bool Flage = false;
            Ping ping = new Ping();
            try
            {
                PingReply pr = ping.Send(strNetAdd, 1000);
                if (pr.Status == IPStatus.TimedOut)
                {
                    Flage = false;
                }
                if (pr.Status == IPStatus.Success)
                {
                    Flage = true;
                }
                else
                {
                    Flage = false;
                }
            }
            catch
            {
                Flage = false;
            }
            return Flage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static bool IsWebServiceAvaiable(string url, int timeOut)
        {
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                bool useProxy = false;
                if (ConfigurationManager.AppSettings["UseProxy"] != null)
                {
                    if (ConfigurationManager.AppSettings["UseProxy"].ToString() == "1")
                    {
                        useProxy = true;
                    }
                }
              
                if (useProxy)
                {
                    string strDomain = ConfigurationManager.AppSettings["Domain"].ToString();
                    //域访问名
                    string strUserName = ConfigurationManager.AppSettings["UserName"].ToString();
                    //域访问密码
                    string strPassWord = ConfigurationManager.AppSettings["PassWord"].ToString();
                    //代理地址
                    string strHost = ConfigurationManager.AppSettings["Host"].ToString();
                    //代理端口
                    int strPort = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());
                    //设置代理
                    System.Net.WebProxy oWebProxy = new System.Net.WebProxy(strHost, strPort);

                    // 获取或设置提交给代理服务器进行身份验证的凭据
                    oWebProxy.Credentials = new System.Net.NetworkCredential(strUserName, strPassWord, strDomain);

                   
                    myHttpWebRequest.Proxy = oWebProxy;
                }
                myHttpWebRequest.Timeout = timeOut*1000;
                using (HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse())
                {
                    return true;
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }
        /// <summary>
        /// 检查WebService是否可用，用于wsdl
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool CheckActiveWebService(string url,int timeOut)
        {
            try
            {
                string uri = url;
                if (!url.ToLower ().Contains(".asmx"))
                {
                     uri = url + "?wsdl";
                }
               
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

                request.UseDefaultCredentials = true;
                request.Method = "GET";
                request.Timeout = timeOut;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentType.Substring(0, 8) == "text/xml")
                {
                    request.Abort();
                    response.Close();
                  
                    return true;
                }
                else
                {
                    request.Abort();
                    response.Close();
                    return false;
                }

                //return true;
            }
            catch (WebException e)
            {
                //response.Close();
                System.Diagnostics.Trace.Write(e.Message);
                return false;
            }
        }

        

    }

}
