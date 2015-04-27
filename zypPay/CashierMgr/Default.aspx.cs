using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CashierMgr
{
    public partial class _Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (LoginUser != null) usrName.InnerText = LoginUser.OprName;
                else Response.Redirect("Login.aspx");
            }
        }
    }
}