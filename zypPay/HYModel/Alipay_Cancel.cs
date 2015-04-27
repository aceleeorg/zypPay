using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace HYModel
{
     [XmlRoot(ElementName = "alipay")]
   public class Alipay_Cancel
    {
        //is_success
        [XmlElement(ElementName = "is_success")]
        public string is_success { get; set; }

        [XmlElement(ElementName = "sign")]
        public string sign { get; set; }


        [XmlElement(ElementName = "sign_type")]
        public string sign_type { get; set; }

        //[XmlElement(ElementName = "request")]
        //public Alipay_RefundRequest[] Requsets
        //{
        //    get;
        //    set;
        //}


        [XmlElement(ElementName = "response")]
        public Alipay_Response[] Response
        {
            get;
            set;
        }


        [XmlElement(ElementName = "error")]
        public string error { get; set; }

    }
}
