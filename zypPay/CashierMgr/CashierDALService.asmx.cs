using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using HYCashierDAL;
using System.Data;

namespace CashierMgr
{
    /// <summary>
    /// CashierDALService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class CashierDALService : System.Web.Services.WebService
    {

        [WebMethod]
        public bool CheckOprInfo(string oprID, string oprpwd, string clientID)
        {
            bool flag = false;
            HYDal hydal=new HYDal ();
           
            flag = hydal.CheckOprInfo(oprID, oprpwd, clientID);


            return flag;
        }

        [WebMethod]
        public DataSet GetOprInfo(string oprID, string oprpwd, string clientID)
        {
            DataSet ds = null;
            HYDal hydal = new HYDal();

            ds = hydal.GetOprInfo(oprID, oprpwd, clientID);


            return ds;
        }
         [WebMethod]
        public bool IsAdmin(string oprID, string oprpwd)
        {
            bool flag = false;

            HYDal hydal = new HYDal();
            flag = hydal.IsAdmin(oprID, oprpwd);


            return flag;
        }
         [WebMethod]
        public bool InsertTradeInfo(string orderNo, string amount, string tradetype, string porderNo, string oprID, string channel, string clientID, string tid)
        {
            bool flag = false;
            HYDal hydal = new HYDal();
            flag = hydal.InsertTradeInfo(orderNo, amount, tradetype, porderNo, oprID, channel, clientID, tid, orderNo);


            return flag;
        }
         [WebMethod]
        public bool InsertBackTradeInfo(string orderNo, string amount, string tradetype, string porderNo, string oprID, string channel, string clientID, string tid, string oldLocalNo, string oldServerNo)
        {
            bool flag = false;
            HYDal hydal = new HYDal();
            flag = hydal.InsertBackTradeInfo(orderNo, amount, tradetype, porderNo, oprID, channel, clientID, tid, oldLocalNo, oldServerNo);


            return flag;
        }
         [WebMethod]
        public bool InsertTradeInfoWithStatus(string orderNo, string amount, string tradetype, string porderNo, string oprID, string channel, string status, string clientID, string tid)
        {
            bool flag = false;
            //if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            HYDal hydal = new HYDal();

            flag = hydal.InsertTradeInfoWithStatus(orderNo, amount, tradetype, porderNo, oprID, channel, status, clientID, tid);


            return flag;
        }
         [WebMethod]
         public string GetSettingValue(string settingName)
         {
             string strValue = "";
             HYDal hydal = new HYDal();
             strValue = hydal.GetSettingValue(settingName);
             return strValue;
         }


         [WebMethod]
        public bool UpdateTradeInfo(string orderNo, string serverflowno, string tradeStatus, string serverReponse, string buyID, string buyAccount, string errorCD)
        {
            bool flag = false;
            HYDal hydal = new HYDal();
            flag = hydal.UpdateTradeInfo(orderNo, serverflowno, tradeStatus, serverReponse, buyID, buyAccount, errorCD);


            return flag;
        }
         [WebMethod]
        public bool UpdateTradeStatus(string orderNo, string tradeStatus)
        {
            bool flag = false;
            HYDal hydal = new HYDal();
            flag = hydal.UpdateTradeStatus(orderNo, tradeStatus);


            return flag;
        }
         [WebMethod]
        public bool SameOrderID(string orderID)
        {
            bool flag = false;

            HYDal hydal = new HYDal();
            flag = hydal.SameOrderID(orderID);

            return flag;
        }
         [WebMethod]
        public int GetTradeCount1(string startTime, string endTime, string status, string flowNo, string tradeType)
        {
            int count = 0;
            // if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            HYDal hydal = new HYDal();
            count = hydal.GetTradeCount(startTime, endTime, status, flowNo, tradeType);
            return count;
        }
         [WebMethod]
         public DataSet GetTodayTradeInfo(string oprID, string clientID, string channel, bool isAdmin)
        {
            DataSet dt = null;
            HYDal hydal = new HYDal();

            dt = hydal.GetTodayTradeInfo(oprID, clientID, channel, isAdmin);

            return dt;
        }
         [WebMethod]
         public DataSet GetTradeInfo(string startTime, string endTime, string status, string flowNo, string tradeType, string channel, string clientID, string oprID, int start, int end, bool isAdmin)
        {
            DataSet dt = null;
            HYDal hydal = new HYDal();
            dt = hydal.GetTradeInfo(startTime, endTime, status, flowNo, tradeType, channel, clientID, oprID, start, end, isAdmin); 
            return dt;
        }
         [WebMethod]
         public DataSet GetAllTradeInfo(string startTime, string endTime, string status, string flowNo, string tradeType, string channel, string clientID, string oprID, bool isAdmin)
        {
            DataSet dt = null;
            // if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            HYDal hydal = new HYDal();
            dt = hydal.GetAllTradeInfo(startTime, endTime, status, flowNo, tradeType, channel, clientID, oprID, isAdmin);
            return dt;
        }

         [WebMethod]
        public int GetTradeCount(string startTime, string endTime, string status, string flowNo, string tradeType, string channel, string clientID, string oprID, bool isAdmin)
        {
            int count = 0;
            HYDal hydal = new HYDal();
            count = hydal.GetTradeCount(startTime, endTime, status, flowNo, tradeType,channel,clientID,oprID,isAdmin);
            return count;
        }


         [WebMethod]
         public DataSet GetTradeStatistic(string startTime, string endTime, string status, string flowNo, string tradeType, string channel, string clientID, string oprID, bool isAdmin)
        {
            DataSet dt = null;
            HYDal hydal = new HYDal();
            dt = hydal.GetTradeStatistic(startTime, endTime, status, flowNo, tradeType, channel, clientID, oprID, isAdmin);
            return dt;
        }

     
    }
}
