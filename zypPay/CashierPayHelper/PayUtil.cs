using System;
using System.Collections.Generic;
using System.Text;
using System.Dynamic;
namespace CashierPayHelper
{
    /// <summary>
    /// 
    /// </summary>
    public  class PayUtil
    {
        public static string GetDynamicValue(dynamic curDy,string name)
        {
            string result = "";
            try
            {
                result = curDy.name.Value;
            }
            catch
            {

            }
            return result;

        }
    }
}
