using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CashierMgr
{
    public partial class UserList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ClientID"].ToString() != "180000")
                {
                    txtMerchantID.Value = Session["ClientID"].ToString();
                    txtMerchantID.Disabled = true;
                }
               
            }
        }

        void LoadData()
        { 
            string clientName=txtMerchantName.Value .Trim ();

            string oprName = txtUsrName.Value.Trim();

            HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();

            int? count = 0;
            string clientID = string.Empty;
            if (!string.IsNullOrEmpty(txtMerchantID.Value.Trim()))
            {
                clientID = txtMerchantID.Value;
            }
            //if (Session["ClientID"].ToString() == "180000")
            //{
            //    clientID = null;
            //}
            //else
            //{
            //    clientID = Session["ClientID"].ToString();
            //}
            count = dal.GetOprCount(clientID, clientName, null, oprName);
            if (count == null) AspNetPager1.RecordCount = 0;
            else AspNetPager1.RecordCount = (int)count;

            DataTable dt = dal.GetOprList(clientID, clientName, null, oprName, AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize);
            dataGrid.DataSource = dt;
            dataGrid.DataBind();

        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnUpdate_Command(object sender, CommandEventArgs e) 
        {
            string id = e.CommandArgument.ToString();
            string name = e.CommandName.ToString();
            string js = "update('" + id + "');";
            // litJs.Text = " <script language='javascript'>" + js + "</script>";
            ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "customJs", js, true);
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "customJs", js, false);
        }

        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            string name = e.CommandName.ToString();
            HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
            if (dal.DeleteOpr(id))
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "customJs", "alert('删除成功');", true);
                LoadData();
            }
            else
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "customJs", "alert('删除失败，请重试')", true);
            }
        } 

        protected void dataGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            string name = e.CommandName.ToString();

            if (name == "cmdUpdate")
            {
                string js = "update('" + id + "');";
               // litJs.Text = " <script language='javascript'>" + js + "</script>";
                ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "customJs", js, true);
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "customJs", js, false);
            }
            else
            {
                //cmdDelete
                HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                if (dal.DeleteOpr(id))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "customJs", "alert('删除成功');", true);
                    LoadData();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "customJs", "alert('删除失败，请重试')", true);
                }

            }
        }
    }
}