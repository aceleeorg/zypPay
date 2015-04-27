using System;
using System.Collections.Generic;
using System.Text;

namespace HYModel
{
     [Serializable]
   public class AgentInfoEntity
    {
        public string AgentID { get; set; }

        public string AgentName { get; set; }
        public string UpAgentID { get; set; }
     

        public string Tel { get; set; }
      
        public string Contact { get; set; }
        public string Address { get; set; }
      
        public string Note { get; set; }
        public int AgentType { get; set; }
        public int Status { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
       
        public string CreateUsr { get; set; }
        public string UpdateUsr { get; set; }
    }
}
