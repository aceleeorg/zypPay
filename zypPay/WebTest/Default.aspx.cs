using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebTest
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ChannelPayService.ChannelPayServiceSoapClient client = new ChannelPayService.ChannelPayServiceSoapClient();
           string str= client.Pay("001", "180066", "4DA8A9DE2E4452BF591788D6ECA7A371", "1", "1.20", "8888", "mc_sp", "15", "0105");
        }
    }
}
