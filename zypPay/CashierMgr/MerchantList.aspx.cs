using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CashierMgr
{
    public partial class MerchantList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack )
            {
                LoadData();
            }
            if (Session["ClientID"].ToString() != "180000")
            {
                btnAdd.Visible = false;
                btnNew.Visible = false;
            }

        }

        void LoadData()
        {
            string clientName = txtMerchantID.Value.Trim();

           // string contactName = txtContact.Value.Trim();

            HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();

            int? count = 0;
            string clientID = string.Empty;
            if (Session["ClientID"].ToString() == "180000")
            {
                clientID = null;
            }
            else
            {
                clientID = Session["ClientID"].ToString();
                btnAdd.Visible = false;
            }
            count = dal.GetClientCount(clientID, clientName);
            if (count == null) AspNetPager1.RecordCount = 0;
            else AspNetPager1.RecordCount = (int)count;

            DataTable dt = dal.GetClientList(clientID, clientName, AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize);
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

        protected void dataGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            string name = e.CommandName.ToString();

            if (name == "cmdUpdate")
            {
                string js = "update('" + id + "');";
                ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "customJs", js, true);
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "customJs", js, false);
            }
            else
            { 
            //cmdDelete
                HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                if (dal.DeleteClient(id))
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