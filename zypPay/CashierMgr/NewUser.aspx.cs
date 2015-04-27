using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HYModel;

namespace CashierMgr
{
    public partial class NewUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                //DataTable dt = dal.GetAllClient();
                //ddlClient.DataSource = dt;

                //ddlClient.DataTextField = "ClientName";
                //ddlClient.DataValueField = "ClientID";
                //ddlClient.DataBind();
                string clientID = string.Empty;
                if (Session["ClientID"].ToString() == "180000")
                {
                    clientID = null;
                }
                else
                {
                    clientID = Session["ClientID"].ToString();
                    //ddlClient.SelectedValue = clientID;
                    //ddlClient.Enabled = false;
                }
            }
        }

        protected void btnCommit_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
           // string clientID = ddlClient.SelectedValue;
            string clientID = txtClient.Value;
            string js = "";
            OprInfoEntiy entity = new OprInfoEntiy();
            entity.OprName = name;
            entity.ClientID = clientID;
            entity.IsAdmin = int.Parse(ddlIsAdmin.SelectedValue);
            entity.OprPWD = HYCashierDAL.CryptographyHelper.MD5Encrypt("123456");
             HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
             string id = dal.GetOprID();
             entity.OprID = id;
             if (dal.AddOpr(entity))
             {
                 js = "alert('新增成功'); window.parent.closeDialog();";
             }
             else
             {
                 js = "alert('新增失败');";
             }
             Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", js, true);
             //ScriptManager.RegisterStartupScript(this, GetType(), "customJs", js, true);
        }
    }
}