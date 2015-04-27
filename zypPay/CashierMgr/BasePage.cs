using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using HYModel;

namespace CashierMgr
{
    public class BasePage : System.Web.UI.Page
    {
        /// <summary>
        /// 初始化页面事件
        /// </summary>
        /// <remarks>
        /// 处理步骤：
        /// </remarks>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!DesignMode)
            {
                if (Session["LoginUser"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "customJs", "top.logout();", true);
                   // Response.End();
                }
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
           
        }

        public OprInfoEntiy LoginUser
        {
            get {
                if (Session["LoginUser"] == null) return null;
                else return Session["LoginUser"] as OprInfoEntiy;
            }
        }
    }
}