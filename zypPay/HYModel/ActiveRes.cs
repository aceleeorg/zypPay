using System;
using System.Collections.Generic;
using System.Text;

namespace HYModel
{
    [Serializable ]
   public class ActiveRes
    {
       public string ClientID { get; set; }

       public string TerminalID { get; set; }

       public string ClientName { get; set; }

       public string PID { get; set; }

       public string PKEY { get; set; }

       public string AlipayAccount { get; set; }

       public bool  Success { get; set; }
    }
}
