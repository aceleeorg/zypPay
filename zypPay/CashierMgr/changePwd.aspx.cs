using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HYModel;
using System.Security.Cryptography;
using System.Text;

namespace CashierMgr
{
    public partial class changePwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCommit_Click(object sender, EventArgs e)
        {

            if (txtPwd1.Text.Trim() != txtPwd2.Text.Trim())
            {
                Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", "alert('密码不匹配，请重新输入');", true);

                return;
            }
            if (Session["LoginUser"] != null)
            {

                OprInfoEntiy usr = (OprInfoEntiy)Session["LoginUser"];
                if (usr != null)
                {

                    
                    if (MD5Encrypt ( txtOldPwd.Text.Trim()) !=usr.OprPWD )
                    {
                        Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", "alert('原密码不正确，请重新输入');", true);

                        return;
                    }

                    HYCashierDAL.CahierMgrDAL dal = new HYCashierDAL.CahierMgrDAL();
                    if (dal.UpdateOprPwd(usr.OprID, MD5Encrypt(txtPwd1.Text.Trim())))
                    {
                        Session["LoginUser"] = null;
                        Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", "alert('修改成功');parent.closeDialog();", true);

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", "alert('修改失败');", true);

                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "customJs", "alert('系统错误，请重试');", true);
            }
        }

        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="str">要加密的string</param>
        /// <returns>密文</returns>
        string MD5Encrypt(string str)
        {
            string pwd = null;
            MD5 m = MD5.Create();
            byte[] s = m.ComputeHash(Encoding.Unicode.GetBytes(str));
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }
    }
}