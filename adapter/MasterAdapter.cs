using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.adapter
{
    public class MasterAdapter
    {
       
        public UserBean AppUserBean { get; set; }
        public string InToStr(Object obj)
        {
            if(obj !=null)
                return obj.ToString();

            return "";
        }

        public string CurrentDateSQL()
        {
            return DateTime.Now.ToString("yyyy/MM/dd");
        }
    }
}
