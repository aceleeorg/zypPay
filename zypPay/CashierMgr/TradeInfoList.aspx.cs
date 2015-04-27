using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HYModel;
namespace CashierMgr
{
    public partial class TradeInfoList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string clientID = string.Empty;
                if (Session["ClientID"].ToString() == "180000")
                {
                    clientID = null;
                }
                else
                {
                    clientID = Session["ClientID"].ToString();

                    txtMerchantID.Text = clientID;
                    txtMerchantID.Enabled = false;
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        void LoadData()
        {
            string clientName = txtName.Value  .Trim();

            string clientID = txtMerchantID.Text .Trim();

            string startDate = txtStartTime.Value.Trim();
            string oprID = txtOprID.Value.Trim();
            string endDate = txtEndTime.Value.Trim();
            WebTradeCondition condition=new WebTradeCondition ();
            condition .ClientID =clientID ;
            if(!string .IsNullOrEmpty (startDate ))  condition .StartDate =DateTime .Parse (startDate );
            if(!string.IsNullOrEmpty (endDate )) condition .EndDate =DateTime.Parse (endDate ).AddDays (1);
            HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
            condition.OprID = oprID;
            int? count = 0;
            count = dal.GetTradeInfoCount(condition);
            if (count == null) AspNetPager1.RecordCount = 0;
            else AspNetPager1.RecordCount = (int)count;

            DataTable dt = dal.GetTradeInfoPager (condition , AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex);
            dataGrid.DataSource = dt;
            dataGrid.DataBind();

        }
    }
}