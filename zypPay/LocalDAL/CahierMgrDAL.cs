using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using HYModel;


namespace HYCashierDAL
{
   public class CahierMgrDAL_L
    {

      MySqlHelper DbHelper = null;
       public CahierMgrDAL_L()
       {
           string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
           //connectionString =HYUtility. CryptographyHelper.MD5Encrypt(connectionString, "HYDB2015");
           connectionString = HYUtility.CryptographyHelper.MD5Decrypt(connectionString, "HYDB2015");
           DbHelper = new MySqlHelper(connectionString);
       }

       public CahierMgrDAL_L(string connectionString)
       {
          DbHelper = new MySqlHelper(connectionString);
       }

       /// <summary>
       /// 检查操作员编号密码商户号是否正确
       /// </summary>
       /// <param name="userID">操作员编号</param>
       /// <param name="pwd">密码</param>
       /// <param name="clientID">商户号</param>
       /// <returns></returns>
       public int CheckLoginUsr(string userID, string pwd,string clientID)
       {
           int res = -1;

           string sql = "select count(*) from OprInfo where OprId=?OprID and OprPWD=?OprPWD and ClientID=?ClientID";
           MySqlParameter para1 = new MySqlParameter();
           para1.Direction = ParameterDirection.Input;
           para1.Value = userID;
           para1.ParameterName = "?OprID";

           MySqlParameter para2 = new MySqlParameter();
           para2.Direction = ParameterDirection.Input;
           para2.Value = HYUtility.CryptographyHelper.MD5Encrypt(pwd);
           para2.ParameterName = "?OprPWD";

           MySqlParameter para3 = new MySqlParameter();
           para3.Direction = ParameterDirection.Input;
           para3.Value = clientID;
           para3.ParameterName = "?ClientID";
          //MySqlHelper .cr
           List <MySqlParameter > paras=new List<MySqlParameter> ();
           paras .Add (para1 );
           paras .Add (para2 );
           paras.Add(para3);

           object obj = DbHelper.ExecuteScalar(sql, paras.ToArray());
           res=int.Parse (obj.ToString ());


           return res;
       }


       public int Bulk2ServerDB(DataTable dt, string tableName)
       {
           int count = 0;

           try
           {
               dt.TableName = tableName;
               count = DbHelper.BulkInsert(dt);
           }
           catch
           { 
           
           }


           return count;

       }

       public int  GetTradeInfoCount(WebTradeCondition queryCondition )
       {
           int count = 0;


           string sql = "select  count(*) from  tradeinfo where 1=1 ";
           List<MySqlParameter> paras = new List<MySqlParameter>();
           if (!string.IsNullOrEmpty(queryCondition.ClientID))
           {
               sql += " and ClientID=?ClientID";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?ClientID";
               para.Value = queryCondition.ClientID;
               para.Direction = ParameterDirection.Input;
               paras.Add(para);

           }

           if (!string.IsNullOrEmpty(queryCondition.OprID ))
           {
               sql += " and OprID=?OprID";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?OprID";
               para.Value = queryCondition.OprID ;
               para.Direction = ParameterDirection.Input;
               paras.Add(para);

           }


           if (!string.IsNullOrEmpty(queryCondition.LocalFlowNo ))
           {
               sql += " and LocalOrderNo=?LocalOrderNo";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?LocalOrderNo";
               para.Value = queryCondition.LocalFlowNo ;
               para.Direction = ParameterDirection.Input;
               paras.Add(para);

           }


           if (!string.IsNullOrEmpty(queryCondition.ServerFlowNo ))
           {
               sql += " and ServerFlowNo=?ServerFlowNo";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?ServerFlowNo";
               para.Value = queryCondition.ServerFlowNo;
               para.Direction = ParameterDirection.Input;
               paras.Add(para);

           }

           if (queryCondition.StartDate != null)
           {
               sql += " and TradeTime>=?StartDate";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?StartDate";
               para.Value = queryCondition.StartDate;
               para.Direction = ParameterDirection.Input;
               para.DbType = DbType.DateTime;
               paras.Add(para);
           }

           if (queryCondition.EndDate  != null)
           {
               sql += " and TradeTime<?EndDate";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?EndDate";
               para.Value = queryCondition.EndDate;
               para.Direction = ParameterDirection.Input;
               para.DbType = DbType.DateTime;
               paras.Add(para);
           }

           count = (int)DbHelper.ExecuteScalar(sql, paras.ToArray());

           return count;
       }

       public DataTable GetTradeInfoPager(WebTradeCondition queryCondition, int pageSize,int pageIndex)
       {
           DataTable dt = null;


           string sql = "select  * from  tradeinfo where 1=1 ";
           List<MySqlParameter> paras = new List<MySqlParameter>();
           if (!string.IsNullOrEmpty(queryCondition.ClientID))
           {
               sql += " and ClientID=?ClientID";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?ClientID";
               para.Value = queryCondition.ClientID;
               para.Direction = ParameterDirection.Input;
               paras.Add(para);

           }

           if (!string.IsNullOrEmpty(queryCondition.OprID))
           {
               sql += " and OprID=?OprID";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?OprID";
               para.Value = queryCondition.OprID;
               para.Direction = ParameterDirection.Input;
               paras.Add(para);

           }


           if (!string.IsNullOrEmpty(queryCondition.LocalFlowNo))
           {
               sql += " and LocalOrderNo=?LocalOrderNo";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?LocalOrderNo";
               para.Value = queryCondition.LocalFlowNo;
               para.Direction = ParameterDirection.Input;
               paras.Add(para);

           }


           if (!string.IsNullOrEmpty(queryCondition.ServerFlowNo))
           {
               sql += " and ServerFlowNo=?ServerFlowNo";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?ServerFlowNo";
               para.Value = queryCondition.ServerFlowNo;
               para.Direction = ParameterDirection.Input;
               paras.Add(para);

           }

           if (queryCondition.StartDate != null)
           {
               sql += " and TradeTime>=?StartDate";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?StartDate";
               para.Value = queryCondition.StartDate;
               para.Direction = ParameterDirection.Input;
               para.DbType = DbType.DateTime;
               paras.Add(para);
           }

           if (queryCondition.EndDate != null)
           {
               sql += " and TradeTime<?EndDate";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?EndDate";
               para.Value = queryCondition.EndDate;
               para.Direction = ParameterDirection.Input;
               para.DbType = DbType.DateTime;
               paras.Add(para);
           }
           int startIndex = pageSize * pageIndex;
           sql += " limit " + startIndex.ToString() + " ," + pageSize.ToString(); ;


           dt = DbHelper.ExecuteDataTable(sql, paras.ToArray());
           return dt;
       }

       public string GetClientID()
       {
           string id = "";
         
           List<MySqlParameter> paras = new List<MySqlParameter>();
           MySqlParameter para = new MySqlParameter();
           para.ParameterName = "?newSerial";
         
           para.Direction = ParameterDirection.Output ;
          
           paras.Add(para);

           DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "GetClientID", paras.ToArray());

           id =DbHelper.DBCommond .Parameters["?newSerial"].Value.ToString();

           
           return id;
       }



       public string GetOprID()
       {
           string id = "";
           List<MySqlParameter> paras = new List<MySqlParameter>();
           MySqlParameter para = new MySqlParameter();
           para.ParameterName = "?newSerial";

           para.Direction = ParameterDirection.Output;

           paras.Add(para);

           DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "GetOprID", paras.ToArray());

           id = DbHelper.DBCommond.Parameters["?newSerial"].Value.ToString();
           return id;
       }

       public bool AddOpr(OprInfoEntiy oprinfo)
       {
           bool flag = false;

           string sql = "insert into oprInfo (oprid,oprname,oprpwd,clientid,createtime,status) values('";
           sql += oprinfo.OprID + "','" + oprinfo.OprName + "','" + oprinfo.OprPWD + "','"
               + oprinfo.ClientID + "',now()," + oprinfo.Status + ")";

           flag = DbHelper.ExecuteNonQuery(sql)>0;


           return flag;
       }

       public bool IsClientExist(string clientName)
       {
           bool flag = false;

           string sql = "select count(*) from clientinfo where clientName='"+clientName +"'";
           object obj = DbHelper.ExecuteScalar(sql);
           if (obj != null) flag = (int.Parse(obj.ToString())) > 0;
           return flag;
       }

       public IList<ClientInfoEntiy> GetClientInfo(string clientID, string clientName)
       {
           IList<ClientInfoEntiy> clients = null;

           //string sql = "select * from clientinfo where 1=1 ";
           //if (!string.IsNullOrEmpty(clientID))
           //{
           //    sql += "  and clientID='"+clientID +"'" ;
           //}

           //if (!string.IsNullOrEmpty(clientName))
           //{
           //    sql += "  and clientName like %" + clientName + "%";
           //}

           //DataTable dt = DbHelper.ExecuteDataTable(sql);

           //if (dt != null && dt.Rows.Count > 0)
           //{
           //    clients = dt.ConvertToList<ClientInfoEntiy >();
           //}

           return clients;
       }

       public bool AddClient(ClientInfoEntiy clientInfo)
       {
           bool flag = false;

           string sql = "insert into clientInfo (clientid,clientName,phone,contact,address,maxnum,activecode,createtime,status) values('";
           sql += clientInfo.ClientID + "','" + clientInfo.ClientName + "','" + clientInfo.Phone  + "','"
               + clientInfo.Contact + "','" + clientInfo.Address + "'," + clientInfo.MaxNum + ",'" + clientInfo.ActiveCode + "',now()," + clientInfo.Status + ")";

           flag = DbHelper.ExecuteNonQuery(sql) > 0;


           return flag;
       }

       public bool UpdateClient(ClientInfoEntiy clientInfo)
       {
           bool flag = false;

           string sql = "update clientInfo set clientName='"+clientInfo .ClientName +"',phone='"+clientInfo .Phone 
               +@"',contact='"+clientInfo.Contact+"',address='"+clientInfo .Address +"',maxnum="+
           clientInfo .MaxNum +",activecode='"+clientInfo.ActiveCode +"' ,updatetime=now(),status="+clientInfo .Status +
           " where clientid='" + clientInfo.ClientID +"'";

           flag = DbHelper.ExecuteNonQuery(sql) > 0;


           return flag;
       }

       public bool UpdateOpr(OprInfoEntiy oprinfo)
       {
           bool flag = false;

           string sql = "update oprInfo set oprname='"+oprinfo .OprName +"',oprpwd='"+oprinfo .OprPWD +"',clientid='"+
           oprinfo.ClientID +"',status=" + oprinfo.Status + ",updatetime=now() where oprid='"+oprinfo .OprID +"'";

           flag = DbHelper.ExecuteNonQuery(sql) > 0;
           return flag;
       }

       public int GetClientCount(string clientID, string clientName )
       {
           int count = 0;
           string sql = @"SELECT count(1) FROM clientinfo where 1=1 ";
           if (!string.IsNullOrEmpty(clientID))
           {
               sql += " and t1.clientid='" + clientID + "'";
           }


           if (!string.IsNullOrEmpty(clientName))
           {
               sql += " and t2.clientName like %" + clientName + "%";
           }

           object obj = DbHelper.ExecuteScalar(sql);

           if(obj !=null ) count=int.Parse (obj .ToString ());
           return count;
       }

       public int GetOprCount(string clientID, string clientName, string oprId, string oprName)
       {
           int count = 0;

           string sql = @"SELECT count(t1.oprid) FROM oprinfo t1 
left join clientinfo t2
on t1.ClientID=t2.ClientID where 1=1 ";
           if (!string.IsNullOrEmpty(clientID))
           {
               sql += " and t1.clientid='" + clientID + "'";
           }

           if (!string.IsNullOrEmpty(oprId))
           {
               sql += " and t1.oprId='" + oprId + "'";
           }

           if (!string.IsNullOrEmpty(clientName))
           {
               sql += " and t2.clientName like %" + clientName + "%";
           }

           if (!string.IsNullOrEmpty(oprName))
           {
               sql += " and oprName like %" + oprName + "%";
           }

           object obj = DbHelper.ExecuteScalar(sql);

           if (obj != null) count = int.Parse(obj.ToString());
           return count;
       }


       public DataTable GetClientList(string clientID, string clientName,int pageIndex,int pageSize )
       {
           DataTable dt = null;

           string sql = @"SELECT * FROM clientinfo where 1=1 ";
           if (!string.IsNullOrEmpty(clientID))
           {
               sql += " and t1.clientid='" + clientID + "'";
           }

        
           if (!string.IsNullOrEmpty(clientName))
           {
               sql += " and t2.clientName like %" + clientName + "%";
           }

           int startIndex = pageSize * (pageIndex-1);
           sql += " limit " + startIndex.ToString() + " ," + pageSize.ToString(); ;

         

           dt = DbHelper.ExecuteDataTable(sql);
           return dt;
       }

       public DataTable GetOprList(string clientID, string clientName, string oprId, string oprName,int pageIndex,int pageSize)
       {
           DataTable dt = null;

           string sql = @"SELECT t1.*,t2.ClientName FROM oprinfo t1 
left join clientinfo t2
on t1.ClientID=t2.ClientID where 1=1 ";
           if (!string.IsNullOrEmpty(clientID))
           {
               sql += " and t1.clientid='"+clientID +"'";
           }

           if (!string.IsNullOrEmpty(oprId))
           {
               sql += " and t1.oprId='" + oprId + "'";
           }

           if (!string.IsNullOrEmpty(clientName))
           {
               sql += " and t2.clientName like %" + clientName + "%";
           }

           if (!string.IsNullOrEmpty(oprName))
           {
               sql += " and oprName like %" + oprName + "%";
           }

           int startIndex = pageSize * (pageIndex-1);
           sql += " limit " + startIndex.ToString() + " ," + pageSize.ToString(); ;


           dt = DbHelper.ExecuteDataTable(sql);
           return dt;
       }


       public bool Active(string activeCode,string clientID, string machineCode)
       {
           bool flag = false;

           string querySql = "select * from clientinfo where activecode=?ACODE";
           List<MySqlParameter> paras = new List<MySqlParameter>();
           MySqlParameter para = new MySqlParameter();
           para.ParameterName = "?ACODE";
           para.Value = activeCode;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.String ;
           paras.Add(para);
          // DbHelper.be
          // MySqlTransaction trans = (MySqlTransaction)DbHelper.DBConnection.BeginTransaction();  //创建事务
// cmd.Transaction = trans;  //绑定事务
//foreach(.........)
//{
//    string sql = ".....";
//    cmd = new MySQLCommand(sqlInsert, mysqlConn);
//    cmd.ExecuteNonQuery();
//}
//trans.Commit();

           return flag;
       }
       
    }
}
