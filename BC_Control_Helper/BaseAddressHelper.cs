using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models.LogVo;

namespace BC_Control_Helper
{
    public class BaseAddressHelper
    {
        public BaseAddressHelper() { }
        public static string AddressToString(object o)
        {
            try
            {
                return (string)o;
            }
            catch (Exception ee)
            {
                return "0";
                
            }
        }
        public static string AddressToString(BaseAddress o)
        {
            try
            {
                return o.Value;
            }
            catch (Exception ee)
            {
                return "0";

            }
        }
    }
}
