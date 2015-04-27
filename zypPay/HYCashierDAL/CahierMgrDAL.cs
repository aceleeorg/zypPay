using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using HYModel;


namespace HYCashierDAL
{
   public class CahierMgrDAL
    {

      MySqlHelper DbHelper = null;
       public CahierMgrDAL()
       {
           string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
          
           DbHelper = new MySqlHelper(connectionString);
       }

       public CahierMgrDAL(string connectionString)
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
       public OprInfoEntiy CheckLoginUsr(string userID, string pwd,string clientID)
       {
           //int res = -1;

           string sql = "select * from oprinfo where OprId=?OprID and OprPWD=?OprPWD and ClientID=?ClientID";
           MySqlParameter para1 = new MySqlParameter();
           para1.Direction = ParameterDirection.Input;
           para1.Value = userID;
           para1.ParameterName = "?OprID";

           MySqlParameter para2 = new MySqlParameter();
           para2.Direction = ParameterDirection.Input;
           para2.Value = CryptographyHelper.MD5Encrypt(pwd);
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

          DataTable dt= DbHelper.ExecuteDataTable (sql, paras.ToArray());
          IList<OprInfoEntiy> oprs = dt.ConvertToList<OprInfoEntiy>();
          if (oprs != null && oprs.Count > 0) return oprs[0];
          else return null;


          // return res;
       }


       public bool IsClientOK(string clientID, string clientKey)
       {
           bool flag = false;
           if (string.IsNullOrEmpty(clientID) || string.IsNullOrEmpty(clientKey)) return false;
           string sql = @"SELECT count(1) FROM clientinfo where status<>1 ";

           if (!string.IsNullOrEmpty(clientID))
           {
               sql += " and clientid='" + clientID + "'";
           }


           if (!string.IsNullOrEmpty(clientKey))
           {
               sql += " and ActiveCode = '" + clientKey + "'";
           }

           object obj = DbHelper.ExecuteScalar(sql);

           if (obj != null) flag = int.Parse(obj.ToString())>0;
           return flag;
       }

       public int Bulk2ServerDB(DataTable dt, string tableName)
       {
           int count = 0;


           dt.TableName = tableName;
           count = DbHelper.BulkInsert(dt);


           return count;

       }

       public int  GetTradeInfoCount(WebTradeCondition queryCondition )
       {
           int count = 0;


           string sql = @"select  count(t1.localorderno) from  tradeinfo t1 left join 
clientinfo t2 on t1.clientID=t2.clientID where tradestatus='Ok' ";
           List<MySqlParameter> paras = new List<MySqlParameter>();
           if (!string.IsNullOrEmpty(queryCondition.ClientID))
           {
               sql += " and t1.ClientID=?ClientID";
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

           if (!string.IsNullOrEmpty(queryCondition.ClientName))
           {
               sql += " and t2.ClientName like ?ClientName";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?ClientName";
               para.Value = "%"+queryCondition.ClientName +"%";
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


           object obj=DbHelper.ExecuteScalar(sql, paras.ToArray());
           if (obj != null) count = int.Parse(obj.ToString());

           return count;
       }

       public DataTable GetTradeInfoPager(WebTradeCondition queryCondition, int pageSize,int pageIndex)
       {
           DataTable dt = null;


           string sql = @"select  tradetime, t1.localorderno,t1.clientID,t1.amount,t1.oprid,t1.tradestatus,
t1.tradetype, t2.clientName from  tradeinfo t1 left join clientinfo t2 on t1.clientid=t2.clientid 
 where tradestatus='Ok' ";
           List<MySqlParameter> paras = new List<MySqlParameter>();
           if (!string.IsNullOrEmpty(queryCondition.ClientID))
           {
               sql += " and t1.ClientID=?ClientID";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?ClientID";
               para.Value = queryCondition.ClientID;
               para.Direction = ParameterDirection.Input;
               paras.Add(para);

           }

           if (!string.IsNullOrEmpty(queryCondition.ClientName))
           {
               sql += " and t2.ClientName like ?ClientName";
               MySqlParameter para = new MySqlParameter();
               para.ParameterName = "?ClientName";
               para.Value ="%"+ queryCondition.ClientName+"%";
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
           int startIndex = pageSize * (pageIndex-1);
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

       public string GetAgentID()
       {
           string id = "";

           List<MySqlParameter> paras = new List<MySqlParameter>();
           MySqlParameter para = new MySqlParameter();
           para.ParameterName = "?newSerial";

           para.Direction = ParameterDirection.Output;

           paras.Add(para);

           DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "GetAgentID", paras.ToArray());

           id = DbHelper.DBCommond.Parameters["?newSerial"].Value.ToString();


           return id;
       }

       public string GetShopID()
       {
           string id = "";

           List<MySqlParameter> paras = new List<MySqlParameter>();
           MySqlParameter para = new MySqlParameter();
           para.ParameterName = "?newSerial";

           para.Direction = ParameterDirection.Output;

           paras.Add(para);

           DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "GetShopID", paras.ToArray());

           id = DbHelper.DBCommond.Parameters["?newSerial"].Value.ToString();


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

       public bool AddAgent(AgentInfoEntity agentInfo)
       {
           bool flag = false;

           string sql = @"insert into AgentInfo (AgentID ,AgentName,AgentType,UpAgentID,Address,Tel,
Contact,Status,Note,CreateTime,CreateUsr) values('";
           sql += agentInfo.AgentID + "','" + agentInfo.AgentName  + "'," + agentInfo.AgentType + ",'"
               + agentInfo.UpAgentID + "','"+agentInfo .Address +"','"+agentInfo .Tel+"','" 
               +agentInfo .Contact +"',"+ agentInfo.Status +",'"+agentInfo.Note + "',now(),'" + agentInfo.CreateUsr + "')";

           flag = DbHelper.ExecuteNonQuery(sql) > 0;


           return flag;
       }

       public bool AddOpr(OprInfoEntiy oprinfo)
       {
           bool flag = false;

           string sql = "insert into oprinfo (oprid,oprname,oprpwd,clientid,createtime,status,isadmin) values('";
           sql += oprinfo.OprID + "','" + oprinfo.OprName + "','" + oprinfo.OprPWD + "','"
               + oprinfo.ClientID + "',now()," + oprinfo.Status +","+oprinfo.IsAdmin+ ")";

           flag = DbHelper.ExecuteNonQuery(sql)>0;


           return flag;
       }

       public bool AddShop(ShopInfoEntity shopInfo)
       {
           bool flag = false;

           string sql = "insert into ShopInfo (ShopID,ShopName,ClientID,Address,Tel,TNum,CreateTime,Status,CreateUsr) values('";
           sql += shopInfo.ShopID + "','" + shopInfo.ShopName + "','" + shopInfo.ClientID + "','"
               + shopInfo.Address +"','"+shopInfo .Tel +"',"+shopInfo .TNum + ",now()," + shopInfo.Status + "," + shopInfo.CreateUsr + ")";

           flag = DbHelper.ExecuteNonQuery(sql) > 0;


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

           string sql = "select * from clientinfo where  status<>1 ";
           if (!string.IsNullOrEmpty(clientID))
           {
               sql += "  and clientID='"+clientID +"'" ;
           }

           if (!string.IsNullOrEmpty(clientName))
           {
               sql += "  and clientName like '%" + clientName + "%'";
           }

           DataTable dt = DbHelper.ExecuteDataTable(sql);

           if (dt != null && dt.Rows.Count > 0)
           {
               clients = dt.ConvertToList<ClientInfoEntiy >();
           }

           return clients;
       }

       public IList<OprInfoEntiy > GetOprInfo(string oprId, string oprName)
       {
           IList<OprInfoEntiy> oprs = null;

           string sql = "select * from oprinfo where  status<>1 ";
           if (!string.IsNullOrEmpty(oprId))
           {
               sql += "  and oprId='" + oprId + "'";
           }

           if (!string.IsNullOrEmpty(oprName))
           {
               sql += "  and oprName like '%" + oprName + "%'";
           }

           DataTable dt = DbHelper.ExecuteDataTable(sql);

           if (dt != null && dt.Rows.Count > 0)
           {
               oprs = dt.ConvertToList<OprInfoEntiy>();
           }

           return oprs;
       }

       public bool DeleteClient(string clientID)
       {
           bool flag = false;

           string sql = "update clientinfo set status=1 where clientID='"+clientID +"'";
           flag = DbHelper.ExecuteNonQuery(sql) > 0;
           return flag;
       }

       public bool DeleteOpr(string oprID)
       {
           bool flag = false;

           string sql = "update oprinfo set status=1 where OprID='" + oprID + "'";
           flag = DbHelper.ExecuteNonQuery(sql) > 0;
           return flag;
       }

       public bool AddClient(ClientInfoEntiy clientInfo)
       {
           bool flag = false;

           string sql = "insert into clientinfo (clientid,clientName,phone,contact,address,maxnum,activecode,createtime,status,pid,pkey,AlipayAccount) values('";
           sql += clientInfo.ClientID + "','" + clientInfo.ClientName + "','" + clientInfo.Phone  + "','"
               + clientInfo.Contact + "','" + clientInfo.Address + "'," + clientInfo.MaxNum + ",'" + clientInfo.ActiveCode + "',now()," + clientInfo.Status + ",'"+clientInfo .PID 
               +"','"+clientInfo .PKey+"','"+clientInfo .AlipayAccount +"')";

           flag = DbHelper.ExecuteNonQuery(sql) > 0;


           return flag;
       }
       public bool UpdateAgent(AgentInfoEntity agentInfo)
       {
           bool flag = false;

           string sql = "update AgentInfo set AgentName='" + agentInfo.AgentName + "',AgentType="+agentInfo .AgentType
               +",UpAgentID='"+agentInfo .UpAgentID +"',Address='"+agentInfo .Address +"',Tel='"+agentInfo .Tel 
               +"',Contact='"+agentInfo .Contact +"',Note='"+agentInfo .Note 
               +"',UpdateTime=now(),UpdateUsr='"+agentInfo .UpdateUsr  
               + "'  where AgentID='" + agentInfo.AgentID + "'";

           flag = DbHelper.ExecuteNonQuery(sql) > 0;


           return flag;
       }

       public bool UpdateClient(ClientInfoEntiy clientInfo)
       {
           bool flag = false;

           string sql = "update clientinfo set clientName='"+clientInfo .ClientName +"',phone='"+clientInfo .Phone 
               +@"',contact='"+clientInfo.Contact+"',AlipayAccount='"+clientInfo .AlipayAccount +"',address='"+clientInfo .Address +"',maxnum="+
           clientInfo .MaxNum +",ActiveCode='"+clientInfo.ActiveCode +"' ,updatetime=now(),status="+clientInfo .Status +
           ",pid='"+clientInfo .PID +"',pkey='"+clientInfo .PKey +"'  where clientid='" + clientInfo.ClientID +"'";

           flag = DbHelper.ExecuteNonQuery(sql) > 0;


           return flag;
       }

       public bool UpdateOpr(OprInfoEntiy oprinfo)
       {
           bool flag = false;

           string sql = "update oprinfo set oprname='"+oprinfo .OprName +"',oprpwd='"+oprinfo .OprPWD +"',clientid='"+
           oprinfo.ClientID +"',status=" + oprinfo.Status + ",IsAdmin="+oprinfo .IsAdmin +",updatetime=now() where oprid='"+oprinfo .OprID +"'";

           flag = DbHelper.ExecuteNonQuery(sql) > 0;
           return flag;
       }
       //ShopName,ClientID,Address,Tel,TNum,CreateTime,Status,CreateUsr
       public bool UpdateShop(ShopInfoEntity shopInfo)
       {
           bool flag = false;

           string sql = "update ShopInfo set ShopName='"+shopInfo .ShopName +"',ClientID='"+shopInfo.ClientID 
               +"',Address='"+shopInfo .Address +"',Tel='"+shopInfo .Tel +"',TNum="+
           shopInfo.TNum + ",UpdateUsr='" + shopInfo.UpdateUsr + ",updatetime=now() where ShopID='" + shopInfo.ShopID + "'";

           flag = DbHelper.ExecuteNonQuery(sql) > 0;
           return flag;
       }

       public bool UpdateOprPwd(string oprID,string pwd)
       {
           bool flag = false;

           string sql = "update oprinfo set oprpwd='" + pwd + "',updatetime=now() where oprid='" + oprID  + "'";

           flag = DbHelper.ExecuteNonQuery(sql) > 0;
           return flag;
       }

       public int GetClientCount(string clientID, string clientName )
       {
           int count = 0;
           string sql = @"SELECT count(1) FROM clientinfo where status<>1 ";
           if (!string.IsNullOrEmpty(clientID))
           {
               sql += " and clientid='" + clientID + "'";
           }


           if (!string.IsNullOrEmpty(clientName))
           {
               sql += " and clientName like '%" + clientName + "%'";
           }

           object obj = DbHelper.ExecuteScalar(sql);

           if(obj !=null ) count=int.Parse (obj .ToString ());
           return count;
       }

       public int GetShopCount(string clientID, string clientName, string shopId, string shopName)
       {
           int count = 0;

//           string sql = @"SELECT count(t1.oprid) FROM oprinfo t1 
//left join clientinfo t2
//on t1.ClientID=t2.ClientID where  t1.status<>1 ";
//           if (!string.IsNullOrEmpty(clientID))
//           {
//               sql += " and t1.clientid='" + clientID + "'";
//           }

//           if (!string.IsNullOrEmpty(oprId))
//           {
//               sql += " and t1.oprId='" + oprId + "'";
//           }

//           if (!string.IsNullOrEmpty(clientName))
//           {
//               sql += " and t2.clientName like '%" + clientName + "%'";
//           }

//           if (!string.IsNullOrEmpty(oprName))
//           {
//               sql += " and oprName like '%" + oprName + "%'";
//           }

//           object obj = DbHelper.ExecuteScalar(sql);

//           if (obj != null) count = int.Parse(obj.ToString());
           return count;
       }

       public int GetOprCount(string clientID, string clientName, string oprId, string oprName)
       {
           int count = 0;

           string sql = @"SELECT count(t1.oprid) FROM oprinfo t1 
left join clientinfo t2
on t1.ClientID=t2.ClientID where  t1.status<>1 ";
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
               sql += " and t2.clientName like '%" + clientName + "%'";
           }

           if (!string.IsNullOrEmpty(oprName))
           {
               sql += " and oprName like '%" + oprName + "%'";
           }

           object obj = DbHelper.ExecuteScalar(sql);

           if (obj != null) count = int.Parse(obj.ToString());
           return count;
       }


       public DataTable GetClientList(string clientID, string clientName,int pageIndex,int pageSize )
       {
           DataTable dt = null;

           string sql = @"SELECT * FROM clientinfo where  status<>1 ";
           if (!string.IsNullOrEmpty(clientID))
           {
               sql += " and clientid='" + clientID + "'";
           }

        
           if (!string.IsNullOrEmpty(clientName))
           {
               sql += " and clientName like '%" + clientName + "%'";
           }

           int startIndex = pageSize * (pageIndex-1);
           sql += " limit " + startIndex.ToString() + " ," + pageSize.ToString(); 

         

           dt = DbHelper.ExecuteDataTable(sql);
           return dt;
       }

       public DataTable GetOprList(string clientID, string clientName, string oprId, string oprName,int pageIndex,int pageSize)
       {
           DataTable dt = null;

           string sql = @"SELECT t1.*,t2.ClientName FROM oprinfo t1 
left join clientinfo t2
on t1.ClientID=t2.ClientID where  t1.status<>1 ";
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
               sql += " and t2.clientName like '%" + clientName + "%'";
           }

           if (!string.IsNullOrEmpty(oprName))
           {
               sql += " and oprName like '%" + oprName + "%'";
           }

           int startIndex = pageSize * (pageIndex-1);
           sql += " limit " + startIndex.ToString() + " ," + pageSize.ToString(); ;


           dt = DbHelper.ExecuteDataTable(sql);
           return dt;
       }

       public DataTable GetAllClient()
       {
           DataTable dt = null;

           string sql = "select * from clientinfo where status<>1";
           dt = DbHelper.ExecuteDataTable(sql);
           return dt;
       }

       public DataTable GetClient(string clientID)
       {
           DataTable dt = null;

           string sql = "select * from clientinfo where status<>1";
           if (!string.IsNullOrEmpty(clientID))
           {
               sql = "select * from clientinfo where status<>1 and clientid='"+clientID +"'";
           }
           dt = DbHelper.ExecuteDataTable(sql);
           return dt;
       }

       public int GetAgentCount(string agentID,string agentName, string upAgentID, int agentType)
       {

           int count = 0;
           string sql = "select count(*) from AgentInfo where status<>1";
           if (!string.IsNullOrEmpty(agentID))
           {
               sql = "select * from AgentInfo where status<>1 and AgentID='" + agentID + "'";
           }

           if (!string.IsNullOrEmpty(agentName ))
           {
               sql += " and AgentName like '%" + upAgentID + "%'";
           }

           if (!string.IsNullOrEmpty(upAgentID))
           {
               sql += " and UpAgentID='" + upAgentID + "'";
           }

           if (agentType > 0)
           {
               sql += " and AgentType=" + agentType;
           }


           object obj = DbHelper.ExecuteScalar(sql);
           if (obj != null) count = int.Parse(obj.ToString());
          
           return count ;
       }

       public DataTable GetAgentList(string agentID,string agentName,string upAgentID,int agentType,int pageIndex,int pageSize)
       {
           DataTable dt = null;

           string sql = "select * from AgentInfo where status<>1";
           if (!string.IsNullOrEmpty(agentID))
           {
               sql = "select * from AgentInfo where status<>1 and AgentID='" + agentID + "'";
           }
           if (!string.IsNullOrEmpty(agentName))
           {
               sql += " and AgentName like '%" + upAgentID + "%'";
           }
           if (!string.IsNullOrEmpty(upAgentID))
           {
               sql += " and UpAgentID='"+upAgentID +"'";
           }

           if (agentType > 0)
           {
               sql += " and AgentType="+agentType ;
           }

           int startIndex = pageSize * (pageIndex - 1);
           sql += " limit " + startIndex.ToString() + " ," + pageSize.ToString(); 

           dt = DbHelper.ExecuteDataTable(sql);
           return dt;
       }

       public DataTable GetShopList(string shopID, int pageIndex, int pageSize)
       {
           DataTable dt = null;

           string sql = "select * from ShopInfo where status<>1";
           if (!string.IsNullOrEmpty(shopID))
           {
               sql = "select * from ShopInfo where status<>1 and AgentID='" + shopID + "'";
           }

           int startIndex = pageSize * (pageIndex - 1);
           sql += " limit " + startIndex.ToString() + " ," + pageSize.ToString(); 
           dt = DbHelper.ExecuteDataTable(sql);
           return dt;
       }


       public DataTable GetAllOpr(string clientID)
       {
           DataTable dt = null;
           
           string querySql = "select * from oprinfo where status=0 and clientid='"+clientID +"'";
           if (string.IsNullOrEmpty(clientID))
           {
               querySql = "select * from oprinfo where status=0";
           }
           dt = DbHelper.ExecuteDataTable(querySql);

           return dt;
       }

       public ActiveRes Active(string activeCode,string machineCode)
       {
          // bool flag = false;
           ActiveRes res = new ActiveRes();
           res.Success = false;
           string querySql = "select * from clientinfo where activecode=?ACODE";
           string clientID = "";
           List<MySqlParameter> paras = new List<MySqlParameter>();
           MySqlParameter para = new MySqlParameter();
           para.ParameterName = "?ACODE";
           para.Value = activeCode;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.String ;
           paras.Add(para);
           DataTable dt1 = DbHelper.ExecuteDataTable(querySql,paras .ToArray ());
           if (dt1 != null && dt1.Rows.Count > 0)
           {
               clientID = dt1.Rows[0]["clientID"].ToString();
               string id = "";
               //检查是否已激活过
               querySql = "select * from activeinfo where machineCode='" + machineCode + "' and ClientID='"+clientID +"'";
               DataTable dt2 = DbHelper.ExecuteDataTable(querySql);
               if (dt2 != null && dt2.Rows.Count > 0)
               {
                   id = dt2.Rows[0]["TerminalID"].ToString();
                   res.Success = true;
                   res.ClientName = dt1.Rows[0]["ClientName"].ToString();
                   res.TerminalID = id;
                   res.ClientID = clientID;
                   res.PID = dt1.Rows[0]["PID"].ToString();
                   res.PKEY = dt1.Rows[0]["PKEY"].ToString();
                   res.AlipayAccount = dt1.Rows[0]["AlipayAccount"].ToString();
               }
               else
               {
                   //获取已激活数量
                   int count=0;
                   querySql = "select count(*) from activeinfo where clientID='" + clientID + "'";
                   object obj = DbHelper.ExecuteScalar(querySql);
                   if (obj != null) count = int.Parse(obj.ToString())+1;
                   id=clientID+count.ToString ().PadLeft (2,'0') ;
                   //插入机器信息
                   string insertSql = "insert into activeinfo(clientID,machineCode,TerminalID,status) values('";
                   insertSql +=clientID + "','"+machineCode +"','"+id;
                   insertSql += "',0)";

                   if (DbHelper.ExecuteNonQuery(insertSql) > 0)
                   {
                       res.Success = true ;
                       res.ClientName = dt1.Rows[0]["ClientName"].ToString();
                       res.TerminalID = id;
                       res.ClientID = clientID;
                       res.PID = dt1.Rows[0]["PID"].ToString();
                       res.PKEY = dt1.Rows[0]["PKEY"].ToString();
                       res.AlipayAccount = dt1.Rows[0]["AlipayAccount"].ToString();

                   }
               }

              

           }
           else
           {
               res.Success = false;
           }

           return res;
          // MySqlTransaction trans = (MySqlTransaction)DbHelper.DBConnection.BeginTransaction();  //创建事务
// cmd.Transaction = trans;  //绑定事务
//foreach(.........)
//{
//    string sql = ".....";
//    cmd = new MySQLCommand(sqlInsert, mysqlConn);
//    cmd.ExecuteNonQuery();
//}
//trans.Commit();

           //return flag;
       }


       public bool AddServiceLog(string flowNo, string requestParas, string ClientID, string ip, DateTime requestTime,string oprID)
       {
           bool flag = false;

           string sql = "insert into ServiceLog(FlowNo,RequestParas,ClientID,IP,RequestTime,OprID) values(";
           sql += "?FlowNo,?RequestParas,?ClientID,?IP,?RequestTime,?OprID)";
           List<MySqlParameter> paras = new List<MySqlParameter>();
           MySqlParameter para = new MySqlParameter();
           para.ParameterName = "?FlowNo";
           para.Value = flowNo ;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.String ;
           paras.Add(para);

           para = new MySqlParameter();
           para.ParameterName = "?RequestParas";
           para.Value = requestParas;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.String;
           paras.Add(para);

           para = new MySqlParameter();
           para.ParameterName = "?ClientID";
           para.Value = ClientID ;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.String;
           paras.Add(para);


           para = new MySqlParameter();
           para.ParameterName = "?RequestTime";
           para.Value = requestTime;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.DateTime;
           paras.Add(para);


           para = new MySqlParameter();
           para.ParameterName = "?OprID";
           para.Value = oprID ;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.String;
           paras.Add(para);

           para = new MySqlParameter();
           para.ParameterName = "?IP";
           para.Value = ip;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.String;
           paras.Add(para);

         

          
           //sql += flowNo + "','" + requestParas + "','" + ClientID + "','" + ip + "'," + requestTime + ",'"+oprID +"')";
           flag = DbHelper.ExecuteNonQuery(sql,paras .ToArray ()) > 0;
           return flag;
       }

       public bool UpdateServiceLog(string flowNo, DateTime channelRequestTime, DateTime channelReponseTime, DateTime responseTime, string channelResponse, string responseParas)
       {
           bool flag = false;
           //string sql = "update ServiceLog set channelResponse='" + channelResponse + "',responseParas='" + responseParas+"',";
           //sql += "channelRequestTime=" + channelRequestTime + ",channelReponseTime=" + channelReponseTime+",";
           //sql += "responseTime=" + responseTime;

           string sql = "update ServiceLog set ChannelResponse=?ChannelResponse,ResponseParas=?responseParas,";
           sql += "ChannelRequestTime=?ChannelRequestTime,ChannelResponseTime=?ChannelResponseTime,";
           sql += "ResponseTime=?ResponseTime where FlowNo=?FlowNo";

           List<MySqlParameter> paras = new List<MySqlParameter>();
           MySqlParameter para = new MySqlParameter();
           para.ParameterName = "?FlowNo";
           para.Value = flowNo;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.String;
           paras.Add(para);

           para = new MySqlParameter();
           para.ParameterName = "?ChannelResponse";
           para.Value = channelResponse;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.String;
           paras.Add(para);

           para = new MySqlParameter();
           para.ParameterName = "?ResponseParas";
           para.Value = responseParas;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.String;
           paras.Add(para);


           para = new MySqlParameter();
           para.ParameterName = "?ChannelRequestTime";
           para.Value = channelRequestTime;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.DateTime;
           paras.Add(para);

           para = new MySqlParameter();
           para.ParameterName = "?ChannelResponseTime";
           para.Value = channelReponseTime ;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.DateTime;
           paras.Add(para);


           para = new MySqlParameter();
           para.ParameterName = "?ResponseTime";
           para.Value = responseTime;
           para.Direction = ParameterDirection.Input;
           para.DbType = DbType.DateTime;
           paras.Add(para);


           flag = DbHelper.ExecuteNonQuery(sql,paras.ToArray()) > 0;
           return flag;
       }
       
    }
}
