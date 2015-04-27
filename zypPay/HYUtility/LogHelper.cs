using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HYUtility
{
   public class LogHelper
    {
      
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");

        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static void SetConfig()
        {
            log4net.Config.DOMConfigurator.Configure();
        }

        public static void SetConfig(FileInfo configFile)
        {
            
            log4net.Config.DOMConfigurator.Configure(configFile); 
        }

        public static void WriteLog(string info)
        {
            if(loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        public static void WriteLog(string info,Exception se)
        {
            if(logerror.IsErrorEnabled)
            {
                logerror.Error(info,se);
            }
        }
    }
}
