using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HYModel;

namespace CashierMgr
{
    public partial class EditMerchant : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Params["id"] != null)
                {
                    string id=Request.Params["id"];
                    HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                    IList <ClientInfoEntiy > clients= dal.GetClientInfo(id, null);
                    if (clients != null)
                    {
                        ViewState["Client"] = clients[0];
                        txtAddress.Text = clients[0].Address;
                        txtContact.Text = clients[0].Contact;
                        txtName.Text = clients[0].ClientName;
                        txtPID.Text = clients[0].PID;
                        txtKey.Text = clients[0].PKey;
                        txtAlipayAccount.Text = clients[0].AlipayAccount;
                    }
                }
            }
        }

        protected void btnCommit_Click(object sender, EventArgs e)
        {
            if (ViewState["Client"] != null)
            {
                ClientInfoEntiy entity = (ClientInfoEntiy)ViewState["Client"];
                entity.Contact = txtContact.Text.Trim();
                entity.Address = txtAddress.Text.Trim();
                entity.PID = txtPID.Text.Trim();
                entity.PKey = txtKey.Text.Trim();
                entity.AlipayAccount = txtAlipayAccount.Text.Trim();
                HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                if (dal.UpdateClient(entity))
                {
                    Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", "alert('修改成功'); window.parent.closeDialog();", true);
                   //ScriptManager.RegisterStartupScript(this, typeof(Page), "customJs", "alert('修改成功'); window.parent.closeDialog();", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", "alert('修改失败，请重试');", true);
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "customJs", "修改失败，请重试", true);
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