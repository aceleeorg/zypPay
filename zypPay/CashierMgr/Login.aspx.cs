using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HYCashierDAL;
using HYModel;
using System.Security.Cryptography;
using System.Text;
namespace CashierMgr
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["LoginUser"] = null;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            
            string oprId = txtOprID.Text.Trim();
            string pwd = txtPwd.Text.Trim();
            string clientID = txtClientID.Text.Trim();
            if (string.IsNullOrEmpty(oprId) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(clientID ))
            {
                Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", "alert('请输入操作员编号、商户号、密码');", true);
                return;
            }

            HYCashierDAL.CahierMgrDAL dal = new CahierMgrDAL();
            OprInfoEntiy opr = dal.CheckLoginUsr(oprId, pwd, clientID);
            if (opr != null)
            {
                Session["LoginUser"] = opr;
                Session["ClientID"] = clientID ;
                if (clientID == "180000")
                {
                    Session["AgentLevel"] = 0;
                }
                Response.Redirect("Default.aspx");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", "alert('登录信息不正确');", true);
            }
        }

        
    }
}