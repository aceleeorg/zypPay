using System;
using System.Collections.Generic;
using System.Text;

namespace HYModel
{
    [Serializable]
    public class ShopInfoEntity
    {

        public string ClientID { get; set; }
        public string ShopID { get; set; }
        public string ShopName { get; set; }
      
        
        public string Tel { get; set; }
        public string Address { get; set; }
        public int TNum { get; set; }
       // public string Note { get; set; }
       
        public int Status { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
       
        public string CreateUsr { get; set; }
        public string UpdateUsr { get; set; }
    }
}
