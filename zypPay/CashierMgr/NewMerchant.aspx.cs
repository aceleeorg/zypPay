using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HYModel;
using HYCashierDAL;

namespace CashierMgr
{
    public partial class NewMerchant : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
            
            }
        }

        protected void btnCommit_Click(object sender, EventArgs e)
        {
            AddNewClient();
        }

        void AddNewClient()
        {
            HYCashierDAL.CahierMgrDAL dal = new CahierMgrDAL();

            string name = txtName.Text.Trim();

            if (dal.IsClientExist(name))
            {

                ScriptManager.RegisterStartupScript(this, typeof(Page), "customJs", "alert('商户名已存在');", true);

                return;
            }

           string id= dal.GetClientID();
         
           string contact = txtContact.Text.Trim();
           string address = txtAddress.Text.Trim();
           string activeCode = HYCashierDAL.CryptographyHelper.MD5Encrypt(id + DateTime.Now.ToString("yyyyMMddHHmm"));
           ClientInfoEntiy entity = new ClientInfoEntiy();
           entity.Address = address;
           entity.PID = txtPID.Text.Trim();
           entity.PKey = txtKey.Text.Trim();
           entity.ClientID = id;
           entity.ClientName = name;
           entity.Contact = contact;
           entity.MaxNum = 10;
           entity.ActiveCode = activeCode;
           entity.AlipayAccount = txtAlipayAccount.Text.Trim();
           string js = "";
           if (dal.AddClient(entity))
           {
               js = "alert('新增成功'); window.parent.closeDialog();";
           }
           else
           {
               js = "alert('新增失败');";
           }
           Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs",js, true);
           //ScriptManager.RegisterStartupScript(this, typeof(Page), "customJs", js, true);

        }
    }
}