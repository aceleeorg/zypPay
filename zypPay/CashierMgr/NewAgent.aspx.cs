using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HYModel;

namespace CashierMgr
{
    public partial class NewAgent : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAgentType();
            }
        }

        void LoadAgentType()
        {
            if (Session ["AgentLevel"] != null)
            {
                string type = Session["AgentLevel"].ToString();
                if (type == "0")
                { 
                //顶级资源派
                    ddlAgentType.Items.Add(new ListItem("--请选择--", ""));
                    ddlAgentType.Items.Add(new ListItem("A", "1"));
                    ddlAgentType.Items.Add(new ListItem("B", "2"));
                    ddlAgentType.Items.Add(new ListItem("C", "3"));
                }
                else if (type == "1")
                {
                    //1级代理
                    ddlAgentType.Items.Add(new ListItem("--请选择--", ""));
                  
                    ddlAgentType.Items.Add(new ListItem("B", "2"));
                    ddlAgentType.Items.Add(new ListItem("C", "3"));
                }
                else if (type == "2")
                {
                    //2级代理
                   // ddlAgentType.Items.Add(new ListItem("--请选择--", ""));
                   
                    ddlAgentType.Items.Add(new ListItem("C", "3"));
                }
            }
        }

        protected void btnCommit_Click(object sender, EventArgs e)
        {
            string agentName = txtName.Text.Trim();
            string agentContact = txtContact.Text.Trim();
            string address = txtAddress.Text.Trim();
            string tel = txtTel.Text.Trim();

            string strType = ddlAgentType.SelectedValue;
             HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
            AgentInfoEntity agent = new AgentInfoEntity();
            agent.Address = address;
            agent.AgentID = dal.GetAgentID();
            agent.AgentName = agentName;
            agent.AgentType = int.Parse(strType);
            agent.Contact = agentContact;
            agent.Tel = tel;
            agent.CreateUsr = LoginUser.ClientID;
            string js = "";
            if (dal.AddAgent(agent))
            {
                js = "alert('新增成功'); window.parent.closeDialog();";
            }
            else
            {
                js = "alert('新增失败');";
            }
            Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", js, true);
           // agent.UpAgentID
        }
    }
}