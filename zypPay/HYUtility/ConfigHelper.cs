using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace HYUtility
{
   public class ConfigHelper
    {
        #region  更新配置文件
        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="ConnenctionString"></param>
        /// <param name="strKey"></param>
        public static void UpdateConfig(string strValue, string strKey)
        {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径
            string strFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            doc.Load(strFileName);
            //找出名称为“add”的所有元素
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性
                XmlAttribute att = nodes[i].Attributes["key"];
                //根据元素的第一个属性来判断当前的元素是不是目标元素
                if (att.Value == strKey)
                {
                    //对目标元素中的第二个属性赋值
                    att = nodes[i].Attributes["value"];
                    att.Value = strValue;
                    break;
                }
            }
            //保存上面的修改
            doc.Save(strFileName);
        }
        #endregion
    }
}
