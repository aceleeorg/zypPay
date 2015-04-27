using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using HYModel;
using Newtonsoft.Json;

namespace CashierMgr
{
    /// <summary>
    /// BaseHandler 的摘要说明
    /// </summary>
    public class BaseHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string res = "";
            string type = "";
            
            if (context.Request.Params["type"] != null)
            {
                type = context.Request.Params["type"];
            }

            if (type == "client")
            {

                string cid = "";
                if (context.Request.Params["cid"] != null)
                {
                    cid = context.Request.Params["cid"];
                }
                HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                DataTable dt = dal.GetClient(cid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    IList<ClientInfoEntiy> clients = DataTableUtility.ToList<ClientInfoEntiy>(dt);
                    res = JsonConvert.SerializeObject(clients);
                }
            }
            else if (type == "shop")
            {
                HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                DataTable dt = dal.GetAllOpr("");
                if (dt != null && dt.Rows.Count > 0)
                {
                    IList<ClientInfoEntiy> clients = DataTableUtility.ToList<ClientInfoEntiy>(dt);
                    res = JsonConvert.SerializeObject(clients);
                }
            }
            else if (type == "agent")
            {
                HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                DataTable dt = dal.GetAllOpr("");
                if (dt != null && dt.Rows.Count > 0)
                {
                    IList<ClientInfoEntiy> clients = DataTableUtility.ToList<ClientInfoEntiy>(dt);
                    res = JsonConvert.SerializeObject(clients);
                }
            }


            context.Response.Write(res);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}