using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HYCashierDAL
{
    public class HYDal
    {
        //DataTable QueryTable = null;

        MySqlHelper DBHelper = null;
        public HYDal()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            //connectionString = HYUtility.CryptographyHelper.MD5Encrypt("", "HYDB2015");
            //connectionString = HYUtility.CryptographyHelper.MD5Decrypt(connectionString, "HYDB2015");
            DBHelper = new MySqlHelper(connectionString);
        }

        public HYDal(string connectionString)
        {
            DBHelper = new MySqlHelper(connectionString);
        }


        public bool CheckOprInfo(string oprID, string oprpwd, string clientID)
        {
            bool flag = false;

            //if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string sql = "select count(oprid) from oprinfo where oprid='" + oprID + "' and oprpwd='" + oprpwd + "' and clientid='" + clientID + "'";
            object obj = DBHelper.ExecuteScalar(sql);
            flag = int.Parse(obj.ToString()) > 0;


            return flag;
        }

        public DataSet  GetOprInfo(string oprID, string oprpwd, string clientID)
        {
            DataSet ds = null;

            //if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string sql = "select * from oprinfo where oprid='" + oprID + "' and oprpwd='" + oprpwd + "' and clientid='" + clientID + "'";
            ds = DBHelper.ExecuteDataSet(sql);
          


            return ds;
        }

        public string GetSettingValue(string settingName)
        {

            string strValue = "";
            string sql = "select value from CommonSetting where SettingName='" + settingName + "' ";
            object obj = DBHelper.ExecuteScalar(sql);

            if (obj != null) strValue = obj.ToString();
          
            return strValue;
        
        }

        public bool IsAdmin(string oprID, string oprpwd)
        {
            bool flag = false;

            // if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string sql = "select IsAdmin from oprinfo where oprid='" + oprID + "' and oprpwd='" + oprpwd + "'";
            object obj = DBHelper.ExecuteScalar(sql);
            flag = int.Parse(obj.ToString()) > 0;


            return flag;
        }

        public bool InsertTradeInfo(string orderNo, string amount, string tradetype, string porderNo, string oprID, string channel, string clientID, string tid, string terminalOrderNo)
        {
            bool flag = false;
            //if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string tradeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "insert into tradeinfo (localorderno,tradetime,amount,tradetype,oprid,Channel,clientID,TID,TradeStatus,TerminalOrderNo";
            if (!string.IsNullOrEmpty(porderNo))
            {
                sql += ",porderno) values ('" + orderNo + "','" + tradeTime + "'," + amount + ",'" + tradetype + "','" + oprID + "','" + channel + "','" + clientID + "','" + tid + "','NoPay','"+terminalOrderNo +"','" + porderNo + "')";
            }
            else
            {
                sql += ") values  ('" + orderNo + "','" + tradeTime + "'," + amount + ",'" + tradetype + "','" + oprID + "','" + channel + "','" + clientID + "','" + tid + "','NoPay','"+terminalOrderNo +"')";
            }

            flag = DBHelper.ExecuteNonQuery(sql) > 0;


            return flag;
        }

        public bool InsertBackTradeInfo(string orderNo, string amount, string tradetype, string porderNo, string oprID, string channel, string clientID, string tid, string oldLocalNo, string oldServerNo)
        {
            bool flag = false;
            // if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string tradeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "insert into tradeinfo (localorderno,tradetime,amount,tradetype,oprid,Channel,clientID,TID,OldLocalOrderNo,OldServerFlowNo,TradeStatus";
            if (!string.IsNullOrEmpty(porderNo))
            {
                sql += ",porderno) values ('" + orderNo + "','" + tradeTime + "'," + amount + ",'" + tradetype + "','" + oprID + "','" + channel + "','" + clientID + "','" + tid + "','" + oldLocalNo + "','" + oldServerNo + "','NoPay','" + porderNo + "')";
            }
            else
            {
                sql += ") values  ('" + orderNo + "','" + tradeTime + "'," + amount + ",'" + tradetype + "','" + oprID + "','" + channel + "','" + clientID + "','" + tid + "','" + oldLocalNo + "','" + oldServerNo + "','NoPay')";
            }

            flag = DBHelper.ExecuteNonQuery(sql) > 0;


            return flag;
        }

        public bool InsertTradeInfoWithStatus(string orderNo, string amount, string tradetype, string porderNo, string oprID, string channel, string status, string clientID, string tid)
        {
            bool flag = false;
            //if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string tradeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "insert into tradeinfo (localorderno,tradetime,amount,tradetype,oprid,Channel,TradeStatus,clientID,TID";
            if (!string.IsNullOrEmpty(porderNo))
            {
                sql += ",porderno) values ('" + orderNo + "','" + tradeTime + "'," + amount + ",'" + tradetype + "','" + oprID + "','" + channel + "','" + status + "','" + clientID + "','" + tid + "','" + porderNo + "')";
            }
            else
            {
                sql += ") values  ('" + orderNo + "','" + tradeTime + "'," + amount + ",'" + tradetype + "','" + oprID + "','" + channel + "','" + status + "','" + clientID + "','" + tid + "')";
            }

            flag = DBHelper.ExecuteNonQuery(sql) > 0;


            return flag;
        }





        public bool UpdateTradeInfo(string orderNo, string serverflowno, string tradeStatus, string serverReponse, string buyID, string buyAccount, string errorCD)
        {
            bool flag = false;
            //if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string tradeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "update tradeinfo set ServerFlowNo='" + serverflowno + "',tradeStatus='" + tradeStatus + "',ServerResponse='" + serverReponse.Replace("'", "''") + "', ";
            sql += "updatetime='" + tradeTime + "',CustomerID='" + buyID + "',CustomerName='" + buyAccount
                + "',ErrorCode='" + errorCD + "'  where localorderno='" + orderNo + "'";

            flag = DBHelper.ExecuteNonQuery(sql) > 0;


            return flag;
        }

        public bool UpdateTradeStatus(string orderNo, string tradeStatus)
        {
            bool flag = false;
            // if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string tradeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "update tradeinfo set tradeStatus='" + tradeStatus + " where localorderno='" + orderNo + "'";

            flag = DBHelper.ExecuteNonQuery(sql) > 0;


            return flag;
        }

        public bool SameOrderID(string orderID)
        {
            bool flag = false;

            string sql = "select count(localorderno) from tradeinfo where localorderno='" + orderID + "'";

            object obj = DBHelper.ExecuteScalar(sql);

            if (obj != null)
            {
                flag = int.Parse(obj.ToString()) > 0;
            }

            return flag;
        }

        public int GetTradeCount(string startTime, string endTime, string status, string flowNo, string tradeType)
        {
            int count = 0;
            // if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string sql = "select count(localorderno) from tradeinfo where 1=1 ";
            if (!string.IsNullOrEmpty(tradeType))
            {
                sql += " and tradetype='" + tradeType + "' ";
            }

            if (!string.IsNullOrEmpty(status))
            {
                sql += " and tradestatus='" + status + "' ";

            }

            if (!string.IsNullOrEmpty(flowNo))
            {
                sql += " and localorderno='" + flowNo + "' ";
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                sql += " and tradetime>='" + startTime + "' ";
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                sql += " and tradetime='" + endTime + " 23:59:59' ";
            }

            object obj = DBHelper.ExecuteScalar(sql);
            count = int.Parse(obj.ToString());
            return count;
        }

        public DataSet GetTodayTradeInfo(string oprID, string clientID, string channel, bool isAdmin)
        {
            DataSet dt = null;
            //if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string strDate = DateTime.Now.ToString("yyyy-MM-dd");
            string sql = "select sum(amount) as 金额,tradetype as 交易类型,case tradestatus when 'OK' then '成功' when 'NoPay' then '未支付'  when 'OK_Refund' then '成功退款' when 'Fail' then '失败' else null end 交易状态,oprid as 操作员 from tradeinfo where 1=1";
            if (!isAdmin)
            {
                sql += " and OprID='" + oprID + "' ";
            }

            if (!string.IsNullOrEmpty(channel))
            {
                sql += " and channel='" + channel + "'";
            }

            if (!string.IsNullOrEmpty(clientID))
            {
                sql += " and clientID='" + clientID + "' ";
            }



            sql += " group by oprid,tradetype,tradestatus";

            dt = DBHelper.ExecuteDataSet(sql);

            return dt;
        }

        public DataSet  GetTradeInfo(string startTime, string endTime, string status, string flowNo, string tradeType, string channel, string clientID, string oprID, int start, int end, bool isAdmin)
        {
            DataSet dt = null;
            //if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string sql = @"select tradetime as 交易时间 ,amount as 金额, localorderno as 系统流水号,
ServerFlowNo as 渠道流水号,case tradestatus when 'OK' then '成功' when 'NoPay' then '未支付' when 'OK_Refund' then '成功退款' when 'Fail' then '失败' else null end 状态, tradetype as 交易类型
           ,ErrorCode as 失败原因,CustomerName as 支付账号,OprID as 操作员 from tradeinfo where 1=1 ";
            if (!string.IsNullOrEmpty(channel))
            {
                sql += " and channel='" + channel + "' ";
            }
            if (!string.IsNullOrEmpty(clientID))
            {
                sql += " and clientID='" + clientID + "' ";
            }

            if (!string.IsNullOrEmpty(tradeType))
            {
                sql += " and tradetype='" + tradeType + "' ";
            }

            if (!string.IsNullOrEmpty(status))
            {
                sql += " and tradestatus='" + status + "' ";

            }

            if (!string.IsNullOrEmpty(flowNo))
            {
                sql += " and localorderno='" + flowNo + "' ";
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                sql += " and tradetime>='" + startTime + "' ";
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                sql += " and tradetime<='" + endTime + " 23:59:59' ";
            }

            if (!isAdmin)
            {
                sql += " and OprID='" + oprID + "' ";
            }

            sql += string.Format(" order by tradetime desc limit {0} offset {1}", end - start + 1, start - 1);//size:每页显示条数，index页码
            dt = DBHelper.ExecuteDataSet(sql);
            return dt;
        }

        public DataSet GetAllTradeInfo(string startTime, string endTime, string status, string flowNo, string tradeType, string channel, string clientID, string oprID, bool isAdmin)
        {
            DataSet dt = null;
            // if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string sql = @"select tradetime as 交易时间 ,amount as 金额, localorderno as 系统流水号,
ServerFlowNo as 渠道流水号,case tradestatus when 'OK' then '成功' when 'NoPay' then '未支付'  when 'OK_Refund' then '成功退款'  when 'Fail' then '失败' else null end 状态,
tradetype as 交易类型,ErrorCode as 失败原因,CustomerName as 支付账号,OprID as 操作员 from tradeinfo where 1=1 ";
            if (!string.IsNullOrEmpty(channel))
            {
                sql += " and channel='" + channel + "' ";
            }

            if (!string.IsNullOrEmpty(clientID))
            {
                sql += " and clientID='" + clientID + "' ";
            }

            if (!string.IsNullOrEmpty(tradeType))
            {
                sql += " and tradetype='" + tradeType + "' ";
            }

            if (!string.IsNullOrEmpty(status))
            {
                sql += " and tradestatus='" + status + "' ";

            }

            if (!string.IsNullOrEmpty(flowNo))
            {
                sql += " and localorderno='" + flowNo + "' ";
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                sql += " and tradetime>='" + startTime + "' ";
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                sql += " and tradetime<='" + endTime + " 23:59:59' ";
            }

            if (!isAdmin)
            {
                sql += " and OprID='" + oprID + "' ";
                //if (oprID != "001")
                //{
                //    sql += " and OprID='" + oprID + "' ";
                //}
            }

            sql += string.Format(" order by tradetime desc ");
            dt = DBHelper.ExecuteDataSet(sql);
            return dt;
        }


        public int GetTradeCount(string startTime, string endTime, string status, string flowNo, string tradeType, string channel, string clientID, string oprID, bool isAdmin)
        {
            int count = 0;
            //if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string sql = "select count( localorderno) num from tradeinfo where 1=1 ";
            if (!string.IsNullOrEmpty(channel))
            {
                sql += " and channel='" + channel + "' ";
            }
            if (!string.IsNullOrEmpty(clientID))
            {
                sql += " and clientID='" + clientID + "' ";
            }

            if (!string.IsNullOrEmpty(tradeType))
            {
                sql += " and tradetype='" + tradeType + "' ";
            }

            if (!string.IsNullOrEmpty(status))
            {
                sql += " and tradestatus='" + status + "' ";

            }

            if (!string.IsNullOrEmpty(flowNo))
            {
                sql += " and localorderno='" + flowNo + "' ";
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                sql += " and tradetime>='" + startTime + "' ";
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                sql += " and tradetime<='" + endTime + " 23:59:59' ";
            }

            if (!isAdmin)
            {
                sql += " and OprID='" + oprID + "' ";
            }
            count = int.Parse(DBHelper.ExecuteScalar(sql).ToString());
            return count;
        }



        public DataSet GetTradeStatistic(string startTime, string endTime, string status, string flowNo, string tradeType, string channel, string clientID, string oprID, bool isAdmin)
        {
            DataSet dt = null;
            // if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            string sql = "select sum(case when tradetype='退款' then -1*Amount else Amount end ) as amount,count(LocalOrderNo) num  from tradeinfo where 1=1 ";
            if (!string.IsNullOrEmpty(channel))
            {
                sql += " and channel='" + channel + "' ";
            }

            if (!string.IsNullOrEmpty(clientID))
            {
                sql += " and clientID='" + clientID + "' ";
            }
            if (!string.IsNullOrEmpty(tradeType))
            {
                sql += " and tradetype='" + tradeType + "' ";
            }

            if (!string.IsNullOrEmpty(status))
            {
                sql += " and tradestatus='" + status + "' ";

            }

            if (!string.IsNullOrEmpty(flowNo))
            {
                sql += " and localorderno='" + flowNo + "' ";
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                sql += " and tradetime>='" + startTime + "' ";
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                sql += " and tradetime<='" + endTime + " 23:59:59' ";
            }

            if (!isAdmin)
            {
                sql += " and OprID='" + oprID + "' ";
            }
            dt = DBHelper.ExecuteDataSet(sql);
            return dt;
        }

        public DataTable GetUnSendTradeinfo()
        {
            DataTable dt = null;

            string sql = @"select localorderno, tradetime , amount,serverflowno,tradestatus,tradetype ,
porderno,createtime,updatetime,  oprid,serverresponse,paycode,channel,
customerid,customername,clientid ,TID,ErrorCode,OldLocalOrderNo,OldServerFlowNo  
from tradeinfo where sendflag is null";
            // if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
            dt = DBHelper.ExecuteDataTable(sql);

            return dt;
        }


        //public static bool UpdateUnSendTradeinfo()
        //{

        //    bool flag = false;
        //    string sql = @"update  tradeinfo set sendflag='1' where  sendflag is null";
        //    if (DBHelper == null) DBHelper = new SQLiteHelper(DBSource);
        //    flag = DBHelper.ExecuteNonQuery(sql) > 0;

        //    return flag;
        //}


    }
}
