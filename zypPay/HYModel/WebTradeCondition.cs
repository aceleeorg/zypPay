using System;
using System.Collections.Generic;
using System.Text;

namespace HYModel
{
    public class WebTradeCondition
    {
        /// <summary>
        /// 
        /// </summary>
        public string OprID { get; set; }

        public string ClientID { get; set; }

        public string ClientName { get; set; }

        public string LocalFlowNo { get; set; }

        public string ServerFlowNo { get; set; }


        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }


    }
}
