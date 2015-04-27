using System;
using System.Collections.Generic;
using System.Text;

namespace HYModel
{
    [Serializable ]
   public class OprInfoEntiy
    {
        public string OprID { get; set; }

        public string OprName { get; set; }

        public string ClientID { get; set; }

        public string OprPWD { get; set; }
        public int Status { get; set; }
        public int IsAdmin { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
