using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HYModel;
using System.Data;

namespace CashierMgr
{
    public partial class EditUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                DataTable dt = dal.GetAllClient();
                ddlClient.DataSource = dt;
                ddlClient.DataTextField = "ClientName";
                ddlClient.DataValueField = "ClientID";
                ddlClient.DataBind();
                if (Request.Params["id"] != null)
                {
                    string id = Request.Params["id"];
                  //  HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                    IList<OprInfoEntiy> oprs = dal.GetOprInfo (id, null);
                    if (oprs != null)
                    {
                        ViewState["Opr"] = oprs[0];
                        ddlIsAdmin.SelectedValue = oprs[0].IsAdmin.ToString ();
                        txtName.Text = oprs[0].OprName;
                        ddlClient.SelectedValue = oprs[0].ClientID;
                    }
                }
            }
        }


         protected void btnCommit_Click(object sender, EventArgs e)
        {
            if (ViewState["Opr"] != null)
            {
                OprInfoEntiy entity = (OprInfoEntiy)ViewState["Opr"];
                entity .ClientID =ddlClient.SelectedValue ;
                entity.IsAdmin = int.Parse(ddlIsAdmin.SelectedValue);
                HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                if (dal.UpdateOpr(entity))
                {
                    Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", "alert('修改成功'); window.parent.closeDialog();", true);
                    //ScriptManager.RegisterStartupScript(this, GetType(), "customJs", "alert('修改成功'); window.parent.closeDialog();", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", "alert('修改失败，请重试');", true);
                    //ScriptManager.RegisterStartupScript(this, GetType(), "customJs", "修改失败，请重试", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", "alert('系统错误，请重试');", true);
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "customJs", "系统错误，请重试", true);
            }
        }
 
    }
}