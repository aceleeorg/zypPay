using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CashierMgr
{
    public partial class commonInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                string clientID = string.Empty;
                if (Session["ClientID"].ToString() == "180000")
                {
                    clientID = null;
                }
                else
                {
                    clientID = Session["ClientID"].ToString();
                   
                }
                int oprCount = dal.GetOprCount(clientID , "", "", "");
                int clientCount = dal.GetClientCount(clientID , "");
                lblClientCount.Text = clientCount.ToString();
                
                lblOprCount.Text = oprCount.ToString();
            }
        }
    }
}