using System;
using System.Collections.Generic;
using System.Text;

namespace HYModel
{
    [Serializable ]
   public class ClientInfoEntiy
    {
       public string ClientID { get; set; }

       public string ClientName { get; set; }
       public string PID { get; set; }
       public string PKey { get; set; }

       public string Phone { get; set; }
       public string AlipayAccount { get; set; }

       public string Contact { get; set; }
       public string Address { get; set; }
       public int MaxNum { get; set; }
       public string Note { get; set; }
       public string ActiveCode { get; set; }
       public int Status { get; set; }
       public DateTime ? CreateTime { get; set; }
       public DateTime? UpdateTime { get; set; }
       public string AgentID { get; set; }
       public string CreateUsr { get; set; }
       public string UpdateUsr { get; set; }
    }
}
